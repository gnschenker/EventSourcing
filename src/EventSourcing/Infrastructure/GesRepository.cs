using System;
using System.Collections.Generic;
using System.Linq;
using EventSourcing.Contracts;
using EventSourcing.Contracts.Events;
using EventStore.ClientAPI;

namespace EventSourcing.Infrastructure
{
    public class GesRepository : IRepository
    {
        private readonly IEventStoreConnection _connection;

        public GesRepository(IEventStoreConnection connection)
        {
            _connection = connection;
        }

        public void Save(Guid id, IEnumerable<IEvent> events)
        {
            var streamName = $"Projects-{id}";
            var eventsToSave = events.Select(e => ExtendsEvent.ToEventData(e)).ToArray(); 
            _connection.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventsToSave).Wait();
        }
    }
}