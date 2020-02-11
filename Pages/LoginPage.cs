using System;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace InfluencerScraper.Pages
{
    public static class LoginPage
    {
        public static async Task SingIn(Page page, string login, string password)
        {
            await page.WaitForSelectorAsync("input[name='username']");
            await page.ClickAsync("input[name='username']");
            await page.Keyboard.TypeAsync(login);
            await page.WaitForSelectorAsync("input[name='password']");
            await page.ClickAsync("input[name='password']");
            await page.Keyboard.TypeAsync(password);
            await page.WaitForSelectorAsync("._0mzm-.sqdOP.L3NKy");
            await page.ClickAsync("._0mzm-.sqdOP.L3NKy");
        }

        public static async Task<Page> WaitUntilLoginCompleted(Browser browser)
        {
            while (true)
            {
                foreach (var page in await browser.PagesAsync())
                {
                    if (!page.Url.Contains("instagram")) return page;
                }
                await Task.Delay(1000);
            }
        }

        public static async Task OpenLoginPage(Page page)
        {
            await page.GoToAsync("https://influence.co/");
            await page.WaitForSelectorAsync("a[href='/users/auth/instagram']");
            await page.ClickAsync("a[href='/users/auth/instagram']");
        }
    }
}
