using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.ModelObjects;
using GedItter.Interfaces;

namespace GedItter.ControlObjects
{
    public class FileEditorControl : EditorBaseControl<Guid>, IFileEditorControl 
    {
        protected new IFileEditorModel Model = null;
        protected new IFileEditorView View = null;


        public FileEditorControl(IFileEditorModel paramModel, IFileEditorView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        
        }

        public FileEditorControl()
        {

        }

        public void RequestSetFilePath(string filePath)
        {
            if (Model != null)
            {
                Model.SetFilePath(filePath);

                if (View != null) SetView();
            }
        }

        public void RequestSetDescription(string fileDescription)
        {
            if (Model != null)
            {
                Model.SetFileDescription(fileDescription);

                if (View != null) SetView();
            }
        }



        //public void SetModel(IFileEditorModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<Guid>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(IFileEditorView paramView)
        //{
        //    this.View = paramView;
        //}

        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (IFileEditorModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (IFileEditorView)paramView;
        }


        public override void SetView()
        {
            View.ShowValidFilePath(Model.IsValidFilePath);
            //View.DisableAddition(Model.IsDataInserted);
            //View.DisableUpdating(Model.IsDataUpdated);
        }

        public override void RequestInsert()
        {
            base.RequestInsert();

            //if (Model.IsDataInserted)
            //    View.CloseView();
        }


        public void RequestSetFileThumbLocat(string fileThumbLocat)
        {
            if (Model != null)
            {
                Model.SetFileThumbLocat(fileThumbLocat);

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
    }
}
