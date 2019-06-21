using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui
{
    public class ShellViewModel : Screen, IShell
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Configuration configuration;
        private readonly IWindowManager windowManager;

        public ShellViewModel(Configuration configuration, IWindowManager windowManager)
        {
            log.Debug("Creating Shell");
            this.configuration = configuration;
            this.windowManager = windowManager;
        }

        public void Configuration()
        {
            this.windowManager.ShowDialog(IoC.Get<Ui.Config.MainViewModel>());
        }

        public void Exit()
        {
            TryClose();
        }
    }
}
