using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using NUnit.Framework;
using GedItter.BLL;
using TDBCore.EntityModel;
using GedItter.ModelObjects;
using GedItter.ControlObjects;

namespace UnitTests
{

    public class ParishView : IParishsEditorView
    {

        public IParishsEditorControl iParishEditorControl = null;
        public IParishsEditorModel iParishEditorModel = null;

        public bool IsValidParishCounty { get; set; }
        public bool IsValidRegistersDeposited { get; set; }
        public bool IsValidYearEnd { get; set; }
        public bool IsValidYearStart { get; set; }
        public bool IsValidParishName { get; set; }
        public bool IsValidParishLong { get; set; }
        public bool IsValidParishLat { get; set; }

        public void ShowInvalidParishCounty(bool valid)
        {
            IsValidParishCounty = valid;
        }

        public void ShowInvalidRegistersDeposited(bool valid)
        {
            IsValidRegistersDeposited = valid;
        }

        public void ShowInvalidEndYear(bool valid)
        {
            IsValidYearEnd = valid;
        }

        public void ShowInvalidParishName(bool valid)
        {
            IsValidParishName = valid;
        }

        public void ShowInvalidStartYear(bool valid)
        {
            IsValidYearStart = valid;
        }

        public void ShowInvalidParishLong(bool valid)
        {
            IsValidParishLong = valid;
        }

        public void ShowInvalidParishLat(bool valid)
        {
            IsValidParishLat = valid;
        }

        public ParishView()
        {
            iParishEditorModel = new ParishsEditorModel();
            iParishEditorControl = new ParishEditorControl();

            if (iParishEditorModel != null)
                iParishEditorControl.SetModel(iParishEditorModel);

            if (iParishEditorControl != null)
                iParishEditorControl.SetView(this);
        }



        public void Update<T>(T paramModel)
        {

        }



    }

    [TestFixture]
    public class NUnitParishTests : TestData
    {


        [Test(Description = "Valid County")]
        public void CheckParishEdValidParishCounty()
        {
            ParishView parishView = new ParishView();

            parishView.iParishEditorControl.RequestSetParishRegistersCounty("xxxx");
            Assert.IsTrue(parishView.iParishEditorModel.IsValidParishCounty);

            parishView.iParishEditorControl.RequestSetParishRegistersCounty(this.GetStringByLen(50));
            Assert.IsTrue(parishView.iParishEditorModel.IsValidParishCounty);
        }

        [Test(Description = "InValid County")]
        public void CheckParishEdInValidParishCounty()
        {
            ParishView parishView = new ParishView();

            parishView.iParishEditorControl.RequestSetParishRegistersCounty("");
            Assert.IsFalse(parishView.iParishEditorModel.IsValidParishCounty);

            parishView.iParishEditorControl.RequestSetParishRegistersCounty(this.GetStringByLen(51));
            Assert.IsFalse(parishView.iParishEditorModel.IsValidParishCounty);


        }


        [Test(Description = "Valid Reg Deposited")]
        public void CheckParishEdValidRegDeposited()
        {
            ParishView parishView = new ParishView();

            parishView.iParishEditorControl.RequestSetParishRegistersDeposited("xxx");
            Assert.IsTrue(parishView.iParishEditorModel.IsValidRegistersDeposited);

            parishView.iParishEditorControl.RequestSetParishRegistersDeposited(this.GetStringByLen(100));
            Assert.IsTrue(parishView.iParishEditorModel.IsValidRegistersDeposited);
        }

        [Test(Description = "Invalid Reg Deposited")]
        public void CheckParishEdInValidRegDeposited()
        {
            ParishView parishView = new ParishView();

            parishView.iParishEditorControl.RequestSetParishRegistersDeposited("");
            Assert.IsFalse(parishView.iParishEditorModel.IsValidRegistersDeposited);

            parishView.iParishEditorControl.RequestSetParishRegistersDeposited(this.GetStringByLen(101));
            Assert.IsFalse(parishView.iParishEditorModel.IsValidRegistersDeposited);


        }

        [Test(Description = "Valid Name")]
        public void CheckParishEdValidName()
        {
            ParishView parishView = new ParishView();

            parishView.iParishEditorControl.RequestSetParishName("xxx");
            Assert.IsTrue(parishView.iParishEditorModel.IsValidParishName);

            parishView.iParishEditorControl.RequestSetParishName(this.GetStringByLen(500));
            Assert.IsTrue(parishView.iParishEditorModel.IsValidParishName);
        }

        [Test(Description = "Invalid Name")]
        public void CheckParishEdInValidName()
        {
            ParishView parishView = new ParishView();

            parishView.iParishEditorControl.RequestSetParishName("");
            Assert.IsFalse(parishView.iParishEditorModel.IsValidParishName);

            parishView.iParishEditorControl.RequestSetParishName(this.GetStringByLen(501));
            Assert.IsFalse(parishView.iParishEditorModel.IsValidParishName);


        }

        [Test(Description = "Valid Start Year")]
        public void CheckParishEdValidStartYear()
        {
            ParishView parishView = new ParishView();

            parishView.iParishEditorControl.RequestSetParishStartYear("1800");
            Assert.IsTrue(parishView.iParishEditorModel.IsValidStartDate);

            parishView.iParishEditorControl.RequestSetParishStartYear("1700");
            Assert.IsTrue(parishView.iParishEditorModel.IsValidStartDate);


        }

        [Test(Description = "Invalid Start Year")]
        public void CheckParishEdInvalidStartYear()
        {
            ParishView parishView = new ParishView();

            parishView.iParishEditorControl.RequestSetParishStartYear("");
            Assert.IsFalse(parishView.iParishEditorModel.IsValidStartDate);

            parishView.iParishEditorControl.RequestSetParishStartYear("0");
            Assert.IsFalse(parishView.iParishEditorModel.IsValidStartDate);

            parishView.iParishEditorControl.RequestSetParishStartYear("xxx");
            Assert.IsFalse(parishView.iParishEditorModel.IsValidStartDate);

            parishView.iParishEditorControl.RequestSetParishStartYear("200000");
            Assert.IsFalse(parishView.iParishEditorModel.IsValidStartDate);
        }


        [Test(Description = "Valid End Year")]
        public void CheckParishEdValidEndYear()
        {
            ParishView parishView = new ParishView();

            parishView.iParishEditorControl.RequestSetParishEndYear("1800");
            Assert.IsTrue(parishView.iParishEditorModel.IsValidEndDate);

            parishView.iParishEditorControl.RequestSetParishEndYear("1700");
            Assert.IsTrue(parishView.iParishEditorModel.IsValidEndDate);


        }

        [Test(Description = "Invalid End Year")]
        public void CheckParishEdInvalidEndYear()
        {
            ParishView parishView = new ParishView();

            parishView.iParishEditorControl.RequestSetParishEndYear("");
            Assert.IsFalse(parishView.iParishEditorModel.IsValidEndDate);

            parishView.iParishEditorControl.RequestSetParishEndYear("0");
            Assert.IsFalse(parishView.iParishEditorModel.IsValidEndDate);

            parishView.iParishEditorControl.RequestSetParishEndYear("xxx");
            Assert.IsFalse(parishView.iParishEditorModel.IsValidEndDate);

            parishView.iParishEditorControl.RequestSetParishEndYear("200000");
            Assert.IsFalse(parishView.iParishEditorModel.IsValidEndDate);
        }


        [Test(Description = "Add Parish.")]
        public void CheckSourceEdValidAddParish()
        {
            ParishView parishView = new ParishView();
            parishView.iParishEditorModel.SetIsSecurityEnabled(false);

            parishView.iParishEditorControl.RequestSetParishName("test parish");
            parishView.iParishEditorControl.RequestSetParishParent("test parish parent");
            parishView.iParishEditorControl.RequestSetParishRegisterNotes("test parish notes");
            parishView.iParishEditorControl.RequestSetParishRegistersCounty("yorkshire");
            parishView.iParishEditorControl.RequestSetParishRegistersDeposited("bihr");
            parishView.iParishEditorControl.RequestSetParishStartYear("1800");
            parishView.iParishEditorControl.RequestSetParishEndYear("1900");
            parishView.iParishEditorControl.RequestSetParishLat("51.00");
            parishView.iParishEditorControl.RequestSetParishLong("52.00");

            Assert.IsTrue(parishView.iParishEditorModel.IsValidEntry);


            parishView.iParishEditorControl.RequestInsert();

            Assert.IsFalse(parishView.iParishEditorModel.SelectedRecordId == Guid.Empty);


            ParishsBLL parishsBll = new ParishsBLL();

            Parish _parish = parishsBll.ModelContainer.Parishs.Where(p => p.ParishId == parishView.iParishEditorModel.SelectedRecordId).FirstOrDefault();



            Assert.IsNotNull(_parish);

            parishView.iParishEditorControl.RequestDelete();


            _parish = parishsBll.ModelContainer.Parishs.Where(p => p.ParishId == parishView.iParishEditorModel.SelectedRecordId).FirstOrDefault();

            Assert.IsNull(_parish);
        }

    }

 

}
