using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
////using TDBCore.Datasets;
using TDBCore.EntityModel;

namespace GedItter.Interfaces
{

    #region ISourceTypeEditor

    public interface ISourceTypeEditorUI
    {
        ISourceTypeEditorModel iSourceTypeEditorModel { get; }


        void SetEditorModal(ISourceTypeEditorModel iSourceTypeEditorModel);


        void Show();
    }

    public interface ISourceTypeEditorModel : IDBRecordModel<int>
    {
        bool IsValidSourceTypeDesc { get; }

        bool IsValidSourceOrder
        {
            get;
        }

        string SourceTypeDesc { get; }

        int SourceTypeOrder { get; }

        void SetSourceTypeDesc(string sourceTypeDesc);
        void SetSourceTypeOrder(string param);


        //void AddObserver(ISourceTypeEditorView paramView);
        //void RemoveObserver(ISourceTypeEditorView paramView);
        //void NotifyObservers();
    }

    public interface ISourceTypeEditorControl : IDBRecordControl<int>
    {

        void RequestSetSourceTypeDesc(string sourceTypeDesc);
        void RequestSetSourceTypeOrder(string param);

        //void SetModel(ISourceTypeEditorModel paramModel);
        //void SetView(ISourceTypeEditorView paramView);
        //void SetView();
    }

    public interface ISourceTypeEditorView : IDBRecordView
    {
        void ShowValidSourceTypeDesc(bool valid);
        void ShowValidSourceTypeOrder(bool valid);

    //    void Update(ISourceTypeEditorModel paramModel);
    }

    #endregion

    #region ISourceTypeFilters

    public interface ISourceTypeFilterModel : IDBRecordsModel<int>
    {

        //mode?

        //DsSourceTypes.SourceTypesDataTable SourceTypesDataTable { get; }
        IList<SourceType> SourceTypesDataTable { get; }


        ISourceTypeEditorUI ISourceTypeEditorUI { get; }

        ISourceTypeEditorModel ISourceTypeEditorModel { get; }


        string SourceTypesDescrip { get; }

        void SetSourceTypesDescrip(string param);

        void SetEditorUI(ISourceTypeEditorUI paramISourceTypeEditorUI);


        // set parent source id
        // add sourceType to parent source
        // remove sourcetype from parent


        //void AddObserver(ISourceTypeFilterView paramView);
        //void RemoveObserver(ISourceTypeFilterView paramView);
        //void NotifyObservers();
    }

    public interface ISourceTypeFilterControl : IDBRecordsControl<int>
    {
        void RequestSetSourceTypesDescrip(string param);
        // set parent
        // remove parent

        //void SetModel(ISourceTypeFilterModel paramModel);
        //void SetView(ISourceTypeFilterView paramView);
        //void SetView();
    }

    public interface ISourceTypeFilterView : IDBRecordView
    {
   //     event EventHandler ShowEditor;
        // set parent
     //   void Update(ISourceTypeFilterModel paramModel);

      
        List<int> SelectedSourceTypes { get; }

        void RefreshLists();
    }

    public interface ISourceTypeFilterDataCon
    {
        //DsSourceTypes.SourceTypesDataTable GetTable(string param);
        IList<SourceType> GetTable(string param);
    }

    #endregion

    #region ISourceTypeMapSource

    public interface ISourceTypeMapSourceModel : IDBRecordsModel<int>
    {
        Guid ParentId { get; }
        int SelectedSourceMapTypeId { get; }
        int SelectedMappedSourceTypeId { get; }

        string SourceTypeDescription { get; }

      //  DsSourceTypes.SourceTypesDataTable SourceTypesDataTable { get; }
        IList<SourceType> SourceTypesDataTable { get; }

      //  DsSourceTypes.SourceTypesDataTable SourceTypesMappedToSourceDataTable { get; }
        IList<SourceType> SourceTypesMappedToSourceDataTable { get; }


        ISourceTypeEditorModel ISourceTypeEditorModel { get; }

        void SetSelectedSourceMapTypeId(int mapTypeId);
        void SetSelectedMappedSourceTypeId(int mapTypeId);

        void SetParent(Guid parentId);
        void SetSourceTypesDescrip(string param);

        void RemoveMapping();
        void AddMapping();

        void AddDisconnectedSourceMapRow(int paramInt);
        void RemoveDisconnectedSourceMapRow(int paramInt);

        void ClearMappings();
        
        void AddOutstandingMappings(Guid oldParentId, Guid newParentId);

        //void AddObserver(ISourceTypeMapSourceView paramView);
        //void RemoveObserver(ISourceTypeMapSourceView paramView);
        //void NotifyObservers();
    }

    public interface ISourceTypeMapSourceControl : IDBRecordsControl<int>
    {

        void RequestSetMultipleSourceMapTypeIds(List<int> mapTypeId);
        void RequestSetSourceTypesDescrip(string param);
        void RequestRemoveMapping();
        void RequestAddMapping();
        void RequestSetSelectedSourceMapTypeId(int mapTypeId);
        void RequestSetSelectedMappedSourceTypeId(int mapTypeId);
        void RequestClearMappings();

        void RequestSetParent(Guid parentId);




        //void SetModel(ISourceTypeMapSourceModel paramModel);
        //void SetView(ISourceTypeMapSourceView paramView);
        //void SetView();



    }

    public interface ISourceTypeMapSourceView : IDBRecordView
    {
        // set parent
       // void Update(ISourceTypeMapSourceModel paramModel);
       
        //event EventHandler ShowEditor;

        void SetSelectedSourceTypeList(List<int> paramSourceTypeList);
        List<int> SelectedSourceTypes { get; }

        void SetParent(Guid parentId);
        void RefreshLists();

    }

    public interface ISourceTypeMappings
    {
     //   DsSourceTypes.SourceTypesDataTable GetTable(string param);
        IList<TDBCore.EntityModel.SourceType> GetTable(string param);

       // DsSourceTypes.SourceTypesDataTable GetMappedTable(string param);
        IList<TDBCore.EntityModel.SourceType> GetMappedTable(string param);

    }

    #endregion

    #region IFileMapSource

    public interface IFileMapSourceModel : IDBRecordsModel<Guid>
    {
        Guid ParentId { get; }
        Guid SelectedFileId { get; }
        Guid SelectedMappedFileId { get; }

        string FileDescription { get; }
        string FileName { get; }
        string FileThumbLocat { get; }
        string FileSystemRoot { get; }




      //  DsFileClass.FilesDataTable FilesDataTable { get; }
        IList<TDBCore.EntityModel.File> FilesList { get; }

     //   DsFileClass.FilesDataTable FilesMappedDataTable { get; }
        IList<TDBCore.EntityModel.File> FilesMappedList { get; }

        IFileEditorUI IFileEditorUI { get; }


        void SetSelectedFileId(Guid mapFileId);
        void SetSelectedMappedFileId(Guid mapFileId);

        void SetParentId(Guid parentId);
        void SetFileDescrip(string param);
        void SetFileName(string param);
        void SetFileThumbLocat(string param);
        void SetFileSystemRoot(string param);
        void ClearMappings();

        void RemoveMapping();
        void AddMapping();

        void AddDisconnectedSourceMapRow(Guid paramGuid);
        void RemoveDisconnectedSourceMapRow(Guid paramGuid);
        void AddOutstandingMappings(Guid oldParentId, Guid newParentId);


        void AddNewFileWithMapping(string file, string thumbNail);
        void AddNewFileWithMapping(string file);
 
    }

    public interface IFileMapSourceControl : IDBRecordsControl<Guid>
    {

        void RequestAddMultipleDisconnectedSourceMapRow(List<Guid> paramGuid);
        void RequestSetSelectedSourceFileId(Guid mapFileId);
        void RequestSetSelectedMappedFileId(Guid mapFileId);

        void RequestSetParent(Guid parentId);
        void RequestSetFileDescrip(string param);
        void RequestSetFileName(string param);
        void RequestSetThumbLocat(string param);
        void RequestSetFileSystemRoot(string param);
        void RequestClearMappings();
        void RequestRemoveMapping();
        void RequestAddMapping();
        void RequestAddNewFileWithMapping(string file, string thumbLocat);
        void RequestAddNewFileWithMapping(string file);

        //void SetModel(IFileMapSourceModel paramModel);
        //void SetView(IFileMapSourceView paramView);
        //void SetView();

    }

    public interface IFileMapSourceView : IDBRecordView
    {
        // set parent
      //  void Update(IFileMapSourceModel paramModel);

        List<Guid> SelectedFiles { get; }

        void SetSelectedSourceTypeList(List<Guid> paramFileList);



        void SetParent(Guid parentId);
        void RefreshLists();

    }

    public interface IFileMapSource
    {
        IList<TDBCore.EntityModel.File> GetTable(string param);
       // IList<TDBCore.EntityModel.File> FilesList { get; }

        IList<TDBCore.EntityModel.File> GetMappedTable(string param);


    }

    #endregion

    #region ISourceMappingParishs
    public interface ISourceMappingParishsModel : IDBRecordsModel<Guid>
    {
        Guid ParentId { get; }

        Guid SelectedParishId { get; }

        Guid SelectedMappedParishId { get; }

        string ParishDescription { get; }

        IList<TDBCore.EntityModel.Parish> ParishsDataTable { get; }

        IList<TDBCore.EntityModel.Parish> SourceMappingParishsDataTable { get; }


        IParishsEditorModel IParishsEditorModel { get; }
        IParishEditorUI IParishEditorUI { get; }
        void ClearMappings();
        void SetSelectedParishId(Guid paramGuid);
        void SetSelectedMappedParishId(Guid paramGuid);

        void SetParent(Guid parentId);
        void SetParishDescription(string param);

        void RemoveMapping();
        void AddMapping();

        void AddDisconnectedSourceMapRow(Guid paramGuid);
        void RemoveDisconnectedSourceMapRow(Guid paramGuid);
        
        void AddOutstandingMappings(Guid oldParentId, Guid newParentId);

        //void AddObserver(ISourceMappingParishsView paramView);
        //void RemoveObserver(ISourceMappingParishsView paramView);
        //void NotifyObservers();
    }

    public interface ISourceMappingParishsControl : IDBRecordsControl<Guid>
    {

        void RequestSetMultipleSelectedParishIds(List<Guid> paramGuids);
        void RequestSetSelectedParishId(Guid paramGuid);
        void RequestSetSelectedMappedParishId(Guid paramGuid);

        void RequestClearMappings();
        void RequestParishDescription(string param);


        void RequestRemoveMapping();
        void RequestAddMapping();

        void RequestSetParent(Guid parentId);

        //void SetModel(ISourceMappingParishsModel paramModel);
        //void SetView(ISourceMappingParishsView paramView);
        //void SetView();
    }

    public interface ISourceMappingParishsView : IDBRecordView
    {
        // set parent
        //void Update(ISourceMappingParishsModel paramModel);

        void SetSelectedParishsList(List<Guid> paramParishsList);
        List<Guid> SelectedParishList { get; }

        void SetParent(Guid parentId);
        void RefreshLists();

    }

    public interface ISourceMappingParishs
    {
        IList<TDBCore.EntityModel.Parish> GetTable(string param);

        IList<TDBCore.EntityModel.Parish> GetMappedTable(string param);


    }

    #endregion


}
