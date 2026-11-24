const assert = require('node:assert/strict');
const { chromium } = require('playwright');

async function main() {
    const origin = process.argv[2];
    const browser = await chromium.launch();
    try {
        const context = await browser.newContext();
        await context.route('**/*', route => {
            if (new URL(route.request().url()).origin !== origin) return route.abort();
            return route.continue();
        });
        const page = await context.newPage();
        const failures = [];
        context.on('response', response => {
            if (response.url().startsWith(origin) && response.status() >= 400) {
                failures.push(response.url() + ' ' + response.status());
            }
        });
        page.on('pageerror', error => failures.push(error.message));
        const manifest = await (await context.request.get(origin + '/framework-views.json')).json();
        const api = manifest.apiPath;
        const factory = api + 'Cuemon.IO.StreamFactory.html';
        const buffer = api + 'Cuemon.IO.BufferWriterOptions.html';
        const legacy = api + 'System.Runtime.CompilerServices.CallerArgumentExpressionAttribute.html';
        const alphanumeric = api + 'Cuemon.Alphanumeric.html';

        const defaultIndex = page.waitForResponse(origin + '/index.json');
        await page.goto(origin + factory);
        assert.equal((await defaultIndex).status(), 200);
        const defaultLink = page.locator('article a[href*="StreamFactory.Create.html"]').first();
        assert.equal(new URL(await defaultLink.getAttribute('href'), page.url()).searchParams.get('view'), null);

        const selector = page.getByRole('combobox', { name: 'Framework' });
        if (Object.keys(manifest.views).length === 1) await selector.waitFor({ state: 'hidden' });
        if (manifest.defaultView === 'net-11.0' && manifest.views['net-10.0']) {
            await selector.waitFor();
            assert.equal(await selector.inputValue(), manifest.defaultView);
            assert.equal(await selector.locator('option:checked').textContent(), '.NET 11');
            await page.goto(origin + factory + '?VIEW=net-10.0');
            await selector.waitFor();
            assert.equal(await selector.inputValue(), 'net-10.0');
            const anchor = '#' + await page.locator('article h1').getAttribute('id');
            await page.goto(origin + factory + '?q=StreamFactory' + anchor);
            await selector.selectOption('net-10.0');
            await page.waitForURL(origin + factory + '?q=StreamFactory&view=net-10.0' + anchor);
            await selector.waitFor();
            assert.equal(await selector.inputValue(), 'net-10.0');
            await page.goBack();
            await selector.waitFor();
            assert.equal(await selector.inputValue(), manifest.defaultView);
        }

        if (manifest.views['netstandard-2.1'] && manifest.views['netstandard-2.0'] && manifest.views['net-10.0']) {
            const fallbackStatus = page.locator('#framework-view-status');
            await page.goto(origin + alphanumeric + '?q=Alphanumeric&view=net-11.0');
            const apiRequests = [];
            const recordApiRequest = request => {
                if (request.isNavigationRequest() && new URL(request.url()).pathname === alphanumeric) apiRequests.push(request);
            };
            page.on('request', recordApiRequest);
            await selector.selectOption('netstandard-2.1');
            await page.waitForURL(origin + alphanumeric + '?q=Alphanumeric&view=net-10.0');
            await fallbackStatus.getByText('This API is not documented for .NET Standard 2.1. Showing .NET 10.').waitFor();
            assert.equal(await selector.inputValue(), 'net-10.0');
            assert.equal(apiRequests.length, 1);
            assert.equal(apiRequests[0].redirectedFrom(), null);
            await selector.selectOption('netstandard-2.1');
            await fallbackStatus.getByText('This API is not documented for .NET Standard 2.1. Showing .NET 10.').waitFor();
            assert.equal(await selector.inputValue(), 'net-10.0');
            assert.equal(apiRequests.length, 1);
            page.off('request', recordApiRequest);

            const legacyUrl = origin + legacy + '?view=netstandard-2.0';
            const legacyResponse = await page.goto(legacyUrl);
            assert.equal(legacyResponse.status(), 200);
            assert.equal(legacyResponse.request().redirectedFrom(), null);
            await selector.selectOption('net-11.0');
            await fallbackStatus.getByText('This API is not documented for .NET 11. Showing .NET Standard 2.0.').waitFor();
            assert.equal(page.url(), legacyUrl);
            assert.equal(await selector.inputValue(), 'netstandard-2.0');

            const missing = await context.request.get(origin + alphanumeric + '?view=netstandard-2.1', { maxRedirects: 0 });
            assert.equal(missing.status(), 404);
            assert.equal(missing.headers().location, undefined);

            const createPage = api + 'Cuemon.IO.StreamFactory.Create.html';
            await page.goto(origin + createPage + '?view=net-11.0');
            const overload = page.locator('article h3[id*="_System_Buffers_IBufferWriter_"]').first();
            const memberId = await overload.getAttribute('id');
            await page.goto(origin + createPage + '?view=net-11.0#' + memberId);
            await selector.selectOption('netstandard-2.0');
            await page.waitForURL(origin + createPage + '?view=netstandard-2.1#' + memberId);
            await fallbackStatus.getByText('This API is not documented for .NET Standard 2.0. Showing .NET Standard 2.1.').waitFor();
            assert.equal(await selector.inputValue(), 'netstandard-2.1');
            assert.equal(await page.locator('[id="' + memberId + '"]').count(), 1);
        }

        for (const [view, entry] of Object.entries(manifest.views)) {
            const query = '?view=' + view;
            const indexRequest = page.waitForResponse(origin + '/_framework-search/' + entry.directory + '/index.json');
            const tocRequest = page.waitForResponse(origin + api + 'toc.json' + query);
            await page.goto(origin + factory + query);
            assert.equal((await indexRequest).status(), 200);
            assert.equal((await tocRequest).status(), 200);

            const old = entry.tfm === 'netstandard2.0';
            const links = page.locator('#toc a[href*="StreamFactory.Create.html"]');
            await links.first().waitFor({ state: 'attached' });
            assert.ok((await links.first().getAttribute('href')).includes(query));
            assert.equal(await page.locator('#toc a[href*="BufferWriterOptions.html"]').count() > 0, !old);
            assert.equal(await page.locator('#toc a[href*="CallerArgumentExpressionAttribute.html"]').count() > 0, old);

            const create = page.locator('article a[href*="StreamFactory.Create.html"]').first();
            const createHref = new URL(await create.getAttribute('href'), page.url());
            await create.press('Enter');
            await page.waitForURL(createHref.href);
            const location = new URL(page.url());
            assert.equal(location.searchParams.get('view'), view);
            assert.ok(location.hash);
            const id = decodeURIComponent(location.hash.slice(1));
            const declaration = page.locator('[id="' + id + '"]');
            await declaration.waitFor({ state: 'attached' });
            await page.reload();
            assert.equal(new URL(page.url()).href, location.href);
            assert.equal(await page.locator('[id="' + id + '"]').count(), 1);

            // Use the native TOC's page link for ordinary document history.
            // DocFX also performs same-document navigation to scroll to a fragment.
            await page.goto(origin + factory + query);
            await page.getByRole('searchbox', { name: 'Filter by title' }).fill('Create');
            await links.first().press('Enter');
            const memberUrl = origin + api + 'Cuemon.IO.StreamFactory.Create.html' + query;
            await page.waitForURL(memberUrl);
            await page.goBack();
            await page.waitForURL(origin + factory + query);
            await page.goForward();
            await page.waitForURL(memberUrl);

            if (!old) {
                await page.locator('article a[href*="BufferWriterOptions.html"]:visible').first().press('Enter');
                await page.waitForURL(origin + buffer + query);
            }
            const external = page.locator('article a[href^="https://"]').first();
            assert.ok(!(await external.getAttribute('href')).includes(query));

            const search = page.locator('#search-query');
            await page.waitForFunction(() => !document.getElementById('search-query').disabled, undefined, { polling: 50 });
            await search.fill(old ? 'CallerArgumentExpressionAttribute' : 'BufferWriterOptions');
            const result = page.locator('#search-results a[href*="' + (old ? legacy : buffer) + '"]', { hasText: /Class/ });
            await result.first().waitFor({ state: 'attached' });
            const resultHref = new URL(await result.first().getAttribute('href'), origin);
            assert.equal(resultHref.pathname, old ? legacy : buffer);
            assert.equal(resultHref.searchParams.get('view'), view);
            assert.ok(resultHref.searchParams.has('q'));
            const label = await result.first().evaluate(link => getComputedStyle(link, '::after').content);
            assert.equal(JSON.parse(label), (old ? legacy : buffer) + query);
            assert.equal(await result.first().locator('xpath=../following-sibling::div[contains(@class,"item-href")]').isVisible(), false);
            const [popup] = await Promise.all([page.waitForEvent('popup'), result.first().press('Enter')]);
            await popup.waitForLoadState();
            assert.equal(popup.url(), resultHref.href);
            await popup.close();

            await search.fill('Cwalina');
            const conceptResult = page.locator('#search-results .item-title > a[href^="/index.html?q="]').first();
            await conceptResult.waitFor({ state: 'attached' });
            assert.equal(await conceptResult.getAttribute('data-framework-url'), '/index.html');

            await page.goto(origin + factory + query);
            const filter = page.getByRole('searchbox', { name: 'Filter by title' });
            await filter.fill('StreamFactory');
            const scopedTocLink = page.locator('#toc a[href*="StreamFactory.html"]').first();
            assert.equal(await scopedTocLink.getAttribute('href'), factory + query);
            const concept = page.locator('#navbar a[href$="index.html"]').first();
            await concept.waitFor({ state: 'attached' });
            assert.equal(new URL(await concept.getAttribute('href'), origin).searchParams.get('view'), null);
            console.log('Browser passed ' + view);
        }
        if (Object.keys(manifest.views).length > 1) {
            const mobile = await browser.newPage({ viewport: { width: 390, height: 844 }, colorScheme: 'dark' });
            await mobile.route('**/*', route => new URL(route.request().url()).origin === origin ? route.continue() : route.abort());
            await mobile.goto(origin + factory);
            await mobile.getByRole('button', { name: 'Show table of contents' }).click();
            const mobileSelector = mobile.getByRole('combobox', { name: 'Framework' });
            await mobileSelector.waitFor();
            await mobileSelector.focus();
            assert.equal(await mobileSelector.evaluate(element => document.activeElement === element), true);
            assert.equal(await mobile.locator('html').getAttribute('data-bs-theme'), 'dark');
            await mobile.close();
        }
        assert.deepEqual(failures, []);
        console.log('Browser passed deep links, dynamic TOC, xrefs, search, refresh and history in every configured view.');
    } finally {
        await browser.close();
    }
}

main().catch(error => {
    console.error(error);
    process.exitCode = 1;
});
