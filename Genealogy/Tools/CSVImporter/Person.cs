using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVImporter
{
    public class Person
    {
        //SubjectChristianName	SubjectSurname	SubjectOccupation	SubjectRelation	OthersideChristianName	OthersideSurname	OthersideOccupation

        public string ChristianName { get; set; }

        public string Surname { get; set; }

        public string Occupation { get; set; }

        public string Relation { get; set; }

        public string OthersideChristianName { get; set; }

        public string OthersideSurname { get; set; }

        public string OthersideOccupation { get; set; }

        public string ToCSVString(bool includeTrailingComma)
        {
            return ChristianName + "," + Surname + "," + Occupation + "," + Relation + "," + OthersideChristianName +
                   "," + OthersideSurname + "," + OthersideOccupation + (includeTrailingComma ? "," : "");

        }
    }
}
