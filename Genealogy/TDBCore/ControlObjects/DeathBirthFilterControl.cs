using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.ModelObjects;

namespace GedItter.BirthDeathRecords
{
    public class DeathBirthFilterControl : EditorBaseControl<Guid>, IDeathBirthFilterControl
    {
        protected new IDeathBirthFilterModel Model;
        protected new iDeathBirthFilterView View;




        public DeathBirthFilterControl(IDeathBirthFilterModel paramModel, iDeathBirthFilterView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        }
        public DeathBirthFilterControl()
        {

        }


        #region IDeathBirthFilterControl Members

        public void RequestSetFilterUpperBirth(string param)
        {
            if (Model != null)
            {
                Model.SetFilterUpperBirth(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterLowerBirth(string param)
        {
            if (Model != null)
            {
                Model.SetFilterLowerBirth(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterUpperDeath(string param)
        {
            if (Model != null)
            {
                Model.SetFilterUpperDeath(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterLowerDeath(string param)
        {
            if (Model != null)
            {
                Model.SetFilterLowerDeath(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterCName(string param)
        {
            if (Model != null)
            {
                Model.SetFilterCName(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterSName(string param)
        {
            if (Model != null)
            {
                Model.SetFilterSName(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterMotherCName(string param)
        {
            if (Model != null)
            {
                Model.SetFilterMotherCName(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterMotherSName(string param)
        {
            if (Model != null)
            {
                Model.SetFilterMotherSName(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterFatherCName(string param)
        {
            if (Model != null)
            {
                Model.SetFilterFatherCName(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterFatherSName(string param)
        {
            if (Model != null)
            {
                Model.SetFilterFatherSName(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterLocation(string param)
        {
            if (Model != null)
            {
                Model.SetFilterLocation(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterLocationCounty(string param)
        {
            if (Model != null)
            {
                Model.SetFilterCountyLocation(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterDeathLocation(string param)
        {
            if (Model != null)
            {
                Model.SetFilterDeathLocation(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterDeathLocationCounty(string param)
        {
            if (Model != null)
            {
                Model.SetFilterDeathCountyLocation(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterSource(string param)
        {
            if (Model != null)
            {
                Model.SetFilterSource(param);

                if (View != null) SetView();
            }
        }


        #endregion



        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (IDeathBirthFilterModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (iDeathBirthFilterView)paramView;
        }

        public override void SetView()
        {
            View.ShowInvalidLowerBoundBirthWarning(Model.IsValidBirthLowerBound);
            View.ShowInvalidUpperBoundBirthWarning(Model.IsValidBirthUpperBound);
            View.ShowInvalidLowerBoundDeathWarning(Model.IsValidDeathLowerBound);
            View.ShowInvalidUpperBoundDeathWarning(Model.IsValidDeathUpperBound);


        }
 


        #region requests

        public void RequestSetFilterSpouseCName(string param)
        {
            if (Model != null)
            {
                Model.SetFilterSpouseCName(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterSpouseSName(string param)
        {
            if (Model != null)
            {
                Model.SetFilterSpouseSName(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterFatherOccupation(string param)
        {
            if (Model != null)
            {
                Model.SetFilterFatherOccupation(param);

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

        public void RequestViewRelations()
        {
            if (Model != null)
            {

                Model.ViewRelations();
                if (View != null) SetView();
            }
        }

        public void RequestDeleteRelation()
        {
            if (Model != null)
            {
                Model.DeleteRelation();

                if (View != null) SetView();
            }
        }

       

        public void RequestSetFilterMode(DeathBirthFilterTypes param)
        {
            if (Model != null)
            {

                Model.SetFilterMode(param);
                if (View != null) SetView();
            }
        }

        public void RequestSetRelation(List<int> typeId)
        {
            if (Model != null)
            {

                Model.SetRelation(typeId);
                if (View != null) SetView();
            }
        }

        public void RequestSetRelation(int typeId)
        {
            if (Model != null)
            {

                Model.SetRelation(typeId);
                if (View != null) SetView();
            }
        }

        public void RequestSetRelationTypeId(int relationTypeId)
        {
            if (Model != null)
            {
                Model.SetRelationTypeId(relationTypeId);
                
          
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




        public void RequestRemoveRelationType()
        {
            if (Model != null)
            {
                switch (Model.FilterMode)
                { 
                    case DeathBirthFilterTypes.DUPLICATES:
                        Model.RemoveRelationType();
                        break;
                    default:
                        Model.RemoveAllRelationType();
                        break;
                }
                
                if (View != null) SetView();
            }
        }


        public void RequestUpdateDateEstimates()
        {
            if (Model != null)
            {
                Model.UpdateDateEstimates();
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


        public void RequestSetFilterTreeResults(bool param)
        {
            if (Model != null)
            {
                Model.SetFilterTreeResults(param);
                if (View != null) SetView();
            }
        }


        public void RequestSetFilterIsIncludeDeaths(bool param)
        {
            if (Model != null)
            {
                Model.SetFilterIsIncludeDeaths(param);
                if (View != null) SetView();
            }
        }

        public void RequestSetFilterIsIncludeBirths(bool param)
        {
            if (Model != null)
            {
                Model.SetFilterIsIncludeBirths(param);
                if (View != null) SetView();
            }
        }


        public void RequestMergeDuplicates()
        {
            if (Model != null)
            {
                Model.MergeDuplicates();
                if (View != null) SetView();
            }
        }

        public void RequestUpdateEstimates()
        {
            if (Model != null)
            {
                Model.UpdateDateEstimates();
                if (View != null) SetView();
            }
        }
       
         
        #endregion



        public void RequestSetDefaultPersonForTree(Guid param)
        {
            if (Model != null)
            {
                Model.SetDefaultPersonForTree(param);
                if (View != null) SetView();
            }
        }
    }
}
