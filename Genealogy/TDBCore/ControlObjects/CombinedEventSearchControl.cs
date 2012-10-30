using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.ModelObjects;
using GedItter.Interfaces;
using TDBCore.Types;

namespace GedItter.ControlObjects
{
    public class CombinedEventSearchControl : EditorBaseControl<Guid>, ICombinedEventControl
    {

        protected new ICombinedEventModel Model;
        protected new ICombinedEventView View;

        

        #region constructors
        public CombinedEventSearchControl(ICombinedEventModel paramModel, ICombinedEventView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;

         
        }
        public CombinedEventSearchControl()
        {
           

        }
        #endregion


        #region ICombinedEventControl Members

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

        public void RequestSetFilterLocationCounty(string param)
        {
            if (Model != null)
            {
                Model.SetFilterLocationCounty(param);

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

        public void RequestSetFilterUpperDate(string param)
        {
            if (Model != null)
            {
                Model.SetFilterUpperDate(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterLowerDate(string param)
        {
            if (Model != null)
            {
                Model.SetFilterLowerDate(param);

                if (View != null) SetView();
            }
        }
        #endregion



        //public void SetModel(ICombinedEventModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<Guid>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(ICombinedEventView paramView)
        //{
        //    this.View = paramView;
        //}

        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (ICombinedEventModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (ICombinedEventView)paramView;
        }



        public override void SetView()
        {

            View.ShowInvalidCNameWarning(Model.IsValidLowerDateBound);
            View.ShowInvalidCountyWarning(Model.IsValidCounty);
            View.ShowInvalidEventSelectionWarning(Model.IsValidEventSelection);
            View.ShowInvalidLocationWarning(Model.IsValidLocation);
            View.ShowInvalidLowerDateBoundWarning(Model.IsValidLowerDateBound);
            View.ShowInvalidSNameWarning(Model.IsValidSName);
            View.ShowInvalidUpperDateBoundWarning(Model.IsValidUpperDateBound);
            

        }

       

        
   


        public void RequestSetFilterEventSelection(TDBCore.Types.EventType param)
        {
            if (Model != null)
            {
                Model.SetFilterEventSelection(param);

                if (View != null) SetView();
            }
        }
    }
}
