using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Akka.Actor;
using Zw.EliteExx.Core.Screenshots;

namespace Zw.EliteExx.Core.Graphics
{
    internal class BitmapConverterActor : ReceiveActor
    {
        private const int INTERVAL_SCAN = 5 * 60 * 1000;
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly string folder;
        private readonly string filefilter;
        private readonly IActorRef processor;
        private FileSystemWatcher fileSystemWatcher;
        private ICancelable subIntervalConvert;

        public BitmapConverterActor(IActorRef processor, string folder, string filefilter)
        {
            this.processor = processor;
            this.folder = folder;
            this.filefilter = filefilter;

            Receive((Action<BitmapConverterMessage.Convert>)ReceivedConvert);
            Receive((Action<BitmapConverterMessage.Init>)ReceivedInit);
        }

        protected override void PreStart()
        {
            this.fileSystemWatcher = new FileSystemWatcher()
            {
                Path = this.folder,
                Filter = this.filefilter,
                EnableRaisingEvents = false,
            };
            IActorRef self = Self;
            BitmapConverterMessage.Convert convertMessage = new BitmapConverterMessage.Convert();
            this.fileSystemWatcher.Changed += new FileSystemEventHandler((sender, e) => { self.Tell(convertMessage); });
            this.subIntervalConvert = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(INTERVAL_SCAN, INTERVAL_SCAN, Self, new BitmapConverterMessage.Convert(), Self);
        }

        protected override void PostStop()
        {
            this.subIntervalConvert.CancelIfNotNull();
            this.fileSystemWatcher.EnableRaisingEvents = false;
            this.fileSystemWatcher.Dispose();
            this.fileSystemWatcher = null;
        }

        private void ReceivedConvert(BitmapConverterMessage.Convert message)
        {
            string[] targetFiles = Directory.GetFiles(this.folder, this.filefilter);
            log.Info("Found {0} files to convert ({1})", targetFiles.Length, this.filefilter);
            // note: we could use child actors to do the actual conversion and a pool router to limit the concurrency of that - but I guess we only process screenshots of one player actually ;)
            foreach (var targetFile in targetFiles)
            {
                try
                {
                    string converterFilename = Path.ChangeExtension(targetFile, "png");
                    using (var image = Image.FromFile(targetFile))
                    {
                        if (File.Exists(converterFilename))
                        {
                            log.Warning("Skipping to convert {0} because it seems to already exist!", targetFile);
                            continue;
                        }
                        image.Save(converterFilename, ImageFormat.Png);
                        log.Info("Saved: {0}", converterFilename);
                    }
                    File.Delete(targetFile);
                    this.processor.Tell(new BitmapConverterMessage.SuccessNotification(converterFilename));
                }
                catch (Exception ex)
                {
                    log.Error(ex, "Failed to convert: {0}", targetFile);
                    this.processor.Tell(new BitmapConverterMessage.FailureNotification(ex.Message, targetFile));
                }
            }
        }

        private void ReceivedInit(BitmapConverterMessage.Init message)
        {
            this.fileSystemWatcher.EnableRaisingEvents = true;
            Self.Tell(new BitmapConverterMessage.Convert());
        }

    }
}
