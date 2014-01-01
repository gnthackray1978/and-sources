using System;

namespace TDBCore.Types.DTOs
{
    public class ServiceParishRecord : ServiceBase
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int DataType { get; set; }
        public string ParishRecordType { get; set; }
        public Guid ParishId { get; set; }
    }
}