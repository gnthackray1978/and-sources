using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TDBCore.BLL;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;
using TDBCore.Types.libs;

namespace TDBCore.Types.domain.import
{
    public class CsImportCsv
    {
        private readonly ParishsBll _parishsBll;
        private readonly SourceMappingsBll _sourceMappingBll;
        private readonly DeathsBirthsBll _deathsBirthsBll;
 
        private readonly SourceBll _sourceBll;
        private readonly SourceMappingParishsBll _sourceMappingParishBll;
        
  
        private readonly SourceMappingsBll _sourceMappingsBll;
        private readonly MarriagesBLL _marriagesBll;
        private readonly MarriageWitnessesBll _mwits;


        public CsImportCsv()
        {
            _parishsBll = new ParishsBll();
          
    
          
            _sourceBll = new SourceBll();
            _sourceMappingParishBll = new SourceMappingParishsBll();
           
            _sourceMappingsBll = new SourceMappingsBll();
            _marriagesBll = new MarriagesBLL();
            _sourceMappingBll = new SourceMappingsBll();
            _deathsBirthsBll = new DeathsBirthsBll();
            _mwits = new MarriageWitnessesBll();
        }



        public string CreatePersonCSV()
        {
            string joined = string.Join(",", CSVFiles.BDFieldList.ToArray());


            return joined;
        }

        public string CreateMarCSV()
        {
            string joined = string.Join(",", CSVFiles.MarriageFieldList.ToArray());


            return joined;
        }

        public string CreateSourceCSV()
        {
            string joined = string.Join(",", CSVFiles.SourceFieldList.ToArray());


            return joined;
        }

        public void ImportPersonCSV(string path)
        {
            var lineList = new List<string>(System.IO.File.ReadAllLines(path, Encoding.ASCII));

            if (IsCombinedHandleError(lineList, CSVFiles.BDFieldList))
                return;

            lineList.RemoveAt(0);

            string[] allLines = lineList.ToArray();
           
            var query = allLines.Select(line => new {line, data = line.Split(',')}).Select(@t => new ServicePerson

            {
                
                IsMale = @t.data.Get(CSVFiles.BDFieldList, CSVField.IsMale),
                ChristianName = @t.data.Get(CSVFiles.BDFieldList, CSVField.ChristianName),
                Surname = @t.data.Get(CSVFiles.BDFieldList, CSVField.Surname),
                BirthLocation = @t.data.Get(CSVFiles.BDFieldList, CSVField.BirthLocation),
                Birth =
                    DateTools.CalcBirthStr(@t.data.Get(CSVFiles.BDFieldList, CSVField.BirthDateStr), "",
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.ReferenceDateStr),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.AgeYear),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.AgeMonth),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.AgeWeek),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.DeathDateStr)),
                Baptism =
                    DateTools.CalcBirthStr(@t.data.Get(CSVFiles.BDFieldList, CSVField.BaptismDateStr), "",
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.ReferenceDateStr),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.AgeYear),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.AgeMonth),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.AgeWeek),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.DeathDateStr)),
                Death = @t.data.Get(CSVFiles.BDFieldList, CSVField.DeathDateStr),
                DeathLocation = @t.data.Get(CSVFiles.BDFieldList, CSVField.DeathLocation),
                FatherChristianName = @t.data.Get(CSVFiles.BDFieldList, CSVField.FatherChristianName),
                FatherSurname = @t.data.Get(CSVFiles.BDFieldList, CSVField.FatherSurname),
                MotherChristianName = @t.data.Get(CSVFiles.BDFieldList, CSVField.MotherChristianName),
                MotherSurname = @t.data.Get(CSVFiles.BDFieldList, CSVField.MotherSurname),
                Notes =
                    @t.data.Get(CSVFiles.BDFieldList, CSVField.Notes) + Environment.NewLine +
                    @t.data.Get(CSVFiles.BDFieldList, CSVField.Notes2),
                SourceDescription = @t.data.Get(CSVFiles.BDFieldList, CSVField.Source),
                BirthYear = 
                    DateTools.CalcBirthInt(@t.data.Get(CSVFiles.BDFieldList, CSVField.BirthDateStr),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.ReferenceDateStr),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.BaptismDateStr),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.AgeYear),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.AgeMonth),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.AgeWeek),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.DeathDateStr)),
                BaptismYear = 
                    DateTools.CalcBirthInt(@t.data.Get(CSVFiles.BDFieldList, CSVField.BaptismDateStr),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.ReferenceDateStr),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.BirthDateStr),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.AgeYear),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.AgeMonth),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.AgeWeek),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.DeathDateStr)),
                DeathYear = 
                    DateTools.CalcDeathInt(@t.data.Get(CSVFiles.BDFieldList, CSVField.DeathInt),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.DeathDateStr)),
                DeathCounty = @t.data.Get(CSVFiles.BDFieldList, CSVField.DeathCounty),
                BirthCounty = @t.data.Get(CSVFiles.BDFieldList, CSVField.BirthCounty),               
                UserId = 1,           
                Occupation = @t.data.Get(CSVFiles.BDFieldList, CSVField.Occupation),
                ReferenceLocation = @t.data.Get(CSVFiles.BDFieldList, CSVField.ReferenceLocation),
                ReferenceDate = @t.data.Get(CSVFiles.BDFieldList, CSVField.ReferenceDateStr),
                ReferenceYear = 
                    DateTools.CalcDeathInt(@t.data.Get(CSVFiles.BDFieldList, CSVField.ReferenceDateInt),
                        @t.data.Get(CSVFiles.BDFieldList, CSVField.ReferenceDateStr)),
                SpouseChristianName = @t.data.Get(CSVFiles.BDFieldList, CSVField.SpouseName),
                SpouseSurname = @t.data.Get(CSVFiles.BDFieldList, CSVField.SpouseSurname),
                FatherOccupation = @t.data.Get(CSVFiles.BDFieldList, CSVField.FatherOccupation),
                ReferenceLocationId = "",            
                SourceId = @t.data.Get(CSVFiles.BDFieldList, CSVField.SourceId).ToGuid(),               
                BirthLocationId = @t.data.Get(CSVFiles.BDFieldList, CSVField.LocationId).ToGuid().ToString(),               
                DeathLocationId = @t.data.Get(CSVFiles.BDFieldList, CSVField.DeathLocationId).ToGuid().ToString(),             
                PersonId = @t.data.Get(CSVFiles.BDFieldList,CSVField.PersonId).ToGuid()
            });

            foreach (var team in query)
            {
                if (team.BirthYear == 0 && team.BaptismYear == 0 && team.DeathYear ==0 && team.ReferenceYear==0)
                {
                    Debug.WriteLine("Person not inserted: invalid date");
                }
                else
                {                     
                    if(team.PersonId == Guid.Empty)
                        team.PersonId = _deathsBirthsBll.InsertDeathBirthRecord(team);
                    else
                        _deathsBirthsBll.UpdateBirthDeathRecord(team);
                }
                if (team.PersonId != Guid.Empty)
                {
                    _sourceMappingBll.Insert(team.SourceId, null, null, 1, team.PersonId, DateTime.Today.ToShortDateString(), null);
                }
                else
                {
                    Debug.WriteLine("Person not inserted: personid is empty");                   
                }

            }

        }

        public void ImportParish(string path)
        {


            var allLines = new List<string>(System.IO.File.ReadAllLines(path));
            var newLines = new List<string>();

            //  _allLines.RemoveAt(0);

            int idx = 0;

            while (idx < allLines.Count)
            {

                if (!allLines[idx].Contains("No results"))
                {
                    if (!allLines[idx].Contains("Huntingdonshire"))
                    {
                        if (!allLines[idx].Contains("geocoding"))
                            newLines.Add(allLines[idx]);
                    }
                }

                idx++;
            }

            // string[] allLines = File.ReadAllLines(path);



            var query = from line in newLines

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

            foreach (var team in query)
            {
                try
                {
                    _parishsBll.AddParish(team.ParishName, "", "", team.ParentParish, 0, team.County, 0, team.XLong, team.YLat);
                    //  Debug.WriteLine(team.ParishName + "," + team.ParentParish + "," + team.County + "," + team.XLong + "," + team.YLat);
                }
                catch (Exception ep)
                {
                    Debug.WriteLine(team.ParishName + " : " + ep.Message);
                }
            }





        }

        public void ImportSources(string path, int userId)
        {

            var allLines = new List<string>(System.IO.File.ReadAllLines(path));

            if (IsCombinedHandleError(allLines, CSVFiles.SourceFieldList))
                return;


            allLines.RemoveAt(0);


            var query = allLines.Select(line => new {line, data = line.Split(',')}).Select(@t => new SourceDto

            {
                SourceDesc = @t.data.Get(CSVFiles.SourceFieldList, CSVField.SourceDesc), //data[0],
                OriginalLocation = @t.data.Get(CSVFiles.SourceFieldList, CSVField.SourceOrigLocat), //data[1],
                IsCopyHeld = @t.data.Get(CSVFiles.SourceFieldList, CSVField.IsCopyHeld).ToBool(), // makeBool(data[2]),
                IsViewed = @t.data.Get(CSVFiles.SourceFieldList, CSVField.IsViewed).ToBool(), // makeBool(data[3]),
                IsThackrayFound = @t.data.Get(CSVFiles.SourceFieldList, CSVField.IsThackrayFound).ToBool(), //makeBool(data[4]),
                SourceDateStr = @t.data.Get(CSVFiles.SourceFieldList, CSVField.SourceDate), //data[5],
                SourceDateStrTo = @t.data.Get(CSVFiles.SourceFieldList, CSVField.SourceDateTo), //data[6],
                SourceRef = @t.data.Get(CSVFiles.SourceFieldList, CSVField.SourceRef), // data[7],
                SourceNotes = @t.data.Get(CSVFiles.SourceFieldList, CSVField.Notes), //data[8],
                Parishs = @t.data.Get(CSVFiles.SourceFieldList, CSVField.SourceParish).ParseToGuidList(), // data[9],
                SourceTypes = @t.data.Get(CSVFiles.SourceFieldList, CSVField.SourceType).ParseToIntList(), //data[10]
                SourceId = @t.data.Get(CSVFiles.SourceFieldList, CSVField.SourceId).ToGuid() //data[10]
            });
            

            var newCSV = new List<string>();

            foreach (var team in query)
            {
                if (team.SourceId == Guid.Empty)
                {                     
                    team.SourceId = _sourceBll.InsertSource(team);
                }
                else
                {
                    _sourceBll.UpdateSource(team);
                }


                if (team .Parishs!= null)
                {
                    _sourceMappingParishBll.InsertSourceMappingParish2(team.Parishs.First(), team.SourceId, userId);
                }

                team.SourceTypes.ForEach(s => _sourceMappingsBll.Insert(team.SourceId, null, null, userId, null, DateTime.Today.ToShortDateString(), s));

                // create 
                newCSV.Add(string.Join(",", CSVFiles.SourceFieldList.Select(sfield => sfield.SourceDto(team)).ToList()));

            }

            System.IO.File.Delete(path);

            System.IO.File.WriteAllLines(path,newCSV);
        }
      
        public void ImportMarCSV(string path, Guid parishId)
        {
            
            var allLines = new List<string>(System.IO.File.ReadAllLines(path));


            if (IsCombinedHandleError(allLines, CSVFiles.MarriageFieldList))
                return;

           // check csv has required columns

           // check columns are in correct order 



            allLines.RemoveAt(0);

            var query = from line in allLines

                        let data = line.Split(',')

                        select new

                        {

                            MaleId = Guid.Empty,

                            MaleCName = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleCName),//data[0],//a

                            MaleSName = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleSName),//data[1],//b

                            MaleLocation = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleLocation),//data[2],//c

                            MaleInfo = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleInfo),// data[3],//d

                            FemaleId = Guid.Empty,

                            FemaleCName = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleCName),//data[4],//e

                            FemaleSName = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleSName),//data[5],//f

                            FemaleLocation = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleLocation),// data[6],//g

                            FemaleInfo = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleInfo),// data[7],//h

                            Date = data.Get(CSVFiles.MarriageFieldList, CSVField.Date),//data[8],//i

                            MarriageLocation = data.Get(CSVFiles.MarriageFieldList, CSVField.MarriageLocation),// data[9],//j

                            YearIntVal =data.Get(CSVFiles.MarriageFieldList, CSVField.YearIntVal).ParseToValidYear(),

                            MarriageCounty = data.Get(CSVFiles.MarriageFieldList, CSVField.MarriageCounty),// data[11],//l

                            Source = data.Get(CSVFiles.MarriageFieldList, CSVField.Source),//  data[12],//m

                            Witness1 = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness1),//data[13],//n

                            Witness2 = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness2),//data[14],//o

                            Witness3 = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness3),//data[15],//p

                            Witness4 = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness4),//data[16],//q

                            DateAdded = DateTime.Today,

                            DateLastEdit = DateTime.Today,

                            UserId = 1,

                            OrigMaleSurname = data.Get(CSVFiles.MarriageFieldList, CSVField.OrigMaleSurname),// data[17],//r

                            OrigFemaleSurname = data.Get(CSVFiles.MarriageFieldList, CSVField.OrigFemaleSurname),// data[18],//s

                            MaleOccupation = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleOccupation),// data[19],//t

                            FemaleOccupation = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleOccupation),// data[20],//u

                            FemaleIsKnownWidow = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleIsKnownWidow).ToBool(),//makeBool(data[21]),//v

                            MaleIsKnownWidower = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleIsKnownWidower).ToBool(),// makeBool(data[22]),//w

                            IsBanns = data.Get(CSVFiles.MarriageFieldList, CSVField.IsBanns).ToBool(),// makeBool(data[23]),//x



                            IsLicence = data.Get(CSVFiles.MarriageFieldList, CSVField.IsLic).ToBool(),//  makeBool(data[24]),//y

                            MarriageLocationId = data.Get(CSVFiles.MarriageFieldList, CSVField.LocationId).ToGuid(),//  makeGuid(data[32], ""),

                            MaleLocationId = Guid.Empty,

                            FemaleLocationId = Guid.Empty,

                            SourceId = data.Get(CSVFiles.MarriageFieldList, CSVField.SourceId).ToGuid(),//data[25],// makeGuid(data[25],""),//data[31]

                            MaleBirthYear = DateTools.CalcMarriageBirthYear(data.Get(CSVFiles.MarriageFieldList, CSVField.MaleAge), data.Get(CSVFiles.MarriageFieldList, CSVField.Date)),//aa

                            FemaleBirthYear = DateTools.CalcMarriageBirthYear(data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleAge), data.Get(CSVFiles.MarriageFieldList, CSVField.Date)),//ab

                            FemaleFather = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleFather), //data[28],//ac

                            MaleFather = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleFather), //data[29],//ad

                            FemaleFatherOccupation = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleFatherOccupation), // data[30],//ae

                            MaleFatherOccupation = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleFatherOccupation) // data[31]//af


                        };


            //SourceId	MaleAge	FemaleAge	FemaleFather	MaleFather	FemaleFatherOccupation	MaleFatherOccupation
            //foreach (var team in query)
            //{
                //Debug.WriteLine(team.ToString());
            //}



            foreach (var team in query)
            {

               
                 

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

               
                Guid marriageId = _marriagesBll.InsertMarriage2(team.Date, team.FemaleCName, team.FemaleId, finfo, team.FemaleLocation, team.FemaleSName,

                    team.MaleCName, team.MaleId, minfo, team.MaleLocation, team.MaleSName, team.MarriageCounty, team.MarriageLocation,

                    team.Source, team.YearIntVal, team.MaleOccupation,

                    team.FemaleOccupation, team.IsLicence, team.IsBanns, team.FemaleIsKnownWidow, team.MaleIsKnownWidower,
                    team.UserId, team.MarriageLocationId, team.MaleLocationId, team.FemaleLocationId, team.MaleBirthYear, team.FemaleBirthYear, Guid.Empty, 0, 0,"","");



                SetWitnesses(marriageId, team.YearIntVal, team.Date, team.MarriageLocation, team.MarriageLocationId,
            team.Witness1, team.Witness2, wit3, wit4,
            "", "", "", "");


                //team.Witness1, team.Witness2, wit3, wit4, 

                _sourceMappingBll.Insert(team.SourceId, null, marriageId, 1, null, DateTime.Today.ToShortDateString(), null);


            }





        }




       

        private void SetWitnesses(Guid marriageId,int marYear, string marDate, string marLocation, Guid marLocId, 
            string witness1, string witness2, string witness3, string witness4,
            string witness1C, string witness2C, string witness3C, string witness4C)
        {
            //delete existing entries
            _mwits.DeleteWitnessesForMarriage(marriageId);

       

            //readd or add 
            if (witness1 != "" || witness1C != "")
            {
                var witPers1 = new Person
                {
                    ReferenceDateInt = marYear,
                    ReferenceDateStr = marDate,
                    ReferenceLocation = marLocation,
                    ReferenceLocationId = marLocId,
                    ChristianName = witness1C,
                    Surname = witness1
                };
                _deathsBirthsBll.InsertPerson(witPers1);
            }

            if (witness2 != "" || witness2C != "")
            {
                var witPers2 = new Person
                {
                    ReferenceDateInt = marYear,
                    ReferenceDateStr = marDate,
                    ReferenceLocation = marLocation,
                    ReferenceLocationId = marLocId,
                    ChristianName = witness2C,
                    Surname = witness2
                };
                _deathsBirthsBll.InsertPerson(witPers2);
            }

            if (witness3 != "" || witness3C != "")
            {
                var witPers3 = new Person
                {
                    ReferenceDateInt = marYear,
                    ReferenceDateStr = marDate,
                    ReferenceLocation = marLocation,
                    ReferenceLocationId = marLocId,
                    ChristianName = witness3C,
                    Surname = witness3
                };
                _deathsBirthsBll.InsertPerson(witPers3);
            }

            if (witness4 != "" || witness4C != "")
            {
                var witPers4 = new Person
                {
                    ReferenceDateInt = marYear,
                    ReferenceDateStr = marDate,
                    ReferenceLocation = marLocation,
                    ReferenceLocationId = marLocId,
                    ChristianName = witness4C,
                    Surname = witness4
                };
                _deathsBirthsBll.InsertPerson(witPers4);
            }




        }

        private bool IsCombinedHandleError(List<string> allLines, IList<CSVField> fieldList)
        {
            List<string> errorRows;
            var allerrors = new List<string>();
            var warningRows = new List<string>();

            bool quotesFound = false;

            int idx = 0;

            while (idx < allLines.Count)
            {

                if (allLines[idx].Contains("\""))
                {
                    allLines[idx] = allLines[idx].Replace("\"", "");
                    quotesFound = true;
                }
                idx++;
            }

            if (quotesFound)
            {             
                warningRows.Add("Corrected: Quotes Found and Removed");
            }


            // check right number of columns 
            if (!IsCorrectNoCols(allLines, fieldList, out errorRows))
            {
                allerrors.AddRange(errorRows);
            }

            if (!IsCorrectCols(allLines,  fieldList, out errorRows))
            {
                allerrors.AddRange(errorRows);
            }

            if (!IsCorrectColOrder(allLines,  fieldList, out errorRows))
            {
                allerrors.AddRange(errorRows);
            }



            if (warningRows.Count > 0)
            {
                Debug.WriteLine("Warnings with CSV File");
                Console.WriteLine("Warnings with CSV File");
                foreach (string error in warningRows)
                {
                    Debug.WriteLine(error);
                    Console.WriteLine(error);
                }
            }

            if (allerrors.Count > 0)
            {
                Debug.WriteLine("Problem with CSV File");
                Console.WriteLine("Problem with CSV File");
                foreach (string error in allerrors)
                {
                    Debug.WriteLine(error);
                    Console.WriteLine(error);
                }

                return true;


            }
            return false;
        }


        private static bool IsCorrectColOrder(List<string> allLines, IEnumerable<CSVField> reqCols, out List<string> errorLog)
        {
            errorLog = new List<string>();


            if (allLines.Count == 0)
            {
                errorLog.Add("CSV empty");
            }
            else
            {
                string topline = allLines[0].Replace(" ", "").Trim();

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

            return errorLog.Count <= 0;
        }

        private static bool IsCorrectCols(List<string> allLines, IEnumerable<CSVField> reqCols, out List<string> errorLog)
        {
            errorLog = new List<string>();

            if (allLines.Count == 0)
            {
                errorLog.Add("CSV empty");
            }
            else
            {
                int idx = 0;
                foreach (CSVField colname in reqCols)
                {
                    if (!allLines[0].Contains(colname.ToString("G")))
                    {
                        errorLog.Add("Column " + colname + " missing at pos: " + idx); 
                    }

                    idx++;
                }
            }

            return errorLog.Count <= 0;
        }

        private static bool IsCorrectNoCols(IEnumerable<string> allLines,IList<CSVField> neededCols, out List<string> wronglengths)
        {
            wronglengths = allLines.Where(line => line.Split(',').Length != neededCols.Count).Select(a=> "row col count mismatch: " + a).ToList();

            

            if (wronglengths.Count <= 0) return true;


           

            return false;
        }

    }

}


