using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Caliburn.Micro;
using Zw.EliteExx.Core;
using Zw.EliteExx.EliteDangerous.Journal;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public class MainViewModel : Screen, IHandle<Entry>, IDisplayEventReceiver, ISystemSummaryReceiver
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly IWindowManager windowManager;
        private readonly ActorSystemManager actorSystemManager;
        private readonly BindableCollection<DisplayEvent> events;
        private readonly ICollectionView eventsView;
        private readonly DisplayEventBuilder displayEventBuilder;
        private readonly BindableCollection<SystemSummaryRow> systemRows;
        private readonly ICollectionView systemRowsView;
        private readonly SystemSummaryBuilder systemSummaryBuilder;
        private string positionSystem;
        private string positionStarPos;
        private string positionStation;
        private string positionSystemBodies;
        private DisplayEvent selectedEvent;
        private long countProcessedEntries;
        private bool isScrollBottom;
        private bool showShip;
        private bool showPosition;
        private string shipName;
        private bool filterHideBoringScans;

        public string PositionSystem
        {
            get => this.positionSystem;
            set
            {
                if (String.Equals(value, this.positionSystem)) return;
                this.positionSystem = value;
                NotifyOfPropertyChange();
            }
        }

        public string PositionStarPos
        {
            get => this.positionStarPos;
            set
            {
                if (String.Equals(value, this.positionStarPos)) return;
                this.positionStarPos = value;
                NotifyOfPropertyChange();
            }
        }

        public string PositionStation
        {
            get => this.positionStation;
            set
            {
                if (String.Equals(value, this.positionStation)) return;
                this.positionStation = value;
                NotifyOfPropertyChange();
            }
        }

        public string PositionSystemBodies
        {
            get => this.positionSystemBodies;
            set
            {
                if (String.Equals(value, this.positionSystemBodies)) return;
                this.positionSystemBodies = value;
                NotifyOfPropertyChange();
            }
        }

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

        public bool FilterHideBoringScans
        {
            get => this.filterHideBoringScans;
            set
            {
                if (value == this.filterHideBoringScans) return;
                this.filterHideBoringScans = value;
                NotifyOfPropertyChange();
                this.eventsView.Refresh();
            }
        }

        public BindableCollection<DisplayEvent> Events => this.events;
        public ICollectionView EventsView => this.eventsView;

        public BindableCollection<SystemSummaryRow> SystemRows => this.systemRows;
        public ICollectionView SystemRowsView => this.systemRowsView;

        public MainViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, ActorSystemManager actorSystemManager)
        {
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;
            this.actorSystemManager = actorSystemManager;

            this.positionSystem = "-- waiting --";
            this.positionStarPos = String.Empty;
            this.positionStation = String.Empty;
            this.positionSystemBodies = String.Empty;
            this.events = new BindableCollection<DisplayEvent>();
            this.eventsView = CollectionViewSource.GetDefaultView(this.events);
            this.eventsView.Filter = FilterDisplayEvents;
            this.systemRows = new BindableCollection<SystemSummaryRow>();
            this.systemRowsView = CollectionViewSource.GetDefaultView(this.systemRows);
            this.systemRowsView.SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));
            this.countProcessedEntries = 0;
            this.isScrollBottom = true;
            this.showPosition = true;
            this.showShip = true;
            this.filterHideBoringScans = false;
            this.displayEventBuilder = new DisplayEventBuilder(this);
            this.systemSummaryBuilder = new SystemSummaryBuilder(this);
        }

        public void Handle(Entry entry)
        {
            this.CountProcessedEntries++;
            this.displayEventBuilder.CreateDisplayEventFor(entry);
            this.systemSummaryBuilder.Process(entry);
        }

        public void CopyPosSysNameToClip(MouseButtonEventArgs e)
        {
            if (e == null || e.ClickCount < 2 || e.ChangedButton != MouseButton.Left) return;
            System.Windows.Clipboard.SetText(this.PositionSystem);
        }

        public void CopyPosStationNameToClip(MouseButtonEventArgs e)
        {
            if (e == null || e.ClickCount < 2 || e.ChangedButton != MouseButton.Left) return;
            System.Windows.Clipboard.SetText(this.PositionStation);
        }

        public void ClearEvents()
        {
            this.events.Clear();
        }

        protected override void OnInitialize()
        {
            this.actorSystemManager.InitEliteDangerousConnector();
            this.eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            if (close) this.eventAggregator.Unsubscribe(this);
        }

        private bool FilterDisplayEvents(object o)
        {
            if (!this.filterHideBoringScans) return true;
            DisplayEvent de = o as DisplayEvent;
            if ((de.EventType == DisplayEventType.Scan) && (de.IsBoring)) return false;
            return true;
        }
    }
}
