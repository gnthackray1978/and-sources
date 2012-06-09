using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.ModelObjects;
using TDBCore.Types;

namespace GedItter.Interfaces
{
    public interface ICombinedEventModel : IDBRecordsModel<Guid>
    {
        bool IsValidUpperDateBound { get; }
        bool IsValidLowerDateBound { get; }
        bool IsValidCName { get; }
        bool IsValidSName { get; }
        bool IsValidLocation { get; }
        bool IsValidCounty { get; }
        bool IsValidEventSelection { get; }
           
        // view filter settings
        string FilterUpperDate { get; }
        string FilterLowerDate { get; }
        string FilterCName { get; }
        string FilterSName { get; }
        string FilterLocationCounty { get; }
        string FilterLocation { get; }
        EventType FilterEventSelection { get; }
        IList<SearchEvent> SearchEvents { get; }


        // set filters
        void SetFilterCName(string param);
        void SetFilterSName(string param);
        void SetFilterLocationCounty(string param);
        void SetFilterLocation(string param);
        void SetFilterUpperDate(string param);
        void SetFilterLowerDate(string param);
        void SetFilterEventSelection(EventType param);       
    }

    public interface ICombinedEventControl : IDBRecordsControl<Guid>
    {
        void RequestSetFilterCName(string param);
        void RequestSetFilterSName(string param);
        void RequestSetFilterLocationCounty(string param);
        void RequestSetFilterLocation(string param);
        void RequestSetFilterUpperDate(string param);
        void RequestSetFilterLowerDate(string param);
        void RequestSetFilterEventSelection(EventType param); 


        //void SetModel(ICombinedEventModel paramModel);
        //void SetView(ICombinedEventView paramView);
        //void SetView();
    }

    public interface ICombinedEventView : IDBRecordView
    {
        void ShowInvalidUpperDateBoundWarning(bool valid);
        void ShowInvalidLowerDateBoundWarning(bool valid);
        void ShowInvalidCNameWarning(bool valid);
        void ShowInvalidSNameWarning(bool valid);
        void ShowInvalidLocationWarning(bool valid);
        void ShowInvalidCountyWarning(bool valid);
        void ShowInvalidEventSelectionWarning(bool valid);
      //  event EventHandler ShowEditor;
    }

    public interface ICombinedEventWeb
    {
        // DsDeathsBirths.PersonsDataTable GetTable(string param);
        IList<SearchEvent> GetTable(string param, string SortExpression);
    }
}
