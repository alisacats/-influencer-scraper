using System;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace InfluencerScraper.PuppeteerPrelude
{
    public static class BrowserProvider
    {
        public static async Task<Browser> PrepareBrowser(Action<LaunchOptions> launchOptionsConfigurator = null)
        {
            var browserPath = ChromePathProvider.GetChromePath();

            var timeout = (int) TimeSpan.FromMinutes(2).TotalMilliseconds;
            var launchOptions = new LaunchOptions
            {
                ExecutablePath = browserPath,
                Headless = false,
                DefaultViewport = new ViewPortOptions { Width = 0, Height = 0 },
                Timeout = timeout
            };
            launchOptionsConfigurator?.Invoke(launchOptions);
            var browser = await Puppeteer.LaunchAsync(launchOptions);
            browser.DefaultWaitForTimeout = timeout;
            foreach (var page in await browser.PagesAsync())
            {
                page.DefaultTimeout = page.DefaultNavigationTimeout = timeout;
            }
            await BrowserMediaBlocker.SetupInterceptor(browser);
            return browser;
        }
    }
}
