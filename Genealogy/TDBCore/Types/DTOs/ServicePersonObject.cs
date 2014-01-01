using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class ServicePersonObject : ServiceBase
    {
        public List<ServicePersonLookUp> servicePersons { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }
}