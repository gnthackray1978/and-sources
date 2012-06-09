using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types
{

    public class ImportPerson
    {
        public bool IsMale {get;set;}
        public string Name {get;set;}
        public string Surname {get;set;}
        public string BirthParish {get;set;}
        public string BirthStr {get;set;}
        public string BapStr {get;set;}
        public string DetStr {get;set;}
        public string DeathParish {get;set;}
        public string FatherCName{get;set;}
        public string FatherSName{get;set;}
        public string MotherCName{get;set;}
        public string MotherSName {get;set;}
        public string Source {get;set;}
        public string Notes {get;set;}
        public int BirthInt {get;set;}
        public int BapInt{get;set;}
        public int DetInt {get;set;}
        public string BirthCounty {get;set;}
        public string DeathCounty {get;set;}
        public string Occupation {get;set;}
        public string Ref_loc {get;set;}
        public string Ref_date {get;set;}
        //0, 
        public string Spouse_cname {get;set;}
        public string Spouse_sname {get;set;}
        public string Father_occupation{get;set;}
                          
        public string XrefId {get;set;}

                             
        public string FatherXRef {get;set;}
        public string MotherXRef { get; set; }        
    }


    public class ImportRelationship 
    {
        public Guid Person1 { get; set; }
        public Guid Person2 { get; set; }
        public int Type { get; set; }
    }

}
