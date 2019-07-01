using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public class DisplayEvent : PropertyChangedBase
    {
        private string text;
        private bool isHighlighted;

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
    }
}
