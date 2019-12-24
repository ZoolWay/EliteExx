using System;
using Caliburn.Micro;
using Zw.EliteExx.Core;
using Zw.EliteExx.EliteDangerous.Journal;

namespace Zw.EliteExx.Ui.EliteDangerous.Position
{
    public class SystemSummaryRow : PropertyChangedBase
    {
        private long order;
        private long bodyId;
        private BodyType bodyType;
        private string description;
        private DoneState doneState;
        private DataOrigin dataOrigin;
        private bool isBoring;
        private bool isHighlighted;
        private string extraInfo;
        private bool isDiscovered;
        private bool isMapped;
        private bool isPlaceholder;

        public long Order
        {
            get => this.order;
            set
            {
                if (this.order == value) return;
                this.order = value;
                NotifyOfPropertyChange();
            }
        }

        public long BodyId
        {
            get => this.bodyId;
            set
            {
                if (this.bodyId == value) return;
                this.bodyId = value;
                NotifyOfPropertyChange();
            }
        }

        public BodyType BodyType
        {
            get => this.bodyType;
            set
            {
                if (this.bodyType == value) return;
                this.bodyType = value;
                NotifyOfPropertyChange();
            }
        }

        public string Description
        {
            get => this.description;
            set
            {
                if (String.Equals(this.description, value)) return;
                this.description = value;
                NotifyOfPropertyChange();
            }
        }

        public DoneState DoneState
        {
            get => this.doneState;
            set
            {
                if (this.doneState == value) return;
                this.doneState = value;
                NotifyOfPropertyChange();
            }
        }

        public DataOrigin DataOrigin
        {
            get => this.dataOrigin;
            set
            {
                if (this.dataOrigin == value) return;
                this.dataOrigin = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsBoring
        {
            get => this.isBoring;
            set
            {
                if (this.isBoring == value) return;
                this.isBoring = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsHighlighted
        {
            get => this.isHighlighted;
            set
            {
                if (this.isHighlighted == value) return;
                this.isHighlighted = value;
                NotifyOfPropertyChange();
            }
        }

        public string ExtraInfo
        {
            get => this.extraInfo;
            set
            {
                if (String.Equals(this.extraInfo, value)) return;
                this.extraInfo = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsDiscovered
        {
            get => this.isDiscovered;
            set
            {
                if (value == this.isDiscovered) return;
                this.isDiscovered = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsMapped
        {
            get => this.isMapped;
            set
            {
                if (value == this.isMapped) return;
                this.isMapped = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsPlaceholder
        {
            get => this.isPlaceholder;
            set
            {
                if (value == this.isPlaceholder) return;
                this.isPlaceholder = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
