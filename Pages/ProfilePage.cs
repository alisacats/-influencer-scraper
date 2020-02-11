using System.Linq;
using System.Threading.Tasks;
using InfluencerScraper.Pages.Model;
using PuppeteerSharp;

namespace InfluencerScraper.Pages
{
    public static class ProfilePage
    {
        public static async Task<MainProfileInfo> GetMainProfileInfo(Page page)
        {
            var url = await page.QuerySelectorAsync(".col-md-3 a[target*='blank']") != null
                ? await page.EvaluateExpressionAsync<string>(
                    "(() => document.querySelector(\".col-md-3 a[target*='blank']\").text)()")
                : null;

            var instagram = await page.EvaluateExpressionAsync<string>(
                    "(() => document.querySelector(\".fa.fa-instagram+a[target*='blank']\").text)()");
            
            var instagramFollowers = await page.QuerySelectorAsync(".fa.fa-instagram+a[target*='blank']+span") != null
                ? await page.EvaluateExpressionAsync<string>(
                    "(() => document.querySelector(\".fa.fa-instagram+a[target*='blank']+span\").textContent)()")
                : null;
            
            var facebook = await page.QuerySelectorAsync(".fa.fa-facebook+a[target*='blank']") != null
              ? await page.EvaluateExpressionAsync<string>(
                  "(() => document.querySelector(\".fa.fa-facebook+a[target*='blank']\").href)()")
              : null;
            
            var facebookFollowers = await page.QuerySelectorAsync(".fa.fa-facebook+a[target*='blank']+span") != null
                ? await page.EvaluateExpressionAsync<string>(
                    "(() => document.querySelector(\".fa.fa-facebook+a[target*='blank']+span\").textContent)()")
                : null;
            
            var twitter = await page.QuerySelectorAsync(".fa.fa-twitter+a[target*='blank']") != null
                ? await page.EvaluateExpressionAsync<string>(
                    "(() => document.querySelector(\".fa.fa-twitter+a[target*='blank']\").href)()")
                : null;
            
            var twitterFollowers = await page.QuerySelectorAsync(".fa.fa-twitter+a[target*='blank']") != null
                ? await page.EvaluateExpressionAsync<string>(
                    "(() => document.querySelector(\".fa.fa-twitter+a[target*='blank']+span\").textContent)()")
                : null;
            
            var youtube = await page.QuerySelectorAsync(".fa.fa-youtube+a[target*='blank']") != null
              ? await page.EvaluateExpressionAsync<string>(
                  "(() => document.querySelector(\".fa.fa-youtube+a[target*='blank']\").href)()")
              : null;
            
            var youtubeFollowers = await page.QuerySelectorAsync(".fa.fa-youtube+a[target*='blank']+span") != null
                ? await page.EvaluateExpressionAsync<string>(
                    "(() => document.querySelector(\".fa.fa-youtube+a[target*='blank']+span\").textContent)()")
                : null;
           
            var blogName =
                await page.EvaluateExpressionAsync<string>(
                    "document.querySelector('div.col-md-4 h1').innerText");

            var demographicsLabel = await page.EvaluateExpressionAsync<string>(
                "[...document.querySelectorAll('p')].map(x => x.innerText).filter(x => x.startsWith('Demographics')).slice(0, 1).map(x => x.split('\\n')[1])[0]"
            );
            demographicsLabel = demographicsLabel?.Split(',').Last()
                .Replace("years old", "").Trim();
            var age = demographicsLabel != null && demographicsLabel.Length < 3 ? demographicsLabel: "";


            var reach = await page.EvaluateExpressionAsync<string>(
                "[...document.querySelectorAll('p')].map(x => x.innerText).filter(x => x.endsWith('Reach')).slice(0, 1).map(x => x.split('|')[1].trim().split(' ')[0])[0]"
            );
            var engagementRate = await page.EvaluateExpressionAsync<string>(
                "[...document.querySelectorAll('p')].map(x => x.innerText).filter(x => x.includes('Engagement')).slice(0, 1).map(x => x.split('\\n')[0])[0]"
            );
            var location = await page.EvaluateExpressionAsync<string>(
                "([...document.querySelectorAll('p')].map(x => x.innerText).filter(x => x.startsWith('Location')).slice(0, 1).map(x => x.split('\\n')))[0][1].replace(/,/gm, ' ').replace(/  /gm, ' ')"
            );

            var categories = await page.EvaluateExpressionAsync<string[]>(
                "[...document.querySelectorAll('.category-ribbon a')].map(x => x.innerText)"
            );

            return new MainProfileInfo
            {
                Url = url,
                Instagram = instagram,
                Facebook = facebook,
                Youtube = youtube,
                BlogName = blogName,
                Age = age,
                Reach = reach,
                EngagementRate = engagementRate,
                Location = location,
                Categories = categories,
                FacebookFollowers = facebookFollowers,
                Twitter = twitter,
                TwitterFollowers = twitterFollowers,
                InstagramFollowers = instagramFollowers,
                YoutubeFollowers = youtubeFollowers
            };
        }
    }
}
