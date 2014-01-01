using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class ServiceParishObject : ServiceBase
    {
        public List<ServiceParish> serviceParishs { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }
}