using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types.DTOs
{
    public class SourceDto :ServiceBase
    {

        public Guid SourceId { get; set; }
        public string SourceRef { get; set; }
        public string SourceDesc { get; set; }
        public string SourceNotes { get; set; }

         
        public int SourceFileCount { get; set; }

        public string SourceDateStr { get; set; }
        public string SourceDateStrTo { get; set; }

        public string OriginalLocation { get; set; }

        public bool IsCopyHeld { get; set; }
        public bool IsViewed { get; set; }
        public bool IsThackrayFound { get; set; }

        public List<Guid> Parishs { get; set; }

        public List<int> SourceTypes { get; set; }

        public List<ServiceFile> Files { get; set; }

        public string VirtualLocation { get; set; }

    }
}
