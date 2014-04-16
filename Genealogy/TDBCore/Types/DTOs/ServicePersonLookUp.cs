using System;

namespace TDBCore.Types.DTOs
{
    public class ServicePersonLookUp : ServiceBase
    {
        public Guid PersonId { get; set; }
        public string ChristianName { get; set; }
        public string Surname { get; set; }
        public string FatherChristianName { get; set; }
        public string FatherSurname { get; set; }
        public string MotherChristianName { get; set; }
        public string MotherSurname { get; set; }
        public int DeathYear { get; set; }
        public int BirthYear { get; set; }
        public int BaptismYear { get; set; }
        public string BirthLocation { get; set; }
        public string DeathLocation { get; set; }
        public string UniqueReference { get; set; }
        public string Sources { get; set; }
        public string Events { get; set; }
        public string Spouse { get; set; }
        public string LinkedTrees { get; set; }
        public string ReferenceLocation { get; set; }

        public string SourceParishName { get; set; } 
       
        public string SourceDateStr { get; set; }

        public string SourceRef { get; set; }

        public int SourceDateInt { get; set; }

        public Guid SourceId { get; set; }
 
    }
}