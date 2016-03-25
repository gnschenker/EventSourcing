using System;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ReadModelBuilder
{
    public static class ExtendsEvents
    {
        public static object DeserializeEvent(this ResolvedEvent re)
        {
            var eventType = JObject.Parse(Encoding.UTF8.GetString(re.OriginalEvent.Metadata)).Property("EventClrType").Value;
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(re.OriginalEvent.Data), Type.GetType((string)eventType));
        }
    }
}