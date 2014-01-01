using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class ParishCollection
    {
        public List<ParishDataType> parishDataTypes = new List<ParishDataType>();
        public List<ParishRecord> parishRecords = new List<ParishRecord>();
        public List<SilverParish> parishs = new List<SilverParish>();
        public List<ParishTranscript> parishTranscripts = new List<ParishTranscript>();
        public List<SourceRecord> sourceRecords = new List<SourceRecord>();

    }
}