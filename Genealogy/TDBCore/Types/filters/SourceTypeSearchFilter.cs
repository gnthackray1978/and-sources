using System.Collections.Generic;

namespace TDBCore.Types.filters
{
    public class SourceTypeSearchFilter
    {
        public SourceTypeSearchFilter()
        {
            Description = "";
        }

        public List<int> SourceTypeIds { get; set; }
        public string Description { get; set; }


    }
}
