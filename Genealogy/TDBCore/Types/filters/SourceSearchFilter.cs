using System;
using System.Collections.Generic;

namespace TDBCore.Types.filters
{
    public class SourceSearchFilter
    {
       
        public List<Guid> Sources { get; set; }    
        public int ToYear { get; set; }
        public int FromYear { get; set; }
        public string Description { get; set; }
        public string OriginalLocation { get; set; }
        public string Ref { get; set; }
        public bool? CopyHeld { get; set; }
        public bool? Viewed { get; set; }
        public bool? ThackrayFound { get; set; }
        public string FileCount { get; set; }
        public List<int> SourceTypes { get; set; } 
        public bool CensusSources1841 { get; set; }
        public bool CensusPlaces1841 { get; set; }
        public bool IncludeDefaultPerson { get; set; }
        public int MarriageCount { get; set; }
        public int PersonCount { get; set; }

        public int LrStart { get; set; }
        public int LrEnd { get; set; }

        public int UrStart { get; set; }
        public int UrEnd { get; set; }


    }
}
