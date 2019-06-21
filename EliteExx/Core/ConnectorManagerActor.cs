using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akka.Actor;
using Zw.EliteExx.Core.Config;
using Zw.EliteExx.EliteDangerous;

namespace Zw.EliteExx.Core
{
    internal class ConnectorManagerActor : ReceiveActor
    {
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly Env env;
        private Config.Config config;
        private IActorRef edConnector;

        public ConnectorManagerActor(Env env, Config.Config config)
        {
            this.env = env;
            this.config = config;
            this.edConnector = ActorRefs.Nobody;

            Receive((Action<EliteDangerous.ConnectorMessage>)ReceivedEliteDangerousConnectorMessage);
            Receive((Action<ConnectorManagerMessage.Init>)ReceivedInit);
            Receive((Action<Core.ConnectorManagerMessage.ConfigUpdated>)ReceivedConfigUpdated);
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
            log.Info("Initializing");

            this.edConnector = Context.ActorOf(Props.Create(() => new EliteDangerous.ConnectorActor(this.config)), "cn-ed");
            this.edConnector.Tell(new EliteDangerous.ConnectorMessage.Init());
        }
    }
}
