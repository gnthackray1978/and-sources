using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class ServiceParishDetailObject : ServiceBase
    {
        public List<ServiceParishRecord> serviceParishRecords { get; set; }
        public List<ServiceParishTranscript> serviceParishTranscripts { get; set; }
        public List<ServiceMapDisplaySource> serviceServiceMapDisplaySource { get; set; }

        public int MarriageCount {get;set;}
        public int PersonCount { get; set; }


    }
}