using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using GedItter.ModelObjects;

namespace GedItter.ControlObjects
{
    public class SourceMappingParishsControl : EditorBaseControl<Guid>, ISourceMappingParishsControl
    {


        protected new ISourceMappingParishsModel Model = null;
        protected new ISourceMappingParishsView View = null;


        public SourceMappingParishsControl()
        {

        }

        public SourceMappingParishsControl(ISourceMappingParishsModel paramModel,
            ISourceMappingParishsView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        }


        //public void SetModel(ISourceMappingParishsModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<Guid>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(ISourceMappingParishsView paramView)
        //{
        //    this.View = paramView;
        //    base.View = (IDBRecordView)paramView;
        //}

        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (ISourceMappingParishsModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (ISourceMappingParishsView)paramView;
        }


        #region ISourceMappingParishsControl Members

        public void RequestSetMultipleSelectedParishIds(List<Guid> paramGuids)
        {
            if (Model != null)
            {
                foreach (Guid _parishId in paramGuids)
                {
                    Model.AddDisconnectedSourceMapRow(_parishId);
                }

                if (View != null) SetView();
            }
        }

        public void RequestSetSelectedParishId(Guid paramGuid)
        {
            if (Model != null)
            {
                Model.SetSelectedParishId(paramGuid);
                if (View != null) SetView();
            }
        }

        public void RequestSetSelectedMappedParishId(Guid paramGuid)
        {
            if (Model != null)
            {
                Model.SetSelectedMappedParishId(paramGuid);
                if (View != null) SetView();
            }
        }

        public void RequestParishDescription(string param)
        {
            if (Model != null)
            {
                Model.SetParishDescription(param);
                Model.Refresh();
                if (View != null) SetView();
            }
        }

        public void RequestRemoveMapping()
        {
            if (Model != null)
            {
                Model.RemoveMapping();
                if (View != null) SetView();
            }
        }

        public void RequestAddMapping()
        {
            if (Model != null)
            {
                Model.AddMapping();
                if (View != null) SetView();
            }
        }

        public void RequestSetParent(Guid parentId)
        {
            if (Model != null)
            {
                Model.SetParent(parentId);
                if (View != null) SetView();
            }
        }

       

        #endregion





        public void RequestClearMappings()
        {
            if (Model != null)
            {
                Model.ClearMappings();
                if (View != null) SetView();
            }

        }
    }
}
