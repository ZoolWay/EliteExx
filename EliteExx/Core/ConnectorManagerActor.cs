using System;
using Akka.Actor;
using Zw.EliteExx.Core.Config;
using Zw.EliteExx.EliteDangerous;

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

        public ConnectorManagerActor(Env env, Config.Config config, IActorRef uiMessenger)
        {
            this.env = env;
            this.config = config;
            this.uiMessenger = uiMessenger;
            this.edConnector = ActorRefs.Nobody;

            Receive((Action<EliteDangerous.ConnectorMessage>)ReceivedEliteDangerousConnectorMessage);
            Receive((Action<ConnectorManagerMessage.InitEliteDangerous>)ReceivedInitEliteDangerous);
            Receive((Action<ConnectorManagerMessage.Init>)ReceivedInit);
            Receive((Action<Core.ConnectorManagerMessage.ConfigUpdated>)ReceivedConfigUpdated);
        }

        private void ReceivedInitEliteDangerous(ConnectorManagerMessage.InitEliteDangerous message)
        {
            log.Info("Initializing Elite Dangerous Connector");
            this.edConnector = Context.ActorOf(Props.Create(() => new EliteDangerous.ConnectorActor(this.env, this.config, this.uiMessenger)), "cn-ed");
            this.edConnector.Tell(new EliteDangerous.ConnectorMessage.Init());
        }

        private void ReceivedEliteDangerousConnectorMessage(ConnectorMessage message)
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
