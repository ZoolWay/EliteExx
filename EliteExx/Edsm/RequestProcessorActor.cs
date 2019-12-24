using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Newtonsoft.Json;
using Zw.EliteExx.Core;
using Zw.EliteExx.Edsm.Messages;

namespace Zw.EliteExx.Edsm
{
    internal class RequestProcessorActor : ReceiveActor
    {
        private const string BASE_URL = "https://www.edsm.net/";

        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly IActorRef uiMessenger;

        public RequestProcessorActor(IActorRef uiMessenger)
        {
            this.uiMessenger = uiMessenger;

            ReceiveAsync<ConnectorMessage.RequestEliteServerState>(ReceivedRequestEliteServerState);
            ReceiveAsync<ConnectorMessage.RequestSystemData>(ReceivedRequestSystemData);
        }

        private async Task ReceivedRequestSystemData(ConnectorMessage.RequestSystemData message)
        {
            try
            {
                HttpWebRequest reqInfo = WebRequest.CreateHttp($"{BASE_URL}api-v1/system?systemName={message.SystemName}&showId=1&showCoordinates=1&showPermit=1&showInformation=0&showPrimaryStar=1");
                reqInfo.Timeout = 5000;
                var x1 = reqInfo.GetResponse();
                HttpWebResponse respInfo = (await reqInfo.GetResponseAsync()) as HttpWebResponse;
                string dataInfo = respInfo.GetContent();
                if ((String.IsNullOrWhiteSpace(dataInfo)) || (dataInfo == "[]") || (respInfo.StatusCode == HttpStatusCode.NotFound))
                {
                    this.uiMessenger.Tell(new UiMessengerMessage.Publish(new NoSystemData(message.SystemName)));
                    return;
                }
                var objInfo = JsonConvert.DeserializeObject<Responses.SystemReq>(dataInfo);
                HttpWebRequest reqBodies = WebRequest.CreateHttp($"{BASE_URL}api-system-v1/bodies?systemName={message.SystemName}");
                reqBodies.Timeout = 5000;
                HttpWebResponse respBodies = (await reqBodies.GetResponseAsync()) as HttpWebResponse;
                string dataBodies = respBodies.GetContent();
                var objBodies = JsonConvert.DeserializeObject<Responses.BodiesReq>(dataBodies);
                List<BodyData> bodies = new List<BodyData>();
                foreach (var b in objBodies.Bodies)
                {
                    bodies.Add(new BodyData(b.Id64, b.Name, b.BodyId, b.Discovery?.Commander, b.TerraformingState));
                }
                SystemData systemData = new SystemData(objInfo.Name, bodies.ToImmutableArray());
                this.uiMessenger.Tell(new UiMessengerMessage.Publish(systemData));
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                log.Warning($"Timeout while getting system data for {message.SystemName}");
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Failed to get system data for {message.SystemName}");
            }
        }

        private async Task ReceivedRequestEliteServerState(ConnectorMessage.RequestEliteServerState message)
        {
            HttpWebRequest req = WebRequest.CreateHttp($"{BASE_URL}api-status-v1/elite-server");
            HttpWebResponse resp = (await req.GetResponseAsync()) as HttpWebResponse;
            string data = resp.GetContent();
            var dataEs = JsonConvert.DeserializeObject<Responses.EliteServerReq>(data);
            this.uiMessenger.Tell(new UiMessengerMessage.Publish(new EliteServerState(dataEs.LastUpdate, dataEs.Message)));
        }
    }
}
