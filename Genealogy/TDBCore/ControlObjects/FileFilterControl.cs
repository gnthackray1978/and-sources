using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.ModelObjects;
using GedItter.Interfaces;

namespace GedItter.ControlObjects
{
    public class FileFilterControl : EditorBaseControl<Guid>, IFileFilterControl
    {

        protected new IFileFilterModel Model = null;
        protected new IFileFilterView View = null;


        public FileFilterControl(IFileFilterModel paramModel, IFileFilterView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        }

        public FileFilterControl()
        { 
        
        }

       

        public void RequestSetFilterDateEditUpper(string param)
        {
            if (Model != null)
            {
                Model.SetFilterDateEditUpper(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterDateEditLower(string param)
        {
            if (Model != null)
            {
                Model.SetFilterDateEditLower(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterDateAddUpper(string param)
        {
            if (Model != null)
            {
                Model.SetFilterDateAddUpper(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterDateAddLower(string param)
        {
            if (Model != null)
            {
                Model.SetFilterDateAddLower(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterFilePath(string param)
        {
            if (Model != null)
            {
                Model.SetFilterFilePath(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetFilterFileDescrip(string param)
        {
            if (Model != null)
            {
                Model.SetFilterFileDescrip(param);

                if (View != null) SetView();
            }
        }

        //public void SetModel(IFileFilterModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<Guid>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(IFileFilterView paramView)
        //{
        //    this.View = paramView;
        //}

        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (IFileFilterModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (IFileFilterView)paramView;
        }

        public override void SetView()
        {
            View.ShowInvalidAddDateLowerBoundWarning(Model.IsValidEditDateLowerBound);
            View.ShowInvalidAddDateUpperBoundWarning(Model.IsValidEditDateUpperBound);
            View.ShowInvalidEditDateLowerBoundWarning(Model.IsValidEditDateLowerBound);
            View.ShowInvalidEditDateUpperBoundWarning(Model.IsValidEditDateUpperBound);


        }





        public void RequestSetFilterFileRootPath(string param)
        {
            if (Model != null)
            {
                Model.SetFilterFileRootPath(param);

                if (View != null) SetView();
            }
        }
    }
}
