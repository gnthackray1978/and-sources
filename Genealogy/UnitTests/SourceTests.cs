using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using GedItter.ModelObjects;
using GedItter.ControlObjects;
using TDBCore.EntityModel;
using NUnit.Framework;
using System.Collections;
using GedItter.BLL;
using GedItter.BirthDeathRecords;
using TDBCore.Types;
using GedItter.BirthDeathRecords.BLL;
using GedItter.MarriageRecords;
using GedItter.MarriageRecords.BLL;
using System.Diagnostics;


namespace UnitTests
{
    // create sourcse
        //[Test(Description = "Addition Test")]
        //[TestCaseSource("TestCaseDataList")]
        //public void CheckFilterSourceRef(string testVal)
        //{

        //    SourceFilterView sourceFilterView = new SourceFilterView();

        //    sourceFilterView.iSourceFilterControl.RequestSetFilterSourceRef(testVal);

        //    Assert.IsFalse(sourceFilterView.iSourceFilterModel.IsValidSourceRef);
          
        //}

        //public IEnumerable TestCaseDataList 
        //{ 
        //    get 
        //    { 
        //      //  List testCaseDataList = new TestDataReader().ReadExcelData(@"D:\Data\TestData.xls"); 
        //        foreach (string testCaseData in MarriageFieldList) 
        //        { 
        //            yield return testCaseData; 
        //        } 
        //    } 
        //}

    #region source view
    public class SourceView : ISourceEditorView
    {
        public ISourceEditorControl iSourceEditorControl = null;
        public ISourceEditorModel iSourceEditorModel = null;

        public bool InvalidSourceDate { get; set; }

        public bool InvalidSourceDateTO { get; set; }

        public bool InvalidSourceDesc { get; set; }

        public bool InvalidSourceRef { get; set; }

        public bool InvalidSourceOrigLoc { get; set; }

        public bool InvalidUser { get; set; }

        public SourceView()
        {
            iSourceEditorModel = new SourceEditorModel();
            iSourceEditorControl = new SourceEditorControl();

            if (iSourceEditorModel != null)
                iSourceEditorControl.SetModel(iSourceEditorModel);

            if (iSourceEditorControl != null)
                iSourceEditorControl.SetView(this);
        }


        #region ISourceEditorView Members

        public void ShowInvalidSourceDate(bool valid)
        {
            this.InvalidSourceDate = valid;
        }

        public void ShowInvalidSourceDateTo(bool valid)
        {
            this.InvalidSourceDateTO = valid;

        }

        public void ShowInvalidSourceDescription(bool valid)
        {
            this.InvalidSourceDesc = valid;
        }

        public void ShowInvalidSourceRef(bool valid)
        {
            this.InvalidSourceRef = valid;
        }

        public void ShowInvalidSourceOriginalLocation(bool valid)
        {
            this.InvalidSourceOrigLoc = valid;
        }

        public void ShowInvalidUser(bool valid)
        {
            this.InvalidUser = valid;
        }

        #endregion



        public void Update<T>(T paramModel)
        {
            
        }
    }
    #endregion

    #region sourcefilterview
    //public class SourceFilterView : ISourceFilterView
    //{
    //    public event EventHandler ShowEditor;
    //    public ISourceFilterModel iSourceFilterModel = null;
    //    public ISourceFilterControl iSourceFilterControl = null;
    //    private int sourceCount;
    //    private string lowerDateRangeLower;
    //    private string lowerDateRangeUpper;
    //    private string upperDateRangeLower;
    //    private string upperDateRangeUpper;
    //    private bool isValidDescription;
    //    private bool isValidReference;
    //    private string sourceRef;
    //    private string sourceDesc;
    //    private bool? isViewed;
    //    private bool? isThackrayFound;
    //    private bool? isCopyHeld;

    //    private string sourceFileCount;
    //    private string sourceOriginalLocation;
    //    private List<int> sourceTypes;

    //    private bool invalidSourceDateUpper;

    //    private bool invalidSourceDateLower;

    //    private bool invalidSourceDateUpperTo;

    //    private bool invalidSourceDateLowerTo;


    //    #region props

    //    public bool InvalidSourceDateUpper 
    //    {
    //        get { return invalidSourceDateUpper; }
    //    }

    //    public bool InvalidSourceDateLower
    //    {
    //        get { return invalidSourceDateLower; }
    //    }

    //    public bool InvalidSourceDateUpperTo
    //    {
    //        get { return invalidSourceDateUpperTo; }
    //    }

    //    public bool InvalidSourceDateLowerTo
    //    {
    //        get { return invalidSourceDateLowerTo; }
    //    }



    //    public bool IsValidDescription
    //    {
    //        get { return isValidDescription; }
             
    //    }

    //    public bool IsValidReference
    //    {
    //        get { return isValidReference; }
           
    //    }



    //    public int SourceCount
    //    {
    //        get { return sourceCount; }
    //        set { sourceCount = value; }
    //    }


    //    public string LowerDateRangeLower
    //    {
    //        get { return lowerDateRangeLower; }
    //        set { lowerDateRangeLower = value; }
    //    }


    //    public string LowerDateRangeUpper
    //    {
    //        get { return lowerDateRangeUpper; }
    //        set { lowerDateRangeUpper = value; }
    //    }


    //    public string UpperDateRangeLower
    //    {
    //        get { return upperDateRangeLower; }
    //        set { upperDateRangeLower = value; }
    //    }



    //    public string UpperDateRangeUpper
    //    {
    //        get { return upperDateRangeUpper; }
    //        set { upperDateRangeUpper = value; }
    //    }



    //    public string SourceRef
    //    {
    //        get { return sourceRef; }
    //        set { sourceRef = value; }
    //    }



    //    public string SourceDesc
    //    {
    //        get { return sourceDesc; }
    //        set { sourceDesc = value; }
    //    }




    //    public bool? IsCopyHeld
    //    {
    //        get { return isCopyHeld; }
    //        set { isCopyHeld = value; }
    //    }


    //    public bool? IsThackrayFound
    //    {
    //        get { return isThackrayFound; }
    //        set { isThackrayFound = value; }
    //    }



    //    public bool? IsViewed
    //    {
    //        get { return isViewed; }
    //        set { isViewed = value; }
    //    }

    //    public string SourceFileCount
    //    {
    //        get { return sourceFileCount; }
    //        set { sourceFileCount = value; }
    //    }

    //    public string SourceOrigLocation
    //    {
    //        get { return sourceOriginalLocation; }
    //        set { sourceOriginalLocation = value; }
    //    }



    //    public List<int> SourceTypes
    //    {
    //        get { return sourceTypes; }
    //        set { sourceTypes = value; }
    //    }


    //    #endregion



    //    #region set valid functions

    //    public void ShowInvalidSourceDateUpperBoundWarning(bool valid)
    //    {
    //        invalidSourceDateUpper = valid;
    //    }

    //    public void ShowInvalidSourceDateLowerBoundWarning(bool valid)
    //    {
    //        this.invalidSourceDateLower = valid;
    //    }

    //    public void ShowInvalidSourceToDateUpperBoundWarning(bool valid)
    //    {
    //        this.invalidSourceDateUpperTo = valid;
    //    }

    //    public void ShowInvalidSourceToDateLowerBoundWarning(bool valid)
    //    {
    //        this.invalidSourceDateLowerTo = valid;
    //    }

    //    public void ShowInvalidSourceDescriptionWarning(bool valid)
    //    {
    //        this.isValidDescription = valid;
    //    }

    //    public void ShowInvalidSourceRefWarning(bool valid)
    //    {
    //        this.isValidReference = valid;
    //    }

    //    #endregion


    //    public SourceFilterView()
    //    { 
    //        iSourceFilterModel = new SourceFilterModel();
    //        iSourceFilterControl = new SourceFilterControl();

    //        if (iSourceFilterModel != null)
    //            iSourceFilterControl.SetModel(iSourceFilterModel);

    //        if (iSourceFilterControl != null)
    //            iSourceFilterControl.SetView(this);
    //    }

    //    //public void SetUp()
    //    //{
           

    //    //}



    //    public IList<TDBCore.EntityModel.Source> GetTable(string param, string SortExpression)
    //    {
    //        return iSourceFilterModel.SourcesDataTable; 
    //    }



    //    public void Update<T>(T paramModel)
    //    {
    //        iSourceFilterModel = (ISourceFilterModel)paramModel;

    //        this.sourceCount = iSourceFilterModel.SourcesDataTable.Count;

    //        this.lowerDateRangeLower = iSourceFilterModel.FilterSourceDateLowerBound;
    //        this.lowerDateRangeUpper = iSourceFilterModel.FilterSourceToDateLowerBound;
            
    //        this.upperDateRangeLower = iSourceFilterModel.FilterSourceDateUpperBound;
    //        this.upperDateRangeUpper= iSourceFilterModel.FilterSourceToDateUpperBound;

    //        this.sourceRef = iSourceFilterModel.FilterSourceRef;
    //        this.sourceDesc = iSourceFilterModel.FilterSourceDescription;

    //        this.isCopyHeld = iSourceFilterModel.FilterIsCopyHeld;
    //        this.isThackrayFound = iSourceFilterModel.FilterIsThackrayFound;
    //        this.isViewed = iSourceFilterModel.FilterIsViewed;

    //        this.sourceFileCount = iSourceFilterModel.FilterSourceFileCount.ToString();
    //    }



    //    public void Refresh()
    //    {

    //        iSourceFilterControl.RequestSetFilterSourceRef(this.sourceRef);
    //        iSourceFilterControl.RequestSetFilterSourceDescription(this.sourceDesc);
    //        iSourceFilterControl.RequestSetFilterSourceOriginalLocation(this.sourceOriginalLocation);
    //        iSourceFilterControl.RequestSetFilterSourceDateLowerBound(lowerDateRangeLower);
    //        iSourceFilterControl.RequestSetFilterSourceToDateLowerBound(lowerDateRangeUpper);
    //        iSourceFilterControl.RequestSetFilterSourceDateUpperBound(upperDateRangeLower);
    //        iSourceFilterControl.RequestSetFilterSourceToDateUpperBound(upperDateRangeUpper);


    //        if(this.IsThackrayFound.HasValue)
    //            iSourceFilterControl.RequestSetFilterIsThackrayFound(this.IsThackrayFound, true);
    //        else
    //            iSourceFilterControl.RequestSetFilterIsThackrayFound(this.IsThackrayFound, false);


    //        if (this.IsCopyHeld.HasValue)
    //            iSourceFilterControl.RequestSetFilterIsCopyHeld(this.IsCopyHeld, true);
    //        else
    //            iSourceFilterControl.RequestSetFilterIsCopyHeld(this.IsCopyHeld, false);


    //        if (this.IsViewed.HasValue)
    //            iSourceFilterControl.RequestSetFilterIsViewed(this.IsViewed, true);
    //        else
    //            iSourceFilterControl.RequestSetFilterIsViewed(this.IsViewed, false);

    //        iSourceFilterControl.RequestSetFilterSourceFileCount(this.SourceFileCount, true);
    //        iSourceFilterControl.RequestSetFilterSourceTypeList(this.sourceTypes);
    //    }





     
    //}
    #endregion



    #region NUnitSourceTests
    [TestFixture]
    public class NUnitSourceTests :TestData
    {
        private List<string> MarriageFieldList = new List<string>(new string[] { "MaleCName", "MaleSName", "MaleLocation", "MaleInfo", "FemaleCName", "FemaleSName", "FemaleLocation", "FemaleInfo", "Date", "MarriageLocation", 
            "YearIntVal", "MarriageCounty", "Source", "Witness1", "Witness2", "Witness3", "Witness4", "OrigMaleSurname", "OrigFemaleSurname", "MaleOccupation", "FemaleOccupation", "FemaleIsKnownWidow", "MaleIsKnownWidower", "IsBanns",
        "IsLic","SourceId","MaleAge","FemaleAge","FemaleFather","MaleFather","FemaleFatherOccupation","MaleFatherOccupation" , "LocationId" });




        [Test(Description = "InValid SetSourceDateStr")]
        [TestCaseSource("InvalidDates")]
        public void CheckSourceEdInValidDate(string testVal)
        {

            SourceView sourceView = new SourceView();
            sourceView.iSourceEditorControl.RequestSetSourceDateStr(testVal);

            Assert.IsFalse(sourceView.iSourceEditorModel.IsValidSourceDate);
        }

        [Test(Description = "Valid SetSourceDateStr")]
        [TestCaseSource("ValidDates")]
        public void CheckSourceEdValidDate(string testVal)
        {

            SourceView sourceView = new SourceView();
            sourceView.iSourceEditorControl.RequestSetSourceDateStr(testVal);

            Assert.IsTrue(sourceView.iSourceEditorModel.IsValidSourceDate);
        }

        [Test(Description = "InValid SetSourceDateStrTo")]
        [TestCaseSource("InvalidDates")]
        public void CheckSourceEdInValidDateTo(string testVal)
        {

            SourceView sourceView = new SourceView();
            sourceView.iSourceEditorControl.RequestSetSourceDateToStr(testVal);

            Assert.IsFalse(sourceView.iSourceEditorModel.IsValidSourceDateTo);
        }

        [Test(Description = "Valid SetSourceDateStrTo")]
        [TestCaseSource("ValidDates")]
        public void CheckSourceEdValidDateTo(string testVal)
        {

            SourceView sourceView = new SourceView();
            sourceView.iSourceEditorControl.RequestSetSourceDateToStr(testVal);

            Assert.IsTrue(sourceView.iSourceEditorModel.IsValidSourceDateTo);
        }


        [Test(Description = "Valid Desc")]
        public void CheckSourceEdValidDesc()
        {

            SourceView sourceView = new SourceView();
            sourceView.iSourceEditorControl.RequestSetSourceDescription("source description");

            Assert.IsTrue(sourceView.iSourceEditorModel.IsValidSourceDescription);


            sourceView.iSourceEditorControl.RequestSetSourceDescription(this.GetStringByLen(1000));

            Assert.IsTrue(sourceView.iSourceEditorModel.IsValidSourceDescription);
        }


        [Test(Description = "InValid Desc")]
        public void CheckSourceEdInValidDesc()
        {

            SourceView sourceView = new SourceView();
            sourceView.iSourceEditorControl.RequestSetSourceDescription("");

            Assert.IsFalse(sourceView.iSourceEditorModel.IsValidSourceDescription);


            sourceView.iSourceEditorControl.RequestSetSourceDescription(this.GetStringByLen(1001));

            Assert.IsFalse(sourceView.iSourceEditorModel.IsValidSourceDescription);
        }


        [Test(Description = "Valid Orig Loc.")]
        public void CheckSourceEdValidOrigLoc()
        {

            SourceView sourceView = new SourceView();
            sourceView.iSourceEditorControl.RequestSetSourceOriginalLocation("Source Original Location");

            Assert.IsTrue(sourceView.iSourceEditorModel.IsValidSourceOriginalLocation);


            sourceView.iSourceEditorControl.RequestSetSourceOriginalLocation(this.GetStringByLen(100));

            Assert.IsTrue(sourceView.iSourceEditorModel.IsValidSourceOriginalLocation);
        }


        [Test(Description = "InValid Orig Loc.")]
        public void CheckSourceEdInValidOrigLoc()
        {

            SourceView sourceView = new SourceView();
            sourceView.iSourceEditorControl.RequestSetSourceOriginalLocation("");

            Assert.IsFalse(sourceView.iSourceEditorModel.IsValidSourceOriginalLocation);


            sourceView.iSourceEditorControl.RequestSetSourceOriginalLocation(this.GetStringByLen(101));

            Assert.IsFalse(sourceView.iSourceEditorModel.IsValidSourceOriginalLocation);
        }




        [Test(Description = "Valid Source Ref.")]
        public void CheckSourceEdValidSourceRef()
        {

            SourceView sourceView = new SourceView();
            sourceView.iSourceEditorControl.RequestSetSourceRef("Source Ref");

            Assert.IsTrue(sourceView.iSourceEditorModel.IsValidSourceRef);


            sourceView.iSourceEditorControl.RequestSetSourceRef(this.GetStringByLen(500));

            Assert.IsTrue(sourceView.iSourceEditorModel.IsValidSourceRef);
        }


        [Test(Description = "InValid Source Ref.")]
        public void CheckSourceEdInValidSourceRef()
        {

            SourceView sourceView = new SourceView();
            sourceView.iSourceEditorControl.RequestSetSourceRef("");

            Assert.IsFalse(sourceView.iSourceEditorModel.IsValidSourceRef);


            sourceView.iSourceEditorControl.RequestSetSourceRef(this.GetStringByLen(501));

            Assert.IsFalse(sourceView.iSourceEditorModel.IsValidSourceRef);
        }


     
    }

    #endregion


    [TestFixture]
    public class NUnitSourceAdd : TestData
    {
        SourceView _sourceView1 = null;
        SourceView _sourceView2 = null;
        Source _source = null;



        [SetUp]
        public void AddSource()
        {

            


            Debug.WriteLine("AddSource");
            _sourceView1 = new SourceView();
            _sourceView1.iSourceEditorModel.SetIsSecurityEnabled(false);

            _sourceView2 = new SourceView();
            _sourceView2.iSourceEditorModel.SetIsSecurityEnabled(false);

            _source = new Source();

            #region create test data
            _source.DateAdded = DateTime.Today;
            _source.IsCopyHeld = true;
            _source.IsThackrayFound = true;
            _source.IsViewed = true;
            _source.OriginalLocation = "orig_loc";
            _source.SourceDate = 1800;
            _source.SourceDateStr = "1 Jan 1800";
            _source.SourceDateStrTo = "1 Jan 1801";
            _source.SourceDateTo = 1801;
            _source.SourceDescription = "testdesc";
            _source.SourceFileCount = 1;
            _source.SourceNotes = "testnotes";
            _source.SourceRef = "testref";
            _source.UserId = 1;
            #endregion

            _sourceView1.iSourceEditorControl.RequestSetSourceRef(_source.SourceRef);
            _sourceView1.iSourceEditorControl.RequestSetSourceDescription(_source.SourceDescription);
            _sourceView1.iSourceEditorControl.RequestSetSourceOriginalLocation(_source.OriginalLocation);
            _sourceView1.iSourceEditorControl.RequestSetSourceDateStr(_source.SourceDateStr);
            _sourceView1.iSourceEditorControl.RequestSetSourceDateToStr(_source.SourceDateStrTo);
            _sourceView1.iSourceEditorControl.RequestSetIsCopyHeld(_source.IsCopyHeld);
            _sourceView1.iSourceEditorControl.RequestSetIsThackrayFound(_source.IsThackrayFound);
            _sourceView1.iSourceEditorControl.RequestSetIsViewed(_source.IsViewed);
            _sourceView1.iSourceEditorControl.RequestSetSourceNotes(_source.SourceNotes);
            _sourceView1.iSourceEditorControl.RequestSetSourceFileCount(_source.SourceFileCount.GetValueOrDefault().ToString());


            Assert.IsTrue(_sourceView1.iSourceEditorModel.IsValidEntry);

            _sourceView1.iSourceEditorControl.RequestInsert();


            Assert.IsFalse(_sourceView1.iSourceEditorModel.SelectedRecordId == Guid.Empty);



            _sourceView2.iSourceEditorControl.RequestSetSelectedId(_sourceView1.iSourceEditorModel.SelectedRecordId);


            _sourceView2.iSourceEditorControl.RequestRefresh();


            //SourceBLL sourceBll = new SourceBLL();

            //Source source = sourceBll.ModelContainer.Sources.Where(s => s.SourceId == _sourceView1.iSourceEditorModel.SelectedRecordId).FirstOrDefault();

            //Assert.IsNotNull(source);

            //_sourceView1.iSourceEditorControl.RequestDelete();


            //source = sourceBll.ModelContainer.Sources.Where(s => s.SourceId == _sourceView1.iSourceEditorModel.SelectedRecordId).FirstOrDefault();

            //Assert.IsNull(source);
        }


        [Test(Description = "Records Present")]
        public void RecordsPresent()
        {

            Assert.IsTrue(_sourceView2.iSourceEditorModel.SelectedRecordId != Guid.Empty);
            Assert.IsTrue(_sourceView2.iSourceEditorModel.SourceRef != "");
        }

        [Test(Description = "CheckSavedData")]
        public void CheckSavedData()
        {
            Assert.IsTrue(_source.IsCopyHeld == _sourceView2.iSourceEditorModel.IsCopyHeld);
            Assert.IsTrue(_source.IsThackrayFound == _sourceView2.iSourceEditorModel.IsThackrayFound);
            Assert.IsTrue(_source.IsViewed == _sourceView2.iSourceEditorModel.IsViewed);
            Assert.IsTrue(_source.OriginalLocation == _sourceView2.iSourceEditorModel.SourceOriginalLocation);
            Assert.IsTrue(_source.SourceDateStr == _sourceView2.iSourceEditorModel.SourceDateStr);
            Assert.IsTrue(_source.SourceDateStrTo == _sourceView2.iSourceEditorModel.SourceDateToStr);
            Assert.IsTrue(_source.SourceDescription == _sourceView2.iSourceEditorModel.SourceDescription);
            Assert.IsTrue(_source.SourceFileCount.GetValueOrDefault().ToString() == _sourceView2.iSourceEditorModel.SourceFileCount);
            Assert.IsTrue(_source.SourceNotes == _sourceView2.iSourceEditorModel.SourceNotes);
            Assert.IsTrue(_source.SourceRef == _sourceView2.iSourceEditorModel.SourceRef);


        }

        [TearDown]
        public void TearDown()
        {

            _sourceView2.iSourceEditorControl.RequestDelete();

            _sourceView2.iSourceEditorControl.RequestRefresh();

            Assert.IsTrue(_sourceView2.iSourceEditorModel.SourceRef == "");



            //DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();

            //Person _person0 = deathsBirthsBLL.ModelContainer.Persons.FirstOrDefault(p => p.Person_id == _personView2.iDeathBirthEditorModel.SelectedRecordId);

            //if (_person0 != null)
            //{
            //    deathsBirthsBLL.ModelContainer.Persons.DeleteObject(_person0);
            //    deathsBirthsBLL.ModelContainer.SaveChanges();
            //}

        }
    }



}
