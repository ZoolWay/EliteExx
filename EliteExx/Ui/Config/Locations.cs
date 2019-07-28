using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.Config
{
    public class Locations : PropertyChangedBase
    {
        private string folderLogs;
        private string folderScreenshotsSteam;
        private string folderScreenshotsElite;
        private string folderScreenshotsOutput;

        public string FolderLogs
        {
            get => this.folderLogs;
            set
            {
                if (String.Equals(value, this.folderLogs)) return;
                this.folderLogs = value;
                NotifyOfPropertyChange();
            }
        }

        public string FolderScreenshotsSteam
        {
            get => this.folderScreenshotsSteam;
            set
            {
                if (String.Equals(value, this.folderScreenshotsSteam)) return;
                this.folderScreenshotsSteam = value;
                NotifyOfPropertyChange();
            }
        }

        public string FolderScreenshotsElite
        {
            get => this.folderScreenshotsElite;
            set
            {
                if (String.Equals(value, this.folderScreenshotsElite)) return;
                this.folderScreenshotsElite = value;
                NotifyOfPropertyChange();
            }
        }

        public string FolderScreenshotsOutput
        {
            get => this.folderScreenshotsOutput;
            set
            {
                if (String.Equals(value, this.folderScreenshotsOutput)) return;
                this.folderScreenshotsOutput = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
