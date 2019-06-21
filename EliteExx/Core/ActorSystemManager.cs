using System;
using Akka.Actor;
using Caliburn.Micro;

namespace Zw.EliteExx.Core
{
    public class ActorSystemManager
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly EnvManager envManager;
        private readonly ActorSystem actorSystem;
        private readonly Configuration configuration;
        private readonly IActorRef uiMessenger;
        private readonly IActorRef connectorManager;

        public ActorSystemManager(IEventAggregator eventAggregator, EnvManager envManager, Configuration configuration)
        {
            this.eventAggregator = eventAggregator;
            this.envManager = envManager;
            this.configuration = configuration;
            var akkaConfig = Akka.Configuration.ConfigurationFactory.Load().WithFallback(Akka.Configuration.ConfigurationFactory.Default());
            this.actorSystem = ActorSystem.Create("elite-exx", akkaConfig);
            this.uiMessenger = this.actorSystem.ActorOf(Props.Create(() => new UiMessengerActor(this.eventAggregator)).WithDispatcher("akka.actor.synchronized-dispatcher"), "ui-messenger");
            this.connectorManager = this.actorSystem.ActorOf(Props.Create(() => new ConnectorManagerActor(envManager.Instance, configuration.Instance)), "cn-mng");
            this.connectorManager.Tell(new ConnectorManagerMessage.Init());
        }

        public void NotifyUpdatedConfiguration(Core.Config.Config newConfig)
        {
            this.connectorManager.Tell(new ConnectorManagerMessage.ConfigUpdated(newConfig));
        }
    }
}
