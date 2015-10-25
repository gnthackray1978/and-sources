using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types.filters
{
    public class BatchSearchFilter
    {
        public Guid BatchId { get; set; }

        public string Ref { get; set; }
    }
}
