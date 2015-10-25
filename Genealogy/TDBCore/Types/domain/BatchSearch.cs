using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using TDBCore.BLL;
using TDBCore.Interfaces;
using TDBCore.Types.domain.import;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace TDBCore.Types.domain
{
    public class BatchSearch
    {
        private readonly IBatchDal _batchDal;

        private readonly ISecurity _security = new NoSecurity();
        private readonly IParishsDal _parishsDal;
        private readonly ISourceMappingsDal _sourceMappingDal;
        private readonly IPersonDal _personDal;        
        private readonly ISourceDal _sourceDal;
        private readonly ISourceMappingParishsDal _sourceMappingParishDal;
        private readonly ISourceMappingsDal _sourceMappingsDal;
        private readonly IMarriagesDal _marriagesDal;
        private readonly IMarriageWitnessesDal _mwits;




        public BatchSearch(ISecurity security, IBatchDal iBatchDal, IParishsDal parishsDal,
                                                                ISourceMappingsDal sourceMappingDal,
                                                                IPersonDal personDal,        
                                                                ISourceDal sourceDal,
                                                                ISourceMappingParishsDal sourceMappingParishDal,
                                                                ISourceMappingsDal sourceMappingsDal,
                                                                IMarriagesDal marriagesDal,
                                                                IMarriageWitnessesDal mwits)
        {
            _security = security;
            _batchDal = iBatchDal;
            _parishsDal = parishsDal;
            _sourceDal = sourceDal;
            _sourceMappingParishDal = sourceMappingParishDal;
            _sourceMappingsDal = sourceMappingDal;
            _marriagesDal = marriagesDal;
            _sourceMappingDal = sourceMappingsDal;
            _personDal = personDal;
            _mwits = mwits;


        }

        public ServiceBatchObject Search(BatchSearchFilter batchSearchFilter, DataShaping shaper, BatchValidator validator = null)
        {
            if (!_security.IsvalidSelect()) return new ServiceBatchObject();

            if (validator != null && !validator.ValidEntry()) return new ServiceBatchObject();

            return _batchDal.GetBatchs(batchSearchFilter).ToServiceBatchObject(shaper.Column, shaper.RecordPageSize, shaper.RecordStart);
        }

        public void DeleteBatch(BatchSearchFilter sourceTypeSearchFilter)
        {
            if (!_security.IsValidDelete()) return;

            _batchDal.RemoveBatch(sourceTypeSearchFilter.BatchId);
        }

       

        // import persons
        // import parishs
        // import marriages
        // import sources




        //@"https://docs.google.com/spreadsheets/d/1nmAHAtyTSeqVNZ0oV0pW1gRSIiuok61ejEsOcy3rZvs/pub?output=csv"

        //public void ImportFromGoogle(string path)
        //{           
        ////    ImportPersonCSVFromGoogle(@"https://docs.google.com/spreadsheets/d/1nmAHAtyTSeqVNZ0oV0pW1gRSIiuok61ejEsOcy3rZvs/pub?output=csv");

        //    ImportPersonCSVFromGoogle(path);
        //}

        #region create csv files
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

        #endregion

        public bool RemoveBatch(Guid batchId) 
        {

            _batchDal.RemoveBatch(batchId);

            return true;
        }

        #region person

        public void ImportPersonCSVFromFile(string path)
        {
            var lineList = new List<string>(System.IO.File.ReadAllLines(path, Encoding.ASCII));

            if (IsCombinedHandleError(lineList, CSVFiles.BDFieldList))
                return;

            lineList.RemoveAt(0);


            ImportPersonCSV(lineList.ToArray());
        }


        public Guid ImportPersonCSVFromGoogle(string path)
        {            
            string csv = new WebClient().DownloadString(path);

            var lineList = csv.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

 
            if (IsCombinedHandleError(lineList, CSVFiles.BDFieldList))
                return Guid.Empty;

            lineList.RemoveAt(0);

          
            return ImportPersonCSV(lineList.ToArray());


        }


        public Guid ImportPersonCSV(string[] allLines)
        {


            var query = allLines.Select(line => new { line, data = line.Split(',') }).Select(@t => new ServicePerson

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
                PersonId = @t.data.Get(CSVFiles.BDFieldList, CSVField.PersonId).ToGuid()
            });

            var batchId = Guid.NewGuid();
            var startTime = DateTime.Now;

            foreach (var team in query)
            {
                //Console.WriteLine("person id:" +team.PersonId.ToString());
                //Console.WriteLine("SourceId id:" + team.SourceId.ToString());
                //Console.WriteLine("BirthLocationId id:" + team.BirthLocationId.ToString());
                //Console.WriteLine("DeathLocationId id:" + team.DeathLocationId.ToString());



                if (team.BirthYear == 0 && team.BaptismYear == 0 && team.DeathYear == 0 && team.ReferenceYear == 0)
                {
                    Debug.WriteLine("Person not inserted: invalid date");
                }
                else
                {
                    if (team.PersonId == Guid.Empty)
                        team.PersonId = _personDal.Insert(team);
                    else
                        _personDal.Update(team);

                    _batchDal.AddRecord(new BatchDto
                    {
                        Id = Guid.NewGuid(),
                        BatchId = batchId,
                        PersonId = team.PersonId,
                        TimeRun = startTime
                    });
                }

                if (team.PersonId != Guid.Empty)
                {
                    _sourceMappingDal.Insert(team.SourceId, null, null, 1, team.PersonId, DateTime.Today.ToShortDateString(), null);
                }
                else
                {
                    Debug.WriteLine("Person not inserted: personid is empty");
                }

            }

            return batchId;

        }

        #endregion



        #region marriages
        public Guid ImportMarriageCSVFromGoogle(string path)
        {
            string csv = new WebClient().DownloadString(path);

            var lineList = csv.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();


            if (IsCombinedHandleError(lineList, CSVFiles.BDFieldList))
                return Guid.Empty;

            lineList.RemoveAt(0);


            return ImportMarCSV(lineList.ToArray());
        }

        public Guid ImportMarriageCSVFromFile(string path)
        {
            var lineList = new List<string>(System.IO.File.ReadAllLines(path, Encoding.ASCII));

            if (IsCombinedHandleError(lineList, CSVFiles.BDFieldList))
                return Guid.Empty;

            lineList.RemoveAt(0);


            return ImportMarCSV(lineList.ToArray());
        }
        public Guid ImportMarCSV(string[] allLines)
        {

            // var allLines = new List<string>(System.IO.File.ReadAllLines(path));


            // if (IsCombinedHandleError(allLines, CSVFiles.MarriageFieldList))
            //     return;

            //// check csv has required columns

            //// check columns are in correct order 



            // allLines.RemoveAt(0);

            var query = from line in allLines

                        let data = line.Split(',')

                        select new ServiceMarriageImports
                        {
                            MaleCName = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleCName),//data[0],//a

                            MaleSName = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleSName),//data[1],//b

                            MaleLocation = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleLocation),//data[2],//c

                            MaleNotes = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleInfo),// data[3],//d

                            FemaleCName = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleCName),//data[4],//e

                            FemaleSName = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleSName),//data[5],//f

                            FemaleLocation = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleLocation),// data[6],//g

                            FemaleNotes = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleInfo),// data[7],//h

                            MarriageDate = data.Get(CSVFiles.MarriageFieldList, CSVField.Date),//data[8],//i

                            MarriageLocation = data.Get(CSVFiles.MarriageFieldList, CSVField.MarriageLocation),// data[9],//j

                            LocationCounty = data.Get(CSVFiles.MarriageFieldList, CSVField.MarriageCounty),// data[11],//l

                            SourceDescription = data.Get(CSVFiles.MarriageFieldList, CSVField.Source),//  data[12],//m

                            Witness1ChristianName = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness1CName),//data[13],//n
                            Witness1Surname = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness1SName),//data[13],//n
                            Witness1Description = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness1Desc),//data[13],//n

                            Witness2ChristianName = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness2CName),//data[23],//n
                            Witness2Surname = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness2SName),//data[23],//n
                            Witness2Description = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness2Desc),//data[13],//n

                            Witness3ChristianName = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness3CName),//data[33],//n
                            Witness3Surname = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness3SName),//data[33],//n
                            Witness3Description = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness3Desc),//data[13],//n

                            Witness4ChristianName = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness4CName),
                            Witness4Surname = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness4SName),
                            Witness4Description = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness4Desc),

                            Witness5ChristianName = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness5CName),
                            Witness5Surname = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness5SName),
                            Witness5Description = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness5Desc),

                            Witness6ChristianName = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness6CName),
                            Witness6Surname = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness6SName),
                            Witness6Description = data.Get(CSVFiles.MarriageFieldList, CSVField.Witness6Desc),

                            UserId = 1,

                            OrigMaleSurname = data.Get(CSVFiles.MarriageFieldList, CSVField.OrigMaleSurname),// data[17],//r

                            OrigFemaleSurname = data.Get(CSVFiles.MarriageFieldList, CSVField.OrigFemaleSurname),// data[18],//s

                            MaleOccupation = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleOccupation),// data[19],//t

                            FemaleOccupation = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleOccupation),// data[20],//u

                            IsWidow = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleIsKnownWidow).ToBool(),//makeBool(data[21]),//v

                            IsWidower = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleIsKnownWidower).ToBool(),// makeBool(data[22]),//w

                            IsBanns = data.Get(CSVFiles.MarriageFieldList, CSVField.IsBanns).ToBool(),// makeBool(data[23]),//x



                            IsLicense = data.Get(CSVFiles.MarriageFieldList, CSVField.IsLic).ToBool(),//  makeBool(data[24]),//y

                            LocationId = data.Get(CSVFiles.MarriageFieldList, CSVField.LocationId).ToGuid().ToString(),//  makeGuid(data[32], ""),


                            SourceId = data.Get(CSVFiles.MarriageFieldList, CSVField.SourceId).ToGuid(),//data[25],// makeGuid(data[25],""),//data[31]

                            MaleBirthYear = DateTools.CalcMarriageBirthYear(data.Get(CSVFiles.MarriageFieldList, CSVField.MaleAge), data.Get(CSVFiles.MarriageFieldList, CSVField.Date)),//aa

                            FemaleBirthYear = DateTools.CalcMarriageBirthYear(data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleAge), data.Get(CSVFiles.MarriageFieldList, CSVField.Date)),//ab

                            FemaleFatherCName = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleFatherCName), //data[28],//ac
                            FemaleFatherSName = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleFatherSName), //data[28],//ac

                            MaleFatherCName = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleFatherCName), //data[28],//ac
                            MaleFatherSName = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleFatherSName), //data[28],//ac


                            FemaleFatherOccupation = data.Get(CSVFiles.MarriageFieldList, CSVField.FemaleFatherOccupation), // data[30],//ae

                            MaleFatherOccupation = data.Get(CSVFiles.MarriageFieldList, CSVField.MaleFatherOccupation) // data[31]//af


                        };

            var batchId = Guid.NewGuid();
            var startTime = DateTime.Now;


            foreach (var team in query)
            {
                team.MarriageId = _marriagesDal.InsertMarriage(team);

                var witnesses = CreateWitnesses(team);

                _mwits.InsertWitnessesForMarriage(team.MarriageId, MarriageWitness.AddWitnesses(witnesses));

                _sourceMappingDal.Insert(team.SourceId, null, team.MarriageId, 1, null, DateTime.Today.ToShortDateString(), null);


                _batchDal.AddRecord(new BatchDto
                {
                    Id = Guid.NewGuid(),
                    BatchId = batchId,
                    MarriageId = team.MarriageId,
                    TimeRun = startTime
                });


            }

            return batchId;

        }


        #endregion


        public Guid ImportParishFromGoogle(string path)
        {
            string csv = new WebClient().DownloadString(path);

            var allLines = csv.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();


          //  var allLines = new List<string>(System.IO.File.ReadAllLines(path));
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


            var batchId = Guid.NewGuid();
            var startTime = DateTime.Now;


            foreach (var team in query)
            {
                try
                {
                    Guid parishId =  _parishsDal.AddParish(team.ParishName, "", "", team.ParentParish, 0, team.County, 0, team.XLong, team.YLat);
                    //  Debug.WriteLine(team.ParishName + "," + team.ParentParish + "," + team.County + "," + team.XLong + "," + team.YLat);

                    _batchDal.AddRecord(new BatchDto
                    {
                        Id = Guid.NewGuid(),
                        BatchId = batchId,
                        ParishId = parishId,
                        TimeRun = startTime
                    });
                }
                catch (Exception ep)
                {
                    Debug.WriteLine(team.ParishName + " : " + ep.Message);
                }
            }


            return batchId;


        }





        #region sources
        public Guid ImportSourceCSVFromGoogle(string path)
        {
            string csv = new WebClient().DownloadString(path);

            var lineList = csv.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            if (IsCombinedHandleError(lineList, CSVFiles.SourceFieldList))
                return Guid.Empty;


            lineList.RemoveAt(0);


            return ImportSources(lineList);


        }


        public Guid ImportSources(List<string> allLines)
        {

           // var allLines = new List<string>(System.IO.File.ReadAllLines(path));

           


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
            var batchId = Guid.NewGuid();
            var startTime = DateTime.Now;
            var userId = 1;

            foreach (var team in query)
            {
                if (team.SourceId == Guid.Empty)
                {                     
                    team.SourceId = _sourceDal.InsertSource(team);
                }
                else
                {
                    _sourceDal.UpdateSource(team);
                }


                if (team .Parishs!= null)
                {
                    _sourceMappingParishDal.InsertSourceMappingParish2(team.Parishs.First(), team.SourceId, userId);
                }

                team.SourceTypes.ForEach(s => _sourceMappingsDal.Insert(team.SourceId, null, null, userId, null, DateTime.Today.ToShortDateString(), s));

                // create 
                newCSV.Add(string.Join(",", CSVFiles.SourceFieldList.Select(sfield => sfield.SourceDto(team)).ToList()));

                _batchDal.AddRecord(new BatchDto
                {
                    Id = Guid.NewGuid(),
                    BatchId = batchId,
                    SourceId = team.SourceId,
                    TimeRun = startTime
                });
            }

            return batchId;
            //System.IO.File.Delete(path);

            //System.IO.File.WriteAllLines(path,newCSV);
        }



        #endregion


        #region privates
        private List<WitnessDto> CreateWitnesses(ServiceMarriageImports team)
        {
            var witnesses = new List<WitnessDto>();

            SetWitnesses(witnesses, team.MarriageDate, team.MarriageLocation, team.LocationId, team.FemaleFatherCName,
                team.FemaleFatherSName, "Father of Bride " + Environment.NewLine + team.FemaleFatherOccupation);

            SetWitnesses(witnesses, team.MarriageDate, team.MarriageLocation, team.LocationId, team.MaleFatherCName,
                team.MaleFatherSName, "Father of Groom " + Environment.NewLine + team.MaleFatherOccupation);

            SetWitnesses(witnesses, team.MarriageDate, team.MarriageLocation, team.LocationId, team.Witness1ChristianName,
                team.Witness1Surname, team.Witness1Description);

            SetWitnesses(witnesses, team.MarriageDate, team.MarriageLocation, team.LocationId, team.Witness2ChristianName,
                team.Witness2Surname, team.Witness2Description);

            SetWitnesses(witnesses, team.MarriageDate, team.MarriageLocation, team.LocationId, team.Witness3ChristianName,
                team.Witness3Surname, team.Witness3Description);

            SetWitnesses(witnesses, team.MarriageDate, team.MarriageLocation, team.LocationId, team.Witness4ChristianName,
                team.Witness4Surname, team.Witness4Description);

            SetWitnesses(witnesses, team.MarriageDate, team.MarriageLocation, team.LocationId, team.Witness5ChristianName,
                team.Witness5Surname, team.Witness5Description);

            SetWitnesses(witnesses, team.MarriageDate, team.MarriageLocation, team.LocationId, team.Witness6ChristianName,
                team.Witness6Surname, team.Witness6Description);

            return witnesses;
        }

        private void SetWitnesses(List<WitnessDto> witnessDtos,string marDate, string marLocation, string marLocId, string wCName, string wSName, string sDesc)
        {
            if (wCName != "")
            {
                witnessDtos.Add( new WitnessDto()
                {
                    Date = marDate,
                    Description = sDesc,
                    Location = marLocation,
                    LocationId = marLocId.ToGuid(),
                    Name = wCName,
                    Surname = wSName
                });
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

        #endregion
    }
}
