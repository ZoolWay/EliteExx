using System;
using Akka.Actor;
using Zw.EliteExx.Core;
using Zw.EliteExx.Core.Config;

namespace Zw.EliteExx.EliteDangerous
{
    /// <summary>
    /// Connects to Elite Dangerous.
    /// </summary>
    internal class ConnectorActor : ReceiveActor
    {
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly IActorRef uiMessenger;
        private readonly Env env;
        private Config config;
        private IActorRef journal;

        public ConnectorActor(Env env, Config config, IActorRef uiMessenger)
        {
            this.uiMessenger = uiMessenger;
            this.env = env;
            this.config = config;
            this.journal = ActorRefs.Nobody;

            Receive((Action<ConnectorMessage.JournalEntry>)ReceivedJournalEntry);
            Receive((Action<ReaderMessage.ProcessError>)ReceivedProcessError);
            Receive((Action<ConnectorMessage.Init>)ReceivedInit);
            Receive((Action<Core.ConnectorManagerMessage.ConfigUpdated>)ReceivedConfigUpdated);
        }

        private void ReceivedJournalEntry(ConnectorMessage.JournalEntry message)
        {
            switch (message.Entry.Event)
            {
                case Journal.Event.Location:
                case Journal.Event.FSDJump:
                case Journal.Event.FSSDiscoveryScan:
                case Journal.Event.FSSAllBodiesFound:
                case Journal.Event.Scan:
                case Journal.Event.Docked:
                case Journal.Event.Undocked:
                case Journal.Event.Fileheader:
                    this.uiMessenger.Tell(new UiMessengerMessage.Publish(message.Entry));
                    break;
            }
        }

        private void ReceivedProcessError(ReaderMessage.ProcessError message)
        {
            this.uiMessenger.Tell(new UiMessengerMessage.Error(message.ErrorMessage));
        }

        private void ReceivedInit(ConnectorMessage.Init message)
        {
            StartJournalProcessing();
        }

        private void ReceivedConfigUpdated(ConnectorManagerMessage.ConfigUpdated message)
        {
            this.config = message.NewConfig;

            if (!this.journal.IsNobody())
            {
                this.journal.GracefulStop(TimeSpan.FromSeconds(10));
                this.journal = ActorRefs.Nobody;
                StartJournalProcessing();
            }
        }

        private void StartJournalProcessing()
        {
            this.journal = Context.ActorOf(Props.Create(() => new JournalActor(this.env, this.config, Self)));
        }
    }
}
