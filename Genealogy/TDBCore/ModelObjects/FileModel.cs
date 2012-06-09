using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using System.IO;
using System.Collections;
using GedItter.BLL;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Web;
using TDBCore.Types;

//using TDBCore.Datasets;

namespace GedItter.ModelObjects
{
    public class FileEditorModel : EditorBaseModel<Guid>, IFileEditorModel
    {
      
        bool isValidFilePath = false;
        bool isValidFileDescription = false;
        bool isValidFileThumbLocation = false;
        bool isValidFileSystemRoot = false;

        string filePath = "";
        string fileDescrip = "";
        string fileThumbLocat = "";
        string fileSystemRoot = "";


        #region validation

        public bool IsValidFilePath
        {
            get
            {
                return this.isValidFilePath;
            }
        }

        public override bool IsValidEntry
        {
            get
            {
                return isValidFilePath && isValidFileDescription;
            }
        }

        public bool IsValidFileDescription
        {
            get 
            {
                return this.isValidFileDescription;
            }
        }

        public bool IsValidFileThumbLocation
        {
            get
            {
                return this.isValidFileThumbLocation;
            }
        }

        public bool IsValidFileSystemRoot
        {
            get 
            {
                return this.isValidFileSystemRoot;
            }
        }
        #endregion
        
        #region read only properties

        public string FilePath
        {
            get 
            {
                return this.filePath;
            
            }
        }

        public string FileDescription
        {
            get
            {
                return this.fileDescrip;
            }
        }

        public string FileThumbLocat
        {
            get
            {
                return this.fileThumbLocat;
            }
        }
    
        public string FileSystemRoot
        {
            get
            {
                return this.fileSystemRoot;
            }
        }

        #endregion



        public void SetFilePath(string filePath)
        {

            if (this.filePath != filePath)
            {
                isValidFilePath = true;


                if (filePath.Length > 0 && filePath.Length <= 50 && File.Exists(filePath))
                {
                    this.filePath = filePath;


                }
                else
                {
                    this.filePath = filePath;

                    isValidFilePath = false;
                }

            }
        }

        public void SetFileDescription(string fileDescrip)
        {
            if (this.fileDescrip != fileDescrip)
            {
                if (fileDescrip.Length >= 0 && fileDescrip.Length <= 500)
                {
                    this.isValidFileDescription = true;
                }
                else
                {
                    this.isValidFileDescription = false;
                }

                this.fileDescrip = fileDescrip;
                this.SetModelStatusFields();
            }
        }

        public void SetFileThumbLocat(string fileThumbLocat)
        {
            if (this.fileThumbLocat != fileThumbLocat)
            {
                if (fileThumbLocat.Length >= 0 && fileThumbLocat.Length <= 500 && File.Exists(fileThumbLocat))
                {
                    this.isValidFileThumbLocation = true;
                }
                else
                {
                    this.isValidFileThumbLocation = false;
                }


                this.fileThumbLocat = fileThumbLocat;
                this.SetModelStatusFields();

            }
        }

        public void SetFileSystemRoot(string param)
        {
            if (this.fileSystemRoot != param)
            {

                if (fileSystemRoot.Length >= 0 && fileSystemRoot.Length <= 500 && Directory.Exists(param))
                {
                    this.isValidFileSystemRoot = true;
                }
                else
                {
                    this.isValidFileSystemRoot = false;
                }


                this.fileSystemRoot = param;
                this.SetModelStatusFields();
            }
        }
 


        public override void DeleteSelectedRecords()
        {

            if (!IsValidDelete()) return;

            if (this.IsValidEntry)
            {
               // if (DialogResult.OK == MessageBox.Show("WARNING", "Ok to Delete?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
              //  {

                FilesBLL filesBll = new GedItter.BLL.FilesBLL(fileSystemRoot);

                    foreach (Guid recIdx in this.SelectedRecordIds)
                    {
                        filesBll.DeleteFile2(recIdx);
                    }

                    Refresh();
              //  }
            }
            else
            {
              //  MessageBox.Show("WARNING", "Invalid  ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public override void EditSelectedRecord()
        {
            if (!IsValidEdit()) return;

            if (this.IsValidEntry)
            {
                FilesBLL filesBll = new GedItter.BLL.FilesBLL(this.fileSystemRoot);

                filesBll.UpdateFile2(this.SelectedRecordId, this.fileDescrip, this.filePath, this.SelectedUserId,fileThumbLocat);
                
                base.EditSelectedRecord();
                
                Refresh();
                 
            }
            else
            {
               // MessageBox.Show("WARNING", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public override void InsertNewRecord()
        {
            if (!IsValidInsert()) return;

            if (this.IsValidEntry)
            {
                FilesBLL filesBll = new GedItter.BLL.FilesBLL();

                this.SetSelectedRecordId(filesBll.AddFile2(this.fileDescrip, this.filePath, this.SelectedUserId,fileThumbLocat));

                Refresh();

            }
            else
            {
                //MessageBox.Show("WARNING", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public override void Refresh()
        {
            if (!IsvalidSelect()) return;

            TDBCore.EntityModel.File filesDataTable = null;
            FilesBLL filesBll = new GedItter.BLL.FilesBLL(this.fileSystemRoot);

            filesDataTable = filesBll.GetFile2(this.SelectedRecordId);

            if (filesDataTable != null )
            {
                //this.SetSelectedUserId(filesDataTable.FilerUserAdded.Value);
                this.SetFileDescription(filesDataTable.FileDescription);

                this.SetFilePath(filesDataTable.FileLocation);
            }


            this.NotifyObservers<FileEditorModel>(this);
        }


        public override void SetFromQueryString(string param)
        {
            Debug.WriteLine("editor model SetFromQueryString :" + param);

            NameValueCollection query = HttpUtility.ParseQueryString(param);
            Guid selectedId = Guid.Empty;

            Guid.TryParse(query["id"], out selectedId);

            query.ReadInErrorsAndSecurity(this);

            //if (query.AllKeys.Contains("error"))
            //{
            //    this.SetErrorState(query["error"] ?? "");
            //}

            this.SetSelectedRecordId(selectedId);

        //    this.Refresh();

        }
       

        
    }
}
