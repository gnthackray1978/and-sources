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
    public class PersonDal :BaseBll, IPersonDal
    {
        private readonly SourceDal _sourceDal;
        private readonly SourceMappingsDal _sourceMappingsDal;
        private readonly ParishsDal _parishsBll;
        private readonly RelationsDal _relationsDal;

        public PersonDal()
        {
            _sourceDal = new SourceDal();
            _sourceMappingsDal = new SourceMappingsDal();
            _parishsBll = new ParishsDal();
            _relationsDal = new RelationsDal();
        }

        public void Delete(Guid deathBirthRecId)
        {


            Person person = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == deathBirthRecId);

            if (person != null)
            {
                person.IsDeleted = true;
                ModelContainer.SaveChanges();
            }



        }
           
        public void Update(ServicePerson person)
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

                if (person.Events!= null)
                    personEntity.TotalEvents = person.Events.ToInt32();
            
                personEntity.UniqueRef = person.UniqueReference.ToGuid();
          

            }

            ModelContainer.SaveChanges();


        }
    
        private void UpdateLocationId(Guid personId, Guid locationId)
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

        private void UpdateLocation(Guid personId, string location)
        {

            var person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            if (person == null) return;

            person.BirthLocation = location;

            ModelContainer.SaveChanges();
        }

        private void UpdateUniqueRef(Guid personId, Guid uniqueRef, int totalEvents, int eventPriority)
        {

            var person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            if (person == null) return;

            person.UniqueRef = uniqueRef;
            person.TotalEvents = totalEvents;
            person.EventPriority = eventPriority;

            ModelContainer.SaveChanges();
        }

        public void UpdateDuplicateRefs(Guid person1, Guid person2)
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
     
        public Guid Insert(ServicePerson person)
        {
            //todo fix the service person so it cant contain nulls!!

            var personEntity = new Person
                {
                    IsMale = person.IsMale.ToBool(),
                    ChristianName = person.ChristianName ?? "",
                    Surname = person.Surname ?? "",
                    BirthLocation = person.BirthLocation ?? "",
                    BirthDateStr = person.Birth ?? "",
                    BaptismDateStr = person.Baptism ?? "",
                    DeathDateStr = person.Death ?? "",
                    ReferenceDateStr = person.ReferenceDate ?? "",
                    DeathLocation = person.DeathLocation ?? "",
                    FatherChristianName = person.FatherChristianName ?? "",
                    FatherSurname = person.FatherSurname ?? "",
                    MotherChristianName = person.MotherChristianName ?? "",
                    MotherSurname = person.MotherSurname ?? "",
                    Notes = person.Notes ?? "",
                    Source = person.SourceDescription ?? "",
                    BapInt = person.BaptismYear,
                    BirthInt = person.BirthYear,
                    DeathInt = person.DeathYear,
                    ReferenceDateInt =person.ReferenceDate.ParseToValidYear(),// DateTools.GetDateYear(),
                    ReferenceLocation = person.ReferenceLocation ?? "",
                    BirthCounty = person.BirthCounty ?? "",
                    DeathCounty = person.DeathCounty ?? "",
                    Occupation = person.Occupation ?? "",
                    FatherOccupation = person.FatherOccupation ?? "",
                    SpouseName = person.SpouseChristianName ?? "",
                    SpouseSurname = person.SpouseSurname ?? "",
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
                    OthersideChristianName = person.OthersideChristianName,
                    OthersideSurname = person.OthersideSurname,
                    OthersideRelationship = person.OthersideRelationship,
                    Person_id = Guid.NewGuid(),
                    DateAdded = DateTime.Today,
                    DateLastEdit = DateTime.Today
                };


            ModelContainer.Persons.Add(personEntity);

            ModelContainer.SaveChanges();

            person.PersonId = personEntity.Person_id;
            return personEntity.Person_id;
        }
  
        public ServicePerson Get(Guid personId)
        {
            var result = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == personId && !o.IsDeleted);
            
            return result.ToServicePerson();


        }
        
        public IList<ServicePerson> GetByDupeRef(Guid dupeRef)
        {

            var persons = ModelContainer.Persons.Where(o => o.UniqueRef == dupeRef && o.IsDeleted == false).ToList();

            return persons.ToServicePersons();


        }
 
        private IEnumerable<Person> GetUniqRefDuplicates(Guid personId)
        {
            var person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            return person != null ? ModelContainer.Persons.Where(o => o.UniqueRef == person.UniqueRef && o.IsDeleted == false) : null;
        }
       
        public List<ServicePerson> GetByFilter(PersonSearchFilter personSearchFilter)
        {

            Guid parishId = personSearchFilter.ParishString.ToGuid();
           
            if (parishId != Guid.Empty)
            {
                var sourceBll = new SourceDal();

                sourceBll.GetParishSourceRecords(parishId).ForEach(p => personSearchFilter.SourceString += "," + p.SourceId.ToString());

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
                                                personSearchFilter.UpperDate, personSearchFilter.County, personSearchFilter.SpouseChristianName,personSearchFilter.OthersideChristianName,personSearchFilter.OthersideSurname,personSearchFilter.OthersideRelationship, personSearchFilter.IsIncludeSources).Select(p => new ServicePerson
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
                                                                                           SourceId = p.RefSourceId,
                                                                                           OthersideChristianName = p.OthersideChristianName,
                                                                                           OthersideSurname = p.OthersideSurname,
                                                                                           OthersideRelationship = p.OthersideRelationship,
                                                                                           ReferenceDate = p.ReferenceDateStr,
                                                                                           ReferenceLocation = p.ReferenceLocation,
                                                                                           ReferenceYear = p.ReferenceDateInt
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
         
        public void UpdateUniqueRefs()
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
                    UpdateUniqueRef(id, newRef, peopleToKeep.Count, evtCount);
                    evtCount++;
                }

                newRef = Guid.NewGuid();
                UpdateUniqueRef(pROw.Person_id, newRef, 1, 1);
            }



        }

        public void MergeDuplicateRecords(Guid personId)
        {


            var record = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == personId);

            MergeDuplicateRecord(record);
        }

        private void MergeDuplicateRecord(Person newRecord)
        {
            var sourceList = new List<Guid>();

            if (newRecord != null)
            {

                foreach (var dupePerson in GetUniqRefDuplicates(newRecord.Person_id))
                {
                    sourceList.AddRange(
                        _sourceDal.FillSourceTableByPersonOrMarriageId2(dupePerson.Person_id).Select(dp => dp.SourceId).
                            ToList());

                    newRecord.MergeInto(dupePerson, _sourceDal);
                }

                _sourceMappingsDal.WritePersonSources2(newRecord.Person_id, sourceList, 1);

                ModelContainer.SaveChanges();
            }


        }

        private void TidyPersonLocations()
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

                UpdateLocation(prow.Person_id, newLocation);
            }
        }
 
        public void UpdateLocationIdsFromParishTable()
        {

            TidyPersonLocations();

            var dummyLocationdId = new Guid("A813A1FF-6093-4924-A7B2-C5D1AF6FF699");

            List<string> counties = ModelContainer.Persons.Where(p => p.BirthLocationId == dummyLocationdId).Select(o => o.BirthCounty).Distinct().ToList();
           
            foreach (string county in counties)
            {
                string county1 = county;
                foreach (var personRec in ModelContainer.Persons.Where(p => p.BirthLocationId == dummyLocationdId).Where(o => o.BirthCounty == county1))
                {
                    var var2 = _parishsBll.GetParishsByCounty2(county).FirstOrDefault(p => personRec.BirthLocation.Contains(p.ParishName));

                    if (var2 != null)
                    {
                        UpdateLocationId(personRec.Person_id, var2.ParishId);
                      
                    }

                    var var3 = GetEntryByLocatAndCounty(personRec.BirthLocation, county);
                    if (var3 != null)
                    {
                        if (var3.ParishId != null) UpdateLocationId(personRec.Person_id, var3.ParishId.Value);
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

                Update(new ServicePerson
                {
                    PersonId = pRow.Person_id, 
                    IsMale = pRow.IsMale.ToString(),
                    ChristianName  = pRow.ChristianName,
                    Surname = pRow.Surname,
                    BirthLocation  = pRow.BirthLocation, 
                    Birth  = pRow.BirthDateStr,
                    Baptism =pRow.BaptismDateStr, 
                    Death  = pRow.DeathDateStr,
                    ReferenceDate  = pRow.ReferenceDateStr,
                    DeathLocation = pRow.DeathLocation,
                    FatherChristianName  = pRow.FatherChristianName, 
                    FatherSurname  = pRow.FatherSurname,
                    MotherChristianName  = pRow.MotherChristianName, 
                    MotherSurname  = pRow.MotherSurname,
                    Notes  = pRow.Notes,
                    Sources   = pRow.Source,
                    BaptismYear  = pRow.BapInt,
                    BirthYear  = pRow.BirthInt, 
                    DeathYear  = pRow.DeathInt,
                    ReferenceYear  = pRow.ReferenceDateInt,
                    ReferenceLocation  = pRow.ReferenceLocation,
                    BirthCounty = pRow.BirthCounty, 
                    DeathCounty  = pRow.DeathCounty, 
                    Occupation   = pRow.Occupation,
                    FatherOccupation  = pRow.FatherOccupation, 
                    SpouseChristianName  = pRow.SpouseName,
                    SpouseSurname  = pRow.SpouseSurname,
                    UserId  = pRow.UserId,
                    BirthLocationId  = pRow.BirthLocationId.ToString(),
                    DeathLocationId  = pRow.DeathLocationId.ToString(), 
                    ReferenceLocationId  = pRow.ReferenceLocationId.ToString(),
                    Events  = pRow.TotalEvents.ToString(),                   
                    UniqueReference  = pRow.UniqueRef.GetValueOrDefault().ToString()              
                });

                

            }

        }

        public List<Guid> DelinkPersons(List<Guid> selectedRecordIds)
        {
            Guid personA = selectedRecordIds[0];


            _relationsDal.DeleteRelationMapping(selectedRecordIds);


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
                UpdateUniqueRef(id, newRef, peopleToKeep.Count, evtCount);
                evtCount++;
            }

            var parentGuids = new List<Guid> {newRef};

            //SetParentRecordIds(parentGuids);

            // records to remove done here
            foreach (Guid personToRemove in selectedRecordIds)
            {
                newRef = Guid.NewGuid();
                UpdateUniqueRef(personToRemove, newRef, 1, 1);
            }


            if (peopleToKeep.Count == 0)
            {
                parentGuids.Clear();
                parentGuids.Add(newRef);

            }


            return parentGuids;
        }

        public LocationDictionary GetEntryByLocatAndCounty(string locat, string county)
        {
            LocationDictionary retVal = ModelContainer.LocationDictionaries.FirstOrDefault(o => o.LocationName == locat && o.LocationCounty == county);

            return retVal;

        }
    }
}
