using System;
using Caliburn.Micro;
using Zw.EliteExx.Core.Config;

namespace Zw.EliteExx.Ui.Config
{
    public class Services : PropertyChangedBase
    {
        private bool journalParser;
        private BmpConverterMode screenshotConverter;
        private bool collectScreenshots;
        
        public bool JournalParser
        {
            get => this.journalParser;
            set
            {
                if (value == this.journalParser) return;
                this.journalParser = value;
                NotifyOfPropertyChange();
            }
        }

        public BmpConverterMode ScreenshotConverter
        {
            get => this.screenshotConverter;
            set
            {
                if (value == this.screenshotConverter) return;
                this.screenshotConverter = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => ScreenshotConverterAllChecked);
                NotifyOfPropertyChange(() => ScreenshotConverterOnlyHighresChecked);
                NotifyOfPropertyChange(() => ScreenshotConverterDeactivatedChecked);
            }
        }

        public bool CollectScreenshots
        {
            get => this.collectScreenshots;
            set
            {
                if (value == this.collectScreenshots) return;
                this.collectScreenshots = value;
                NotifyOfPropertyChange();
            }
        }

        public bool ScreenshotConverterAllChecked
        {
            get => this.screenshotConverter == BmpConverterMode.All;
            set
            {
                if (!value) return; // only when setting to true
                if (value == this.ScreenshotConverterAllChecked) return;
                this.ScreenshotConverter = BmpConverterMode.All; // will trigger notifications
            }
        }
        public bool ScreenshotConverterOnlyHighresChecked
        {
            get => this.screenshotConverter == BmpConverterMode.OnlyHighRes;
            set
            {
                if (!value) return; // only when setting to true
                if (value == this.ScreenshotConverterOnlyHighresChecked) return;
                this.ScreenshotConverter = BmpConverterMode.OnlyHighRes; // will trigger notifications
            }
        }

        public bool ScreenshotConverterDeactivatedChecked
        {
            get => this.screenshotConverter == BmpConverterMode.Deactivated;
            set
            {
                if (!value) return; // only when setting to true
                if (value == this.ScreenshotConverterDeactivatedChecked) return;
                this.ScreenshotConverter = BmpConverterMode.Deactivated; // will trigger notifications
            }
        }
    }
}
