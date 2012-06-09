using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using GedItter.ModelObjects;
using GedItter.ControlObjects;
using NUnit.Framework;
using GedItter.BLL;
using TDBCore.EntityModel;

namespace UnitTests
{
   
    public class SourceTypeView : ISourceTypeEditorView
    {



        public bool IsValidSourceTypeDesc { get; set; }
        public bool IsValidSourceTypeOrder { get; set; }


        public ISourceTypeEditorModel iSourceTypeEditorModel = null;
        public ISourceTypeEditorControl iSourceTypeEditorControl = null;


        public void ShowValidSourceTypeDesc(bool valid)
        {
            this.IsValidSourceTypeDesc = valid;
        }

        public void ShowValidSourceTypeOrder(bool valid)
        {
            this.IsValidSourceTypeOrder = valid;
        }


        public SourceTypeView()
        {
            iSourceTypeEditorModel = new SourceTypeEditorModel();
            iSourceTypeEditorControl = new SourceTypeEditorControl();

            if (iSourceTypeEditorModel != null)
                iSourceTypeEditorControl.SetModel(iSourceTypeEditorModel);

            if (iSourceTypeEditorControl != null)
                iSourceTypeEditorControl.SetView(this);
        }


        public void Update<T>(T paramModel)
        {

        }



    }


    [TestFixture]
    public class NUnitSourceTypes : TestData
    {


        [Test(Description = "Valid Source Type Desc")]
        public void CheckValidSourceTypeDesc()
        {
            SourceTypeView sourceview = new SourceTypeView();

            sourceview.iSourceTypeEditorControl.RequestSetSourceTypeDesc("testx");

            Assert.IsTrue(sourceview.iSourceTypeEditorModel.IsValidSourceTypeDesc);

            sourceview.iSourceTypeEditorControl.RequestSetSourceTypeDesc(this.GetStringByLen(50));
            Assert.IsTrue(sourceview.iSourceTypeEditorModel.IsValidSourceTypeDesc);
        }

        [Test(Description = "Invalid Source Type Desc")]
        public void CheckInvalidSourceTypeDesc()
        {
            SourceTypeView sourceview = new SourceTypeView();

            sourceview.iSourceTypeEditorControl.RequestSetSourceTypeDesc("");
            Assert.IsFalse(sourceview.iSourceTypeEditorModel.IsValidSourceTypeDesc);

            sourceview.iSourceTypeEditorControl.RequestSetSourceTypeDesc(this.GetStringByLen(51));
            Assert.IsFalse(sourceview.iSourceTypeEditorModel.IsValidSourceTypeDesc);


        }



        [Test(Description = "Valid Source Type Order")]
        public void CheckValidSourceTypeOrder()
        {
            SourceTypeView sourceview = new SourceTypeView();

            sourceview.iSourceTypeEditorControl.RequestSetSourceTypeOrder("3");

            Assert.IsTrue(sourceview.iSourceTypeEditorModel.IsValidSourceOrder);


        }

        [Test(Description = "Invalid Source Type Order")]
        public void CheckInvalidSourceTypeOrder()
        {
            SourceTypeView sourceview = new SourceTypeView();

            sourceview.iSourceTypeEditorControl.RequestSetSourceTypeOrder("x");
            Assert.IsFalse(sourceview.iSourceTypeEditorModel.IsValidSourceOrder);

            sourceview.iSourceTypeEditorControl.RequestSetSourceTypeOrder("");
            Assert.IsFalse(sourceview.iSourceTypeEditorModel.IsValidSourceOrder);


        }




        [Test(Description = "Add Source Type")]
        public void CheckAddSourceType()
        {
            SourceTypeView sourceview = new SourceTypeView();
            sourceview.iSourceTypeEditorModel.SetIsSecurityEnabled(false);

            sourceview.iSourceTypeEditorControl.RequestSetSelectedId(0);
            sourceview.iSourceTypeEditorControl.RequestSetSourceTypeDesc("test desc");
            sourceview.iSourceTypeEditorControl.RequestSetSourceTypeOrder("1");

            Assert.IsTrue(sourceview.iSourceTypeEditorModel.IsValidEntry);


            sourceview.iSourceTypeEditorControl.RequestInsert();



            Assert.IsFalse(sourceview.iSourceTypeEditorModel.SelectedRecordId == 0);

            SourceTypesBLL sourceTypesBll = new SourceTypesBLL();


            SourceType _sourceType = sourceTypesBll.ModelContainer.SourceTypes.Where(st => st.SourceTypeId == sourceview.iSourceTypeEditorModel.SelectedRecordId).FirstOrDefault();
            //Parish _parish = parishsBll.ModelContainer.Parishs.Where(p => p.ParishId == parishView.iParishEditorModel.SelectedRecordId).FirstOrDefault();



            Assert.IsNotNull(_sourceType);

            sourceview.iSourceTypeEditorControl.RequestDelete();


            _sourceType = sourceTypesBll.ModelContainer.SourceTypes.Where(st => st.SourceTypeId == sourceview.iSourceTypeEditorModel.SelectedRecordId).FirstOrDefault();

            Assert.IsNull(_sourceType);
        }

    }

 


}
