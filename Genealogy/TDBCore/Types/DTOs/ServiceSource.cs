using System;

namespace TDBCore.Types.DTOs
{
    public class ServiceSource : ServiceBase
    {
        public Guid SourceId { get; set; }
        public Guid DefaultPerson { get; set; }
        public int? SourceYear { get; set; }
        public int? SourceYearTo { get; set; }
        public string SourceRef { get; set; }
        public string SourceDesc { get; set; }
        public int FileCount { get; set; }
        public int PersonCount { get; set; }
        public int MarriageCount { get; set; }

    }
}