using System;
using System.Collections.Generic;
using Akka.Actor;
using Zw.EliteExx.Core.Config;
using Zw.EliteExx.Core.Graphics;

namespace Zw.EliteExx.Core
{
    internal class ScreenshotProcessorActor : ReceiveActor
    {
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly Env env;
        private readonly IActorRef uiMessenger;
        private readonly List<IActorRef> converters;
        private Config.Config config;
        private bool notifiedUiAboutFailures;

        public ScreenshotProcessorActor(Env env, Config.Config config, IActorRef uiMessenger)
        {
            this.env = env;
            this.config = config;
            this.uiMessenger = uiMessenger;
            this.converters = new List<IActorRef>();
            this.notifiedUiAboutFailures = false;

            Receive((Action<ScreenshotProcessorMessage.ConfigUpdated>)ReceivedConfigUpdated);
            Receive((Action<Graphics.BitmapConverterMessage.FailureNotification>)ReceivedFailureNotification);
            Receive((Action<ScreenshotProcessorMessage.Init>)ReceivedInit);
        }

        private void ReceivedFailureNotification(BitmapConverterMessage.FailureNotification message)
        {
            log.Debug("Converter '{0}' reported '{1}' while working on: {2}", Sender.Path.ToStringWithoutAddress(), message.ErrorMessage, message.FailedFilename);
            if (!this.notifiedUiAboutFailures)
            {
                this.uiMessenger.Tell(new UiMessengerMessage.Publish(new EliteDangerous.Journal.EntryMetaMessage(DateTime.UtcNow, EliteDangerous.Journal.Event.MetaMessage, "Some Bitmap conversion failed!")));
                this.notifiedUiAboutFailures = true;
            }
        }

        private void ReceivedConfigUpdated(ScreenshotProcessorMessage.ConfigUpdated message)
        {
            this.config = message.NewConfig;
            ClearConverters();
            CreateConverters();
        }

        private void ReceivedInit(ScreenshotProcessorMessage.Init message)
        {
            CreateConverters();
        }

        private void CreateConverters()
        {
            bool convertEliteStandardShots = (config.Services.ScreenshotConverter == BmpConverterMode.All);
            bool convertEliteHighresShots = (config.Services.ScreenshotConverter == BmpConverterMode.All || config.Services.ScreenshotConverter == BmpConverterMode.OnlyHighRes);
            if (convertEliteStandardShots)
            {
                IActorRef newConverter = Context.ActorOf(Props.Create(() => new BitmapConverterActor(Self, config.Locations.FolderScreenshotsElite, "Screenshot_*.bmp")));
                newConverter.Tell(new BitmapConverterMessage.Init());
                converters.Add(newConverter);
            }
            if (convertEliteHighresShots)
            {
                IActorRef newConverter = Context.ActorOf(Props.Create(() => new BitmapConverterActor(Self, config.Locations.FolderScreenshotsElite, "HighResScreenShot_*.bmp")));
                newConverter.Tell(new BitmapConverterMessage.Init());
                converters.Add(newConverter);
            }
        }

        private void ClearConverters()
        {
            foreach (var converter in converters)
            {
                converter.GracefulStop(TimeSpan.FromSeconds(10));
            }
            converters.Clear();
        }
    }
}
