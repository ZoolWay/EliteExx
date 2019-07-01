using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class StarPosJsonConverter : JsonConverter
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return new StarPos(null, null, null);
            if (reader.TokenType == JsonToken.StartArray)
            {
                try
                {
                    JArray array = JArray.Load(reader);
                    var values = array.Values<double>().ToArray();
                    if (values.Length != 3)
                    {
                        log.Warn($"Invalid starpos (lengh={values.Length},value={array.ToString()})");
                        return new StarPos(null, null, null);
                    }
                    return new StarPos(values[0], values[1], values[2]);
                }
                finally
                {
                    if (reader.TokenType != JsonToken.EndArray) throw new Exception("Invalid JSON structure at StarPos");
                }
            }
            log.Error("Failed to deserialize array to StarPos");
            return new StarPos(null, null, null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
