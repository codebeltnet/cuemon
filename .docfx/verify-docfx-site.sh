#!/bin/sh
set -eu

# Verifies Cuemon DocFX run-action assets inside a build context or rendered site directory.
# Uses only POSIX shell + standard BusyBox/Alpine utilities so it runs inside both
# codebeltnet/docfx and nginx runtime containers without extra dependencies.

usage() {
    cat <<'EOF' >&2
Usage:
  verify-docfx-site.sh --context [docfx-root]
  verify-docfx-site.sh --site <site-root> [--source <docfx-root>] [--manifest <manifest-path>]
EOF
    exit 2
}

mode=""
target=""
source_root=""
manifest_path=""

while [ "$#" -gt 0 ]; do
    case "$1" in
        --context)
            [ -n "$mode" ] && usage
            mode="context"
            if [ "$#" -gt 1 ] && [ "${2#-}" = "$2" ]; then
                target="$2"
                shift 2
            else
                target="."
                shift 1
            fi
            ;;
        --site)
            [ -n "$mode" ] && usage
            [ "$#" -lt 2 ] && usage
            mode="site"
            target="$2"
            shift 2
            ;;
        --source)
            [ "$#" -lt 2 ] && usage
            source_root="$2"
            shift 2
            ;;
        --manifest)
            [ "$#" -lt 2 ] && usage
            manifest_path="$2"
            shift 2
            ;;
        *)
            usage
            ;;
    esac
done

[ -n "$mode" ] || usage

fail() {
    printf 'ERROR: %s\n' "$1" >&2
    exit 1
}

check_manifest_ids() {
    mf="$1"
    scripts_dir="$2"
    [ -s "$mf" ] || fail "Missing or empty run-actions manifest: $mf"

    ids_file=$(mktemp)
    trap 'rm -f "$ids_file"' EXIT

    grep -oE '"id"[[:space:]]*:[[:space:]]*"[a-f0-9]{24}"' "$mf" \
        | sed -E 's/.*"([a-f0-9]{24})".*/\1/' \
        | sort -u > "$ids_file"

    [ -s "$ids_file" ] || fail "No valid run-action IDs found in manifest: $mf"

    missing_ids=0
    while IFS= read -r id; do
        [ -n "$id" ] || continue
        if [ ! -s "$scripts_dir/$id/native.ps1" ] && [ ! -s "$scripts_dir/$id/docker.ps1" ]; then
            printf 'ERROR: Missing generated run-action script(s) for manifest id %s in %s\n' "$id" "$scripts_dir" >&2
            missing_ids=$((missing_ids + 1))
        fi
    done < "$ids_file"

    rm -f "$ids_file"
    trap - EXIT
    [ "$missing_ids" -eq 0 ] || fail "$missing_ids manifest example(s) are missing generated scripts in $scripts_dir."
}

if [ "$mode" = "context" ]; then
    docfx_root=$(cd "$target" && pwd)

    [ -s "$docfx_root/run-actions.manifest.json" ] || fail "Missing or empty run-actions.manifest.json in $docfx_root."
    [ -s "$docfx_root/templates/cuemon/public/run-actions.js" ] || fail "Missing or empty templates/cuemon/public/run-actions.js in $docfx_root."

    if [ ! -d "$docfx_root/run-actions" ] || [ -z "$(find "$docfx_root/run-actions" -type f -name '*.ps1' 2>/dev/null | head -n 1)" ]; then
        cat >&2 <<EOF
ERROR: Missing generated DocFX run-action scripts in Docker build context ($docfx_root/run-actions/).
Generate and validate them before invoking Dockerfile.docfx directly:
  pwsh .docfx/BuildRunActions.ps1
  pwsh .docfx/TestRunActions.ps1
Or use the scripted build entry point:
  pwsh .docfx/BuildDocfxImage.ps1
EOF
        exit 1
    fi

    refs_file=$(mktemp)
    trap 'rm -f "$refs_file"' EXIT

    find "$docfx_root" \
        \( -path "$docfx_root/obj" -o -path "$docfx_root/wwwroot" -o -path "$docfx_root/packages" -o -path "$docfx_root/run-actions" \) -prune -o \
        -type f -name '*.md' -print \
        | xargs -r grep -ohE '~/run-actions/[a-f0-9]{24}/(native|docker)\.ps1' 2>/dev/null \
        | sed 's|^~/||' \
        | sort -u > "$refs_file" || true

    [ -s "$refs_file" ] || fail "No run-actions script references found in Markdown under $docfx_root."

    missing_refs=0
    ref_count=0
    while IFS= read -r rel_path; do
        [ -n "$rel_path" ] || continue
        ref_count=$((ref_count + 1))
        if [ ! -s "$docfx_root/$rel_path" ]; then
            printf 'ERROR: Missing or empty referenced run-action script in build context: %s\n' "$rel_path" >&2
            missing_refs=$((missing_refs + 1))
        fi
    done < "$refs_file"

    rm -f "$refs_file"
    trap - EXIT
    [ "$missing_refs" -eq 0 ] || fail "$missing_refs referenced run-action script(s) are missing from Docker build context."

    check_manifest_ids "$docfx_root/run-actions.manifest.json" "$docfx_root/run-actions"

    printf 'Verified Docker build context: %s referenced run-action script(s) present in %s/run-actions.\n' "$ref_count" "$docfx_root"
    exit 0
fi

# --site mode
[ -d "$target" ] || fail "Published DocFX site directory not found: $target"
site_root=$(cd "$target" && pwd)

[ -s "$site_root/public/run-actions.js" ] || fail "Missing or empty published template script: $site_root/public/run-actions.js"
[ -d "$site_root/run-actions" ] || fail "Missing published run-actions directory: $site_root/run-actions"

site_ps1_count=$(find "$site_root/run-actions" -type f -name '*.ps1' | wc -l | tr -d ' ')
[ "$site_ps1_count" -gt 0 ] || fail "Published run-actions directory contains no .ps1 scripts: $site_root/run-actions"

if [ -n "$source_root" ]; then
    source_abs=$(cd "$source_root" && pwd)
    [ -d "$source_abs/run-actions" ] || fail "Source run-actions directory not found: $source_abs/run-actions"

    src_list=$(mktemp)
    site_list=$(mktemp)
    trap 'rm -f "$src_list" "$site_list"' EXIT

    (cd "$source_abs/run-actions" && find . -type f -name '*.ps1' | sed 's|^\./||' | sort) > "$src_list"
    (cd "$site_root/run-actions" && find . -type f -name '*.ps1' | sed 's|^\./||' | sort) > "$site_list"

    missing_from_site=$(comm -23 "$src_list" "$site_list")
    if [ -n "$missing_from_site" ]; then
        printf 'ERROR: Generated run-actions missing from published site %s:\n%s\n' "$site_root" "$missing_from_site" >&2
        exit 1
    fi

    rm -f "$src_list" "$site_list"
    trap - EXIT
fi

if [ -n "$manifest_path" ]; then
    check_manifest_ids "$manifest_path" "$site_root/run-actions"
fi

html_list=$(mktemp)
trap 'rm -f "$html_list"' EXIT
find "$site_root" -type f -name '*.html' | sort > "$html_list"
[ -s "$html_list" ] || fail "No HTML files found in published site: $site_root"

check_file=$(mktemp)
trap 'rm -f "$html_list" "$check_file"' EXIT

framework_views=0
[ ! -s "$site_root/framework-views.json" ] || framework_views=1

awk -v site_root="$site_root" -v framework_views="$framework_views" '
function normalize_rel(base_dir, href,    full, n, i, seg, out_n, res, stack) {
    if (href ~ /^\//) {
        full = substr(href, 2)
    } else if (base_dir == "" || base_dir == ".") {
        full = href
    } else {
        full = base_dir "/" href
    }
    n = split(full, seg, "/")
    out_n = 0
    for (i = 1; i <= n; i++) {
        if (seg[i] == "" || seg[i] == ".") continue
        if (seg[i] == "..") {
            if (out_n == 0) return "__ESCAPES_ROOT__"
            out_n--
        } else {
            stack[++out_n] = seg[i]
        }
    }
    res = ""
    for (i = 1; i <= out_n; i++) {
        res = (i == 1 ? stack[i] : res "/" stack[i])
    }
    return res
}
{
    file = $0
    rel_file = substr(file, length(site_root) + 2)
    dir = rel_file
    # NGINX serves alternate API pages at the public path, without the physical view prefix.
    if (framework_views) sub(/^_frameworks\/[^\/]+\//, "", dir)
    if (index(dir, "/") > 0) {
        sub(/\/[^\/]+$/, "", dir)
    } else {
        dir = ""
    }
    while ((getline line < file) > 0) {
        pos = 1
        while (match(substr(line, pos), /href="[^"]*run-actions\/[^"]*"/)) {
            m = substr(line, pos + RSTART - 1, RLENGTH)
            href = substr(m, 7, length(m) - 7)
            links++
            resolved = normalize_rel(dir, href)
            if (resolved == "__ESCAPES_ROOT__" || resolved !~ /^run-actions\/[a-f0-9]{24}\/(native|docker)\.ps1$/) {
                printf("INVALID|%s|%s|%s\n", rel_file, href, resolved)
                bad++
            } else if (!(resolved in seen)) {
                seen[resolved] = rel_file
                print "CHECK|" resolved "|" rel_file
            }
            pos += RSTART + RLENGTH - 1
        }
    }
    close(file)
}
END {
    printf("SUMMARY|%d|%d\n", links + 0, bad + 0)
}
' "$html_list" > "$check_file"

total_links=0
invalid_links=0

while IFS='|' read -r kind col1 col2 col3; do
    case "$kind" in
        INVALID)
            printf 'ERROR: Invalid run-action href "%s" in %s (resolved: "%s")\n' "$col2" "$col1" "$col3" >&2
            invalid_links=$((invalid_links + 1))
            ;;
        CHECK)
            if [ ! -s "$site_root/$col1" ]; then
                printf 'ERROR: Missing or empty published script "%s" referenced by %s\n' "$col1" "$col2" >&2
                invalid_links=$((invalid_links + 1))
            fi
            ;;
        SUMMARY)
            total_links="$col1"
            ;;
    esac
done < "$check_file"

rm -f "$html_list" "$check_file"
trap - EXIT

[ "$total_links" -gt 0 ] || fail "No run-actions links found in rendered HTML under $site_root"
[ "$invalid_links" -eq 0 ] || fail "$invalid_links invalid or missing run-action reference(s) found in $site_root."

printf 'Verified published DocFX site %s: %s HTML run-action link(s), %s script file(s) in run-actions/.\n' "$site_root" "$total_links" "$site_ps1_count"
