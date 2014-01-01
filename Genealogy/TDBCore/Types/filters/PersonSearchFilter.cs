using System;
using System.Collections.Generic;
 
 
namespace TDBCore.Types
{
    public class PersonSearchFilter
    {
        public string CName { get;  set; }
        public string Surname { get;  set; }
        public string Location { get;  set; }
        public int LowerDate { get;  set; }
        public int UpperDate { get;  set; }
        public string FatherChristianName { get;  set; }
        public string FatherSurname { get;  set; }

        public string MotherChristianName { get;  set; }
        public string MotherSurname { get;  set; }
        public string SourceString { get;  set; }
        public List<Guid> Sources { get;  set; }
        public string ParishString { get; set; }

        public string County { get;  set; }
        public string SpouseChristianName { get;  set; }

        public bool IsIncludeBirths { get; set; }
        public bool IsIncludeDeaths { get; set; }

        public Guid ParentId { get; set; }
       
    }
}
