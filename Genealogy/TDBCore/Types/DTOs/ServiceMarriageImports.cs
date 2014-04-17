using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types.DTOs
{
    public class ServiceMarriageImports : ServiceMarriage
    {

        public string Witness1 { get; set; }
        public string Witness2 { get; set; }
        public string Witness3 { get; set; }
        public string Witness4 { get; set; }

        public string OrigFemaleSurname { get; set; }

        public string OrigMaleSurname { get; set; }

        public Guid SourceId { get; set; }

        public string FemaleFatherCName { get; set; }
        public string FemaleFatherSName { get; set; }


        public string MaleFatherCName { get; set; }
        public string MaleFatherSName { get; set; }

        public string MaleFatherOccupation { get; set; }

        public string FemaleFatherOccupation { get; set; }

    }
}
