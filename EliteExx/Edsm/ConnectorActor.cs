using System;
using Akka.Actor;
using Akka.Routing;
using Zw.EliteExx.Core.Config;

namespace Zw.EliteExx.Edsm
{
    internal class ConnectorActor : ReceiveActor
    {
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly IActorRef uiMessenger;
        private readonly Env env;
        private readonly IActorRef processor;
        private Config config;

        public ConnectorActor(Env env, Config config, IActorRef uiMessenger)
        {
            this.uiMessenger = uiMessenger;
            this.env = env;
            this.config = config;
            this.processor = Context.ActorOf(Props.Create(() => new RequestProcessorActor(uiMessenger)).WithRouter(new RoundRobinPool(1)), "processors");

            Receive((Action<ConnectorMessage.RequestSystemData>)ReceivedRequestSystemData);
            Receive((Action<ConnectorMessage.RequestEliteServerState>)ReceivedRequestEliteServerState);
            Receive((Action<ConnectorMessage.Init>)ReceivedInit);
        }

        private void ReceivedRequestEliteServerState(ConnectorMessage.RequestEliteServerState message)
        {
            this.processor.Forward(message);
        }

        private void ReceivedRequestSystemData(ConnectorMessage.RequestSystemData message)
        {
            this.processor.Forward(message);
        }

        private void ReceivedInit(ConnectorMessage.Init message)
        {
        }
    }
}
