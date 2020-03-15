using System;

namespace Zw.EliteExx.Core.Config
{
    public class MainLayout
    {
        public bool? ShowShip { get; }
        public bool? ShowRouter { get; }
        public bool? ShowPosition { get; }
        public bool? FilterHideBoringScans { get; }

        public MainLayout(bool? showShip, bool? showRouter, bool? showPosition, bool? filterHideBoringScans)
        {
            this.ShowShip = showShip;
            this.ShowRouter = showRouter;
            this.ShowPosition = showPosition;
            this.FilterHideBoringScans = filterHideBoringScans;
        }
    }
}
