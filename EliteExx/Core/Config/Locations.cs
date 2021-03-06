﻿using System;

namespace Zw.EliteExx.Core.Config
{
    public class Locations
    {
        public string FolderLogs { get; } // C:\Program Files (x86)\Steam\steamapps\common\Elite Dangerous\Products\elite-dangerous-64\Logs
        public string FolderScreenshotsSteam { get; }
        public string FolderScreenshotsElite { get; }
        public string FolderCollectedScreenshots { get; }

        public Locations(string folderLogs, string folderScreenshotsSteam, string folderScreenshotsElite, string folderCollectedScreenshots)
        {
            this.FolderLogs = folderLogs;
            this.FolderScreenshotsSteam = folderScreenshotsSteam;
            this.FolderScreenshotsElite = folderScreenshotsElite;
            this.FolderCollectedScreenshots = folderCollectedScreenshots;
        }
    }
}
