using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public void TitleMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            Window window = GetView() as Window; // make title mouse move window move
            if ((window != null) && (e.ChangedButton == System.Windows.Input.MouseButton.Left))
            {
                window.DragMove();
            }
        }

        public void CloseMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((e.ChangedButton == System.Windows.Input.MouseButton.Left) && (e.ClickCount == 2))
            {
                Exit();
            }
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
