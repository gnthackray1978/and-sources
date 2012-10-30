using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using TDBCore.Datasets;
using TDBCore.Types;
using System.Drawing;

namespace GedItter.Interfaces
{

    public interface IParishEditorUI
    {
        // interface is the form which contains the marriageEditorControl


        IParishsEditorModel IParishsEditorModel { get; }


        void SetEditorModal(IParishsEditorModel iParishsEditorModel);


        void Show();

    }

    #region iparish filter

    public interface IParishsFilterModel : IDBRecordsModel<Guid>
    {
  
        IList<TDBCore.EntityModel.Parish> ParishEntities { get; }

        int ParishRecordCount { get; }
    

        IParishsEditorModel IParishsEditorModel { get; }
        IParishEditorUI IParishEditorUI { get; }

        string ParishBoxX { get; }
        string ParishBoxY { get; }
        string ParishBoxLen { get; }

        string ParishDeposited { get; }
        string ParishCounty { get; }

        string ParishName { get; }
        List<RectangleD> ParishAreaLocations { get; }

        void SetParishBoxX(string param);
        void SetParishBoxY(string param);
        void SetParishBoxLen(string param);
        void SetLocations(string param);
        void SetParishName(string param);
        void SetParishDeposited(string param);
        void SetParishCounty(string param);

    }

    public interface IParishsFilterControl : IDBRecordsControl<Guid>
    {
        void RequestSetParishBoxX(string param);
        void RequestSetParishBoxY(string param);
        void RequestSetParishBoxLen(string param);

        void RequestSetParishName(string param);
        void RequestSetParishDeposited(string param);
        void RequestSetParishCounty(string param);

        void RequestSetLocations(string param);

    }

    public interface IParishsFilterView : IDBRecordView
    {

        List<Guid> SelectedRecordIds { get; }

        event EventHandler ShowEditor;

        void RefreshLists();

    }

    public interface IParishsFilter
    {
        IList<TDBCore.EntityModel.Parish> GetTable(string param);


       // IMarriageEditorModel IMarriageEditorModel { get; }
    }
    #endregion


    public interface IParishsEditorModel : IDBRecordsModel<Guid>
    {
        //DsParishs.ParishsDataTable ParishDataTable { get; }
      //  IList<TDBCore.EntityModel.Parish> ParishList { get; }


        TDBCore.EntityModel.Parish ParishEntity { get; }

        bool IsValidParishName { get; }
        bool IsValidStartDate { get; }
        bool IsValidEndDate { get; }

        bool IsValidParishCounty { get; }
        bool IsValidRegistersDeposited { get; }

        bool IsValidParishLong { get; }
        bool IsValidParishLat { get; }


        string ParishName { get; }
        string ParishRegistersDeposited { get; }
        string ParishRegistersCounty {get;}
        string ParishRegisterNotes { get; }
        string ParishParent { get; }
        string ParishStartYear { get; }
        string ParishEndYear { get; }
        string ParishLong { get; }
        string ParishLat { get; }


        void SetParishName(string param);
        void SetParishRegistersDeposited(string param);
        void SetParishRegistersCounty(string param);
        void SetParishRegisterNotes(string param);
        void SetParishParent(string param);
        void SetParishStartYear(string param);
        void SetParishEndYear(string param);
        void SetParishLong(string param);
        void SetParishLat(string param);


        //void AddObserver(IParishsEditorView paramView);
        //void RemoveObserver(IParishsEditorView paramView);
        //void NotifyObservers();
    }


    public interface IParishsEditorControl : IDBRecordsControl<Guid>
    {
        void RequestSetParishName(string param);

        void RequestSetParishRegistersDeposited(string param);
        void RequestSetParishRegistersCounty(string param);
        void RequestSetParishRegisterNotes(string param);
        void RequestSetParishParent(string param);
        void RequestSetParishStartYear(string param);
        void RequestSetParishEndYear(string param);
        void RequestSetParishLong(string param);
        void RequestSetParishLat(string param);


        //void SetModel(IParishsEditorModel paramModel);
        //void SetView(IParishsEditorView paramView);
        //void SetView();
    }

    public interface IParishsEditorView : IDBRecordView
    {

        void ShowInvalidParishName(bool valid);
        void ShowInvalidParishCounty(bool valid);
        void ShowInvalidRegistersDeposited(bool valid);

        void ShowInvalidStartYear(bool valid);
        void ShowInvalidEndYear(bool valid);


        void ShowInvalidParishLong(bool valid);
        void ShowInvalidParishLat(bool valid );

     //   void Update(IParishsEditorModel paramModel);
    }

}
