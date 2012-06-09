using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.BirthDeathRecords;
using TDBCore.Types;
using NUnit.Framework;
using GedItter.BirthDeathRecords.BLL;
using TDBCore.EntityModel;
using GedItter.BLL;

namespace UnitTests
{
    public class PersonView : IDeathBirthEditorView
    {

        public PersonValidation personValidation = new PersonValidation();
        public IDeathBirthEditorControl iDeathBirthEditorControl = null;
        public IDeathBirthEditorModel iDeathBirthEditorModel = null;


        public PersonView()
        {
            iDeathBirthEditorModel = new DeathBirthEditorModel();
            iDeathBirthEditorControl = new DeathBirthEditorControl();

            if (iDeathBirthEditorModel != null)
                iDeathBirthEditorControl.SetModel(iDeathBirthEditorModel);

            if (iDeathBirthEditorControl != null)
                iDeathBirthEditorControl.SetView(this);
        }

        #region set validation

        public void ShowValidDeathDate(bool valid)
        {
            this.personValidation.ValidDeathDate = valid;
        }

        public void ShowValidBirthDate(bool valid)
        {
            this.personValidation.ValidBirthDate = valid;
        }

        public void ShowValidName(bool valid)
        {
            this.personValidation.ValidName = valid;
        }

        public void ShowValidLocation(bool valid)
        {
            this.personValidation.ValidLocation = valid;
        }

        public void ShowValidDeathLocation(bool valid)
        {
            this.personValidation.ValidDeathLocation = valid;
        }

        public void ShowValidReferenceLocation(bool valid)
        {
            this.personValidation.ValidReferenceLocation = valid;
        }

        public void ShowValidReferenceDate(bool valid)
        {
            this.personValidation.ValidReferenceDate = valid;
        }

        public void ShowValidSpouseCName(bool valid)
        {
            this.personValidation.ValidSpouseCName = valid;
        }

        public void ShowValidSpouseSName(bool valid)
        {
            this.personValidation.ValidSpouseSName = valid;
        }

        public void ShowValidFatherOccupation(bool valid)
        {
            this.personValidation.ValidFatherOccupation = valid;
        }

        public void ShowValidOccupation(bool valid)
        {
            this.personValidation.ValidOccupation = valid;
        }

        public void ShowValidBapDate(bool valid)
        {
            this.personValidation.ValidBapDate = valid;
        }

        public void ShowValidSurname(bool valid)
        {
            this.personValidation.ValidSurname = valid;
        }

        public void ShowValidFatherChristianName(bool valid)
        {
            this.personValidation.ValidFatherChristianName = valid;
        }

        public void ShowValidFatherSurname(bool valid)
        {
            this.personValidation.ValidFatherSurname = valid;
        }

        public void ShowValidMotherChristianName(bool valid)
        {
            this.personValidation.ValidMotherChristianName = valid;
        }

        public void ShowValidMotherSurname(bool valid)
        {
            this.personValidation.ValidMotherSurname = valid;
        }

        public void ShowValidBirthCountyLocation(bool valid)
        {
            this.personValidation.ValidBirthCountyLocation = valid;
        }

        public void ShowValidDeathCountyLocation(bool valid)
        {
            this.personValidation.ValidDeathCountyLocation = valid;
        }

        public void ShowValidBirthLocationId(bool valid)
        {
            this.personValidation.ValidBirthLocationId = valid;
        }

        public void ShowValidDeathLocationId(bool valid)
        {
            this.personValidation.ValidDeathLocationId = valid;
        }

        public void ShowValidReferenceLocationId(bool valid)
        {
            this.personValidation.ValidReferenceLocationId = valid;
        }

        public void ShowValidSource(bool valid)
        {
            this.personValidation.ValidSource = valid;
        }

        public void ShowValidNotes(bool valid)
        {
            this.personValidation.ValidNotes = valid;
        }

        public void ShowValidUniqueRef(bool valid)
        {
            this.personValidation.ValidUniqueRef = valid;
        }

        public void ShowValidOriginalName(bool valid)
        {
            this.personValidation.ValidOriginalName = valid;
        }

        public void ShowValidOriginalFatherName(bool valid)
        {
            this.personValidation.ValidOriginalFatherName = valid;
        }

        public void ShowValidOriginalMotherName(bool valid)
        {
            this.personValidation.ValidOriginalMotherName = valid;
        }

        #endregion


        public void Update<T>(T paramModel)
        {

        }
    }


    public class PersonFilterView : iDeathBirthFilterView
    {
       

        public bool InvalidLowerDeathDate { get; set; }
        public bool InvalidUpperDeathDate { get; set; }
        public bool InvalidUpperBirthDate { get; set; }
        public bool InvalidLowerBirthDate { get; set; }
        public IDeathBirthFilterControl iDeathBirthFilterControl = null;
        public IDeathBirthFilterModel iDeathBirthFilterModel = null;
        public event EventHandler ShowEditor;


        #region validation



        public void ShowInvalidUpperBoundDeathWarning(bool valid)
        {
            this.InvalidUpperDeathDate = valid;
        }


        public void ShowInvalidLowerBoundDeathWarning(bool valid)
        {
            this.InvalidLowerDeathDate = valid;
        }


        public void ShowInvalidUpperBoundBirthWarning(bool valid)
        {
            this.InvalidUpperBirthDate = valid;
        }


        public void ShowInvalidLowerBoundBirthWarning(bool valid)
        {
            this.InvalidLowerBirthDate = valid;
        }

        #endregion

        public PersonFilterView()
        {
            iDeathBirthFilterModel = new DeathBirthFilterModel();
            iDeathBirthFilterControl = new DeathBirthFilterControl();

            if (iDeathBirthFilterModel != null)
                iDeathBirthFilterControl.SetModel(iDeathBirthFilterModel);

            if (iDeathBirthFilterControl != null)
                iDeathBirthFilterControl.SetView(this);
        }


        public void SetFilterMode(DeathBirthFilterTypes param)
        {
            iDeathBirthFilterControl.RequestSetFilterMode(param);
        }

        public void SetParentIds(List<Guid> Ids)
        {
            iDeathBirthFilterControl.RequestSetParentRecordIds(Ids);
        }



        public void Update<T>(T paramModel)
        {
            
        }
    }


    [TestFixture]
    public class NUnitPersonFilter : TestData
    {
        PersonFilterView personFilterView = null;
        List<Person> newPersons = null;
        List<Guid> sourcesList = null;

        Person _person1 = null;
        Person _person2 = null;
        Person _person3 = null;
        Person _person4 = null;
        Person _person5 = null;
        Person _person6 = null;


        Source _source1 = null;
        Source _source2 = null;
        Source _source3 = null;

        [SetUp]
        public void SetUpPersonFilter()
        {

            personFilterView = new PersonFilterView();
            personFilterView.iDeathBirthFilterModel.SetIsSecurityEnabled(false);
            DeathsBirthsBLL personsBLL = new DeathsBirthsBLL();
            SourceBLL sourceBLL = new SourceBLL();
            SourceMappingsBLL smap = new SourceMappingsBLL();



            _person1 = personsBLL.CreateBasicPerson("testcname1", "testsname1", "testlocat", 1800);

            _person2 = personsBLL.CreateBasicPerson("testcname2", "testsname2", "testlocat", 1800);
            _person2.DeathLocation = "person 2 loc";

            _person3 = personsBLL.CreateBasicPerson("testcname3", "testsname3", "testlocat", 1800);
            _person3.Occupation = "person 3 occupation";

            _person4 = personsBLL.CreateBasicPerson("testcname4", "testsname4", "testlocat", 1800);
            _person5 = personsBLL.CreateBasicPerson("testcname5", "testsname5", "testlocat", 1800);
            _person6 = personsBLL.CreateBasicPerson("testcname6", "testsname6", "testlocat", 1800);


            _source1 = sourceBLL.CreateBasicSource("test_src1", 1800, "test_desc1");
            _source2 = sourceBLL.CreateBasicSource("test_src2", 1800, "test_desc2");
            _source3 = sourceBLL.CreateBasicSource("test_src3", 1800, "test_desc3");

            sourcesList = new List<Guid>();
            
            sourcesList.Clear();
            sourcesList.Add(_source1.SourceId);
            smap.WritePersonSources2(_person1.Person_id, sourcesList, 1);
            
            sourcesList.Clear();
            sourcesList.Add(_source2.SourceId);
            smap.WritePersonSources2(_person2.Person_id, sourcesList, 1);
            
            sourcesList.Clear();
            sourcesList.Add(_source3.SourceId);
            smap.WritePersonSources2(_person3.Person_id, sourcesList, 1);

            
        }

        [Test(Description = "CheckSavedData")]
        public void CheckSavedData()
        {
            Assert.IsNotNull(_person1);
            Assert.IsNotNull(_person2);
            Assert.IsNotNull(_person3);
            Assert.IsNotNull(_person4);
            Assert.IsNotNull(_person5);
            Assert.IsNotNull(_person6);

            //sourceMappingView1.iSourceMappingControl.
            // iControl.RequestSetSelectedIds((Guid)ListView1.DataKeys[ListView1.SelectedIndex].Values["Person_id"]);
        }


        [Test(Description = "TestGroupDuplicates")]
        public void TestGroupDuplicates()
        {
            DeathsBirthsBLL personsBLL = new DeathsBirthsBLL();


            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person1.Person_id);
            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person2.Person_id);
            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person3.Person_id);

            personFilterView.iDeathBirthFilterControl.RequestSetRelationTypeId(1);




            personsBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins, _person1);
            personsBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins, _person2);
            personsBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins, _person3);

            newPersons = personsBLL.ModelContainer.Persons.Where(p => p.UniqueRef == _person1.UniqueRef).OrderBy(po => po.EventPriority).ToList();

            Assert.IsTrue(newPersons.Count == 3);

            Assert.IsTrue(newPersons[0].EventPriority == 1);
            Assert.IsTrue(newPersons[1].EventPriority == 2);
            Assert.IsTrue(newPersons[2].EventPriority == 3);

            Assert.IsTrue(newPersons[0].TotalEvents == 3);

            // selecting the same id twice removes it
          //  personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person1.Person_id);
          //  personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person2.Person_id);

            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person3.Person_id);
            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person4.Person_id);
            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person5.Person_id);
            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person6.Person_id);

            personFilterView.iDeathBirthFilterControl.RequestSetRelationTypeId(1);

            personsBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins, _person1);



            newPersons = personsBLL.ModelContainer.Persons.Where(p => p.UniqueRef == _person1.UniqueRef).OrderBy(po => po.EventPriority).ToList();

            Assert.IsTrue(newPersons.Count == 6);

            Assert.IsTrue(newPersons[0].EventPriority == 1);
            Assert.IsTrue(newPersons[1].EventPriority == 2);
            Assert.IsTrue(newPersons[2].EventPriority == 3);
            Assert.IsTrue(newPersons[3].EventPriority == 4);
            Assert.IsTrue(newPersons[4].EventPriority == 5);
            Assert.IsTrue(newPersons[5].EventPriority == 6);

            Assert.IsTrue(newPersons[0].TotalEvents == 6);
        }


        [Test(Description = "TestMergeDuplicates")]
        public void TestMergeDuplicates()
        {
            SourceMappingsBLL smaps = new SourceMappingsBLL();
            DeathsBirthsBLL personsBLL = new DeathsBirthsBLL();

            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person1.Person_id);
            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person2.Person_id);
            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person3.Person_id);
            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person4.Person_id);
            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person5.Person_id);
            personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person6.Person_id);

            personFilterView.iDeathBirthFilterControl.RequestSetRelationTypeId(1);

            personsBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins, _person1);



            newPersons = personsBLL.ModelContainer.Persons.Where(p => p.UniqueRef == _person1.UniqueRef).OrderBy(po => po.EventPriority).ToList();


            Person _person = this.newPersons.FirstOrDefault(p => p.EventPriority == 1);

            Assert.IsNotNull(_person);

            DeathBirthFilterModel.MergeDuplicateRecord(_person);



            Assert.IsTrue(_person.Occupation == "person 3 occupation");
            Assert.IsTrue(_person.DeathLocation == "person 2 loc");


            List<string> sources = smaps.GetByMarriageIdOrPersonId2(_person.Person_id).Select(sm => sm.Source.SourceRef).ToList();

            Assert.IsTrue(sources.Count == 3);

            Assert.IsTrue(sources.Contains("test_src1"));
            Assert.IsTrue(sources.Contains("test_src2"));
            Assert.IsTrue(sources.Contains("test_src3"));
        }

        [TearDown]
        public void TearDown()
        {
            DeathsBirthsBLL personsBLL = new DeathsBirthsBLL();

            SourceMappingsBLL smaps = new SourceMappingsBLL();

            smaps.DeleteSourcesForPersonOrMarriage(_person1.Person_id);
            smaps.DeleteSourcesForPersonOrMarriage(_person2.Person_id);
            smaps.DeleteSourcesForPersonOrMarriage(_person3.Person_id);
            smaps.DeleteSourcesForPersonOrMarriage(_person4.Person_id);
            smaps.DeleteSourcesForPersonOrMarriage(_person5.Person_id);
            smaps.DeleteSourcesForPersonOrMarriage(_person6.Person_id);


            personsBLL.ModelContainer.Persons.DeleteObject(_person1);
            personsBLL.ModelContainer.Persons.DeleteObject(_person2);
            personsBLL.ModelContainer.Persons.DeleteObject(_person3);
            personsBLL.ModelContainer.Persons.DeleteObject(_person4);
            personsBLL.ModelContainer.Persons.DeleteObject(_person5);
            personsBLL.ModelContainer.Persons.DeleteObject(_person6);



 

            personsBLL.ModelContainer.Sources.DeleteObject(_source1);
            personsBLL.ModelContainer.Sources.DeleteObject(_source2);
            personsBLL.ModelContainer.Sources.DeleteObject(_source3);

            personsBLL.ModelContainer.SaveChanges();
        }
    }

    [TestFixture]
    public class NUnitPerson : TestData
    {
        #region validate properties

        //150
        [Test(Description = "InvalidBirthCountyLocation")]
        public void CheckInvalidCountyLocation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorBirthCountyLocation(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidBirthCountyLocation);
        }
        //150
        [Test(Description = "InvalidSpouseCName")]
        public void CheckInvalidSpouseCName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorSpouseCName(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidSpouseCName);
        }
        //150
        [Test(Description = "InvalidSpouseSName")]
        public void CheckInvalidSpouseSName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorSpouseSName(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidSpouseSName);
        }
        //150
        [Test(Description = "InvalidFatherOccupation")]
        public void CheckInvalidFatherOccupation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorFatherOccupation(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidFatherOccupation);



        }


        //150
        [Test(Description = "InvalidReferenceLocation")]
        public void CheckInvalidReferenceLocation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorReferenceLocation(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidReferenceLocation);

            //personView.iDeathBirthEditorControl.RequestSetEditorReferenceLocation("");
            //Assert.IsFalse(personView.iDeathBirthEditorModel.IsValidReferenceLocation);
        }
        //150
        [Test(Description = "InvalidOccupation")]
        public void CheckInvalidOccupation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorOccupation(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidOccupation);
        }


        //150
        [Test(Description = "InvalidChristianName")]
        public void CheckInvalidChristianName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorChristianName(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidName);

            personView.iDeathBirthEditorControl.RequestSetEditorChristianName("");
            Assert.IsFalse(personView.personValidation.ValidName);
        }


        //500
        [Test(Description = "InvalidSurnameName")]
        public void CheckInvalidSurnameName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorSurnameName(this.GetStringByLen(501));
            Assert.IsFalse(personView.personValidation.ValidSurname);

            personView.iDeathBirthEditorControl.RequestSetEditorSurnameName("");
            Assert.IsFalse(personView.personValidation.ValidSurname);
        }

        //150
        [Test(Description = "InvalidFatherChristianName")]
        public void CheckInvalidFatherChristianName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorFatherChristianName(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidFatherChristianName);



        }
        //500
        [Test(Description = "InvalidFatherSurname")]
        public void CheckInvalidFatherSurname()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorFatherSurname(this.GetStringByLen(501));
            Assert.IsFalse(personView.personValidation.ValidFatherSurname);



        }
        //150
        [Test(Description = "InvalidMotherChristianName")]
        public void CheckInvalidMotherChristianName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorMotherChristianName(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidMotherChristianName);

        }
        //500
        [Test(Description = "InvalidMotherSurname")]
        public void CheckInvalidMotherSurname()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorMotherSurname(this.GetStringByLen(501));
            Assert.IsFalse(personView.personValidation.ValidMotherSurname);
        }



        //150
        [Test(Description = "InvalidBirthCountyLocation")]
        public void CheckInvalidBirthCountyLocation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorBirthCountyLocation(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidBirthCountyLocation);
        }

        //150
        [Test(Description = "InvalidDeathCountyLocation")]
        public void CheckInvalidDeathCountyLocation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorDeathCountyLocation(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidDeathCountyLocation);

        }
        //500
        [Test(Description = "InvalidBirthLocation")]
        public void CheckInvalidBirthLocation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorBirthLocation(this.GetStringByLen(501));
            Assert.IsFalse(personView.personValidation.ValidLocation);

        }
        //500
        [Test(Description = "InvalidDeathLocation")]
        public void CheckInvalidDeathLocation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorDeathLocation(this.GetStringByLen(501));
            Assert.IsFalse(personView.personValidation.ValidDeathLocation);
        }


        //50
        [Test(Description = "InvalidSource")]
        public void CheckInvalidSource()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorSource(this.GetStringByLen(51));
            Assert.IsFalse(personView.personValidation.ValidSource);
        }
        //8000
        [Test(Description = "InvalidNotes")]
        public void CheckInvalidNotes()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorNotes(this.GetStringByLen(8001));
            Assert.IsFalse(personView.personValidation.ValidNotes);
        }
        //50
        //[Test(Description = "InvalidUniqueRef")]
        //public void CheckInvalidUniqueRef()
        //{
        //    PersonView personView = new PersonView();
        //    personView.iDeathBirthEditorModel.SetEditorUniqueRef(this.GetStringByLen(51));
        //    Assert.IsFalse(personView.iDeathBirthEditorModel.IsValidUniqueRef);

        //}
        //150
        [Test(Description = "InvalidOriginalName")]
        public void CheckInvalidOriginalName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorModel.SetFilterOriginalName(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidOriginalName);

        }
        //150
        [Test(Description = "InvalidOriginalFatherName")]
        public void CheckInvalidOriginalFatherName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorModel.SetFilterOriginalFatherName(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidOriginalFatherName);

        }
        //150
        [Test(Description = "InvalidOriginalMotherName")]
        public void CheckInvalidOriginalMotherName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorModel.SetFilterOriginalMotherName(this.GetStringByLen(151));
            Assert.IsFalse(personView.personValidation.ValidOriginalMotherName);
        }

        //50
        [Test(Description = "InvalidReferenceDate")]
        [TestCaseSource("InvalidDates")]
        public void CheckInvalidReferenceDate(string testVal)
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorReferenceDate(this.GetStringByLen(51));
            Assert.IsFalse(personView.personValidation.ValidReferenceDate);

            personView.iDeathBirthEditorControl.RequestSetEditorReferenceDate(testVal);
            Assert.IsFalse(personView.personValidation.ValidReferenceDate);
        }



        //50
        [Test(Description = "InvalidDateBirthString")]
        [TestCaseSource("InvalidDates")]
        public void CheckInvalidDateBirthString(string testVal)
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorDateBirthString(this.GetStringByLen(51));
            Assert.IsFalse(personView.personValidation.ValidBirthDate);

            personView.iDeathBirthEditorControl.RequestSetEditorDateBirthString(testVal);
            Assert.IsFalse(personView.personValidation.ValidBirthDate);
        }

        //50
        [Test(Description = "InvalidDateDeathString")]
        [TestCaseSource("InvalidDates")]
        public void CheckInvalidDateDeathString(string testVal)
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorDateDeathString(this.GetStringByLen(51));
            Assert.IsFalse(personView.personValidation.ValidDeathDate);

            personView.iDeathBirthEditorControl.RequestSetEditorDateDeathString(testVal);
            Assert.IsFalse(personView.personValidation.ValidDeathDate);
        }

        //50
        [Test(Description = "InvalidDateBapString")]
        [TestCaseSource("InvalidDates")]
        public void CheckInvalidDateBapString(string testVal)
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorDateBapString(this.GetStringByLen(51));
            Assert.IsFalse(personView.personValidation.ValidBapDate);

            personView.iDeathBirthEditorControl.RequestSetEditorDateBapString(testVal);
            Assert.IsFalse(personView.personValidation.ValidBapDate);

        }

        //Guid EditorBirthLocationId { get; }
        //bool IsValidBirthLocationId { get; }

        //Guid EditorDeathLocationId { get; }
        //bool IsValidDeathLocationId { get; }

        //Guid EditorReferenceLocationId { get; }
        //bool IsValidReferenceLocationId { get; }

        [Test(Description = "ValidBirthCountyLocation")]
        public void CheckValidBirthCountyLocation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorBirthCountyLocation("yorkshire");
            Assert.IsTrue(personView.personValidation.ValidBirthCountyLocation);
        }



        [Test(Description = "ValidSpouseCName")]
        public void CheckValidSpouseCName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorSpouseCName("test");
            Assert.IsTrue(personView.personValidation.ValidSpouseCName);
        }

        [Test(Description = "ValidSpouseSName")]
        public void CheckValidSpouseSName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorSpouseSName("test");
            Assert.IsTrue(personView.personValidation.ValidSpouseSName);
        }

        [Test(Description = "ValidFatherOccupation")]
        public void CheckValidFatherOccupation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorFatherOccupation("test");
            Assert.IsTrue(personView.personValidation.ValidFatherOccupation);
        }



        [Test(Description = "ValidReferenceLocation")]
        public void CheckValidReferenceLocation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorReferenceLocation("test");
            Assert.IsTrue(personView.personValidation.ValidReferenceLocation);
        }

        [Test(Description = "ValidOccupation")]
        public void CheckValidOccupation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorOccupation("test");
            Assert.IsTrue(personView.personValidation.ValidOccupation);

        }



        [Test(Description = "ValidChristianName")]
        public void CheckValidChristianName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorChristianName("test");
            Assert.IsTrue(personView.personValidation.ValidName);
        }

        [Test(Description = "ValidSurnameName")]
        public void CheckValidSurnameName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorSurnameName("test");
            Assert.IsTrue(personView.personValidation.ValidSurname);
        }

        [Test(Description = "ValidFatherChristianName")]
        public void CheckValidFatherChristianName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorFatherChristianName("test");
            Assert.IsTrue(personView.personValidation.ValidFatherChristianName);

        }

        [Test(Description = "ValidFatherSurname")]
        public void CheckValidFatherSurname()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorFatherSurname("test");
            Assert.IsTrue(personView.personValidation.ValidFatherSurname);

        }

        [Test(Description = "ValidMotherChristianName")]
        public void CheckValidMotherChristianName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorMotherChristianName("test");
            Assert.IsTrue(personView.personValidation.ValidMotherChristianName);

        }

        [Test(Description = "ValidMotherSurname")]
        public void CheckValidMotherSurname()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorMotherSurname("test");
            Assert.IsTrue(personView.personValidation.ValidMotherSurname);
        }




        //[Test(Description = "ValidBirthCountyLocation")]
        //public void CheckValidBirthCountyLocation()
        //{
        //    PersonView personView = new PersonView();
        //    personView.iDeathBirthEditorControl.RequestSetEditorBirthCountyLocation("test");
        //    Assert.IsTrue(personView.iDeathBirthEditorModel.IsValidBirthCountyLocation);
        //}


        [Test(Description = "ValidDeathCountyLocation")]
        public void CheckValidDeathCountyLocation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorDeathCountyLocation("test");
            Assert.IsTrue(personView.personValidation.ValidDeathCountyLocation);

        }

        [Test(Description = "ValidBirthLocation")]
        public void CheckValidBirthLocation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorBirthLocation("test");
            Assert.IsTrue(personView.personValidation.ValidLocation);

        }

        [Test(Description = "ValidDeathLocation")]
        public void CheckValidDeathLocation()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorDeathLocation("test");
            Assert.IsTrue(personView.personValidation.ValidDeathLocation);
        }



        [Test(Description = "ValidSource")]
        public void CheckValidSource()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorSource("test");
            Assert.IsTrue(personView.personValidation.ValidSource);
        }

        [Test(Description = "ValidNotes")]
        public void CheckValidNotes()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorNotes("test");
            Assert.IsTrue(personView.personValidation.ValidNotes);
        }

        //[Test(Description = "ValidUniqueRef")]
        //public void CheckValidUniqueRef()
        //{
        //    PersonView personView = new PersonView();
        //    personView.iDeathBirthEditorModel.SetEditorUniqueRef("test");
        //    Assert.IsTrue(personView.iDeathBirthEditorModel.IsValidUniqueRef);

        //}

        [Test(Description = "ValidOriginalName")]
        public void CheckValidOriginalName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetFilterOriginalName("test");
            Assert.IsTrue(personView.personValidation.ValidOriginalName);

        }

        [Test(Description = "ValidOriginalFatherName")]
        public void CheckValidOriginalFatherName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetFilterOriginalFatherName("test");
            Assert.IsTrue(personView.personValidation.ValidOriginalFatherName);

        }

        [Test(Description = "ValidOriginalMotherName")]
        public void CheckValidOriginalMotherName()
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetFilterOriginalMotherName("test");
            Assert.IsTrue(personView.personValidation.ValidOriginalMotherName);
        }

        //  [TestCaseSource("InvalidDates")]
        //public void CheckInvalidDateBapString(string testVal)

        [Test(Description = "ValidReferenceDate")]
        [TestCaseSource("ValidDates")]
        public void CheckValidReferenceDate(string testVal)
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorReferenceDate(testVal);
            Assert.IsTrue(personView.personValidation.ValidReferenceDate);
        }

        [Test(Description = "ValidDateBirthString")]
        [TestCaseSource("ValidDates")]
        public void CheckValidDateBirthString(string testVal)
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorDateBirthString(testVal);
            Assert.IsTrue(personView.personValidation.ValidBirthDate);
        }

        [Test(Description = "ValidDateDeathString")]
        [TestCaseSource("ValidDates")]
        public void CheckValidDateDeathString(string testVal)
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorDateDeathString(testVal);
            Assert.IsTrue(personView.personValidation.ValidDeathDate);
        }

        [Test(Description = "ValidDateBapString")]
        [TestCaseSource("ValidDates")]
        public void CheckValidDateBapString(string testVal)
        {
            PersonView personView = new PersonView();
            personView.iDeathBirthEditorControl.RequestSetEditorDateBapString(testVal);
            Assert.IsTrue(personView.personValidation.ValidBapDate);

        }

        #endregion

        [Test(Description = "Add Person")]
        public void AddPerson()
        {
            PersonView personView = new PersonView();

            personView.iDeathBirthEditorModel.SetIsSecurityEnabled(false);

            personView.iDeathBirthEditorControl.RequestSetEditorChristianName("mrtest");
            personView.iDeathBirthEditorControl.RequestSetEditorSurnameName("mrtestsurname");
            personView.iDeathBirthEditorControl.RequestSetEditorBirthLocation("testbirthlocation");
            personView.iDeathBirthEditorControl.RequestSetEditorDateBirthString("1 Jan 1812");

            Assert.IsTrue(personView.iDeathBirthEditorModel.IsValidEntry);


            personView.iDeathBirthEditorControl.RequestInsert();

            Assert.IsTrue(personView.iDeathBirthEditorModel.SelectedRecordId != Guid.Empty);

            DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();

            Person _person = deathsBirthsBLL.ModelContainer.Persons.Where(p => p.ChristianName == "mrtest" && p.Surname == "mrtestsurname" && p.BirthLocation == "testbirthlocation" && p.BirthInt == 1812).FirstOrDefault();


            Assert.IsNotNull(_person);

            personView.iDeathBirthEditorControl.RequestDelete();

            // we dont delete things we just set a flag as deleted
            // that way i cant inadvertantly delete important records.
            _person = deathsBirthsBLL.ModelContainer.Persons.Where(p => p.ChristianName == "mrtest" && p.Surname == "mrtestsurname" && p.BirthLocation == "testbirthlocation" && p.BirthInt == 1812 && !p.IsDeleted).FirstOrDefault();


            Assert.IsNull(_person);

            _person = deathsBirthsBLL.ModelContainer.Persons.Where(p => p.ChristianName == "mrtest" && p.Surname == "mrtestsurname" && p.BirthLocation == "testbirthlocation" && p.BirthInt == 1812).FirstOrDefault();


            deathsBirthsBLL.ModelContainer.Persons.DeleteObject(_person);
            deathsBirthsBLL.ModelContainer.SaveChanges();

            _person = deathsBirthsBLL.ModelContainer.Persons.Where(p => p.ChristianName == "mrtest" && p.Surname == "mrtestsurname" && p.BirthLocation == "testbirthlocation" && p.BirthInt == 1812).FirstOrDefault();

            Assert.IsNull(_person);
        }

    }

    [TestFixture]
    public class NUnitPersonAdd : TestData
    {
        PersonView _personView1 = null;
        PersonView _personView2 = null;
        Person _person = null;


        [SetUp]
        public void AddPerson()
        {
            Guid locationId = Guid.NewGuid();
            int fatherid = 99;
            int motherid =100;

            _personView1 = new PersonView();
            _personView1.iDeathBirthEditorModel.SetIsSecurityEnabled(false);
            _personView2 = new PersonView();
            _personView2.iDeathBirthEditorModel.SetIsSecurityEnabled(false);
            _person = new Person();
       
            #region create test data
            _person.BapInt = 1800;
            _person.BaptismDateStr = "1 Jan 1800";
            _person.BirthCounty = "yorkshire";
            _person.BirthDateStr = "1 Jan 1800";
            _person.BirthInt = 1800;
            _person.BirthLocation = "leeds";
            _person.BirthLocationId = locationId;
            _person.ChristianName = "bobtest";
            _person.DateAdded = DateTime.Today;
            _person.DateLastEdit = DateTime.Today;
            _person.DeathCounty = "yorkshire";
            _person.DeathDateStr = "1 Jan 1900";
            _person.DeathInt = 1800;
            _person.DeathLocation = "leeds";
            _person.DeathLocationId = locationId;
            _person.EstBirthYearInt = 1800;
            _person.EstDeathYearInt = 1900;
            _person.EventPriority = 10;
            _person.FatherChristianName = "bobdadtest";
            _person.FatherId = fatherid;
            _person.FatherOccupation = "testfatherocc";
            _person.FatherSurname = "bobdadtestsurname";
            _person.IsDeleted = false;
            _person.IsEstBirth = true;
            _person.IsEstDeath = true;
            _person.IsMale = true;
            _person.MotherChristianName = "mothercname";
            _person.MotherId = motherid;
            _person.MotherSurname = "mothersname";
            _person.Notes = "testnotes";
            _person.Occupation = "testocc";
            _person.OrigFatherSurname = "testorigfatname";
            _person.OrigMotherSurname = "testorigmotname";
            _person.OrigSurname = "testorigsurname";

            _person.ReferenceDateInt = 1800;
            _person.ReferenceDateStr = "1 Jan 1800";
            _person.ReferenceLocation = "grantham";
            _person.ReferenceLocationId = locationId;
            _person.Source = "testsource";
            _person.SpouseName = "testspousecname";
            _person.SpouseSurname = "testspousesname";
            _person.Surname = "testsurname";
            _person.TotalEvents = 99;
            _person.UniqueRef = Guid.NewGuid();
            _person.UserId = 1;

            #endregion


            _personView1.iDeathBirthEditorControl.RequestSetEditorBirthCountyLocation(_person.BirthCounty);
            _personView1.iDeathBirthEditorControl.RequestSetEditorBirthLocation(_person.BirthLocation);
            _personView1.iDeathBirthEditorControl.RequestSetEditorBirthLocationId(_person.BirthLocationId);
            _personView1.iDeathBirthEditorControl.RequestSetEditorChristianName(_person.ChristianName);
            _personView1.iDeathBirthEditorControl.RequestSetEditorDateBapString(_person.BaptismDateStr);
            _personView1.iDeathBirthEditorControl.RequestSetEditorDateBirthString(_person.BirthDateStr);
            _personView1.iDeathBirthEditorControl.RequestSetEditorDateDeathString(_person.DeathDateStr);
            _personView1.iDeathBirthEditorControl.RequestSetEditorDeathCountyLocation(_person.DeathCounty);
            _personView1.iDeathBirthEditorControl.RequestSetEditorDeathLocation(_person.DeathLocation);
            _personView1.iDeathBirthEditorControl.RequestSetEditorDeathLocationId(_person.DeathLocationId);
            _personView1.iDeathBirthEditorControl.RequestSetEditorFatherChristianName(_person.FatherChristianName);
            _personView1.iDeathBirthEditorControl.RequestSetEditorFatherOccupation(_person.FatherOccupation);
            _personView1.iDeathBirthEditorControl.RequestSetEditorFatherSurname(_person.FatherSurname);
            _personView1.iDeathBirthEditorControl.RequestSetEditorIsMale(_person.IsMale);
            _personView1.iDeathBirthEditorControl.RequestSetEditorMotherChristianName(_person.MotherChristianName);
            _personView1.iDeathBirthEditorControl.RequestSetEditorMotherSurname(_person.MotherSurname);
            _personView1.iDeathBirthEditorControl.RequestSetEditorNotes(_person.Notes);
            _personView1.iDeathBirthEditorControl.RequestSetEditorOccupation(_person.Occupation);
            _personView1.iDeathBirthEditorControl.RequestSetEditorReferenceDate(_person.ReferenceDateStr);
            _personView1.iDeathBirthEditorControl.RequestSetEditorReferenceLocation(_person.ReferenceLocation);
            _personView1.iDeathBirthEditorControl.RequestSetEditorReferenceLocationId(_person.ReferenceLocationId);
            _personView1.iDeathBirthEditorControl.RequestSetEditorSource(_person.Source);
            _personView1.iDeathBirthEditorControl.RequestSetEditorSpouseCName(_person.SpouseName);
            _personView1.iDeathBirthEditorControl.RequestSetEditorSpouseSName(_person.SpouseSurname);
            _personView1.iDeathBirthEditorControl.RequestSetEditorSurnameName(_person.Surname);
            _personView1.iDeathBirthEditorControl.RequestSetFilterOriginalFatherName(_person.OrigFatherSurname);
            _personView1.iDeathBirthEditorControl.RequestSetFilterOriginalMotherName(_person.OrigMotherSurname);
            _personView1.iDeathBirthEditorControl.RequestSetFilterOriginalName(_person.OrigSurname);
            _personView1.iDeathBirthEditorModel.SetEditorEventPriority(_person.EventPriority);
            _personView1.iDeathBirthEditorModel.SetEditorUniqueRef(_person.UniqueRef);
            _personView1.iDeathBirthEditorModel.SetEditorTotalEvents(_person.TotalEvents);

            Assert.IsTrue(_personView1.iDeathBirthEditorModel.IsValidEntry);

            _personView1.iDeathBirthEditorControl.RequestInsert();

            Assert.IsTrue(_personView1.iDeathBirthEditorModel.SelectedRecordId != Guid.Empty);

            _personView2.iDeathBirthEditorControl.RequestSetSelectedId(_personView1.iDeathBirthEditorModel.SelectedRecordId);

            _personView2.iDeathBirthEditorControl.RequestRefresh();




        }


        [Test(Description = "Records Present")]
        public void RecordsPresent()
        {
            Assert.IsTrue(_personView1.iDeathBirthEditorModel.SelectedRecordId != Guid.Empty);
            Assert.IsTrue(_personView2.iDeathBirthEditorModel.EditorChristianName != "");
        }

        [Test(Description = "CheckSavedData")]
        public void CheckSavedData()
        {
            Assert.IsTrue(_person.BaptismDateStr == _personView2.iDeathBirthEditorModel.EditorDateBapString);
            Assert.IsTrue(_person.BirthCounty == _personView2.iDeathBirthEditorModel.EditorBirthCountyLocation);
            Assert.IsTrue(_person.BirthDateStr == _personView2.iDeathBirthEditorModel.EditorDateBirthString);
            Assert.IsTrue(_person.BirthLocation == _personView2.iDeathBirthEditorModel.EditorBirthLocation);
            Assert.IsTrue(_person.BirthLocationId == _personView2.iDeathBirthEditorModel.EditorBirthLocationId);
            Assert.IsTrue(_person.ChristianName == _personView2.iDeathBirthEditorModel.EditorChristianName);
            Assert.IsTrue(_person.DeathCounty == _personView2.iDeathBirthEditorModel.EditorDeathCountyLocation);
            Assert.IsTrue(_person.DeathDateStr == _personView2.iDeathBirthEditorModel.EditorDateDeathString);
            Assert.IsTrue(_person.DeathLocation == _personView2.iDeathBirthEditorModel.EditorDeathLocation);
            Assert.IsTrue(_person.DeathLocationId == _personView2.iDeathBirthEditorModel.EditorDeathLocationId);
            Assert.IsTrue(_person.EventPriority == _personView2.iDeathBirthEditorModel.EditorEventPriority);
            Assert.IsTrue(_person.FatherChristianName == _personView2.iDeathBirthEditorModel.EditorFatherChristianName);
            Assert.IsTrue(_person.FatherOccupation == _personView2.iDeathBirthEditorModel.EditorFatherOccupation);
            Assert.IsTrue(_person.FatherSurname == _personView2.iDeathBirthEditorModel.EditorFatherSurname);
            Assert.IsTrue(_person.MotherChristianName == _personView2.iDeathBirthEditorModel.EditorMotherChristianName);
            Assert.IsTrue(_person.MotherSurname == _personView2.iDeathBirthEditorModel.EditorMotherSurname);
            Assert.IsTrue(_person.Notes == _personView2.iDeathBirthEditorModel.EditorNotes);
            Assert.IsTrue(_person.Occupation == _personView2.iDeathBirthEditorModel.EditorOccupation);
            Assert.IsTrue(_person.OrigFatherSurname == _personView2.iDeathBirthEditorModel.FilterOriginalFatherName);
            Assert.IsTrue(_person.OrigMotherSurname == _personView2.iDeathBirthEditorModel.FilterOriginalMotherName);
            Assert.IsTrue(_person.OrigSurname == _personView2.iDeathBirthEditorModel.FilterOriginalName);
            Assert.IsTrue(_person.ReferenceDateStr == _personView2.iDeathBirthEditorModel.EditorReferenceDateString);
            Assert.IsTrue(_person.ReferenceLocation == _personView2.iDeathBirthEditorModel.EditorReferenceLocation);
            Assert.IsTrue(_person.ReferenceLocationId == _personView2.iDeathBirthEditorModel.EditorReferenceLocationId);
            Assert.IsTrue(_person.Source == _personView2.iDeathBirthEditorModel.EditorSource);
            Assert.IsTrue(_person.SpouseName == _personView2.iDeathBirthEditorModel.EditorSpouseCName);
            Assert.IsTrue(_person.SpouseSurname == _personView2.iDeathBirthEditorModel.EditorSpouseSName);
            Assert.IsTrue(_person.Surname == _personView2.iDeathBirthEditorModel.EditorSurnameName);
            Assert.IsTrue(_person.TotalEvents == _personView2.iDeathBirthEditorModel.EditorTotalEvents);
            Assert.IsTrue(_person.UniqueRef == _personView2.iDeathBirthEditorModel.EditorUniqueRef);




        }

        [TearDown]
        public void TearDown()
        {

            _personView2.iDeathBirthEditorControl.RequestDelete();

            _personView2.iDeathBirthEditorControl.RequestRefresh();

            Assert.IsTrue(_personView2.iDeathBirthEditorModel.EditorChristianName == "");



            DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();

            Person _person0 = deathsBirthsBLL.ModelContainer.Persons.FirstOrDefault(p => p.Person_id == _personView2.iDeathBirthEditorModel.SelectedRecordId);

            if (_person0 != null)
            {
                deathsBirthsBLL.ModelContainer.Persons.DeleteObject(_person0);
                deathsBirthsBLL.ModelContainer.SaveChanges();
            }

        }

    }



}
