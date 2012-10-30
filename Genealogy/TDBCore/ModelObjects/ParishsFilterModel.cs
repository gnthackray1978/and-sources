using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using TDBCore.Types;
//using TDBCore.Datasets;
using GedItter.BLL;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Web;

namespace GedItter.ModelObjects
{
    public class ParishsFilterModel : EditorBaseModel<Guid>, IParishsFilterModel
    {


        ParishsEditorModel parishEditorModel = new ParishsEditorModel();

        IList<TDBCore.EntityModel.Parish> parishEntities = null;

   

        List<RectangleD> locations = new List<RectangleD>();



        string parishName = "";
        string parishDeposited = "";
        string parishCounty = "";

        string parishXStr = "";
        string parishYStr = "";
        string parishLenStr = "";
        string parishLocationStr = "";

        double parishXdouble = 0;
        double parishYdouble = 0;
        double parishLenDouble = 0;



        public override bool IsValidEntry
        {
            get
            {
                if ((parishName == "" && parishDeposited == "" && parishCounty == "")&&
                    (this.parishLenDouble == 0  || this.parishXdouble == 0 || this.parishYdouble ==0) && this.locations.Count ==0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public  ParishsFilterModel()
        {
            this.SetIsReadOnly(false);
        }

        public List<RectangleD> Locations
        {
            get
            {
                return this.locations;
            }
        }

  
        public IList<TDBCore.EntityModel.Parish> ParishEntities
        {
            get
            {
                return this.parishEntities;
            }
        }



        public string ParishDeposited
        {
            get
            {
                return this.parishDeposited;
            }

        }

        public string ParishCounty
        {
            get
            {
                return this.parishCounty;
            }
        }

        public string ParishName
        {
            get 
            {
                return this.parishName;
            }
        }

        public void SetParishDeposited(string param)
        {
            if (this.parishDeposited != param)
            {
                this.parishDeposited = param;
                this.isDataChanged = true;
                this.SetModelStatusFields();

            }
        }

        public void SetParishCounty(string param)
        {
            if (this.parishCounty != param)
            {
                this.parishCounty = param;
                this.isDataChanged = true;
                this.SetModelStatusFields();

            }
        }

        public void SetParishName(string param)
        {
            if (this.parishName != param)
            {
                this.parishName = param;
                this.isDataChanged = true;
                this.SetModelStatusFields();
               
            }
        }

        public void SetLocations(string param)
        {
            BLL.ParishsBLL parishsBll = new GedItter.BLL.ParishsBLL();

            if (this.parishLocationStr != param)
            {
                this.isDataChanged = true;
            }
            else
            {
                this.isDataChanged = false;
            }

            parishLocationStr = param;
            locations.AddRange(parishsBll.GetLocationList(param));
        }

 


        public void SetEditorUI()
        {
            throw new NotImplementedException();
        }



        public override void DeleteSelectedRecords()
        {
            if (!IsValidDelete()) return;

            BLL.ParishsBLL parishsBll = new GedItter.BLL.ParishsBLL();

            foreach (Guid parishIdx in this.SelectedRecordIds)
            {
                parishsBll.DeleteParishById(parishIdx);
            }

            this.SetSelectedRecordIds(new List<Guid>());

            this.isDataChanged = true;
            Refresh();
        }

        public override void EditSelectedRecord()
        {

            if (!IsvalidSelect()) return;

            parishEditorModel.SetSelectedRecordId(this.SelectedRecordId);


            this.isDataChanged = true;
            this.ShowDialogEdit(this);
        }

        public override void InsertNewRecord()
        {

            if (!IsValidInsert()) return;

            parishEditorModel = new ParishsEditorModel();
            //parishEditorModel.DataSaved += new EventHandler(parishEditorModel_DataSaved);

            base.SetDataSaved(new EventHandler(parishEditorModel_DataSaved));
            this.ShowDialogInsert(this);
        }

        public override void Refresh()
        {
            if (IsValidEntry && isDataChanged)
            {

                BLL.ParishsBLL parishsBll = new GedItter.BLL.ParishsBLL();

                if (this.parishXdouble == 0 && this.parishYdouble == 0 && this.parishLenDouble == 0)
                {
                    this.parishEntities = parishsBll.GetParishByFilter2(this.parishName, this.parishDeposited, this.parishCounty).ToList();

                }
                else
                {

                    double tpX = this.parishXdouble - (this.parishLenDouble / 2);
                    double tpY = this.parishYdouble - (this.parishLenDouble / 2);

                    this.parishEntities = parishsBll.GetParishsByLocationBox2(tpX, tpY, this.parishLenDouble).ToList();

                }

                isDataChanged = false;
            }    
            this.NotifyObservers<ParishsFilterModel>(this);
        }

        void parishEditorModel_DataSaved(object sender, EventArgs e)
        {
            this.Refresh();
        }
 

        public IParishEditorUI IParishEditorUI
        {
            get { throw new NotImplementedException(); }
        }

        public IParishsEditorModel IParishsEditorModel
        {
            get
            {
                return this.parishEditorModel;
            }
        }



        public string ParishBoxX
        {
            get 
            {
                return this.parishXdouble.ToString();
            }
        }

        public string ParishBoxY
        {
            get 
            {
                return this.parishYdouble.ToString();
            }
        }

        public string ParishBoxLen
        {
            get 
            {
                return this.parishLenDouble.ToString();
            }
        }

        public void SetParishBoxX(string param)
        {
            if (this.parishXStr != param)
            {
                this.parishXStr = param;
                this.isDataChanged = true;

                if (!double.TryParse(this.parishXStr, out this.parishXdouble))
                {
                    this.parishXdouble = 0;
                    
                }
            }
        }

        public void SetParishBoxY(string param)
        {
            if (this.parishYStr != param)
            {
                this.parishYStr = param;
                this.isDataChanged = true;
                if (!double.TryParse(this.parishYStr, out this.parishYdouble))
                {
                    this.parishYdouble = 0;
                    
                }
            }
        }

        public void SetParishBoxLen(string param)
        {
            if (this.parishLenStr != param)
            {
                this.parishLenStr = param;
                this.isDataChanged = true;

                if (!double.TryParse(this.parishLenStr, out this.parishLenDouble))
                {
                    this.parishLenDouble = 0;
                }
            }
        }


        public List<RectangleD> ParishAreaLocations
        {
            get 
            
            {
                return this.locations;
            }
        }



        public override void SetFromQueryString(string param)
        {

            Debug.WriteLine("filter model SetFromQueryString :" + param);

            NameValueCollection query = HttpUtility.ParseQueryString(param);

            query.ReadInErrorsAndSecurity(this);

            //if (query.AllKeys.Contains("error"))
            //{
            //    this.SetErrorState(query["error"] ?? "");
            //}

            this.SetSelectedRecordIds(new List<Guid>());
            int pageNo = Convert.ToInt32(query["p"] ?? "0");

            this.SetRecordStart(pageNo);

            this.SetParishDeposited(query["pardep"] ?? "");
            this.SetParishName(query["parnam"] ?? "");
            this.SetParishCounty(query["parcot"] ?? "");

        //    this.Refresh();
        }



        #region IParishsFilterModel Members

    

        #endregion




        public int ParishRecordCount
        {
            get 
            {
                if (parishEntities != null)
                    return this.ParishEntities.Count();
                else
                    return 0;
            }
        }
    }



}
