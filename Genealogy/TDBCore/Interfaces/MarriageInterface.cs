using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using TDBCore.Datasets;
using GedItter.Interfaces;
using TDBCore.Types;

namespace GedItter.MarriageRecords
{


    // i think there must be a more elegant way of doing this
    // but i cant really think of one.




    public interface IEditorUI
    {
        // interface is the form which contains the marriageEditorControl


        IMarriageEditorModel IMarriageEditorModel { get; }


        void SetEditorModal(IMarriageEditorModel iMarriageEditorModel);
   

        void Show();

    }


   


    public interface IMarriageEditorModel : IDBRecordModel<Guid>
    {
      //  DsMarriages.MarriagesDataTable MarriagesTable { get; }

        bool IsValidMarriageDate { get; }
        string EditorDateMarriageString { get; }

        bool IsValidMaleName { get; }
        string EditorMaleCName { get; }
        bool IsValidMaleSurname { get; }
        string EditorMaleSName { get; }

        bool IsValidFemaleName { get; }
        string EditorFemaleCName { get; }
        bool IsValidFemaleSurname { get; }
        string EditorFemaleSName { get; }


        bool IsValidLocation { get; }
        string EditorLocation { get; }

        bool IsValidMaleLocation { get; }
        string EditorMaleLocation { get; }

        bool IsValidFemaleLocation { get; }
        string EditorFemaleLocation { get; }

        bool IsValidMaleInfo { get; }
        string EditorMaleInfo { get; }

        bool IsValidFemaleInfo { get; }
        string EditorFemaleInfo { get; }

        bool IsValidMarriageCounty { get; }
        string EditorMarriageCounty { get; }

        bool IsValidSource { get; }
        string EditorSource { get; }



        bool IsValidWitnessSName1 { get; }
        string EditorWitness1 { get; }
        bool IsValidWitnessSName2 { get; }
        string EditorWitness2 { get; }
        bool IsValidWitnessSName3 { get; }
        string EditorWitness3 { get; }
        bool IsValidWitnessSName4 { get; }
        string EditorWitness4 { get; }

        bool IsValidWitnessCName1 { get; }
        string EditorWitnessCName1 { get; }
        bool IsValidWitnessCName2 { get; }
        string EditorWitnessCName2 { get; }
        bool IsValidWitnessCName3 { get; }
        string EditorWitnessCName3 { get; }
        bool IsValidWitnessCName4 { get; }
        string EditorWitnessCName4 { get; }


        bool IsValidMaleOccupation { get; }
        string EditorMaleOccupation { get; }
        bool IsValidFemaleOccupation { get; }
        string EditorFemaleOccupation { get; }

        bool IsValidFemaleBirthYear { get; }
        string EditorFemaleBirthYear { get; }
        bool IsValidMaleBirthYear { get; }
        string EditorMaleBirthYear { get; }


        bool IsValidOriginalName { get; }
        string EditorOrigMaleName { get; }

        bool IsValidOriginalFemaleName {get;}
        string EditorOrigFemaleName { get; }

       // bool IsValidOriginalName { get; }
      //  string EditorOrigMaleName { get; }



        // no validation (yet)
        Guid EditorMaleId { get; }
        Guid EditorFemaleId { get; }
        Guid EditorUniqueRef { get; }
        int EditorTotalEvents { get; }
        int EditorEventPriority { get; }
        bool EditorIsWidow { get; }
        bool EditorIsWidower { get; }
        bool EditorIsLicence { get; }
        bool EditorIsBanns { get; }
        Guid EditorMarriageLocationId { get; }
        Guid EditorMaleLocationId { get; }
        Guid EditorFemaleLocationId { get; }
  


        // set editor values


        void SetEditorMaleId(Guid param);
        void SetEditorFemaleId(Guid param);

       
        void SetEditorOrigFemaleName(string param);
        void SetEditorOrigMaleName(string param);


        void SetMarriageLocationId(Guid param);
        void SetMaleLocationId(Guid param);
        void SetFemaleLocationId(Guid param);

        void SetEditorMaleCName(string cName);
        void SetEditorMaleSName(string sName);

        void SetEditorFemaleCName(string cName);
        void SetEditorFemaleSName(string sName);

        void SetEditorUniqueRef ( Guid param);
        void SetEditorTotalEvents ( int param );
        void SetEditorEventPriority(int param);

        void SetEditorMarriageDate(string date);
        void SetEditorLocation(string location);
        void SetEditorMaleLocation(string maleLocation);
        void SetEditorFemaleLocation(string femaleLocation);
        void SetEditorMarriageCounty(string marriageCounty);
        void SetEditorSource(string source);

        void SetEditorWitness1(string witness1);
        void SetEditorWitness2(string witness2);
        void SetEditorWitness3(string witness3);
        void SetEditorWitness4(string witness4);

        void SetEditorWitness1CName(string witness1);
        void SetEditorWitness2CName(string witness2);
        void SetEditorWitness3CName(string witness3);
        void SetEditorWitness4CName(string witness4);

        void SetEditorFemaleInfo(string finfo);
        void SetEditorMaleInfo(string minfo);

        void SetEditorMaleOccupation(string paramOccupation);
        void SetEditorFemaleOccupation(string paramOccupation);
        void SetEditorIsWidow(bool paramIsWidow);
        void SetEditorIsWidower(bool paramIsWidower);
        void SetEditorIsLicence(bool paramIsLicence);
        void SetEditorIsBanns(bool paramBanns);

        void SetEditorFemaleBirthYear(string param);
        void SetEditorMaleBirthYear(string param);


        //void AddObserver(IMarriageEditorView paramView);
        //void RemoveObserver(IMarriageEditorView paramView);
        //void NotifyObservers();
    }

    public interface IMarriageEditorControl: IDBRecordControl<Guid>
    {


        void RequestSetEditorMarriageLocationId(Guid param);
        void RequestSetEditorMaleLocationId(Guid param);
        void RequestSetEditorFemaleLocationId(Guid param);
        void RequestSetEditorMaleOccupation(string paramOccupation);
        void RequestSetEditorFemaleOccupation(string paramOccupation);
        void RequestSetEditorIsWidow(bool paramIsWidow);
        void RequestSetEditorIsWidower(bool paramIsWidower);
        void RequestSetEditorIsLicence(bool paramIsLicence);
        void RequestSetEditorIsBanns(bool paramBanns);

        void RequestSetEditorMaleName(string cName, string sName);
        void RequestSetEditorFemaleName(string cName, string sName);
        void RequestSetEditorMarriageDate(string date);
        void RequestSetEditorLocation(string location);
        void RequestSetEditorMaleLocation(string maleLocation);
        void RequestSetEditorFemaleLocation(string femaleLocation);
        void RequestSetEditorMarriageCounty(string marriageCounty);
        void RequestSetEditorSource(string source);
        
        void RequestSetEditorWitness1(string witness);
        void RequestSetEditorWitness2(string witness);
        void RequestSetEditorWitness3(string witness);
        void RequestSetEditorWitness4(string witness);

        void RequestSetEditorWitness1CName(string witness);
        void RequestSetEditorWitness2CName(string witness);
        void RequestSetEditorWitness3CName(string witness);
        void RequestSetEditorWitness4CName(string witness);

        void RequestSetEditorMaleInfo(string minfo);
        void RequestSetEditorFemaleInfo(string finfo);


        void RequestSetEditorFemaleBirthYear(string param);
        void RequestSetEditorMaleBirthYear(string param);

        //void SetModel(IMarriageEditorModel paramModel);
        //void SetView(IMarriageEditorView paramView);
        //void SetView();
    }

    public interface IMarriageEditorView : IDBRecordView
    {

        void ShowInvalidMarriageDate(bool valid);

        void ShowInvalidMaleName(bool valid);
        void ShowInvalidMaleSurname(bool valid);

        void ShowInvalidFemaleName(bool valid);
        void ShowInvalidFemaleSurname(bool valid);

        void ShowInvalidLocation(bool valid);
        void ShowInvalidMaleLocation(bool valid);
        void ShowInvalidFemaleLocation(bool valid);
        void ShowInvalidMaleInfo(bool valid);
        void ShowInvalidFemaleInfo(bool valid);
        void ShowInvalidMarriageCounty(bool valid);
        void ShowInvalidSource(bool valid);
        void ShowInvalidWitnessSName1(bool valid);
        void ShowInvalidWitnessSName2(bool valid);
        void ShowInvalidWitnessSName3(bool valid);
        void ShowInvalidWitnessSName4(bool valid);
        void ShowInvalidWitnessCName1(bool valid);
        void ShowInvalidWitnessCName2(bool valid);
        void ShowInvalidWitnessCName3(bool valid);
        void ShowInvalidWitnessCName4(bool valid);

        void ShowInvalidMaleOccupation(bool valid);
        void ShowInvalidFemaleOccupation(bool valid);
        void ShowInvalidFemaleBirthYear(bool valid);
        void ShowInvalidMaleBirthYear(bool valid);


        void ShowInvalidOriginalName(bool valid);
        void ShowInvalidOriginalFemaleName(bool valid);
       

      //  void Update(IMarriageEditorModel paramModel);
    }

    public interface IMarriageFilterModel : IDBRecordsModel<Guid>
    {
    
        IList<MarriageResult> MarriagesTable { get; }


        bool IsValidMarriageUpperBound { get; }
        bool IsValidMarriageLowerBound { get; }

        IMarriageEditorModel IMarriageEditorModel { get; }
        IEditorUI IMarriageEditorUI { get; }


        string QueryString { get; }

        #region filters


        // view filter settings
        string FilterUpperMarriage { get; }
        string FilterLowerMarriage { get; }


        string FilterMaleName { get; }
        string FilterMaleCName { get; }
        string FilterMaleSName { get; }

        string FilterFemaleCName { get; }
        string FilterFemaleSName { get; }
        string FilterFemaleName { get; }
        string FilterLocationCounty { get; }
        string FilterLocation { get; }
        string FilterMaleLocation { get; }
        string FilterFemaleLocation { get; }
        string FilterMarriageSource { get; }
        string FilterDupeInterval { get; }
        string FilterWitness { get; }

        MarriageFilterTypes FilterMode {get;}

        string FilteredPrintableResults { get; }


        void SetFilteredPrintableResults(string param, bool isTabular);

        // set filters
        void ViewDuplicates();
        void SetFilterMode(MarriageFilterTypes param);//  
        void SetFilterWitness(string param);
        void SetFilterMaleName(string name);
        void SetFilterMaleName(string cName, string sName);
        void SetFilterFemaleName(string name);
        void SetFilterFemaleName(string cName, string sName);
        void SetFilterLocation(string location);
        void SetFilterMaleLocation(string maleLocation);
        void SetFilterFemaleLocation(string femaleLocation);
        void SetFilterSource(string source);
        void SetFilterLowerBound(string lowerYear);
        void SetFilterUpperBound(string upperYear);
        void SetFilterMarriageLocationCounty(string county);
        
        void SetShowDialogDupes(EventHandler paramEventHandler);
        void SetShowDialogRels(EventHandler paramEventHandler);

        void SetFilterDupeInterval(string interval);
        void SetRemoveSelectedFromDuplicateList();
   
        void SetSelectedDuplicateMarriage();
        void SetMergeSources();
        void SetReorderDupes();
        #endregion


        void SetEditorUI(IEditorUI paramIEditorUI);


    }

    public interface IMarriageFilterControl :IDBRecordsControl<Guid>
    {
        void RequestViewDuplicates();
        void RequestSetFilterMode(MarriageFilterTypes param);//  
        void RequestSetFilterMaleName(string name);
        void RequestSetFilterFemaleName(string name);
        void RequestSetFilterMaleName(string cName, string sName);
        void RequestSetFilterFemaleName(string cName, string sName);
        void RequestSetFilterLocation(string location);
        void RequestSetFilterLocationCounty(string county);
        void RequestSetFilterMaleLocation(string maleLocation);
        void RequestSetFilterFemaleLocation(string femaleLocation);
        void RequestSetFilterSource(string source);
        void RequestSetFilterMarriageBoundLower(string lowerb);
        void RequestSetFilterMarriageBoundUpper(string upperb);
        void RequestSetSelectedDuplicateMarriage();
        void RequestSetFilteredPrintableResults(string param, bool isTabular);
        void RequestSetFilterWitness(string param);

        void SetFilterDupeInterval(string interval);
        void RequestSetRemoveSelectedFromDuplicateList();
        void RequestSetMergeSources();
        void RequestSetReorderDupes();
        void RequestSetShowDialogDupes(EventHandler paramEventHandler);
        void RequestSetShowDialogRels(EventHandler paramEventHandler);


        //void SetModel(IMarriageFilterModel paramModel);
        //void SetView(iMarriageFilterView paramView);
        //void SetView();
    }

    public interface iMarriageFilterView : IDBRecordView
    {



        void SetParentIds(List<Guid> Ids);
         
        void SetFilterMode(MarriageFilterTypes param);//  
        void ShowInvalidUpperBoundMarriageWarning(bool valid);
        void ShowInvalidLowerBoundMarriageWarning(bool valid);
        event EventHandler ShowEditor;
     //   void Update(IMarriageFilterModel paramModel);



    }


    public interface IMarriageFilterWeb
    {
        //DsMarriages.MarriagesDataTable GetTable(string param);
        IList<MarriageResult> GetTable(string param, string SortExpression);
    }


}
