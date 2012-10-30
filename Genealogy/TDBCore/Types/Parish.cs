using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types
{
    public class SilverParish
    {
        public Guid ParishId;
        public string Name;
        public string County;
        public double ParishX;
        public double ParishY;
        public string Deposited;
        public int LocationCount;
        public int LocationOrder;
        public string groupRef;

    }

    public class ParishCollection
    {
        public List<ParishDataType> parishDataTypes = new List<ParishDataType>();
        public List<ParishRecord> parishRecords = new List<ParishRecord>();
        public List<SilverParish> parishs = new List<SilverParish>();
        public List<ParishTranscript> parishTranscripts = new List<ParishTranscript>();
        public List<SourceRecord> sourceRecords = new List<SourceRecord>();

    }


    public class ParishTranscript
    {
        public Guid ParishId;
        public string ParishTranscriptRecord;
    }

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

    public class SearchResult
    {
        public bool IsBaptism;
        public bool IsMarriage;
        public bool IsBurial;
    
    }

}
