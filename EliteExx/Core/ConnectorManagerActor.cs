using System;
using Akka.Actor;
using Zw.EliteExx.Core.Config;

namespace Zw.EliteExx.Core
{
    /// <summary>
    /// Connector Manager handles all integration connections.
    /// </summary>
    internal class ConnectorManagerActor : ReceiveActor
    {
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly Env env;
        private readonly IActorRef uiMessenger;
        private Config.Config config;
        private IActorRef edConnector;
        private IActorRef edsmConnector;

        public ConnectorManagerActor(Env env, Config.Config config, IActorRef uiMessenger)
        {
            this.env = env;
            this.config = config;
            this.uiMessenger = uiMessenger;
            this.edConnector = ActorRefs.Nobody;
            this.edsmConnector = ActorRefs.Nobody;

            Receive((Action<EliteDangerous.ConnectorMessage>)ReceivedEliteDangerousConnectorMessage);
            Receive((Action<Edsm.ConnectorMessage>)ReceivedEdsmConnectorMessage);
            Receive((Action<ConnectorManagerMessage.InitEliteDangerous>)ReceivedInitEliteDangerous);
            Receive((Action<ConnectorManagerMessage.InitEdsm>)ReceivedInitEdsm);
            Receive((Action<ConnectorManagerMessage.Init>)ReceivedInit);
            Receive((Action<Core.ConnectorManagerMessage.ConfigUpdated>)ReceivedConfigUpdated);
        }

        private void ReceivedInitEdsm(ConnectorManagerMessage.InitEdsm message)
        {
            log.Info("Initializing the EDSM Connector");
            this.edsmConnector = Context.ActorOf(Props.Create(() => new Edsm.ConnectorActor(this.env, this.config, this.uiMessenger)), "cn-edsm");
            this.edsmConnector.Tell(new Edsm.ConnectorMessage.Init());
        }

        private void ReceivedInitEliteDangerous(ConnectorManagerMessage.InitEliteDangerous message)
        {
            log.Info("Initializing Elite Dangerous Connector");
            this.edConnector = Context.ActorOf(Props.Create(() => new EliteDangerous.ConnectorActor(this.env, this.config, this.uiMessenger)), "cn-ed");
            this.edConnector.Tell(new EliteDangerous.ConnectorMessage.Init());
        }

        private void ReceivedEdsmConnectorMessage(Edsm.ConnectorMessage message)
        {
            this.edsmConnector.Tell(message);
        }

        private void ReceivedEliteDangerousConnectorMessage(EliteDangerous.ConnectorMessage message)
        {
            this.edConnector.Tell(message);
        }

        private void ReceivedConfigUpdated(ConnectorManagerMessage.ConfigUpdated message)
        {
            log.Info("Applying updated config");
            this.config = message.NewConfig;
            if (!this.edConnector.IsNobody()) this.edConnector.Tell(message);
        }

        private void ReceivedInit(ConnectorManagerMessage.Init message)
        {
            log.Info("Initializing ConnectorManager");
        }
    }
}
