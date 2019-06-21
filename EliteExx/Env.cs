using System;
using System.IO;
using System.Reflection;

namespace Zw.EliteExx
{
    public class Env
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string AppFolder { get; private set; }

        public Env()
        {
            InitAppFolder();
        }

        private void InitAppFolder()
        {
            try
            {
                Uri assemblyLocation = new Uri(Assembly.GetExecutingAssembly().Location);
                this.AppFolder = Path.GetDirectoryName(assemblyLocation.AbsolutePath);
                log.Debug($"Detected app folder: {AppFolder}");
            }
            catch (Exception ex)
            {
                log.Fatal("Could not detect app folder location!", ex);
                this.AppFolder = String.Empty;
            }
        }
    }
}
