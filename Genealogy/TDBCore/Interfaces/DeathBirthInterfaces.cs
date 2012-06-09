using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.MarriageRecords;

//using TDBCore.Datasets;
using GedItter.Interfaces;

namespace GedItter.BirthDeathRecords
{
    class DeathBirthInterfaces
    {
    }






    public interface IDeathBirthFilterModel : IDBRecordsModel<Guid>
    {
       // DsDeathsBirths.PersonsDataTable PersonsDataTable{get;}
        IList<TDBCore.EntityModel.Person> PersonsDataTable { get; }
     //   DsRelationTypes.RelationTypesDataTable RelationTypesDataTable { get; }


        bool IsValidBirthUpperBound { get; }
        bool IsValidBirthLowerBound { get; }
        bool IsValidDeathUpperBound { get; }
        bool IsValidDeathLowerBound { get; }

        IDeathBirthEditorUI IDeathBirthEditorUI { get; }

        void SetEditorUI(IDeathBirthEditorUI paramIDeathBirthEditorUI);


        IDeathBirthEditorModel DeathBirthEditorModel { get; }

        void ViewDuplicates();
        void ViewRelations();
        void DeleteRelation();

        void UpdateDateEstimates();

        void MergeDuplicates();

        void SetShowDialogDupes(EventHandler paramEventHandler);
        
        void SetShowDialogRels(EventHandler paramEventHandler);


       Guid  MyGuid { get; }
       Guid  MyFurthestAncestorGuid { get; }


        #region filters

        DeathBirthFilterTypes FilterMode { get; }

        int RelationTypeId { get; }
        // view filter settings
        string FilterUpperBirth { get; }
        string FilterLowerBirth { get; }
        string FilterUpperDeath { get; }
        string FilterLowerDeath { get; }

        bool IsIncludeDeaths{ get; }

        bool IsIncludeBirths{ get; }


        string FilterCName { get; }
        string FilterSName { get; }



        string FilterMotherCName {get;}
        string FilterMotherSName {get;}
        string FilterFatherCName {get;}
        string FilterFatherSName {get;}

        string FilterCountyLocation { get; }
        string FilterDeathCountyLocation { get; }


        string FilterLocation { get; }
        string FilterDeathLocation { get; }

        string FilterSource { get; }

        string FilterSpouseCName { get; }
        string FilterSpouseSName { get; }
        string FilterFatherOccupation { get; }

        string FilteredPrintableResults { get; }
        bool FilterTreeResults { get; }

        void SetFilteredPrintableResults(string param, bool isTabular);

        List<int> FilterRelations { get; }
        void SetRelation(int typeId);
        void SetRelation(List<int> typeId);
        void SetFilterMode(DeathBirthFilterTypes param);//  
        void SetFilterSource(string param);
        void SetFilterTreeResults(bool param);

        void SetFilterIsIncludeDeaths(bool param);
        void SetFilterIsIncludeBirths(bool param);

        // set filters
        void SetRelationTypeId(int relationTypeId);

        void RemoveRelationType();
        void RemoveAllRelationType();

        void SetFilterUpperBirth(string param);
        void SetFilterLowerBirth(string param);
        void SetFilterUpperDeath(string param);
        void SetFilterLowerDeath(string param);

        void SetFilterSpouseCName(string param);
        void SetFilterSpouseSName(string param);
        void SetFilterFatherOccupation(string param);

        void SetFilterCName(string param);
        void SetFilterSName(string param);




        void SetFilterMotherCName(string param);
        void SetFilterMotherSName(string param);
        void SetFilterFatherCName(string param);
        void SetFilterFatherSName(string param);

        void SetFilterDeathLocation(string param);
        void SetFilterLocation(string param);

        void SetFilterDeathCountyLocation(string param);
        void SetFilterCountyLocation(string param);

        #endregion

        
    }

    public interface IDeathBirthFilterControl : IDBRecordsControl<Guid>
    {
        void RequestSetShowDialogDupes(EventHandler paramEventHandler);

        void RequestSetShowDialogRels(EventHandler paramEventHandler);

        //void RequestSetSelectedDeathBirthId(Guid marriageId);
        //void RequestSetSelectedDeathBirthIds(List<Guid> marriageIds);


        //void RequestDeleteDeathBirth();
        //void RequestUpdateDeathBirth();
        //void RequestInsertDeathBirth();
        //void SetFilteredPrintableResults(string param);


        void RequestUpdateDateEstimates();

        void RequestMergeDuplicates();
        void RequestUpdateEstimates();

        void RequestViewDuplicates();
        void RequestViewRelations();
        void RequestDeleteRelation();
        void RequestSetRelation(List<int> typeId);
        void RequestSetRelation(int typeId);
        void RequestSetFilterMode(DeathBirthFilterTypes param);//  


        void RequestRemoveRelationType();
       

        void RequestSetRelationTypeId(int relationTypeId);
        void RequestSetFilterSpouseCName(string param);
        void RequestSetFilterSpouseSName(string param);
        void RequestSetFilterFatherOccupation(string param);

        void RequestSetFilteredPrintableResults(string param, bool isTabular);


        void RequestSetFilterUpperBirth(string param);
        void RequestSetFilterLowerBirth(string param);
        void RequestSetFilterUpperDeath(string param);
        void RequestSetFilterLowerDeath(string param);


        void RequestSetFilterCName(string param);
        void RequestSetFilterSName(string param);



        void RequestSetFilterMotherCName(string param);
        void RequestSetFilterMotherSName(string param);
        void RequestSetFilterFatherCName(string param);
        void RequestSetFilterFatherSName(string param);

        void RequestSetFilterDeathLocation(string param);
        void RequestSetFilterDeathLocationCounty(string param);

        void RequestSetFilterLocationCounty(string param);
        void RequestSetFilterLocation(string param);
 
        void RequestSetFilterSource(string param);
        void RequestSetFilterTreeResults(bool param);

        void RequestSetFilterIsIncludeDeaths(bool param);
        void RequestSetFilterIsIncludeBirths(bool param);


        //void SetModel(IDeathBirthFilterModel paramModel);
        //void SetView(iDeathBirthFilterView paramView);
        //void SetView();
    }

    public interface iDeathBirthFilterView : IDBRecordsView<Guid>
    {
        void ShowInvalidUpperBoundDeathWarning(bool valid);
        void ShowInvalidLowerBoundDeathWarning(bool valid);

        void ShowInvalidUpperBoundBirthWarning(bool valid);
        void ShowInvalidLowerBoundBirthWarning(bool valid);

        void SetFilterMode(DeathBirthFilterTypes param);//  
        void SetParentIds(List<Guid> Ids);

        event EventHandler ShowEditor;

        //void Update(IDeathBirthFilterModel paramModel);
    }


    public interface IDeathBirthFilterWeb
    {
       // DsDeathsBirths.PersonsDataTable GetTable(string param);
        IList<TDBCore.EntityModel.Person> GetTable(string param, string SortExpression);
    }


    public interface IDeathBirthEditorUI
    {
        // interface is the form which contains the marriageEditorControl


       // IDeathBirthEditorModel IDeathBirthEditorModel { get; }


        void SetEditorModal(IDeathBirthEditorModel iDeathBirthEditorModel);


        void Show();

    }


    public interface IDeathBirthEditorModel: IDBRecordModel<Guid>
    {



        bool IsDataUpdated { get; }
        bool IsDataInserted { get; }

        string EditorSpouseCName { get; }
        bool IsValidSpouseCName { get; }
        string EditorSpouseSName { get; }
        bool IsValidSpouseSName { get; }
        string EditorFatherOccupation { get; }
        bool IsValidFatherOccupation { get; }
        string EditorReferenceDateString { get; }
        bool IsValidReferenceDate { get; }
        string EditorReferenceLocation { get; }
        bool IsValidReferenceLocation { get; }


        string EditorOccupation { get; }
        bool IsValidOccupation { get; }

        string EditorDateBirthString { get; }
        bool IsValidBirthDate { get; }
        string EditorDateDeathString {get;}
        bool IsValidDeathDate{get;}
        string EditorDateBapString { get; }
        bool IsValidBapDate { get; }

        string EditorChristianName { get; }
        bool IsValidName { get; }
        string EditorSurnameName { get; }
        bool IsValidSurname { get; }



        string EditorFatherChristianName { get; }
        bool IsValidFatherChristianName { get; }

        string EditorFatherSurname { get; }
        bool IsValidFatherSurname { get; }

        string EditorMotherChristianName { get; }
        bool IsValidMotherChristianName { get; }

        string EditorMotherSurname { get; }
        bool IsValidMotherSurname { get; }

        bool EditorIsMale{get;}

        string EditorBirthCountyLocation { get; }
        bool IsValidBirthCountyLocation { get; }

        string EditorDeathCountyLocation { get; }
        bool IsValidDeathCountyLocation { get; }

        string EditorBirthLocation { get; }
        bool IsValidLocation { get; }

        string EditorDeathLocation { get; }
        bool IsValidDeathLocation { get; }
 

        Guid EditorBirthLocationId { get; }
        bool IsValidBirthLocationId { get; }

        Guid EditorDeathLocationId { get; }
        bool IsValidDeathLocationId { get; }

        Guid EditorReferenceLocationId { get; }
        bool IsValidReferenceLocationId { get; }


        string EditorSource { get; }
        bool IsValidSource { get; }
        string EditorNotes { get; }
        bool IsValidNotes { get; }


        Guid EditorUniqueRef { get; }
        bool IsValidUniqueRef { get; }

        string FilterOriginalName { get; }
        bool IsValidOriginalName { get; }

        string FilterOriginalFatherName { get; }
        bool IsValidOriginalFatherName { get; }

        string FilterOriginalMotherName { get; }
        bool IsValidOriginalMotherName { get; }



        //int EstBirthInt{ get; }

        //int EstDeathInt{ get; }

        int EditorTotalEvents { get; }
        int EditorEventPriority { get; }

        //bool IsEstBirth{ get; }
        //bool IsEstDeath { get; }






        void SetFilterOriginalName(string param);
        void SetFilterOriginalFatherName(string param);
        void SetFilterOriginalMotherName(string param);


        // set editor values

        //void SetEditorBirthEstBirthInt(string param);
        //void SetEditorBirthEstDeathInt(string param);
        //void SetEditorBirthIsEstBirth(bool param);
        //void SetEditorBirthIsEstDeath(bool param);

         void SetEditorBirthLocationId(Guid param);
         void SetEditorDeathLocationId(Guid param);
         void SetEditorReferenceLocationId(Guid param);
         void SetEditorDateBirthString(string dBirth);
         void SetEditorDateDeathString(string dDeath);
         void SetEditorDateBapString(string dBap);

         void SetEditorChristianName(string cName);
         void SetEditorSurnameName(string sName);
         void SetEditorFatherChristianName(string fCName);
         void SetEditorFatherSurname(string fSName);
         void SetEditorMotherChristianName(string mCName);
         void SetEditorMotherSurname(string mSName);


         void SetEditorReferenceLocation(string paramReferenceLocation);
         void SetEditorReferenceDate(string paramReferenceDate);
         void SetEditorOccupation(string paramOccupation);
         void SetEditorIsMale(bool isMale);

         void SetEditorBirthLocation(string bLocation);
         void SetEditorDeathLocation(string dLocation);

         void SetEditorBirthCountyLocation(string bLocation);
         void SetEditorDeathCountyLocation(string dLocation);

         void SetEditorSource(string source);
         void SetEditorNotes(string notes);


         void SetEditorSpouseCName(string param);
         void SetEditorSpouseSName(string param);
         void SetEditorFatherOccupation(string param);

         void SetEditorUniqueRef(Guid param);

         void SetEditorTotalEvents(int param);
         void SetEditorEventPriority(int param);

 
        //void AddObserver(IDeathBirthEditorView paramView);
        //void RemoveObserver(IDeathBirthEditorView paramView);
        //void NotifyObservers();
    }

    public interface IDeathBirthEditorControl : IDBRecordControl<Guid>
    {
         void RequestSetEditorBirthLocationId(Guid param);
         void RequestSetEditorDeathLocationId(Guid param);
         void RequestSetEditorReferenceLocationId(Guid param);

         void RequestSetEditorReferenceLocation(string paramReferenceLocation);
         void RequestSetEditorReferenceDate(string paramReferenceDate);
         void RequestSetEditorOccupation(string paramOccupation);
         void RequestSetEditorDateBirthString(string paramBirth);
         void RequestSetEditorDateDeathString(string dDeath);
         void RequestSetEditorDateBapString(string dBap);

         void RequestSetEditorChristianName(string cName);
         void RequestSetEditorSurnameName(string sName);
         void RequestSetEditorFatherChristianName(string fCName);
         void RequestSetEditorFatherSurname(string fSName);
         void RequestSetEditorMotherChristianName(string mCName);
         void RequestSetEditorMotherSurname(string mSName);

         void RequestSetEditorSpouseCName(string param);
         void RequestSetEditorSpouseSName(string param);
         void RequestSetEditorFatherOccupation(string param);

         void RequestSetEditorIsMale(bool isMale);

         void RequestSetEditorBirthLocation(string bLocation);
         void RequestSetEditorDeathLocation(string dLocation);

         void RequestSetEditorBirthCountyLocation(string bLocation);
         void RequestSetEditorDeathCountyLocation(string dLocation);

         void RequestSetEditorSource(string source);
         void RequestSetEditorNotes(string notes);

         void RequestSetFilterOriginalName(string param);
       
         void RequestSetFilterOriginalFatherName(string param);

         void RequestSetFilterOriginalMotherName(string param);


        //void SetModel(IDeathBirthEditorModel paramModel);
        //void SetView(IDeathBirthEditorView paramView);

        // void SetModel(IDBRecordModel<Guid> paramModel);
        // void SetView(IDBRecordView paramView);

        //void SetView();
    }

    public interface IDeathBirthEditorView :IDBRecordView
    {

        void ShowValidDeathDate(bool valid);
        void ShowValidBirthDate(bool valid);
        void ShowValidName(bool valid);
        void ShowValidLocation(bool valid);
        void ShowValidDeathLocation(bool valid);
        void ShowValidReferenceLocation(bool valid);
        void ShowValidReferenceDate(bool valid);


        void ShowValidSpouseCName(bool valid);
        void ShowValidSpouseSName(bool valid);
        void ShowValidFatherOccupation(bool valid);
 
        void ShowValidOccupation(bool valid);
        //void ShowValidBirthDate(bool valid);
        //void ShowValidDeathDate(bool valid);
        void ShowValidBapDate(bool valid);
     
        void ShowValidSurname(bool valid);
        void ShowValidFatherChristianName(bool valid);
        void ShowValidFatherSurname(bool valid);
        void ShowValidMotherChristianName(bool valid);
        void ShowValidMotherSurname(bool valid);
        void ShowValidBirthCountyLocation(bool valid);
        void ShowValidDeathCountyLocation(bool valid);
    
 
        void ShowValidBirthLocationId(bool valid);
        void ShowValidDeathLocationId(bool valid);
        void ShowValidReferenceLocationId(bool valid);
        void ShowValidSource(bool valid);
        void ShowValidNotes(bool valid);
        void ShowValidUniqueRef(bool valid);
        void ShowValidOriginalName(bool valid);
        void ShowValidOriginalFatherName(bool valid);
        void ShowValidOriginalMotherName(bool valid);


        //void Update(IDeathBirthEditorModel paramModel);
    }







}
