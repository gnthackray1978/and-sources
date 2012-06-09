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

    public class FileView : IFileEditorView
    {

        public bool IsValidFilePath { get; set; }
        public bool IsValidFileDescription { get; set; }
        public bool IsValidFileThumbLocation { get; set; }
        public bool IsValidFileSystemRoot { get; set; }

        public IFileEditorControl ifileEditorControl = null;
        public IFileEditorModel iFileEditorModel = null;

        public void ShowValidFilePath(bool valid)
        {
            this.IsValidFilePath = valid;
        }

        public void ShowValidFileDescription(bool valid)
        {
            this.IsValidFileDescription = valid;
        }

        public void ShowValidFileThumbLocation(bool valid)
        {
            this.IsValidFileThumbLocation = valid;
        }

        public void ShowValidFileSystemRoot(bool valid)
        {
            this.IsValidFileSystemRoot = valid;
        }



        public FileView()
        {
            iFileEditorModel = new FileEditorModel();
            ifileEditorControl = new FileEditorControl();

            if (iFileEditorModel != null)
                ifileEditorControl.SetModel(iFileEditorModel);

            if (ifileEditorControl != null)
                ifileEditorControl.SetView(this);
        }

        public void Update<T>(T paramModel)
        {

        }



    }



    
    [TestFixture]
    public class NUnitFile : TestData
    {

        [Test(Description = "Valid File Path")]
        public void CheckValidFilePath()
        {
            FileView fileView = new FileView();
            fileView.ifileEditorControl.RequestSetFilePath(@"c:\test.txt");
            Assert.IsTrue(fileView.iFileEditorModel.IsValidFilePath);
        }

        [Test(Description = "Invalid File Path")]
        public void CheckInvalidFilePath()
        {
            FileView fileView = new FileView();
            fileView.ifileEditorControl.RequestSetFilePath("");
            Assert.IsFalse(fileView.iFileEditorModel.IsValidFilePath);
        }


        //public bool IsValidFileDescription { get; set; }
        [Test(Description = "Valid File Desc")]
        public void CheckValidFileDesc()
        {
            FileView fileView = new FileView();
            fileView.ifileEditorControl.RequestSetDescription("file desc");
            Assert.IsTrue(fileView.iFileEditorModel.IsValidFileDescription);
        }

        [Test(Description = "Invalid File Desc")]
        public void CheckInvalidFileDesc()
        {
            FileView fileView = new FileView();
            fileView.ifileEditorControl.RequestSetDescription(this.GetStringByLen(501));
            Assert.IsFalse(fileView.iFileEditorModel.IsValidFilePath);
        }




        //public bool IsValidFileThumbLocation { get; set; }
        [Test(Description = "ValidFileThumbLocat")]
        public void CheckValidFileThumbLocat()
        {
            FileView fileView = new FileView();
            fileView.ifileEditorControl.RequestSetFileThumbLocat(@"c:\test.txt");
            Assert.IsTrue(fileView.iFileEditorModel.IsValidFileThumbLocation);
        }

        [Test(Description = "InvalidFileThumbLocat")]
        public void CheckInvalidFileThumbLocat()
        {
            FileView fileView = new FileView();
            fileView.ifileEditorControl.RequestSetFileThumbLocat("");

            Assert.IsFalse(fileView.iFileEditorModel.IsValidFileThumbLocation);

            fileView.ifileEditorControl.RequestSetFileThumbLocat(@"c:\xx.txxxt");

            Assert.IsFalse(fileView.iFileEditorModel.IsValidFileThumbLocation);
        }



        //public bool IsValidFileSystemRoot { get; set; }
        [Test(Description = "ValidFileSystemRoot")]
        public void CheckValidFileSystemRoot()
        {
            FileView fileView = new FileView();
            fileView.ifileEditorControl.RequestSetFileSystemRoot(@"c:\windows\");
            Assert.IsTrue(fileView.iFileEditorModel.IsValidFileSystemRoot);
        }

        [Test(Description = "InvalidFileSystemRoot")]
        public void CheckInvalidFileFileSystemRoot()
        {
            FileView fileView = new FileView();
            fileView.ifileEditorControl.RequestSetFileSystemRoot("");
            Assert.IsFalse(fileView.iFileEditorModel.IsValidFileSystemRoot);

            fileView.ifileEditorControl.RequestSetFileSystemRoot(@"c:\xxxxx\");
            Assert.IsFalse(fileView.iFileEditorModel.IsValidFileSystemRoot);
        }

        [Test(Description = "Add File")]
        public void AddFile()
        {
            FileView fileView = new FileView();

            fileView.iFileEditorModel.SetIsSecurityEnabled(false);
            fileView.ifileEditorControl.RequestSetFilePath(@"c:\test.txt");

            fileView.ifileEditorControl.RequestSetDescription("test file desc");

            Assert.IsTrue(fileView.iFileEditorModel.IsValidEntry);

            fileView.ifileEditorControl.RequestInsert();



            Assert.IsTrue(fileView.iFileEditorModel.SelectedRecordId != Guid.Empty);


            FilesBLL filesBll = new FilesBLL();

            File _file = filesBll.ModelContainer.Files.Where(f => f.FiletId == fileView.iFileEditorModel.SelectedRecordId).FirstOrDefault();


            Assert.IsNotNull(_file);

            fileView.ifileEditorControl.RequestDelete();


            _file = filesBll.ModelContainer.Files.Where(f => f.FiletId == fileView.iFileEditorModel.SelectedRecordId).FirstOrDefault();


            Assert.IsNull(_file);
        }
    }
 


}
