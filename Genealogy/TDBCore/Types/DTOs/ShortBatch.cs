using System;

namespace TDBCore.Types.DTOs
{
    public class ShortBatch
    {     
        public Guid BatchId { get; set; }

        public DateTime TimeRun { get; set; }
 
        public string Ref { get; set; }

        public bool IsDeleted { get; set; }
    }
}
