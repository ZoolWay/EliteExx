using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.Config
{
    public class MainViewModel : Screen
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Configuration configuration;

        public Config Config { get; }

        public MainViewModel(Configuration configuration)
        {
            this.configuration = configuration;
            this.Config = new Config(this.configuration.Instance);
        }

        public void Save()
        {
            log.Info("Saving new configuration");
            var newConfig = this.Config.BuildModel(this.configuration.Instance.WindowLayout, this.configuration.Instance.RouterSettings, this.configuration.Instance.MainLayout);
            bool success = this.configuration.Save(newConfig);
            if (success) TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
