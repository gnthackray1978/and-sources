using System;

namespace TDBCore.Types.DTOs
{
    public class ServiceParishCounter : ServiceBase
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public Guid ParishId { get; set; }
        public string ParishName { get; set; }

        public int Counter { get; set; }

        public decimal PX {get; set; }
        public decimal PY { get; set; }
    }
}