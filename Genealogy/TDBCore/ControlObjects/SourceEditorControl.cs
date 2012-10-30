using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using GedItter.ModelObjects;

namespace GedItter.ControlObjects
{
    public class SourceEditorControl : EditorBaseControl<Guid>, ISourceEditorControl 
    {


        protected new ISourceEditorModel Model = null;
        protected new ISourceEditorView View = null;
        
        
        public SourceEditorControl()
        {

        }

        public SourceEditorControl(ISourceEditorModel paramModel, ISourceEditorView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            base.View = (IDBRecordView)paramView;
            this.Model = paramModel;
            this.View = paramView;
        
        }



        #region ISourceEditorControl Members

        public void RequestSetSourceDateStr(string param)
        {
            if (Model != null)
            {
                Model.SetSourceDateStr(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetSourceDateToStr(string param)
        {
            if (Model != null)
            {
                Model.SetSourceDateToStr(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetSourceDescription(string param)
        {
            if (Model != null)
            {
                Model.SetSourceDescription(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetSourceOriginalLocation(string param)
        {
            if (Model != null)
            {
                Model.SetSourceOriginalLocation(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetIsCopyHeld(bool? param)
        {
            if (Model != null)
            {
                Model.SetIsCopyHeld(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetIsViewed(bool? param)
        {
            if (Model != null)
            {
                Model.SetIsViewed(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetIsThackrayFound(bool? param)
        {
            if (Model != null)
            {
                Model.SetIsThackrayFound(param);

                if (View != null) SetView();
            }
        }

        //public void SetModel(ISourceEditorModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<Guid>)paramModel;
        //    this.Model = paramModel;
            
        //}

        //public void SetView(ISourceEditorView paramView)
        //{
        //    this.View = paramView;
        //    base.View = (IDBRecordView)paramView;
        //}


        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (ISourceEditorModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (ISourceEditorView)paramView;
        }



        public override void SetView()
        {
            View.ShowInvalidSourceDate(Model.IsValidSourceDate);
            View.ShowInvalidSourceDateTo(Model.IsValidSourceDateTo);
            View.ShowInvalidSourceDescription(Model.IsValidSourceDescription);
            View.ShowInvalidSourceOriginalLocation(Model.IsValidSourceOriginalLocation);
            View.ShowInvalidUser(Model.IsValidUser);

            //View.DisableAddition(Model.IsDataInserted);
            //View.DisableUpdating(Model.IsDataUpdated);
        }

        public override void RequestInsert()
        {
            base.RequestInsert();

            //if (Model.IsDataInserted)
            //    View.CloseView();
        }

        #endregion


        public void RequestSetSourceNotes(string param)
        {
            if (Model != null)
            {
                Model.SetSourceNotes(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetSourceFileCount(string param)
        {
            if (Model != null)
            {
                Model.SetSourceFileCount(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetSourceRef(string param)
        {
            if (Model != null)
            {
                Model.SetSourceRef(param);

                if (View != null) SetView();
            }
        }


       
        public void RequestSetSourceFileIds(List<Guid> param)
        {
            if (Model != null)
            {
                Model.SetSourceFileIds(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetSourceTypeIdList(List<int> param)
        {
            if (Model != null)
            {
                Model.SetSourceTypeIdList(param);

                if (View != null) SetView();
            }
        }


 

        public void RequestSetSourceParishs(List<Guid> param)
        {
            if (Model != null)
            {
                Model.SetSourceParishs(param);

                if (View != null) SetView();
            }
        }



       

        

       
    }
}
