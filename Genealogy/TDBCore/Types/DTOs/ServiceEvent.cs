using System;

namespace TDBCore.Types.DTOs
{
    public class ServiceEvent : ServiceBase
    {
        public int EventDate { get; set; }

        public string EventDescription { get; set; }
        public string EventChristianName { get; set; }
        public string EventSurname { get; set; }
        public string EventLocation { get; set; }
        public string EventText { get; set; }


        public Guid EventId { get; set; }
        public Guid LinkId { get; set; }
        public int LinkTypeId { get; set; }
    }
}