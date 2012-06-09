using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.ModelObjects;
using GedItter.Interfaces;

namespace GedItter.ControlObjects
{
    public class SourceMappingControl : EditorBaseControl<Guid>, ISourceMappingControl
    {

        protected new ISourceMappingModel Model = null;
        protected new ISourceMappingView View = null;

        public SourceMappingControl()
        {

        }

        public SourceMappingControl(ISourceMappingModel paramModel, ISourceMappingView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        
        }




        public void RequestSetSelectedSourceSourceId(Guid mapFileId)
        {
            if (Model != null)
            {
                Model.SetSelectedSourceId(mapFileId);

                if (View != null) SetView();
            }
        }

        public void RequestSetSelectedMappedSourceId(Guid mapFileId)
        {
            if (Model != null)
            {
                Model.SetSelectedMappedSourceId(mapFileId);

                if (View != null) SetView();
            }
        }

        //public void RequestSetParentPerson(Guid parentId)
        //{
        //    if (Model != null)
        //    {
        //        Model.SetParentPersonId(parentId);

        //        if (View != null) SetView();
        //    }
        //}

        //public void RequestSetParentMarriage(Guid parentId)
        //{
        //    if (Model != null)
        //    {
        //        Model.SetParentMarriageId(parentId);

        //        if (View != null) SetView();
        //    }
        //}

        #region ISourceMappingControl Members
        //public void RequestSetSourceDescrip(string param)
        //{
        //    if (Model != null)
        //    {
        //        Model.SetSourceDescrip(param);

        //        if (View != null) SetView();
        //    }
        //}

        public void RequestSetSourceRef(string param)
        {
            if (Model != null)
            {
                Model.SetSourceRef(param);

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

        //public void SetModel(ISourceMappingModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<Guid>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(ISourceMappingView paramView)
        //{
        //    this.View = paramView;
        //    base.View = (IDBRecordView)paramView;
        //}

        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (ISourceMappingModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (ISourceMappingView)paramView;
        }

        #endregion


        
        public override void SetView()
        {
       
   
            //View.DisableAddition(Model.IsDataInserted);
            //View.DisableUpdating(Model.IsDataUpdated);
        }


   
        // used in lots of places
        //public void RequestAddDisconnectedSourceMapRow(Guid paramGuid)
        //{
        //    if (Model != null)
        //    {
        //        Model.AddDisconnectedSourceMapRow(paramGuid);

        //        if (View != null) SetView();
        //    }
        //}
        // not used anywhere
        //public void RequestRemoveDisconnectedSourceMapRow(Guid paramGuid)
        //{
        //    if (Model != null)
        //    {
        //        Model.RemoveDisconnectedSourceMapRow(paramGuid);

        //        if (View != null) SetView();
        //    }
        //}

        public void RequestAddMultipleDisconnectedSourceMapRow(List<Guid> paramGuid)
        {
            if (Model != null)
            {


                foreach (Guid _param in paramGuid)
                {
                    Model.AddDisconnectedSourceMapRow(_param);
                }
                if (View != null) SetView();
            }
        }

      
    }
}
