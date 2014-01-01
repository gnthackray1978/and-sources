using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types.filters
{
    public class MarriageSearchFilter
    {

        public Guid ParentId { get; set; }

        public string Parish { get; set; }
        public string Source { get; set; }


        public string MaleCName { get; set; }
        public string MaleSName { get; set; }

        public string FemaleCName { get; set; }
        public string FemaleSName { get; set; }

        public string County { get; set; }
        public string Location { get; set; }

        public string MaleLocation { get; set; }
        public string FemaleLocation { get; set; }


        public string Witness { get; set; }


        public int LowerDate { get; set; }
        public int UpperDate { get; set; }
    }
}
