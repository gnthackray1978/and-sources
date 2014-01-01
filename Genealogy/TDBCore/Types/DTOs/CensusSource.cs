using System;
using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class CensusSource : ServiceBase
    {
        public Guid SourceId { get; set; }
        public List<CensusPerson> attachedPersons { get; set; }
        public int CensusYear { get; set; }
        public string CensusRef { get; set; }
        public string CensusDesc { get; set; }

        public string Address { get; set; }
        public string Civil_Parish { get; set; }
        public string County { get; set; }
        public string Municipal_Borough { get; set; }
        public string Registration_District { get; set; }
        public string Page { get; set; }
        public string Piece { get; set; }

   


    }
}