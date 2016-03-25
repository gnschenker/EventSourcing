using System;
using System.Collections.Generic;
using System.Text;
using EventSourcing.Contracts.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace EventSourcing.Infrastructure
{
    public static class ExtendsEvent
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None
        };

        public static EventData ToEventData(this IEvent e)
        {
            var eventId = Guid.NewGuid();
            var typeName = e.GetType().Name;
            var eventHeaders = new Dictionary<string, object>
            {
                {
                    "EventClrType", e.GetType().AssemblyQualifiedName
                }
            };
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e, SerializerSettings));
            var metaData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, SerializerSettings));
            return new EventData(eventId, typeName, true, data, metaData);
        }
    }
}