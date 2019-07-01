using System;
using Akka.Actor;
using Caliburn.Micro;

namespace Zw.EliteExx.Core
{
    /// <summary>
    /// Manages the Akka.NET actor system.
    /// </summary>
    public class ActorSystemManager
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly EnvManager envManager;
        private readonly ActorSystem actorSystem;
        private readonly Configuration configuration;
        private IActorRef uiMessenger;
        private IActorRef connectorManager;

        public ActorSystemManager(IEventAggregator eventAggregator, EnvManager envManager, Configuration configuration)
        {
            this.eventAggregator = eventAggregator;
            this.envManager = envManager;
            this.configuration = configuration;
            this.uiMessenger = ActorRefs.Nobody;
            this.connectorManager = ActorRefs.Nobody;

            var akkaConfig = Akka.Configuration.ConfigurationFactory.Load().WithFallback(Akka.Configuration.ConfigurationFactory.Default());
            this.actorSystem = ActorSystem.Create("elite-exx", akkaConfig);
        }

        /// <summary>
        /// Initialize the core actors. Call once.
        /// </summary>
        public void Init()
        {
            this.uiMessenger = this.actorSystem.ActorOf(Props.Create(() => new UiMessengerActor(this.eventAggregator)).WithDispatcher("akka.actor.synchronized-dispatcher"), "ui-messenger");
            this.connectorManager = this.actorSystem.ActorOf(Props.Create(() => new ConnectorManagerActor(envManager.Instance, configuration.Instance, this.uiMessenger)), "cn-mng");
            this.connectorManager.Tell(new ConnectorManagerMessage.Init());
        }

        /// <summary>
        /// Initialize the Elite Dangerous connector. Call once.
        /// </summary>
        public void InitEliteDangerousConnector()
        {
            this.connectorManager.Tell(new ConnectorManagerMessage.InitEliteDangerous());
        }

        /// <summary>
        /// Notify the actor system about a new configuration.
        /// </summary>
        /// <param name="newConfig"></param>
        public void NotifyUpdatedConfiguration(Core.Config.Config newConfig)
        {
            this.connectorManager.Tell(new ConnectorManagerMessage.ConfigUpdated(newConfig));
        }
    }
}
