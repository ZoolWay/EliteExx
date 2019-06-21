using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Zw.EliteExx.Core;
using Zw.EliteExx.Core.Config;

namespace Zw.EliteExx.EliteDangerous
{
    internal class ConnectorActor : ReceiveActor
    {
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private Config config;

        public ConnectorActor(Config config)
        {
            this.config = config;

            Receive((Action<ConnectorMessage.Init>)ReceivedInit);
            Receive((Action<Core.ConnectorManagerMessage.ConfigUpdated>)ReceivedConfigUpdated);
        }

        private void ReceivedInit(ConnectorMessage.Init message)
        {
        }

        private void ReceivedConfigUpdated(ConnectorManagerMessage.ConfigUpdated message)
        {
            this.config = message.NewConfig;
        }
    }
}
