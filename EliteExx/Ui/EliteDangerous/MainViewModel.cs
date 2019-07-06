using System;
using System.Windows.Input;
using Caliburn.Micro;
using Zw.EliteExx.Core;
using Zw.EliteExx.EliteDangerous.Journal;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public class MainViewModel : Screen, IHandle<Entry>
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly IWindowManager windowManager;
        private readonly ActorSystemManager actorSystemManager;
        private string positionSystem;
        private string positionStarPos;
        private string positionStation;
        private BindableCollection<DisplayEvent> events;
        private DisplayEvent selectedEvent;
        private long countProcessedEntries;
        private bool isScrollBottom;

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

        public BindableCollection<DisplayEvent> Events => this.events;

        public MainViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, ActorSystemManager actorSystemManager)
        {
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;
            this.actorSystemManager = actorSystemManager;

            this.positionSystem = "-- waiting --";
            this.positionStarPos = String.Empty;
            this.positionStation = String.Empty;
            this.events = new BindableCollection<DisplayEvent>();
            this.countProcessedEntries = 0;
            this.isScrollBottom = true;
        }

        public void Handle(Entry entry)
        {
            this.CountProcessedEntries++;
            if (entry is EntryFsdJump j)
            {
                this.PositionSystem = j.StarSystem;
                this.PositionStarPos = String.Format("({0}/{1}/{2})", j.StarPos.X, j.StarPos.Y, j.StarPos.Z);
                DisplayEvent de = new DisplayEvent()
                {
                    Text = $"Jumped to {j.StarSystem} ({j.JumpDist}ly dist, {j.FuelUsed}t fuel)",
                    EventType = DisplayEventType.ShipPiloting,
                    Symbol1 = '\xf6b0', // alicorn
                    Symbol1Tooltip = "jump",
                };
                this.events.Add(de);
            }
            else if (entry is EntryDocked docked)
            {
                this.PositionStation = docked.StationName;
                DisplayEvent de = new DisplayEvent()
                {
                    Text = $"Docked at {docked.StationName} ({docked.StationType})",
                    EventType = DisplayEventType.ShipPiloting,
                    Symbol1 = '\xf5af', // plane-arrival
                    Symbol1Tooltip = "docked",
                };
                this.events.Add(de);
            }
            else if (entry is EntryUndocked ud)
            {
                this.PositionStation = String.Empty;
                DisplayEvent de = new DisplayEvent()
                {
                    Text = $"Undocked from {ud.StationName}",
                    EventType = DisplayEventType.ShipPiloting,
                    Symbol1 = '\xf5b0', // plane-departure
                    Symbol1Tooltip = "undocked",
                };
                this.events.Add(de);
            }
            else if (entry is EntryScanDetailed ds)
            {
                DisplayEvent de = new DisplayEvent()
                {
                    Text = $"Scanned {ds.BodyName} ({ds.PlanetClass}) {ds.WasDiscovered} {ds.WasMapped} {ds.TerraformState} {ds.Landable}",
                    EventType = DisplayEventType.Scan,
                };
                if (ds.WasDiscovered == false)
                {
                    de.IsHighlighted = true;
                    de.Symbol2 = '\xf890'; // sparkles
                    de.Symbol2Tooltip = "undiscovered!";
                }
                if (String.Compare(ds.TerraformState, "Terraformable", true) == 0)
                {
                    de.IsHighlighted = true;
                    de.Symbol1 = '\xf7a2'; // globe-europe
                    de.Symbol1Tooltip = "terraformable!";
                }
                this.events.Add(de);
            }
            else if (entry is EntryScanAutoScan @as)
            {
                DisplayEvent de = new DisplayEvent()
                {
                    Text = $"Auto-Scanned {@as.BodyName} {@as.WasDiscovered} {@as.WasMapped}",
                    EventType = DisplayEventType.Scan,
                };
                if (@as.WasDiscovered == false)
                {
                    de.IsHighlighted = true;
                    de.Symbol2 = '\xf890'; // sparkles
                    de.Symbol2Tooltip = "undiscovered!";
                }
                this.events.Add(de);
            }
            else if (entry is EntryFssAllBodiesFound fabf)
            {
                this.events.Add(new DisplayEvent()
                {
                    Text = $"{fabf.Count} bodies in {fabf.SystemName}",
                    EventType = DisplayEventType.Scan,
                });
            }
            else if (entry is EntryFssDiscoveryScan fds)
            {
                this.events.Add(new DisplayEvent()
                {
                    Text = $"{fds.Progress * 100}% {fds.BodyCount} bodies, {fds.NonBodyCount} non-bodies",
                    EventType = DisplayEventType.Scan,
                });
            }
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

        protected override void OnInitialize()
        {
            this.actorSystemManager.InitEliteDangerousConnector();
            this.eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            if (close) this.eventAggregator.Unsubscribe(this);
        }
    }
}
