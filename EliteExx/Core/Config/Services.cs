using System;

namespace Zw.EliteExx.Core.Config
{
    public class Services
    {
        public bool JournalParser { get; }
        public BmpConverterMode ScreenshotConverter { get; }

        public Services(bool journalParser, BmpConverterMode screenshotConverter)
        {
            this.JournalParser = journalParser;
            this.ScreenshotConverter = screenshotConverter;
        }
    }
}
