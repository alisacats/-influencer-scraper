using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InfluencerScraper
{
    public sealed class Config
    {
        public string Login { get; set; }
        public string Password { get; set; }

        const string FileName = "config.json";
        public static async Task<Config> Read()
        {
            var json = await File.ReadAllTextAsync(FileName);
            return JsonConvert.DeserializeObject<Config>(json);
        }
    }
}
