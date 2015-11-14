using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVImporter
{
    public class Source
    {
        //SourceId	sourceRef	PhysicalPath	Type	From	To	Location	County

        private List<Person> _linkPersons = new List<Person>();

        public Guid SourceId { get; set; }
        public string SourceRef { get; set; }

        public string PhysicalPath { get; set; }

        public string Type { get; set; }

        public string From { get; set; }
        public string To { get; set; }


        public int FromYear { get; set; }
        public int ToYear { get; set; }




        public string Location { get; set; }

        public List<Person> LinkedPersons {
            get { return _linkPersons; }
            set
            {
                _linkPersons = value;
            }
        }


        public string ToCSVString(bool includeTrailingComma)
        {
            //SourceId	sourceRef	PhysicalPath	Type	From	To	Location	County
            return SourceId + "," + SourceRef + "," + PhysicalPath + "," + Type + "," + From + "," + To + "," + Location + (includeTrailingComma ? "," : "");
        }

    }
}
