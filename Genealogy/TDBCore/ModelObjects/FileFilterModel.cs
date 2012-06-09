using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
//using TDBCore.Datasets;
using System.Collections;
using GedItter.BLL;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Web;
using TDBCore.Types;

namespace GedItter.ModelObjects
{
    public class FileFilterModel : EditorBaseModel<Guid>, IFileFilterModel
    {
        FileEditorModel fileEditorModel = new FileEditorModel();
        protected new ArrayList aList = new ArrayList();
        //DsFileClass.FilesDataTable filesDataTable = null;
        IList<TDBCore.EntityModel.File> filesDataTable = null;
        bool isValidEditDateUpperBound = false;
        bool isValidEditDateLowerBound = false;
        bool isValidAddDateUpperBound = false;
        bool isValidAddDateLowerBound = false;
        IFileEditorUI iFileEditorUI = null;

        


        string filterStrDateEditUpper = "01 Jan 2020";
        string filterStrDateEditLower = "01 Jan 1754";
        string filterStrDateAddUpper = "01 Jan 2020";
        string filterStrDateAddLower = "01 Jan 1754";
        string filterFileRootPath = "";




        string filterStrFilePath = "";
        string filterStrFileDescrip = "";



        DateTime filterDTDateEditUpper = new DateTime(2020, 1, 1);//DateTime.Today;
        DateTime filterDTDateEditLower = new DateTime(1754, 1, 1);
        DateTime filterDTDateAddUpper = new DateTime(2020, 1, 1);
        DateTime filterDTDateAddLower = new DateTime(1754, 1, 1);


       
        #region validation props

        public IFileEditorUI IFileEditorUI
        {
            get
            {
                return this.iFileEditorUI;
            }
        }

        public IList<TDBCore.EntityModel.File> FilesList
        {
            get 
            {

                if (this.filesDataTable == null)
                    return new List<TDBCore.EntityModel.File>();
                else
                    return filesDataTable;
            }
        }
        public IFileEditorModel IFileEditorModel
        {
            get
            {
                return this.fileEditorModel;
            }
        }



        public string FilterFileRootPath
        {
            get
            {
                return this.filterFileRootPath;
            }

        }



        public bool IsValidEditDateUpperBound
        {
            get 
            {
                return this.isValidEditDateUpperBound;
            }
        }

        public bool IsValidEditDateLowerBound
        {
            get 
            {
                return this.isValidEditDateLowerBound;
            }
        }

        public bool IsValidAddDateUpperBound
        {
            get 
            {
                return this.isValidAddDateUpperBound;
            }
        }

        public bool IsValidAddDateLowerBound
        {
            get
            {
                return this.isValidAddDateLowerBound;
            }
        }

        #endregion

        #region fields
        public string FilterDateEditUpper
        {
            get
            { 
                return filterStrDateEditUpper;
            }
        
        }

        public string FilterDateEditLower
        {
           get
            {
                return filterStrDateEditLower;
            }
        }

        public string FilterDateAddUpper
        {
            get
            {
                return filterStrDateAddUpper;
            }
        }

        public string FilterDateAddLower
        {
            get
            {
                return filterStrDateAddLower;
            }
        }

        public string FilterFilePath
        {
            get
            {
                return filterStrFilePath;
            }
        }

        public string FilterFileDescrip
        {
            get
            {
                return filterStrFileDescrip;
            }
        }


        public void SetFilterDateEditUpper(string param)
        {

            this.isValidEditDateUpperBound = true;

            if (param != this.filterStrDateEditUpper)
            {
                if (DateTime.TryParse(param, out this.filterDTDateEditUpper))
                {
                    this.filterStrDateEditUpper = param;
                    this.isDataChanged = true;
                }
                else
                {
                    this.filterDTDateEditUpper = new DateTime(1000, 1, 1);
                    this.isValidEditDateUpperBound = false;
                }

            }
            else
            {
                this.isValidEditDateUpperBound = false;
            }
            
        }

        public void SetEditorUI(IFileEditorUI paramIFileEditorUI)
        {
            this.iFileEditorUI = paramIFileEditorUI;
        }

        public void SetFilterDateEditLower(string param)
        {
            this.isValidEditDateLowerBound = true;


            if (param != this.filterStrDateEditLower)
            {
                if (DateTime.TryParse(param, out this.filterDTDateEditLower))
                {
                    this.filterStrDateEditLower = param;
                    this.isDataChanged = true;
                }
                else
                {
                    this.filterDTDateEditLower = new DateTime(1000, 1, 1);
                    this.isValidEditDateLowerBound = false;
                }

            }
            else
            {
                this.isValidEditDateLowerBound = false;
            }
        }

        public void SetFilterDateAddUpper(string param)
        {
            this.isValidAddDateUpperBound = true;


            if (param != this.filterStrDateAddUpper)
            {
                if (DateTime.TryParse(param, out this.filterDTDateAddUpper))
                {
                    this.filterStrDateAddUpper = param;
                    this.isDataChanged = true;
                }
                else
                {
                    this.filterDTDateAddUpper = new DateTime(1000, 1, 1);
                    this.isValidAddDateUpperBound = false;
                }

            }
            else
            {
                this.isValidAddDateUpperBound = false;
            }
        }

        public void SetFilterDateAddLower(string param)
        {
            this.isValidAddDateLowerBound = true;


            if (param != this.filterStrDateAddLower)
            {
                if (DateTime.TryParse(param, out this.filterDTDateAddLower))
                {
                    this.filterStrDateAddLower = param;
                    this.isDataChanged = true;
                }
                else
                {
                    this.filterDTDateAddLower = new DateTime(1000, 1, 1);
                    this.isValidAddDateLowerBound = false;
                }

            }
            else
            {
                this.isValidAddDateLowerBound = false;
            }
        }

        public void SetFilterFilePath(string param)
        {
            if (param != this.filterStrFilePath)
            {
  
                this.filterStrFilePath = param;
                this.isDataChanged = true;
            }
        }

        

        public void SetFilterFileDescrip(string param)
        {
            if (param != this.filterStrFileDescrip)
            {
                this.filterStrFileDescrip = param;
                this.isDataChanged = true;
            }
        }

        #endregion

        public override void Refresh()
        {
            if (!IsvalidSelect()) return;

            if (isDataChanged)
            {
                FilesBLL filesBll = new GedItter.BLL.FilesBLL(filterFileRootPath);

                this.filesDataTable = filesBll.GetFiles2(this.filterStrFilePath, this.filterStrFileDescrip,
                    this.filterDTDateEditUpper, this.filterDTDateEditLower, this.filterDTDateAddUpper,
                    this.filterDTDateAddLower).ToList();
            }


            this.NotifyObservers<FileFilterModel>(this);
        }

        public override void DeleteSelectedRecords()
        {

            if (!IsValidDelete()) return;

            FilesBLL filesBll = new GedItter.BLL.FilesBLL(filterFileRootPath);

            foreach (Guid recIdx in this.SelectedRecordIds)
            {
                filesBll.DeleteFile2(recIdx);    
            }


            this.SetSelectedRecordIds(new List<Guid>());


            this.isDataChanged = true;
            Refresh();
            
        }

        public override void EditSelectedRecord()
        {

        

            if (!IsValidEdit()) return;

            fileEditorModel.SetSelectedRecordId(this.SelectedRecordId);

            this.isDataChanged = true;

            this.ShowDialogEdit(this);

        
        }

        public override void InsertNewRecord()
        {
            if (!IsValidInsert()) return;
           

            fileEditorModel.SetSelectedRecordId(this.SelectedRecordId);

            this.ShowDialogInsert(this);

        }

        public override void SetModelStatusFields()
        {
            base.SetModelStatusFields();
        }



        public void SetEditorUI()
        {
           
        }



        //public void ViewImage()
        //{
        //    if (!IsvalidSelect()) return;


        //    FilesBLL filesBll = new GedItter.BLL.FilesBLL(filterFileRootPath);
        //   // DsFileClass.FilesDataTable tpfilesDataTable = null;
        //  //  IList<TDBCore.EntityModel.File> 

        //    foreach (Guid recIdx in this.SelectedRecordIds)
        //    {


        //        var tpfilesDataTable = filesBll.GetFile2(recIdx);

        //        if (tpfilesDataTable != null)
        //        {
        //            CsUtils.ViewImage(tpfilesDataTable.FileLocation);
                
        //        }
        //    }

        //}



        public void SetFilterFileRootPath(string param)
        {
            if (param != this.filterFileRootPath)
            {
                this.filterFileRootPath = param;
            }
        }


        public override void SetFromQueryString(string param)
        {

            Debug.WriteLine("filter model SetFromQueryString :" + param);

            NameValueCollection query = HttpUtility.ParseQueryString(param);

            query.ReadInErrorsAndSecurity(this);

            //if (query.AllKeys.Contains("error"))
            //{
            //    this.SetErrorState(query["error"] ?? "");
            //}

            this.SetSelectedRecordIds(new List<Guid>());
            int pageNo = Convert.ToInt32(query["pg"] ?? "0");

            this.SetRecordStart(pageNo);


            this.SetFilterFileDescrip(query["filedesc"] ?? "");
            this.SetFilterFilePath(query["filename"] ?? "");

         //   this.Refresh();
        }


    }
}
