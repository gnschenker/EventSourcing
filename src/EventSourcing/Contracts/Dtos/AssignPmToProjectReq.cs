using System;

namespace EventSourcing.Contracts.Dtos
{
    public class AssignPmToProjectReq
    {
        public Guid ProjectId { get; set; }
        public Guid StaffId { get; set; }
    }
}