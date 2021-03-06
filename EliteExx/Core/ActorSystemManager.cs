﻿using System;
using System.Threading.Tasks;
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
        private IActorRef uiProcessor;
        private IActorRef connectorManager;
        private IActorRef screenshotProcessor;
        private bool isInited;

        public IActorRef UiProcessor => this.uiProcessor;

        public IActorRef UiMessenger => this.uiMessenger;

        public ActorSystemManager(IEventAggregator eventAggregator, EnvManager envManager, Configuration configuration)
        {
            this.eventAggregator = eventAggregator;
            this.envManager = envManager;
            this.configuration = configuration;
            this.uiMessenger = ActorRefs.Nobody;
            this.connectorManager = ActorRefs.Nobody;
            this.uiProcessor = ActorRefs.Nobody;
            this.screenshotProcessor = ActorRefs.Nobody;
            this.isInited = false;

            var akkaConfig = Akka.Configuration.ConfigurationFactory.Load().WithFallback(Akka.Configuration.ConfigurationFactory.Default());
            this.actorSystem = ActorSystem.Create("elite-exx", akkaConfig);
        }

        /// <summary>
        /// Initialize the core actors. Call once.
        /// </summary>
        public void Init()
        {
            if (this.isInited) throw new Exception("ActorSystem is already initialized!");
            this.uiMessenger = this.actorSystem.ActorOf(Props.Create(() => new UiMessengerActor(this.eventAggregator)).WithDispatcher("akka.actor.synchronized-dispatcher"), "ui-messenger");
            this.uiProcessor = this.actorSystem.ActorOf(Props.Create(() => new UiProcessorActor(this.uiMessenger)), "ui-processor");
            this.connectorManager = this.actorSystem.ActorOf(Props.Create(() => new ConnectorManagerActor(envManager.Instance, configuration.Instance, this.uiMessenger)), "cn-mng");
            this.connectorManager.Tell(new ConnectorManagerMessage.Init());
            this.screenshotProcessor = this.actorSystem.ActorOf(Props.Create(() => new ScreenshotProcessorActor(envManager.Instance, configuration.Instance, this.uiMessenger)), "bmp-processor");
            this.screenshotProcessor.Tell(new ScreenshotProcessorMessage.Init());
            this.isInited = true;
        }

        /// <summary>
        /// Sends the actor system the given message.
        /// </summary>
        /// <param name="message"></param>
        public void Tell(IConnectorMessage message)
        {
            this.connectorManager.Tell(message);
        }

        /// <summary>
        /// Initialize the Elite Dangerous connector. Call once.
        /// </summary>
        public void InitEliteDangerousConnector()
        {
            this.connectorManager.Tell(new ConnectorManagerMessage.InitEliteDangerous());
        }

        /// <summary>
        /// Initialize the EDSM connector. Call once.
        /// </summary>
        public void InitEdsmConnector()
        {
            this.connectorManager.Tell(new ConnectorManagerMessage.InitEdsm());
        }

        public Task Shutdown()
        {
            if (this.actorSystem == null) return Task.CompletedTask;
            return this.actorSystem.Terminate();
        }

        /// <summary>
        /// Notify the actor system about a new configuration.
        /// </summary>
        /// <param name="newConfig"></param>
        public void NotifyUpdatedConfiguration(Core.Config.Config newConfig)
        {
            this.connectorManager.Tell(new ConnectorManagerMessage.ConfigUpdated(newConfig));
            this.screenshotProcessor.Tell(new ScreenshotProcessorMessage.ConfigUpdated(newConfig));
        }
    }
}
