// Optional modern-template enhancement. The association remains usable without JavaScript.
const icons = {
  native: '<path d="M12.6 11.2l-2.3 1.4a3.4 3.4 0 0 0-2.9-1.4c-2.1 0-3.6 1.6-3.6 3.8s1.5 3.8 3.6 3.8c1.3 0 2.3-.6 2.9-1.5l2.3 1.4a6.1 6.1 0 0 1-5.2 2.7C3.7 21.4 1 18.7 1 15s2.7-6.4 6.4-6.4c2.2 0 4.1 1 5.2 2.6Z"/>' +
    '<path d="M15.2 9h1.9l-.7 3h2l.7-3H21l-.7 3H22l-.4 1.8h-1.7l-.5 2.2h1.7l-.4 1.8H19l-.7 3.2h-1.9l.7-3.2h-2l-.7 3.2h-1.9l.7-3.2h-1.7l.4-1.8h1.7l.5-2.2h-1.7l.4-1.8h1.7Zm.8 4.8-.5 2.2h2l.5-2.2Z"/>',
  docker: '<path d="M4 11h2.5v2.5H4zm3 0h2.5v2.5H7zm3 0h2.5v2.5H10zm3 0h2.5v2.5H13zm3 0h2.5v2.5H16zM7 8h2.5v2.5H7zm3 0h2.5v2.5H10zm3 0h2.5v2.5H13zm0-3h2.5v2.5H13z"/>' +
    '<path fill-rule="evenodd" d="M1.5 14h17.2c.5 0 1-.1 1.4-.3-.3-1.1.1-2.5 1-3.6 1.1.5 1.7 1.3 2 2.4a4.8 4.8 0 0 1 3 .4c-.7 1.5-2.1 2.1-3.6 2.1-1.9 4.4-5.5 6.5-10.9 6.5C5.7 21.5 2.2 19 1.5 14Zm5.7 2.4a.8.8 0 1 0 0 1.6.8.8 0 0 0 0-1.6Z"/>',
};
const waitingForCopy = new WeakSet();
// Generated scripts are one complete `& { ... }` paste unit, so execution and verification run together.
const executionScript = /^& \{\r?\n#requires -Version 7\.0\r?\n[\s\S]*\r?\n\}\r?\n?$/;

async function fetchScript(link) {
  const url = new URL(link.href);
  if (url.origin !== location.origin) throw new Error('Script must be hosted on this site.');
  // Asset URLs are stable per example, so always revalidate rather than copy a stale cached script.
  const response = await fetch(url, { mode: 'same-origin', cache: 'no-cache' });
  if (!response.ok) throw new Error('Script unavailable.');
  const script = await response.text();
  if (!executionScript.test(script)) throw new Error('Response is not an execution script.');
  return script;
}

function previewScript(script, label) {
  const dialog = document.createElement('dialog');
  dialog.className = 'docfx-run-preview';
  dialog.setAttribute('aria-label', label + ' run script');
  const heading = document.createElement('h2');
  heading.textContent = label + ' run script';
  const source = document.createElement('pre');
  source.tabIndex = 0;
  source.textContent = script;
  const close = document.createElement('button');
  close.type = 'button';
  close.className = 'btn btn-secondary';
  close.textContent = 'Close';
  close.addEventListener('click', () => dialog.close());
  dialog.addEventListener('close', () => dialog.remove(), { once: true });
  dialog.append(heading, source, close);
  document.body.append(dialog);
  dialog.showModal();
}

function createAction(link, report, onCopyFailure) {
  const transport = link.dataset.transport;
  const label = transport === 'native' ? 'Native' : 'Docker';
  const name = 'Copy ' + label + ' run script';
  const button = document.createElement('button');
  button.type = 'button';
  button.className = 'btn border-0 docfx-run-copy';
  button.title = name + '\nShift-click or right-click to preview';
  button.setAttribute('aria-label', name);
  button.setAttribute('aria-description', 'Press Shift+Enter to preview the PowerShell script.');
  button.innerHTML = '<svg class="docfx-run-icon-' + transport + '" width="25" height="20" viewBox="0 0 28 22" fill="currentColor" aria-hidden="true">' +
    icons[transport] + '<path class="docfx-run-play" d="M21.5 1.5 27 4.8l-5.5 3.3Z" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linejoin="round"/></svg>';
  const icon = button.firstElementChild;
  const check = document.createElement('i');
  check.className = 'bi bi-check-lg';
  check.setAttribute('aria-hidden', 'true');

  let pending = false;
  let resetIcon;
  async function act(preview) {
    if (pending) return;
    pending = true;
    report('');
    try {
      const script = fetchScript(link);
      if (preview) {
        previewScript(await script, label);
      } else {
        // Start the clipboard write during the user's gesture, including while the fetch is pending.
        const clipboard = navigator.clipboard;
        if (clipboard?.write && typeof ClipboardItem === 'function') {
          const text = script.then(value => new Blob([value], { type: 'text/plain' }));
          await Promise.all([text, clipboard.write([new ClipboardItem({ 'text/plain': text })])]);
        } else {
          const text = await script;
          await clipboard.writeText(text);
        }
        clearTimeout(resetIcon);
        // Measure DocFX's success markup in the same code block so template overrides apply.
        const reference = document.createElement('a');
        reference.className = 'btn border-0 link-success code-action';
        reference.hidden = true;
        reference.setAttribute('aria-hidden', 'true');
        reference.style.transition = 'none';
        const referenceIcon = check.cloneNode(false);
        referenceIcon.removeAttribute('style');
        reference.append(referenceIcon);
        button.closest('pre').prepend(reference);
        const appearance = getComputedStyle(referenceIcon, '::before');
        check.style.color = appearance.color;
        check.style.fontSize = appearance.fontSize;
        reference.remove();
        button.replaceChildren(check);
        resetIcon = setTimeout(() => button.replaceChildren(icon), 1000);
        report(label + ' script copied.', true);
      }
    } catch {
      if (!preview) onCopyFailure?.();
      report('Could not ' + (preview ? 'preview ' : 'copy ') + label + ' script.');
    } finally {
      pending = false;
    }
  }

  button.addEventListener('click', event => { void act(event.shiftKey); });
  button.addEventListener('contextmenu', event => {
    event.preventDefault();
    void act(true);
  });
  button.addEventListener('keydown', event => {
    if (event.shiftKey && event.key === 'Enter') {
      event.preventDefault();
      void act(true);
    }
  });
  return button;
}

export function enhanceRunActions(root = document) {
  for (const links of root.querySelectorAll('.docfx-run-actions:not([data-enhanced])')) {
    const previous = links.previousElementSibling;
    const pre = previous?.matches('pre') ? previous : previous?.querySelector('pre');
    if (!pre?.querySelector(':scope > code:is(.lang-csharp, .language-csharp, .lang-cs, .language-cs)')) continue;
    const assets = [...links.querySelectorAll('a[data-transport="native"], a[data-transport="docker"]')];
    if (!assets.length) continue;

    // DocFX imports the custom module before rendering its Lit-managed copy anchor.
    if (!pre.querySelector(':scope > a.code-action')) {
      if (waitingForCopy.has(pre)) continue;
      waitingForCopy.add(pre);
      const observer = new MutationObserver(() => {
        if (!pre.querySelector(':scope > a.code-action')) return;
        observer.disconnect();
        waitingForCopy.delete(pre);
        attachToolbar(pre, links, assets);
      });
      observer.observe(pre, { childList: true });
      continue;
    }

    attachToolbar(pre, links, assets);
  }
}

function attachToolbar(pre, links, assets) {
  if ('enhanced' in links.dataset) return;
  const toolbar = document.createElement('span');
  toolbar.className = 'docfx-run-toolbar';
  toolbar.setAttribute('role', 'group');
  toolbar.setAttribute('aria-label', 'Run scripts');
  const status = document.createElement('span');
  status.setAttribute('role', 'status');
  status.className = 'docfx-run-status';
  let clearStatus;
  function report(message, visuallyHidden = false) {
    clearTimeout(clearStatus);
    status.classList.toggle('visually-hidden', visuallyHidden);
    status.textContent = message;
    if (message) clearStatus = setTimeout(() => { status.textContent = ''; }, 3000);
  }
  toolbar.append(...assets.map(link => createAction(link, report, () => { links.hidden = false; })));
  // Keep our nodes before DocFX's Lit render range, which replaces the original anchor on copy.
  pre.prepend(toolbar, status);
  pre.classList.add('docfx-runnable');
  links.dataset.enhanced = 'true';
  // Keep the recipe explanation visible when the toolbar replaces the fallback links.
  links.after(...links.querySelectorAll(':scope > p'));
  links.hidden = true;
}

if (typeof document !== 'undefined') {
  const style = document.createElement('style');
  style.textContent = `
pre.docfx-runnable {
  --run-action-size: 2.25rem;
}
.docfx-run-icon-native {
  color: #926bbb;
}
.docfx-run-icon-docker {
  color: #4d79d7;
}
.docfx-run-play {
  color: #628b2f;
}
pre.docfx-runnable > .code-action,
.docfx-run-toolbar {
  display: flex;
  position: absolute;
  top: .25rem;
  opacity: 0;
  pointer-events: none;
  z-index: 1;
}
pre.docfx-runnable > .code-action,
.docfx-run-copy {
  align-items: center;
  justify-content: center;
  inline-size: var(--run-action-size);
  block-size: var(--run-action-size);
  padding: 0;
  background-color: var(--bs-body-bg);
}
.docfx-run-toolbar {
  right: calc(.35rem + var(--run-action-size));
  gap: .15rem;
}
.docfx-run-copy {
  display: inline-flex;
}
.docfx-run-copy .bi-check-lg {
  pointer-events: none;
}
.docfx-run-copy:hover {
  background-color: var(--bs-tertiary-bg);
}
pre.docfx-runnable:hover > :is(.code-action, .docfx-run-toolbar),
pre.docfx-runnable:focus-within > :is(.code-action, .docfx-run-toolbar) {
  opacity: 1;
  pointer-events: auto;
}
pre.docfx-runnable > .code-action:focus-visible,
.docfx-run-copy:focus-visible {
  outline: 2px solid currentColor;
  outline-offset: -2px;
}
.docfx-run-status {
  position: absolute;
  z-index: 1;
  top: .25rem;
  left: .25rem;
  max-inline-size: calc(100% - 3 * var(--run-action-size) - 1rem);
  white-space: normal;
  pointer-events: none;
}
.docfx-run-status:not(:empty) {
  padding: .3rem .5rem;
  border-radius: .25rem;
  background: var(--bs-body-bg);
  color: var(--bs-body-color);
}
.docfx-run-preview {
  inline-size: min(52rem, 94vw);
  padding: 1rem;
  color: var(--bs-body-color);
  background: var(--bs-body-bg);
  border: 1px solid var(--bs-border-color);
  border-radius: .5rem;
}
.docfx-run-preview::backdrop {
  background: rgb(0 0 0 / 45%);
}
.docfx-run-preview pre {
  max-block-size: 65vh;
  padding: .75rem;
  background: var(--bs-tertiary-bg);
}
@media (hover: none), (any-pointer: coarse) {
  pre.docfx-runnable > :is(.code-action, .docfx-run-toolbar) {
    opacity: 1;
    pointer-events: auto;
  }
}
`;
  document.head.append(style);
  if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', () => enhanceRunActions(), { once: true });
  else enhanceRunActions();
}
