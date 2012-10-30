using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.Types;
using GedItter.ModelObjects;
////using TDBCore.Datasets;

namespace GedItter.Interfaces
{

    public interface ISourceEditorUI
    {
        ISourceEditorModel ISourceEditorModel { get; }
        void SetEditorModal(ISourceEditorModel ISourceEditorModel);
        void Show();
    }

    public interface ISourceEditorModel : IDBRecordModel<Guid>
    {
        bool IsValidSourceDate { get; }
        bool IsValidSourceDateTo { get; }
        bool IsValidSourceDescription { get; }
        bool IsValidSourceRef { get; }
        bool IsValidSourceOriginalLocation { get; }

        List<Guid> SourceParishs { get; }
        List<Guid> SourceFileIds { get; }
        List<int> SourceTypeIdList { get; }

 

        string SourceDateStr { get; }
        string SourceDateToStr { get; }
        string SourceDescription { get; }
        string SourceRef { get; }
        string SourceOriginalLocation { get; }
        bool? IsCopyHeld { get; }
        bool? IsViewed { get; }
        bool? IsThackrayFound { get; }

        string SourceNotes { get; }
        string SourceFileCount { get; }

        void SetSourceNotes(string param);
        void SetSourceFileCount(string param);

        void SetSourceParishs(List<Guid> param);
        void SetSourceFileIds(List<Guid> param);
        void SetSourceTypeIdList(List<int> param);



        void SetSourceDateStr(string param); 
        void SetSourceDateToStr (string param); 
        void SetSourceDescription(string param);
        void SetSourceRef(string param);
        void SetSourceOriginalLocation(string param); 
        void SetIsCopyHeld(bool? param); 
        void SetIsViewed(bool? param); 
        void SetIsThackrayFound(bool? param);
        
 
    }

    public interface ISourceEditorControl : IDBRecordControl<Guid>
    {

        void RequestSetSourceNotes(string param);
        void RequestSetSourceFileCount(string param);

        void RequestSetSourceParishs(List<Guid> param);
        void RequestSetSourceFileIds(List<Guid> param);
        void RequestSetSourceTypeIdList(List<int> param);
        void RequestSetSourceDateStr(string param);
        void RequestSetSourceDateToStr(string param);
        void RequestSetSourceDescription(string param);
        void RequestSetSourceRef(string param);
        void RequestSetSourceOriginalLocation(string param);
        void RequestSetIsCopyHeld(bool? param);
        void RequestSetIsViewed(bool? param);
        void RequestSetIsThackrayFound(bool? param);
       // void RequestSetUserId(int param); 

        //void SetModel(ISourceEditorModel paramModel);
        //void SetView(ISourceEditorView paramView);
        //void SetView();
    }

    public interface ISourceEditorView : IDBRecordView
    {

        void ShowInvalidSourceDate(bool valid);
        void ShowInvalidSourceDateTo(bool valid);
        void ShowInvalidSourceDescription(bool valid);
        void ShowInvalidSourceRef(bool valid);
        void ShowInvalidSourceOriginalLocation(bool valid);
        void ShowInvalidUser(bool valid);


     //   void Update(ISourceEditorModel paramModel);

    }

    public interface ISourceEditor
    {
        IList<TDBCore.EntityModel.Source> GetTable(string param);

    }



    public interface ISourceFilterModel : IDBRecordsModel<Guid>
    {

        ///DsFileClass.FilesDataTable FilesDataTable { get; }
        //IList<TDBCore.EntityModel.Source> SourcesDataTable { get; }
        ServiceSourceObject SourcesDataTable { get; }
        ISourceEditorModel ISourceEditorModel { get; }
        ISourceEditorUI ISourceEditorUI { get; }
        
        bool IsValidSourceDateUpperBound { get; }
        bool IsValidSourceDateLowerBound { get; }
        bool IsValidSourceToDateUpperBound { get; }
        bool IsValidSourceToDateLowerBound { get; }
        bool IsValidSourceDescription { get; }
        bool IsValidSourceRef { get; }
        bool IsIncludeDefaultPerson { get; }


        string FilterSourceDateUpperBound { get; }
        string FilterSourceDateLowerBound { get; }
        string FilterSourceToDateUpperBound { get; }
        string FilterSourceToDateLowerBound { get; }

        string FilterSourceRef { get; }
        string FilterSourceDescription { get; }
        string FilterSourceOriginalLocation { get; }
        SourceFilterTypes FilterSourceType { get; }


        bool? FilterIsCopyHeld { get; }
        bool? FilterIsViewed { get; }

        bool? FilterIsThackrayFound { get; }

        int SourceRecordCount { get; }


        List<int> FilterSourceTypeList { get; }

        string FilterSourceFileCount { get; }


        List<string> SourceRefs { get; }
        string FilteredPrintableResults { get; }

        void SetFilterMode(SourceFilterTypes param);//  

        void SetFilteredPrintableResults(string param, bool isTabular);
        void SetFilterSourceFileCount(string param, bool useParam);
        void SetFilterSourceRef(string param);
        void SetFilterSourceDescription(string param);
        void SetFilterSourceOriginalLocation(string param);
        void SetFilterIsCopyHeld(bool? param, bool? useParam);
        void SetFilterIsViewed(bool? param, bool? useParam);
        void SetFilterIsThackrayFound(bool? param, bool? useParam);
        void SetFilterSourceTypeList(List<int> param);
        void SetFilterSourceDateUpperBound(string param);
        void SetFilterSourceDateLowerBound(string param);
        void SetFilterSourceToDateUpperBound(string param);
        void SetFilterSourceToDateLowerBound(string param);
        void SetFilterIncludeDefaultPerson(string param);


        void SetEditorUI(ISourceEditorUI paramISourceEditorUI);
    }

    public interface ISourceFilterControl : IDBRecordsControl<Guid>
    {
        void RequestSetFilterSourceRef(string param);
        void RequestSetFilterSourceDescription(string param);
        void RequestSetFilterSourceOriginalLocation(string param);
        void RequestSetFilterIsCopyHeld(bool? param, bool? useParam);
        void RequestSetFilterIsViewed(bool? param, bool? useParam);
        void RequestSetFilterIsThackrayFound(bool? param, bool? useParam);
        void RequestSetFilterSourceTypeList(List<int> param);
        void RequestSetFilteredPrintableResults(string param, bool isTabular);
        void RequestSetFilterSourceDateUpperBound(string param);
        void RequestSetFilterSourceDateLowerBound(string param);
        void RequestSetFilterSourceToDateUpperBound(string param);
        void RequestSetFilterSourceToDateLowerBound(string param);
        void RequestSetFilterSourceFileCount(string param, bool useParam);
        void RequestSetFilterIncludeDefaultPerson(string param);
        void RequestSetFilterMode(SourceFilterTypes param);//  

    }

    public interface ISourceFilterView : IDBRecordView
    {

       

    //    string SourceIdsFound { get; }
      //  List<Guid> SelectedRecordIds { get; }
      
      //  event EventHandler ShowEditor;

        void ShowInvalidSourceDateUpperBoundWarning(bool valid);
        void ShowInvalidSourceDateLowerBoundWarning(bool valid);
        void ShowInvalidSourceToDateUpperBoundWarning(bool valid);
        void ShowInvalidSourceToDateLowerBoundWarning(bool valid);
        void ShowInvalidSourceDescriptionWarning(bool valid);
        void ShowInvalidSourceRefWarning(bool valid);

     //   void RefreshLists();

        //IList<TDBCore.EntityModel.Source> GetTable(string param, string SortExpression);
    }

    public interface ISourceFilterWeb  
    {
        IList<TDBCore.EntityModel.Source> GetTable(string param, string SortExpression);
    }


    public interface ISourceMappingModel : IDBRecordsModel<Guid>
    {
        Guid ParentId { get; }
        Guid SelectedSourceId { get; }
        Guid SelectedMappedSourceId { get; }
        string SourceRef { get; }
        IList<TDBCore.EntityModel.Source> SourcesDataTable { get; }
        IList<TDBCore.EntityModel.Source> SourcesMappedDataTable { get; }

        ISourceEditorUI ISourceEditorUI { get; }

        void SetSelectedSourceId(Guid mapFileId);
        void SetSelectedMappedSourceId(Guid mapFileId);
        void SetParentMarriageId(Guid parentId);
        void SetSourceRef(string param);



        void RemoveMapping();
        void AddMapping();

        void AddDisconnectedSourceMapRow(Guid paramGuid);

        void AddOutstandingMappings(Guid oldParentId, Guid newParentId);

     //   string SourceDescription { get; }
     //  void SetParentPersonId(Guid parentId);
      //  void SetSourceDescrip(string param);
    //    void RemoveDisconnectedSourceMapRow(Guid paramGuid);
        //void AddObserver(ISourceMappingView paramView);
        //void RemoveObserver(ISourceMappingView paramView);
        //void NotifyObservers();
    }


    public interface ISourceMappingControl : IDBRecordsControl<Guid>
    {

      //  void RequestSetParentPerson(Guid parentId);
       // void RequestSetParentMarriage(Guid parentId);
     //   void RequestSetSourceDescrip(string param);
        //void RequestRemoveDisconnectedSourceMapRow(Guid paramGuid);
     //   void RequestAddDisconnectedSourceMapRow(Guid paramGuid);

        void RequestAddMultipleDisconnectedSourceMapRow(List<Guid> paramGuid);
        void RequestSetSelectedSourceSourceId(Guid mapFileId);
        void RequestSetSelectedMappedSourceId(Guid mapFileId);
        void RequestSetSourceRef(string param);

        void RequestRemoveMapping();
        void RequestAddMapping();


        //void SetModel(ISourceMappingModel paramModel);
        //void SetView(ISourceMappingView paramView);
        //void SetView();

    }


    public interface ISourceMappingView : IDBRecordView
    {
        // set parent
     //   void Update(ISourceMappingModel paramModel);
        //void SelectionListAddSource(Guid paramGuid);
        //void SelectionListRemoveSource(Guid paramGuid);

        //void SetParentMarriageId(Guid parentId);
        //void SetParentPersonId(Guid parentId);


        //void RefreshLists();


        void AddSources(List<Guid> paramGuids);
        List<Guid> SelectedSourceIds { get; }

    }


    public interface ISourceMapping
    {
        IList<TDBCore.EntityModel.Source> GetTable(string param);

        IList<TDBCore.EntityModel.Source> GetMappedTable(string param);


    }
}
