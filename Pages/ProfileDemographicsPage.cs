using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using InfluencerScraper.Pages.Model;
using PuppeteerSharp;

namespace InfluencerScraper.Pages
{
    public static class ProfileDemographicsPage
    {
        static async Task<double[]> GetChartInfo(Page page, bool male)
        {
            var script = $@"
                html = document.documentElement.innerHTML;
                regexRes = /getElementById\(('{(male ? "male" : "female")}-age'|'audience-age'|'sidebar-age')\).+?var vals = \[(.+?)\]/.exec(html.replace(/\n/gm, """"));
                regexRes == null || regexRes[2] == null ? null : regexRes[2]
                    .split(',')
                    .map(parseFloat)
                    .map(x => x * 100)";
            var menStatistic = await page.EvaluateExpressionAsync<double[]>(script);
            if (menStatistic == null) return null;
            if (menStatistic.Sum() > 1000)
                menStatistic = menStatistic.Select(x => x / 100).ToArray();
            return menStatistic;
        }

        public static async Task<DemographicsInfo> GetDemographicsInfo(Page page)
        {
            if (await page.QuerySelectorAsync("a[href$='/demographics']") == null) return null;

            var href = await page.EvaluateExpressionAsync<string>("document.querySelector(\"a[href$='/demographics']\").href");
            await page.WaitForSelectorAsync("div.col-md-3 > div:nth-child(2) > div:nth-child(4) > div:nth-child(2) > p");
            var tmp = await page.EvaluateExpressionAsync<string[]>(
                "[...document.querySelectorAll('div.col-md-3 > div:nth-child(2) > div:nth-child(4) > div:nth-child(2) > p')].map(x => x.innerText.replace(/[, ]/gm, ' ').split(' ').filter(x => x.length > 0))[0]"
            );

            var menPercent = tmp?[0];
            var womenPercent = tmp?[1];
            
            await page.GoToAsync(href);
            var menStatistic = await GetChartInfo(page, true);
            var womenStatistic = await GetChartInfo(page, false);
            
            var script = @"
                [...document.querySelectorAll('div.col-sm-4 > div.row > div:first-child')]
                    .slice(0, 3)
                    .map(x => x.innerText.trim())
                    .join('; ')";
            var topCountries = await page.EvaluateExpressionAsync<string>(script);

            script = @"
                [...document.querySelectorAll('div.col-sm-4 > div.row > div:first-child')]
                    .slice(5)
                    .map(x => x.innerText.trim())
                    .join('; ')";
            var topCities = await page.EvaluateExpressionAsync<string>(script);

            double? Get(double[] arr, int idx) => arr == null || idx >= arr.Length ? (double?) null : arr[idx];
            
            return new DemographicsInfo
            {
                MenPercentCommon = menPercent,
                MenPercent1317 = Get(menStatistic, 0),
                MenPercent1824 = Get(menStatistic, 1),
                MenPercent2534 = Get(menStatistic, 2),
                MenPercent3544 = Get(menStatistic, 3),
                MenPercent4564 = Get(menStatistic, 4),
                WomenPercentCommon = womenPercent,
                WomenPercent1317 = Get(womenStatistic, 0),
                WomenPercent1824 = Get(womenStatistic, 1),
                WomenPercent2534 = Get(womenStatistic, 2),
                WomenPercent3544 = Get(womenStatistic, 3),
                WomenPercent4564 = Get(womenStatistic, 4),
                TopCountries = topCountries,
                TopCites = topCities
            };
        }
    }
}
