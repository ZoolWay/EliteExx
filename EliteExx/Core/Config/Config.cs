using System;

namespace Zw.EliteExx.Core.Config
{
    public class Config
    {
        public Locations Locations { get; }
        public WindowLayout WindowLayout { get; }

        public Config(Locations locations, WindowLayout windowLayout)
        {
            this.Locations = locations;
            this.WindowLayout = windowLayout;
        }
    }
}
