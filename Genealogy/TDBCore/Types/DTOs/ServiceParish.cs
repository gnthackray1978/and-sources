using System;

namespace TDBCore.Types.DTOs
{
    public class ServiceParish : ServiceBase
    {
        public Guid ParishId { get; set; }
        public string ParishName { get; set; }
        public string ParishDeposited { get; set; }
        public string ParishParent { get; set; }
        public string ParishCounty { get; set; } 
        public int ParishStartYear { get; set; }
        public int ParishEndYear { get; set; }
        public double ParishLat { get; set; }
        public double ParishLong { get; set; }
        public string ParishNote { get; set; }

    }

    public class ServiceParishAdd
    {
        public string ParishId { get; set; }
        public string ParishName { get; set; }
        public string ParishDeposited { get; set; }
        public string ParishParent { get; set; }
        public string ParishCounty { get; set; }
        public string ParishStartYear { get; set; }
        public string ParishEndYear { get; set; }
        public string ParishLat { get; set; }
        public string ParishLong { get; set; }
        public string ParishNote { get; set; }

    }


}