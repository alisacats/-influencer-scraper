using System.Threading.Tasks;
using PuppeteerSharp;

namespace InfluencerScraper.Pages
{
    public static class SearchPage
    {
        public static async Task GoToSearchPage(Page page)
        {
            await page.GoToAsync("https://influence.co/influencer_searches/advanced");
        }

        const string SearchResultCount = @"
[...document.querySelectorAll('h3')]
.map(x => x.innerText)
.filter(x => x.includes('Members') || x.includes('Influencers'))
.map(x => parseInt(x[0].split(' ')))
[0]
";

        static async Task WaitForSearchResultCount(Page page)
        {
            await page.WaitForExpressionAsync(SearchResultCount);
        }

        static async Task<int?> GetSearchResultCount(Page page)
        {
            return await page.EvaluateExpressionAsync<int?>(SearchResultCount);
        }

        public static async Task ClickSearchButton(Page page)
        {
            await page.ClickAsync(".btn.btn-lg.btn-primary.track-event");
        }

        public static async Task WaitForFilledAndSubmittedSearchPage(Page page)
        {
            while (true)
            {
                try
                {
                    if (await GetSearchResultCount(page) != null) break;
                }
                catch (PuppeteerException) { }
                await Task.Delay(1000);
            }
        }

        public static async Task WaitForFirstPageLoad(Page page)
        {
            var script = @"document.querySelector(""[src^='/assets/looading-gif']"") == null";
            await page.WaitForExpressionAsync(script);

            await WaitForSearchResultCount(page);
            var count = await GetSearchResultCount(page);

            if (count == 0) return;
            await page.WaitForExpressionAsync("document.querySelector('.lazy-load-image') == null");
        }

        public static async Task<bool> CanGoToNextPage(Page page)
        {
            await WaitForFirstPageLoad(page);
            const string script = @"(() => document.querySelector('.btn.btn-default.btn-lg') !== null )()";
            var result = await page.EvaluateExpressionAsync<bool>(script);
            return result;
        }

        public static async Task GoToNextPage(Page page)
        {
            await page.WaitForSelectorAsync(".btn.btn-default.btn-lg");
            try
            {
                await page.EvaluateExpressionAsync("document.querySelector('.btn.btn-default.btn-lg').click()");
            }
            catch (PuppeteerException) { }
        }


        public static async Task<string[]> GetLinks(Page page)
        {
            var links = await page.EvaluateExpressionAsync<string[]>(
                "(()=>[...document.querySelectorAll('.advanced-search-card.clearfix')].map((x) => x.href))()");
            return links;
        }
    }
}
