using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.ModelObjects;
using GedItter.Interfaces;

namespace GedItter.ControlObjects
{
    public class FileMappingControl : EditorBaseControl<Guid>, IFileMapSourceControl
    {
        protected new IFileMapSourceModel Model = null;
        protected new IFileMapSourceView View = null;

        #region constructor

        public FileMappingControl()
        {

        }

        public FileMappingControl(IFileMapSourceModel paramModel,
            IFileMapSourceView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        }

        #endregion

        public void RequestSetSelectedSourceFileId(Guid mapFileId)
        {
            if (Model != null)
            {
                Model.SetSelectedFileId(mapFileId);

                if (View != null) SetView();
            }
        }

        public void RequestSetSelectedMappedFileId(Guid mapFileId)
        {
            if (Model != null)
            {
                Model.SetSelectedMappedFileId(mapFileId);

                if (View != null) SetView();
            }
        }

        public void RequestSetParent(Guid parentId)
        {
            if (Model != null)
            {
                Model.SetParentId(parentId);
               // Model.Refresh();
                if (View != null) SetView();
            }
        }

        public void RequestSetFileDescrip(string param)
        {
            if (Model != null)
            {
                Model.SetFileDescrip(param);

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
        
        //public void SetModel(IFileMapSourceModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<Guid>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(IFileMapSourceView paramView)
        //{
        //    this.View = paramView;
        //    base.View = (IDBRecordView)paramView;
        //}

        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (IFileMapSourceModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (IFileMapSourceView)paramView;
        }



        public void RequestSetFileName(string param)
        {
            if (Model != null)
            {
                Model.SetFileName(param);

                if (View != null) SetView();
            }
        }

        
        public void RequestAddNewFileWithMapping(string file, string thumbLocat)
        {
            if (Model != null)
            {
                Model.AddNewFileWithMapping(file, thumbLocat);

                if (View != null) SetView();
            }
        }


    
        public void RequestAddMultipleDisconnectedSourceMapRow(List<Guid> paramGuid)
        {

            if (Model != null)
            {
                foreach(Guid _guid in paramGuid)
                {
                    Model.AddDisconnectedSourceMapRow(_guid);
                }

                if (View != null) SetView();
            }

        }




        public void RequestSetThumbLocat(string param)
        {
            if (Model != null)
            {
                Model.SetFileThumbLocat(param);

                if (View != null) SetView();
            }
        }




        public void RequestAddNewFileWithMapping(string file)
        {
            if (Model != null)
            {
                Model.AddNewFileWithMapping(file);

                if (View != null) SetView();
            }
        }


        public void RequestSetFileSystemRoot(string param)
        {
            if (Model != null)
            {
                Model.SetFileSystemRoot(param);

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
