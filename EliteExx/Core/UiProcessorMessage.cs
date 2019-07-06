using System;

namespace Zw.EliteExx.Core
{
    public abstract class UiProcessorMessage
    {
        public class QueueBufferedScrollToBottom : UiProcessorMessage
        {
            public object ListBox { get; }
            public object ScrollToItem { get; }

            public QueueBufferedScrollToBottom(object listBox, object scrollToItem)
            {
                this.ListBox = listBox;
                this.ScrollToItem = scrollToItem;
            }
        }

        public class ProcessScrollToBottomQueue : UiProcessorMessage
        {
        }
    }
}
