using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.Types;
using GedItter.MarriageRecords;
using NUnit.Framework;
using TDBCore.EntityModel;
using GedItter.MarriageRecords.BLL;
using TDBCore.BLL;
using GedItter.BLL;
using GedItter.BirthDeathRecords.BLL;
using System.Collections;
using System.Diagnostics;

namespace UnitTests
{
    public class MarriageView : IMarriageEditorView
    {
        public MarriageValidation marriageValidation = new MarriageValidation();
        public IMarriageEditorControl iMarriageEditorControl = new MarriagesEditorControl();
        public IMarriageEditorModel iMarriageEditorModel = new MarriagesEditorModel();

        public MarriageView()
        {
            iMarriageEditorModel = new MarriagesEditorModel();
            iMarriageEditorControl = new MarriagesEditorControl();

            if (iMarriageEditorModel != null)
                iMarriageEditorControl.SetModel(iMarriageEditorModel);

            if (iMarriageEditorControl != null)
                iMarriageEditorControl.SetView(this);
        }

        #region validation

        public void ShowInvalidMarriageDate(bool valid)
        {
            marriageValidation.IsValidMarriageDate = valid;
        }

        public void ShowInvalidMaleName(bool valid)
        {
            marriageValidation.IsValidMaleName = valid;
        }

        public void ShowInvalidFemaleName(bool valid)
        {
            marriageValidation.IsValidFemaleName = valid;
        }

        public void ShowInvalidFemaleSurname(bool valid)
        {
            marriageValidation.IsValidFemaleSurname = valid;
        }

        public void ShowInvalidLocation(bool valid)
        {
            marriageValidation.IsValidLocation = valid;
        }

        public void ShowInvalidMaleLocation(bool valid)
        {
            marriageValidation.IsValidMaleLocation = valid;
        }

        public void ShowInvalidFemaleLocation(bool valid)
        {
            marriageValidation.IsValidFemaleLocation = valid;
        }

        public void ShowInvalidMaleInfo(bool valid)
        {
            marriageValidation.IsValidMaleInfo = valid;
        }

        public void ShowInvalidFemaleInfo(bool valid)
        {
            marriageValidation.IsValidFemaleInfo = valid;
        }

        public void ShowInvalidMarriageCounty(bool valid)
        {
            marriageValidation.IsValidMarriageCounty = valid;
        }

        public void ShowInvalidSource(bool valid)
        {
            marriageValidation.IsValidSource = valid;
        }

        public void ShowInvalidWitnessSName1(bool valid)
        {
            marriageValidation.IsValidWitnessSName1 = valid;
        }

        public void ShowInvalidWitnessSName2(bool valid)
        {
            marriageValidation.IsValidWitnessSName2 = valid;
        }

        public void ShowInvalidWitnessSName3(bool valid)
        {
            marriageValidation.IsValidWitnessSName3 = valid;
        }

        public void ShowInvalidWitnessSName4(bool valid)
        {
            marriageValidation.IsValidWitnessSName4 = valid;
        }

        public void ShowInvalidWitnessCName1(bool valid)
        {
            marriageValidation.IsValidWitnessCName1 = valid;
        }

        public void ShowInvalidWitnessCName2(bool valid)
        {
            marriageValidation.IsValidWitnessCName2 = valid;
        }

        public void ShowInvalidWitnessCName3(bool valid)
        {
            marriageValidation.IsValidWitnessCName3 = valid;
        }

        public void ShowInvalidWitnessCName4(bool valid)
        {
            marriageValidation.IsValidWitnessCName4 = valid;
        }

        public void ShowInvalidMaleOccupation(bool valid)
        {
            marriageValidation.IsValidMaleOccupation = valid;
        }

        public void ShowInvalidFemaleOccupation(bool valid)
        {
            marriageValidation.IsValidFemaleOccupation = valid;
        }

        public void ShowInvalidFemaleBirthYear(bool valid)
        {
            marriageValidation.IsValidFemaleBirthYear = valid;
        }

        public void ShowInvalidMaleBirthYear(bool valid)
        {
            marriageValidation.IsValidMaleBirthYear = valid;
        }

        public void ShowInvalidOriginalName(bool valid)
        {
            marriageValidation.IsValidOriginalName = valid;
        }

        public void ShowInvalidOriginalFemaleName(bool valid)
        {
            marriageValidation.IsValidOriginalFemaleName = valid;
        }

        public void ShowInvalidMaleSurname(bool valid)
        {
            marriageValidation.IsValidMaleSurname = valid;
        }

        #endregion

        public void Update<T>(T paramModel)
        {

        }





    }

    public class MarriageFilterView : iMarriageFilterView
    {
        public event EventHandler ShowEditor;
        public bool ValidLowerBoundDate { get; set; }
        public bool ValidUpperBoundDate { get; set; }

        public IMarriageFilterControl iMarriageFilterControl = null;
        public IMarriageFilterModel iMarriageFilterModel = null;


        public MarriageFilterView()
        {
            iMarriageFilterModel = new MarriagesFilterModel();
            iMarriageFilterControl = new MarriagesFilterControl();

            if (iMarriageFilterModel != null)
                iMarriageFilterControl.SetModel(iMarriageFilterModel);

            if (iMarriageFilterControl != null)
                iMarriageFilterControl.SetView(this);
        }


        #region validation

        public void ShowInvalidUpperBoundMarriageWarning(bool valid)
        {
            this.ValidUpperBoundDate = valid;
        }

        public void ShowInvalidLowerBoundMarriageWarning(bool valid)
        {
            this.ValidLowerBoundDate = valid;
        }

        #endregion


        public void SetParentIds(List<Guid> Ids)
        {
            iMarriageFilterControl.RequestSetParentRecordIds(Ids);
        }

        public void SetFilterMode(MarriageFilterTypes param)
        {
            iMarriageFilterControl.RequestSetFilterMode(param);
        }

        public void Update<T>(T paramModel)
        {
        
        }

       
    }


    [TestFixture]
    public class NUnitMarriageProperties : TestData
    {

        #region validation

        [Test(Description = "CheckValidMaleOccupation")]
        public void CheckValidMaleOccupation()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMaleOccupation("");
            Assert.IsTrue(marriageView.marriageValidation.IsValidMaleOccupation);
            marriageView.iMarriageEditorControl.RequestSetEditorMaleOccupation("test male occ");
            Assert.IsTrue(marriageView.marriageValidation.IsValidMaleOccupation);

        }

        [Test(Description = "CheckValidFemaleOccupation")]
        public void CheckValidFemaleOccupation()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorFemaleOccupation("");
            Assert.IsTrue(marriageView.marriageValidation.IsValidFemaleOccupation);

            marriageView.iMarriageEditorControl.RequestSetEditorFemaleOccupation("test female occ");
            Assert.IsTrue(marriageView.marriageValidation.IsValidFemaleOccupation);
        }


        [Test(Description = "CheckValidMaleName")]
        public void CheckValidMaleName()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMaleName("joe", "bloggs");
            Assert.IsTrue(marriageView.marriageValidation.IsValidMaleName);
            Assert.IsTrue(marriageView.marriageValidation.IsValidMaleSurname);
        }

        [Test(Description = "CheckValidFemaleName")]
        public void CheckValidFemaleName()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorFemaleName("sally", "bloggs");
            Assert.IsTrue(marriageView.marriageValidation.IsValidFemaleName);
            Assert.IsTrue(marriageView.marriageValidation.IsValidFemaleSurname);
        }

        [Test(Description = "CheckValidMarriageDate")]
        [TestCaseSource("ValidDates")]
        public void CheckValidMarriageDate(string testval)
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMarriageDate(testval);
            Assert.IsTrue(marriageView.marriageValidation.IsValidMarriageDate);
        }

        [Test(Description = "CheckValidLocation")]
        public void CheckValidLocation()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorLocation("Sleaford");
            Assert.IsTrue(marriageView.marriageValidation.IsValidLocation);
        }

        [Test(Description = "CheckValidMaleLocation")]
        public void CheckValidMaleLocation()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMaleLocation("Sleaford");
            Assert.IsTrue(marriageView.marriageValidation.IsValidMaleLocation);
        }

        [Test(Description = "CheckValidFemaleLocation")]
        public void CheckValidFemaleLocation()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMaleLocation("Sleaford");
            Assert.IsTrue(marriageView.marriageValidation.IsValidMaleLocation);
        }

        [Test(Description = "CheckValidMarriageCounty")]
        public void CheckValidMarriageCounty()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMarriageCounty("Lincolnshire");
            Assert.IsTrue(marriageView.marriageValidation.IsValidMarriageCounty);
        }

        [Test(Description = "CheckValidSource")]
        public void CheckValidSource()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorSource("test");
            Assert.IsTrue(marriageView.marriageValidation.IsValidSource);
        }

        [Test(Description = "CheckValidWitness1")]
        public void CheckValidWitness1()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness1("wit sname 1");
            Assert.IsTrue(marriageView.marriageValidation.IsValidWitnessSName1);
        }

        [Test(Description = "CheckValidWitness2")]
        public void CheckValidWitness2()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness2("wit sname 2");
            Assert.IsTrue(marriageView.marriageValidation.IsValidWitnessSName2);
        }

        [Test(Description = "CheckValidWitness3")]
        public void CheckValidWitness3()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness3("wit sname 3");
            Assert.IsTrue(marriageView.marriageValidation.IsValidWitnessSName3);
        }

        [Test(Description = "CheckValidWitness4")]
        public void CheckValidWitness4()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness4("wit sname 4");
            Assert.IsTrue(marriageView.marriageValidation.IsValidWitnessSName4);
        }

        [Test(Description = "CheckValidWitness1CName")]
        public void CheckValidWitness1CName()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness1CName("wit cname 1");
            Assert.IsTrue(marriageView.marriageValidation.IsValidWitnessCName1);
        }

        [Test(Description = "CheckValidWitness2CName")]
        public void CheckValidWitness2CName()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness2CName("wit cname 2");
            Assert.IsTrue(marriageView.marriageValidation.IsValidWitnessCName2);
        }

        [Test(Description = "CheckValidWitness3CName")]
        public void CheckValidWitness3CName()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness3CName("wit cname 3");
            Assert.IsTrue(marriageView.marriageValidation.IsValidWitnessCName3);
        }

        [Test(Description = "CheckValidWitness4CName")]
        public void CheckValidWitness4CName()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness4CName("wit cname 4");
            Assert.IsTrue(marriageView.marriageValidation.IsValidWitnessCName4);
        }

        [Test(Description = "CheckValidMaleInfo")]
        public void CheckValidMaleInfo()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMaleInfo("male info");
            Assert.IsTrue(marriageView.marriageValidation.IsValidMaleInfo);
        }

        [Test(Description = "CheckValidFemaleInfo")]
        public void CheckValidFemaleInfo()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorFemaleInfo("female info");
            Assert.IsTrue(marriageView.marriageValidation.IsValidFemaleInfo);
        }

        [Test(Description = "CheckValidFemaleBirthYear")]
        public void CheckValidFemaleBirthYear()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorFemaleBirthYear("1980");
            Assert.IsTrue(marriageView.marriageValidation.IsValidFemaleBirthYear);
        }

        [Test(Description = "CheckValidMaleBirthYear")]
        public void CheckValidMaleBirthYear()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMaleBirthYear("1980");
            Assert.IsTrue(marriageView.marriageValidation.IsValidMaleBirthYear);
        }

        //invalid


        [Test(Description = "CheckInvalidMaleOccupation")]
        public void CheckInvalidMaleOccupation()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMaleOccupation(this.GetStringByLen(501));
            Assert.IsFalse(marriageView.marriageValidation.IsValidMaleOccupation);


        }

        [Test(Description = "CheckInvalidFemaleOccupation")]
        public void CheckInvalidFemaleOccupation()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorFemaleOccupation(this.GetStringByLen(501));
            Assert.IsFalse(marriageView.marriageValidation.IsValidFemaleOccupation);


        }


        [Test(Description = "CheckInvalidMaleName")]
        public void CheckInvalidMaleName()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMaleName(this.GetStringByLen(51), this.GetStringByLen(51));
            Assert.IsFalse(marriageView.marriageValidation.IsValidMaleName);
            Assert.IsFalse(marriageView.marriageValidation.IsValidMaleSurname);
        }

        [Test(Description = "CheckInvalidFemaleName")]
        public void CheckInvalidFemaleName()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorFemaleName(this.GetStringByLen(51), this.GetStringByLen(51));
            Assert.IsFalse(marriageView.marriageValidation.IsValidFemaleName);
            Assert.IsFalse(marriageView.marriageValidation.IsValidFemaleSurname);
        }

        [Test(Description = "CheckInvalidMarriageDate")]
        [TestCaseSource("InvalidDates")]
        public void CheckInvalidMarriageDate(string testval)
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMarriageDate(testval);
            Assert.IsFalse(marriageView.marriageValidation.IsValidMarriageDate);
        }

        [Test(Description = "CheckInvalidLocation")]
        public void CheckInvalidLocation()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorLocation(this.GetStringByLen(51));
            Assert.IsFalse(marriageView.marriageValidation.IsValidLocation);
        }

        [Test(Description = "CheckInvalidMaleLocation")]
        public void CheckInvalidMaleLocation()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMaleLocation(this.GetStringByLen(51));
            Assert.IsFalse(marriageView.marriageValidation.IsValidMaleLocation);
        }

        [Test(Description = "CheckInvalidFemaleLocation")]
        public void CheckInvalidFemaleLocation()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMaleLocation(this.GetStringByLen(51));
            Assert.IsFalse(marriageView.marriageValidation.IsValidMaleLocation);
        }

        [Test(Description = "CheckInvalidMarriageCounty")]
        public void CheckInvalidMarriageCounty()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMarriageCounty(this.GetStringByLen(51));
            Assert.IsFalse(marriageView.marriageValidation.IsValidMarriageCounty);
        }

        [Test(Description = "CheckInvalidSource")]
        public void CheckInvalidSource()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorSource(this.GetStringByLen(51));
            Assert.IsFalse(marriageView.marriageValidation.IsValidSource);
        }

        [Test(Description = "CheckInvalidWitness1")]
        public void CheckInvalidWitness1()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness1(this.GetStringByLen(151));
            Assert.IsFalse(marriageView.marriageValidation.IsValidWitnessSName1);
        }

        [Test(Description = "CheckInvalidWitness2")]
        public void CheckInvalidWitness2()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness2(this.GetStringByLen(151));
            Assert.IsFalse(marriageView.marriageValidation.IsValidWitnessSName2);
        }

        [Test(Description = "CheckInvalidWitness3")]
        public void CheckInvalidWitness3()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness3(this.GetStringByLen(151));
            Assert.IsFalse(marriageView.marriageValidation.IsValidWitnessSName3);
        }

        [Test(Description = "CheckInvalidWitness4")]
        public void CheckInvalidWitness4()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness4(this.GetStringByLen(151));
            Assert.IsFalse(marriageView.marriageValidation.IsValidWitnessSName4);
        }

        [Test(Description = "CheckInvalidWitness1CName")]
        public void CheckInvalidWitness1CName()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness1CName(this.GetStringByLen(151));
            Assert.IsFalse(marriageView.marriageValidation.IsValidWitnessCName1);
        }

        [Test(Description = "CheckInvalidWitness2CName")]
        public void CheckInvalidWitness2CName()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness2CName(this.GetStringByLen(151));
            Assert.IsFalse(marriageView.marriageValidation.IsValidWitnessCName2);
        }

        [Test(Description = "CheckInvalidWitness3CName")]
        public void CheckInvalidWitness3CName()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness3CName(this.GetStringByLen(151));
            Assert.IsFalse(marriageView.marriageValidation.IsValidWitnessCName3);
        }

        [Test(Description = "CheckInvalidWitness4CName")]
        public void CheckInvalidWitness4CName()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorWitness4CName(this.GetStringByLen(151));
            Assert.IsFalse(marriageView.marriageValidation.IsValidWitnessCName4);
        }

        [Test(Description = "CheckInvalidMaleInfo")]
        public void CheckInvalidMaleInfo()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMaleInfo(this.GetStringByLen(501));
            Assert.IsFalse(marriageView.marriageValidation.IsValidMaleInfo);
        }

        [Test(Description = "CheckInvalidFemaleInfo")]
        public void CheckInvalidFemaleInfo()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorFemaleInfo(this.GetStringByLen(501));
            Assert.IsFalse(marriageView.marriageValidation.IsValidFemaleInfo);
        }

        [Test(Description = "CheckInvalidFemaleBirthYear")]
        public void CheckInvalidFemaleBirthYear()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorFemaleBirthYear("x");
            Assert.IsFalse(marriageView.marriageValidation.IsValidFemaleBirthYear);
            marriageView.iMarriageEditorControl.RequestSetEditorFemaleBirthYear("");
            Assert.IsFalse(marriageView.marriageValidation.IsValidFemaleBirthYear);
            marriageView.iMarriageEditorControl.RequestSetEditorFemaleBirthYear("0");
            Assert.IsFalse(marriageView.marriageValidation.IsValidFemaleBirthYear);
            marriageView.iMarriageEditorControl.RequestSetEditorFemaleBirthYear("10000");
            Assert.IsFalse(marriageView.marriageValidation.IsValidFemaleBirthYear);
        }

        [Test(Description = "CheckInvalidMaleBirthYear")]
        public void CheckInvalidMaleBirthYear()
        {
            MarriageView marriageView = new MarriageView();
            marriageView.iMarriageEditorControl.RequestSetEditorMaleBirthYear("x");
            Assert.IsFalse(marriageView.marriageValidation.IsValidMaleBirthYear);
            marriageView.iMarriageEditorControl.RequestSetEditorMaleBirthYear("");
            Assert.IsFalse(marriageView.marriageValidation.IsValidMaleBirthYear);
            marriageView.iMarriageEditorControl.RequestSetEditorMaleBirthYear("0");
            Assert.IsFalse(marriageView.marriageValidation.IsValidMaleBirthYear);
            marriageView.iMarriageEditorControl.RequestSetEditorMaleBirthYear("10000");
            Assert.IsFalse(marriageView.marriageValidation.IsValidMaleBirthYear);
        }


        #endregion


         

    }


    [TestFixture]
    public class NUnitMarriageAdd : TestData
    {
        MarriageView marriageview = null;
        MarriageView _marriageView2 = null;
        Marriage _marriage = null;
        string witcname1 = "";
        string witsname1 = "";
        string witcname2 = "";
        string witsname2 = "";
        string witcname3 = "";
        string witsname3 = "";
        string witcname4 = "";
        string witsname4 = "";


        [SetUp]
        public void AddMarriage()
        {
            marriageview = new MarriageView();
            marriageview.iMarriageEditorModel.SetIsSecurityEnabled(false);

            Guid locationGuid = Guid.NewGuid();
            Guid femaleId = Guid.NewGuid();
            Guid maleId = Guid.NewGuid();

             witcname1 = "joe1";
             witsname1 = "witness1";
             witcname2 = "joe2";
             witsname2 = "witness2";
             witcname3 = "joe3";
             witsname3 = "witness3";
             witcname4 = "joe4";
             witsname4 = "witness4";

            _marriage = new Marriage();

            //todo move to setup 
            #region setup some test data

            _marriage.FemaleCName = "mary";
            _marriage.FemaleSName = "smith";
            _marriage.FemaleBirthYear = 1800;
            _marriage.FemaleInfo = "whatever";
            _marriage.FemaleLocation = "female location";
            _marriage.FemaleOccupation = "fem occ";
            _marriage.IsBanns = true;
            _marriage.IsLicence = true;
            _marriage.FemaleIsKnownWidow = true;
            _marriage.MaleIsKnownWidower = true;
            _marriage.MarriageLocation = "marloc";
            _marriage.MarriageLocationId = locationGuid;
            _marriage.OrigFemaleSurname = "origfemname";
            _marriage.OrigMaleSurname = "origmalname";
            _marriage.Source = "source";
            _marriage.Date = "1 Jan 1800";
            _marriage.FemaleId = femaleId;
            _marriage.FemaleLocationId = locationGuid;
            _marriage.MaleBirthYear = 1800;
            _marriage.MaleCName = "joe";
            _marriage.MaleId = maleId;
            _marriage.MaleInfo = "maleinfo";
            _marriage.MaleLocation = "malloc";
            _marriage.MaleLocationId = locationGuid;
            _marriage.MaleOccupation = "maleocc";
            _marriage.MaleSName = "bloggs";
            _marriage.MarriageCounty = "yorkshire";
            _marriage.EventPriority = 10;
            _marriage.TotalEvents = 10;
            _marriage.UniqueRef = locationGuid;



            #endregion

         
            marriageview.iMarriageEditorControl.RequestSetEditorFemaleName(_marriage.FemaleCName, _marriage.FemaleSName);
            marriageview.iMarriageEditorControl.RequestSetEditorFemaleBirthYear(_marriage.FemaleBirthYear.GetValueOrDefault().ToString());
            marriageview.iMarriageEditorControl.RequestSetEditorFemaleInfo(_marriage.FemaleInfo);
            marriageview.iMarriageEditorControl.RequestSetEditorFemaleLocation(_marriage.FemaleLocation);
            marriageview.iMarriageEditorControl.RequestSetEditorFemaleLocationId(_marriage.FemaleLocationId.GetValueOrDefault());
            marriageview.iMarriageEditorControl.RequestSetEditorFemaleOccupation(_marriage.FemaleOccupation);

            marriageview.iMarriageEditorControl.RequestSetEditorIsBanns(_marriage.IsBanns.GetValueOrDefault());
            marriageview.iMarriageEditorControl.RequestSetEditorIsLicence(_marriage.IsLicence.GetValueOrDefault());
            marriageview.iMarriageEditorControl.RequestSetEditorIsWidow(_marriage.FemaleIsKnownWidow.GetValueOrDefault());
            marriageview.iMarriageEditorControl.RequestSetEditorIsWidower(_marriage.MaleIsKnownWidower.GetValueOrDefault());
            marriageview.iMarriageEditorControl.RequestSetEditorLocation(_marriage.MarriageLocation);
            marriageview.iMarriageEditorControl.RequestSetEditorMaleBirthYear(_marriage.MaleBirthYear.GetValueOrDefault().ToString());
            marriageview.iMarriageEditorControl.RequestSetEditorMaleInfo(_marriage.MaleInfo);
            marriageview.iMarriageEditorControl.RequestSetEditorMaleLocation(_marriage.MaleLocation);
            marriageview.iMarriageEditorControl.RequestSetEditorMaleLocationId(_marriage.MaleLocationId.GetValueOrDefault());
            marriageview.iMarriageEditorControl.RequestSetEditorMaleName(_marriage.MaleCName, _marriage.MaleSName);
            marriageview.iMarriageEditorControl.RequestSetEditorMaleOccupation(_marriage.MaleOccupation);
            marriageview.iMarriageEditorControl.RequestSetEditorMarriageCounty(_marriage.MarriageCounty);
            marriageview.iMarriageEditorControl.RequestSetEditorMarriageDate(_marriage.Date);
            marriageview.iMarriageEditorControl.RequestSetEditorMarriageLocationId(_marriage.MarriageLocationId.GetValueOrDefault());
            marriageview.iMarriageEditorControl.RequestSetEditorSource(_marriage.Source);
            marriageview.iMarriageEditorControl.RequestSetEditorWitness1(witsname1);
            marriageview.iMarriageEditorControl.RequestSetEditorWitness1CName(witcname1);
            marriageview.iMarriageEditorControl.RequestSetEditorWitness2(witsname2);
            marriageview.iMarriageEditorControl.RequestSetEditorWitness2CName(witcname2);
            marriageview.iMarriageEditorControl.RequestSetEditorWitness3(witsname3);
            marriageview.iMarriageEditorControl.RequestSetEditorWitness3CName(witcname3);
            marriageview.iMarriageEditorControl.RequestSetEditorWitness4(witsname4);
            marriageview.iMarriageEditorControl.RequestSetEditorWitness4CName(witcname4);
            marriageview.iMarriageEditorModel.SetEditorOrigFemaleName(_marriage.OrigFemaleSurname);
            marriageview.iMarriageEditorModel.SetEditorOrigMaleName(_marriage.OrigMaleSurname);

            marriageview.iMarriageEditorModel.SetEditorEventPriority(_marriage.EventPriority.GetValueOrDefault());
            marriageview.iMarriageEditorModel.SetEditorTotalEvents(_marriage.TotalEvents.GetValueOrDefault());
            marriageview.iMarriageEditorModel.SetEditorUniqueRef(_marriage.UniqueRef.GetValueOrDefault());
            marriageview.iMarriageEditorModel.SetEditorFemaleId(_marriage.FemaleId.GetValueOrDefault());
            marriageview.iMarriageEditorModel.SetEditorMaleId(_marriage.MaleId.GetValueOrDefault());


            Assert.IsTrue(marriageview.iMarriageEditorModel.IsValidEntry);

            marriageview.iMarriageEditorControl.RequestInsert();

          

            //MarriagesBLL marriagesBll = new MarriagesBLL ();

            _marriageView2 = new MarriageView();
            _marriageView2.iMarriageEditorModel.SetIsSecurityEnabled(false);
            _marriageView2.iMarriageEditorControl.RequestSetSelectedId(marriageview.iMarriageEditorModel.SelectedRecordId);

            _marriageView2.iMarriageEditorControl.RequestRefresh();


            
        }

        [Test(Description = "RecordsPresent")]
        public void RecordsPresent()
        {
            Assert.IsTrue(marriageview.iMarriageEditorModel.SelectedRecordId != Guid.Empty);
            Assert.IsTrue(_marriageView2.iMarriageEditorModel.EditorLocation != "");
        }


        [Test(Description = "CheckSavedData")]
        public void CheckSavedData()
        { 
            Assert.IsTrue(_marriage.Date == _marriageView2.iMarriageEditorModel.EditorDateMarriageString);
            Assert.IsTrue(_marriage.EventPriority == _marriageView2.iMarriageEditorModel.EditorEventPriority);
            Assert.IsTrue(_marriage.FemaleBirthYear.GetValueOrDefault().ToString() == _marriageView2.iMarriageEditorModel.EditorFemaleBirthYear);
            Assert.IsTrue(_marriage.FemaleCName == _marriageView2.iMarriageEditorModel.EditorFemaleCName);

            Assert.IsTrue(_marriage.FemaleId.GetValueOrDefault() == _marriageView2.iMarriageEditorModel.EditorFemaleId);
            Assert.IsTrue(_marriage.FemaleInfo == _marriageView2.iMarriageEditorModel.EditorFemaleInfo);
            Assert.IsTrue(_marriage.FemaleIsKnownWidow == _marriageView2.iMarriageEditorModel.EditorIsWidow);
            Assert.IsTrue(_marriage.FemaleLocation == _marriageView2.iMarriageEditorModel.EditorFemaleLocation);
            Assert.IsTrue(_marriage.FemaleLocationId == _marriageView2.iMarriageEditorModel.EditorFemaleLocationId);
            Assert.IsTrue(_marriage.FemaleOccupation == _marriageView2.iMarriageEditorModel.EditorFemaleOccupation);
            Assert.IsTrue(_marriage.FemaleSName == _marriageView2.iMarriageEditorModel.EditorFemaleSName);
            Assert.IsTrue(_marriage.IsBanns == _marriageView2.iMarriageEditorModel.EditorIsBanns);
            Assert.IsTrue(_marriage.IsLicence == _marriageView2.iMarriageEditorModel.EditorIsLicence);
            Assert.IsTrue(_marriage.MaleBirthYear.GetValueOrDefault().ToString() == _marriageView2.iMarriageEditorModel.EditorMaleBirthYear);
            Assert.IsTrue(_marriage.MaleCName == _marriageView2.iMarriageEditorModel.EditorMaleCName);

            Assert.IsTrue(_marriage.MaleId == _marriageView2.iMarriageEditorModel.EditorMaleId);
            Assert.IsTrue(_marriage.MaleInfo == _marriageView2.iMarriageEditorModel.EditorMaleInfo);
            Assert.IsTrue(_marriage.MaleIsKnownWidower == _marriageView2.iMarriageEditorModel.EditorIsWidower);
            Assert.IsTrue(_marriage.MaleLocation == _marriageView2.iMarriageEditorModel.EditorMaleLocation);
            Assert.IsTrue(_marriage.MaleLocationId == _marriageView2.iMarriageEditorModel.EditorMaleLocationId);
            Assert.IsTrue(_marriage.MaleOccupation == _marriageView2.iMarriageEditorModel.EditorMaleOccupation);

            Assert.IsTrue(_marriage.MaleSName == _marriageView2.iMarriageEditorModel.EditorMaleSName);
            Assert.IsTrue(_marriage.MarriageCounty == _marriageView2.iMarriageEditorModel.EditorMarriageCounty);
            Assert.IsTrue(_marriage.MarriageLocation == _marriageView2.iMarriageEditorModel.EditorLocation);
            Assert.IsTrue(_marriage.MarriageLocationId == _marriageView2.iMarriageEditorModel.EditorMarriageLocationId);

            Assert.IsTrue(_marriage.OrigFemaleSurname == _marriageView2.iMarriageEditorModel.EditorOrigFemaleName);
            Assert.IsTrue(_marriage.OrigMaleSurname == _marriageView2.iMarriageEditorModel.EditorOrigMaleName);

            Assert.IsTrue(_marriage.Source == _marriageView2.iMarriageEditorModel.EditorSource);

            Assert.IsTrue(_marriage.TotalEvents == _marriageView2.iMarriageEditorModel.EditorTotalEvents);
            Assert.IsTrue(_marriage.EventPriority == _marriageView2.iMarriageEditorModel.EditorEventPriority);
            Assert.IsTrue(_marriage.UniqueRef == _marriageView2.iMarriageEditorModel.EditorUniqueRef);

            


        }





        [TearDown]
        public void TearDown()
        {

            _marriageView2.iMarriageEditorControl.RequestDelete();

            _marriageView2.iMarriageEditorControl.RequestRefresh();

            Assert.IsTrue(_marriageView2.iMarriageEditorModel.EditorLocation == "");

            MarriagesBLL marriagesBLL = new MarriagesBLL();
            MarriageWitnessesBLL mwitsbll = new MarriageWitnessesBLL();

            mwitsbll.DeleteWitnessesForMarriage(_marriageView2.iMarriageEditorModel.SelectedRecordId);

            Marriage _marriage = marriagesBLL.ModelContainer.Marriages.Where(m => m.Marriage_Id == _marriageView2.iMarriageEditorModel.SelectedRecordId).FirstOrDefault();

            if (_marriage != null)
            {
                marriagesBLL.ModelContainer.Marriages.DeleteObject(_marriage);
                marriagesBLL.ModelContainer.SaveChanges();
            }

            
        }
    }


    [TestFixture]
    public class NUnitMarriageFilter : TestData
    {
        MarriageFilterView marriageFilterView = null;
        List<Marriage> newMarriages = null;
        List<Guid> sourcesList = null;
        List<Person> witList = null;

        Marriage _marriage1 = null;
        Marriage _marriage2 = null;
        Marriage _marriage3 = null;
        Marriage _marriage4 = null;
        Marriage _marriage5 = null;
        Marriage _marriage6 = null;


        Source _source1 = null;
        Source _source2 = null;
        Source _source3 = null;

        Person _person1 = null;
        Person _person2 = null;
        Person _person3 = null;


        [SetUp]
        public void SetUpMarriageFilter()
        {
            Debug.WriteLine("SetUp");
            
            marriageFilterView = new MarriageFilterView();
            
            marriageFilterView.iMarriageFilterModel.SetIsSecurityEnabled(false);
            
            MarriagesBLL marriagesBLL = new MarriagesBLL();
            SourceBLL sourceBLL = new SourceBLL();
            SourceMappingsBLL smap = new SourceMappingsBLL();
            DeathsBirthsBLL personsBLL = new DeathsBirthsBLL();
            MarriageWitnessesBLL marriageWitnessesBLL = new MarriageWitnessesBLL();

            
            _marriage1 = marriagesBLL.CreateBasicMarriage("testgcname1", "testgsname1", "testbcname1", "testbsname1", "testlocat", 1800);

            _marriage1.FemaleOccupation = "femocc";

            _marriage2 = marriagesBLL.CreateBasicMarriage("testgcname2", "testgsname2", "testbcname2", "testbsname2", "testlocat", 1800);
            _marriage1.MaleOccupation = "maleocc";

       
            _marriage3 = marriagesBLL.CreateBasicMarriage("testgcname3", "testgsname3", "testbcname3", "testbsname3", "testlocat", 1800);
            _marriage3.MaleInfo = "malinf";

           
            _marriage4 = marriagesBLL.CreateBasicMarriage("testgcname4", "testgsname4", "testbcname4", "testbsname4", "testlocat", 1800);
            
            _marriage5 = marriagesBLL.CreateBasicMarriage("testgcname5", "testgsname5", "testbcname5", "testbsname5", "testlocat", 1800);
    
            _marriage6 = marriagesBLL.CreateBasicMarriage("testgcname6", "testgsname6", "testbcname6", "testbsname6", "testlocat", 1800);

            _source1 = sourceBLL.CreateBasicSource("test_src1", 1800, "test_desc1");
            _source2 = sourceBLL.CreateBasicSource("test_src2", 1800, "test_desc2");
            _source3 = sourceBLL.CreateBasicSource("test_src3", 1800, "test_desc3");
            
            sourcesList = new List<Guid>();
            sourcesList.Clear();
            sourcesList.Add(_source1.SourceId);
           
            smap.WriteMarriageSources(_marriage1.Marriage_Id, sourcesList, 1);

            sourcesList.Clear();
            sourcesList.Add(_source2.SourceId);
            smap.WriteMarriageSources(_marriage2.Marriage_Id, sourcesList, 1);

            sourcesList.Clear();
            sourcesList.Add(_source3.SourceId);
            smap.WriteMarriageSources(_marriage3.Marriage_Id, sourcesList, 1);




            _person1 = personsBLL.CreateBasicPerson("testcnamewit1", "testsnamewit1", "testlocat", 1800);

            _person2 = personsBLL.CreateBasicPerson("testcnamewit2", "testsnamewit2", "testlocat", 1800);
            _person2.DeathLocation = "person 2 loc";

            _person3 = personsBLL.CreateBasicPerson("testcnamewit3", "testsnamewit3", "testlocat", 1800);

            witList = new List<Person>();
            witList.Clear();
            witList.Add(_person1);
            marriageWitnessesBLL.InsertWitnessesForMarriage(_marriage1.Marriage_Id,  witList);


            witList.Clear();
            witList.Add(_person2);
            marriageWitnessesBLL.InsertWitnessesForMarriage(_marriage2.Marriage_Id, witList);


            witList.Clear();
            witList.Add(_person3);
            marriageWitnessesBLL.InsertWitnessesForMarriage(_marriage3.Marriage_Id, witList);

        }

        [Test(Description = "CheckSingleDelete")]
        public void CheckSingleDelete()
        {
            marriageFilterView.iMarriageFilterModel.SetFilterMaleName("testgcname", "testgsname");
            marriageFilterView.iMarriageFilterModel.SetFilterLocation("testlocat");
            marriageFilterView.iMarriageFilterModel.SetFilterLowerBound("1795");
            marriageFilterView.iMarriageFilterModel.SetFilterUpperBound("1805");
            marriageFilterView.iMarriageFilterModel.Refresh();

            Assert.IsTrue(marriageFilterView.iMarriageFilterModel.MarriagesTable.Count == 6);


            marriageFilterView.iMarriageFilterModel.SetSelectedRecordId(_marriage1.Marriage_Id);
            marriageFilterView.iMarriageFilterModel.DeleteSelectedRecords();

            Assert.IsTrue(marriageFilterView.iMarriageFilterModel.MarriagesTable.Count == 5);
        }


        [Test(Description = "CheckMultiDelete")]
        public void CheckMultiDelete()
        {
            marriageFilterView.iMarriageFilterModel.SetFilterMaleName("testgcname", "testgsname");
            marriageFilterView.iMarriageFilterModel.SetFilterLocation("testlocat");
            marriageFilterView.iMarriageFilterModel.SetFilterLowerBound("1795");
            marriageFilterView.iMarriageFilterModel.SetFilterUpperBound("1805");
            marriageFilterView.iMarriageFilterModel.Refresh();

            Assert.IsTrue(marriageFilterView.iMarriageFilterModel.MarriagesTable.Count == 6);


            List<Guid> selectedIds = new List<Guid>();
            selectedIds.Add(_marriage1.Marriage_Id);
            selectedIds.Add(_marriage2.Marriage_Id);
            selectedIds.Add(_marriage3.Marriage_Id);

            marriageFilterView.iMarriageFilterModel.SetSelectedRecordIds(selectedIds);
             


            marriageFilterView.iMarriageFilterModel.DeleteSelectedRecords();

            Assert.IsTrue(marriageFilterView.iMarriageFilterModel.MarriagesTable.Count == 3);
        }

        [Test(Description = "CheckControlMultiDelete")]
        public void CheckControlMultiDelete()
        {
            // the way to select records is handled differently in the model and the controller

            marriageFilterView.iMarriageFilterModel.SetFilterMaleName("testgcname", "testgsname");
            marriageFilterView.iMarriageFilterModel.SetFilterLocation("testlocat");
            marriageFilterView.iMarriageFilterModel.SetFilterLowerBound("1795");
            marriageFilterView.iMarriageFilterModel.SetFilterUpperBound("1805");
            marriageFilterView.iMarriageFilterModel.Refresh();

            Assert.IsTrue(marriageFilterView.iMarriageFilterModel.MarriagesTable.Count == 6);


            marriageFilterView.iMarriageFilterModel.SetSelectedRecordIds(_marriage1.Marriage_Id);
            marriageFilterView.iMarriageFilterModel.SetSelectedRecordIds(_marriage2.Marriage_Id);
            marriageFilterView.iMarriageFilterModel.SetSelectedRecordIds(_marriage3.Marriage_Id);

            marriageFilterView.iMarriageFilterModel.DeleteSelectedRecords();

            Assert.IsTrue(marriageFilterView.iMarriageFilterModel.MarriagesTable.Count == 3);
        }



        [Test(Description = "CheckRefresh")]
        public void CheckRefresh()
        {
            marriageFilterView.iMarriageFilterModel.SetFilterMaleName("testgcname", "testgsname");
            marriageFilterView.iMarriageFilterModel.SetFilterLocation("testlocat");
            marriageFilterView.iMarriageFilterModel.SetFilterLowerBound("1795");
            marriageFilterView.iMarriageFilterModel.SetFilterUpperBound("1805");
            marriageFilterView.iMarriageFilterModel.Refresh();

            Assert.IsTrue(marriageFilterView.iMarriageFilterModel.MarriagesTable.Count == 6);


            //marriageFilterView.iMarriageFilterModel.SetSelectedRecordId(_marriage1.Marriage_Id);
            //marriageFilterView.iMarriageFilterModel.DeleteSelectedRecords();


        }

        [Test(Description = "CheckSavedData")]
        public void CheckSavedData()
        {
            Assert.IsNotNull(_marriage1);
            Assert.IsNotNull(_marriage2);
            Assert.IsNotNull(_marriage3);
            Assert.IsNotNull(_marriage4);
            Assert.IsNotNull(_marriage5);
            Assert.IsNotNull(_marriage6);

            //sourceMappingView1.iSourceMappingControl.
            // iControl.RequestSetSelectedIds((Guid)ListView1.DataKeys[ListView1.SelectedIndex].Values["Person_id"]);
        }


        [Test(Description = "TestGroupDuplicates")]
        public void TestGroupDuplicates()
        {
            //DeathsBirthsBLL personsBLL = new DeathsBirthsBLL();
            MarriagesBLL marriagesBLL = new MarriagesBLL();


            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage1.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage2.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage3.Marriage_Id);

            marriageFilterView.iMarriageFilterControl.RequestSetSelectedDuplicateMarriage();

            marriagesBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins, _marriage1);
            marriagesBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins, _marriage2);
            marriagesBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins, _marriage3);

            newMarriages = marriagesBLL.ModelContainer.Marriages.Where(p => p.UniqueRef == _marriage1.UniqueRef).OrderBy(po => po.EventPriority).ToList();

            Assert.IsTrue(newMarriages.Count == 3);

            Assert.IsTrue(newMarriages[0].EventPriority == 0);
            Assert.IsTrue(newMarriages[1].EventPriority == 1);
            Assert.IsTrue(newMarriages[2].EventPriority == 2);

            Assert.IsTrue(newMarriages[0].TotalEvents == 3);

            //// selecting the same id twice removes it
            ////  personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person1.Person_id);
            ////  personFilterView.iDeathBirthFilterControl.RequestSetSelectedIds(_person2.Person_id);

            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage3.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage4.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage5.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage6.Marriage_Id);

            marriageFilterView.iMarriageFilterControl.RequestSetSelectedDuplicateMarriage();

            marriagesBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins, _marriage1);



            newMarriages = marriagesBLL.ModelContainer.Marriages.Where(p => p.UniqueRef == _marriage1.UniqueRef).OrderBy(po => po.EventPriority).ToList();

            Assert.IsTrue(newMarriages.Count == 6);

            Assert.IsTrue(newMarriages[0].EventPriority == 0);
            Assert.IsTrue(newMarriages[1].EventPriority == 1);
            Assert.IsTrue(newMarriages[2].EventPriority == 2);
            Assert.IsTrue(newMarriages[3].EventPriority == 3);
            Assert.IsTrue(newMarriages[4].EventPriority == 4);
            Assert.IsTrue(newMarriages[5].EventPriority == 5);

            Assert.IsTrue(newMarriages[0].TotalEvents == 6);
        }

        [Test(Description = "TestRemoveDuplicate")]
        public void TestRemoveDuplicate()
        {
            MarriagesBLL marriagesBLL = new MarriagesBLL();

           

            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage1.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage2.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage3.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage4.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage5.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage6.Marriage_Id);

            marriageFilterView.iMarriageFilterControl.RequestSetSelectedDuplicateMarriage();

            marriagesBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins, _marriage1);

            Assert.IsTrue(marriageFilterView.iMarriageFilterModel.SelectedRecordIds.Count==0);

            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage1.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetRemoveSelectedFromDuplicateList();

            marriagesBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins, _marriage2);



            newMarriages = marriagesBLL.ModelContainer.Marriages.Where(p => p.UniqueRef == _marriage2.UniqueRef).OrderBy(po => po.EventPriority).ToList();

            Assert.IsTrue(newMarriages.Count == 5);


            Marriage _marriage = this.newMarriages.Where(p => p.EventPriority == 0).FirstOrDefault();
        }


        [Test(Description = "TestMergeDuplicates")]
        public void TestMergeDuplicates()
        {
            MarriagesBLL marriagesBLL = new MarriagesBLL();

            


            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage1.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage2.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage3.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage4.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage5.Marriage_Id);
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedIds(_marriage6.Marriage_Id);
            
            marriageFilterView.iMarriageFilterControl.RequestSetSelectedDuplicateMarriage();

            marriagesBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins, _marriage1);



            newMarriages = marriagesBLL.ModelContainer.Marriages.Where(p => p.UniqueRef == _marriage1.UniqueRef).OrderBy(po => po.EventPriority).ToList();


            Marriage _marriage = this.newMarriages.Where(p => p.EventPriority == 0).FirstOrDefault();



            Assert.IsNotNull(_marriage);
         
            MarriagesFilterModel.MergeDuplicateRecord(_marriage);

            MarriageWitnessesBLL marriageWitnessesBLL = new MarriageWitnessesBLL();
            SourceMappingsBLL smaps = new SourceMappingsBLL();


            Assert.IsTrue( _marriage.FemaleOccupation == "femocc");
            Assert.IsTrue( _marriage.MaleOccupation == "maleocc");
            Assert.IsTrue(_marriage.MaleInfo == "malinf");


            List<Person> _persons = marriageWitnessesBLL.GetWitnessesForMarriage(_marriage.Marriage_Id).OrderBy(w=>w.ChristianName).ToList();

            Assert.IsTrue(_persons.Count == 3);

            Assert.IsTrue(_persons[0].ChristianName == "testcnamewit1");
            Assert.IsTrue(_persons[1].ChristianName == "testcnamewit2");
            Assert.IsTrue(_persons[2].ChristianName == "testcnamewit3");

            List<string> sources = smaps.GetByMarriageIdOrPersonId2(_marriage.Marriage_Id).Select(sm=>sm.Source.SourceRef).ToList();

            Assert.IsTrue(sources.Count == 3);

            Assert.IsTrue(sources.Contains("test_src1"));
            Assert.IsTrue(sources.Contains("test_src2"));
            Assert.IsTrue(sources.Contains("test_src3"));



        }





        [TearDown]
        public void TearDown()
        {
            MarriagesBLL marriagesBLL = new MarriagesBLL();
            MarriageWitnessesBLL marriageWitnessesBLL = new MarriageWitnessesBLL();
            SourceMappingsBLL smaps = new SourceMappingsBLL();
            DeathsBirthsBLL personsBLL = new DeathsBirthsBLL();

            smaps.DeleteSourcesForPersonOrMarriage(_marriage1.Marriage_Id);
            smaps.DeleteSourcesForPersonOrMarriage(_marriage2.Marriage_Id);
            smaps.DeleteSourcesForPersonOrMarriage(_marriage3.Marriage_Id);
            smaps.DeleteSourcesForPersonOrMarriage(_marriage4.Marriage_Id);
            smaps.DeleteSourcesForPersonOrMarriage(_marriage5.Marriage_Id);
            smaps.DeleteSourcesForPersonOrMarriage(_marriage6.Marriage_Id);


            marriageWitnessesBLL.DeleteWitnessEntriesForMarriage(_marriage1.Marriage_Id);
            marriageWitnessesBLL.DeleteWitnessEntriesForMarriage(_marriage2.Marriage_Id);
            marriageWitnessesBLL.DeleteWitnessEntriesForMarriage(_marriage3.Marriage_Id);
            marriageWitnessesBLL.DeleteWitnessEntriesForMarriage(_marriage4.Marriage_Id);
            marriageWitnessesBLL.DeleteWitnessEntriesForMarriage(_marriage5.Marriage_Id);
            marriageWitnessesBLL.DeleteWitnessEntriesForMarriage(_marriage6.Marriage_Id);

            

            marriagesBLL.ModelContainer.Marriages.DeleteObject(_marriage1);
            marriagesBLL.ModelContainer.Marriages.DeleteObject(_marriage2);
            marriagesBLL.ModelContainer.Marriages.DeleteObject(_marriage3);
            marriagesBLL.ModelContainer.Marriages.DeleteObject(_marriage4);
            marriagesBLL.ModelContainer.Marriages.DeleteObject(_marriage5);
            marriagesBLL.ModelContainer.Marriages.DeleteObject(_marriage6);

         //   personsBLL.ModelContainer.Refresh(System.Data.Objects.RefreshMode.StoreWins,);

            personsBLL.ModelContainer.Persons.DeleteObject(_person1);
            personsBLL.ModelContainer.Persons.DeleteObject(_person2);
            personsBLL.ModelContainer.Persons.DeleteObject(_person3);


            marriagesBLL.ModelContainer.Sources.DeleteObject(_source1);
            marriagesBLL.ModelContainer.Sources.DeleteObject(_source2);
            marriagesBLL.ModelContainer.Sources.DeleteObject(_source3);

            marriagesBLL.ModelContainer.SaveChanges();
        }
    }

}
