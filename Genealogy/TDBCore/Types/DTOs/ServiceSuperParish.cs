using System;

namespace TDBCore.Types.DTOs
{
    public class ServiceSuperParish : ServiceBase
    {
        public Guid ParishId { get; set; }//public Guid ParishId;
        public string ParishName { get; set; }//public string Name;
        public string ParishDeposited { get; set; }//public string Deposited;
        public string ParishCounty { get; set; } //public string County;
        public double ParishX { get; set; }
        public double ParishY { get; set; }
        public int ParishLocationCount { get; set; }
        public int ParishLocationOrder { get; set; }
        public string ParishGroupRef { get; set; }
    }
}