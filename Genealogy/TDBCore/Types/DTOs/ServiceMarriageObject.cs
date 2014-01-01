using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class ServiceMarriageObject : ServiceBase
    {
        public List<ServiceMarriageLookup> serviceMarriages { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }
}