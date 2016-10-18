using System;
using System.Collections.Generic;
 
 
namespace TDBCore.Types
{
    public class PersonSearchFilter
    {
        public List<Guid> Ids { get; set; }
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

        public bool IsIncludeSources { get; set; }

        public Guid ParentId { get; set; }

        public string OthersideChristianName { get; set; }
        public string OthersideSurname { get; set; }
        public string OthersideRelationship { get; set; }

    }
}
