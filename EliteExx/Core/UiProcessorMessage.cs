using System;

namespace Zw.EliteExx.Core
{
    /// <summary>
    /// Messages for the UiProcessor.
    /// </summary>
    public abstract class UiProcessorMessage
    {
        /// <summary>
        /// Queues a debounced ScrollToBottom command.
        /// Sent by: ScrollToBottomBehavior (UI component).
        /// Received by: UiProcessor.
        /// Rate: Unlimited.
        /// </summary>
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

        /// <summary>
        /// Instructs the UiProcessor to process the current ScrollToBottom commands.
        /// Sent by: UiProcessor.
        /// Received by: UiProcessor.
        /// Rate: Unlimited.
        /// </summary>
        public class ProcessScrollToBottomQueue : UiProcessorMessage
        {
        }
    }
}
