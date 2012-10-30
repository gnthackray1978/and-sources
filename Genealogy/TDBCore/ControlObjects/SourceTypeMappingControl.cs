using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.ModelObjects;
using GedItter.Interfaces;

namespace GedItter.ControlObjects
{
    public class SourceTypeMappingControl : EditorBaseControl<int>, ISourceTypeMapSourceControl 
    {
        protected new ISourceTypeMapSourceModel Model = null;
        protected new ISourceTypeMapSourceView View = null;


        public SourceTypeMappingControl()
        {

        }

        public SourceTypeMappingControl(ISourceTypeMapSourceModel paramModel,
            ISourceTypeMapSourceView paramView)
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


        public void SetModel(Interfaces.IDBRecordModel<int> paramModel)
        {
            base.Model = (EditorBaseModel<int>)paramModel;
            this.Model = (ISourceTypeMapSourceModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (ISourceTypeMapSourceView)paramView;
        }

        //public void SetModel(ISourceTypeMapSourceModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<int>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(ISourceTypeMapSourceView paramView)
        //{
        //    this.View = paramView;
        //    base.View = (IDBRecordView)paramView;
        //}
 
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



        public void RequestSetSelectedSourceMapTypeId(int mapTypeId)
        {
            if (Model != null)
            {
                Model.SetSelectedSourceMapTypeId(mapTypeId);

                //if (View != null) SetView();
            }
        }
        public void RequestSetSelectedMappedSourceTypeId(int mapTypeId)
        {
            if (Model != null)
            {
                Model.SetSelectedMappedSourceTypeId(mapTypeId);

               // if (View != null) SetView();
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

        public void RequestSetMultipleSourceMapTypeIds(List<int> mapTypeId)
        {
            if (Model != null)
            {
                foreach (int _mapTypeId in mapTypeId)
                {
                    Model.AddDisconnectedSourceMapRow(_mapTypeId);
                }

                if (View != null) SetView();
            }
        }




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
