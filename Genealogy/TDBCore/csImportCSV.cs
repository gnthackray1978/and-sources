using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using GedItter.BirthDeathRecords.BLL;

using System.Data.OleDb;

using System.Data;

using System.IO;

using System.Text.RegularExpressions;

using System.Diagnostics;

using GedItter.MarriageRecords.BLL;

using GedItter.BLL;



using Gedcom;
using GedcomParser;
using GedItter.Tools;
using TDBCore.BLL;
using TDBCore.EntityModel;
////using TDBCore.Datasets;



namespace GedItter
{



    public class csImportCsv
    {


     //   private string MarriageFields = "";
      //  private string BDFields = "";
     //   private string SourceFields = "";


        private List<string> MarriageFieldList = new List<string>(new string[] { "MaleCName", "MaleSName", "MaleLocation", "MaleInfo", "FemaleCName", "FemaleSName", "FemaleLocation", "FemaleInfo", "Date", "MarriageLocation", 
            "YearIntVal", "MarriageCounty", "Source", "Witness1", "Witness2", "Witness3", "Witness4", "OrigMaleSurname", "OrigFemaleSurname", "MaleOccupation", "FemaleOccupation", "FemaleIsKnownWidow", "MaleIsKnownWidower", "IsBanns",
        "IsLic","SourceId","MaleAge","FemaleAge","FemaleFather","MaleFather","FemaleFatherOccupation","MaleFatherOccupation" , "LocationId" });

        //, , ,,, , , ,, , 																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																					

        private List<string> BDFieldList = new List<string>(new string[] { "IsMale", "ChristianName", "Surname", "BirthLocation", "BirthDateStr", "BaptismDateStr", "DeathDateStr", "DeathLocation", "FatherChristianName", "FatherSurname",
            "MotherChristianName", "MotherSurname", "Notes", "Source", "BirthInt", "BapInt" , "DeathInt" , "DeathCounty" , "BirthCounty" , "Occupation" , "ReferenceLocation" , "ReferenceDateStr" , 
            "ReferenceDateInt" , "SpouseName" , "SpouseSurname" , "FatherOccupation" , "AgeYear" , "AgeMonth" , "AgeDay" , "AgeWeek" , "Notes2" , "SourceId" , "LocationId" , "DeathLocationId" });


        private List<string> SourceFieldList = new List<string>(new string[] { "SourceDesc", "SourceOrigLocat","IsCopyHeld","IsViewed","IsThackrayFound","SourceDate","SourceDateTo","SourceRef","Notes","SourceParish","SourceType"});

        public csImportCsv() { }

        #region temp non comformist stuff

        public void ImportNonComformistBurials(string path, Guid parishId)
        {

            DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();



         //   string lineList = File.ReadAllText(path);


         //   string[] lines = Regex.Split(lineList, "Marriage of");
            
            string[] lines = System.IO.File.ReadAllLines(path);
            
            foreach (string _line in lines)
            { 
                string[] marParts = _line.Split(',');

               
                
                foreach (string marPart in marParts)
                {

                   
                }


            }
            
            return;


//            Burial of ? ?//|PD Register of Burials at the Wesleyan Chapel of East Brook in Bradford, Yorkshire from 1826 to 1837
            //|TNA ReferenceRG4 / Piece 3158 / Folio 3
            //|[Chapel/Registry]
            //|Full Name? ?


            //|Date of Burial Unclear - see original page image
            //|Date of Birth
            //|Date of Death
            //|Place of AbodeLime Kilns
            //|Parish of Abode
            //|County of Abode
            //|Registration Date
            //|Registration Town/CountyBradford, Yorkshire
            //|Ceremony Performed by

            //|Place of Burial East End
            //|Profession
            //|Husband's/Father's Profession
            //|Cause of death
            //|Grave Number
            //|Undertaker|
            //|Relation
            //|Description




            //|Age



            //|Husband/Father William Thackray
            //|Wife/Mother








            int idx = 0;

            foreach (string line in lines)
            {

                if (line.Trim() == "") continue;

                string[] parts = line.Split('|');

                string cname = "";
                string sname = "";
                string fathercname = "";
                string fathersname = "";
                string mothercname = "";
                string mothersname = "";
                string birthlocation = "";
                string birthCounty = "";
                string birthDate = "";
                string bapDate = "";
                string deathLocation = "";
                string deathCounty = "";
                string deathDate = "";

                string notes = "";
                string source = "";
                string fatherOccupation = "";

                string spouseCName = "";
                string spouseSName = "";
                string age = "";
                int intAge = -1;
                    
        
                
//Death of Unknown Thackery
                //|PD Register of Births and Baptisms and a Register of Deaths in the British Lying-In Hospital in Endell Street, St Giles in the Fields, Holborn, Middlesex from 1760 to 1763
                //|TNA ReferenceRG8 / Piece 55 / Folio|[Chapel/Registry]
                //|Full Name? Thackery
                //|Date of Death 16 February 1763
                //|Place of Death
                //|Date of Birth
                //|Age
                //|ProfessionStarch Maker
                //|Relation
                //|DescriptionHospital Record Of Child Death
                //|Place of Abode
                //|Parish of AbodeSt John Westminster
                //|County of Abode-
                //|Registration Date
                //|Registration Town/CountyHolborn, Middlesex
                //|Ceremony Performed by
                //|Husband/Father William Thackery
                //|Husband's/Father's Profession
                //|Wife/Mother Ann Thackery
                //|Cause of death
                //|Grave Number
                //|Undertaker|

                Dictionary<string, int> tempList = new Dictionary<string, int>();

                            #region keys
                            tempList.Add("5y & 11m", 5);


                            tempList.Add("1y 8m", 2);
                            tempList.Add("2y 6m", 2);
                            tempList.Add("9 Mo", 1);
                            tempList.Add("1y 7m", 2);
                            tempList.Add("10 M", 1);

                            tempList.Add("2 Y 9 M",3);
                            tempList.Add("3w", 0);
                            tempList.Add("12w",0);
                            tempList.Add("9 M",1);
                            tempList.Add("1 Y 9 M", 2);
                            tempList.Add("20 W", 0);
                            tempList.Add("17 W", 0);
                            tempList.Add("5 W", 0);
                            tempList.Add("11d", 0);

                            tempList.Add("2m", 0);
                            tempList.Add("3m", 0);
                            tempList.Add("4m",0);
                            tempList.Add("7m", 1);
                            tempList.Add("8m", 1);
                            tempList.Add("9m", 1);
                            tempList.Add("10m",1);
                            tempList.Add("11m", 1);
                            tempList.Add("12m", 1);
                            tempList.Add("13m", 1);
                            tempList.Add("14m", 1);                     

                            tempList.Add("18m", 1);
                            tempList.Add("21m", 2);
                            tempList.Add("22m", 2);

                            tempList.Add("21 M", 2);
                            tempList.Add("72 Y", 72);
                            tempList.Add("1y 3m", 1);

                          //  tempList.Add("9 Mo", 1);

                            tempList.Add("1 Y 6 M", 1);


                            tempList.Add("6 Y 10 M", 7);
                            tempList.Add("20 Da", 0);
                        //    tempList.Add("9 Mo", 1);
                          //  tempList.Add("21 M", 2);
                            tempList.Add("22d", 0);
                            tempList.Add("16 Y", 16);

                            tempList.Add("12 M", 1);


                            tempList.Add("6 Mo", 1);

                            tempList.Add("10w", 0);
                            tempList.Add("8d", 0);
                       //     tempList.Add("9 Mo", 1);
                            #endregion



                foreach (string part in parts)
                {
                    string tp = "";



                    try
                    {

                        #region fill in vars

                        if (part.Contains("Age"))
                        {
                            tp = part.Replace("Age", "").Trim();

                            age = tp;



                            if (tempList.ContainsKey(tp))
                            {
                                intAge = tempList[tp];
                            }
                            else
                            {
                                if (!Int32.TryParse(age, out intAge))
                                {
                                    intAge = -1;
                                }

                            }



                        }

                        bool hasProfession = false;

                        if (part.Contains("Profession"))
                        {
                            if (part.Substring(0, 10) == "Profession")
                            {
                                tp = part.Replace("Profession", "").Trim();

                                if (tp != "")
                                    hasProfession = true;
                            }
                        }




                        if (part.Contains("Husband/Father"))
                        {
                            tp = part.Replace("Husband/Father", "").Trim();

                            string[] nameParts = tp.Split(' ');
                            string tpCName = "";
                            string tpSName = "";

                            if (nameParts.Length > 1)
                            {

                                if (nameParts.Length == 1)
                                {
                                    tpCName = nameParts[0];

                                }
                                else
                                {
                                    tpCName = nameParts[0];
                                    tpSName = nameParts[1];
                                }

                            }


                            if (hasProfession)
                            {
                                spouseCName = tpCName;
                                spouseSName = tpSName;
                            }
                            else
                            {
                                if (intAge != -1)
                                {
                                    if (intAge > 20 )
                                    {
                                        spouseCName = tpCName;
                                        spouseSName = tpSName;
                                    }
                                    else
                                    {
                                        fathercname = tpCName;
                                        fathersname = tpSName;
                                    }
                                }
                                else
                                {
                                    fathercname = tpCName;
                                    fathersname = tpSName;
                                }
                            }


                        }

                        if (part.Contains("Wife/Mother"))
                        {
                            tp = part.Replace("Wife/Mother", "").Trim();

                            string[] nameParts = tp.Split(' ');
                            string tpCName = "";
                            string tpSName = "";

                            if (nameParts.Length > 1)
                            {

                                if (nameParts.Length == 1)
                                {
                                    tpCName = nameParts[0];

                                }
                                else
                                {
                                    tpCName = nameParts[0];
                                    tpSName = nameParts[1];
                                }

                            }

                            if (intAge != -1)
                            {
                                if (intAge > 20)
                                {
                                    spouseCName = tpCName;
                                    spouseSName = tpSName;
                                }
                                else
                                {
                                    mothercname = tpCName;
                                    mothersname = tpSName;
                                }
                            }
                            else
                            {
                                mothercname = tpCName;
                                mothersname = tpSName;
                            }



                        }

                        if (part.Contains("PD"))
                        {
                            tp = part.Replace("PD", "").Trim();

                            notes = tp;

                        }





                        if (part.Contains("TNA"))
                        {
                            tp = part.Replace("TNA", "").Trim();

                            if (tp != "")
                            {
                                if (source != "")
                                    source += Environment.NewLine + tp;
                                else
                                    source = tp;
                            }
                        }
                        // fix multi part christian names !
                        if (part.Contains("Full Name"))
                        {
                            tp = part.Replace("Full Name", "").Trim();

                            string[] nameParts = tp.Split(' ');

                            if (nameParts.Length > 1)
                            {
                               
                                if (nameParts.Length > 2)
                                {
                                    cname = nameParts[0] + " " + nameParts[1];
                                    sname = nameParts[2];
                                }
                                else
                                { 
                                    cname = nameParts[0];
                                    sname = nameParts[1];
                                }


                            }
                            else
                            {
                                if (nameParts.Length ==1)
                                {
                                    cname = nameParts[0];
                                }
                            }



                        }



                        if (part.Contains("Date of Baptism"))
                        {
                            tp = part.Replace("Date of Baptism", "").Trim();
                            bapDate = tp;

                        }

                        if (part.Contains("Place of Burial"))
                        {
                            tp = part.Replace("Place of Burial", "").Trim();
                            deathLocation = tp;

                        }

                        if (part.Contains("Date of Burial"))
                        {
                            tp = part.Replace("Date of Burial", "").Trim();
                            deathDate = tp;

                        }

                        if (part.Contains("Date of Death"))
                        {
                            tp = part.Replace("Date of Death", "").Trim();
                            
                            if(deathDate =="")
                                deathDate = tp;

                        }

                        if (part.Contains("Date of Birth"))
                        {
                            tp = part.Replace("Date of Birth", "").Trim();

                            birthDate = tp;

                        }


                        if (part.Contains("Place of Baptism"))
                        {
                            tp = part.Replace("Place of Baptism", "").Trim();

                            deathLocation = tp.Replace("-", "").Trim();


                        }

                        if (part.Contains("Undertaker"))
                        {
                            tp = part.Replace("Undertaker", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Undertaker " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Undertaker " + Environment.NewLine + tp;
                                }
                            }
                        }

                        if (part.Contains("Relation"))
                        {
                            tp = part.Replace("Relation", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Relation " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Relation " + Environment.NewLine + tp;
                                }
                            }
                        }

                        if (part.Contains("Description"))
                        {
                            tp = part.Replace("Description", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Description " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Description " + Environment.NewLine + tp;
                                }
                            }
                        }



                        if (part.Contains("Cause of death"))
                        {
                            tp = part.Replace("Cause of death", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Cause of death " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Cause of death " + Environment.NewLine + tp;
                                }
                            }
                        }

                        if (part.Contains("Grave Number"))
                        {
                            tp = part.Replace("Grave Number", "").Trim();
                            if (tp != "")
                            {

                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Grave Number " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Grave Number " + Environment.NewLine + tp;
                                }
                            }
                        }








                        if (part.Contains("Place of Abode"))
                        {
                            tp = part.Replace("Place of Abode", "").Trim();

                            if (tp == "-") tp = tp.Replace("-", "");

                            if (tp != "")
                            {
                                if (deathLocation != "")
                                {
                                    if (notes == "")
                                        notes += "Place of Abode " + Environment.NewLine + deathLocation;
                                    else
                                        notes += Environment.NewLine + "Place of Abode " + Environment.NewLine + deathLocation;
                                }
                                else
                                {
                                    deathLocation = tp;
                                }
                            }
                        }






                        if (part.Contains("Parish of Abode"))
                        {
                            tp = part.Replace("Parish of Abode", "").Trim();
                            if (tp == "-") tp = tp.Replace("-", "");

                            if (tp != "")
                            {
                                if (deathLocation != "")
                                {
                                    if (notes == "")
                                        notes += "Parish of Abode " + Environment.NewLine + deathLocation;
                                    else
                                        notes += Environment.NewLine + "Parish of Abode " + Environment.NewLine + deathLocation;
                                }
                                else
                                {
                                    deathLocation = tp;
                                }
                            }
                        }
                        if (part.Contains("County of Abode"))
                        {
                            tp = part.Replace("County of Abode", "").Trim();
                            if (tp == "-") tp = tp.Replace("-", "");

                            // if (tp != "")
                            //{
                            //    if (birthCounty != "")
                            //    {
                            //        birthCounty = tp + " " + birthCounty;
                            //    }
                            //    else
                            //    {
                            birthCounty = tp;
                            //      }
                            //  }

                        }

                        if (part.Contains("Registration Date"))
                        {


                            tp = part.Replace("Registration Date", "").Trim();

                            if (birthDate == "")
                            {
                                birthDate = tp;
                            }
                            else
                            {
                                if (tp != "")
                                {
                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += tp;
                                    }
                                }
                            }


                        }
                        if (part.Contains("Registration Town/County"))
                        {


                            tp = part.Replace("Registration Town/County", "").Trim();

                            if (deathLocation == "")
                            {


                                tp = tp.Replace(",", "");

                                if (tp.ToLower().Contains("yorkshire"))
                                {
                                    birthCounty = "Yorkshire";
                                    deathLocation = tp.Replace("Yorkshire", "");
                                }
                                else
                                {
                                    deathLocation = tp;
                                }



                            }
                            else
                            {
                                if (tp != "")
                                {

                                    if (tp.ToLower().Contains("Yorkshire"))
                                    {
                                        birthCounty = "Yorkshire";
                                    }


                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += tp;
                                    }
                                }
                            }

                        }



                        if (part.Contains("Port of Arrival"))
                        {
                            tp = part.Replace("Port of Arrival", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Port of Arrival: " + tp;
                                }
                                else
                                {
                                    notes += "Port of Arrival: " + tp;
                                }
                            }

                        }

                        if (part.Contains("Onboard"))
                        {
                            tp = part.Replace("Onboard", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Onboard: " + tp;
                                }
                                else
                                {
                                    notes += "Onboard: " + tp;
                                }
                            }

                        }








                        if (part.Contains("Ceremony Performed by"))
                        {
                            tp = part.Replace("Ceremony Performed by", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += tp;
                                }
                            }

                        }




                        if (part.Contains("Godparents"))
                        {
                            tp = part.Replace("Godparents", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Godparents " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Godparents " + Environment.NewLine + tp;
                                }
                            }

                        }
                        if (part.Contains("Godfather"))
                        {
                            tp = part.Replace("Godfather", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Godfather " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Godfather " + Environment.NewLine + tp;
                                }
                            }
                        }

                        if (part.Contains("Godmother"))
                        {
                            tp = part.Replace("Godmother", "").Trim();
                            if (tp != "")
                            {

                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Godmother " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Godmother " + Environment.NewLine + tp;
                                }
                            }
                        }



                        /////////////////////////////////////////////////////////////////



                        if (part.Contains("Parents"))
                        {


                            if (part.Substring(0, 7) == "Parents")
                            {
                                tp = part.Replace("Parents", "").Trim();


                                if (tp != "")
                                {
                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + "Parents " + Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += "Parents " + Environment.NewLine + tp;
                                    }
                                }
                            }


                        }

                        if (part.Contains("Father") && !part.Contains("Father's Profession"))
                        {

                            tp = part.Substring(0, 6);

                            if (tp == "Father")
                            {


                                tp = part.Replace("Father", "").Trim();


                                if (tp != "")
                                {
                                    string[] fatherParts = tp.Split(' ');

                                    if (fatherParts.Length > 0)
                                    {
                                        if (fatherParts.Length == 1)
                                        {
                                            fathercname = fatherParts[0];
                                            fathersname = sname;
                                        }
                                        else
                                        {
                                            if (fatherParts.Length > 2)
                                            {
                                                fathercname = fatherParts[0] + " " + fatherParts[1];
                                                fathersname = fatherParts[2];
                                            }
                                            else
                                            {
                                                fathercname = fatherParts[0];
                                                fathersname = fatherParts[1];
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (part.Contains("Father's Profession"))
                        {
                            tp = part.Replace("Father's Profession", "").Trim();

                            if (tp != "")
                            {
                                fatherOccupation = tp;
                            }
                        }

                        if (part.Contains("Mother") && !part.Contains("Mother's Maiden Name") && !part.Contains("Mother's Parish"))
                        {

                            tp = part.Substring(0, 6);

                            if (tp == "Mother")
                            {
                                tp = part.Replace("Mother", "").Trim();



                                if (tp != "")
                                {
                                    string[] motherParts = tp.Split(' ');

                                    if (motherParts.Length > 0)
                                    {
                                        if (motherParts.Length == 1)
                                        {
                                            mothercname = motherParts[0];
                                            mothersname = "";
                                        }
                                        else
                                        {
                                            mothercname = motherParts[0];
                                            mothersname = motherParts[1];
                                        }
                                    }
                                }
                            }

                        }
                        if (part.Contains("Mother's Maiden Name"))
                        {
                            tp = part.Replace("Mother's Maiden Name", "").Trim();

                            if (tp != "")
                            {
                                mothersname = tp;
                            }
                        }

                        if (part.Contains("Mother's Parish"))
                        {
                            tp = part.Replace("Mother's Parish", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Mother Parish" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Mother Parish" + Environment.NewLine + tp;
                                }
                            }
                        }


                        /////////////////////////////////////////////////////////////////



                        if (part.Contains("Date of Marriage"))
                        {
                            tp = part.Replace("Date of Marriage", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Date of Marriage" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Date of Marriage" + Environment.NewLine + tp;
                                }
                            }

                        }
                        if (part.Contains("Place of Marriage"))
                        {
                            tp = part.Replace("Place of Marriage", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Place of Marriage" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Place of Marriage" + Environment.NewLine + tp;
                                }
                            }

                        }
                        if (part.Contains("Maternal Parents"))
                        {
                            tp = part.Replace("Maternal Parents", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Maternal Parents" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Maternal Parents" + Environment.NewLine + tp;
                                }
                            }
                        }

                        if (part.Contains("Name(s)"))
                        {
                            tp = part.Replace("Name(s)", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Name(s)" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Name(s)" + Environment.NewLine + tp;
                                }
                            }
                        }


                        if (part.Contains("Profession"))
                        {
                            if (part.Substring(0, 10) == "Profession")
                            {
                                tp = part.Replace("Profession", "").Trim();


                                if (tp != "")
                                {
                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + "Profession" + Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += "Profession" + Environment.NewLine + tp;
                                    }
                                }
                            }
                        }

                        if (part.Contains("Pedigree Chart"))
                        {
                            tp = part.Replace("Pedigree Chart", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Pedigree Chart" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Pedigree Chart" + Environment.NewLine + tp;
                                }
                            }
                        }




                        /////////////////////////////////////////////////////////////////



                        //if (part.Contains("|Date of Marriage"))
                        //{
                        //    tp = part.Replace("|Date of Marriage", "");


                        //}
                        //if (part.Contains("|Place of Marriage"))
                        //{
                        //    tp = part.Replace("|Place of Marriage", "");


                        //}
                        //if (part.Contains("|Maternal Parents"))
                        //{
                        //    tp = part.Replace("|Maternal Parents", "");


                        //}
                        //if (part.Contains("|Name(s)"))
                        //{
                        //    tp = part.Replace("|PD", "");


                        //}


                        //if (part.Contains("|Profession"))
                        //{
                        //    tp = part.Replace("|Profession", "");


                        //}

                        //if (part.Contains("|Pedigree Chart"))
                        //{
                        //    tp = part.Replace("|Pedigree Chart", "");


                        //}



                        /////////////////////////////////////////////////////////////////



                        if (part.Contains("Grandparent(s)"))
                        {
                            tp = part.Replace("Grandparent(s)", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Grandparent(s)" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Grandparent(s)" + Environment.NewLine + tp;
                                }
                            }

                        }

                        if (part.Contains("Grandparent(s)"))
                        {
                            tp = part.Replace("Grandparent(s)", "").Trim(); if (tp != "")

                                if (tp != "")
                                {
                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + "Grandparent(s)" + Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += "Grandparent(s)" + Environment.NewLine + tp;
                                    }
                                }
                        }



                        #endregion

                    }
                    catch (Exception ex1)
                    {
                        Debug.WriteLine(ex1.Message);
                    }


                }






                Guid personId = Guid.Empty;

                Guid newSourceId = parishId;

                SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();


                int birthYear = getDateYear(birthDate);

                int deathYear = getDateYear(deathDate);

                int bapYear = getDateYear(bapDate);

                int deathEstYear = 0;
                int birthEstYear = 0;

                //if (fathercname != "")
                //{

                //    if (birthYear > 0)
                //    {
                //        deathEstYear = birthYear + 50;
                //        birthEstYear = birthYear;
                //    }

                //    if (bapYear > 0)
                //    {
                //        deathEstYear = bapYear + 50;
                //        birthEstYear = bapYear;
                //    }


                //}


                bool isEstBirth = false;
                bool isEstDeath = false;


                if (birthYear > 0)
                {
                    deathEstYear = birthYear + 50;
                    birthEstYear = birthYear;

                    if (bapYear == 0)
                        bapYear = birthYear;
                }

                if (bapYear > 0)
                {
                    deathEstYear = bapYear + 50;
                    birthEstYear = bapYear;


                    if (birthYear == 0)
                        birthYear = bapYear;
                }

                if (deathYear > 0)
                {
                    deathEstYear = deathYear;


                    if (birthYear == 0 && bapYear == 0)
                    {
                        if (intAge >= 0)
                        {
                            birthYear = deathYear - intAge;
                            bapYear = deathYear - intAge;
                            birthEstYear = deathYear - intAge;
                        }
                        else
                        {
                            if (fathercname != "")
                            {
                                birthEstYear = deathYear - 10;
                             
                            }
                            else
                            {
                                birthEstYear = deathYear - 50;
                                 
                            }
                            isEstBirth = true;
                        }
                    }

                }


                if (mothercname == fathercname)
                    mothercname = "";
                if (mothersname == fathersname)
                    mothersname = "";


              //  string uniqueRef = Guid.NewGuid().ToString();


                if (birthYear == 0 || bapYear == 0)
                    Debug.WriteLine("birthYear bapYear");


                if (birthlocation.Trim() == "")
                    Debug.WriteLine("birthlocation");


                if (cname.Trim() == "")
                    Debug.WriteLine("cname");

                if (sname.Trim() == "")
                    Debug.WriteLine("sname");


                if (notes.Contains("Yorkshire")) deathCounty = "Yorkshire";
                if (notes.Contains("Middlesex")) deathCounty = "Greater London";
                if (notes.Contains("Derbyshire")) deathCounty = "Derbyshire";
                if (notes.Contains("Staffordshire")) deathCounty = "Staffordshire";
                if (notes.Contains("London")) deathCounty = "Greater London";



                try
                {

                  //  Debug.WriteLine(cname + "" + sname + "" + birthlocation + "" + birthDate + "" + bapDate + "" + deathDate + "" + deathLocation + "" + fathercname + "" + fathersname + "" + mothercname  + "" + mothersname + "" + source + "" + notes + );
                    personId = deathsBirthsBll.InsertDeathBirthRecord2(true, cname, sname, birthlocation, birthDate

                        , bapDate, deathDate, deathLocation, fathercname, fathersname, mothercname

                        , mothersname, source, notes, birthYear, bapYear, deathYear, birthCounty, deathCounty, ""

                        , "", "", 0, spouseCName, spouseSName, fatherOccupation,

                        Guid.Empty, 1, Guid.Empty, Guid.Empty, 1, 1, Guid.NewGuid(), birthEstYear, deathEstYear, isEstBirth, isEstDeath, "", "", "");

                }
                catch (Exception ex2)
                {
                    Debug.WriteLine(ex2.Message);
                }



                try
                {
                    sourceMappingBll.Insert(newSourceId, null, null, 1, personId, DateTime.Today.ToShortDateString(), null);

                }
                catch (Exception ex3)
                {
                    Debug.WriteLine(ex3.Message);
                }


                idx++;
            }





        }





        public void ImportNonComformistBirths(string path, Guid parishId)
        {

            DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();



            string lineList = System.IO.File.ReadAllText(path);


            string[] lines = Regex.Split(lineList, "Birth of");


          //  Birth of ? Thackeray

 
            //|Gender
            //|Onboard Star Of Peace
            //|Nationality
            //|Port of ArrivalAberdeen

  
            //|Ceremony Performed by
      
            //|Father Rev Richard Thackeray
      
      
            //|Town & County
  

//Birth of Benjamin Thackera
            //|PD Register of Births for the Monthly Meeting of Gildersome, Yorkshire from 1710 to 1748, with a Register of Marriages from 1717 to 1746, and a Register of Burials from 1710 to 1749
            //|TNA ReferenceRG6 / Piece 1175 / Folio 0|[Chapel/Registry]
            //|Full NameBenjamin Thackera
            //|Date of Birth 15 August 1664
            //|Place of Birth
            //|Place of Abode
            //|Parish of AbodeMorley
            //|County of Abode
            //|Registration Date
            //|Registration Town/CountyGildersome, Yorkshire
            //|Ceremony Performed by
            //|Parents
            //|Father
            //|Father's Profession
            //|Mother
            //|Mother's Maiden Name
            //|Mother's Parish
            //|Date of Marriage
            //|Place of Marriage
            //|Maternal Parents
            //|Name(s)
            //|Profession
            //|Town & County
            //|Paternal Parents
            //|Name(s)
            //|Profession
            //|Pedigree Chart
            //|Grandparent(s)
            //|Grandparent(s)|






            int idx = 0;

            foreach (string line in lines)
            {

                if (line.Trim() == "") continue;

                string[] parts = line.Split('|');

                string cname = "";
                string sname = "";
                string fathercname = "";
                string fathersname = "";
                string mothercname = "";
                string mothersname = "";
                string birthlocation = "";
                string birthCounty = "";
                string birthDate = "";
                string bapDate = "";
                string deathLocation = "";
                string deathCounty = "";
                string deathDate = "";

                string notes = "";
                string source = "";
                string fatherOccupation = "";

                string spouseCName = "";
                string spouseSName = "";



                foreach (string part in parts)
                {
                    string tp = "";



                    try
                    {

                        #region fill in vars

                        if (part.Contains("PD"))
                        {
                            tp = part.Replace("PD", "").Trim();

                            notes = tp;

                        }
                        if (part.Contains("TNA"))
                        {
                            tp = part.Replace("TNA", "").Trim();

                            if (tp != "")
                            {
                                if (source != "")
                                    source += Environment.NewLine + tp;
                                else
                                    source = tp;
                            }
                        }

                        if (part.Contains("Full Name"))
                        {
                            tp = part.Replace("Full Name", "").Trim();

                            string[] nameParts = tp.Split(' ');

                            if (nameParts.Length > 1)
                            {
                                cname = nameParts[0];
                                sname = nameParts[1];
                            }

                        }
                        if (part.Contains("Date of Baptism"))
                        {
                            tp = part.Replace("Date of Baptism", "").Trim();
                            bapDate = tp;

                        }



                        if (part.Contains("Date of Birth"))
                        {
                            tp = part.Replace("Date of Birth", "").Trim();

                            birthDate = tp;

                        }


                        if (part.Contains("Place of Baptism"))
                        {
                            tp = part.Replace("Place of Baptism", "").Trim();

                            birthlocation = tp.Replace("-", "").Trim();


                        }


                        if (part.Contains("Place of Abode"))
                        {
                            tp = part.Replace("Place of Abode", "").Trim();

                            if (tp == "-") tp = tp.Replace("-", "");

                            if (tp != "")
                            {
                                if (birthlocation != "")
                                {
                                    if (notes == "")
                                        notes += "Place of Abode " + Environment.NewLine + birthlocation;
                                    else
                                        notes += Environment.NewLine + "Place of Abode " + Environment.NewLine + birthlocation;
                                }
                                else
                                {
                                    birthlocation = tp;
                                }
                            }
                        }

                        if (part.Contains("Parish of Abode"))
                        {
                            tp = part.Replace("Parish of Abode", "").Trim();
                            if (tp == "-") tp = tp.Replace("-", "");

                            if (tp != "")
                            {
                                if (birthlocation != "")
                                {
                                    if (notes == "")
                                        notes += "Parish of Abode " + Environment.NewLine + birthlocation;
                                    else
                                        notes += Environment.NewLine + "Parish of Abode " + Environment.NewLine + birthlocation;
                                }
                                else
                                {
                                    birthlocation = tp;
                                }
                            }
                        }
                        if (part.Contains("County of Abode"))
                        {
                            tp = part.Replace("County of Abode", "").Trim();
                            if (tp == "-") tp = tp.Replace("-", "");

                            // if (tp != "")
                            //{
                            //    if (birthCounty != "")
                            //    {
                            //        birthCounty = tp + " " + birthCounty;
                            //    }
                            //    else
                            //    {
                            birthCounty = tp;
                            //      }
                            //  }

                        }

                        if (part.Contains("Registration Date"))
                        {


                            tp = part.Replace("Registration Date", "").Trim();

                            if (birthDate == "")
                            {
                                birthDate = tp;
                            }
                            else
                            {
                                if (tp != "")
                                {
                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += tp;
                                    }
                                }
                            }


                        }
                        if (part.Contains("Registration Town/County"))
                        {


                            tp = part.Replace("Registration Town/County", "").Trim();

                            if (birthlocation == "")
                            {


                                tp = tp.Replace(",", "");

                                if (tp.ToLower().Contains("yorkshire"))
                                {
                                    birthCounty = "Yorkshire";
                                    birthlocation = tp.Replace("Yorkshire", "");
                                }
                                else
                                {
                                    birthlocation = tp;
                                }



                            }
                            else
                            {
                                if (tp != "")
                                {

                                    if (tp.ToLower().Contains("Yorkshire"))
                                    {
                                        birthCounty = "Yorkshire";
                                    }


                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += tp;
                                    }
                                }
                            }

                        }



                        if (part.Contains("Port of Arrival"))
                        {
                            tp = part.Replace("Port of Arrival", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Port of Arrival: " + tp;
                                }
                                else
                                {
                                    notes += "Port of Arrival: " + tp;
                                }
                            }

                        }

                        if (part.Contains("Onboard"))
                        {
                            tp = part.Replace("Onboard", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Onboard: " + tp;
                                }
                                else
                                {
                                    notes += "Onboard: " + tp;
                                }
                            }

                        }








                        if (part.Contains("Ceremony Performed by"))
                        {
                            tp = part.Replace("Ceremony Performed by", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += tp;
                                }
                            }

                        }




                        if (part.Contains("Godparents"))
                        {
                            tp = part.Replace("Godparents", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Godparents " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Godparents " + Environment.NewLine + tp;
                                }
                            }

                        }
                        if (part.Contains("Godfather"))
                        {
                            tp = part.Replace("Godfather", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Godfather " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Godfather " + Environment.NewLine + tp;
                                }
                            }
                        }

                        if (part.Contains("Godmother"))
                        {
                            tp = part.Replace("Godmother", "").Trim();
                            if (tp != "")
                            {

                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Godmother " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Godmother " + Environment.NewLine + tp;
                                }
                            }
                        }



                        /////////////////////////////////////////////////////////////////



                        if (part.Contains("Parents"))
                        {


                            if (part.Substring(0, 7) == "Parents")
                            {
                                tp = part.Replace("Parents", "").Trim();


                                if (tp != "")
                                {
                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + "Parents " + Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += "Parents " + Environment.NewLine + tp;
                                    }
                                }
                            }


                        }

                        if (part.Contains("Father") && !part.Contains("Father's Profession"))
                        {
                            tp = part.Replace("Father", "").Trim();


                            if (tp != "")
                            {
                                string[] fatherParts = tp.Split(' ');

                                if (fatherParts.Length > 0)
                                {
                                    if (fatherParts.Length == 1)
                                    {
                                        fathercname = fatherParts[0];
                                        fathersname = sname;
                                    }
                                    else
                                    {
                                        if (fatherParts.Length > 2)
                                        {
                                            fathercname = fatherParts[0] + " " + fatherParts[1];
                                            fathersname = fatherParts[2];
                                        }
                                        else
                                        {
                                            fathercname = fatherParts[0];
                                            fathersname = fatherParts[1];
                                        }
                                    }
                                }
                            }

                        }

                        if (part.Contains("Father's Profession"))
                        {
                            tp = part.Replace("Father's Profession", "").Trim();

                            if (tp != "")
                            {
                                fatherOccupation = tp;
                            }
                        }

                        if (part.Contains("Mother") && !part.Contains("Mother's Maiden Name") && !part.Contains("Mother's Parish"))
                        {

                            tp = part.Substring(0, 6);

                            if (tp == "Mother")
                            {
                                tp = part.Replace("Mother", "").Trim();



                                if (tp != "")
                                {
                                    string[] motherParts = tp.Split(' ');

                                    if (motherParts.Length > 0)
                                    {
                                        if (motherParts.Length == 1)
                                        {
                                            mothercname = motherParts[0];
                                            mothersname = "";
                                        }
                                        else
                                        {
                                            mothercname = motherParts[0];
                                            mothersname = motherParts[1];
                                        }
                                    }
                                }
                            }

                        }
                        if (part.Contains("Mother's Maiden Name"))
                        {
                            tp = part.Replace("Mother's Maiden Name", "").Trim();

                            if (tp != "")
                            {
                                mothersname = tp;
                            }
                        }

                        if (part.Contains("Mother's Parish"))
                        {
                            tp = part.Replace("Mother's Parish", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Mother Parish" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Mother Parish" + Environment.NewLine + tp;
                                }
                            }
                        }


                        /////////////////////////////////////////////////////////////////



                        if (part.Contains("Date of Marriage"))
                        {
                            tp = part.Replace("Date of Marriage", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Date of Marriage" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Date of Marriage" + Environment.NewLine + tp;
                                }
                            }

                        }
                        if (part.Contains("Place of Marriage"))
                        {
                            tp = part.Replace("Place of Marriage", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Place of Marriage" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Place of Marriage" + Environment.NewLine + tp;
                                }
                            }

                        }
                        if (part.Contains("Maternal Parents"))
                        {
                            tp = part.Replace("Maternal Parents", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Maternal Parents" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Maternal Parents" + Environment.NewLine + tp;
                                }
                            }
                        }

                        if (part.Contains("Name(s)"))
                        {
                            tp = part.Replace("Name(s)", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Name(s)" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Name(s)" + Environment.NewLine + tp;
                                }
                            }
                        }


                        if (part.Contains("Profession"))
                        {
                            if (part.Substring(0, 10) == "Profession")
                            {
                                tp = part.Replace("Profession", "").Trim();


                                if (tp != "")
                                {
                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + "Profession" + Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += "Profession" + Environment.NewLine + tp;
                                    }
                                }
                            }
                        }

                        if (part.Contains("Pedigree Chart"))
                        {
                            tp = part.Replace("Pedigree Chart", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Pedigree Chart" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Pedigree Chart" + Environment.NewLine + tp;
                                }
                            }
                        }




                        /////////////////////////////////////////////////////////////////



                        //if (part.Contains("|Date of Marriage"))
                        //{
                        //    tp = part.Replace("|Date of Marriage", "");


                        //}
                        //if (part.Contains("|Place of Marriage"))
                        //{
                        //    tp = part.Replace("|Place of Marriage", "");


                        //}
                        //if (part.Contains("|Maternal Parents"))
                        //{
                        //    tp = part.Replace("|Maternal Parents", "");


                        //}
                        //if (part.Contains("|Name(s)"))
                        //{
                        //    tp = part.Replace("|PD", "");


                        //}


                        //if (part.Contains("|Profession"))
                        //{
                        //    tp = part.Replace("|Profession", "");


                        //}

                        //if (part.Contains("|Pedigree Chart"))
                        //{
                        //    tp = part.Replace("|Pedigree Chart", "");


                        //}



                        /////////////////////////////////////////////////////////////////



                        if (part.Contains("Grandparent(s)"))
                        {
                            tp = part.Replace("Grandparent(s)", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Grandparent(s)" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Grandparent(s)" + Environment.NewLine + tp;
                                }
                            }

                        }

                        if (part.Contains("Grandparent(s)"))
                        {
                            tp = part.Replace("Grandparent(s)", "").Trim(); if (tp != "")

                                if (tp != "")
                                {
                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + "Grandparent(s)" + Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += "Grandparent(s)" + Environment.NewLine + tp;
                                    }
                                }
                        }



                        #endregion

                    }
                    catch (Exception ex1)
                    {
                        Debug.WriteLine(ex1.Message);
                    }


                }






                Guid personId = Guid.Empty;

                Guid newSourceId = parishId;

                SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();


                int birthYear = getDateYear(birthDate);

                int deathYear = 0;

                int bapYear = getDateYear(bapDate);

                int deathEstYear = 0;
                int birthEstYear = 0;

                //if (fathercname != "")
                //{

                //    if (birthYear > 0)
                //    {
                //        deathEstYear = birthYear + 50;
                //        birthEstYear = birthYear;
                //    }

                //    if (bapYear > 0)
                //    {
                //        deathEstYear = bapYear + 50;
                //        birthEstYear = bapYear;
                //    }


                //}

                if (birthYear > 0)
                {
                    deathEstYear = birthYear + 50;
                    birthEstYear = birthYear;

                    if (bapYear == 0)
                        bapYear = birthYear;
                }

                if (bapYear > 0)
                {
                    deathEstYear = bapYear + 50;
                    birthEstYear = bapYear;


                    if (birthYear == 0)
                        birthYear = bapYear;
                }




                if (mothercname == fathercname)
                    mothercname = "";
                if (mothersname == fathersname)
                    mothersname = "";


             //   string uniqueRef = Guid.NewGuid().ToString();


                if (birthYear == 0 || bapYear == 0)
                    Debug.WriteLine("poo");


                if (birthlocation.Trim() == "")
                    Debug.WriteLine("poo");


                if (cname.Trim() == "")
                    Debug.WriteLine("poo");

                if (sname.Trim() == "")
                    Debug.WriteLine("poo");


                if (notes.Contains("Yorkshire")) birthCounty = "Yorkshire";
                if (notes.Contains("Middlesex")) birthCounty = "Greater London";
                if (notes.Contains("Derbyshire")) birthCounty = "Derbyshire";
                if (notes.Contains("Staffordshire")) birthCounty = "Staffordshire";
                if (notes.Contains("London")) birthCounty = "Greater London";



                try
                {
                    personId = deathsBirthsBll.InsertDeathBirthRecord2(true, cname, sname, birthlocation, birthDate

                        , bapDate, deathDate, deathLocation, fathercname, fathersname, mothercname

                        , mothersname, source, notes, birthYear, bapYear, deathYear, birthCounty, deathCounty, ""

                        , "", "", 0, spouseCName, spouseSName, fatherOccupation,

                        Guid.Empty, 1, Guid.Empty, Guid.Empty, 1, 1, Guid.NewGuid(), birthEstYear, deathEstYear, false, true, "", "", "");

                }
                catch (Exception ex2)
                {
                    Debug.WriteLine(ex2.Message);
                }



                try
                {
                    sourceMappingBll.Insert(newSourceId, null, null, 1, personId, DateTime.Today.ToShortDateString(), null);

                }
                catch (Exception ex3)
                {
                    Debug.WriteLine(ex3.Message);
                }


                idx++;
            }





        }


        /// <summary>
        /// Witnesses arent being imported here
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parishId"></param>
        public void ImportNonComMarriage(string path, Guid parishId)
        {
         


            MarriagesBLL marriagesBLL = new MarriagesBLL();



         //   string[] allLines = ;

            List<string> _allLines = new List<string>(System.IO.File.ReadAllLines(path));

            _allLines.RemoveAt(0);

            var query = from line in _allLines

                        let data = line.Split(',')

                        select new

                        {

                            MaleId = Guid.Empty,

                            MaleCName = data[0],

                            MaleSName = data[1],


                            FemaleCName = data[2],

                            FemaleSName = data[3],

                            MaleInfo = data[4],


                            Source = data[5],

                            MaleOccupation = data[6],

                            Date = data[7],

                            YearIntVal = getDateYear(data[7]),

                            MarriageLocation = data[9],

                            Witness1 = data[10],


                            MaleLocation = data[11],

                            FemaleLocation = data[12],

                            Witness2 = data[13],

                            Witness3 = data[14],

                            Witness4 = data[15],

                            FemaleInfo = data[16],






                            MarriageCounty = "",

                            FemaleOccupation = "",


 


                            FemaleId = Guid.Empty,

                            DateAdded = DateTime.Today,

                            DateLastEdit = DateTime.Today,

                            UserId = 1,

                            OrigMaleSurname = "",

                            OrigFemaleSurname = "",



                            FemaleIsKnownWidow = false,

                            MaleIsKnownWidower = false,

                            IsBanns = false,



                            IsLicence =  false,

                            MarriageLocationId = Guid.Empty,

                            MaleLocationId = Guid.Empty,

                            FemaleLocationId = Guid.Empty,

                            MaleBirthYear = 0,

                            FemaleBirthYear =0

                        };





            //foreach (var team in query)
            //{



            //    //Debug.WriteLine(team.ToString());

            //    Guid personId = Guid.Empty;

            //    Guid newSourceId = parishId;

            //    SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();



            //    sourceMappingBll.Insert(newSourceId, null, null, 1, personId, DateTime.Today.ToShortDateString(), null);





            //}



            foreach (var team in query)
            {

                    Guid personId = Guid.Empty;

                    personId = marriagesBLL.InsertMarriage2(team.Date, team.FemaleCName, team.FemaleId, team.FemaleInfo, team.FemaleLocation, team.FemaleSName,

                    team.MaleCName, team.MaleId, team.MaleInfo, team.MaleLocation, team.MaleSName, team.MarriageCounty, team.MarriageLocation,

                    team.Source, team.YearIntVal, team.MaleOccupation,

                    team.FemaleOccupation, team.IsLicence, team.IsBanns, team.FemaleIsKnownWidow, team.MaleIsKnownWidower, team.UserId, team.MarriageLocationId, team.MaleLocationId, team.FemaleLocationId, team.MaleBirthYear, team.FemaleBirthYear,Guid.Empty,0,0,"","");


                    // team.Witness1, team.Witness2, team.Witness3, team.Witness4,

                //    Guid newSourceId = parishId;

                    SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();



                    sourceMappingBll.Insert(parishId, null, personId, 1, null, DateTime.Today.ToShortDateString(), null);


            }




        
        }




        public void ImportNonComformistBaps(string path, Guid parishId)
        {

            DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();



            string lineList = System.IO.File.ReadAllText(path);


            string[] lines = Regex.Split(lineList, "Baptism of");


           //  Benjamin Thackeray
            //|PD Register of Births and Baptisms at Queen Street Independent Chapel (formerly Whitehall Chapel) in Leeds, Yorkshire from 1756 to 1791
            //|TNA ReferenceRG4 / Piece 3399 / Folio 7|[Chapel/Registry]
            //|Full NameBenjamin Thackeray
            //|Date of Baptism 13 June 1762
            //|Place of Baptism
            //|Date of Birth19 May 1762
            //|Place of Abode-
            //|Parish of Abode
            //|County of Abode
            //|Registration Date
            //|Registration Town/CountyLeeds, Yorkshire
            //|Ceremony Performed by
            //|Godparents
            //|Godfather
            //|Godmother
            //|Parents
            //|Father John Thackeray
            //|Father's Profession
            //|Mother
            //|Mother's Maiden Name
            //|Mother's Parish
            //|Date of Marriage
            //|Place of Marriage
            //|Maternal Parents
            //|Name(s)
            //|Profession
            //|Town & County
            //|Paternal Parents
            //|Name(s)
            //|Profession
            //|Pedigree Chart
            //|Grandparent(s)
            //|Grandparent(s)
            //|Father
            //|John Thackeray Mother
            //|Benjamin Thackeray
            //|Baptism 13 June 1762
            //||||

        




            int idx = 0;

            foreach (string line in lines)
            {

                if (line.Trim() == "") continue;

                string[] parts = line.Split('|');

                string cname = "";
                string sname = "";
                string fathercname = "";
                string fathersname = "";
                string mothercname = "";
                string mothersname = "";
                string birthlocation = "";
                string birthCounty = "";
                string birthDate = "";
                string bapDate = "";
                string deathLocation = "";
                string deathCounty = "";
                string deathDate = "";

                string notes = "";
                string source = "";
                string fatherOccupation = "";

                string spouseCName = "";
                string spouseSName = "";

               
  
                    foreach (string part in parts)
                    {
                         string tp = "";

                        
                         
                        try
                         {

                        #region fill in vars

                        if (part.Contains("PD"))
                        {
                            tp = part.Replace("PD", "").Trim();

                            notes = tp;

                        }
                        if (part.Contains("TNA"))
                        {
                            tp = part.Replace("TNA", "").Trim();

                            if (tp != "")
                            {
                                if (source != "")
                                    source += Environment.NewLine + tp;
                                else
                                    source = tp;
                            }
                        }

                        if (part.Contains("Full Name"))
                        {
                            tp = part.Replace("Full Name", "").Trim();

                            string[] nameParts = tp.Split(' ');

                            if (nameParts.Length > 1)
                            {
                                cname = nameParts[0];
                                sname = nameParts[1];
                            }

                        }
                        if (part.Contains("Date of Baptism"))
                        {
                            tp = part.Replace("Date of Baptism", "").Trim();
                            bapDate = tp;

                        }



                        if (part.Contains("Date of Birth"))
                        {
                            tp = part.Replace("Date of Birth", "").Trim();

                            birthDate = tp;

                        }


                        if (part.Contains("Place of Baptism"))
                        {
                            tp = part.Replace("Place of Baptism", "").Trim();

                            birthlocation = tp.Replace("-", "").Trim() ;


                        }


                        if (part.Contains("Place of Abode"))
                        {
                            tp = part.Replace("Place of Abode", "").Trim();

                            if (tp == "-") tp = tp.Replace("-", "");

                            if (tp != "")
                            {
                                if (birthlocation != "")
                                {
                                    if (notes == "")
                                        notes += "Place of Abode " + Environment.NewLine + birthlocation;
                                    else
                                        notes += Environment.NewLine + "Place of Abode " + Environment.NewLine + birthlocation;
                                }
                                else
                                {
                                    birthlocation = tp;
                                }
                            }
                        }

                        if (part.Contains("Parish of Abode"))
                        {
                            tp = part.Replace("Parish of Abode", "").Trim();
                            if (tp == "-") tp = tp.Replace("-", "");

                            if (tp != "")
                            {
                                if (birthlocation != "")
                                {
                                    if (notes == "")
                                        notes += "Parish of Abode " + Environment.NewLine + birthlocation;
                                    else
                                        notes += Environment.NewLine + "Parish of Abode " + Environment.NewLine + birthlocation;
                                }
                                else
                                {
                                    birthlocation = tp;
                                }
                            }
                        }
                        if (part.Contains("County of Abode"))
                        {
                            tp = part.Replace("County of Abode", "").Trim();
                            if (tp == "-") tp = tp.Replace("-", "");

                            // if (tp != "")
                            //{
                            //    if (birthCounty != "")
                            //    {
                            //        birthCounty = tp + " " + birthCounty;
                            //    }
                            //    else
                            //    {
                            birthCounty = tp;
                            //      }
                            //  }

                        }

                        if (part.Contains("Registration Date"))
                        {


                            tp = part.Replace("Registration Date", "").Trim();

                            if (birthDate == "")
                            {
                                birthDate = tp;
                            }
                            else
                            {
                                if (tp != "")
                                {
                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += tp;
                                    }
                                }
                            }


                        }
                        if (part.Contains("Registration Town/County"))
                        {


                            tp = part.Replace("Registration Town/County", "").Trim();

                            if (birthlocation == "")
                            {


                                tp = tp.Replace(",", "");

                                if (tp.ToLower().Contains("yorkshire"))
                                {
                                    birthCounty = "Yorkshire";
                                    birthlocation = tp.Replace("Yorkshire", "");
                                }
                                else
                                { 
                                    birthlocation = tp;
                                }
                                


                            }
                            else
                            {
                                if (tp != "")
                                {

                                    if (tp.ToLower().Contains("Yorkshire"))
                                    {
                                        birthCounty = "Yorkshire";
                                    }


                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += tp;
                                    }
                                }
                            }

                        }
                        if (part.Contains("Ceremony Performed by"))
                        {
                            tp = part.Replace("Ceremony Performed by", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += tp;
                                }
                            }

                        }
                        if (part.Contains("Godparents"))
                        {
                            tp = part.Replace("Godparents", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Godparents " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Godparents " + Environment.NewLine + tp;
                                }
                            }

                        }
                        if (part.Contains("Godfather"))
                        {
                            tp = part.Replace("Godfather", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Godfather " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Godfather " + Environment.NewLine + tp;
                                }
                            }
                        }

                        if (part.Contains("Godmother"))
                        {
                            tp = part.Replace("Godmother", "").Trim();
                            if (tp != "")
                            {

                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Godmother " + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Godmother " + Environment.NewLine + tp;
                                }
                            }
                        }



                        /////////////////////////////////////////////////////////////////



                        if (part.Contains("Parents"))
                        {


                            if (part.Substring(0, 7) == "Parents")
                            {
                                tp = part.Replace("Parents", "").Trim();


                                if (tp != "")
                                {
                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + "Parents " + Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += "Parents " + Environment.NewLine + tp;
                                    }
                                }
                            }


                        }

                        if (part.Contains("Father") && !part.Contains("Father's Profession"))
                        {
                            tp = part.Replace("Father", "").Trim();


                            if (tp != "")
                            {
                                string[] fatherParts = tp.Split(' ');

                                if (fatherParts.Length > 0)
                                {
                                    if (fatherParts.Length == 1)
                                    {
                                        fathercname = fatherParts[0];
                                        fathersname = sname;
                                    }
                                    else
                                    {
                                        fathercname = fatherParts[0];
                                        fathersname = fatherParts[1];
                                    }
                                }
                            }

                        }

                        if (part.Contains("Father's Profession"))
                        {
                            tp = part.Replace("Father's Profession", "").Trim();

                            if (tp != "")
                            {
                                fatherOccupation = tp;
                            }
                        }

                        if (part.Contains("Mother") && !part.Contains("Mother's Maiden Name") && !part.Contains("Mother's Parish"))
                        {

                            tp = part.Substring(0, 6);

                            if (tp == "Mother")
                            {
                                tp = part.Replace("Mother", "").Trim();



                                if (tp != "")
                                {
                                    string[] motherParts = tp.Split(' ');

                                    if (motherParts.Length > 0)
                                    {
                                        if (motherParts.Length == 1)
                                        {
                                            mothercname = motherParts[0];
                                            mothersname = "";
                                        }
                                        else
                                        {
                                            mothercname = motherParts[0];
                                            mothersname = motherParts[1];
                                        }
                                    }
                                }
                            }

                        }
                        if (part.Contains("Mother's Maiden Name"))
                        {
                            tp = part.Replace("Mother's Maiden Name", "").Trim();

                            if (tp != "")
                            {
                                mothersname = tp;
                            }
                        }

                        if (part.Contains("Mother's Parish"))
                        {
                            tp = part.Replace("Mother's Parish", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Mother Parish" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Mother Parish" + Environment.NewLine + tp;
                                }
                            }
                        }


                        /////////////////////////////////////////////////////////////////



                        if (part.Contains("Date of Marriage"))
                        {
                            tp = part.Replace("Date of Marriage", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Date of Marriage" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Date of Marriage" + Environment.NewLine + tp;
                                }
                            }

                        }
                        if (part.Contains("Place of Marriage"))
                        {
                            tp = part.Replace("Place of Marriage", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Place of Marriage" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Place of Marriage" + Environment.NewLine + tp;
                                }
                            }

                        }
                        if (part.Contains("Maternal Parents"))
                        {
                            tp = part.Replace("Maternal Parents", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Maternal Parents" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Maternal Parents" + Environment.NewLine + tp;
                                }
                            }
                        }

                        if (part.Contains("Name(s)"))
                        {
                            tp = part.Replace("Name(s)", "").Trim();
                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Name(s)" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Name(s)" + Environment.NewLine + tp;
                                }
                            }
                        }


                        if (part.Contains("Profession"))
                        {
                            if (part.Substring(0, 10) == "Profession")
                            {
                                tp = part.Replace("Profession", "").Trim();


                                if (tp != "")
                                {
                                    if (notes != "")
                                    {
                                        notes += Environment.NewLine + "Profession" + Environment.NewLine + tp;
                                    }
                                    else
                                    {
                                        notes += "Profession" + Environment.NewLine + tp;
                                    }
                                }
                            }
                        }

                        if (part.Contains("Pedigree Chart"))
                        {
                            tp = part.Replace("Pedigree Chart", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Pedigree Chart" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Pedigree Chart" + Environment.NewLine + tp;
                                }
                            }
                        }




                        /////////////////////////////////////////////////////////////////



                        //if (part.Contains("|Date of Marriage"))
                        //{
                        //    tp = part.Replace("|Date of Marriage", "");


                        //}
                        //if (part.Contains("|Place of Marriage"))
                        //{
                        //    tp = part.Replace("|Place of Marriage", "");


                        //}
                        //if (part.Contains("|Maternal Parents"))
                        //{
                        //    tp = part.Replace("|Maternal Parents", "");


                        //}
                        //if (part.Contains("|Name(s)"))
                        //{
                        //    tp = part.Replace("|PD", "");


                        //}


                        //if (part.Contains("|Profession"))
                        //{
                        //    tp = part.Replace("|Profession", "");


                        //}

                        //if (part.Contains("|Pedigree Chart"))
                        //{
                        //    tp = part.Replace("|Pedigree Chart", "");


                        //}



                        /////////////////////////////////////////////////////////////////



                        if (part.Contains("Grandparent(s)"))
                        {
                            tp = part.Replace("Grandparent(s)", "").Trim();

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Grandparent(s)" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Grandparent(s)" + Environment.NewLine + tp;
                                }
                            }

                        }

                        if (part.Contains("Grandparent(s)"))
                        {
                            tp = part.Replace("Grandparent(s)", "").Trim(); if (tp != "")

                            if (tp != "")
                            {
                                if (notes != "")
                                {
                                    notes += Environment.NewLine + "Grandparent(s)" + Environment.NewLine + tp;
                                }
                                else
                                {
                                    notes += "Grandparent(s)" + Environment.NewLine + tp;
                                }
                            }
                        }



                    #endregion

                         }
                         catch (Exception ex1)
                         {
                             Debug.WriteLine(ex1.Message);
                         }


                    }


              



                Guid personId = Guid.Empty;

                Guid newSourceId = parishId;

                SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();


                int birthYear = getDateYear(birthDate);
                
                int deathYear = 0;

                int bapYear = getDateYear(bapDate);

                int deathEstYear = 0;
                int birthEstYear = 0;

                //if (fathercname != "")
                //{

                //    if (birthYear > 0)
                //    {
                //        deathEstYear = birthYear + 50;
                //        birthEstYear = birthYear;
                //    }

                //    if (bapYear > 0)
                //    {
                //        deathEstYear = bapYear + 50;
                //        birthEstYear = bapYear;
                //    }


                //}

                if (birthYear > 0)
                {
                    deathEstYear = birthYear + 50;
                    birthEstYear = birthYear;

                    if (bapYear == 0)
                        bapYear = birthYear;
                }

                if (bapYear > 0)
                {
                    deathEstYear = bapYear + 50;
                    birthEstYear = bapYear;


                    if (birthYear == 0)
                        birthYear = bapYear;
                }




                if (mothercname == fathercname)
                    mothercname = "";
                if (mothersname == fathersname)
                    mothersname = "";


               // string uniqueRef = Guid.NewGuid().ToString();


                if (birthYear == 0 || bapYear == 0)
                    Debug.WriteLine("poo");


                if (birthlocation.Trim() == "")
                    Debug.WriteLine("poo");


                if(cname.Trim() == "")
                    Debug.WriteLine("poo");

                if (sname.Trim() == "")
                    Debug.WriteLine("poo");


                if (notes.Contains("Yorkshire")) birthCounty = "Yorkshire";
                if (notes.Contains("Middlesex")) birthCounty = "Greater London";
                if (notes.Contains("Derbyshire")) birthCounty = "Derbyshire";
                if (notes.Contains("Staffordshire")) birthCounty = "Staffordshire";
                if (notes.Contains("London")) birthCounty = "Greater London";



                try
                {
                    personId = deathsBirthsBll.InsertDeathBirthRecord2(true, cname, sname, birthlocation, birthDate

                        , bapDate, deathDate, deathLocation, fathercname, fathersname, mothercname

                        , mothersname, source, notes, birthYear, bapYear, deathYear, birthCounty, deathCounty, ""

                        , "", "", 0, spouseCName, spouseSName, fatherOccupation,

                        Guid.Empty, 1, Guid.Empty, Guid.Empty, 1, 1, Guid.NewGuid(), birthEstYear, deathEstYear, false, true, "", "", "");

                }
                catch (Exception ex2)
                {
                    Debug.WriteLine(ex2.Message);
                }



                 try
                {
                    sourceMappingBll.Insert(newSourceId, null, null, 1, personId, DateTime.Today.ToShortDateString(), null);

                }
                 catch (Exception ex3)
                 {
                     Debug.WriteLine(ex3.Message);
                 }


                 idx++;
            }





        }

        #endregion

        #region import births and deaths functions


        public Guid makeGuid(string param, string extra)
        {
            Guid newGuid = Guid.Empty;

            //if (extra.Contains("Thomas"))
            //{ 
                
            //}

            try
            {
                newGuid = new Guid(param);
            }
            catch 
            { 
                
            }

            return newGuid;
        }

        private int makeYear(string yearVal)
        {

            int retVal = 0;



            Regex regex = new Regex(@"\d\d\d\d");

            Match _match = regex.Match(yearVal);



            if (_match.Success)
            {

                retVal = Convert.ToInt32(_match.Value);

            }



            return retVal;

        }



        private bool makeBool(string _bool)
        {

            bool retVal = false;



            if (bool.TryParse(_bool, out retVal))
            {

                return retVal;

            }

            else
            {

                return false;

            }



        }





        public string calcBirthStr(string mainBirthStr, string alternateBirth, string ageYear, string ageMonth, string ageDay, string ageWeek, string strDeathDate)
        {

            Regex regex = new Regex(@"\d\d\d\d");


            if (mainBirthStr == "" && alternateBirth != "")
            {
                mainBirthStr = alternateBirth;
            }


            if (mainBirthStr == "" && strDeathDate == "")
            {

                Match _match0 = regex.Match(alternateBirth);



                if (_match0.Success)
                {

                    mainBirthStr = "Abt" + Convert.ToInt32(_match0.Value).ToString();

                }



            }


            Match _match = regex.Match(mainBirthStr);


            if (!_match.Success)
            {

                #region we dont have a birth string supplied so try to calc. it

                int year = 0;

                int month = 1;

                int day = 1;

                int weeks = 0;

                bool isValidBirthYear = false;

                bool isValidWeeks = false;

                bool isValidMonths = false;



                isValidBirthYear = Int32.TryParse(ageYear, out year);

                isValidMonths = Int32.TryParse(ageMonth, out month);

                isValidWeeks = Int32.TryParse(ageWeek, out weeks);





                _match = regex.Match(strDeathDate);



                if (_match.Success && (isValidBirthYear || isValidMonths || isValidWeeks))
                {

                    DateTime deathDate = DateTime.Today;

                    DateTime birthDate = DateTime.Today;



                    bool isValidDeathDate = DateTime.TryParse(strDeathDate, out deathDate);



                    if (!isValidDeathDate)

                        deathDate = new DateTime(Convert.ToInt32(_match.Value), 1, 1);





                    if (isValidWeeks)
                    {

                        int _days = weeks * 7;



                        birthDate = deathDate.AddDays(_days - (_days * 2));

                    }

                    if (isValidMonths)
                    {



                        birthDate = deathDate.AddMonths(month - (month * 2));

                    }

                    if (isValidBirthYear)
                    {

                        birthDate = deathDate.AddYears(year - (year * 2));

                    }





                    return birthDate.ToString("dd-MMM-yyyy");

                }

                #endregion

            }

            else
            {

                DateTime origBirthDate = DateTime.Today;



                bool isValidBirthDate = DateTime.TryParse(mainBirthStr, out origBirthDate);



                if (!isValidBirthDate)

                    return mainBirthStr;

                else

                    return origBirthDate.ToString("dd-MMM-yyyy");

            }



            return "";



        }

        public int calcDeathInt(string deathInt, string deathStr)
        {
            int retVal = 0;
         
            Regex regex = new Regex(@"\d\d\d\d");

            Match _match = regex.Match(deathStr);

            if (_match.Success)
            {
                 
                retVal = Convert.ToInt32(_match.Value);
            }

            _match = regex.Match(deathInt);
            if (_match.Success)
            {
           
                retVal = Convert.ToInt32(_match.Value);
            }
 
            return retVal;
        }

        public int calcBirthInt(string birthStr, string alternateBirth, string ageYear, string ageMonth, string ageDay, string ageWeek, string strDeathDate)
        {

            Regex regex = new Regex(@"\d\d\d\d");

            if (strDeathDate == "1698 11 27")
            {
                Debug.WriteLine("");
            }


            if (birthStr == "" && alternateBirth != "")
            {
                birthStr =  alternateBirth;
            }



            //if (birthStr == "" && strDeathDate == "")
            //{

            //    birthStr = alternateBirth;

            //}



            Match _match = regex.Match(birthStr);



            if (!_match.Success)
            {

                #region we dont have a birth string supplied so try to calc. it

                int year = 0;

                int month = 1;

                int day = 1;

                int weeks = 0;

                bool isValidBirthYear = false;

                bool isValidWeeks = false;

                bool isValidMonths = false;



                isValidBirthYear = Int32.TryParse(ageYear, out year);

                isValidMonths = Int32.TryParse(ageMonth, out month);

                isValidWeeks = Int32.TryParse(ageWeek, out weeks);





                _match = regex.Match(strDeathDate);



                if (_match.Success && (isValidBirthYear || isValidMonths || isValidWeeks))
                {

                    DateTime deathDate = DateTime.Today;

                    DateTime birthDate = DateTime.Today;



                    bool isValidDeathDate = DateTime.TryParse(strDeathDate, out deathDate);



                    if (!isValidDeathDate)

                        deathDate = new DateTime(Convert.ToInt32(_match.Value), 1, 1);





                    if (isValidWeeks)
                    {

                        int _days = weeks * 7;



                        birthDate = deathDate.AddDays(_days - (_days * 2));

                    }

                    if (isValidMonths)
                    {



                        birthDate = deathDate.AddMonths(month - (month * 2));

                    }

                    if (isValidBirthYear)
                    {

                        birthDate = deathDate.AddYears(year - (year * 2));

                    }





                    return birthDate.Year;

                }

                #endregion

            }

            else
            {

                DateTime origBirthDate = DateTime.Today;



                bool isValidBirthDate = DateTime.TryParse(birthStr, out origBirthDate);



                if (!isValidBirthDate)

                    return Convert.ToInt32(_match.Value);

                else

                    return origBirthDate.Year;

            }



            return 0;

        }




        private int getDateYear(string _date)
        {

            int retVal = 0;



            Regex regex = new Regex(@"\d\d\d\d");

            Match _match = regex.Match(_date);



            if (_match.Success)
            {

                retVal = Convert.ToInt32(_match.Value);

            }



            return retVal;

        }

        private int getDateYear(string _date, string marriageDate)
        {

            int retVal = 0;



            Regex regex = new Regex(@"\d\d\d\d");

            Match _match = regex.Match(marriageDate);



            if (_match.Success)
            {

                retVal = Convert.ToInt32(_match.Value);

            }

            int age = 0;

            Int32.TryParse(_date, out age);

            if (age > 0)
            {
                retVal = retVal - age;
            }

            return retVal;

        }



        #endregion


        public string CreateBDCSV()
        {
            string joined = string.Join(",", BDFieldList.ToArray());


            return joined;
        }

        public string CreateMarCSV()
        {
            string joined = string.Join(",", MarriageFieldList.ToArray());


            return joined;
        }

        public string CreateSourceCSV()
        {
            string joined = string.Join(",", SourceFieldList.ToArray());


            return joined;
        }

        public void ImportBDCSV(string path, Guid parishId)
        {

            DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();



            List<string> lineList = new List<string>(System.IO.File.ReadAllLines(path, Encoding.ASCII));

          


            if (IsCombinedHandleError(lineList, BDFieldList))
                return;

            lineList.RemoveAt(0);



            string[] allLines = lineList.ToArray();//File.ReadAllLines(path);



            var query = from line in allLines

                        let data = line.Split(',')

                        select new

                        {

                            MotherId = 0,

                            FatherId = 0,

                            IsMale = makeBool(data[0]),

                            ChristianName = data[1],

                            Surname = data[2],

                            BirthLocation = data[3],

                            BirthDateStr = calcBirthStr(data[4], "", data[28], data[27], data[26], data[25], data[6]),

                            BaptismDateStr = calcBirthStr(data[5], "", data[28], data[27], data[26], data[25], data[6]),

                            DeathDateStr = data[6],

                            DeathLocation = data[7],

                            FatherChristianName = data[8],

                            FatherSurname = data[9],

                            MotherChristianName = data[10],

                            MotherSurname = data[11],

                            Notes = data[12] + Environment.NewLine + data[32],

                            Source = data[13],

                            BirthInt = calcBirthInt(data[4], data[5], data[28], data[27], data[26], data[25], data[6]),

                            BapInt = calcBirthInt(data[5], data[4], data[28], data[27], data[26], data[25], data[6]),

                            DeathInt = calcDeathInt(data[16],data[6]),

                            DeathCounty = data[17],

                            BirthCounty = data[18],

                            DateAdded = DateTime.Today.ToShortDateString(),

                            DateLastEdit = DateTime.Today.ToShortDateString(),

                            UserId = 1,

                            OrigSurname = "",

                            OrigFatherSurname = "",

                            OrigMotherSurname = "",

                            Occupation = data[19],

                            ReferenceLocation = data[20],

                            ReferenceDateStr = data[21],

                            ReferenceDateInt = makeYear(data[22]),

                            SpouseName = data[23],

                            SpouseSurname = data[24],

                            FatherOccupation = data[25],

                      //      BirthLocationId = Guid.Empty,

                        //    DeathLocationId = Guid.Empty,

                            ReferenceLocationId = Guid.Empty,

                            Unknown = "",

                            UnknownOther = "",

                            AgeYear = data[26],

                            AgeMonth = data[27],

                            AgeDay = data[28],

                            AgeWeek = data[29],


                            Notes2 = data[30],

                            SourceId = makeGuid(data[31], data[12] + Environment.NewLine + data[32]),

                            BirthLocationId = makeGuid(data[32], data[12] + Environment.NewLine + data[32]),

                            DeathLocationId = makeGuid(data[33], data[12] + Environment.NewLine + data[32]),

                            UniqueRef=  Guid.Empty,
                            
                            TotalEvents= 1,
                            
                            EventPriority = 1,



                            EstBirthYear =0,

                            EstDeathYear =0,

                            IsBirthEst = false,

                            IsDeathEst =false
                        };





            foreach (var team in query)
            {



                Debug.WriteLine(team.ToString());


            }



            Guid personId = Guid.Empty;

            Guid newSourceId = parishId;

            SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();



            foreach (var team in query)
            {

                if (team.BirthInt == 0 && team.BapInt == 0 && team.DeathInt ==0 )
                {
                    Debug.WriteLine("Person not inserted: invalid date");

                }
                else
                {
                    personId = deathsBirthsBll.InsertDeathBirthRecord2(team.IsMale, team.ChristianName, team.Surname, team.BirthLocation, team.BirthDateStr

                        , team.BaptismDateStr, team.DeathDateStr, team.DeathLocation, team.FatherChristianName, team.FatherSurname, team.MotherChristianName

                        , team.MotherSurname, team.Source, team.Notes, team.BirthInt, team.BapInt, team.DeathInt, team.BirthCounty, team.DeathCounty, team.Occupation

                        , team.ReferenceLocation, team.ReferenceDateStr, team.ReferenceDateInt, team.SpouseName, team.SpouseSurname, team.FatherOccupation,

                        team.BirthLocationId, team.UserId, team.DeathLocationId, team.ReferenceLocationId, Convert.ToInt32(team.TotalEvents), Convert.ToInt32(team.EventPriority), team.UniqueRef, team.EstBirthYear, team.EstDeathYear, team.IsBirthEst, team.IsDeathEst, "", "", "");

                }


                if (personId != Guid.Empty)
                {
                    sourceMappingBll.Insert(team.SourceId, null, null, 1, personId, DateTime.Today.ToShortDateString(), null);

                }
                else
                {
                    Debug.WriteLine("Person not inserted: personid is empty");
                }




            }







        }

        public void ImportSources(string path, int userId)
        {

            // NOTES
            // check for commas inside place description e.g. york unknown shouldnt be york,unknown
            // make sure dates have 4 digits.
            // also check the csvs arent being saved with any funny quotation marks around the descriptions etc


            SourceBLL sourceBll = new SourceBLL();

            SourceMappingParishsBLL sourceMappingParishBll = new SourceMappingParishsBLL();

            SourceTypesBLL sourceTypesBll = new SourceTypesBLL();

            ParishsBLL parishsBll = new ParishsBLL();

            SourceMappingsBLL sourceMappingsBll = new SourceMappingsBLL();

            ///sourceBll.
            ///

          //  string[] allLines = File.ReadAllLines(path);


            List<string> _allLines = new List<string>(System.IO.File.ReadAllLines(path));

            if (IsCombinedHandleError(_allLines, SourceFieldList))
                return;


            _allLines.RemoveAt(0);


            var query = from line in _allLines

            let data = line.Split(',')

            select new

            {
                SourceDesc = data[0],
                SourceOrigLocat = data[1],
                IsCopyHeld = makeBool(data[2]),
                IsViewed = makeBool(data[3]),
                IsThackrayFound = makeBool(data[4]),
                SourceDate = data[5],
                SourceDateTo = data[6],
                SourceRef = data[7],
                Notes = data[8],
                SourceParish = data[9],
                SourceType = data[10]
            };
            
            
            foreach (var team in query)
            {

                int sourceDate = getDateYear(team.SourceDate);
                int sourceDateTo = getDateYear(team.SourceDateTo);
                Guid newSourceId = Guid.Empty;
                Guid parishId = Guid.Empty;
                int sourceTypeId = 0;

                //newSourceId = sourceBll.InsertSource(team.SourceDesc, team.SourceOrigLocat, team.IsCopyHeld, team.IsViewed, team.IsThackrayFound, 1
                //   , team.SourceDate, team.SourceDateTo, sourceDate, sourceDateTo, team.SourceRef, 1, team.SourceType);



                ////this will never be completely reliable because obviously lots of places
                ////have similiar names!
                // DsParishs.ParishsDataTable parishsDataTable = parishsBll.GetParishByNameFilter(team.SourceParish);
                // DsSourceTypes.SourceTypesDataTable sourceTypesDataTable = sourceTypesBll.GetSourceTypeByFilter(team.SourceType, 1);


                // if (parishsDataTable.Count > 0)
                // {
                //     parishId = parishsDataTable[0].ParishId;
                //     sourceMappingParishBll.InsertSourceMappingParish(parishId, newSourceId, userId);
                // }

                // if (sourceTypesDataTable.Count > 0)
                // {
                //     sourceTypeId = sourceTypesDataTable[0].SourceTypeId;
                //     sourceMappingsBll.Insert(newSourceId, null, null, userId, null, DateTime.Today.ToShortDateString(), sourceTypeId);
                // }

                
            }


//

        }

        /// <summary>
        /// witnesses only import surnames 
        /// needs improving to handle christian names 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parishId"></param>
        public void ImportMarCSV(string path, Guid parishId)
        {

            MarriagesBLL marriagesBLL = new MarriagesBLL();



         //   string[] allLines = ;

            List<string> _allLines = new List<string>(System.IO.File.ReadAllLines(path));


            if (IsCombinedHandleError(_allLines, MarriageFieldList))
                return;

           // check csv has required columns

           // check columns are in correct order 



            _allLines.RemoveAt(0);

            var query = from line in _allLines

                        let data = line.Split(',')

                        select new

                        {

                            MaleId = Guid.Empty,

                            MaleCName = data[0],//a

                            MaleSName = data[1],//b

                            MaleLocation = data[2],//c

                            MaleInfo = data[3],//d

                            FemaleId = Guid.Empty,

                            FemaleCName = data[4],//e

                            FemaleSName = data[5],//f

                            FemaleLocation = data[6],//g

                            FemaleInfo = data[7],//h

                            Date = data[8],//i

                            MarriageLocation = data[9],//j

                            YearIntVal = getDateYear(data[8]),//k

                            MarriageCounty = data[11],//l

                            Source = data[12],//m

                            Witness1 = data[13],//n

                            Witness2 = data[14],//o

                            Witness3 = data[15],//p

                            Witness4 = data[16],//q

                            DateAdded = DateTime.Today,

                            DateLastEdit = DateTime.Today,

                            UserId = 1,

                            OrigMaleSurname = data[17],//r

                            OrigFemaleSurname = data[18],//s

                            MaleOccupation = data[19],//t

                            FemaleOccupation = data[20],//u

                            FemaleIsKnownWidow = makeBool(data[21]),//v

                            MaleIsKnownWidower = makeBool(data[22]),//w

                            IsBanns = makeBool(data[23]),//x



                            IsLicence = makeBool(data[24]),//y

                            MarriageLocationId = makeGuid(data[32], ""),

                            MaleLocationId = Guid.Empty,

                            FemaleLocationId = Guid.Empty,

                            SourceId = makeGuid(data[25],""),//data[31]

                            MaleBirthYear = getDateYear(data[26], data[8]),//aa

                            FemaleBirthYear = getDateYear(data[27], data[8]),//ab

                            FemaleFather = data[28],//ac

                            MaleFather = data[29],//ad

                            FemaleFatherOccupation = data[30],//ae

                            MaleFatherOccupation = data[31]//af


                        };


            //SourceId	MaleAge	FemaleAge	FemaleFather	MaleFather	FemaleFatherOccupation	MaleFatherOccupation



            foreach (var team in query)
            {



                //Debug.WriteLine(team.ToString());

              

              




            }



            foreach (var team in query)
            {

               

                Guid newSourceId = parishId;

                SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();




                string wit3 = team.Witness3 ;
                string wit4 = team.Witness4 ;



                string minfo = team.MaleInfo ;
                string finfo = team.FemaleInfo ;


                if (team.FemaleFather != "")
                {
                     wit3 = team.Witness3 +" Brides Father" + Environment.NewLine + team.FemaleFather;
                }

                if (team.MaleFather != "")
                {
                    wit4 = team.Witness4 +" Grooms Father" + Environment.NewLine + team.MaleFather;
                }

                if (team.MaleFatherOccupation != "")
                {
                    minfo = team.MaleInfo + Environment.NewLine + "Grooms Fathers Occupation" + Environment.NewLine + team.MaleFatherOccupation;
                }

                if (team.FemaleFatherOccupation != "")
                {
                    finfo = team.FemaleInfo + Environment.NewLine + "Brides Fathers Occupation" + Environment.NewLine + team.FemaleFatherOccupation;
                }

                Debug.WriteLine(team.ToString());

               // string notes = team.

                Guid marriageId = Guid.Empty;

                marriageId = marriagesBLL.InsertMarriage2(team.Date, team.FemaleCName, team.FemaleId, finfo, team.FemaleLocation, team.FemaleSName,

                    team.MaleCName, team.MaleId, minfo, team.MaleLocation, team.MaleSName, team.MarriageCounty, team.MarriageLocation,

                    team.Source, team.YearIntVal, team.MaleOccupation,

                    team.FemaleOccupation, team.IsLicence, team.IsBanns, team.FemaleIsKnownWidow, team.MaleIsKnownWidower,
                    team.UserId, team.MarriageLocationId, team.MaleLocationId, team.FemaleLocationId, team.MaleBirthYear, team.FemaleBirthYear, Guid.Empty, 0, 0,"","");



                SetWitnesses(marriageId, team.YearIntVal, team.Date, team.MarriageLocation, team.MarriageLocationId,
            team.Witness1, team.Witness2, team.Witness3, team.Witness4,
            "", "", "", "");


                //team.Witness1, team.Witness2, wit3, wit4, 

                sourceMappingBll.Insert(team.SourceId, null, marriageId, 1, null, DateTime.Today.ToShortDateString(), null);


            }





        }

        private void SetWitnesses(Guid marriageId,int marYear, string marDate, string marLocation, Guid marLocId, 
            string witness1, string witness2, string witness3, string witness4,
            string witness1C, string witness2C, string witness3C, string witness4C)
        {
            MarriageWitnessesBll mwits = new MarriageWitnessesBll();
            DeathsBirthsBLL _deathsBirthsBll = new DeathsBirthsBLL();
            //delete existing entries
            mwits.DeleteWitnessesForMarriage(marriageId);

       

            //readd or add 
            if (witness1 != "" || witness1C != "")
            {
                Person witPers1 = new Person();
                witPers1.ReferenceDateInt = marYear;
                witPers1.ReferenceDateStr = marDate;
                witPers1.ReferenceLocation = marLocation;
                witPers1.ReferenceLocationId = marLocId;
                witPers1.ChristianName = witness1C;
                witPers1.Surname = witness1;
                _deathsBirthsBll.InsertPerson(witPers1);
            }

            if (witness2 != "" || witness2C != "")
            {
                Person witPers2 = new Person();
                witPers2.ReferenceDateInt = marYear;
                witPers2.ReferenceDateStr = marDate;
                witPers2.ReferenceLocation = marLocation;
                witPers2.ReferenceLocationId = marLocId;
                witPers2.ChristianName = witness2C;
                witPers2.Surname = witness2;
                _deathsBirthsBll.InsertPerson(witPers2);
            }

            if (witness3 != "" || witness3C != "")
            {
                Person witPers3 = new Person();
                witPers3.ReferenceDateInt = marYear;
                witPers3.ReferenceDateStr = marDate;
                witPers3.ReferenceLocation = marLocation;
                witPers3.ReferenceLocationId = marLocId;
                witPers3.ChristianName = witness3C;
                witPers3.Surname = witness3;
                _deathsBirthsBll.InsertPerson(witPers3);
            }

            if (witness4 != "" || witness4C != "")
            {
                Person witPers4 = new Person();
                witPers4.ReferenceDateInt = marYear;
                witPers4.ReferenceDateStr = marDate;
                witPers4.ReferenceLocation = marLocation;
                witPers4.ReferenceLocationId = marLocId;
                witPers4.ChristianName = witness4C;
                witPers4.Surname = witness4;
                _deathsBirthsBll.InsertPerson(witPers4);
            }




        }


        private bool IsCombinedHandleError(List<string> _allLines, List<string> fieldList)
        {
            List<string> errorRows = new List<string>();
            List<string> allerrors = new List<string>();
            List<string> warningRows = new List<string>();

            bool quotesFound = false;

            int idx = 0;

            while (idx < _allLines.Count)
            {

                if (_allLines[idx].Contains("\""))
                {
                    _allLines[idx] = _allLines[idx].Replace("\"", "");
                    quotesFound = true;
                }
                idx++;
            }

            if (quotesFound)
            {
                string error = "Corrected: Quotes Found and Removed";

                warningRows.Add(error);
            }


            // check right number of columns 
            if (!IsCorrectNoCols(_allLines, fieldList, out errorRows))
            {
                allerrors.AddRange(errorRows);
            }

            if (!IsCorrectCols(_allLines,  fieldList, out errorRows))
            {
                allerrors.AddRange(errorRows);
            }

            if (!IsCorrectColOrder(_allLines,  fieldList, out errorRows))
            {
                allerrors.AddRange(errorRows);
            }



            if (warningRows.Count > 0)
            {
                Debug.WriteLine("Warnings with CSV File");
                foreach (string error in warningRows)
                {
                    Debug.WriteLine(error);
                }
            }

            if (allerrors.Count > 0)
            {
                Debug.WriteLine("Problem with CSV File");
                foreach (string error in allerrors)
                {
                    Debug.WriteLine(error);
                }

                return true;


            }
            else
            {
                return false;
            }

        }


        private static bool IsCorrectColOrder(List<string> _allLines, List<string> reqCols, out List<string> errorLog)
        {
            errorLog = new List<string>();


            if (_allLines.Count == 0)
            {
                errorLog.Add("CSV empty");
            }
            else
            {
                string topline = _allLines[0].Replace(" ", "").Trim();

                string joined = string.Join(",", reqCols.ToArray()).Trim();

                if (topline.Length != joined.Length)
                {
                    errorLog.Add("Columns have length mismatch");
                  //  errorLog.Add("Correct Column list: " + joined);
                 //   errorLog.Add("CSV Column list    : " + topline);
                }

                if (topline != joined)
                {
                    errorLog.Add("Columns in wrong order");
                    errorLog.Add("Correct Column list: " + joined);
                    errorLog.Add("CSV Column list    : " + topline);
                }


            }

            if (errorLog.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool IsCorrectCols(List<string> _allLines, List<string> reqCols, out List<string> errorLog)//, out List<string> wronglengths
        {
            errorLog = new List<string>();

            if (_allLines.Count == 0)
            {
                errorLog.Add("CSV empty");
            }
            else
            {
                int idx = 0;
                foreach (string colname in reqCols)
                {
                    if (!_allLines[0].Contains(colname))
                    {
                        errorLog.Add("Column " + colname + " missing at pos: " + idx.ToString()); 
                    }

                    idx++;
                }
            }

            if (errorLog.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }


        }


        private static bool IsCorrectNoCols(List<string> _allLines,List<string> _neededCols, out List<string> wronglengths)
        {
            wronglengths = new List<string>();

            foreach (string line in _allLines)
            {
                if (line.Split(',').Length != _neededCols.Count)
                {
                    wronglengths.Add(line);
                }
            }

            if (wronglengths.Count > 0)
            {
                Debug.WriteLine("rows wrong length");
                foreach (string line in wronglengths)
                {
                    Debug.WriteLine(line);
                }


                return false;

            }


            return true;
        }

        public void ImportParish(string path)
        {


            List<string> _allLines = new List<string>(System.IO.File.ReadAllLines(path));
            List<string> _newLines = new List<string>();

          //  _allLines.RemoveAt(0);

            int idx = 0;

            while (idx < _allLines.Count)
            {

                if (!_allLines[idx].Contains("No results"))
                {
                    if (!_allLines[idx].Contains("Huntingdonshire"))
                    {
                        if (!_allLines[idx].Contains("geocoding"))
                            _newLines.Add(_allLines[idx]);
                    }
                }

                idx++;
            }

           // string[] allLines = File.ReadAllLines(path);



            var query = from line in _newLines

            let data = line.Split(',')

            select new

            {

                ParishName = data[0],//.Replace("\"",""),

                ParentParish = "",//data[1].Replace("\"", ""),

                County = data[1],//.Replace("\"", ""),

                XLong = Convert.ToDecimal(data[2]),

                YLat = Convert.ToDecimal(data[3]),

          //      temp = data[5]

            };

            ParishsBLL marriagesBLL = new ParishsBLL();
            foreach (var team in query)
            {
                try
                {
                    marriagesBLL.AddParish(team.ParishName, "", "", team.ParentParish, 0, team.County, 0, team.XLong, team.YLat);
                  //  Debug.WriteLine(team.ParishName + "," + team.ParentParish + "," + team.County + "," + team.XLong + "," + team.YLat);
                }
                catch (Exception ep)
                {
                    Debug.WriteLine(team.ParishName + " : " +ep.Message);
                }
            }





        }


        public string ImportTree(string p, Guid guid)
        {
            CsImportGeds _csImportGeds = new CsImportGeds();

            return _csImportGeds.ImportGeds(p, guid);



        }
    }




    public class cdTreeController
    {
       
        private Gedcom.GedcomDatabase _GedcomDatabase = null;
    //    private Gedcom.GedcomIndividualRecord _CurrentGedcomIndividualRecord = null;
 






        public cdTreeController(Gedcom.GedcomDatabase gedData)
        {
         
            _GedcomDatabase = gedData;
        }



        #region find top of tree
        private GedcomIndividualRecord FindTopOfTree(string indRec)
        {
            GedcomIndividualRecord topDog = null;

            try
            {
                topDog = (GedcomIndividualRecord)this._GedcomDatabase[indRec];
                while (topDog.ChildIn.Count > 0)
                {
                    GedcomFamilyRecord gfr = (GedcomFamilyRecord)this._GedcomDatabase[topDog.ChildIn[0].Family];

                    if (gfr.Husband != null)
                        topDog = (GedcomIndividualRecord)this._GedcomDatabase[gfr.Husband];
                }
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message);
              //  MessageBox.Show(ex1.StackTrace, ex1.Message);
                topDog = null;
            }

            return topDog;
        }
        #endregion


        public void DrawTree(string indRec)
        {
          


            GedcomIndividualRecord gInd = (GedcomIndividualRecord)this._GedcomDatabase[indRec];

            GedcomIndividualRecord topDog = FindTopOfTree(indRec);

          

            int idxSpouse = 0;


     
            List<string> topDogMarriages = new List<string>();

            //******************
            // DRAW FIRST MAN!
        

            Debug.WriteLine(topDog.Names[0].Name);
            //******************

            GedcomFamilyRecord topDogsFamRec = topDog.GetFamily();
           



            // cycle through the spouses
            // of first man
            while (idxSpouse < topDog.SpouseIn.Count)
            {
                GedcomFamilyRecord gfrec = (GedcomFamilyRecord)this._GedcomDatabase[topDog.SpouseIn[idxSpouse].Family];

                GedcomIndividualRecord gdWifeRec = null;

                if (gfrec.Wife != null)
                    gdWifeRec = (GedcomIndividualRecord)this._GedcomDatabase[gfrec.Wife];

                if (gfrec.Wife == indRec)
                {
                    gdWifeRec = (GedcomIndividualRecord)this._GedcomDatabase[gfrec.Husband];
                }

                //print marriage description
                Debug.WriteLine(GetMarriageDescription(gfrec));
               

                foreach (string childStr in gfrec.Children)
                {
                    // at this point start recursive function
                    // to draw the children of the marriage
                    // downwards and so on

                    #region cycle through the children and display them and their wifes

                    GedcomIndividualRecord gdChildRec = (GedcomIndividualRecord)this._GedcomDatabase[childStr];

                    // print child name
                    Debug.WriteLine(gdChildRec.Names[0].Name);



                    // the child has a spouse
                    // add that in 

                    if (gdChildRec.SpouseIn.Count > 0)
                    {
                        int childSpouse = 0;

                        while (childSpouse < gdChildRec.SpouseIn.Count)
                        {
                            GedcomFamilyRecord childFamRec = (GedcomFamilyRecord)this._GedcomDatabase[gdChildRec.SpouseIn[childSpouse].Family];

                            GedcomIndividualRecord childSpouseRec = null;

                            if (childFamRec.Wife != null)
                                childSpouseRec = (GedcomIndividualRecord)this._GedcomDatabase[childFamRec.Wife];

                            if (gdChildRec.XRefID == childFamRec.Wife)
                            {
                                childSpouseRec = (GedcomIndividualRecord)this._GedcomDatabase[childFamRec.Husband];
                            }
                            
                            DrawMarriageBox(childSpouseRec, childFamRec);
                            DrawSpouseBox(childSpouseRec);

                            if (childFamRec != null)
                                DrawMarriageNodesChildren(childFamRec);

                            childSpouse++;
                        }


                   
                    }

                    #endregion


                }

                DrawSpouseBox(gdWifeRec);

               

                idxSpouse++;
            }

        }

        public void DrawMarriageNodesChildren(GedcomFamilyRecord gfrec)
        {

            foreach (string childStr in gfrec.Children)
            {
                #region cycle through the children and display them and their wifes

                GedcomIndividualRecord gdChildRec = (GedcomIndividualRecord)this._GedcomDatabase[childStr];


                Debug.WriteLine(gdChildRec.Names[0].Name);
             

                // the child has a spouse
                // add that in 

                if (gdChildRec.SpouseIn.Count > 0)
                {
                    int childSpouse = 0;
                    //int childMarriageNodeX = currentLevel2X + this.SpouseBoxWidth + this.HorizontalSeperationDistance;
                    //int childSpouseNodeX = childMarriageNodeX + MarriageBoxWidth + this.HorizontalSeperationDistance;

                    // there might be more than one spouse so handle them all
                    while (childSpouse < gdChildRec.SpouseIn.Count)
                    {
                        GedcomFamilyRecord childFamRec = (GedcomFamilyRecord)this._GedcomDatabase[gdChildRec.SpouseIn[childSpouse].Family];

                        GedcomIndividualRecord childSpouseRec = null;
                        if (childFamRec.Wife != null)
                            childSpouseRec = (GedcomIndividualRecord)this._GedcomDatabase[childFamRec.Wife];

                        if (gdChildRec.XRefID == childFamRec.Wife)
                        {
                            if (childFamRec.Husband != null)
                                childSpouseRec = (GedcomIndividualRecord)this._GedcomDatabase[childFamRec.Husband];
                        }


                        /*create marriage*/

                        DrawMarriageBox(childSpouseRec, childFamRec);

                        /*record wife/husband */
                        DrawSpouseBox(childSpouseRec);


                        /**/


                        if (childFamRec != null)
                            DrawMarriageNodesChildren(childFamRec);

             
                        childSpouse++;
                    }


                
                }
             


           

                #endregion

            }


         
        }


        private void DrawMarriageBox(GedcomIndividualRecord topDog, GedcomFamilyRecord gfrec)
        {
            //print marriage description 
            Debug.WriteLine(GetMarriageDescription(gfrec));
        }

        private void DrawPersonBox(GedcomIndividualRecord topDog)
        {

            //print person name
            Debug.WriteLine( topDog.Names[0].Name);

     
        }

        private void DrawSpouseBox(GedcomIndividualRecord topDog)
        {
            // print spouse name
            if (topDog != null)
                Debug.WriteLine(topDog.Names[0].Name);
            else
                Debug.WriteLine("Unknown");          
        }

        #region get a marriage description string
        private static string GetMarriageDescription(GedcomFamilyRecord gfrec)
        {
            string marriageDescrip = "";

            foreach (GedcomFamilyEvent gfe in gfrec.Events)
            {
                if (gfe.EventType == GedcomEvent.GedcomEventType.MARR)
                {
                    if (gfe.Date != null)
                    {
                        if (gfe.Date.DateString.Length == 4)
                        {
                            marriageDescrip = gfe.Date.DateString;
                        }
                        else
                        {
                            if (gfe.Date.DateTime1 != null)
                            {
                                marriageDescrip = gfe.Date.DateTime1.Value.Year.ToString();
                            }
                            else if (gfe.Date.DateTime2 != null)
                            {
                                marriageDescrip = gfe.Date.DateTime2.Value.Year.ToString();
                            }
                        }
                    }
                }

            }
            return marriageDescrip;
        }
        #endregion

    }





}


