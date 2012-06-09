using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using GedItter.ModelObjects;

namespace GedItter.ControlObjects
{
    public class ParishEditorControl : EditorBaseControl<Guid>, IParishsEditorControl
    {

        protected new IParishsEditorModel Model = null;
        protected new IParishsEditorView View = null;
 
        public ParishEditorControl()
        {

        }

        public ParishEditorControl(IParishsEditorModel paramModel, IParishsEditorView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        }



        #region IParishsEditorControl Members

        public void RequestSetParishName(string param)
        {
            if (Model != null)
            {
                Model.SetParishName(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetParishRegistersDeposited(string param)
        {
            if (Model != null)
            {
                Model.SetParishRegistersDeposited(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetParishRegisterNotes(string param)
        {
            if (Model != null)
            {
                Model.SetParishRegisterNotes(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetParishParent(string param)
        {
            if (Model != null)
            {
                Model.SetParishParent(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetParishStartYear(string param)
        {
            if (Model != null)
            {
                Model.SetParishStartYear(param);

                if (View != null) SetView();
            }
        }

        //public void SetModel(IParishsEditorModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<Guid>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(IParishsEditorView paramView)
        //{
        //    this.View = paramView;
        //    base.View = (IDBRecordView)paramView;
        //}



        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (IParishsEditorModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (IParishsEditorView)paramView;
        }



        public override void SetView()
        {
            if (this.View != null)
            {
                View.ShowInvalidParishName(Model.IsValidParishName);
                View.ShowInvalidStartYear(Model.IsValidStartDate);
                //View.DisableAddition(Model.IsDataInserted);
                //View.DisableUpdating(Model.IsDataUpdated);
            }
        }

        #endregion

       


        public void RequestSetParishRegistersCounty(string param)
        {
            if (Model != null)
            {
                Model.SetParishRegistersCounty(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetParishEndYear(string param)
        {
            if (Model != null)
            {
                Model.SetParishEndYear(param);

                if (View != null) SetView();
            }
        }


 

        public void RequestSetParishLong(string param)
        {
            if (Model != null)
            {
                Model.SetParishLong(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetParishLat(string param)
        {
            if (Model != null)
            {
                Model.SetParishLat(param);

                if (View != null) SetView();
            }
        }

      
    }
}
