using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class StationFactionConverter : JsonConverter
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return new StationFaction(null, null);
            if (reader.TokenType == JsonToken.String) return new StationFaction(reader.Value as string, null);
            if (reader.TokenType == JsonToken.StartObject)
            {
                try
                {
                    JObject obj = JObject.Load(reader);
                    return obj.ToObject<StationFaction>();
                }
                finally
                {
                    if (reader.TokenType != JsonToken.EndObject) throw new Exception("Invalid JSON structure at StationFaction");
                }
            }
            log.Error("Failed to deserialize object to StationFaction");
            return new StationFaction(null, null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
