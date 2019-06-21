using System;

namespace Zw.EliteExx.Core.Config
{
    public class Locations
    {
        public string FolderLogs { get; } // C:\Program Files (x86)\Steam\steamapps\common\Elite Dangerous\Products\elite-dangerous-64\Logs
        public string FolderScreenshots { get; }

        public Locations(string folderLogs, string folderScreenshots)
        {
            this.FolderLogs = folderLogs;
            this.FolderScreenshots = folderScreenshots;
        }
    }
}
