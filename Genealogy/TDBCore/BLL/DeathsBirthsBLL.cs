using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using TDBCore.Datasets.DsDeathsBirthsTableAdapters;
////using TDBCore.Datasets;
using GedItter.BLL;
using System.Data.SqlClient;
 
using TDBCore.EntityModel;
using System.Diagnostics;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data;
using TDBCore.BLL;
using TDBCore.Types;



namespace GedItter.BirthDeathRecords.BLL
{
    public class DeathsBirthsBLL :BaseBLL
    {

 
        public DeathsBirthsBLL()
        {
           
         
           
        }
        /// <summary>
        /// takes sourceid of tree and returns list of people in it
        /// </summary>
        public List<ServicePersonLookUp> GetPersonsForTree(string sourceId, string start, string end)
        {
            List<ServicePersonLookUp> personsInTree = new List<ServicePersonLookUp>();

            Guid sourceGuid = sourceId.ToGuid();

            DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();

            int startInt = start.ToInt32();
            int endInt = end.ToInt32();

            personsInTree = deathsBirthsBLL.GetBySourceId2(sourceGuid).Where(ss => ss.BirthInt >= startInt
                && ss.BirthInt <= endInt).Select(s => new ServicePersonLookUp()
                {
                    PersonId = s.Person_id,
                    ChristianName = s.ChristianName,
                    Surname = s.Surname,
                    BirthYear = s.BirthInt,
                    BirthLocation = s.BirthLocation,
                    XREF = s.OrigSurname

                }).OrderBy(o => o.Surname).ThenBy(t => t.BirthYear).ToList();

            return personsInTree;
        }



        public void TrimPersonNames()
        {
            int count = this.ModelContainer.Persons.Count();
            
            int idx = 0;
            foreach (Person _person in this.ModelContainer.Persons)
            {
               // _person.ChristianName = _person.ChristianName.Trim();
              //  _person.Surname = _person.Surname.Trim();

                Console.WriteLine(idx.ToString()+""+count.ToString());
                _person.ChristianName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(_person.ChristianName.Trim().ToLower());
                _person.Surname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(_person.Surname.Trim().ToLower());
                idx++;
            }

            this.ModelContainer.SaveChanges();
        }


        /// <summary>
        /// Delete records from relations table that dont exist in persons table.
        /// </summary>
        public void DeleteOrphanRelations()
        {
            ModelContainer.DeleteRelationOrphans();
        }


        /// <summary>
        /// Delete all entries from persons table for this particular source.
        /// </summary>
        public void DeletePersonsFromSource(Guid sourceId)
        {
            ModelContainer.DeletePersonsForSource(sourceId);
        }



        public void DeleteDeathBirthRecord2(Guid deathBirthRecId)
        {


            Person _person = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == deathBirthRecId);

            if (_person != null)
            {
                _person.IsDeleted = true;
                ModelContainer.SaveChanges();
            }



        }
 


        #region update

        public void ClearUniqueRefs()
        {
            SqlConnection SQLConnection = GetConnection();
            string updStr = "Update Persons Set UniqueRef = '', TotalEvents = 1, EventPriority = 1 ";

            SqlCommand sqlCommand = new SqlCommand(updStr, SQLConnection);

            if (SQLConnection.State != System.Data.ConnectionState.Open)
                SQLConnection.Open();

            sqlCommand.ExecuteNonQuery();

            SQLConnection.Close();
            SQLConnection.Dispose();



        }

       



        #region old

        //public void UpdateBirthDeathRecord(Guid personId, bool isMale,
        //                                                        string cName,
        //                                                        string sName,
        //                                                        string bLocation,
        //                                                        string bDate,
        //                                                        string bapDate,
        //                                                        string detDate,
        //                                                        string refDate,
        //                                                        string dLocation,
        //                                                        string fatherCName,
        //                                                        string fatherSName,
        //                                                        string motherCName,
        //                                                        string motherSName,
        //                                                        string notes,
        //                                                        string source,
        //                                                        int bapInt,
        //                                                        int birtInt,
        //                                                        int detInt,
        //                                                        int refInt,
        //                                                        string referenceLocation,
        //                                                        string bCounty,
        //                                                        string dCounty,
        //                                                        string occupation,
        //                                                        string fatherOccupation,
        //                                                        string spouseCName,
        //                                                        string spouseSName,
        //                                                        int userId,
        //                                                        Guid birthLocationId,
        //                                                        Guid deathLocationId,
        //                                                        Guid referenceLocationId,
        //                                                        int totalEvents,
        //                                                        int eventPriority,
        //                                                        string uniqueRef,
        //                                                        int estBirthInt,
        //                                                        int estDeathInt,
        //                                                        bool isEstBirth,
        //                                                        bool isEstDeath,
        //                                                        string origName,
        //                                                        string origFatherName,
        //                                                        string origMotherName)
        //{
        //    Adapter.Update(0, 0, isMale, cName, sName, bLocation, dLocation, fatherCName, fatherSName, motherCName, motherSName,
        //        notes, source, birtInt, bapInt, detInt, bDate, bapDate, detDate, dCounty, bCounty, personId, occupation, referenceLocation,
        //        refDate, refInt, spouseCName, spouseSName, fatherOccupation,
        //        birthLocationId, userId, deathLocationId, referenceLocationId, uniqueRef,
        //        totalEvents, eventPriority, estBirthInt, estDeathInt, isEstBirth, isEstDeath, origName, origFatherName, origMotherName, false, personId);


        //}
        #endregion



        public void UpdatePersonSpouseName(Guid personId,  string scName, string ssName )
        {
            Person _person = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == personId);

            _person.SpouseName = scName;
            _person.SpouseSurname = ssName;

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
            Person _person = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == personId);

            if (_person != null)
            {

                _person.IsMale = isMale;
                _person.ChristianName = cName;
                _person.Surname = sName;
                _person.BirthLocation = bLocation;
                _person.BirthDateStr = bDate;
                _person.BaptismDateStr = bapDate;
                _person.DeathDateStr = detDate;
                _person.ReferenceDateStr = refDate;
                _person.DeathLocation = dLocation;
                _person.FatherChristianName = fatherCName;
                _person.FatherSurname = fatherSName;
                _person.MotherChristianName = motherCName;
                _person.MotherSurname = motherSName;
                _person.Notes = notes;
                _person.Source = source;
                _person.BapInt = bapInt;
                _person.BirthInt = birtInt;
                _person.DeathInt = detInt;
                _person.ReferenceDateInt = refInt;
                _person.ReferenceLocation = referenceLocation;
                _person.BirthCounty = bCounty;
                _person.DeathCounty = dCounty;
                _person.Occupation = occupation;
                _person.FatherOccupation = fatherOccupation;
                _person.SpouseName = spouseCName;
                _person.SpouseSurname = spouseSName;
                _person.UserId = userId;
                _person.BirthLocationId = birthLocationId;
                _person.DeathLocationId = deathLocationId;
                _person.ReferenceLocationId = referenceLocationId;
                _person.TotalEvents = totalEvents;
                _person.EventPriority = eventPriority;
                _person.UniqueRef = uniqueRef;
                _person.EstBirthYearInt = estBirthInt;
                _person.EstDeathYearInt = estDeathInt;
                _person.IsEstBirth = isEstBirth;
                _person.IsEstDeath = isEstDeath;
                _person.OrigSurname = origName;
                _person.OrigFatherSurname = origFatherName;
                _person.OrigMotherSurname = origMotherName;

            }
            
            ModelContainer.SaveChanges();


        }



        #region old
        public void UpdateBirthLocationId(Guid personId, Guid locationId)
        {

            var _person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);



            SqlConnection SQLConnection = GetConnection();

            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Persons SET BirthLocationId = @birthLocationId WHERE Person_id = @personId", SQLConnection);
                cmd.CommandType = CommandType.Text;
                SqlParameter myParm1 = cmd.Parameters.Add("@birthLocationId", SqlDbType.UniqueIdentifier);
                myParm1.Value = locationId;
                SqlParameter myParm2 = cmd.Parameters.Add("@personId", SqlDbType.UniqueIdentifier);
                myParm2.Value = personId;
                if (SQLConnection.State != System.Data.ConnectionState.Open)
                    SQLConnection.Open();

                cmd.ExecuteNonQuery();
            }
            finally
            {
                SQLConnection.Close();
                SQLConnection.Dispose();
            }


        }
        #endregion

        public void UpdateBirthLocationId2(Guid personId, Guid locationId)
        {

            var _person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            if (_person != null)
            {
                _person.BirthLocationId = locationId;

                ModelContainer.SaveChanges();
            }

        }

        #region old
        //public void UpdateBirthLocation(Guid personId, string location)
        //{

        //    try
        //    {


        //        Adapter.Connection.Open();
        //        SqlCommand cmd = new SqlCommand(
        //            "UPDATE Persons SET BirthLocation = @birthLocation WHERE Person_id = @personId", Adapter.Connection);
        //        cmd.CommandType = CommandType.Text;

        //        SqlParameter myParm1 = cmd.Parameters.Add("@birthLocation", SqlDbType.NVarChar,500);
        //        myParm1.Value = location;

        //        SqlParameter myParm2 = cmd.Parameters.Add("@personId", SqlDbType.UniqueIdentifier);
        //        myParm2.Value = personId;


        //        cmd.ExecuteNonQuery();
        //        Adapter.Connection.Close();


        //    }
        //    finally
        //    {
        //        if (Adapter.Connection != null)
        //        {
        //            Adapter.Connection.Close();
        //        }

        //    }


        //}
        #endregion


        public void UpdateBirthLocation2(Guid personId, string location)
        {

            var _person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            if (_person != null)
            {
                _person.BirthLocation = location;

                ModelContainer.SaveChanges();
            }

        }

        #region old
        //public void UpdateBirthCounty(Guid personId, string county)
        //{

        //    try
        //    {


        //        Adapter.Connection.Open();
        //        SqlCommand cmd = new SqlCommand(
        //            "UPDATE Persons SET BirthCounty = @county WHERE Person_id = @personId", Adapter.Connection);
        //        cmd.CommandType = CommandType.Text;

        //        SqlParameter myParm1 = cmd.Parameters.Add("@county", SqlDbType.NVarChar,500);
        //        myParm1.Value = county;

        //        SqlParameter myParm2 = cmd.Parameters.Add("@personId", SqlDbType.UniqueIdentifier);
        //        myParm2.Value = personId;


        //        cmd.ExecuteNonQuery();
        //        Adapter.Connection.Close();


        //    }
        //    finally
        //    {
        //        if (Adapter.Connection != null)
        //        {
        //            Adapter.Connection.Close();
        //        }

        //    }

        //}
        #endregion

        public void UpdateBirthCounty2(Guid personId, string county)
        {
            var _person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            if (_person != null)
            {
                _person.BirthCounty = county;

                ModelContainer.SaveChanges();
            }



        }

        #region old
        //public void UpdateSources(Guid personId, string source, string desc)
        //{
         

        //    Adapter.UpdateSources(desc, source, personId);

        //}
        #endregion


        public void UpdateSources2(Guid personId, string source, string desc)
        {
            //      Notes = @Notes, Source = @Source
            var _person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            if (_person != null)
            {
                _person.Notes = desc;
                _person.Source = source;

                ModelContainer.SaveChanges();
            }


           // Adapter.UpdateSources(desc, source, personId);

        }

        #region old
        //public void UpdateUniqueRefs(Guid personId, string uniqueRef, int totalEvents, int eventPriority)
        //{
        //    SqlConnection SQLConnection = GetConnection();
        //    string updStr = "Update Persons Set UniqueRef = '" + uniqueRef + "', TotalEvents = "
        //        + totalEvents.ToString() + ", EventPriority = " + eventPriority.ToString() + " WHERE Person_id = '" + personId.ToString() + "'";

        //    SqlCommand sqlCommand = new SqlCommand(updStr, SQLConnection);

        //    if (SQLConnection.State != System.Data.ConnectionState.Open)
        //        SQLConnection.Open();

        //    sqlCommand.ExecuteNonQuery();

        //    SQLConnection.Close();
        //    SQLConnection.Dispose();
        //}
        #endregion


        public void UpdateUniqueRefs2(Guid personId, Guid uniqueRef, int totalEvents, int eventPriority)
        {

            var _person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            if (_person != null)
            {
                _person.UniqueRef = uniqueRef;
                _person.TotalEvents = totalEvents;
                _person.EventPriority = eventPriority;

                ModelContainer.SaveChanges();
            }

        }

        #region old
       // public static void UpdateDuplicateRefs(Guid person1, Guid person2)
       // {
       //     //if (!IsValidEdit()) return;

       //     // ok the plan is:

       //     // create a list of rows to update in the database

       //     //  BirthsDeathsDLL birthsDeathsDll = new BirthsDeathsDLL();
       //     DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();
       //     //   BirthsDeathsDLL deathsBirthsDLL = new BirthsDeathsDLL();

       // //    DsDeathsBirths.PersonsDataTable dsDeathsBirths = new DsDeathsBirths.PersonsDataTable();



       ////     DsDeathsBirths.PersonsDataTable dsDBTemp = new DsDeathsBirths.PersonsDataTable();


       //     List<Guid> effectedPeople = new List<Guid>();
       //     List<string> uniqRefs = new List<string>();


       //     // get list of unique refs if they exist for the 2 persons

       //     var dsDBTemp = deathsBirthsBLL.GetDeathBirthRecordById2(person1).Where(p=>p.UniqueRef != "").FirstOrDefault();

       //     if (dsDBTemp != null)
       //     {
       //         uniqRefs.Add(dsDBTemp.UniqueRef);
       //     }
       //     else
       //     {
       //         effectedPeople.Add(person1);
       //     }

       //     dsDBTemp = deathsBirthsBLL.GetDeathBirthRecordById2(person2).Where(p => p.UniqueRef != "").FirstOrDefault();

       //     if (dsDBTemp != null)
       //     {
       //         uniqRefs.Add(dsDBTemp.UniqueRef);
       //     }
       //     else
       //     {
       //         if (!effectedPeople.Contains(person2))
       //             effectedPeople.Add(person2);
       //     }

       //     // find all people that have the same unique refs as our duplicates


       //     foreach (string _uniqRef in uniqRefs)
       //     {
       //        // dsDBTemp = deathsBirthsBLL.GetDataByDupeRef(_uniqRef);
       //         foreach (var _personsRow in deathsBirthsBLL.GetDataByDupeRef2(_uniqRef))
       //         {
       //             if (!effectedPeople.Contains(_personsRow.Person_id))
       //                 effectedPeople.Add(_personsRow.Person_id);
       //         }
       //     }


       //     Guid newRef = Guid.NewGuid();

       //     int evtCount = 1;
       //     foreach (Guid id_ in effectedPeople)
       //     {
       //         deathsBirthsBLL.UpdateUniqueRefs2(id_, newRef.ToString(), effectedPeople.Count, evtCount);
       //         evtCount++;
       //     }


       // }
        #endregion

        public void UpdateDuplicateRefs2(Guid person1, Guid person2)
        {
        
            List<Person> effectedPeople = new List<Person>();
            
           
            foreach (var _person in ModelContainer.Persons.Where(p => (p.Person_id == person2 || p.Person_id == person1) && p.UniqueRef == Guid.Empty))
            {
                effectedPeople.Add(_person);
            }

            
            // if our 2 people have unique refs fetch them
            foreach (var _uniqRef in ModelContainer.Persons.Where(p => (p.Person_id == person2 || p.Person_id == person1) && p.UniqueRef != Guid.Empty))
            {       
                foreach (var _dupePerson in ModelContainer.Persons.Where(p => p.UniqueRef == _uniqRef.UniqueRef))
                {
                    if (!effectedPeople.Contains(_dupePerson))
                        effectedPeople.Add(_dupePerson);
                }
            }


            Guid newRef = Guid.NewGuid();

            int evtCount = 1;
            foreach (Person _effectedPerson in effectedPeople)
            {
               
                _effectedPerson.UniqueRef = newRef;
                _effectedPerson.TotalEvents = effectedPeople.Count;
                _effectedPerson.EventPriority = evtCount;
                evtCount++;
            }


           // ModelContainer.AcceptAllChanges();

            ModelContainer.SaveChanges();

        }

        #endregion


        #region insert
 
        public Guid InsertPerson(Person _person)
        {
            _person.Person_id = Guid.NewGuid();
     

            if (_person.BaptismDateStr == null) _person.BaptismDateStr = "";
            if (_person.BirthCounty == null) _person.BirthCounty = "";
            if (_person.BirthDateStr == null) _person.BirthDateStr = "";
            if (_person.BirthLocation == null) _person.BirthLocation = "";
            if (_person.BirthLocationId == null) _person.BirthLocationId = Guid.Empty;
            if (_person.ChristianName == null) _person.ChristianName = "";
            if (_person.DateAdded == null) _person.DateAdded = DateTime.Today;
            if (_person.DateLastEdit == null) _person.DateLastEdit = DateTime.Today;
            if (_person.DeathCounty == null) _person.DeathCounty = "";
            if (_person.DeathDateStr == null) _person.DeathDateStr = "";
            if (_person.DeathLocation == null) _person.DeathLocation = "";
            if (_person.FatherChristianName == null) _person.FatherChristianName = "";
            if (_person.FatherOccupation == null) _person.FatherOccupation = "";
            if (_person.FatherSurname == null) _person.FatherSurname = "";
            if (_person.MotherChristianName == null) _person.MotherChristianName = "";
            if (_person.MotherSurname == null) _person.MotherSurname = "";
            if (_person.Notes == null) _person.Notes = "";
            if (_person.Occupation == null) _person.Occupation = "";
            if (_person.OrigFatherSurname == null) _person.OrigFatherSurname = "";
            if (_person.OrigMotherSurname == null) _person.OrigMotherSurname = "";
            if (_person.OrigSurname == null) _person.OrigSurname = "";
            if (_person.ReferenceDateStr == null) _person.ReferenceDateStr = "";
            if (_person.ReferenceLocation == null) _person.ReferenceLocation = "";
            if (_person.ReferenceLocationId == null) _person.ReferenceLocationId = Guid.Empty;
            if (_person.Source == null) _person.Source = "";
            if (_person.SpouseName == null) _person.SpouseName = "";
            if (_person.SpouseSurname == null) _person.SpouseSurname = "";
            if (_person.Surname == null) _person.Surname = "";
            if (_person.UniqueRef == null) _person.UniqueRef = Guid.Empty;
            if (_person.UserId == 0) _person.UserId = 1;
            if (_person.TotalEvents == 0) _person.TotalEvents = 1;
            if (_person.EventPriority == 0) _person.EventPriority = 1;



            ModelContainer.Persons.AddObject(_person);

            ModelContainer.SaveChanges();

            return _person.Person_id;
        }

        public Person CreateBasicPerson(string cname, string sname, string blocat, int birthyear)
        {
            Person _person = new Person();
            _person.ChristianName = cname;
            _person.Surname = sname;
            _person.BirthDateStr = birthyear.ToString();
            _person.BirthLocation = blocat;

            InsertPerson(_person);

            return _person;
        }


        public Guid InsertDeathBirthRecord2(bool isMale, string cName, string sName, string bLocation,
                                           string bDate, string bapDate, string detDate, string dLocation, string fCName, string fSName,
                                           string mCName, string mSName, string source, string notes,
                                           int birthInt, int bapInt, int detInt, string birthCounty, string deathCounty
                                           , string occupation, string referenceLocation, string referenceDateString
                                           , int referenceDateInt, string spouseCName, string spouseSName, string fatherOccupation,
                                           Guid birthLocationId, int userId, Guid deathLocationId, Guid referenceLocationId,
                                           int totalEvents,
                                           int eventPriority,
                                           Guid uniqueRef,
                                           int estBirthInt,
                                           int estDeathInt,
                                           bool isEstBirth,
                                           bool isEstDeath,
                                           string origSurname,
                                           string origFatherSurname,
                                           string origMotherSurname)
        {

            Person _person = new Person();
            _person.IsMale = isMale;
            _person.ChristianName = cName;
            _person.Surname = sName;
            _person.BirthLocation = bLocation;
            _person.BirthDateStr = bDate;
            _person.BaptismDateStr = bapDate;
            _person.DeathDateStr = detDate;
            _person.ReferenceDateStr = referenceDateString;
            _person.DeathLocation = dLocation;
            _person.FatherChristianName = fCName;
            _person.FatherSurname = fSName;
            _person.MotherChristianName = mCName;
            _person.MotherSurname = mSName;
            _person.Notes = notes;
            _person.Source = source;
            _person.BapInt = bapInt;
            _person.BirthInt = birthInt;
            _person.DeathInt = detInt;
            _person.ReferenceDateInt = referenceDateInt;
            _person.ReferenceLocation = referenceLocation;
            _person.BirthCounty = birthCounty;
            _person.DeathCounty = deathCounty;
            _person.Occupation = occupation;
            _person.FatherOccupation = fatherOccupation;
            _person.SpouseName = spouseCName;
            _person.SpouseSurname = spouseSName;
            _person.UserId = userId;
            _person.BirthLocationId = birthLocationId;
            _person.DeathLocationId = deathLocationId;
            _person.ReferenceLocationId = referenceLocationId;
            _person.TotalEvents = totalEvents;
            _person.EventPriority = eventPriority;
            _person.UniqueRef = uniqueRef;
            _person.EstBirthYearInt = estBirthInt;
            _person.EstDeathYearInt = estDeathInt;
            _person.IsEstBirth = isEstBirth;
            _person.IsEstDeath = isEstDeath;
            _person.OrigSurname = origSurname;
            _person.OrigFatherSurname = origFatherSurname;
            _person.OrigMotherSurname = origMotherSurname;
            _person.Person_id = Guid.NewGuid();



            ModelContainer.Persons.AddObject(_person);

            ModelContainer.SaveChanges();

            return _person.Person_id;
        }
        #endregion




        #region selects

        public IQueryable<Person> GetDeathBirthRecordByIds(List<Guid> personIds)
        {
            return ModelContainer.Persons.Where( p=> personIds.Contains(p.Person_id));
        }

        public IQueryable<Person> GetDeathBirthRecordById2(Guid personId)
        {
            return ModelContainer.Persons.Where(o => o.Person_id == personId && !o.IsDeleted );
        }

        public IQueryable<Person> GetDeathsBirths2()
        { 
            return ModelContainer.Persons;
        }
   
        public IQueryable<Person> GetDataByDupeRef2(Guid dupeRef)
        {
            return ModelContainer.Persons.Where(o => o.UniqueRef == dupeRef && o.IsDeleted == false);
        }

        public IQueryable<Person> GetUniqRefDuplicates(Guid personId)
        {
            Person _person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            if (_person != null)
            {
                return ModelContainer.Persons.Where(o => o.UniqueRef == _person.UniqueRef && o.IsDeleted == false);
            }
            else
            {
                return null;
            }

        }

        public string MakeSourceString(Guid _person)
        {
            SourceBLL sourceBll = new SourceBLL();
            string sourceStr = "";
            foreach (var _source in sourceBll.FillSourceTableByPersonOrMarriageId2(_person))
            {
                sourceStr += Environment.NewLine + _source.SourceRef;
            }

            return sourceStr;

        }

        public IQueryable<Person> GetRelationDeathBirthRecord2(Guid personId)
        {

            // there are some duplicate entries in the relations table which 
            // can cause confusion.
            //todo 
            // remove them
            return ModelContainer.Persons.Where(o => o.RelationB.Any(r => r.PersonA.Person_id == personId));

        }
 
        public IQueryable<Person> GetFilterDeathBirthRecord2(string cName, string sName, string bLocation,
                                            int bDateLower, int bDateUpper, int bapDateLower, int bapDateUpper, int detDateLower, int detDateUpper, string dLocation, string fCName, string fSName,
                                            string mCName, string mSName, string source, string birthCounty, string deathCounty, string spouseCName, string spouseSName, string fatherOccupation)
        {
            #region format location variables
            string bLocation2 = bLocation;
            string bLocation3 = bLocation;
            string bLocation4 = bLocation;
            string bLocation5 = bLocation;

            if (bLocation.Contains(";"))
            {


                string[] locations = bLocation.Split(';');

                bLocation2 = locations[0];
                bLocation3 = locations[0];
                bLocation4 = locations[0];
                bLocation5 = locations[0];

                if (locations.Length > 0) bLocation = locations[0];
                if (locations.Length > 1) bLocation2 = locations[1];
                if (locations.Length > 2) bLocation3 = locations[2];
                if (locations.Length > 3) bLocation4 = locations[3];
                if (locations.Length > 4) bLocation5 = locations[4];
            }

            string dLocation2 = dLocation;
            string dLocation3 = dLocation;
            string dLocation4 = dLocation;
            string dLocation5 = dLocation;

            if (dLocation.Contains(";"))
            {


                string[] locations = dLocation.Split(';');

                dLocation2 = locations[0];
                dLocation3 = locations[0];
                dLocation4 = locations[0];
                dLocation5 = locations[0];

                if (locations.Length > 0) dLocation = locations[0];
                if (locations.Length > 1) dLocation2 = locations[1];
                if (locations.Length > 2) dLocation3 = locations[2];
                if (locations.Length > 3) dLocation4 = locations[3];
                if (locations.Length > 4) dLocation5 = locations[4];
            }
            #endregion



            if (source.Trim() == "") source = "x";

            //dsdb = Adapter.GetDataByFilteredWithSources(null, cName, sName, dLocation, dLocation2, dLocation3, dLocation4, dLocation5,
            //    fCName, fSName, mCName, mSName, source, bLocation, bLocation2, bLocation3, bLocation4, bLocation5, bDateLower, bDateUpper,
            //    bapDateLower, bapDateUpper, detDateLower, detDateUpper, birthCounty, deathCounty, spouseCName, spouseSName, fatherOccupation);


            IQueryable<Person> dsdb = ModelContainer.GetPersWithSources(null, cName, sName, dLocation, dLocation2, dLocation3, dLocation4, dLocation5,
                fCName, fSName, mCName, mSName, source, bLocation, bLocation2, bLocation3, bLocation4, bLocation5, bDateLower, bDateUpper,
                bapDateLower, bapDateUpper, detDateLower, detDateUpper, birthCounty, deathCounty, spouseCName, spouseSName, fatherOccupation).AsQueryable();

            return dsdb;

        }

        public IQueryable<Person> GetFilterSimple2(string cName, string sName, string bLocation,
                                        int bLower, int bUpper, string fCName, string fSName, string mCName, string mSName, string source, string county)
        {
 
            if (source.Trim() == "") source = "x";
 
            IQueryable<Person> dsdb = ModelContainer.GetSimPersonWithSources(cName, sName, fCName, fSName, mCName, mSName, source, bLocation, bLower, bUpper, county).AsQueryable();

            return dsdb;

        }

        public IQueryable<Person> GetByLocation2(Guid deathLocationId, Guid birthLocationId, string birthLoc, string deathLocat
                                            , string birthCounty, string deathCounty)
        { 
            return ModelContainer.Persons.Where(p => p.BirthLocationId == birthLocationId && p.BirthLocation.Contains(birthLoc) && p.DeathLocation.Contains(deathLocat) && p.DeathCounty.Contains(deathCounty) && p.BirthCounty.Contains(birthCounty));
        }
 
        public IQueryable<Person>  GetByBirthLocationId2(Guid birthId)
        {
 
            return   ModelContainer.Persons.Where(p => p.BirthLocationId == birthId);

        }
         
        public IQueryable<Person> GetByMissingBirthCounty2()
        {
 
            return ModelContainer.Persons.Where(p => p.BirthLocation != "" && p.BirthCounty == "");

        }
         
        public IQueryable<Person> GetBySourceId2(Guid sourceId)
        {
            return ModelContainer.Persons.Where(s => s.SourceMappings.Any(o => o.Source.SourceId == sourceId));

        }
 
        public IQueryable<Person> GetByXRef2()
        {
            return ModelContainer.Persons.Where(s => s.OrigSurname.Contains("XREF"));
        }


        #endregion
    }
}
