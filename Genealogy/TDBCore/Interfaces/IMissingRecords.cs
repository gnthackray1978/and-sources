using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
////using TDBCore.Datasets;
using TDBCore.EntityModel;

namespace GedItter.Interfaces
{
    public interface IMissingRecordsModel : IDBRecordsModel<int>
    {
        //DsSourceTypes.SourceTypesDataTable SourceTypesDataTable { get; }
       // DsMissingRecords.MissingRecordsDataTable MissingRecordsDataTable { get; }
        IList<MissingRecord> MissingRecordsList { get; }

        bool IsValidStartYear { get; }
        bool IsValidEndYear { get; }
        bool IsValidDistance { get; }
        bool IsValidOriginLong { get; }
        bool IsValidOriginLat { get; }

        double OriginLat { get; }
        double OriginLong { get; }
        double Distance { get; }
        Guid OriginParishId { get; }


        string StartDate { get; }
        string EndDate { get; }
        string ParishName { get; }
        string ParishDeposited { get; }
        bool? IncludeBaptisms { get; }



        void SetIncludeBaptisms(bool paramBap, bool paramMar);
        void SetIncludeBaptisms(bool? param);
        void SetParishName(string param);
        void SetStartDate(string param);
        void SetEndDate(string param);
        void SetOriginParish(Guid param);
        void SetOriginParish(List<Guid> param);
        void SetOriginLet(string param);
        void SetOriginLong(string param);
        void SetDistance(string param);
        void SetParishDeposited(string param);

        // set parent source id
        // add sourceType to parent source
        // remove sourcetype from parent


        //void AddObserver(IMissingRecordsView paramView);
        //void RemoveObserver(IMissingRecordsView paramView);
        //void NotifyObservers();

    }



    public interface IMissingRecordsControl : IDBRecordsControl<int>
    {
        void RequestSetParishName(string param);
        void RequestSetStartDate(string param);
        void RequestSetEndDate(string param);
        void RequestSetIncludeBaptisms(bool? param);
        void RequestSetIncludeBaptisms(bool paramBap, bool paramMar);

        void RequestSetOriginParish(Guid param);
        void RequestSetOriginParish(List<Guid> param);
        void RequestSetOriginLet(string param);
        void RequestSetOriginLong(string param);

        void RequestSetDistance(string param);
        void RequestSetParishDeposited(string param);


        //void SetModel(IMissingRecordsModel paramModel);
        //void SetView(IMissingRecordsView paramView);
        //void SetView();
    }

    public interface IMissingRecordsView : IDBRecordView
    {
        void ShowInvalidStartDateWarning(bool valid);
        void ShowInvalidEndDateWarning(bool valid);



      //  void Update(IMissingRecordsModel paramModel);


        //string SourceIdsFound { get; }
        //List<Guid> SelectedRecordIds { get; }

        //event EventHandler ShowEditor;

        //void RefreshLists();
    }
}
