using System.Runtime.InteropServices;

namespace InfluencerScraper.PuppeteerPrelude
{
    public static class PlatformInfo
    {
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static readonly bool IsMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }
}
