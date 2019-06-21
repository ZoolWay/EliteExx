using System;
using System.IO;
using System.Reflection;

namespace Zw.EliteExx
{
    public class EnvManager
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Core.Config.Env Instance { get; private set; }

        public EnvManager()
        {
            Init();
        }

        private void Init()
        {
            string appFolder = String.Empty;
            try
            {
                Uri assemblyLocation = new Uri(Assembly.GetExecutingAssembly().Location);
                appFolder = Path.GetDirectoryName(assemblyLocation.AbsolutePath);
                log.Debug($"Detected app folder: {appFolder}");
            }
            catch (Exception ex)
            {
                log.Fatal("Could not detect app folder location!", ex);
            }
            this.Instance = new Core.Config.Env(appFolder);
        }
    }
}
