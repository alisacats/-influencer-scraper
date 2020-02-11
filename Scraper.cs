using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluencerScraper.Pages;
using InfluencerScraper.Pages.Model;
using InfluencerScraper.PuppeteerPrelude;
using PuppeteerSharp;

namespace InfluencerScraper
{
    public static class Scraper
    {
        static async Task<Page> SignIn(Page page, string login, string password)
        {
            await LoginPage.OpenLoginPage(page);
            await LoginPage.SingIn(page, login, password);
            Console.WriteLine("Waiting for sign in completion");
            return await LoginPage.WaitUntilLoginCompleted(page.Browser);
        }

        static async Task SelectFilters(Page page)
        {
            await SearchPage.GoToSearchPage(page);
            Console.WriteLine("Waiting for search button click");
            await SearchPage.WaitForFilledAndSubmittedSearchPage(page);
            await SearchPage.WaitForFirstPageLoad(page);
        }

        static async Task ScrollTillEnd(Page page)
        {
            while (await SearchPage.CanGoToNextPage(page))
            {
                await SearchPage.GoToNextPage(page);
            }
        }

        sealed class Disposable : IDisposable
        {
            private readonly Action _onDispose;
            public Disposable(Action onDispose) => _onDispose = onDispose;
            public void Dispose() => _onDispose();
        }
        
        static IDisposable InterceptNewTabs(Browser browser)
        {
            async void Handler(object x, TargetChangedArgs y)
            {
                try
                {
                    var page = await y.Target.PageAsync();
                    await page.CloseAsync();
                }
                catch (PuppeteerException) { }
            }

            browser.TargetCreated += Handler;
            return new Disposable(() => browser.TargetCreated -= Handler);
        }

        static async Task<T> RetryAFewTimes<T>(Func<Task<T>> f)
        {
            const int attemptCount = 10;
            for (var i = 0; i < attemptCount; i++)
            {
                try
                {
                    return await f();
                }
                catch (PuppeteerException)
                {
                    if (i == attemptCount - 1) throw;
                    await Task.Delay(6000);
                }
            }
            throw new Exception("WTF");
        }

        public static async Task<List<ProfileInfo>> Scrape(string login, string password)
        {
            using (var browser = await BrowserProvider.PrepareBrowser())
            {
                var pages = await browser.PagesAsync();
                var page = pages.Single();

                page = await SignIn(page, login, password);
                foreach (var p in (await browser.PagesAsync()).Where(p => p != page)) await p.CloseAsync();
                
                await SelectFilters(page);
                using (InterceptNewTabs(browser)) await ScrollTillEnd(page);
                
                var accountsInfo = new List<ProfileInfo>();
                var links = await SearchPage.GetLinks(page);
                var counter = 0;
                
                page = await browser.NewPageAsync();
                foreach (var badPage in (await browser.PagesAsync()).Where(x => x != page)) await badPage.CloseAsync();

                using (InterceptNewTabs(browser))
                {
                    foreach (var link in links)
                    {
                        try
                        {
                            Console.WriteLine($"Scraping {link} ({++counter}/{links.Length})");
                            var accountInfo = await RetryAFewTimes(async () =>
                            {
                                await ProfileInfoService.GoToProfile(page, link);
                                return await ProfileInfoService.GetInfo(page);
                            });
                            accountsInfo.Add(accountInfo);
                        }
                        catch (Exception e)
                        { 
                            Console.WriteLine($"Scraping {link} ({++counter}/{links.Length})fail");
                            continue;
                        }
                    }
                }

                return accountsInfo;
            }
        }
    }
}
