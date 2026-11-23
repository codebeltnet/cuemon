(() => {
    const meta = name => document.querySelector(`meta[name="docfx:${name}"]`);
    const current = new URL(location.href);
    const api = meta('framework-api').content;
    const view = current.searchParams.get('view');
    if (!view || !current.pathname.startsWith(api)) return;

    const toc = meta('tocrel');
    if (toc?.content) {
        const url = new URL(toc.content.replace(/\.html$/i, '.json'), current);
        if (url.origin === current.origin && url.pathname.startsWith(api)) {
            url.searchParams.set('view', view);
            toc.content = url.href;
        }
    }

    const searchRoot = `/_framework-search/${meta('framework-directory').content}/`;
    const search = meta('rel');
    if (search) search.content = searchRoot;

    function preserveView(anchor) {
        const href = anchor.getAttribute('href');
        if (!href || href.startsWith('#')) return;
        const url = new URL(href, current);
        if (url.origin !== current.origin) return;
        const fromSearch = url.pathname.startsWith(searchRoot);
        if (fromSearch) url.pathname = '/' + url.pathname.slice(searchRoot.length);
        const isApi = url.pathname.startsWith(api);
        if (isApi) url.searchParams.set('view', view);
        if (fromSearch || isApi) {
            const canonical = url.pathname + url.search + url.hash;
            if (canonical !== href) anchor.setAttribute('href', canonical);
        }
    }

    function updateLinks(node) {
        if (node instanceof HTMLAnchorElement) preserveView(node);
        node.querySelectorAll?.('a[href]').forEach(preserveView);
    }

    // DocFX renders the TOC, breadcrumbs, prev/next and search results after the HTML loads.
    // Observe href changes too because its renderer reuses links on search and TOC updates.
    new MutationObserver(records => {
        for (const record of records) {
            if (record.target instanceof HTMLAnchorElement) preserveView(record.target);
            for (const node of record.addedNodes) updateLinks(node);
        }
    }).observe(document.documentElement, {
        subtree: true, childList: true, attributes: true, attributeFilter: ['href']
    });
    document.addEventListener('DOMContentLoaded', () => updateLinks(document));
})();
