using System;
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
        private long countProcessedEntries;

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
        }

        public void Handle(Entry entry)
        {
            this.CountProcessedEntries++;
            if (entry is EntryFsdJump j)
            {
                this.PositionSystem = j.StarSystem;
                this.PositionStarPos = String.Format("({0}/{1}/{2})", j.StarPos.X, j.StarPos.Y, j.StarPos.Z);
            }
            else if (entry is EntryDocked docked)
            {
                this.PositionStation = docked.StationName;
            }
            else if (entry is EntryUndocked ud)
            {
                this.PositionStation = String.Empty;
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
                }
                if (String.Compare(ds.TerraformState, "Terraformable", true) == 0)
                {
                    de.IsHighlighted = true;
                    de.Symbol1 = '\xf7a2'; // globe-europe
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
                }
                this.events.Add(de);
            }
            else if (entry is EntryFssAllBodiesFound fabf)
            {
                this.events.Add(new DisplayEvent() { Text = $"{fabf.Count} bodies in {fabf.SystemName}" });
            }
            else if (entry is EntryFssDiscoveryScan fds)
            {
                this.events.Add(new DisplayEvent() { Text = $"{fds.Progress * 100}% {fds.BodyCount} bodies, {fds.NonBodyCount} non-bodies" });
            }
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
