using System;
using System.Diagnostics;
using System.Windows;
using Caliburn.Micro;
using Zw.EliteExx.Core;
using Zw.EliteExx.Ui.Events;

namespace Zw.EliteExx.Ui
{
    public class ShellViewModel : Screen, IShell, IHandle<ShellContextError>
    {
        private const int EXITCODE_OKAY = 0;
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Configuration configuration;
        private readonly IWindowManager windowManager;
        private readonly ActorSystemManager actorSystemManager;
        private double windowLeft;
        private double windowTop;
        private double windowWidth;
        private double windowHeight;
        private object canvasContent;

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

        public object CanvasContent
        {
            get => this.canvasContent;
            set
            {
                if (Object.ReferenceEquals(value, this.canvasContent)) return;
                this.canvasContent = value;
                NotifyOfPropertyChange();
            }
        }

        public ShellViewModel(Configuration configuration, IWindowManager windowManager, ActorSystemManager actorSystemManager)
        {
            log.Debug("Creating Shell");
            this.configuration = configuration;
            this.windowManager = windowManager;
            this.actorSystemManager = actorSystemManager;
            InitWindowLayout();
            var edMain = IoC.Get<EliteDangerous.MainViewModel>();
            this.CanvasContent = edMain;
            ScreenExtensions.ActivateWith(edMain, this);
        }

        protected override void OnInitialize()
        {
            this.actorSystemManager.Init();
        }

        protected override void OnActivate()
        {
            
        }

        protected override async void OnDeactivate(bool close)
        {
            if (close)
            {
                await this.actorSystemManager.Shutdown();
                System.Windows.Application.Current.Shutdown(EXITCODE_OKAY);
            }
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

        public void OpenJournalFolder()
        {
            if (String.IsNullOrWhiteSpace(this.configuration.Instance.Locations.FolderLogs)) return;
            Process.Start(this.configuration.Instance.Locations.FolderLogs);
        }

        public void OpenScreenshotFolderSteam()
        {
            if (String.IsNullOrWhiteSpace(this.configuration.Instance.Locations.FolderScreenshotsSteam)) return;
            Process.Start(this.configuration.Instance.Locations.FolderScreenshotsSteam);
        }

        public void OpenScreenshotFolderElite()
        {
            if (String.IsNullOrWhiteSpace(this.configuration.Instance.Locations.FolderScreenshotsElite)) return;
            Process.Start(this.configuration.Instance.Locations.FolderScreenshotsElite);
        }

        public void OpenScreenshotFolderCollected()
        {
            if (String.IsNullOrWhiteSpace(this.configuration.Instance.Locations.FolderCollectedScreenshots)) return;
            Process.Start(this.configuration.Instance.Locations.FolderCollectedScreenshots);
        }

        public void Exit()
        {
            PersistWindowLayout();
            TryClose();
        }

        public void Handle(ShellContextError message)
        {
            MessageBox.Show($"Message: {message.Message}\n\nOrigin: {message.Origin}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
