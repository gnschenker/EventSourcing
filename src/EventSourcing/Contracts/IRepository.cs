using System;
using System.Collections.Generic;
using EventSourcing.Contracts.Events;

namespace EventSourcing.Contracts
{
    public interface IRepository
    {
        void Save(Guid id, IEnumerable<IEvent> events);
    }
}