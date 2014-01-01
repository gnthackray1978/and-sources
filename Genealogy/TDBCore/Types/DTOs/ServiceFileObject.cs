using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class ServiceFileObject : ServiceBase
    {
        public List<ServiceFile> serviceFiles { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }
}