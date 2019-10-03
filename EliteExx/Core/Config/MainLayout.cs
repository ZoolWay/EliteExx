using System;

namespace Zw.EliteExx.Core.Config
{
    public class MainLayout
    {
        public bool? ShowShip { get; }
        public bool? ShowRouter { get; }
        public bool? ShowPosition { get; }

        public MainLayout(bool? showShip, bool? showRouter, bool? showPosition)
        {
            this.ShowShip = showShip;
            this.ShowRouter = showRouter;
            this.ShowPosition = showPosition;
        }
    }
}
