﻿using System;

namespace Zw.EliteExx.Core.Config
{
    public class Config
    {
        public Locations Locations { get; }
        public Services Services { get; }
        public WindowLayout WindowLayout { get; }
        public RouterSettings RouterSettings { get; }
        public MainLayout MainLayout { get; }

        public Config(Locations locations, Services services, WindowLayout windowLayout, RouterSettings routerSettings, MainLayout mainLayout)
        {
            this.Locations = locations;
            this.Services = services;
            this.WindowLayout = windowLayout;
            this.RouterSettings = routerSettings;
            this.MainLayout = mainLayout;
        }
    }
}
