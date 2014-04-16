using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using TDBCore.Types;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;


namespace TDBCore.BLL
{

    public class EnhanceMarriage
    {

        private Marriage marriage;

        public Marriage Marriage
        {
            get { return marriage; }
            set { marriage = value; }
        }
        

        private string witness5;

        public EnhanceMarriage()
        {
        }

        public EnhanceMarriage(Marriage _m)
        {
            marriage = _m;

        }

        public string Witness5
        {
            get { return witness5; }
            set { witness5 = value; }
        }
        
    }

    public class MarriagesBLL : BaseBll
    {
       // private string connectionString = "";

        public MarriagesBLL()
        {
            
           // connectionString = TDBCore.Properties.Settings.Default.ThackrayDBConnectionString;

            EnhanceMarriage _m = new EnhanceMarriage();
            
        }



        public Guid ReorderMarriages(Guid marriageId)
        {
            
            var result = ModelContainer.ReorderMarriages(marriageId);
            Guid retVal = Guid.Empty;
 

            foreach (var _re in result.ToList())
            {
                string c1 = _re.Column1.ToString();

                retVal = new Guid(c1);            
            }
            
            return retVal;

        }


        public string SwapSpouses(List<Guid> marriageIds)
        {
        

            foreach (Guid marriageId in marriageIds)
            {
                var result = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

                if (result != null)
                {
                    var o_cname = result.MaleCName;
                    var o_sname = result.MaleSName;

                    result.MaleCName = result.FemaleCName;
                    result.MaleSName = result.FemaleSName;


                    result.FemaleCName = o_cname;
                    result.FemaleSName = o_sname;

                  //  ModelContainer.Marriages.Add(result);

                    ModelContainer.SaveChanges();

                    
                }                
            }

            return "Success";
            //return "Fail - no such marriage ID exists";
        }
 
      
        private static Person _ProcessWitness(Marriage marriage, List<string> witnessParts)
        {
          //  witnessParts.Remove(s=>s.

            witnessParts.Remove(s => s.Trim().Length == 0);

            if (witnessParts.Count > 0)
            {
                Person newPerson = new Person();
                DeathsBirthsBll deathsBirthsBll = new DeathsBirthsBll();


                newPerson.ReferenceDateInt = marriage.YearIntVal.GetValueOrDefault();
                newPerson.ReferenceDateStr = marriage.Date;
                newPerson.ReferenceLocation = marriage.MarriageLocation;
                newPerson.OrigSurname = "witness";
                if (marriage.MarriageLocationId.HasValue)
                    newPerson.ReferenceLocationId = marriage.MarriageLocationId.Value;
                if (witnessParts.Count > 1)
                {
                    int startIdx = 0;
                    foreach (string part in witnessParts)
                    {
                        if (startIdx > 0)
                        {
                            newPerson.Surname += " " + part;

                        }
                        else
                        {
                            newPerson.ChristianName = part;
                        }
                        startIdx++;
                    }
                }
                else
                {
                    if (witnessParts.Count == 1)
                    {
                        newPerson.Surname = witnessParts[0];
                    }
                }


                newPerson.Surname = newPerson.Surname.Trim();

                if (newPerson.Surname.Contains("Surrogate"))
                {
                    newPerson.Occupation = "Surrogate";
                    newPerson.Surname = newPerson.Surname.Replace("Surrogate", "");

                }

                // we need to add person

                deathsBirthsBll.InsertPerson(newPerson);

                //mwitnessBll.InsertWitnessesForMarriage(
                // we need to add witmapperson
                return newPerson;

            }

            return null;


        }





        #region marriage selection

        public List<Guid> GetDeletedMarriages()
        {
            return ModelContainer.Marriages.Where(m=>m.IsDeleted == true).Select(p=>p.Marriage_Id).ToList();
        }
 
        public IQueryable<Marriage> GetMarriages()
        {
            return ModelContainer.Marriages;
        }
         

        public IQueryable<Marriage> GetMarriageById(Guid marriageId)
        {
            return ModelContainer.Marriages.Where(m => m.Marriage_Id == marriageId && m.IsDeleted == false);
        }

        public Guid GetMarriageUniqueRef(Guid marriageId)
        {
            var a = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId && m.IsDeleted == false);

            return a.UniqueRef.GetValueOrDefault();
        }
        public IList<Guid> GetMarriageUniqueRefs(List<Guid> marriageIds, bool returnEmpty = false)
        {
            var a = ModelContainer.Marriages.Where(m => marriageIds.Contains(m.Marriage_Id)).ToList().Select(p=>p.UniqueRef.GetValueOrDefault()).ToList();

            return returnEmpty ? a.Where(m => m == Guid.Empty).ToList() : a;
        }

        public ServiceMarriage GetMarriageById2(Guid marriageId)
        {
            var ma =
                ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId && m.IsDeleted == false) ??
                new Marriage();

            return ma.ToServiceMarriage();
        }


        public IList<Guid> GetMarriageIdsByUniqueRef(Guid uniqueRef)
        {
            return ModelContainer.Marriages.Where(m => m.UniqueRef == uniqueRef).Select(p=>p.Marriage_Id).ToList();
        }


        public IList<MarriageResult> GetDataByUniqueRef(Guid uniqueRef)
        {
            IList<Marriage> retVal = null;
            SourceBll sourceBll = new SourceBll();

            retVal = ModelContainer.Marriages.Where(m => m.UniqueRef == uniqueRef).ToList();

            var selectPred = new Func<Marriage, MarriageResult>(m => new MarriageResult()
            {
                UniqueRefStr = m.UniqueRef.GetValueOrDefault(),
                UniqueRef = m.UniqueRef.GetValueOrDefault(),
                MarriageSource = m.Source,
                MarriageId = m.Marriage_Id,
                FemaleCName = m.FemaleCName,
                FemaleSName = m.FemaleSName,
                MaleCName = m.MaleCName,
                MaleSName = m.MaleSName,
                MarriageLocation = m.MarriageLocation,
                MarriageTotalEvents = m.TotalEvents.Value,
                MarriageYear = m.YearIntVal.GetValueOrDefault()
            });
            
            int idx = 0;
            while (idx < retVal.Count)
            {
                retVal[idx].Source = sourceBll.GetSourcesRef(retVal[idx].Marriage_Id);
                idx++;
            }



            return retVal.Select(selectPred).ToList();

        }

        /// <summary>
        /// takes a marriage id and returns all known duplicate marriages
        /// looks up using marriageid unique ref.
        /// </summary>

        public IList<Guid> GetDataByDupeRefByMarriageId(Guid marriageId)
        {
            var _marriage = ModelContainer.Marriages.FirstOrDefault(p => p.Marriage_Id == marriageId);

            if (_marriage != null)
            {
               

                return ModelContainer.Marriages.Where(o => o.UniqueRef == _marriage.UniqueRef && o.IsDeleted == false).Select(p=>p.Marriage_Id).ToList();
            }
            else
            {
                return new List<Guid>();
            }

        }


        public List<MarriageResult> GetFilteredMarriages(MarriageSearchFilter m)
        {
                    
            if (m.Parish.ToGuid() != Guid.Empty)
            {
                m.Source = "";

                var sources = ModelContainer.Sources.Where(o => o.SourceMappingParishs.Any(a => a.Parish.ParishId == m.Parish.ToGuid())).ToList();
                
                if(sources.Count>0)
                    sources.ForEach(p => m.Source += "," + p.SourceId.ToString());
               
                m.Source = m.Source.Substring(1);
            }
 

            var result=  ModelContainer.USP_Marriages_Filtered(m.Witness, m.Source, m.FemaleCName, m.FemaleSName, m.MaleCName,
                                                         m.MaleSName, m.County, m.Location, m.LowerDate,
                                                         m.UpperDate).Select(p => new MarriageResult()
                                                         {
                                                             FemaleCName = p.FemaleCName,
                                                             FemaleSName = p.FemaleSName,
                                                             MaleCName = p.MaleCName,
                                                             MaleSName = p.MaleSName,
                                                             MarriageId = p.Marriage_Id,
                                                             MarriageLocation = p.MarriageLocation,
                                                             MarriageSource = p.Source,
                                                             MarriageTotalEvents = p.TotalEvents.GetValueOrDefault(),
                                                             MarriageYear = p.YearIntVal.GetValueOrDefault(),
                                                             UniqueRef = p.UniqueRef.GetValueOrDefault(),
                                                             UniqueRefStr = p.UniqueRef.GetValueOrDefault(),
                                                             SourceTrees = p.links
                                                         }).ToList();


            foreach (var mr in result)
            {
               mr.Witnesses = string.Join(" ", ModelContainer.MarriageMapWitnesses.Where(mw => mw.Marriage.Marriage_Id == mr.MarriageId).Select(p => p.Person.Surname).ToArray());
            }
            


            return result;
        }

        public IList<MarriageResult> GetFilteredMarriages(string femaleCName,                                                            
                                                            string femaleLocation,
                                                            string femaleSName,
                                                            string maleCName,                                                        
                                                            string maleLocation,
                                                            string maleSName,
                                                            string marriageCounty,
                                                            string marriageLocation,
                                                            string sources,
                                                            int yearIntLower,
                                                            int yearIntUpper,
                                                            string marriageWitness)
        {


            return ModelContainer.USP_Marriages_Filtered(marriageWitness, sources, femaleCName, femaleSName, maleCName,
                                                         maleSName, marriageCounty, marriageLocation, yearIntLower,
                                                         yearIntUpper).Select(p =>  new MarriageResult()
                                                                                        {
                                                                                            FemaleCName = p.FemaleCName,
                                                                                            FemaleSName = p.FemaleSName,
                                                                                            MaleCName = p.MaleCName,
                                                                                            MaleSName = p.MaleSName,
                                                                                            MarriageId = p.Marriage_Id,
                                                                                            MarriageLocation = p.MarriageLocation,
                                                                                            MarriageSource = p.Source,
                                                                                            MarriageTotalEvents = p.TotalEvents.GetValueOrDefault(),
                                                                                            MarriageYear = p.YearIntVal.GetValueOrDefault(),
                                                                                            UniqueRef = p.UniqueRef.GetValueOrDefault(),
                                                                                            UniqueRefStr = p.UniqueRef.GetValueOrDefault(),
                                                                                            SourceTrees = p.links
                                                                                        }).ToList();


        }
 
 
        #endregion


        public void MergeMarriages(Guid marriageToMergeIntoId, Guid marriageToMergeId)
        {
            var m1 = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageToMergeIntoId);

            var m2 = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageToMergeId);

            m1.MergeInto(m2);


            ModelContainer.SaveChanges();
        }

        #region insert marriage

        public Guid InsertMarriage(ServiceMarriage sm)
        {


             

            this.Reset();

            var _marriage = new Marriage();

            if (sm.MarriageId == Guid.Empty) _marriage.Marriage_Id = Guid.NewGuid();

            _marriage.MaleCName = sm.MaleCName;
            _marriage.MaleSName = sm.MaleSName;
            _marriage.MaleLocation = sm.MaleLocation;
            _marriage.MaleInfo = sm.MaleNotes;
            _marriage.FemaleId = Guid.Empty;
            _marriage.FemaleCName = sm.FemaleCName;
            _marriage.FemaleSName = sm.FemaleSName;
            _marriage.FemaleLocation = sm.FemaleLocation;
            _marriage.FemaleInfo = sm.FemaleNotes;
            _marriage.Date = sm.MarriageDate;
            _marriage.MarriageLocation = sm.MarriageLocation;
            _marriage.YearIntVal = sm.MarriageDate.ParseToValidYear();
            _marriage.MarriageCounty = sm.LocationCounty;
            _marriage.Source = sm.SourceDescription;       
            _marriage.IsLicence = sm.IsLicense;
            _marriage.IsBanns = sm.IsBanns;
            _marriage.MaleIsKnownWidower = sm.IsWidower;
            _marriage.FemaleIsKnownWidow = sm.IsWidow;
            _marriage.FemaleOccupation = sm.FemaleOccupation;
            _marriage.MaleOccupation = sm.MaleOccupation;
            _marriage.MarriageLocationId = sm.LocationId.ToGuid();
            _marriage.MaleLocationId = Guid.Empty;
            _marriage.FemaleLocationId = Guid.Empty;
            _marriage.UserId = sm.UserId;
            _marriage.MaleBirthYear = sm.MaleBirthYear;
            _marriage.FemaleBirthYear = sm.FemaleBirthYear;
            _marriage.UniqueRef = sm.UniqueRef.ToGuid();
            _marriage.TotalEvents = sm.TotalEvents.ToInt32();
            _marriage.EventPriority = sm.Priority.ToInt32();
            _marriage.Marriage_Id = System.Guid.NewGuid();
            _marriage.IsDeleted = false;
            _marriage.MaleId = Guid.Empty;
           
            _marriage.IsComposite = false;
            _marriage.DateAdded = DateTime.Today;
            _marriage.DateLastEdit = DateTime.Today;
          
            ModelContainer.Marriages.Add(_marriage);



            ModelContainer.SaveChanges();

            //ModelContainer.Detach(_marriage);

            return _marriage.Marriage_Id;
        }


        public Guid InsertMarriage2(
                                    string marriageDate,
                                    string FemaleCName,
                                    Guid femaleId,
                                    string FemaleInfo,
                                    string FemaleLocation,
                                    string FemaleSName,
                                    string MaleCName,
                                    Guid MaleId,
                                    string MaleInfo,
                                    string MaleLocation,
                                    string MaleSName,
                                    string MarriageCounty,
                                    string MarriageLocation,
                                    string source,
                                    int yearIntVal,
                                    string maleOccupation,
                                    string femaleOccupation,
                                    bool isLicence,
                                    bool isBanns,
                                    bool isWidow,
                                    bool isWidower,
                                    int userId,
                                    Guid marriageLocationId,
                                    Guid maleLocationId,
                                    Guid femaleLocationId,
                                    int maleBirthYear,
                                    int femaleBirthYear,
                                    Guid uniqueRef,
                                    int totalEvents,
                                    int eventPriority,
                                    string origMaleName,
                                    string origFemaleName)
        {

            string wit1 = "";
            string wit2 = "";
            string wit3 = "";
            string wit4 = "";
            
            this.Reset();

            Marriage _marriage = new Marriage();

            if(uniqueRef == Guid.Empty) uniqueRef = Guid.NewGuid();

            _marriage.MaleCName = MaleCName;
            _marriage.MaleSName = MaleSName;
            _marriage.MaleLocation = MaleLocation;
            _marriage.MaleInfo = MaleInfo;
            _marriage.FemaleId = femaleId;
            _marriage.FemaleCName = FemaleCName;
            _marriage.FemaleSName = FemaleSName;
            _marriage.FemaleLocation = FemaleLocation;
            _marriage.FemaleInfo = FemaleInfo;
            _marriage.Date = marriageDate;
            _marriage.MarriageLocation = MarriageLocation;
            _marriage.YearIntVal = yearIntVal;
            _marriage.MarriageCounty = MarriageCounty;
            _marriage.Source = source;
            _marriage.Witness1 = wit1;
            _marriage.Witness2 = wit2;
            _marriage.Witness3 = wit3;
            _marriage.Witness4 = wit4;
            _marriage.IsLicence = isLicence;
            _marriage.IsBanns = isBanns;
            _marriage.MaleIsKnownWidower = isWidower;
            _marriage.FemaleIsKnownWidow = isWidow;
            _marriage.FemaleOccupation = femaleOccupation;
            _marriage.MaleOccupation = maleOccupation;
            _marriage.MarriageLocationId = marriageLocationId;
            _marriage.MaleLocationId = maleLocationId;
            _marriage.FemaleLocationId = femaleLocationId;
            _marriage.UserId = userId;
            _marriage.MaleBirthYear = maleBirthYear;
            _marriage.FemaleBirthYear = femaleBirthYear;
            _marriage.UniqueRef = uniqueRef;
            _marriage.TotalEvents = totalEvents;
            _marriage.EventPriority = eventPriority;
            _marriage.Marriage_Id = System.Guid.NewGuid();
            _marriage.IsDeleted = false;
            _marriage.MaleId = MaleId;
          //  _marriage.FemaleId = Guid.Empty;
            _marriage.IsComposite = false;
            _marriage.DateAdded = DateTime.Today;
            _marriage.DateLastEdit = DateTime.Today;
            _marriage.OrigFemaleSurname = origFemaleName;
            _marriage.OrigMaleSurname = origMaleName;

            ModelContainer.Marriages.Add(_marriage);

          

            ModelContainer.SaveChanges();

         //   ModelContainer.Detach(_marriage);

            return _marriage.Marriage_Id;
        }

        public Marriage CreateBasicMarriage(string groomcname, string groomsname, string bridecname, string bridesname, string locat, int year)
        {
            Marriage _marriage = new Marriage();

            _marriage.MaleCName = groomcname;
            _marriage.MaleSName = groomsname;
            _marriage.FemaleCName = bridecname;
            _marriage.FemaleSName = bridesname;
            _marriage.MarriageLocation = locat;
            _marriage.YearIntVal = year;
            _marriage.Date = year.ToString();

            InsertMarriageEntity(_marriage);

            return _marriage;
        }

        public Guid InsertMarriageEntity(Marriage _marriage)
        {
 
            if (_marriage.Marriage_Id == null || _marriage.Marriage_Id == Guid.Empty)
                _marriage.Marriage_Id = System.Guid.NewGuid();

            if (_marriage.MaleCName == null)
                _marriage.MaleCName = "";

            if (_marriage.MaleSName == null)
                _marriage.MaleSName = "";

            if (_marriage.MaleLocation == null)
                _marriage.MaleLocation = "";

            if (_marriage.MaleInfo == null)
                _marriage.MaleInfo = "";

            if(_marriage.FemaleId == null)
                _marriage.FemaleId = Guid.Empty;

            if (_marriage.FemaleCName == null)
                _marriage.FemaleCName = "";

            if (_marriage.FemaleSName == null)
                _marriage.FemaleSName = "";

            if (_marriage.FemaleLocation == null)
                _marriage.FemaleLocation = "";

            if (_marriage.FemaleInfo == null)
                _marriage.FemaleInfo = "";

            if (_marriage.Date == null)
                _marriage.Date = "";

            if (_marriage.MarriageLocation == null)
                _marriage.MarriageLocation = "";

            if (_marriage.MarriageCounty == null)
                _marriage.MarriageCounty = "";

            if (_marriage.Source == null)
                _marriage.Source = "";

            if (!_marriage.IsLicence.HasValue)
                _marriage.IsLicence = false;

            if (!_marriage.IsBanns.HasValue)
                _marriage.IsBanns = false;

            if (!_marriage.MaleIsKnownWidower.HasValue)
                _marriage.MaleIsKnownWidower = false;

            if (!_marriage.FemaleIsKnownWidow.HasValue)
                _marriage.FemaleIsKnownWidow = false;

            if (_marriage.FemaleOccupation == null)
                _marriage.FemaleOccupation = "";

            if (_marriage.MaleOccupation == null)
                _marriage.MaleOccupation = "";

            if (_marriage.MarriageLocationId == null)
                _marriage.MarriageLocationId = Guid.Empty;

            if (_marriage.MaleLocationId == null)
                _marriage.MaleLocationId = Guid.Empty;

            if (_marriage.FemaleLocationId == null)
                _marriage.FemaleLocationId = Guid.Empty;

            if (_marriage.UserId == 0)
                _marriage.UserId = 1;

            if (_marriage.UniqueRef == null)
                _marriage.UniqueRef = Guid.Empty;

            if (_marriage.IsDeleted == null)
                _marriage.IsDeleted = false;

            if (_marriage.DateAdded == null)
                _marriage.DateAdded = DateTime.Today;

            if (_marriage.DateLastEdit == null)
                _marriage.DateLastEdit = DateTime.Today;

            if (_marriage.OrigFemaleSurname == null)
                _marriage.OrigFemaleSurname = "";

            if (_marriage.OrigMaleSurname == null)
                _marriage.OrigMaleSurname = "";

            if (_marriage.UserId == null)
                _marriage.UserId = 1;

            if (_marriage.MaleBirthYear == null)
                _marriage.MaleBirthYear = 0;

            if (_marriage.FemaleBirthYear == null)
                _marriage.FemaleBirthYear = 0;

            if (_marriage.TotalEvents == null)
                _marriage.TotalEvents = 1;

            if (_marriage.EventPriority == null)
                _marriage.EventPriority = 0;


            _marriage.Witness1 = "";
            _marriage.Witness2 = "";
            _marriage.Witness3 = "";
            _marriage.Witness4 = "";

            ModelContainer.Marriages.Add(_marriage);
            ModelContainer.SaveChanges();

            return _marriage.Marriage_Id;
        }

        #endregion

        #region update marriage

        /// <summary>
        /// Update unique reference, total events and event priority field on marriage record.
        /// unique reference fields are used to join together duplicate marriages.
        /// total events is the number of duplicate marriages.
        /// event priority a sort order for the duplicate records. the marriage record with event priority 0 is always 
        /// displayed in the filtered results.
        /// </summary>
        public void UpdateNotes(Guid marriageId,string _notes, string groomname, string groomcname, string bridecname, string bridesname)
        {


            var _marriage = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

            if (_marriage != null)
            {

                if (groomcname != "")
                    _marriage.MaleCName = groomcname;
                if (groomname != "")
                    _marriage.MaleSName = groomname;

                if (bridecname != "")
                    _marriage.FemaleCName = bridecname;

                if (bridesname != "")
                    _marriage.FemaleSName = bridesname;

                _marriage.MaleInfo += " " + _notes;
                ModelContainer.SaveChanges();
            }

        }

        /// <summary>
        /// Update unique reference, total events and event priority field on marriage record.
        /// unique reference fields are used to join together duplicate marriages.
        /// total events is the number of duplicate marriages.
        /// event priority a sort order for the duplicate records. the marriage record with event priority 0 is always 
        /// displayed in the filtered results.
        /// </summary>
        public void UpdateMarriageUniqRef(Guid marriageId,
                                Guid uniqueRef,
                                int totalEvents,
                                int eventPriority)
        {
           
        
            var _marriage = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

            if (_marriage != null)
            {
                _marriage.UniqueRef = uniqueRef;
                _marriage.TotalEvents = totalEvents;
                _marriage.EventPriority = eventPriority;

                ModelContainer.SaveChanges();
            }

        }

        public void UpdateMarriageLocat2(Guid marriageId,
                                    string FemaleLocation,
                                    string MaleLocation,
                                    string MarriageCounty,
                                    string MarriageLocation,
                                    Guid marLocatId,
                                    Guid malLocatId,
                                    Guid femLocatId
                                    )
        {

           // Adapter.UpdateLocats(MaleLocation, FemaleLocation, MarriageLocation, MarriageCounty, marLocatId, femLocatId, malLocatId, marriageId);

            var _marriage = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

            if (_marriage != null)
            {
                _marriage.FemaleLocation = FemaleLocation;
                _marriage.MaleLocation = MaleLocation;
                _marriage.MarriageCounty = MarriageCounty;
                _marriage.MarriageLocation = MarriageLocation;
                _marriage.MarriageLocationId = marLocatId;
                _marriage.MaleLocationId = malLocatId;
                _marriage.FemaleLocationId = femLocatId;


                ModelContainer.SaveChanges();
            }

        }

        public void UpdateMarriage(ServiceMarriage serviceMarriage)
        {
            var marriage = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == serviceMarriage.MarriageId);

            if (marriage != null)
            {
                marriage.MaleCName = serviceMarriage.MaleCName;
                marriage.MaleSName = serviceMarriage.MaleSName;
                marriage.MaleLocation = serviceMarriage.MaleLocation;
                marriage.MaleInfo = serviceMarriage.MaleNotes;
                marriage.FemaleId = Guid.Empty;
                marriage.FemaleCName = serviceMarriage.FemaleCName;
                marriage.FemaleSName = serviceMarriage.FemaleSName;
                marriage.FemaleLocation = serviceMarriage.FemaleLocation;
                marriage.FemaleInfo = serviceMarriage.FemaleNotes;
                marriage.Date = serviceMarriage.MarriageDate;
                marriage.MarriageLocation = serviceMarriage.MarriageLocation;
                marriage.YearIntVal = serviceMarriage.MarriageDate.ParseToValidYear();
                marriage.MarriageCounty = serviceMarriage.LocationCounty;
                marriage.Source = serviceMarriage.SourceDescription;
                marriage.IsLicence = serviceMarriage.IsLicense;
                marriage.IsBanns = serviceMarriage.IsBanns;
                marriage.MaleIsKnownWidower = serviceMarriage.IsWidower;
                marriage.FemaleIsKnownWidow = serviceMarriage.IsWidow;
                marriage.FemaleOccupation = serviceMarriage.FemaleOccupation;
                marriage.MaleOccupation = serviceMarriage.MaleOccupation;
                marriage.MarriageLocationId = serviceMarriage.LocationId.ToGuid();
                marriage.MaleLocationId = serviceMarriage.MaleLocationId.ToGuid();
                marriage.FemaleLocationId = serviceMarriage.FemaleLocationId.ToGuid();           
                marriage.MaleBirthYear = serviceMarriage.MaleBirthYear;
                marriage.FemaleBirthYear = serviceMarriage.FemaleBirthYear;
           

                ModelContainer.SaveChanges();
            }

        }


        public void UpdateMarriage2(Guid marriageId,
          string marriageDate, string FemaleCName,  Guid femaleId,
          string FemaleInfo, string FemaleLocation,
          string FemaleSName,   string MaleCName, Guid MaleId, string MaleInfo, string MaleLocation,  string MaleSName,
          string MarriageCounty,
          string MarriageLocation,
          string source,
          int yearIntVal,
          string maleOccupation,
          string femaleOccupation,
          bool isLicence,
          bool isBanns,
          bool isWidower,
          bool isWidow,
          Guid marriageLocationId,
          Guid maleLocationId,
          Guid femaleLocationId,
          int userId, int maleBirthYear, int femaleBirthYear,
          Guid uniqueRef,
              int totalEvents,
              int eventPriority,
          string origFemaleName,
          string origMaleName)
        {


            string witness1 = "";
          string witness2= "";
          string witness3 ="";
          string witness4="";

            var _marriage = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

            if (_marriage != null)
            {
                _marriage.MaleCName = MaleCName;
                _marriage.MaleSName = MaleSName;
                _marriage.MaleLocation = MaleLocation;
                _marriage.MaleInfo = MaleInfo;
                _marriage.FemaleId = femaleId;
                _marriage.FemaleCName = FemaleCName;
                _marriage.FemaleSName = FemaleSName;
                _marriage.FemaleLocation = FemaleLocation;
                _marriage.FemaleInfo = FemaleInfo;
                _marriage.Date = marriageDate;
                _marriage.MarriageLocation = MarriageLocation;
                _marriage.YearIntVal = yearIntVal;
                _marriage.MarriageCounty = MarriageCounty;
                _marriage.Source = source;
                _marriage.Witness1 = witness1;
                _marriage.Witness2 = witness2;
                _marriage.Witness3 = witness3;
                _marriage.Witness4 = witness4;
                _marriage.IsLicence = isLicence;
                _marriage.IsBanns = isBanns;
                _marriage.MaleIsKnownWidower = isWidower;
                _marriage.FemaleIsKnownWidow = isWidow;
                _marriage.FemaleOccupation = femaleOccupation;
                _marriage.MaleOccupation = maleOccupation;
                _marriage.MarriageLocationId = marriageLocationId;
                _marriage.MaleLocationId = maleLocationId;
                _marriage.FemaleLocationId = femaleLocationId;
                _marriage.UserId = userId;
                _marriage.MaleBirthYear = maleBirthYear;
                _marriage.FemaleBirthYear = femaleBirthYear;
                _marriage.UniqueRef = uniqueRef;
                _marriage.TotalEvents = totalEvents;
                _marriage.EventPriority = eventPriority;
               // _marriage.Marriage_Id = System.Guid.NewGuid();

                ModelContainer.SaveChanges();
            }
 
        }


        #endregion



        #region delete marriage
        #region old
        //public void DeleteMarriage(Guid marriageId)
        //{
        //    Adapter.DeleteById(marriageId);
        //}
        #endregion

        public void DeleteMarriage2(Guid marriageId)
        {
           // Adapter.DeleteById(marriageId);
            var marriage = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

            if (marriage != null)
            {
                ModelContainer.Marriages.Remove(marriage);
                ModelContainer.SaveChanges();
            }
        }


        #region old
//        public void DeleteMarriageTemp(Guid marriageId)
//        {

////            UPDATE       Marriages
////SET                IsDeleted = @IsDeleted
////WHERE        (Marriage_Id = @Original_Marriage_Id)
//            Adapter.DeleteTemp(true, marriageId);
//        }
        #endregion

        public void DeleteMarriageTemp2(Guid marriageId)
        {

        
  
            var marriage = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

            if (marriage != null)
            {
                marriage.IsDeleted = true;
                ModelContainer.SaveChanges();
            }

        }
        #endregion

  

    
    }
}
