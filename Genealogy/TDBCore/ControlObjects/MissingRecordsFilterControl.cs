using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using GedItter.ModelObjects;

namespace GedItter.ControlObjects
{
    public class MissingRecordsFilterControl : EditorBaseControl<int>, IMissingRecordsControl
    {

        protected new IMissingRecordsModel Model = null;
        protected new IMissingRecordsView View = null;

        public MissingRecordsFilterControl()
        {

        }

        public MissingRecordsFilterControl(IMissingRecordsModel paramModel, IMissingRecordsView paramView)
        {
            base.Model = (EditorBaseModel<int>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        }


 
        public void RequestSetParishName(string param)
        {
            if (Model != null)
            {
                Model.SetParishName(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetStartDate(string param)
        {
            if (Model != null)
            {
                Model.SetStartDate(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetEndDate(string param)
        {
            if (Model != null)
            {
                Model.SetEndDate(param);

                if (View != null) SetView();
            }
        }





        //public void SetModel(IMissingRecordsModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<int>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(IMissingRecordsView paramView)
        //{
        //    this.View = paramView;
        //    base.View = (IDBRecordView)paramView;
        //}




        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<int>)paramModel;
            this.Model = (IMissingRecordsModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (IMissingRecordsView)paramView;
        }

        public void RequestSetIncludeBaptisms(bool? param)
        {
            if (Model != null)
            {
                Model.SetIncludeBaptisms(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetIncludeBaptisms(bool paramBap, bool paramMar)
        {
            if (Model != null)
            {
                Model.SetIncludeBaptisms(paramBap,paramMar);

                if (View != null) SetView();
            }
        }



       


        public void RequestSetOriginLet(string param)
        {
            if (Model != null)
            {
                Model.SetOriginLet(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetOriginLong(string param)
        {
            if (Model != null)
            {
                Model.SetOriginLong(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetDistance(string param)
        {
            if (Model != null)
            {
                Model.SetDistance(param);

                if (View != null) SetView();
            }
        }



         


        public void RequestSetOriginParish(Guid param)
        {
            if (Model != null)
            {
                Model.SetOriginParish(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetOriginParish(List<Guid> param)
        {
            if (Model != null)
            {
                Model.SetOriginParish(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetParishDeposited(string param)
        {
            if (Model != null)
            {
                Model.SetParishDeposited(param);

                if (View != null) SetView();
            }
        }

        
    }
}
