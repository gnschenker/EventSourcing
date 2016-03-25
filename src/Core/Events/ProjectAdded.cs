using System;

namespace EventSourcing.Contracts.Events
{
    public class ProjectAdded : IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}