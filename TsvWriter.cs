using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfluencerScraper
{
    public static class TsvWriter
    {
        const string FileName = "data.tsv";

        public static async Task WriteTsv(IEnumerable<string[]> content)
        {
            if (File.Exists(FileName)) File.Delete(FileName);
            using (var file = new StreamWriter(FileName, false, Encoding.Unicode))
            {
                foreach (var line in content) await file.WriteLineAsync(string.Join('\t', line.Select(x => x?.Replace("\t", " "))));
            }
        }
    }
}
