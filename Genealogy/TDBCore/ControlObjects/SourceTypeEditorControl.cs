using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using GedItter.ModelObjects;

namespace GedItter.ControlObjects
{
    public class SourceTypeEditorControl : EditorBaseControl<int>, ISourceTypeEditorControl 
    {

        protected new ISourceTypeEditorModel Model = null;
        protected new ISourceTypeEditorView View = null;
        
        
        public SourceTypeEditorControl()
        {

        }

        public SourceTypeEditorControl(ISourceTypeEditorModel paramModel, ISourceTypeEditorView paramView)
        {
            base.Model = (EditorBaseModel<int>)paramModel;
            base.View = (IDBRecordView)paramView;
            this.Model = paramModel;
            this.View = paramView;
        
        }

        public void RequestSetSourceTypeOrder(string param)
        {
            if (Model != null)
            {
                Model.SetSourceTypeOrder(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetSourceTypeDesc(string sourceTypeDesc)
        {
            if (Model != null)
            {
                Model.SetSourceTypeDesc(sourceTypeDesc);

                if (View != null) SetView();
            }
        }

        public void SetModel(IDBRecordModel<int> paramModel)
        {
            base.Model = (EditorBaseModel<int>)paramModel;
            this.Model = (ISourceTypeEditorModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (ISourceTypeEditorView)paramView;
        }

        //public void SetModel(ISourceTypeEditorModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<int>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(ISourceTypeEditorView paramView)
        //{
        //    this.View = paramView;
        //    base.View = (IDBRecordView)paramView;
        //}

        public override void SetView()
        {
            //View.DisableAddition(Model.IsDataInserted);
            //View.DisableUpdating(Model.IsDataUpdated);
        }

        public override void RequestInsert()
        {
            base.RequestInsert();

            //if (Model.IsDataInserted)
            //    View.CloseView();
        }


      
    }
}
