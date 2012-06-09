using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
////using TDBCore.Datasets;


namespace GedItter.Interfaces
{


    public interface IFileEditorUI
    {



     //   IFileEditorModel iFileEditorModel { get; }


        void SetEditorModal(IFileEditorModel iFileEditorModel);


        void Show();

    }

    public interface IFileEditorModel : IDBRecordModel<Guid>
    {
        bool IsValidFilePath { get; }
        bool IsValidFileDescription { get; }
        bool IsValidFileThumbLocation { get; }
        bool IsValidFileSystemRoot { get; }


        string FilePath { get; }
        string FileDescription { get; }
        string FileThumbLocat { get; }
        string FileSystemRoot { get; }

        void SetFileThumbLocat(string fileThumbLocat);
        void SetFilePath(string filePath);
        void SetFileDescription(string fileDescrip);
        void SetFileSystemRoot(string param);
  

    }

    public interface IFileEditorControl : IDBRecordControl<Guid>
    {
        void RequestSetFilePath(string filePath);
        void RequestSetDescription(string fileDescription);
        void RequestSetFileThumbLocat(string fileThumbLocat);
        void RequestSetFileSystemRoot(string param);


        //void SetModel(IFileEditorModel paramModel);
        //void SetView(IFileEditorView paramView);
        //void SetView();
    }

    public interface IFileEditorView : IDBRecordView
    {
        void ShowValidFilePath(bool valid);
        void ShowValidFileDescription(bool valid);
        void ShowValidFileThumbLocation(bool valid);
        void ShowValidFileSystemRoot(bool valid);
     //   void Update(IFileEditorModel paramModel);

    }


    public interface IFileFilterModel : IDBRecordsModel<Guid>
    {

    //    DsFileClass.FilesDataTable FilesDataTable { get; }
        IList<TDBCore.EntityModel.File> FilesList { get; }


        bool IsValidEditDateUpperBound { get; }
        bool IsValidEditDateLowerBound { get; }
        bool IsValidAddDateUpperBound { get; }
        bool IsValidAddDateLowerBound { get; }

        IFileEditorUI IFileEditorUI { get; }
        IFileEditorModel IFileEditorModel { get; }

        string FilterDateEditUpper { get; }
        string FilterDateEditLower { get; }
        string FilterDateAddUpper { get; }
        string FilterDateAddLower { get; }

        string FilterFilePath { get; }
        string FilterFileDescrip { get; }
        string FilterFileRootPath { get; }


        void SetEditorUI(IFileEditorUI paramIFileEditorUI);
        void SetFilterDateEditUpper(string param);
        void SetFilterDateEditLower(string param);
        void SetFilterDateAddUpper(string param);
        void SetFilterDateAddLower(string param);
        void SetFilterFilePath ( string param );
        void SetFilterFileDescrip ( string param );
        void SetFilterFileRootPath(string param);

       // void ViewImage();

        //void AddObserver(IFileFilterView paramView);
        //void RemoveObserver(IFileFilterView paramView);
        //void NotifyObservers();

    }

    public interface IFileFilterControl : IDBRecordsControl<Guid>
    {
        void RequestSetFilterDateEditUpper(string param);
        void RequestSetFilterDateEditLower(string param);
        void RequestSetFilterDateAddUpper(string param);
        void RequestSetFilterDateAddLower(string param);
        void RequestSetFilterFilePath(string param);
        void RequestSetFilterFileDescrip(string param);
        void RequestSetFilterFileRootPath(string param);

        //void SetModel(IFileFilterModel paramModel);
        //void SetView(IFileFilterView paramView);
        //void SetView();
    }


    public interface IFileFilterView : IDBRecordView
    {

        void ShowInvalidEditDateUpperBoundWarning(bool valid);
        void ShowInvalidEditDateLowerBoundWarning(bool valid);
        void ShowInvalidAddDateUpperBoundWarning(bool valid);
        void ShowInvalidAddDateLowerBoundWarning(bool valid);
     //   event EventHandler ShowEditor;

        //void Update(IFileFilterModel paramModel);
    }


    public interface IFileFilter
    {
       // DsFileClass.FilesDataTable GetTable(string param);
        IList<TDBCore.EntityModel.File> GetTable(string param);

    }
}
