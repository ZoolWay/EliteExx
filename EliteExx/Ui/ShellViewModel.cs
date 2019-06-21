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
        private double windowLeft;
        private double windowTop;
        private double windowWidth;
        private double windowHeight;

        public double WindowLeft
        {
            get => this.windowLeft;
            set
            {
                if (this.windowLeft == value) return;
                this.windowLeft = value;
                NotifyOfPropertyChange();
            }
        }

        public double WindowTop
        {
            get => this.windowTop;
            set
            {
                if (this.windowTop == value) return;
                this.windowTop = value;
                NotifyOfPropertyChange();
            }
        }

        public double WindowWidth
        {
            get => this.windowWidth;
            set
            {
                if (this.windowWidth == value) return;
                this.windowWidth = value;
                NotifyOfPropertyChange();
            }
        }

        public double WindowHeight
        {
            get => this.windowHeight;
            set
            {
                if (this.windowHeight == value) return;
                this.windowHeight = value;
                NotifyOfPropertyChange();
            }
        }

        public ShellViewModel(Configuration configuration, IWindowManager windowManager)
        {
            log.Debug("Creating Shell");
            this.configuration = configuration;
            this.windowManager = windowManager;
            InitWindowLayout();
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
            PersistWindowLayout();
            TryClose();
        }

        private void PersistWindowLayout()
        {
            Core.Config.WindowLayout newWindowLayout = new Core.Config.WindowLayout(this.windowLeft, this.windowTop, this.windowWidth, this.windowHeight);
            this.configuration.SaveWindowLayout(newWindowLayout);
        }

        private void InitWindowLayout()
        {
            this.windowLeft = 0;
            this.windowTop = 0;
            this.windowWidth = 450;
            this.windowHeight = 800;
            CalculateCenterPosition();

            var layoutConfig = this.configuration.Instance?.WindowLayout;
            if (layoutConfig == null) return;

            if ((layoutConfig.Left != null) && (layoutConfig.Top != null))
            {
                this.windowLeft = layoutConfig.Left.Value;
                this.windowTop = layoutConfig.Top.Value;
            }

            if (layoutConfig.Width != null) this.windowWidth = layoutConfig.Width.Value;
            if (layoutConfig.Height != null) this.windowHeight = layoutConfig.Height.Value;
        }

        private void CalculateCenterPosition()
        {
            this.windowLeft = (SystemParameters.MaximizedPrimaryScreenWidth - this.windowWidth) / 2.0;
            this.windowTop = (SystemParameters.MaximizedPrimaryScreenHeight - this.windowHeight) / 2.0;
        }
    }
}
