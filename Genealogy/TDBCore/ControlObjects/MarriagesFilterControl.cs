using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.ModelObjects;
using GedItter.Interfaces;

namespace GedItter.MarriageRecords
{
    public class MarriagesFilterControl : EditorBaseControl<Guid>, IMarriageFilterControl
    {

        protected new IMarriageFilterModel Model;
        protected new iMarriageFilterView View;

        #region constructors
        public MarriagesFilterControl(IMarriageFilterModel paramModel, iMarriageFilterView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;

            //Model.SetType(individualType);
        }
        public MarriagesFilterControl()
        {
           // Model.SetType(individualType);

        }
        #endregion



        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (IMarriageFilterModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (iMarriageFilterView)paramView;
        }

        public override void SetView()
        {

            View.ShowInvalidLowerBoundMarriageWarning(Model.IsValidMarriageLowerBound);
            View.ShowInvalidUpperBoundMarriageWarning(Model.IsValidMarriageUpperBound);

        }

        #region  requests



        public void RequestSetFilterMaleName(string cName, string sName)
        {
            if (Model != null)
            {
                Model.SetFilterMaleName(cName, sName);//(resDate, resLocation);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterFemaleName(string cName, string sName)
        {
            if (Model != null)
            {
                Model.SetFilterFemaleName(cName, sName);//(resDate, resLocation);

                if (View != null) SetView();
            }
        }
        
        public void RequestSetFilterMaleName(string name)
        {
            if (Model != null)
            {
                Model.SetFilterMaleName(name);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterFemaleName(string name)
        {
            if (Model != null)
            {
                Model.SetFilterFemaleName(name);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterLocation(string location)
        {
            if (Model != null)
            {
                Model.SetFilterLocation(location);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterMaleLocation(string maleLocation)
        {
            if (Model != null)
            {
                Model.SetFilterMaleLocation(maleLocation);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterFemaleLocation(string femaleLocation)
        {
            if (Model != null)
            {
                Model.SetFilterFemaleLocation(femaleLocation);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterSource(string source)
        {
            if (Model != null)
            {
                Model.SetFilterSource(source);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterMarriageBoundLower(string lowerb)
        {
            if (Model != null)
            {
                Model.SetFilterLowerBound(lowerb);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterMarriageBoundUpper(string upperb)
        {
            if (Model != null)
            {
                Model.SetFilterUpperBound(upperb);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterLocationCounty(string county)
        {
            if (Model != null)
            {
                Model.SetFilterMarriageLocationCounty(county);

                if (View != null) SetView();
            }
        }

        public void RequestSetSelectedDuplicateMarriage()
        {
            if (Model != null)
            {
                Model.SetSelectedDuplicateMarriage();
               
                if (View != null) SetView();
            }
        }


        public void SetFilterDupeInterval(string interval)
        {
            if (Model != null)
            {
                Model.SetFilterDupeInterval(interval);

                if (View != null) SetView();
            }
        }

        public void RequestSetRemoveSelectedFromDuplicateList()
        {
            if (Model != null)
            {
                Model.SetRemoveSelectedFromDuplicateList();

                if (View != null) SetView();
            }
        }




       

        public void RequestSetFilterMode(MarriageFilterTypes param)
        {
            if (Model != null)
            {
                Model.SetFilterMode(param);

                if (View != null) SetView();
            }
        }



       

        public void RequestViewDuplicates()
        {
            if (Model != null)
            {
                Model.ViewDuplicates();

                if (View != null) SetView();
            }
        }

        public void RequestSetShowDialogDupes(EventHandler paramEventHandler)
        {
            if (Model != null)
            {
                Model.SetShowDialogDupes(paramEventHandler);

                if (View != null) SetView();
            }
        }

        public void RequestSetShowDialogRels(EventHandler paramEventHandler)
        {
             if (Model != null)
            {
                Model.SetShowDialogRels(paramEventHandler);

                if (View != null) SetView();
            }
        }




        public void RequestSetFilteredPrintableResults(string param, bool isTabular)
        {
            if (Model != null)
            {
                Model.SetFilteredPrintableResults(param, isTabular);

                if (View != null) SetView();
            }
        }


        public void RequestSetFilterWitness(string param)
        {
            if (Model != null)
            {
                Model.SetFilterWitness(param);

                if (View != null) SetView();
            }
        }


        #endregion


        public void RequestSetMergeSources()
        {
            if (Model != null)
            {
                Model.SetMergeSources();

                if (View != null) SetView();
            }
        }
    }
}
