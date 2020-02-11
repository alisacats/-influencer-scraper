using System.Threading.Tasks;
using InfluencerScraper.Pages.Model;
using PuppeteerSharp;

namespace InfluencerScraper.Pages
{
    public static class ProfileInfoService
    {
        public static async Task GoToProfile(Page page, string link)
        {
            await page.GoToAsync(link);
            try
            {
                await page.WaitForSelectorAsync("#myChart.chartjs-render-monitor");
            }
            catch (PuppeteerException) { } // there is a rare case when there is no chart
        }

        public static async Task<ProfileInfo> GetInfo(Page page)
        {
            var mainProfileInfo = await ProfilePage.GetMainProfileInfo(page);
            var demographicsInfo = await ProfileDemographicsPage.GetDemographicsInfo(page);
            return new ProfileInfo { MainProfileInfo = mainProfileInfo, DemographicsInfo = demographicsInfo };
        }
    }
}
