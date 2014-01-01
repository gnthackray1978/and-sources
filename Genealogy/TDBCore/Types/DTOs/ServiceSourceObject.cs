using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class ServiceSourceObject : ServiceBase
    {
        public List<ServiceSource> serviceSources { get; set; }
        public List<CensusSource> CensusSources { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }
}