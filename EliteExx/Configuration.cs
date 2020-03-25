using System;
using System.IO;
using System.Text;
using Caliburn.Micro;
using Newtonsoft.Json;
using Zw.EliteExx.Core;
using Zw.EliteExx.Core.Config;

namespace Zw.EliteExx
{
    public class Configuration
    {
        private static readonly string CONFIGFILE_NAME = "config.json";
        private static readonly Encoding CONFIGFILE_ENCODING = Encoding.UTF8;
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly EnvManager envManager;
        private readonly IEventAggregator eventAggregator;

        public Core.Config.Config Instance { get; private set; }
        
        public Configuration(EnvManager envManager, IEventAggregator eventAggregator)
        {
            this.envManager = envManager;
            this.eventAggregator = eventAggregator;
            LoadInternal(false);
        }

        public bool Load()
        {
            return LoadInternal(true);
        }

        public bool SaveWindowLayout(WindowLayout newWindowLayout)
        {
            var newInstance = new Config(this.Instance.Locations, this.Instance.Services, newWindowLayout, this.Instance.RouterSettings, this.Instance.MainLayout, this.Instance.PositionSettings);
            return SaveInternal(newInstance, false);
        }

        public bool SaveRouterSettings(RouterSettings newRouterSettings)
        {
            var newInstance = new Config(this.Instance.Locations, this.Instance.Services, this.Instance.WindowLayout, newRouterSettings, this.Instance.MainLayout, this.Instance.PositionSettings);
            return SaveInternal(newInstance, false);
        }

        public bool SaveMainLayout(MainLayout newMainLayout)
        {
            var newInstance = new Config(this.Instance.Locations, this.Instance.Services, this.Instance.WindowLayout, this.Instance.RouterSettings, newMainLayout, this.Instance.PositionSettings);
            return SaveInternal(newInstance, false);
        }

        public bool SavePositionSettings(PositionSettings newPositionSettings)
        {
            var newInstance = new Config(this.Instance.Locations, this.Instance.Services, this.Instance.WindowLayout, this.Instance.RouterSettings, this.Instance.MainLayout, newPositionSettings);
            return SaveInternal(newInstance, false);
        }

        public bool Save(Core.Config.Config configInstance)
        {
            return SaveInternal(configInstance, true);
        }

        private bool SaveInternal(Core.Config.Config configInstance, bool notify)
        {
            string configFile = CONFIGFILE_NAME;
            try
            {
                configFile = Path.Combine(this.envManager.Instance.AppFolder, CONFIGFILE_NAME);
                string contents = JsonConvert.SerializeObject(configInstance);
                File.WriteAllText(configFile, contents, CONFIGFILE_ENCODING);
                this.Instance = configInstance;
                if (notify) NotifyChangedConfiguration();
                return true;
            }
            catch (Exception ex)
            {
                log.Fatal($"Failed to load configuration from: {configFile}", ex);
                return false;
            }
        }

        private bool LoadInternal(bool notify)
        {
            string configFile = CONFIGFILE_NAME;
            try
            {
                configFile = Path.Combine(this.envManager.Instance.AppFolder, CONFIGFILE_NAME);
                string contents = File.ReadAllText(configFile, CONFIGFILE_ENCODING);
                this.Instance = JsonConvert.DeserializeObject<Core.Config.Config>(contents);
                if (notify) NotifyChangedConfiguration();
                log.InfoFormat("Loaded configuration from '{0}', contains {1} waypoints", configFile, this.Instance?.RouterSettings?.Waypoints.Length ?? 0);
                return true;
            }
            catch (Exception ex)
            {
                log.Fatal($"Failed to load configuration from: {configFile}", ex);
                return false;
            }
        }

        private void NotifyChangedConfiguration()
        {
            this.eventAggregator.PublishOnUIThread(new Ui.Events.ConfigurationChanged(this.Instance));
            var asm = IoC.Get<ActorSystemManager>(); // lazy fetch because of circular dep
            asm.NotifyUpdatedConfiguration(this.Instance);
        }
    }
}
