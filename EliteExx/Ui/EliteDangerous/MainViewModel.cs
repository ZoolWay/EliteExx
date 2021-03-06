﻿using System;
using System.ComponentModel;
using System.Windows.Data;
using Caliburn.Micro;
using Zw.EliteExx.Core;
using Zw.EliteExx.Edsm;
using Zw.EliteExx.EliteDangerous.Journal;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public class MainViewModel : Screen, IHandle<Entry>, IHandle<Edsm.Messages.EliteServerState>, IDisplayEventReceiver
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly IWindowManager windowManager;
        private readonly ActorSystemManager actorSystemManager;
        private readonly Configuration configuration;
        private readonly BindableCollection<DisplayEvent> events;
        private readonly ICollectionView eventsView;
        private readonly DisplayEventBuilder displayEventBuilder;
        private readonly object routerViewModel;
        private readonly object positionViewModel;
        private DisplayEvent selectedEvent;
        private long countProcessedEntries;
        private bool isScrollBottom;
        private bool showShip;
        private bool showPosition;
        private bool showRouter;
        private string shipName;
        private string ship;
        private string shipIdent;
        private double fuelLevel;
        private double fuelCapacity;
        private bool filterHideBoringScans;

        public long CountProcessedEntries
        {
            get => this.countProcessedEntries;
            set
            {
                if (this.countProcessedEntries == value) return;
                this.countProcessedEntries = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsScrollBottom
        {
            get => this.isScrollBottom;
            set
            {
                if (this.isScrollBottom == value) return;
                this.isScrollBottom = value;
                NotifyOfPropertyChange();
            }
        }

        public DisplayEvent SelectedEvent
        {
            get => this.selectedEvent;
            set
            {
                if (Object.ReferenceEquals(value, this.selectedEvent)) return;
                this.selectedEvent = value;
                NotifyOfPropertyChange();
            }
        }

        public bool ShowPosition
        {
            get => this.showPosition;
            set
            {
                if (value == this.showPosition) return;
                this.showPosition = value;
                NotifyOfPropertyChange();
                PersistLayout();
            }
        }

        public bool ShowShip
        {
            get => this.showShip;
            set
            {
                if (value == this.showShip) return;
                this.showShip = value;
                NotifyOfPropertyChange();
                PersistLayout();
            }
        }

        public bool ShowRouter
        {
            get => this.showRouter;
            set
            {
                if (value == this.showRouter) return;
                this.showRouter = value;
                NotifyOfPropertyChange();
                PersistLayout();
            }
        }

        public string ShipName
        {
            get => this.shipName;
            set
            {
                if (String.Equals(this.shipName, value)) return;
                this.shipName = value;
                NotifyOfPropertyChange();
            }
        }

        public string Ship
        {
            get => this.ship;
            set
            {
                if (String.Equals(this.ship, value)) return;
                this.ship = value;
                NotifyOfPropertyChange();
            }
        }

        public string ShipIdent
        {
            get => this.shipIdent;
            set
            {
                if (String.Equals(this.shipIdent, value)) return;
                this.shipIdent = value;
                NotifyOfPropertyChange();
            }
        }

        public double FuelLevel
        {
            get => this.fuelLevel;
            set
            {
                if (value == this.fuelLevel) return;
                this.fuelLevel = value;
                NotifyOfPropertyChange();
            }
        }

        public double FuelCapacity
        {
            get => this.fuelCapacity;
            set
            {
                if (value == this.fuelCapacity) return;
                this.fuelCapacity = value;
                NotifyOfPropertyChange();
            }
        }

        public object Router
        {
            get => this.routerViewModel;
        }

        public object Position
        {
            get => this.positionViewModel;
        }

        public bool FilterHideBoringScans
        {
            get => this.filterHideBoringScans;
            set
            {
                if (value == this.filterHideBoringScans) return;
                this.filterHideBoringScans = value;
                NotifyOfPropertyChange();
                PersistLayout();
                this.eventsView.Refresh();
            }
        }

        public BindableCollection<DisplayEvent> Events => this.events;
        public ICollectionView EventsView => this.eventsView;

        public MainViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, ActorSystemManager actorSystemManager, Configuration configuration)
        {
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;
            this.actorSystemManager = actorSystemManager;
            this.configuration = configuration;

            this.events = new BindableCollection<DisplayEvent>();
            this.eventsView = CollectionViewSource.GetDefaultView(this.events);
            this.eventsView.Filter = FilterDisplayEvents;
            this.countProcessedEntries = 0;
            this.isScrollBottom = true;
            this.showPosition = true;
            this.showShip = true;
            this.showRouter = false;
            this.filterHideBoringScans = false;
            this.displayEventBuilder = new DisplayEventBuilder(this);

            var vmRouter = IoC.Get<Router.RouterViewModel>();
            this.routerViewModel = vmRouter;
            ScreenExtensions.ActivateWith(vmRouter, this);

            var vmPosition = IoC.Get<Position.PositionViewModel>();
            this.positionViewModel = vmPosition;
            ScreenExtensions.ActivateWith(vmPosition, this);

            LoadLayout();
        }

        public void Handle(Entry entry)
        {
            this.CountProcessedEntries++;
            this.displayEventBuilder.Process(entry);
        }

        public void Handle(Edsm.Messages.EliteServerState message)
        {
            this.events.Add(new DisplayEvent() { Text = $"Elite Server: {message.Message}" } );
        }

        public void ClearEvents()
        {
            this.events.Clear();
        }

        protected override void OnInitialize()
        {
            this.eventAggregator.Subscribe(this);
            this.actorSystemManager.InitEdsmConnector();
            this.actorSystemManager.InitEliteDangerousConnector();
            this.actorSystemManager.Tell(new Edsm.ConnectorMessage.RequestEliteServerState());
        }

        protected override void OnDeactivate(bool close)
        {
            if (close) this.eventAggregator.Unsubscribe(this);
        }

        private void LoadLayout()
        {
            this.showPosition = this.configuration.Instance.MainLayout?.ShowPosition.GetValueOrDefault(false) ?? false;
            this.showShip = this.configuration.Instance.MainLayout?.ShowShip.GetValueOrDefault(true) ?? true;
            this.showRouter = this.configuration.Instance.MainLayout?.ShowRouter.GetValueOrDefault(false) ?? false;
            this.filterHideBoringScans = this.configuration.Instance.MainLayout?.FilterHideBoringScans.GetValueOrDefault(false) ?? false;
        }

        private void PersistLayout()
        {
            Core.Config.MainLayout newLayout = new Core.Config.MainLayout(this.ShowShip, this.ShowRouter, this.ShowPosition, this.FilterHideBoringScans);
            this.configuration.SaveMainLayout(newLayout);
        }

        private bool FilterDisplayEvents(object o)
        {
            if (!this.filterHideBoringScans) return true;
            DisplayEvent de = o as DisplayEvent;
            if (de.IsBoring) return false;
            return true;
        }
    }
}
