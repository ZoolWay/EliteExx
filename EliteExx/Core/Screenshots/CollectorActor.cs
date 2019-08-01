using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Akka.Actor;

namespace Zw.EliteExx.Core.Screenshots
{
    internal class CollectorActor : ReceiveActor
    {
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly IActorRef processor;
        private readonly ImmutableArray<string> sourceFolders;
        private readonly FileSystemWatcher[] pngWatchers;
        private readonly FileSystemWatcher[] jpgWatchers;
        private readonly string destinationFolder;

        public CollectorActor(IActorRef processor, ImmutableArray<string> sourceFolders, string destinationFolder)
        {
            this.processor = processor;
            this.sourceFolders = sourceFolders;
            this.pngWatchers = new FileSystemWatcher[this.sourceFolders.Length];
            this.jpgWatchers = new FileSystemWatcher[this.sourceFolders.Length];
            this.destinationFolder = destinationFolder;

            Receive((Action<CollectorMessage.Collect>)ReceivedCollect);
            Receive((Action<CollectorMessage.Init>)ReceivedInit);
        }

        protected override void PreStart()
        {
            IActorRef self = Self;
            CollectorMessage.Collect collect = new CollectorMessage.Collect();

            for (int i = 0; i < this.sourceFolders.Length; i++)
            {
                this.pngWatchers[i] = new FileSystemWatcher()
                {
                    Path = this.sourceFolders[i],
                    Filter = "*.png",
                    EnableRaisingEvents = false,
                };
                this.pngWatchers[i].Changed += new FileSystemEventHandler((sender, e) => { self.Tell(collect); });
                this.jpgWatchers[i] = new FileSystemWatcher()
                {
                    Path = this.sourceFolders[i],
                    Filter = "*.jpg",
                    EnableRaisingEvents = false,
                };
                this.jpgWatchers[i].Changed += new FileSystemEventHandler((sender, e) => { self.Tell(collect); });
            }
        }

        protected override void PostStop()
        {
            for (int i = 0; i < this.pngWatchers.Length; i++)
            {
                this.pngWatchers[i].EnableRaisingEvents = false;
                this.pngWatchers[i].Dispose();
                this.pngWatchers[i] = null;
            }
            for (int i = 0; i < this.jpgWatchers.Length; i++)
            {
                this.jpgWatchers[i].EnableRaisingEvents = false;
                this.jpgWatchers[i].Dispose();
                this.jpgWatchers[i] = null;
            }
        }

        private void ReceivedCollect(CollectorMessage.Collect message)
        {
            foreach (string sourceFolder in this.sourceFolders)
            {
                
                string[] files = Directory.GetFiles(sourceFolder, "*.png").Concat(Directory.GetFiles(sourceFolder, "*.jpg")).ToArray();
                foreach (string file in files)
                {
                    try
                    {
                        string sourceName = Path.GetFileNameWithoutExtension(file);
                        string sourceExt = Path.GetExtension(file);
                        string targetName = $"{sourceName}.{sourceExt}";
                        int idx = 0;
                        string targetFullname = Path.Combine(this.destinationFolder, targetName);
                        while (File.Exists(targetFullname))
                        {
                            targetName = $"{sourceName}.{idx:D3}.{sourceExt}";
                            targetFullname = Path.Combine(this.destinationFolder, targetName);
                            idx++;
                        }
                        File.Move(file, targetFullname);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex, "Failed to move '{0}' to collection destination", file);
                    }
                }
            }
        }

        private void ReceivedInit(CollectorMessage.Init message)
        {
            Self.Tell(new CollectorMessage.Collect());
        }
    }
}
