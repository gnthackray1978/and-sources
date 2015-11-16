using System;
using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class SourceAjaxDto : ServiceBase
    {


        public Guid SourceId { get; set; }
        public string SourceRef { get; set; }
        public string SourceDesc { get; set; }
        public string SourceNotes { get; set; }

        // public int SourceDate { get; set; }
        // public int SourceDateTo { get; set; }

        public int SourceFileCount { get; set; }

        public string SourceDateStr { get; set; }
        public string SourceDateStrTo { get; set; }

        public string OriginalLocation { get; set; }

        public bool IsCopyHeld { get; set; }
        public bool IsViewed { get; set; }
        public bool IsThackrayFound { get; set; }

        public string Parishs { get; set; }

        public string SourceTypes { get; set; }

        public string VirtualLocation { get; set; }

        public List<FileBasicInfo> Files { get; set; }

    }
}