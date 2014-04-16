using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Data.SqlClient;
using TDBCore.EntityModel;
using System.Data;
using TDBCore.Types;
using TDBCore.Types.DTOs;
using TDBCore.Types.libs;


namespace TDBCore.BLL
{
    public class DeathsBirthsBll :BaseBll
    {

 
        public void DeleteDeathBirthRecord2(Guid deathBirthRecId)
        {


            Person person = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == deathBirthRecId);

            if (person != null)
            {
                person.IsDeleted = true;
                ModelContainer.SaveChanges();
            }



        }
 
        #region update

     
  
        public void UpdateBirthDeathRecord(ServicePerson person)
        {
            var personEntity = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == person.PersonId);

            if (personEntity != null)
            {

                personEntity.BapInt = person.BaptismYear;
                personEntity.BirthInt = person.BirthYear;
                personEntity.DeathInt = person.DeathYear;
                personEntity.ReferenceDateInt = person.ReferenceDate.ParseToValidYear();// DateTools.GetDateYear();

                //  _person.UserId = userId;
                //_person.EventPriority = eventPriority;
                //      _person.EstBirthYearInt = estBirthInt;
                //_person.EstDeathYearInt = estDeathInt;
                //_person.IsEstBirth = isEstBirth;
                //_person.IsEstDeath = isEstDeath;
                //_person.OrigSurname = origName;
                //_person.OrigFatherSurname = origFatherName;
                //_person.OrigMotherSurname = origMotherName;




                personEntity.IsMale = person.IsMale.ToBool();
                personEntity.ChristianName = person.ChristianName;
                personEntity.Surname = person.Surname;
                personEntity.BirthLocation = person.BirthLocation;
                personEntity.BirthDateStr = person.Birth;
                personEntity.BaptismDateStr = person.Baptism;
                personEntity.DeathDateStr = person.Death;
                personEntity.ReferenceDateStr = person.ReferenceDate;
                personEntity.DeathLocation = person.DeathLocation;
                personEntity.FatherChristianName = person.FatherChristianName;
                personEntity.FatherSurname = person.FatherSurname;
                personEntity.MotherChristianName = person.MotherChristianName;
                personEntity.MotherSurname = person.MotherSurname;
                personEntity.Notes = person.Notes;
                personEntity.Source = person.SourceDescription;
             
               
                personEntity.ReferenceLocation = person.ReferenceLocation;
                personEntity.BirthCounty = person.BirthCounty;
                personEntity.DeathCounty = person.DeathCounty;
                personEntity.Occupation = person.Occupation;
                personEntity.FatherOccupation = person.FatherOccupation;
                personEntity.SpouseName = person.SpouseChristianName;
                personEntity.SpouseSurname = person.SpouseSurname;
              
                personEntity.BirthLocationId = person.BirthLocationId.ToGuid();
                personEntity.DeathLocationId = person.DeathLocationId.ToGuid();
                personEntity.ReferenceLocationId = person.ReferenceLocationId.ToGuid();
                personEntity.TotalEvents = person.Events.ToInt32();
                
                personEntity.UniqueRef = person.UniqueReference.ToGuid();
          

            }

            ModelContainer.SaveChanges();


        }

        public void UpdateBirthDeathRecord2(Guid personId, bool isMale,
                                                             string cName,
                                                             string sName,
                                                             string bLocation,
                                                             string bDate,
                                                             string bapDate,
                                                             string detDate,
                                                             string refDate,
                                                             string dLocation,
                                                             string fatherCName,
                                                             string fatherSName,
                                                             string motherCName,
                                                             string motherSName,
                                                             string notes,
                                                             string source,
                                                             int bapInt,
                                                             int birtInt,
                                                             int detInt, 
                                                             int refInt,
                                                             string referenceLocation,
                                                             string bCounty,
                                                             string dCounty,
                                                             string occupation,
                                                             string fatherOccupation,
                                                             string spouseCName,
                                                             string spouseSName,
                                                             int userId,
                                                             Guid birthLocationId,
                                                             Guid deathLocationId,
                                                             Guid referenceLocationId,
                                                             int totalEvents,
                                                             int eventPriority,
                                                             Guid uniqueRef,
                                                             int estBirthInt,
                                                             int estDeathInt,
                                                             bool isEstBirth,
                                                             bool isEstDeath,
                                                             string origName,
                                                             string origFatherName,
                                                             string origMotherName)
        {
            var person = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == personId);

            if (person != null)
            {

                person.IsMale = isMale;
                person.ChristianName = cName;
                person.Surname = sName;
                person.BirthLocation = bLocation;
                person.BirthDateStr = bDate;
                person.BaptismDateStr = bapDate;
                person.DeathDateStr = detDate;
                person.ReferenceDateStr = refDate;
                person.DeathLocation = dLocation;
                person.FatherChristianName = fatherCName;
                person.FatherSurname = fatherSName;
                person.MotherChristianName = motherCName;
                person.MotherSurname = motherSName;
                person.Notes = notes;
                person.Source = source;
                person.BapInt = bapInt;
                person.BirthInt = birtInt;
                person.DeathInt = detInt;
                person.ReferenceDateInt = refInt;
                person.ReferenceLocation = referenceLocation;
                person.BirthCounty = bCounty;
                person.DeathCounty = dCounty;
                person.Occupation = occupation;
                person.FatherOccupation = fatherOccupation;
                person.SpouseName = spouseCName;
                person.SpouseSurname = spouseSName;
                person.UserId = userId;
                person.BirthLocationId = birthLocationId;
                person.DeathLocationId = deathLocationId;
                person.ReferenceLocationId = referenceLocationId;
                person.TotalEvents = totalEvents;
                person.EventPriority = eventPriority;
                person.UniqueRef = uniqueRef;
                person.EstBirthYearInt = estBirthInt;
                person.EstDeathYearInt = estDeathInt;
                person.IsEstBirth = isEstBirth;
                person.IsEstDeath = isEstDeath;
                person.OrigSurname = origName;
                person.OrigFatherSurname = origFatherName;
                person.OrigMotherSurname = origMotherName;

            }
            
            ModelContainer.SaveChanges();


        }



    
        private void UpdateBirthLocationId(Guid personId, Guid locationId)
        {

          

            SqlConnection sqlConnection = GetConnection();

            try
            {
                var cmd = new SqlCommand("UPDATE Persons SET BirthLocationId = @birthLocationId WHERE Person_id = @personId", sqlConnection)
                {
                    CommandType = CommandType.Text
                };
                var myParm1 = cmd.Parameters.Add("@birthLocationId", SqlDbType.UniqueIdentifier);
                myParm1.Value = locationId;
                var myParm2 = cmd.Parameters.Add("@personId", SqlDbType.UniqueIdentifier);
                myParm2.Value = personId;
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                cmd.ExecuteNonQuery();
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }


        }

        private void UpdateBirthLocation2(Guid personId, string location)
        {

            var person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            if (person == null) return;

            person.BirthLocation = location;

            ModelContainer.SaveChanges();
        }

        private void UpdateUniqueRefs2(Guid personId, Guid uniqueRef, int totalEvents, int eventPriority)
        {

            var person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            if (person == null) return;

            person.UniqueRef = uniqueRef;
            person.TotalEvents = totalEvents;
            person.EventPriority = eventPriority;

            ModelContainer.SaveChanges();
        }

        public void UpdateDuplicateRefs2(Guid person1, Guid person2)
        {
        
            List<Person> effectedPeople = ModelContainer.Persons.Where(p => (p.Person_id == person2 || p.Person_id == person1) && p.UniqueRef == Guid.Empty).ToList();


            // if our 2 people have unique refs fetch them
            foreach (var uniqRef in ModelContainer.Persons.Where(p => (p.Person_id == person2 || p.Person_id == person1) && p.UniqueRef != Guid.Empty))
            {
                Person @ref = uniqRef;
                foreach (var dupePerson in ModelContainer.Persons.Where(p => p.UniqueRef == @ref.UniqueRef))
                {
                    if (!effectedPeople.Contains(dupePerson))
                        effectedPeople.Add(dupePerson);
                }
            }


            Guid newRef = Guid.NewGuid();

            int evtCount = 1;
            foreach (Person effectedPerson in effectedPeople)
            {
               
                effectedPerson.UniqueRef = newRef;
                effectedPerson.TotalEvents = effectedPeople.Count;
                effectedPerson.EventPriority = evtCount;
                evtCount++;
            }


           // ModelContainer.AcceptAllChanges();

            ModelContainer.SaveChanges();

        }

        #endregion

        #region insert
 
        public Guid InsertPerson(Person person)
        {
            person.Person_id = Guid.NewGuid();
     

            if (person.BaptismDateStr == null) person.BaptismDateStr = "";
            if (person.BirthCounty == null) person.BirthCounty = "";
            if (person.BirthDateStr == null) person.BirthDateStr = "";
            if (person.BirthLocation == null) person.BirthLocation = "";            
            if (person.ChristianName == null) person.ChristianName = "";           
            if (person.DeathCounty == null) person.DeathCounty = "";
            if (person.DeathDateStr == null) person.DeathDateStr = "";
            if (person.DeathLocation == null) person.DeathLocation = "";
            if (person.FatherChristianName == null) person.FatherChristianName = "";
            if (person.FatherOccupation == null) person.FatherOccupation = "";
            if (person.FatherSurname == null) person.FatherSurname = "";
            if (person.MotherChristianName == null) person.MotherChristianName = "";
            if (person.MotherSurname == null) person.MotherSurname = "";
            if (person.Notes == null) person.Notes = "";
            if (person.Occupation == null) person.Occupation = "";
            if (person.OrigFatherSurname == null) person.OrigFatherSurname = "";
            if (person.OrigMotherSurname == null) person.OrigMotherSurname = "";
            if (person.OrigSurname == null) person.OrigSurname = "";
            if (person.ReferenceDateStr == null) person.ReferenceDateStr = "";
            if (person.ReferenceLocation == null) person.ReferenceLocation = "";
            if (person.ReferenceLocationId == null) person.ReferenceLocationId = Guid.Empty;
            if (person.Source == null) person.Source = "";
            if (person.SpouseName == null) person.SpouseName = "";
            if (person.SpouseSurname == null) person.SpouseSurname = "";
            if (person.Surname == null) person.Surname = "";
            if (person.UniqueRef == null) person.UniqueRef = Guid.Empty;
            if (person.UserId == 0) person.UserId = 1;
            if (person.TotalEvents == 0) person.TotalEvents = 1;
            if (person.EventPriority == 0) person.EventPriority = 1;



            ModelContainer.Persons.Add(person);

            ModelContainer.SaveChanges();

            return person.Person_id;
        }

        public Person CreateBasicPerson(string cname, string sname, string blocat, int birthyear)
        {
            var person = new Person
                {
                    ChristianName = cname,
                    Surname = sname,
                    BirthDateStr = birthyear.ToString(),
                    BirthLocation = blocat
                };

            InsertPerson(person);

            return person;
        }

        public Guid InsertDeathBirthRecord(ServicePerson person)
        {

            var personEntity = new Person
                {
                    IsMale = person.IsMale.ToBool(),
                    ChristianName = person.ChristianName,
                    Surname = person.Surname,
                    BirthLocation = person.BirthLocation,
                    BirthDateStr = person.Birth,
                    BaptismDateStr = person.Baptism,
                    DeathDateStr = person.Death,
                    ReferenceDateStr = person.ReferenceDate,
                    DeathLocation = person.DeathLocation,
                    FatherChristianName = person.FatherChristianName,
                    FatherSurname = person.FatherSurname,
                    MotherChristianName = person.MotherChristianName,
                    MotherSurname = person.MotherSurname,
                    Notes = person.Notes,
                    Source = person.SourceDescription,
                    BapInt = person.BaptismYear,
                    BirthInt = person.BirthYear,
                    DeathInt = person.DeathYear,
                    ReferenceDateInt =person.ReferenceDate.ParseToValidYear(),// DateTools.GetDateYear(),
                    ReferenceLocation = person.ReferenceLocation,
                    BirthCounty = person.BirthCounty,
                    DeathCounty = person.DeathCounty,
                    Occupation = person.Occupation,
                    FatherOccupation = person.FatherOccupation,
                    SpouseName = person.SpouseChristianName,
                    SpouseSurname = person.SpouseSurname,
                    UserId = 1,
                    BirthLocationId = person.BirthLocationId.ToGuid(),
                    DeathLocationId = person.DeathLocationId.ToGuid(),
                    ReferenceLocationId = person.ReferenceLocationId.ToGuid(),
                    TotalEvents = 1,
                    EventPriority = 1,
                    UniqueRef = Guid.NewGuid(),
                    EstBirthYearInt = 0,
                    EstDeathYearInt = 0,
                    IsEstBirth = false,
                    IsEstDeath = false,
                    OrigSurname = "",
                    OrigFatherSurname = "",
                    OrigMotherSurname = "",
                    Person_id = Guid.NewGuid(),
                    DateAdded = DateTime.Today,
                    DateLastEdit = DateTime.Today
                };


            ModelContainer.Persons.Add(personEntity);

            ModelContainer.SaveChanges();

            return personEntity.Person_id;
        }
 
        #endregion

        #region selects

       
        
        public ServicePerson GetDeathBirthRecordById(Guid personId)
        {
            var result = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == personId && !o.IsDeleted);
            
            return result.ToServicePerson();


        }
        
        public IList<ServicePerson> GetDataByDupeRef(Guid dupeRef)
        {

            var persons = ModelContainer.Persons.Where(o => o.UniqueRef == dupeRef && o.IsDeleted == false).ToList();

            return persons.ToServicePersons();


        }
 
        private IEnumerable<Person> GetUniqRefDuplicates(Guid personId)
        {
            var person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            return person != null ? ModelContainer.Persons.Where(o => o.UniqueRef == person.UniqueRef && o.IsDeleted == false) : null;
        }

        

        public List<ServicePerson> GetFilterSimple2(PersonSearchFilter personSearchFilter)
        {

            Guid parishId = personSearchFilter.ParishString.ToGuid();
           
            if (parishId != Guid.Empty)
            {
                var parishsBll = new ParishsBll();

                parishsBll.GetParishSourceRecords(parishId).ForEach(p => personSearchFilter.SourceString += "," + p.SourceId.ToString());

                personSearchFilter.SourceString = personSearchFilter.SourceString.Substring(1);
            }

            var inMemoryLocFilter = "";
            if (personSearchFilter.Location.Contains(','))
            {
                inMemoryLocFilter = personSearchFilter.Location;
                personSearchFilter.Location = "";
            }

            var temp =  ModelContainer.USP_Persons_Filtered_1(personSearchFilter.CName, personSearchFilter.Surname,
                personSearchFilter.FatherChristianName, personSearchFilter.FatherSurname,
                personSearchFilter.MotherChristianName, personSearchFilter.MotherSurname,
                personSearchFilter.SourceString, personSearchFilter.Location, personSearchFilter.LowerDate,
                                                personSearchFilter.UpperDate, personSearchFilter.County, personSearchFilter.SpouseChristianName, personSearchFilter.IsIncludeSources).Select(p => new ServicePerson
                                                    {
                                                                                           PersonId = p.Person_id,
                                                                                           ChristianName = p.ChristianName,
                                                                                           Surname = p.Surname,
                                                                                           BirthLocation = p.BirthLocation,
                                                                                           Birth = p.BirthDateStr,
                                                                                           Baptism = p.BaptismDateStr,
                                                                                           Death = p.DeathDateStr,
                                                                                           DeathLocation = p.DeathLocation,
                                                                                           FatherChristianName = p.FatherChristianName,
                                                                                           FatherSurname = p.FatherSurname,
                                                                                           MotherChristianName = p.MotherChristianName,
                                                                                           MotherSurname = p.MotherSurname,
                                                                                           SourceDescription = p.Source,
                                                                                           BirthYear = (p.BirthInt==0) ? p.BapInt : p.BirthInt,
                                                                                           DeathYear = p.DeathInt,
                                                                                           LinkedTrees = p.tree_links,
                                                                                           Occupation = p.Occupation,                                                                                           
                                                                                           SpouseChristianName = p.SpouseName,
                                                                                           SpouseSurname = p.SpouseSurname,
                                                                                           Spouse = p.SpouseName + " " + p.SpouseSurname,
                                                                                           FatherOccupation = p.FatherOccupation,
                                                                                           Events = p.TotalEvents.ToString(CultureInfo.InvariantCulture),
                                                                                           UniqueReference = p.UniqueRef.GetValueOrDefault().ToString(),
                                                                                           SourceDateInt = p.SourceDateInt,
                                                                                           SourceDateStr = p.SourceDateStr,
                                                                                           SourceParishName = p.SourceParishName,
                                                                                           SourceRef = p.SourceRef,
                                                                                           SourceId = p.RefSourceId
                                                                                           
                                                                                       }).ToList();


            if (inMemoryLocFilter.Length > 0)
            {
                //filter by multiple locations in memory cause the sql is too much a pain
                temp = temp.Where(servicePerson => inMemoryLocFilter.Split(',').ToList().Any(l => servicePerson.BirthLocation.LazyContains(l) || servicePerson.DeathLocation.LazyContains(l))).ToList();               
            }

            if (personSearchFilter.IsIncludeDeaths) temp = temp.Where(p => p.DeathYear > 0).ToList();

            if (personSearchFilter.IsIncludeBirths) temp = temp.Where(p => p.BirthYear > 0).ToList();

            return temp;

        }

        public IQueryable<Person> GetByLocation2(Guid deathLocationId, Guid birthLocationId, string birthLoc, string deathLocat
                                            , string birthCounty, string deathCounty)
        { 
            return ModelContainer.Persons.Where(p => p.BirthLocationId == birthLocationId && p.BirthLocation.Contains(birthLoc) && p.DeathLocation.Contains(deathLocat) && p.DeathCounty.Contains(deathCounty) && p.BirthCounty.Contains(birthCounty));
        }
 
      

        #endregion



        public void UpdateDeletedBirths()
        {


            foreach (var pROw in ModelContainer.Persons.Where(m => m.IsDeleted && m.TotalEvents > 1))
            {
                var peopleToKeep = new List<Guid>();

                var dsDbTemp = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == pROw.Person_id && !o.IsDeleted);

                if (dsDbTemp != null)
                {
                    var personList =
                        ModelContainer.Persons.Where(
                            o =>
                            o.UniqueRef == dsDbTemp.Person_id && o.IsDeleted == false && o.Person_id != pROw.Person_id)
                                      .Select(p => p.Person_id).ToList();

                    peopleToKeep.AddRange(personList);
                }

                Guid newRef = Guid.NewGuid();

                int evtCount = 1;
                foreach (Guid id in peopleToKeep)
                {
                    UpdateUniqueRefs2(id, newRef, peopleToKeep.Count, evtCount);
                    evtCount++;
                }

                newRef = Guid.NewGuid();
                UpdateUniqueRefs2(pROw.Person_id, newRef, 1, 1);
            }



        }

        /// <summary>
        /// Attempt to merge ALL duplicate records
        /// </summary>
        public void MergeDuplicateRecords()
        {

            var records = ModelContainer.Persons.Where(o => !o.IsDeleted && o.TotalEvents > 1 && o.EventPriority == 1).ToList();      
            records.ForEach(MergeDuplicateRecord);           
        }

        public void MergeDuplicateRecords(Guid personId)
        {


            var record = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == personId);

            MergeDuplicateRecord(record);
        }

        private void MergeDuplicateRecord(Person newRecord)
        {
       
            var sourceBll = new SourceBll();
            var sourceMappingsBll = new SourceMappingsBll();
            var sourceList = new List<Guid>();

            if (newRecord != null)
            {

                foreach (var dupePerson in GetUniqRefDuplicates(newRecord.Person_id))
                {
                    sourceList.AddRange(
                        sourceBll.FillSourceTableByPersonOrMarriageId2(dupePerson.Person_id).Select(dp => dp.SourceId).
                            ToList());

                    newRecord.MergeInto(dupePerson);
                }

                sourceMappingsBll.WritePersonSources2(newRecord.Person_id, sourceList, 1);

                ModelContainer.SaveChanges();
            }


        }

        private void UpdateTidyBirthLocations()
        {
            // get all persons with no county
            // look in the countydictionary
            // and fill out the county using the dictionary
          

           

            var dummyLocationdId = new Guid("A813A1FF-6093-4924-A7B2-C5D1AF6FF699");



            foreach (var prow in ModelContainer.Persons.Where(p => p.BirthLocationId == dummyLocationdId).Where(d => d.BirthCounty != ""
                && d.BirthLocation != "" && d.BirthLocation.ToLower() != "unknown" && d.BirthLocation.Contains(d.BirthCounty)))
            {
                char[] charsToTrim = { ',', '.', ' ' };
                string newLocation = prow.BirthLocation.Replace(prow.BirthCounty, "").TrimEnd(charsToTrim);

                newLocation = newLocation.Replace(",,", ",");

                UpdateBirthLocation2(prow.Person_id, newLocation);
            }
        }

        public void UpdateLocationIdsFromDictionary()
        {

            var locationDictionaryBll = new LocationDictionaryBll();


         
            Guid dummyLocationdId = new Guid("A813A1FF-6093-4924-A7B2-C5D1AF6FF699");



            foreach (var prow in ModelContainer.Persons.Where(p => p.BirthLocationId == dummyLocationdId).Where(p => p.BirthLocation != ""))// personDataTable.Where(o => o.BirthLocation != ""))
            {
                var dictEntry = locationDictionaryBll.GetEntryByLocatAndCounty(prow.BirthLocation, prow.BirthCounty);

                if (dictEntry != null && dictEntry.ParishId != null)
                   UpdateBirthLocationId(prow.Person_id, dictEntry.ParishId.Value);
            }



        }

        public void UpdateLocationIdsFromParishTable()
        {

            UpdateTidyBirthLocations();

            var locationDictionaryBll = new LocationDictionaryBll();

            var parishsBll = new ParishsBll();
          
            var dummyLocationdId = new Guid("A813A1FF-6093-4924-A7B2-C5D1AF6FF699");

            List<string> counties = ModelContainer.Persons.Where(p => p.BirthLocationId == dummyLocationdId).Select(o => o.BirthCounty).Distinct().ToList();
           
            foreach (string county in counties)
            {
                string county1 = county;
                foreach (var personRec in ModelContainer.Persons.Where(p => p.BirthLocationId == dummyLocationdId).Where(o => o.BirthCounty == county1))
                {
                    var var2 = parishsBll.GetParishsByCounty2(county).FirstOrDefault(p => personRec.BirthLocation.Contains(p.ParishName));

                    if (var2 != null)
                    {
                        UpdateBirthLocationId(personRec.Person_id, var2.ParishId);
                      
                    }

                    var var3 = locationDictionaryBll.GetEntryByLocatAndCounty(personRec.BirthLocation, county);
                    if (var3 != null)
                    {
                        if (var3.ParishId != null) UpdateBirthLocationId(personRec.Person_id, var3.ParishId.Value);
                    }

                }
            }
        }

        public void UpdateDateEstimates()
        {



            foreach (var pRow in ModelContainer.Persons.Where(p => p.EstBirthYearInt == 0 && p.EstDeathYearInt == 0).ToList())
            {

                int estBirthYear;
                int estDeathYear;
                bool isEstBirth;
                bool isEstDeath;


                DateTools.CalcEstDates(pRow.BirthInt, pRow.BapInt, pRow.DeathInt, out estBirthYear,
                    out estDeathYear, out isEstBirth, out isEstDeath, pRow.FatherChristianName, pRow.MotherChristianName);

                UpdateBirthDeathRecord2(pRow.Person_id, pRow.IsMale, pRow.ChristianName, pRow.Surname,
                    pRow.BirthLocation, pRow.BirthDateStr, pRow.BaptismDateStr, pRow.DeathDateStr, pRow.ReferenceDateStr,
                    pRow.DeathLocation, pRow.FatherChristianName, pRow.FatherSurname, pRow.MotherChristianName, pRow.MotherSurname,
                    pRow.Notes, pRow.Source, pRow.BapInt, pRow.BirthInt, pRow.DeathInt, pRow.ReferenceDateInt, pRow.ReferenceLocation,
                    pRow.BirthCounty, pRow.DeathCounty, pRow.Occupation, pRow.FatherOccupation, pRow.SpouseName, pRow.SpouseSurname,
                    pRow.UserId, pRow.BirthLocationId, pRow.DeathLocationId, pRow.ReferenceLocationId, pRow.TotalEvents, pRow.EventPriority, pRow.UniqueRef.GetValueOrDefault(),
                    estBirthYear, estDeathYear, isEstBirth, isEstDeath, pRow.OrigSurname, pRow.OrigFatherSurname, pRow.OrigMotherSurname);

            }

        }

        public List<Guid> DelinkPersons(List<Guid> selectedRecordIds)
        {
          
            var relationsBll = new RelationsBll();

            Guid personA = selectedRecordIds[0];


            relationsBll.DeleteRelationMapping(selectedRecordIds);


            // what we are left with is the identical unique refs in the persons table
            // remember we are deleting family links as well so this wont apply to them!
            //find the unique ref for the two people
            // look up everyone with that unique ref give our 2 people we are detaching new unique refs 
            // and update the count of the remaining ones


            // get list of unique refs if they exist for the 2 persons


            Guid newRef = Guid.NewGuid();

            int evtCount = 1;


        
            List<Guid> peopleToKeep = (from pRow in GetUniqRefDuplicates(personA) where !selectedRecordIds.Contains(pRow.Person_id) select pRow.Person_id).ToList();


            foreach (Guid id in peopleToKeep)
            {
                UpdateUniqueRefs2(id, newRef, peopleToKeep.Count, evtCount);
                evtCount++;
            }

            var parentGuids = new List<Guid> {newRef};

            //SetParentRecordIds(parentGuids);

            // records to remove done here
            foreach (Guid personToRemove in selectedRecordIds)
            {
                newRef = Guid.NewGuid();
                UpdateUniqueRefs2(personToRemove, newRef, 1, 1);
            }


            if (peopleToKeep.Count == 0)
            {
                parentGuids.Clear();
                parentGuids.Add(newRef);

            }


            return parentGuids;
        }

        public void DelinkAllRelatedPersons(List<Guid> selectedRecordIds)
        {
            if (selectedRecordIds.Count <= 0) return;

            var relationsBll = new RelationsBll();
            relationsBll.DeleteRelationMapping(selectedRecordIds);
            Guid personA = selectedRecordIds[0];

            foreach (var pRow in GetUniqRefDuplicates(personA).ToList())
            {
                UpdateUniqueRefs2(pRow.Person_id, Guid.NewGuid(), 1, 1);
            }
        }
    }
}
