using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Caliburn.Micro;
using Zw.EliteExx.Edsm.Messages;
using Zw.EliteExx.EliteDangerous.Journal;

namespace Zw.EliteExx.Ui.EliteDangerous.Position
{
    public class PositionViewModel : Screen, IPositionReceiver, IHandle<Entry>, IHandle<SystemData>, IHandle<NoSystemData>
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly BindableCollection<SystemSummaryRow> systemRows;
        private readonly ICollectionView systemRowsView;
        private readonly PositionBuilder systemSummaryBuilder;
        private readonly Core.ActorSystemManager actorSystemManager;
        private Configuration configuration;
        private string positionSystem;
        private string positionStarPos;
        private string positionStation;
        private string positionSystemBodies;
        private bool isSystemRowsFontNormal;
        private bool isSystemRowsFontMini;
        private int systemRowsFontSize;
        private double srWidthType;
        private double srWidthDescription;
        private double srWidthDone;
        private double srWidthDiscovered;
        private double srWidthExtra;
        private double srWidthOrigin;
        private bool showSystemData;

        public double SrWidthType
        {
            get => this.srWidthType;
            set
            {
                if (value == this.srWidthType) return;
                this.srWidthType = value;
                NotifyOfPropertyChange();
                PersistPositionSettings();
            }
        }

        public double SrWidthDescription
        {
            get => this.srWidthDescription;
            set
            {
                if (value == this.srWidthDescription) return;
                this.srWidthDescription = value;
                NotifyOfPropertyChange();
                PersistPositionSettings();
            }
        }

        public double SrWidthDone
        {
            get => this.srWidthDone;
            set
            {
                if (value == this.srWidthDone) return;
                this.srWidthDone = value;
                NotifyOfPropertyChange();
                PersistPositionSettings();
            }
        }

        public double SrWidthDiscovered
        {
            get => this.srWidthDiscovered;
            set
            {
                if (value == this.srWidthDiscovered) return;
                this.srWidthDiscovered = value;
                NotifyOfPropertyChange();
                PersistPositionSettings();
            }
        }

        public double SrWidthExtra
        {
            get => this.srWidthExtra;
            set
            {
                if (value == this.srWidthExtra) return;
                this.srWidthExtra = value;
                NotifyOfPropertyChange();
                PersistPositionSettings();
            }
        }

        public double SrWidthOrigin
        {
            get => this.srWidthOrigin;
            set
            {
                if (value == this.srWidthOrigin) return;
                this.srWidthOrigin = value;
                NotifyOfPropertyChange();
                PersistPositionSettings();
            }
        }

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

        public bool IsSystemRowsFontNormal
        {
            get => this.isSystemRowsFontNormal;
            set
            {
                if (this.isSystemRowsFontNormal == value) return;
                this.isSystemRowsFontNormal = value;
                if (value)
                {
                    this.IsSystemRowsFontMini = false;
                    SetFontSize();
                    PersistPositionSettings();
                }
                NotifyOfPropertyChange();
            }
        }

        public bool IsSystemRowsFontMini
        {
            get => this.isSystemRowsFontMini;
            set
            {
                if (this.isSystemRowsFontMini == value) return;
                this.isSystemRowsFontMini = value;
                if (value)
                {
                    this.IsSystemRowsFontNormal = false;
                    SetFontSize();
                    PersistPositionSettings();
                }
                NotifyOfPropertyChange();
            }
        }

        public int SystemRowsFontSize
        {
            get => this.systemRowsFontSize;
            set
            {
                if (this.systemRowsFontSize == value) return;
                this.systemRowsFontSize = value;
                NotifyOfPropertyChange();
            }
        }

        public bool ShowSystemData
        {
            get => this.showSystemData;
            set
            {
                if (this.showSystemData == value) return;
                this.showSystemData = value;
                NotifyOfPropertyChange();
                PersistPositionSettings();
            }
        }

        public BindableCollection<SystemSummaryRow> SystemRows => this.systemRows;
        public ICollectionView SystemRowsView => this.systemRowsView;

        public PositionViewModel(IEventAggregator eventAggregator, Core.ActorSystemManager asm, Configuration cfg)
        {
            this.eventAggregator = eventAggregator;
            this.actorSystemManager = asm;
            this.configuration = cfg;

            this.positionSystem = "-- waiting --";
            this.positionStarPos = String.Empty;
            this.positionStation = String.Empty;
            this.positionSystemBodies = String.Empty;
            this.isSystemRowsFontNormal = true;
            this.showSystemData = true;
            this.systemRowsFontSize = 12;
            this.systemRows = new BindableCollection<SystemSummaryRow>();
            this.systemRowsView = CollectionViewSource.GetDefaultView(this.systemRows);
            this.systemRowsView.SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));
            this.systemSummaryBuilder = new PositionBuilder(this, this.actorSystemManager);
            
            LoadPositionSettings();
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

        public void Handle(Entry entry)
        {
            this.systemSummaryBuilder.Process(entry);
        }

        public void Handle(SystemData systemData)
        {
            this.systemSummaryBuilder.Process(systemData);
        }

        public void Handle(NoSystemData message)
        {
            if (!String.Equals(message.Name, this.positionSystem)) return;
            this.systemRows.Add(new SystemSummaryRow() { Order = 9000, Description = "no-edsm", IsPlaceholder = true });
        }

        protected override void OnInitialize()
        {
            this.eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            if (close) this.eventAggregator.Unsubscribe(this);
        }

        private void SetFontSize()
        {
            this.SystemRowsFontSize = (this.IsSystemRowsFontMini) ? 8 : 12;
        }

        private void LoadPositionSettings()
        {
            var ps = this.configuration.Instance.PositionSettings;

            this.srWidthType = 26;
            if (ps?.SrWidthType > 0) this.srWidthType = ps.SrWidthType;
            this.srWidthDescription = 58;
            if (ps?.SrWidthDescription > 0) this.srWidthDescription = ps.SrWidthDescription;
            this.srWidthDone = 32;
            if (ps?.SrWidthDone > 0) this.srWidthDone = ps.SrWidthDone;
            this.srWidthDiscovered = 32;
            if (ps?.SrWidthDiscovered > 0) this.srWidthDiscovered = ps.SrWidthDiscovered;
            this.srWidthExtra = 64;
            if (ps?.SrWidthExtra > 0) this.srWidthExtra = ps.SrWidthExtra;
            this.srWidthOrigin = 58;
            if (ps?.SrWidthOrigin > 0) this.srWidthOrigin = ps.SrWidthOrigin;

            this.isSystemRowsFontMini = ps?.IsMiniMode.GetValueOrDefault(false) ?? false;
            this.isSystemRowsFontNormal = !this.isSystemRowsFontMini;
            this.showSystemData = ps?.ShowSystemData.GetValueOrDefault(true) ?? true;

            SetFontSize();
        }

        private void PersistPositionSettings()
        {
            Core.Config.PositionSettings newPositionSettings = new Core.Config.PositionSettings(this.isSystemRowsFontMini, this.showSystemData, this.srWidthType, this.srWidthDescription, this.srWidthDone, this.srWidthDiscovered, this.srWidthExtra, this.srWidthOrigin);
            this.configuration.SavePositionSettings(newPositionSettings);
        }
    }
}
