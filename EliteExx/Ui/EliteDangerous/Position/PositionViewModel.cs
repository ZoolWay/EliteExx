﻿using System;
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
        private string positionSystem;
        private string positionStarPos;
        private string positionStation;
        private string positionSystemBodies;
        private bool isSystemRowsFontNormal;
        private bool isSystemRowsFontMini;
        private int systemRowsFontSize;

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
                    this.SystemRowsFontSize = 12;
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
                    this.SystemRowsFontSize = 8;
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

        public BindableCollection<SystemSummaryRow> SystemRows => this.systemRows;
        public ICollectionView SystemRowsView => this.systemRowsView;

        public PositionViewModel(IEventAggregator eventAggregator, Core.ActorSystemManager asm)
        {
            this.eventAggregator = eventAggregator;
            this.actorSystemManager = asm;

            this.positionSystem = "-- waiting --";
            this.positionStarPos = String.Empty;
            this.positionStation = String.Empty;
            this.positionSystemBodies = String.Empty;
            this.isSystemRowsFontNormal = true;
            this.systemRowsFontSize = 12;
            this.systemRows = new BindableCollection<SystemSummaryRow>();
            this.systemRowsView = CollectionViewSource.GetDefaultView(this.systemRows);
            this.systemRowsView.SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));
            this.systemSummaryBuilder = new PositionBuilder(this, this.actorSystemManager);
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
    }
}
