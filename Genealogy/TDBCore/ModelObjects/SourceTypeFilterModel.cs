using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using System.Collections.Specialized;
using System.Web;
using TDBCore.Types;
//using TDBCore.Datasets;
//using GedItter.Forms;

namespace GedItter.ModelObjects
{
    public class SourceTypeFilterModel : EditorBaseModel<int>, ISourceTypeFilterModel
    {
        string sourceTypeDesc = "";
        ISourceTypeEditorUI iSourceTypeEditorUI = null;
        IList<TDBCore.EntityModel.SourceType> sourceTypesDataTable = null;
        SourceTypeEditorModel sourceTypeEditorModel = new SourceTypeEditorModel();


        #region ISourceTypeFilterModel Members


        public ISourceTypeEditorUI ISourceTypeEditorUI
        {
            get
            {
                return this.iSourceTypeEditorUI;
            }
        }

        public string SourceTypesDescrip
        {
            get
            {
                return this.sourceTypeDesc;
            }
        }

        public void SetSourceTypesDescrip(string param)
        {
            if (param != sourceTypeDesc)
            {
                this.sourceTypeDesc = param;
                this.SetModelStatusFields();
            }
        }


        //public void AddObserver(ISourceTypeFilterView paramView)
        //{
        //    aList.Add(paramView);
        //}

        //public void RemoveObserver(ISourceTypeFilterView paramView)
        //{
        //    aList.Remove(paramView);
        //}

        //public void NotifyObservers()
        //{
        //    foreach (ISourceTypeFilterView view in aList)
        //    {
        //        view.Update(this);
        //    }
        //}



        #endregion

        public IList<TDBCore.EntityModel.SourceType> SourceTypesDataTable
        {
            get 
            {
                if (this.sourceTypesDataTable == null)
                    return new List<TDBCore.EntityModel.SourceType>();
                else
                    return this.sourceTypesDataTable;
            }
        }

        public override void Refresh()
        {
           // DsSourceTypes.SourceTypesDataTable sourceTypesDataTable = null;

            if (!IsvalidSelect()) return;

            BLL.SourceTypesBLL sourceTypesBll = new GedItter.BLL.SourceTypesBLL();

            if (this.IsValidSelectedRecordId)
            {
                sourceTypesDataTable = sourceTypesBll.GetSourceTypeById2(this.SelectedRecordId).ToList();
            }
            else
            {
                sourceTypesDataTable = sourceTypesBll.GetSourceTypeByFilter2(sourceTypeDesc, this.SelectedUserId).ToList();
            }

            this.NotifyObservers<SourceTypeFilterModel>(this);
        }

        public override void EditSelectedRecord()
        {
            if (!IsvalidSelect()) return;

            sourceTypeEditorModel = new SourceTypeEditorModel();
            //sourceTypeEditorModel.DataSaved += new EventHandler(sourceTypeEditorModel_DataSaved);
            this.SetDataSaved(new EventHandler(sourceTypeEditorModel_DataSaved));

            sourceTypeEditorModel.SetSelectedRecordId(this.SelectedRecordId);

            this.ShowDialogEdit(this);
        }

        void sourceTypeEditorModel_DataSaved(object sender, EventArgs e)
        {
            this.Refresh();
        }

        public override void InsertNewRecord()
        {
            if (!IsValidInsert()) return;


            sourceTypeEditorModel = new SourceTypeEditorModel();
            //sourceTypeEditorModel.DataSaved += new EventHandler(sourceTypeEditorModel_DataSaved);
            this.SetDataSaved(new EventHandler(sourceTypeEditorModel_DataSaved));

          //  sourceTypeEditorModel.SetSelectedRecordId(0);

            this.ShowDialogInsert(this);
        }

        public override void DeleteSelectedRecords()
        {

            if (!IsValidDelete()) return;

            BLL.SourceTypesBLL sourceTypesBLL = new BLL.SourceTypesBLL();

            foreach (int deathbirthRecordId in this.SelectedRecordIds)
            {
                sourceTypesBLL.DeleteSourceType(deathbirthRecordId);
            }

            this.SetSelectedRecordIds(new List<int>());

            Refresh();
        }

        public override bool IsValidEntry
        {
            get
            {
                return true;
            }
        }

        public override void SetModelStatusFields()
        {
            base.SetModelStatusFields();
        }


        public void SetEditorUI(ISourceTypeEditorUI paramISourceTypeEditorUI)
        {
            this.iSourceTypeEditorUI = paramISourceTypeEditorUI;
        }
        public void SetEditorUI()
        {
            //this.iSourceTypeEditorUI = new FrmEditSourceTypes();
        }


       


        public ISourceTypeEditorModel ISourceTypeEditorModel
        {
            get 
            {
                return this.sourceTypeEditorModel;
            }
        }

        public override void SetFromQueryString(string param)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(param);
            List<int> sourceTypes = (query["stype"] ?? "").Split(',').ToList<string>().ConvertAll<int>(delegate(string i) { int ret = 0; Int32.TryParse(i, out ret); return ret; }).ToList();

            sourceTypes.RemoveAll(i => i == 0);

            query.ReadInErrorsAndSecurity(this);

            //if (query.AllKeys.Contains("error"))
            //{
            //    this.SetErrorState(query["error"] ?? "");
            //}

            this.SetSelectedRecordIds(sourceTypes);

            this.SetSourceTypesDescrip(query["stypedesc"] ?? "");

       //     this.Refresh();
        }


    
    }
}
