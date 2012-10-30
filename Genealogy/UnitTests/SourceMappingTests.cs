using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using NUnit.Framework;
using GedItter.ModelObjects;
using GedItter.ControlObjects;
using GedItter.BLL;
using TDBCore.EntityModel;
using GedItter.BirthDeathRecords.BLL;
using GedItter.MarriageRecords.BLL;

namespace UnitTests
{
    #region source mapping view
    public class SourceMappingView : ISourceMappingView
    {
        public ISourceMappingControl iSourceMappingControl = null;
        public ISourceMappingModel iSourceMappingModel = null;

        public SourceMappingView()
        {
            iSourceMappingModel = new SourceMappingModel();
            iSourceMappingControl = new SourceMappingControl();

            if (iSourceMappingModel != null)
                iSourceMappingControl.SetModel(iSourceMappingModel);

            if (iSourceMappingControl != null)
                iSourceMappingControl.SetView(this);
        }



        public void AddSources(List<Guid> paramGuids)
        {
             
        }

        //public List<Guid> SelectedSourceIds
        //{
            
        //}
         

       

        public void Update<T>(T paramModel)
        {
            
        }

        public List<Guid> SelectedSourceIds
        {
            get
            {
                return new List<Guid>();
            }
        }

       
    }

    #endregion

    [TestFixture]
    public class NUnitSourceMappingTests : TestData
    {
        SourceBLL sourceBLL = new SourceBLL();
        DeathsBirthsBLL personsBLL = new DeathsBirthsBLL();
        MarriagesBLL marriagesBll = new MarriagesBLL();
        SourceMappingView sourceMappingView1 = null;
        Source _source1 = null;
        Source _source2 = null;
        Person _person1 = null;
        Person _person2 = null;
        Marriage _marriage1 = null;
        Marriage _marriage2 = null;

        // we need a source
        // we need some persons
        // we need some marriages



        [SetUp]
        public void SetupTest()
        {
            sourceMappingView1 = new SourceMappingView();
            sourceMappingView1.iSourceMappingModel.SetIsSecurityEnabled(false);

            _source1 = sourceBLL.GetNewSource("sdesc test", "sorigloc", false, false, false, 1, "1 Jan 1800", "2 Jan 1801", 1800, 1801, "sreftest1", 0, "notes");
            _source2 = sourceBLL.GetNewSource("sdesc test", "sorigloc", false, false, false, 1, "1 Jan 1800", "2 Jan 1801", 1800, 1801, "sreftest2", 0, "notes");

            _person1 = personsBLL.CreateBasicPerson("testcname1", "testsname1", "testlocat", 1800);
            _person2 = personsBLL.CreateBasicPerson("testcname2", "testsname2", "testlocat2", 1800);

            _marriage1 = marriagesBll.CreateBasicMarriage("testgcname1", "testgsname1", "testbcname1", "testbsname1", "testlocat1", 1800);
            _marriage2 = marriagesBll.CreateBasicMarriage("testgcname2", "testgsname2", "testbcname2", "testbsname2", "testlocat2", 1800);
        }


        [Test(Description = "CheckSavedData")]
        public void CheckSavedData()
        {
            Assert.IsNotNull(_source1);
            Assert.IsNotNull(_source2);
            Assert.IsNotNull(_person1);
            Assert.IsNotNull(_person2);
            Assert.IsNotNull(_marriage1);
            Assert.IsNotNull(_marriage2);

            //sourceMappingView1.iSourceMappingControl.
        }

        [Test(Description = "CheckAddSource")]
        public void CheckAddSource()
        {
            //sourceMappingView1.iSourceMappingControl.RequestSetParentRecordIds  

            sourceMappingView1.iSourceMappingControl.RequestSetSelectedSourceSourceId(_source1.SourceId);
            sourceMappingView1.iSourceMappingControl.RequestAddMapping();
            sourceMappingView1.iSourceMappingControl.RequestSetSelectedSourceSourceId(_source2.SourceId);
            sourceMappingView1.iSourceMappingControl.RequestAddMapping();

            Assert.IsTrue(sourceMappingView1.iSourceMappingModel.SourcesMappedDataTable.Count == 2);


        }

        [Test(Description = "CheckRemoveSource")]
        public void CheckRemoveSource()
        {
            sourceMappingView1.iSourceMappingControl.RequestSetSelectedSourceSourceId(_source1.SourceId);
            sourceMappingView1.iSourceMappingControl.RequestAddMapping();
            sourceMappingView1.iSourceMappingControl.RequestSetSelectedSourceSourceId(_source2.SourceId);
            sourceMappingView1.iSourceMappingControl.RequestAddMapping();

            Assert.IsTrue(sourceMappingView1.iSourceMappingModel.SourcesMappedDataTable.Count == 2);

            sourceMappingView1.iSourceMappingControl.RequestSetSelectedSourceSourceId(_source1.SourceId);
            sourceMappingView1.iSourceMappingControl.RequestRemoveMapping();
            sourceMappingView1.iSourceMappingControl.RequestSetSelectedSourceSourceId(_source2.SourceId);
            sourceMappingView1.iSourceMappingControl.RequestRemoveMapping();
        }


        [TearDown]
        public void TearDown()
        {
            sourceBLL.ModelContainer.Persons.DeleteObject(_person1);
            sourceBLL.ModelContainer.Persons.DeleteObject(_person2);

            sourceBLL.ModelContainer.Marriages.DeleteObject(_marriage1);
            sourceBLL.ModelContainer.Marriages.DeleteObject(_marriage2);

            sourceBLL.ModelContainer.Sources.DeleteObject(_source1);
            sourceBLL.ModelContainer.Sources.DeleteObject(_source2);
        }
    }
}
