using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.Config
{
    public class Config : PropertyChangedBase
    {
        public Locations Locations { get; }

        public Config()
        {
            this.Locations = new Locations();
        }

        public Config(Core.Config.Config configurationModel) : this()
        {
            this.Locations.FolderLogs = configurationModel?.Locations?.FolderLogs;
            this.Locations.FolderScreenshots = configurationModel?.Locations?.FolderScreenshots;
        }

        public Core.Config.Config BuildModel(Core.Config.WindowLayout existingWindowLayout)
        {
            Core.Config.Locations locations = new Core.Config.Locations(this.Locations.FolderLogs, this.Locations.FolderScreenshots);
            Core.Config.Config configModel = new Core.Config.Config(locations, existingWindowLayout);
            return configModel;
        }
    }
}
