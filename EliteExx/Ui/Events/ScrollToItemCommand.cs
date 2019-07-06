using System;

namespace Zw.EliteExx.Ui.Events
{
    public class ScrollToItemCommand
    {
        public object ListBox { get; }
        public object TargetItem { get; }

        public ScrollToItemCommand(object listBox, object targetItem)
        {
            this.ListBox = listBox;
            this.TargetItem = targetItem;
        }
    }
}
