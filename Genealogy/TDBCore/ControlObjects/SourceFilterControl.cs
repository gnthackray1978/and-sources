using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.ModelObjects;
using GedItter.Interfaces;

namespace GedItter.ControlObjects
{
    public class SourceFilterControl : EditorBaseControl<Guid>, ISourceFilterControl 
    {

        protected new ISourceFilterModel Model = null;
        protected new ISourceFilterView View = null;

        public SourceFilterControl()
        {

        }

        public SourceFilterControl(ISourceFilterModel paramModel, ISourceFilterView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        
        }


        #region ISourceFilterControl Members
        
        
        public void RequestSetFilterSourceFileCount(string param, bool useParam)
        {
            if (Model != null)
            {
                Model.SetFilterSourceFileCount(param, useParam);

                if (View != null) SetView();
            }
        }


        public void RequestSetFilterSourceDescription(string param)
        {
            if (Model != null)
            {
                Model.SetFilterSourceDescription(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterSourceOriginalLocation(string param)
        {
            if (Model != null)
            {
                Model.SetFilterSourceOriginalLocation(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterIsCopyHeld(bool? param, bool? useParam)
        {
            if (Model != null)
            {
                Model.SetFilterIsCopyHeld(param, useParam);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterIsViewed(bool? param, bool? useParam)
        {
            if (Model != null)
            {
                Model.SetFilterIsViewed(param, useParam);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterIsThackrayFound(bool? param, bool? useParam)
        {
            if (Model != null)
            {
                Model.SetFilterIsThackrayFound(param, useParam);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterSourceTypeList(List<int> param)
        {
            if (Model != null)
            {
                Model.SetFilterSourceTypeList(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterSourceDateUpperBound(string param)
        {
            if (Model != null)
            {
                Model.SetFilterSourceDateUpperBound(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterSourceDateLowerBound(string param)
        {
            if (Model != null)
            {
                Model.SetFilterSourceDateLowerBound(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterSourceToDateUpperBound(string param)
        {
            if (Model != null)
            {
                Model.SetFilterSourceToDateUpperBound(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterSourceToDateLowerBound(string param)
        {
            if (Model != null)
            {
                Model.SetFilterSourceToDateLowerBound(param);

                if (View != null) SetView();
            }
        }

        #endregion

        //public void SetModel(ISourceFilterModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<Guid>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(ISourceFilterView paramView)
        //{
        //    this.View = paramView;
        //    base.View = (IDBRecordView)paramView;
        //}

        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (ISourceFilterModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (ISourceFilterView)paramView;
        }

        public override void SetView()
        {
            View.ShowInvalidSourceDateLowerBoundWarning(Model.IsValidSourceDateLowerBound);
            View.ShowInvalidSourceDateUpperBoundWarning(Model.IsValidSourceDateUpperBound);
            View.ShowInvalidSourceDescriptionWarning(Model.IsValidSourceDescription);
            View.ShowInvalidSourceToDateLowerBoundWarning(Model.IsValidSourceToDateLowerBound);
            View.ShowInvalidSourceToDateUpperBoundWarning(Model.IsValidSourceToDateUpperBound);

            //View.DisableAddition(Model.IsDataInserted);
            //View.DisableUpdating(Model.IsDataUpdated);
        }




        public void RequestSetFilterSourceRef(string param)
        {
            if (Model != null)
            {
                Model.SetFilterSourceRef(param);

                if (View != null) SetView();
            }
        }






        public void RequestSetFilteredPrintableResults(string param, bool isTabular)
        {
            if (Model != null)
            {
                Model.SetFilteredPrintableResults(param,isTabular);

                if (View != null) SetView();
            }
        }
    }
}
