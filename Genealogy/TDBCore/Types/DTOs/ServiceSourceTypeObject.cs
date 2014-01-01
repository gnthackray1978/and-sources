using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class ServiceSourceTypeObject : ServiceBase
    {
        public List<ServiceSourceType> serviceSources { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }
}