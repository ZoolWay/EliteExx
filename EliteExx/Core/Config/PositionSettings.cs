using System;

namespace Zw.EliteExx.Core.Config
{
    public class PositionSettings
    {
        public bool? IsMiniMode { get; }
        public bool? ShowSystemData { get; }
        public double SrWidthType { get; }
        public double SrWidthDescription { get; }
        public double SrWidthDone { get; }
        public double SrWidthDiscovered { get; }
        public double SrWidthExtra { get; }
        public double SrWidthOrigin { get; }
        public bool? HideBelts { get; }

        public PositionSettings(bool? isMiniMode, bool? showSystemData, double srWidthType, double srWidthDescription, double srWidthDone, double srWidthDiscovered, double srWidthExtra, double srWidthOrigin, bool? hideBelts)
        {
            this.IsMiniMode = isMiniMode;
            this.ShowSystemData = showSystemData;
            this.SrWidthType = srWidthType;
            this.SrWidthDescription = srWidthDescription;
            this.SrWidthDone = srWidthDone;
            this.SrWidthDiscovered = srWidthDiscovered;
            this.SrWidthExtra = srWidthExtra;
            this.SrWidthOrigin = srWidthOrigin;
            this.HideBelts = hideBelts;
        }
    }
}
