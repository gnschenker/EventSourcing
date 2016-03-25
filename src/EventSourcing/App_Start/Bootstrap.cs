using System;
using System.Collections.Generic;
using EventSourcing.Contracts;
using EventSourcing.Contracts.Events;

namespace EventSourcing
{
    public class Bootstrap
    {
        public static void Init()
        {
            IoC.GesRepository = new RepositoryMock();
        }
    }

    public class RepositoryMock : IRepository
    {
        public void Save(Guid id, IEnumerable<IEvent> events)
        {
            //do nothing
        }
    }
}