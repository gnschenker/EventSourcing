using System;

namespace EventSourcing.Contracts.Dtos
{
    public class Project
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }
}