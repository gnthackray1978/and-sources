using System;

namespace TDBCore.Types.DTOs
{
    public class ServiceMarriageLookup :ServiceBase
    {
        public Guid MarriageId { get; set; }
        public string MaleCName { get; set; }
        public string MaleSName { get; set; }
        public string FemaleCName { get; set; }
        public string FemaleSName { get; set; }
        public string MarriageDate { get; set; }
        public string MarriageLocation { get; set; }
        public string Witnesses { get; set; }
        public string UniqueRef { get; set; }
        public string Sources { get; set; }
        public string TotalEvents { get; set; }
        public string Notes { get; set; }
        public string LinkedTrees { get; set; }
    }
}