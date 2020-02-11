using System.Linq;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace InfluencerScraper.PuppeteerPrelude
{
    static class BrowserMediaBlocker
    {
        public static async Task<Browser> SetupInterceptor(Browser browser)
        {
            var blacklist = new[]
            {
                ResourceType.Image,
                ResourceType.Media,
                ResourceType.Font,
                ResourceType.WebSocket
            };

            async Task DisableMedia(Page page)
            {
                await page.SetRequestInterceptionAsync(true);
                page.Request += async (obj, args) =>
                {
                    var req = args.Request;
                    var isBlacked = blacklist.Contains(req.ResourceType);
                    if (isBlacked) await req.RespondAsync(new ResponseData {BodyData = new byte[0]});
                    else await req.ContinueAsync();
                };
            }

            var pages = await browser.PagesAsync();
            foreach (var page in pages) await DisableMedia(page);

            browser.TargetCreated += async (obj1, args1) =>
            {
                try
                {
                    var target = args1.Target;
                    var page = await target.PageAsync();
                    if (page == null) return;
                    await DisableMedia(page);
                }
                // Protocol error (Page.createIsolatedWorld): Could not create isolated world
                catch (PuppeteerException) { }
            };

            return browser;
        }
    }
}
