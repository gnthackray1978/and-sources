using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.ModelObjects;

namespace GedItter.Interfaces
{
    public interface IDBRecordModel<T>
    {


        ISourceEditorModel ISourceEditorModel {get;}
        EventHandler ShowDialogEditEvent { get; }
        EventHandler ShowDialogInsertEvent { get; }
        EventHandler DataSaved { get; }
        T SelectedRecordId { get; }
       
        int SelectedUserId { get; }
        string SourceIdsFound { get; }
        string SourceGuidListAsString{get;}
        string PermissionState { get; }
        string ErrorState { get; }
        string StatusMessage { get; }
        string Name { get; }
        string User { get; }

        List<Guid> SourceGuidList { get; }
        List<int> LinkIntIds {get;}
        bool IsDataChanged { get; }
        bool IsValidEntry { get; }
        bool IsValidUser { get; }
        bool IsValidSelectedRecordId { get; }
        bool IsReadOnly { get; }
        bool IsDataUpdated { get; }
        bool IsDataInserted { get; }
        bool IsSecurityEnabled { get; }

        bool ASC { get; }
        int RecordCount { get; }
        int RecordPageSize { get; }
        int RecordStart { get; }

        void SetIsSecurityEnabled(bool param);    
        void SetRecordCount(int param);
        void SetASC(bool param);
        void SetRecordPageSize(int param);
        void SetRecordStart(int param);
        void SetIsDataChanged(bool param);


        void SetLinkInts(List<int> param);
        void SetSourceGuidList(List<Guid> param);

        List<T> ParentRecordIds { get; }

        void SetDataSaved(EventHandler param);
        void SetISourceEditorModel(ISourceEditorModel param);
        void SetParentRecordIds(List<T> recordIds);
        
        void SetParentRecordIds(T recordIds);

        void SetFromQueryString(string param);

        void SetPermissionState(string param);
        void SetErrorState(string param);
        void SetIsReadOnly(bool paramIsReadOnly);    
        void SetSelectedRecordId(T recordId);
        void SetSelectedUserId(int _id);
        void SetSelectedUser(string param);

        void SetModelStatusFields();
        void DeleteSelectedRecords();
        void EditSelectedRecord();
        void InsertNewRecord();
        void Refresh();


         void SetShowDialogEdit(EventHandler paramEventHandler);

         void SetShowDialogInsert(EventHandler paramEventHandler);

         void ShowDialogEdit(object sender);

         void ShowDialogInsert(object sender);


         void AddObserver(IDBRecordView paramView);
         void RemoveObserver(IDBRecordView paramView);
         void NotifyObservers<T>(T model);

         void RemoveAllObservers();

         bool IsValidEdit();
         bool IsValidDelete();
         bool IsValidInsert();
         bool IsvalidSelect();



        
    }

    public interface IDBRecordsModel<T> : IDBRecordModel<T>
    {
        List<T> SelectedRecordIds { get; }        
        void SetSelectedRecordIds(List<T> recordIds);
        void SetSelectedRecordIds(T Ids);
        void AddObserver(IDBRecordView paramView);
        void RemoveObserver(IDBRecordView paramView);
        void NotifyObservers<T>(T model);


        void SetEditorUI();
    }


    public interface IDBRecordView
    {
        


        void Update<T>(T paramModel);
    }

    public interface IDBRecordsView<T> : IDBRecordView
    {
        void SetParentIds(List<T> Ids);//  
    }


    public interface IDBRecordControl<T>
    {
        void RequestSetDataSaved(EventHandler param);
        void RequestSetISourceEditorModel(ISourceEditorModel param);
        void RequestSetLinkInts(List<int> param);
        void RequestSetSourceGuidsList(List<Guid> param);
        void RequestSetParentRecordIds(List<T> Ids);
        void RequestSetParentRecordIds(T Ids);
        void RequestSetSelectedId(T Id);
        void RequestSetUserId(int _id);
        void RequestSetUser(string param);
        void RequestSetErrorState(string param);
        void RequestSetIsDataChanged(bool param);
        void RequestSetRecordCount(int param);
        void RequestSetASC(bool param);
        void RequestSetRecordPageSize(int param);
        void RequestSetRecordStart(int param);

        // Deletes marriage currently selected for editting
        void RequestDelete();

        // update editor values
        void RequestUpdate();

        // insert editor marriage
        void RequestInsert();

        void RequestRefresh();

        void SetModel(IDBRecordModel<T> paramModel);
        void SetView(IDBRecordView paramView);

        void SetView();

    }

    public interface IDBRecordsControl<T> :IDBRecordControl<T>
    {
        void RequestSetSelectedIds(List<T> Ids);
        void RequestSetSelectedIds(T Ids);
    
    }
}
