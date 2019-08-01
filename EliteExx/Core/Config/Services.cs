using System;

namespace Zw.EliteExx.Core.Config
{
    public class Services
    {
        public bool JournalParser { get; }
        public BmpConverterMode ScreenshotConverter { get; }
        public bool CollectScreenshots { get; }

        public Services(bool journalParser, BmpConverterMode screenshotConverter, bool collectScreenshots)
        {
            this.JournalParser = journalParser;
            this.ScreenshotConverter = screenshotConverter;
            this.CollectScreenshots = collectScreenshots;
        }
    }
}
