using System;
using System.IO;
using System.Text;
using Caliburn.Micro;
using Newtonsoft.Json;

namespace Zw.EliteExx
{
    public class Configuration
    {
        private static readonly string CONFIGFILE_NAME = "config.json";
        private static readonly Encoding CONFIGFILE_ENCODING = Encoding.UTF8;
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Env env;
        private readonly IEventAggregator eventAggregator;

        public Core.Config.Config Instance { get; private set; }
        
        public Configuration(Env env, IEventAggregator eventAggregator)
        {
            this.env = env;
            this.eventAggregator = eventAggregator;
            Load();
        }

        public void Load()
        {
            string configFile = CONFIGFILE_NAME;
            try
            {
                configFile = Path.Combine(this.env.AppFolder, CONFIGFILE_NAME);
                string contents = File.ReadAllText(configFile, CONFIGFILE_ENCODING);
                this.Instance = JsonConvert.DeserializeObject<Core.Config.Config>(contents);
                NotifyChangedConfiguration();
            }
            catch (Exception ex)
            {
                log.Fatal($"Failed to load configuration from: {configFile}", ex);
            }            
        }
        
        public bool Save(Core.Config.Config configInstance)
        {
            string configFile = CONFIGFILE_NAME;
            try
            {
                configFile = Path.Combine(this.env.AppFolder, CONFIGFILE_NAME);
                string contents = JsonConvert.SerializeObject(configInstance);
                File.WriteAllText(configFile, contents, CONFIGFILE_ENCODING);
                this.Instance = configInstance;
                NotifyChangedConfiguration();
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
        }
    }
}
