using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using GedItter.ModelObjects;

namespace GedItter.ControlObjects
{
    public class ParishsFilterControl : EditorBaseControl<Guid>, IParishsFilterControl 
    {

        protected new IParishsFilterModel Model = null;
        protected new IParishsFilterView View = null;
 
        public ParishsFilterControl()
        {

        }

        public ParishsFilterControl(IParishsFilterModel paramModel, IParishsFilterView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        }

        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (IParishsFilterModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (IParishsFilterView)paramView;
        }


        public override void SetView()
        {
            base.SetView();
        }




        public void RequestSetParishName(string param)
        {
            if (Model != null)
            {
                Model.SetParishName(param);

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

        public void RequestSetParishCounty(string param)
        {
            if (Model != null)
            {
                Model.SetParishCounty(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetParishBoxX(string param)
        {
            if (Model != null)
            {
                Model.SetParishBoxX(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetParishBoxY(string param)
        {
            if (Model != null)
            {
                Model.SetParishBoxY(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetParishBoxLen(string param)
        {
            if (Model != null)
            {
                Model.SetParishBoxLen(param);

                if (View != null) SetView();
            }
        }


        public void RequestSetLocations(string param)
        {
            if (Model != null)
            {
                Model.SetLocations(param);

                if (View != null) SetView();
            }
        }
    }
}


//public void SetModel(IParishsFilterModel paramModel)
//{
//    base.Model = (EditorBaseModel<Guid>)paramModel;
//    this.Model = paramModel;
//}

//public void SetView(IParishsFilterView paramView)
//{
//    this.View = paramView;
//    base.View = (IDBRecordView)paramView;
//}