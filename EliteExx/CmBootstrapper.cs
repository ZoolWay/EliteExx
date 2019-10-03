using System;
using System.Collections.Generic;
using Caliburn.Micro;

namespace Zw.EliteExx
{
    public class CmBootstrapper : BootstrapperBase
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SimpleContainer container;

        public CmBootstrapper()
        {
            log.Debug("Creating bootstrapper");
            Initialize();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<EnvManager>();
            container.Singleton<Configuration>();
            container.Singleton<Core.ActorSystemManager>();
            container.PerRequest<Ui.IShell, Ui.ShellViewModel>();
            container.PerRequest<Ui.EliteDangerous.MainViewModel>();
            container.PerRequest<Ui.EliteDangerous.Router.RouterViewModel>();
            container.PerRequest<Ui.EliteDangerous.Router.WaypointViewModel>();
            container.PerRequest<Ui.EliteDangerous.Position.PositionViewModel>();
            container.PerRequest<Ui.Config.MainViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            log.Info("Bootrstrapper starting");
            System.Windows.Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            System.AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            System.Windows.Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            DisplayRootViewFor<Ui.IShell>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            log.Info("Bootstrapper exiting");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Fatal(String.Format("Unhandles appdomain exception; object: '{0}' ({1})", e.ExceptionObject, e.ExceptionObject?.GetType().FullName), e.ExceptionObject as Exception);
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            log.Fatal(String.Format("Unhandled dispatcher exception; dispatcher associated thread: '{0}'", e.Dispatcher?.Thread?.ManagedThreadId), e.Exception);
        }

    }
}
