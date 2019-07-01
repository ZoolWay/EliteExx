using System;
using Akka.Actor;
using Zw.EliteExx.Core;
using Zw.EliteExx.Core.Config;

namespace Zw.EliteExx.EliteDangerous
{
    /// <summary>
    /// Parent actor to handle journal parsing.
    /// </summary>
    public class JournalActor : ReceiveActor
    {
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly Env env;
        private readonly Config config;
        private readonly IActorRef connector;
        private IActorRef filesReader;
        private IActorRef fileReader;
        private IActorRef processor;

        public JournalActor(Env env, Config config, IActorRef connector)
        {
            this.env = env;
            this.config = config;
            this.connector = connector;
            this.filesReader = ActorRefs.Nobody;
            this.fileReader = ActorRefs.Nobody;
            this.processor = ActorRefs.Nobody;
        }

        protected override void PreStart()
        {
            log.Info($"Starting journal parsing from: {config.Locations.FolderLogs}");
            this.processor = Context.ActorOf(Props.Create(() => new JournalProcessorActor(this.connector)));
            this.fileReader = Context.ActorOf(Props.Create(() => new NdJsonFileReaderActor(this.processor)));
            this.filesReader = Context.ActorOf(Props.Create(() => new SequenceFilesReaderActor(this.env, "journal", config.Locations.FolderLogs, "Journal.*.log", this.fileReader)));

            this.filesReader.Tell(new ReaderMessage.ScanFiles());
        }

        protected override void PostStop()
        {
            // stop of children is normal behavior
        }
    }
}
