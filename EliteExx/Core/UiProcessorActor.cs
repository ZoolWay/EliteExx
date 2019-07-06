using System;
using System.Collections.Generic;
using Akka.Actor;

namespace Zw.EliteExx.Core
{
    internal class UiProcessorActor : ReceiveActor
    {
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly IActorRef uiMessenger;
        private readonly Dictionary<object, object> lastQueuedScrollItemPerListBox;
        private ICancelable scheduledProcessScrollToBottomMessage;

        public UiProcessorActor(IActorRef uiMessenger)
        {
            this.uiMessenger = uiMessenger;
            this.lastQueuedScrollItemPerListBox = new Dictionary<object, object>();
            Receive((Action<UiProcessorMessage.QueueBufferedScrollToBottom>)ReceivedQueueBufferedScrollToBottom);
            Receive((Action<UiProcessorMessage.ProcessScrollToBottomQueue>)ReceivedProcessScrollToBottomQueue);
        }

        private void ReceivedProcessScrollToBottomQueue(UiProcessorMessage.ProcessScrollToBottomQueue message)
        {
            foreach (var kvp in this.lastQueuedScrollItemPerListBox)
            {
                var listbox = kvp.Key;
                var lastQueuedItem = kvp.Value;
                this.uiMessenger.Tell(new UiMessengerMessage.Publish(new Ui.Events.ScrollToItemCommand(listbox, lastQueuedItem)));
            }
            this.lastQueuedScrollItemPerListBox.Clear();
            CancelProcessingScrollToBottomIfQueueEmpty();
        }

        private void ReceivedQueueBufferedScrollToBottom(UiProcessorMessage.QueueBufferedScrollToBottom message)
        {
            this.lastQueuedScrollItemPerListBox[message.ListBox] = message.ScrollToItem;
            ScheduleProcessingScrollToBottom();
        }

        private void ScheduleProcessingScrollToBottom()
        {
            if ((this.scheduledProcessScrollToBottomMessage != null) && (!this.scheduledProcessScrollToBottomMessage.IsCancellationRequested)) return;
            this.scheduledProcessScrollToBottomMessage = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(500, 250, Self, new UiProcessorMessage.ProcessScrollToBottomQueue(), Self);
        }

        private void CancelProcessingScrollToBottomIfQueueEmpty()
        {
            if ((this.scheduledProcessScrollToBottomMessage == null) || (this.scheduledProcessScrollToBottomMessage.IsCancellationRequested)) return;
            if (this.lastQueuedScrollItemPerListBox.Count > 0) return;
            this.scheduledProcessScrollToBottomMessage.Cancel();
        }
    }
}
