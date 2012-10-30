using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using GedItter.MarriageRecords.Datasets.DsMarriagesTableAdapters;
//using GedItter.MarriageRecords.Datasets;
//using TDBCore.Datasets.DsMarriagesTableAdapters;

//using TDBCore.Datasets;
using GedItter.BLL;
using System.Data.SqlClient;
using TDBCore.BLL;
using TDBCore.EntityModel;
using System.Diagnostics;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data;
using System.Text.RegularExpressions;
using TDBCore.Types;


namespace GedItter.MarriageRecords.BLL
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

    public class MarriagesBLL : BaseBLL
    {
       // private string connectionString = "";

        public MarriagesBLL()
        {
            
           // connectionString = TDBCore.Properties.Settings.Default.ThackrayDBConnectionString;

            EnhanceMarriage _m = new EnhanceMarriage();
            
        }


 


        public void GetMarriageWitForProc()
        {

            MarriageWitnessesBLL mwitnessBll = new MarriageWitnessesBLL();

            foreach (var marriage in ModelContainer.Marriages.Where(m => m.Witness1 != "" || m.Witness2 != "" || m.Witness3 != "" || m.Witness4 != "").ToList())
            {
                List<string> witnessParts = new List<string>();
                Person newPerson = new Person();
                List<Person> witnesses = new List<Person>();
                if (marriage.Witness1 != "")
                {
                     // just incase i got careless and entered the data a bit wrong
                     marriage.Witness1 = marriage.Witness1.Replace("  ", " ");
                     marriage.Witness1 = marriage.Witness1.Replace("   ", " ");

                     witnessParts = Regex.Split(marriage.Witness1, " ").ToList();

                     newPerson = _ProcessWitness(marriage, witnessParts);
                     
                    if(newPerson!= null)
                         witnesses.Add(newPerson);
                }
                if (marriage.Witness2 != "")
                {
                    // just incase i got careless and entered the data a bit wrong
                    marriage.Witness2 = marriage.Witness2.Replace("  ", " ");
                    marriage.Witness2 = marriage.Witness2.Replace("   ", " ");

                    witnessParts = Regex.Split(marriage.Witness2, " ").ToList();
                    newPerson = _ProcessWitness(marriage, witnessParts);
                    
                    if(newPerson!= null)
                         witnesses.Add(newPerson);
                }

                if (marriage.Witness3 != "")
                {
                    // just incase i got careless and entered the data a bit wrong
                    marriage.Witness3 = marriage.Witness3.Replace("  ", " ");
                    marriage.Witness3 = marriage.Witness3.Replace("   ", " ");

                    witnessParts = Regex.Split(marriage.Witness3, " ").ToList();
                    newPerson = _ProcessWitness(marriage, witnessParts);
                   
                    if(newPerson!= null)
                         witnesses.Add(newPerson);
                }

                if (marriage.Witness4 != "")
                {
                    // just incase i got careless and entered the data a bit wrong
                    marriage.Witness4 = marriage.Witness4.Replace("  ", " ");
                    marriage.Witness4 = marriage.Witness4.Replace("   ", " ");

                    witnessParts = Regex.Split(marriage.Witness4, " ").ToList();
                    newPerson = _ProcessWitness(marriage, witnessParts);    
                   
                    if(newPerson!= null)
                         witnesses.Add(newPerson);
                }

                mwitnessBll.InsertWitnessesForMarriage(marriage.Marriage_Id,witnesses);

            }


        }




        public  void ImportMarriageWits(string marriageId, string wit1, string wit2, string wit3, string wit4)
        {

            Guid marriageGuid = new Guid(marriageId);
            MarriageWitnessesBLL mwitnessBll = new MarriageWitnessesBLL();

           // MarriagesBLL _marriagesbll = new MarriagesBLL ();

            Marriage marriage = GetMarriageById2(marriageGuid).FirstOrDefault();

            List<string> witnessParts = new List<string>();
            Person newPerson = new Person();
            List<Person> witnesses = new List<Person>();
            if (wit1 != "")
            {
                // just incase i got careless and entered the data a bit wrong
                wit1 = wit1.Replace("  ", " ");
                wit1 = wit1.Replace("   ", " ");

                witnessParts = Regex.Split(wit1, " ").ToList();

                newPerson = _ProcessWitness(marriage, witnessParts);

                if (newPerson != null)
                    witnesses.Add(newPerson);
            }
            if (wit2 != "")
            {
                // just incase i got careless and entered the data a bit wrong
                wit2 = wit2.Replace("  ", " ");
                wit2 = wit2.Replace("   ", " ");

                witnessParts = Regex.Split(wit2, " ").ToList();
                newPerson = _ProcessWitness(marriage, witnessParts);

                if (newPerson != null)
                    witnesses.Add(newPerson);
            }

            if (wit3 != "")
            {
                // just incase i got careless and entered the data a bit wrong
                wit3 = wit3.Replace("  ", " ");
                wit3 = wit3.Replace("   ", " ");

                witnessParts = Regex.Split(wit3, " ").ToList();
                newPerson = _ProcessWitness(marriage, witnessParts);

                if (newPerson != null)
                    witnesses.Add(newPerson);
            }

            if (wit4 != "")
            {
                // just incase i got careless and entered the data a bit wrong
                wit4 = wit4.Replace("  ", " ");
                wit4 = wit4.Replace("   ", " ");

                witnessParts = Regex.Split(wit4, " ").ToList();
                newPerson = _ProcessWitness(marriage, witnessParts);

                if (newPerson != null)
                    witnesses.Add(newPerson);
            }

            mwitnessBll.InsertWitnessesForMarriage(marriage.Marriage_Id, witnesses);
        }


        private static Person _ProcessWitness(Marriage marriage, List<string> witnessParts)
        {
          //  witnessParts.Remove(s=>s.

            witnessParts.Remove(s => s.Trim().Length == 0);

            if (witnessParts.Count > 0)
            {
                Person newPerson = new Person();
                BirthDeathRecords.BLL.DeathsBirthsBLL deathsBirthsBll = new BirthDeathRecords.BLL.DeathsBirthsBLL();


                newPerson.ReferenceDateInt = marriage.YearIntVal;
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



        public IQueryable<EnhanceMarriage> GetMarriages3()
        {
            return ModelContainer.Marriages.Where(m => m.YearIntVal == 1801).Select(o => new EnhanceMarriage() { Marriage = o });
        }

        public IQueryable<Marriage> GetMarriages2()
        {
            return ModelContainer.Marriages;
        }

        public IQueryable<Marriage> GetUndeletedMarkedMarriage()
        {
            return ModelContainer.Marriages.Where(m => m.IsDeleted == false && m.TotalEvents > 1 && m.EventPriority == 1);
        }

        public IQueryable<Marriage> GetMarriageById2(Guid marriageId)
        {
            return ModelContainer.Marriages.Where(m => m.Marriage_Id == marriageId && m.IsDeleted == false);
        }

        public IQueryable<Marriage> GetDataByDateRange2(int lower, int upper)
        {
            return ModelContainer.Marriages.Where(m => m.YearIntVal >= lower && m.YearIntVal <= upper);
        }



        public IList<Marriage> GetDataByUniqueRef2(Guid uniqueRef)
        {
            IList<Marriage> retVal = null;
            SourceBLL sourceBll = new SourceBLL();

            retVal = ModelContainer.Marriages.Where(m => m.UniqueRef == uniqueRef).ToList();
            int idx = 0;
            while (idx < retVal.Count)
            {
                retVal[idx].Source = sourceBll.GetSourcesRef(retVal[idx].Marriage_Id);
                idx++;
            }

            return retVal;

        }

        /// <summary>
        /// takes a marriage id and returns all known duplicate marriages
        /// looks up using marriageid unique ref.
        /// </summary>

        public IList<Marriage> GetDataByDupeRefByMarriageId(Guid marriageId)
        {
            Marriage _marriage = ModelContainer.Marriages.FirstOrDefault(p => p.Marriage_Id == marriageId);

            if (_marriage != null)
            {
                return ModelContainer.Marriages.Where(o => o.UniqueRef == _marriage.UniqueRef && o.IsDeleted == false).ToList();
            }
            else
            {
                return new List<Marriage>();
            }

        }


        public IQueryable<Marriage> GetMarByLocat2(string marLocat, string marCount)
        {
            return ModelContainer.Marriages.Where(m => m.MarriageLocation.Contains(marLocat) && m.MarriageCounty.Contains(marCount));

        }

        public IQueryable<Marriage> GetFilteredMarriages2(
                                                                string FemaleCName,
                                                                string FemaleInfo,
                                                                string FemaleLocation,
                                                                string FemaleSName,
                                                                string MaleCName,
                                                                string MaleInfo,
                                                                string MaleLocation,
                                                                string MaleSName,
                                                                string MarriageCounty,
                                                                string MarriageLocation,
                                                                string source,
                                                                int yearIntLower,
                                                                int yearIntUpper)
        {

            // ok these cant be empty that will really really fuck things up! 
            //-except if you are looking for every location in which case thats fine
            string MarriageLocation2 = MarriageLocation;
            string MarriageLocation3 = MarriageLocation;
            string MarriageLocation4 = MarriageLocation;
            string MarriageLocation5 = MarriageLocation;

            if (MarriageLocation.Contains(","))
            {


                string[] locations = MarriageLocation.Split(',');

                MarriageLocation2 = locations[0];
                MarriageLocation3 = locations[0];
                MarriageLocation4 = locations[0];
                MarriageLocation5 = locations[0];

                if (locations.Length > 0) MarriageLocation = locations[0];
                if (locations.Length > 1) MarriageLocation2 = locations[1];
                if (locations.Length > 2) MarriageLocation3 = locations[2];
                if (locations.Length > 3) MarriageLocation4 = locations[3];
                if (locations.Length > 4) MarriageLocation5 = locations[4];
            }

            //(MaleCName LIKE '%' + @maleCName + '%') AND (MaleSName LIKE '%' + @maleSName + '%') AND (MaleLocation LIKE '%' + @maleLocation + '%')
            //AND (MaleInfo LIKE '%' + @maleInfo + '%') AND (FemaleCName LIKE '%' + @femaleCName + '%') 
            //AND (FemaleSName LIKE '%' + @femaleSName + '%') AND (FemaleLocation LIKE '%' + @femaleLocation + '%') AND 
            //    (FemaleInfo LIKE '%' + @femaleInfo + '%') AND (MarriageLocation LIKE '%' + @marriageLocation + '%' OR 
            //        MarriageLocation LIKE '%' + @marriageLocation2 + '%' OR MarriageLocation LIKE '%' + @marriageLocation3 + '%'
            //OR MarriageLocation LIKE '%' + @marriageLocation4 + '%' OR MarriageLocation LIKE '%' + @marriageLocation5 + '%')
            //AND (YearIntVal >= @yearLowerBound) AND (YearIntVal <= @yearUpperBound) AND (MarriageCounty LIKE N'%' + @marriageCounty + N'%')
            //AND (Source LIKE N'%' + @source + N'%') AND (EventPriority = 0)


            return ModelContainer.Marriages.Where(m=>m.MaleCName.Contains(MaleCName) && m.MaleSName.Contains(MaleSName) && m.MaleLocation.Contains(MaleLocation) &&
                m.MaleInfo.Contains(MaleInfo) && m.FemaleCName.Contains(FemaleCName) && m.FemaleSName.Contains(FemaleSName) && m.FemaleLocation.Contains(FemaleLocation) &&
                m.FemaleInfo.Contains(FemaleInfo) && (
                m.MarriageLocation.Contains(MarriageLocation) || m.MarriageLocation.Contains(MarriageLocation2) || m.MarriageLocation.Contains(MarriageLocation3) 
                || m.MarriageLocation.Contains(MarriageLocation4) || m.MarriageLocation.Contains(MarriageLocation5))
                && m.YearIntVal >= yearIntLower && m.YearIntVal <= yearIntUpper && m.MarriageCounty.Contains(MarriageCounty) &&
                m.Source.Contains(source) && m.EventPriority ==0 && !m.IsDeleted.Value);
       
        }


        public IQueryable<Marriage> GetFilteredMarriagesBySource2(
                                                                string FemaleCName,
                                                                string FemaleInfo,
                                                                string FemaleLocation,
                                                                string FemaleSName,
                                                                string MaleCName,
                                                                string MaleInfo,
                                                                string MaleLocation,
                                                                string MaleSName,
                                                                string MarriageCounty,
                                                                string MarriageLocation,
                                                                string source,
                                                                int yearIntLower,
                                                                int yearIntUpper)
        {

            // ok these cant be empty that will really really fuck things up! 
            //-except if you are looking for every location in which case thats fine
            string MarriageLocation2 = MarriageLocation;
            string MarriageLocation3 = MarriageLocation;
            string MarriageLocation4 = MarriageLocation;
            string MarriageLocation5 = MarriageLocation;

            if (MarriageLocation.Contains(";"))
            {


                string[] locations = MarriageLocation.Split(';');

                MarriageLocation2 = locations[0];
                MarriageLocation3 = locations[0];
                MarriageLocation4 = locations[0];
                MarriageLocation5 = locations[0];

                if (locations.Length > 0) MarriageLocation = locations[0];
                if (locations.Length > 1) MarriageLocation2 = locations[1];
                if (locations.Length > 2) MarriageLocation3 = locations[2];
                if (locations.Length > 3) MarriageLocation4 = locations[3];
                if (locations.Length > 4) MarriageLocation5 = locations[4];
            }
            //uspfilterwithsource
          //  ModelContainer.USPFilteredMarriagesWithSources(

            //DsMarriages.MarriagesDataTable tp = Adapter.GetDataWithSources(@"'c41dfad2-f3d4-4682-9c52-610851c36dc6','6982eec2-610d-48c6-949a-c3ed120b72e3'"
            
            return ModelContainer.USPFilteredMarriagesWithSources(MaleCName,
                MaleSName,
                MaleLocation,
                MaleInfo,
                FemaleCName,
                FemaleSName,
                FemaleLocation,
                FemaleInfo,
                MarriageLocation,
                MarriageLocation2,
                MarriageLocation3,
                MarriageLocation4,
                MarriageLocation5,
                yearIntLower,
                yearIntUpper,
                MarriageCounty,
                source).AsQueryable();
        }



        #endregion

        #region insert marriage

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

            Marriage _marriage = new Marriage();

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

            ModelContainer.Marriages.AddObject(_marriage);
            ModelContainer.SaveChanges();

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

            ModelContainer.Marriages.AddObject(_marriage);
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



        public void UpdateMarriage2(Guid marriageId,
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
                ModelContainer.DeleteObject(marriage);
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
