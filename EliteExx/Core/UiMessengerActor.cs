using System;
using Akka.Actor;
using Caliburn.Micro;

namespace Zw.EliteExx.Core
{
    /// <summary>
    /// Sends events to the UI EventAggregator.
    /// Actor instance must use a synchronized dispatcher.
    /// </summary>
    internal class UiMessengerActor : ReceiveActor
    {
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly IEventAggregator eventAggregator;

        public UiMessengerActor(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            Receive((Action<UiMessengerMessage.Publish>)ReceivedPublish);
            Receive((Action<UiMessengerMessage.Error>)ReceivedError);
        }

        private void ReceivedError(UiMessengerMessage.Error message)
        {
            log.Info($"Will publish error message: {message.Message}");
            this.eventAggregator.BeginPublishOnUIThread(new Ui.Events.ShellContextError(message.Message, $"ActorSystem:{Sender.Path.ToStringWithoutAddress()}"));
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
