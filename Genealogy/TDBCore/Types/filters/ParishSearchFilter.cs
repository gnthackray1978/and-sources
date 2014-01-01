using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types.filters
{
    public class ParishSearchFilter
    {
        public List<Guid> ParishIds { get; set; }
        public int DateFrom { get; set; }
        public int DateTo { get; set; }
        public string Deposited { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string County { get; set; }
        public string Location { get; set; }

    }
}
