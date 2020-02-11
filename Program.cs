using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfluencerScraper
{
    static class Program
    {
        static async Task Main()
        {
            try
            {
                var config = await Config.Read();
                var accountsInfo = await Scraper.Scrape(config.Login, config.Password);

                var data = new List<string[]>
                {
                    new[]
                    {
                        "Url",
                        "Instagram",
                        "Instagram Followers",
                        "Facebook",
                        "Facebook Followers",
                        "Twitter",
                        "Twitter Followers",
                        "Youtube",
                        "Youtube Followers",
                        "Blog name",
                        "Age",
                        "Reach",
                        "Engagement Rate",
                        "Location",
                        "Categories",
                        "Men Percent Common",
                        "Women Percent Common",
                        "Men Percent 13-17",
                        "Men Percent 18-24",
                        "Men Percent 25-34",
                        "Men Percent 35-44",
                        "Men Percent 45-64",
                        "Women Percent 13-17",
                        "Women Percent 18-24",
                        "Women Percent 25-34",
                        "Women Percent 35-44",
                        "Women Percent 45-64",
                        "Top Countries",
                        "Top Cites"
                    }
                };
                accountsInfo.ForEach(x =>
                {
                    var main = x.MainProfileInfo;
                    var demographics = x.DemographicsInfo;

                    string Percent(double? v) => v == null ? null : $"{v.Value.ToString("F").Replace(',', '.')}%";
                    data.Add(new[]
                    {
                        main.Url,
                        main.Instagram,
                        main.InstagramFollowers,
                        main.Facebook,
                        main.FacebookFollowers,
                        main.Twitter,
                        main.TwitterFollowers,
                        main.Youtube,
                        main.YoutubeFollowers,
                        main.BlogName,
                        main.Age,
                        main.Reach,
                        main.EngagementRate,
                        main.Location,
                        string.Join(" | ", main.Categories),
                        demographics?.MenPercentCommon,
                        demographics?.WomenPercentCommon,
                        Percent(demographics?.MenPercent1317),
                        Percent(demographics?.MenPercent1824),
                        Percent(demographics?.MenPercent2534),
                        Percent(demographics?.MenPercent3544),
                        Percent(demographics?.MenPercent4564),
                        Percent(demographics?.WomenPercent1317),
                        Percent(demographics?.WomenPercent1824),
                        Percent(demographics?.WomenPercent2534),
                        Percent(demographics?.WomenPercent3544),
                        Percent(demographics?.WomenPercent4564),
                        demographics?.TopCountries,
                        demographics?.TopCites
                    });
                });

                await TsvWriter.WriteTsv(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex);
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
