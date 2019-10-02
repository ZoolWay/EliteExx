using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.EliteDangerous.Router
{
    public class Item : PropertyChangedBase
    {
        private int order;
        private string name;
        private DoneState done;
        private bool isHighlighted;

        public int Order
        {
            get => this.order;
            set
            {
                if (value == this.order) return;
                this.order = value;
                NotifyOfPropertyChange();
            }
        }

        public string Name
        {
            get => this.name;
            set
            {
                if (String.Equals(value, this.name)) return;
                this.name = value;
                NotifyOfPropertyChange();
            }
        }

        public DoneState Done
        {
            get => this.done;
            set
            {
                if (value == this.done) return;
                this.done = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsHighlighted
        {
            get => this.isHighlighted;
            set
            {
                if (value == this.isHighlighted) return;
                this.isHighlighted = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
