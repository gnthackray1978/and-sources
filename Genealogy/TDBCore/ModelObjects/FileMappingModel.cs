using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
////using TDBCore.Datasets;
using GedItter.BLL;
using System.IO;
using TDBCore.ModelObjects;
using TDBCore.Types;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace GedItter.ModelObjects
{
    public class FileMappingModel : EditorBaseModel<Guid>, IFileMapSourceModel
    {
        Guid parentId;
        Guid selectedFileId;
        Guid selectedMappedFileId;
        string fileDescription = "";
        string fileName = "";
        string fileThumbName = "";
        string fileSystemRoot = "";

        IList<TDBCore.EntityModel.File> filesDataTable = null;

        IList<TDBCore.EntityModel.File> filesMappedDataTable = null;

        IFileEditorUI iFileEditorUI = null;

        public string FileSystemRoot
        {
            get
            {
                return this.fileSystemRoot;
            }
        }

        public void SetFileSystemRoot(string param)
        {
            if (this.fileSystemRoot != param)
            {
                this.fileSystemRoot = param;
                this.SetModelStatusFields();
            }
        }


        #region props

        public IFileEditorUI IFileEditorUI
        {
            get
            {
                return this.iFileEditorUI;
            }
        }

        public Guid ParentId
        {
            get
            {
                return parentId;
            }
        }

        public Guid SelectedFileId
        {
            get 
            {
                return selectedFileId;
            }
        }

        public Guid SelectedMappedFileId
        {
            get 
            {
                return this.selectedMappedFileId; 
            }
        }

        public string FileDescription
        {
            get
            {
                return this.fileDescription;
            }
        }
        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }
        public string FileThumbLocat
        {
            get
            {
                return this.fileThumbName;
            }
        }

        public IList<TDBCore.EntityModel.File> FilesList
        {
            get
            {
                return this.filesDataTable;
            }
        }

        public IList<TDBCore.EntityModel.File> FilesMappedList
        {
            get 
            {
                return this.filesMappedDataTable;
            }
        }
        #endregion

        #region set fields 
        public void SetSelectedFileId(Guid mapFileId)
        {
            if (mapFileId != selectedFileId)
            {
                this.selectedFileId = mapFileId;
                this.SetModelStatusFields();
            }
        }

        public void SetSelectedMappedFileId(Guid mapFileId)
        {
            if (mapFileId != selectedMappedFileId)
            {
                this.selectedMappedFileId = mapFileId;
                this.SetModelStatusFields();
            }
        }

        public void SetParentId(Guid paramGuid)
        {
            if (this.parentId != paramGuid)
            {
                AddOutstandingMappings(this.parentId, paramGuid);
                this.parentId = paramGuid;
                this.SetModelStatusFields();
                this.Refresh();
            }
        }

        public void SetFileDescrip(string param)
        {
            if (this.fileDescription != param)
            {
                this.fileDescription = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFileName(string param)
        {
            if (this.fileName != param)
            {
                this.fileName = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFileThumbLocat(string param)
        {
            if (this.fileName != param)
            {
                this.fileThumbName = param;
                this.SetModelStatusFields();
            }
        
        }
        #endregion


//
        public void RemoveMapping()
        {
            //MessageBox.Show("request remove with: " + this.selectedMappedSourceTypeId.ToString());

             if (!IsValidDelete()) return;

             if (this.parentId != Guid.Empty)
             {
                BLL.SourceMappingsBLL sourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();
                sourceMappingsBLL.DeleteByFileIdAndSourceId(this.parentId, this.selectedMappedFileId);
             }
             else
             {
                 RemoveDisconnectedSourceMapRow(this.selectedMappedFileId);
             }


            this.Refresh();
        }
        //
        public void AddMapping()
        {

            if (!IsValidInsert()) return;

            if (this.parentId != Guid.Empty)
            {
                BLL.SourceMappingsBLL sourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();
                sourceMappingsBLL.Insert(parentId, selectedFileId, null, this.SelectedUserId, null, DateTime.Today.ToShortDateString(), null);
            }
            else
            {
                AddDisconnectedSourceMapRow(selectedFileId);
            }


            this.Refresh();
        }


       
        public void AddDisconnectedSourceMapRow(Guid paramGuid)
        {


            //BLL.SourceMappingsBLL sourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();
            //BLL.SourceTypesBLL sourceTypeBLL = new GedItter.BLL.SourceTypesBLL();
            BLL.FilesBLL filesBLL = new FilesBLL(fileSystemRoot);

            TDBCore.EntityModel.File tpTable = filesBLL.GetFile2(paramGuid);

            if (filesMappedDataTable == null) filesMappedDataTable = new List<TDBCore.EntityModel.File>();

            TDBCore.EntityModel.File sRow = new TDBCore.EntityModel.File();

            if (tpTable != null)
            {
                sRow.FileDate = tpTable.FileDate;
                sRow.FileDescription = tpTable.FileDescription;
                sRow.FileEntryAdded = tpTable.FileEntryAdded;
                sRow.FileLocation = tpTable.FileLocation;
                sRow.FilerUserAdded = tpTable.FilerUserAdded;
                sRow.FiletId = tpTable.FiletId;
                sRow.FileThumbLocation = tpTable.FileThumbLocation;
            }

            bool isFound = false;

            foreach (var _fRow in filesMappedDataTable)
            {
                if (_fRow.FiletId == paramGuid) isFound = true;
            }

            if(!isFound)
                this.filesMappedDataTable.Add(sRow);

          
        }
        
        public void RemoveDisconnectedSourceMapRow(Guid paramGuid)
        {
            if (filesMappedDataTable != null)
            {

                filesMappedDataTable.Remove(st => st.FiletId == paramGuid);


                //int idx = 0;

                

                //while (idx < filesMappedDataTable.Count)
                //{
                //    if (filesMappedDataTable[idx].FiletId == paramGuid)
                //    {
                //        filesMappedDataTable.RemoveAt(idx);
                //    }
                //    idx++;
                //}

              //  this.filesMappedDataTable.AcceptChanges();
            }
        }
        //
        public void AddOutstandingMappings(Guid oldParentId, Guid newParentId)
        {

            if (!IsValidInsert()) return;

            if (oldParentId == Guid.Empty &&
                newParentId != Guid.Empty)
            {
                if (filesMappedDataTable != null)
                {
                    foreach (var sRow in filesMappedDataTable)
                    {
                        BLL.SourceMappingsBLL sourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();

                        sourceMappingsBLL.Insert(newParentId, sRow.FiletId, null, this.SelectedUserId, null, DateTime.Today.ToShortDateString(), null);

                    }
                }
            }
        }

     
        public void AddNewFileWithMapping(string file, string thumbNail)
        {
            if (!IsValidInsert()) return;

            BLL.FilesBLL filesBll = new GedItter.BLL.FilesBLL(this.fileSystemRoot);
            BLL.SourceMappingsBLL sourcesMappingBLL = new GedItter.BLL.SourceMappingsBLL();

            Guid fileId = Guid.Empty;

            TDBCore.EntityModel.File filesDataTable = filesBll.GetFileByName2(file).FirstOrDefault();



            if (filesDataTable != null)
            {
                fileId = filesDataTable.FiletId;
            }
            else
            {
                string completePath = Path.Combine(this.fileSystemRoot, file);


                if (File.Exists(completePath))
                {
                    FileInfo finfo = new FileInfo(file);

                    string[] parts = finfo.Name.Split('.');

                    if (parts.Length > 0)
                    {
                        fileId = filesBll.AddFile2(parts[0], file, this.SelectedUserId, thumbNail);
                    }

                }
            }

            if (fileId != Guid.Empty
                && this.parentId != Guid.Empty)
            {
                sourcesMappingBLL.Insert(this.ParentId, fileId, null, this.SelectedUserId, null, DateTime.Today.ToShortDateString(), null);
            }
            else
            {
                Debug.WriteLine("FAILED AddNewFileWithMapping");
                Debug.Assert(false);
            }



            this.Refresh();
        }
       
        public void AddNewFileWithMapping(string file)
        {
             //create thumbnail here

            //this.SetErrorState(

            

            string fileWithPath = this.FileSystemRoot + file;



            if (!File.Exists(fileWithPath))
            {
                this.SetErrorState("File: " + fileWithPath + " doesnt exist" );
            }
            else
            {
                Image _tp = Image.FromFile(fileWithPath);

                Size sz = new Size(200, 200);

                Image sImg = null;
                sImg = resizeImage(_tp, sz);

                // relies on path having trailing seperator!
                string savePath = Path.Combine(this.FileSystemRoot, FilesBLL.DirContentThumbs()) + file;

                sImg.Save(savePath);



                //check file exsits
                AddNewFileWithMapping(file, "thumbs\\" + file);
            }
        }


       




        public override void DeleteSelectedRecords()
        {
            if (!IsValidDelete()) return;

            FilesBLL filesBll = new GedItter.BLL.FilesBLL(this.fileSystemRoot);
            filesBll.DeleteFile2(this.selectedFileId);
            this.Refresh();
        }
        public override void EditSelectedRecord()
        {

            if (!IsvalidSelect()) return;

            FileEditorModel fileEditorModel = new FileEditorModel();
            this.SetEditorUI();

            fileEditorModel.SetSelectedRecordId(this.selectedFileId);

            this.iFileEditorUI.SetEditorModal(fileEditorModel);

            this.iFileEditorUI.Show();


        }

        public override void InsertNewRecord()
        {

            if (!IsValidInsert()) return;

            FileEditorModel fileEditorModel = new FileEditorModel();
            this.SetEditorUI();

            fileEditorModel.SetSelectedRecordId(this.selectedFileId);

            this.iFileEditorUI.SetEditorModal(fileEditorModel);

            this.iFileEditorUI.Show();
        }

        public override void Refresh()
        {
            if (!IsvalidSelect()) return;


            BLL.FilesBLL filesBLL = new FilesBLL(fileSystemRoot);
        
            filesDataTable = filesBLL.GetFiles2(fileName, fileDescription).ToList();
            
            if (this.parentId != Guid.Empty && !this.IsReadOnly)
                filesMappedDataTable = filesBLL.GetFilesByParentId2(this.parentId).ToList();

            List<Guid> tpList = new List<Guid>();


            if (filesMappedDataTable != null)
            {
                foreach (var _fr in filesMappedDataTable)
                {
                    tpList.Add(_fr.FiletId);
                }
            }

            this.SetSelectedRecordIds(tpList);

            this.NotifyObservers<FileMappingModel>(this);
        }


        #region set UI

        public void SetEditorUI()
        {
         //   this.iFileEditorUI = new Forms.FrmEditFile();
        }

        #endregion

        public void ClearMappings()
        {
            filesMappedDataTable = new List<TDBCore.EntityModel.File>();
            this.Refresh();
        }


        private static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }
    }
}
