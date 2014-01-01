using System;
using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
   
    public class TreePerson
    {
        public int Index { get; set; }
       // public int Index;

        public double X1 { get; set; }
       // public double X1;
        
        public double X2 { get; set; }
       // public double X2;

        public double Y1 { get; set; }

      //  public double Y1;
        public double Y2 { get; set; }
       // public double Y2;


        public int RelationType { get; set; }
        //public int RelationType;
        public bool IsFamilyStart { get; set; }
        //public bool IsFamilyStart;
        public bool IsFamilyEnd { get; set; }
        //public bool IsFamilyEnd;
        public bool IsParentalLink { get; set; }
        //public bool IsParentalLink;
        public bool IsDisplayed { get; set; }
        //public bool IsDisplayed;


        public int ChildCount { get; set; }
       // public int ChildCount;
        public int FatherIdx { get; set; }
       // public int FatherIdx;
        public int MotherIdx { get; set; }
       // public int MotherIdx;
        public int ChildIdx { get; set; }
      //  public int ChildIdx;
        public int GenerationIdx { get; set; }
      //  public int GenerationIdx;



        public string FatherDOB { get; set; }
        //public string FatherDOB;
        public string Name { get; set; }
        //public string Name;
        public string Occupation { get; set; }
   //     public string Occupation;
        public string DOB { get; set; }
       // public string DOB;
        public string DOD { get; set; }
       // public string DOD;
        public string BirthLocation { get; set; }
      //  public string BirthLocation;
        public string DeathLocation { get; set; }
      //  public string DeathLocation;


        public bool IsHtmlLink { get; set; }
        //public bool IsHtmlLink;
        public List<Guid> SpouseLst { get; set; }
        //public List<Guid> SpouseLst;
        public List<Guid> ChildLst { get; set; }
        //public List<Guid> ChildLst;
        public List<int> SpouseIdxLst { get; set; }
        //public List<int> SpouseIdxLst;
        public List<int> ChildIdxLst { get; set; }
        //public List<int> ChildIdxLst;
        public Guid FatherId { get; set; }
        //public Guid FatherId;
        public Guid MotherId { get; set; }
        //public Guid MotherId;
        public Guid PersonId { get; set; }
        //public Guid PersonId;

        //public double distFromCent;
        //public double distFromTop;
        
       // public int zoom;
        public int zoom { get; set; }
        //public bool isVis


        public int DescendentCount { get; set; }
    }
}
