using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.Config
{
    public class Locations : PropertyChangedBase
    {
        private string folderLogs;
        private string folderScreenshots;

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

        public string FolderScreenshots
        {
            get => this.folderScreenshots;
            set
            {
                if (String.Equals(value, this.folderScreenshots)) return;
                this.folderScreenshots = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
