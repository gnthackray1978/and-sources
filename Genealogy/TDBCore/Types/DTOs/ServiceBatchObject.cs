using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class ServiceBatchObject : ServiceBase
    {
        public List<ShortBatch> Batches { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }
}
