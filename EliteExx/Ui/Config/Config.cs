using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.Config
{
    public class Config : PropertyChangedBase
    {
        public Locations Locations { get; }
        public Services Services { get; }

        public Config()
        {
            this.Locations = new Locations();
            this.Services = new Services();
        }

        public Config(Core.Config.Config configurationModel) : this()
        {
            this.Locations.FolderLogs = configurationModel?.Locations?.FolderLogs;
            this.Locations.FolderScreenshotsSteam = configurationModel?.Locations?.FolderScreenshotsSteam;
            this.Locations.FolderScreenshotsElite = configurationModel?.Locations?.FolderScreenshotsElite;
            this.Locations.FolderCollectedScreenshots = configurationModel?.Locations?.FolderCollectedScreenshots;
            this.Services.JournalParser = configurationModel?.Services?.JournalParser ?? false;
            this.Services.ScreenshotConverter = configurationModel?.Services?.ScreenshotConverter ?? Core.Config.BmpConverterMode.Deactivated;
            this.Services.CollectScreenshots = configurationModel?.Services?.CollectScreenshots ?? false;
        }

        public Core.Config.Config BuildModel(Core.Config.WindowLayout existingWindowLayout)
        {
            Core.Config.Locations locations = new Core.Config.Locations(this.Locations.FolderLogs, this.Locations.FolderScreenshotsSteam, this.Locations.FolderScreenshotsElite, this.Locations.FolderCollectedScreenshots);
            Core.Config.Services services = new Core.Config.Services(this.Services.JournalParser, this.Services.ScreenshotConverter, this.Services.CollectScreenshots);
            Core.Config.Config configModel = new Core.Config.Config(locations, services, existingWindowLayout);
            return configModel;
        }
    }
}
