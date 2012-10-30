using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using GedItter.ModelObjects;

namespace GedItter.ControlObjects
{
    public class SourceTypeFilterControl : EditorBaseControl<int>, ISourceTypeFilterControl 
    {

        protected new ISourceTypeFilterModel Model = null;
        protected new ISourceTypeFilterView View = null;

        public SourceTypeFilterControl()
        {

        }

        public SourceTypeFilterControl(ISourceTypeFilterModel paramModel, ISourceTypeFilterView paramView)
        {
            base.Model = (EditorBaseModel<int>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        }

        public void RequestSetSourceTypesDescrip(string param)
        {
            if (Model != null)
            {
                Model.SetSourceTypesDescrip(param);

                if (View != null) SetView();
            }
        }

        //public void SetModel(ISourceTypeFilterModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<int>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(ISourceTypeFilterView paramView)
        //{
        //    this.View = paramView;
        //    base.View = (IDBRecordView)paramView;
        //}

        public void SetModel(Interfaces.IDBRecordModel<int> paramModel)
        {
            base.Model = (EditorBaseModel<int>)paramModel;
            this.Model = (ISourceTypeFilterModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (ISourceTypeFilterView)paramView;
        }

        public override void SetView()
        {
            base.SetView();
        }
    }
}
