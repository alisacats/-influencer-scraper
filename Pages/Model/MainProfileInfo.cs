namespace InfluencerScraper.Pages.Model
{
    public sealed class MainProfileInfo
    {
        public string Url { get; set; }

        public string Instagram { get; set; }
        
        public string InstagramFollowers { get; set; }
        public string Facebook { get; set; }
        public string FacebookFollowers { get; set; }
        
        public string Twitter { get; set; }
        
        public string TwitterFollowers { get; set; }
        public string Youtube { get; set; }
        
        public string YoutubeFollowers { get; set; }
        public string BlogName { get; set; }

        public string Age { get; set; }
        public string Reach { get; set; }
        public string EngagementRate { get; set; }
        public string Location { get; set; }

        public string[] Categories { get; set; }
    }
}
