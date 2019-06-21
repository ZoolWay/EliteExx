using System;
using Akka.Actor;
using Caliburn.Micro;

namespace Zw.EliteExx.Core
{
    internal class UiMessengerActor : ReceiveActor
    {
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly IEventAggregator eventAggregator;

        public UiMessengerActor(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            Receive((Action<UiMessengerMessage.Publish>)ReceivedPublish);
        }

        private void ReceivedPublish(UiMessengerMessage.Publish message)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug("Publishing message of type '{0}' on UI thread", message.Message.GetType().Name);
            }
            this.eventAggregator.BeginPublishOnUIThread(message.Message);
        }
    }
}
