using System;

namespace EventSourcing.Contracts.Events
{
    public class PmToProjectAssigned : IEvent
    {
        public Guid Id { get; set; }
        public Guid PmId { get; set; }
    }
}