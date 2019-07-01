using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public class DisplayEvent : PropertyChangedBase
    {
        private string text;
        private bool isHighlighted;
        private DisplayEventType eventType;
        private bool isSelected;
        private char symbol1;
        private char symbol2;

        public string Text
        {
            get => this.text;
            set
            {
                if (String.Equals(value, this.text)) return;
                this.text = value;
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

        public DisplayEventType EventType
        {
            get => this.eventType;
            set
            {
                if (value == this.eventType) return;
                this.eventType = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsSelected
        {
            get => this.isSelected;
            set
            {
                if (value == this.isSelected) return;
                this.isSelected = value;
                NotifyOfPropertyChange();
            }
        }

        public char Symbol1
        {
            get => this.symbol1;
            set
            {
                if (value == this.symbol1) return;
                this.symbol1 = value;
                NotifyOfPropertyChange();
            }
        }

        public char Symbol2
        {
            get => this.symbol2;
            set
            {
                if (value == this.symbol2) return;
                this.symbol2 = value;
                NotifyOfPropertyChange();
            }
        }

        public DisplayEvent()
        {
            this.eventType = DisplayEventType.GenericEvent;
            this.isSelected = false;
            this.isHighlighted = false;
        }
    }
}
