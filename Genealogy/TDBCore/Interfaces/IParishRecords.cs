using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
////using TDBCore.Datasets;
using TDBCore.Types;

namespace GedItter.Interfaces
{
    public interface IParishRecordsModel
    {

        List<ParishDataType> ParishDataTypes { get; }
        List<ParishRecord> ParishRecords { get; }
        List<SilverParish> Parishs { get; }


        bool IsValidStartYear { get; }
        bool IsValidEndYear { get; }
     


 

        string StartDate { get; }
        string EndDate { get; }
        string ParishName { get; }
        string ParishDeposited { get; }
        string ParishCounty { get; }

        double OriginDistance { get; }
        double OriginLat { get; }
        double OriginLong { get; }



        bool Baptisms { get; }
        bool Marriages { get; }
        bool Deaths { get; }
  

        void SetBaptisms(bool param);
        void SetMarriages(bool param);
        void SetDeaths(bool param);



        void SetParishCounty(string param);
        void SetParishDeposited(string param);        
        void SetParishName(string param);
        void SetStartDate(string param);
        void SetEndDate(string param);



        // filters to look up by distance 
        void SetOriginDistance(string param);
        void SetOriginLat(string param);
        void SetOriginLong(string param);

        void Refresh();
        void NotifyObservers();
        void AddObserver(IParishRecordsView paramView);
        void RemoveAllObservers();
        void RemoveObserver(IParishRecordsView paramView);

    }



    public interface IParishRecordsControl 
    {


        void RequestSetBaptisms(bool param);
        void RequestSetMarriages(bool param);
        void RequestSetDeaths(bool param);
        void RequestSetParishCounty(string param);
        void RequestSetParishDeposited(string param);
        void RequestSetParishName(string param);
        void RequestSetStartDate(string param);
        void RequestSetEndDate(string param);
        void RequestSetOriginDistance(string param);
        void RequestSetOriginLat(string param);
        void RequestSetOriginLong(string param);

        void RequestRefresh();
        


        //void SetModel(IParishRecordsModel paramModel);
        //void SetView(IParishRecordsView paramView);
        //void SetView();
    }

    public interface IParishRecordsView 
    {
        void ShowInvalidStartDateWarning(bool valid);
        void ShowInvalidEndDateWarning(bool valid);

    }
}
