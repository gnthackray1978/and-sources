using System;

namespace TDBCore.Types.DTOs
{
    public class ServiceMapDisplaySource :ServiceBase
    {
        public Guid SourceId { get; set; }
        public string SourceRef { get; set; }
        public string SourceDesc { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string OriginalLocation { get; set; }
        public bool IsCopyHeld { get; set; }
        public bool IsViewed { get; set; }
        public bool IsThackrayFound { get; set; }
        //public List<int> sourceTypes;
        public int DisplayOrder { get; set; }
    }
}