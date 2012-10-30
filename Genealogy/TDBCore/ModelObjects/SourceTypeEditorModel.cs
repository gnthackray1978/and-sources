using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Web;
using TDBCore.Types;
//using TDBCore.Datasets;

namespace GedItter.ModelObjects
{
    public class SourceTypeEditorModel : EditorBaseModel<int>, ISourceTypeEditorModel
    {
        bool isValidSourceTypeDesc = false;
        bool isValidSourceTypeOrder = false;

        int intSourceTypeOrder = 0;
        string sourceTypeOrder = "";
        string sourceTypeDesc = "";


        #region validation


        public bool IsValidSourceTypeDesc
        {
            get
            {
                return isValidSourceTypeDesc;
            }
        }

        public bool IsValidSourceOrder
        {
            get 
            {
                return isValidSourceTypeOrder;
            }
        }

        #endregion

       

        #region read only properties

        public string SourceTypeDesc
        {
            get
            {
                return this.sourceTypeDesc;
            }
        }

        public int SourceTypeOrder
        {
            get 
            {
                return intSourceTypeOrder;
            }
        }

        #endregion

        public void SetSourceTypeDesc(string sourceTypeDesc)
        {

            if (this.sourceTypeDesc != sourceTypeDesc)
            {
                if (sourceTypeDesc.Length >= 5 && sourceTypeDesc.Length <= 50)
                {
                    this.isValidSourceTypeDesc = true;
                }
                else
                {
                    this.isValidSourceTypeDesc = false;
                    this.SetErrorState("Source Type Description either too short or too long");
                }

                this.sourceTypeDesc = sourceTypeDesc;
                this.SetModelStatusFields();
            }
        }

        public void SetSourceTypeOrder(string param)
        {

            if (intSourceTypeOrder.ToString() != param)
            {
                if (Int32.TryParse(param, out intSourceTypeOrder))
                {
                    this.isValidSourceTypeOrder = true; 
                }
                else
                {
                    this.isValidSourceTypeOrder = false;
                    this.SetErrorState("Source Type order is invalid number");
                }

                this.sourceTypeOrder = param;
                this.SetModelStatusFields();
            }
        }
  
       


        public override void InsertNewRecord()
        {

            if (!IsValidInsert()) return;

            if (this.IsValidEntry)
            {

                BLL.SourceTypesBLL sourceTypesBll = new GedItter.BLL.SourceTypesBLL();

                this.SetSelectedRecordId(sourceTypesBll.InsertSourceType(this.sourceTypeDesc,  this.SelectedUserId, this.intSourceTypeOrder));

                this.OnDataSaved();

            }
            

            this.NotifyObservers<SourceTypeEditorModel>(this);
        }

        public override void EditSelectedRecord()
        {

            if (!IsValidEdit()) return;

            if (this.IsValidEntry)
            {
                BLL.SourceTypesBLL sourceTypesBll = new GedItter.BLL.SourceTypesBLL();

                sourceTypesBll.UpdateSourceType(this.sourceTypeDesc, this.SelectedUserId, this.SelectedRecordId, this.intSourceTypeOrder);

              //  base.EditSelectedRecord();

                Refresh();

            }
             
        }

        public override void DeleteSelectedRecords()
        {

            if (!IsValidDelete()) return;

            if (this.IsValidEntry)
            {
               
                    BLL.SourceTypesBLL sourceTypesBll = new GedItter.BLL.SourceTypesBLL();

                    foreach (int recIdx in this.SelectedRecordIds)
                    {
                        sourceTypesBll.DeleteSourceType(recIdx);
                    }

                    Refresh();
            
            }
          
        }

        public override void Refresh()
        {

            if (!IsvalidSelect()) return;

         //   IList<TDBCore.EntityModel.SourceType> sourceTypesDataTable = null;

            BLL.SourceTypesBLL sourceTypesBll = new GedItter.BLL.SourceTypesBLL();

            var sourceTypesDataTable = sourceTypesBll.GetSourceTypeById2(this.SelectedRecordId).FirstOrDefault();

            if (sourceTypesDataTable != null )
            {
                this.SetSelectedUserId(sourceTypesDataTable.SourceUserAdded);
                this.SetSourceTypeDesc(sourceTypesDataTable.SourceTypeDesc);
                this.SetSourceTypeOrder(sourceTypesDataTable.SourceTypeOrder.ToString());
            }


            this.NotifyObservers<SourceTypeEditorModel>(this);
             
        }

        //public override void SetModelStatusFields()
        //{
        //    base.SetModelStatusFields();
        //}




        public override void SetFromQueryString(string param)
        {
            Debug.WriteLine("editor model SetFromQueryString :" + param);

            NameValueCollection query = HttpUtility.ParseQueryString(param);
            int selectedId = 0;

            Int32.TryParse(query["id"], out selectedId);

            //if (query.AllKeys.Contains("error"))
            //{
            //    this.SetErrorState(query["error"] ?? "");
            //}

            query.ReadInErrorsAndSecurity(this);


            this.SetSelectedRecordId(selectedId);

         //   this.Refresh();

        }


    }
}
