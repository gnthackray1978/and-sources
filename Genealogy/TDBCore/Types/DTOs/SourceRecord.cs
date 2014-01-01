using System;
using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class SourceRecord
    {
        public Guid SourceId;
        public string SourceRef;
        public string SourceDesc;
        public int YearStart;
        public int YearEnd;
        public string OriginalLocation;
        public bool IsCopyHeld;
        public bool IsViewed;
        public bool IsThackrayFound;
        public List<int> sourceTYpes;
        public int DisplayOrder;
    }
}