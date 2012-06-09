using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
////using TDBCore.Datasets;

using GedItter.BLL;
using TDBCore.EntityModel;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Web;
using TDBCore.Types;

namespace GedItter.ModelObjects
{
    public class ParishsEditorModel : EditorBaseModel<Guid>, IParishsEditorModel
    {

  //      DsParishs.ParishsDataTable parishDataTable = new DsParishs.ParishsDataTable();

        Parish parishEntity = new Parish();

        bool isValidParishName = false;
        bool isValidParishCounty = false;
        bool isValidRegsDeposited = false;
        bool isValidStartDate = false;
        bool isValidEndDate = false;
        bool isValidParishLong = false;
        bool isValidParishLat = false;
       

        string parishName = "";
        string parishRegisterNotes = "";
        string parishRegistersDeposited = "";
        string parishRegistersCounty = "";
        string parishParent = "";
        string parishStartYearStr = "";
        int parishStartYearInt = 0;
        int parishEndYearInt = 0;
        string parishEndYearStr = "";

        double parishLongDub = 0;
        string parishLongStr = "";
        double parishLatDub = 0;
        string parishLatStr = "";


        #region validation

        public override bool IsValidEntry
        {
            get
            {
                return isValidParishName || isValidStartDate || isValidParishLat || isValidParishLong || isValidEndDate || isValidRegsDeposited || isValidParishCounty;
            }
        }


        public bool IsValidEndDate
        {
            get
            {
                return this.isValidEndDate;
            }
        }




        public bool IsValidParishCounty
        {
            get
            {
                return this.isValidParishCounty;
            }
        }

        public bool IsValidRegistersDeposited
        {
            get
            {
                return this.isValidRegsDeposited;
            }
        }

        public bool IsValidParishLong
        {
            get
            {
                return this.isValidParishLong;
            }
        }

        public bool IsValidParishLat
        {
            get
            {
                return this.isValidParishLat;
            }
        }

        public bool IsValidParishName
        {
            get
            {
                return this.isValidParishName;
            }
        }

        public bool IsValidStartDate
        {
            get
            {
                return this.isValidStartDate;
            }
        }


        #endregion

        #region IParishsEditorModel Members


        #region read only properties
        //public TDBCore.Datasets.DsParishs.ParishsDataTable ParishDataTable
        //{
        //    get 
        //    {
        //        return this.parishDataTable;
        //    }
        //}

        public Parish ParishEntity
        {
            get
            {
                return this.parishEntity;
            }
        }

        public string ParishName
        {
            get
            {
                return this.parishName;
            }
        }

        public string ParishRegistersDeposited
        {
            get
            {
                return this.parishRegistersDeposited;
            }
        }

        public string ParishRegisterNotes
        {
            get
            {
                return this.parishRegisterNotes;
            }
        }

        public string ParishParent
        {
            get 
            {
                return this.parishParent;
            }
        }

        public string ParishStartYear
        {
            get
            {
                return this.parishStartYearStr;
            }
        }

        
        public string ParishEndYear
        {
            get
            {
                return this.parishEndYearStr;
            }
        }

        public string ParishRegistersCounty
        {
            get
            {
                return this.parishRegistersCounty;
            }
        }

        public string ParishLong
        {
            get
            {
                return this.parishLongStr;
            }
        }

        public string ParishLat
        {
            get
            {
                return this.parishLatStr;
            }
        }

        #endregion
        
        //500
        public void SetParishName(string param)
        {
            
            if (this.parishName != param)
            {
                this.parishName = param;

                if (param.Length >= 3 && param.Length <= 500)
                {
                    this.isValidParishName = true;
                }
                else
                {
                    this.isValidParishName = false;
                    this.SetErrorState("parish name must be longer than zero characters and shorter than 500");
                }



                this.SetModelStatusFields();

            }
        }
        //50
        public void SetParishRegistersCounty(string param)
        {
            if (this.parishRegistersCounty != param)
            {
                if (param.Length > 3 && param.Length <= 50)
                {
                    this.isValidParishCounty = true;
                }
                else
                {
                    this.isValidParishCounty = false;
                    this.SetErrorState("parish county must be longer than zero characters and shorter than 50");
                }
                this.parishRegistersCounty = param;
                this.SetModelStatusFields();
            }
          

        }
        //100
        public void SetParishRegistersDeposited(string param)
        {
            if (this.parishRegistersDeposited != param)
            {
                if (param.Length > 2 && param.Length <= 100)
                {
                    this.isValidRegsDeposited = true;
                }
                else
                {
                    this.SetErrorState("parish county must be longer than zero characters and shorter than 100");
                    this.isValidRegsDeposited = false;
                }

                this.parishRegistersDeposited = param;
                this.SetModelStatusFields();

            }
        }

        public void SetParishRegisterNotes(string param)
        {
            if (this.parishRegisterNotes != param)
            {
                this.parishRegisterNotes = param;
                this.SetModelStatusFields();
            }
        }

        public void SetParishParent(string param)
        {
            if (this.parishParent != param)
            {
                this.parishParent = param;
                this.SetModelStatusFields();
            }
        }

        public void SetParishStartYear(string param)
        {

            if (this.parishStartYearStr != param)
            {
                this.parishStartYearStr = param;

                

                if (CsUtils.ValidYear(this.parishStartYearStr,out this.parishStartYearInt) )
                {
                    this.isValidStartDate = true;
                }
                else
                {
                    this.isValidStartDate = false;
                    this.SetErrorState("Start year must be a valid number");
                }


                this.SetModelStatusFields();

            }


        }

        public void SetParishEndYear(string param)
        {

            if (this.parishEndYearStr != param)
            {
                this.parishEndYearStr = param;

                if (CsUtils.ValidYear(this.parishEndYearStr, out this.parishEndYearInt))
                {
                    this.isValidEndDate = true;
                }
                else
                {
                    this.isValidEndDate = false;
                    this.SetErrorState("End year must be a valid number");
                }


                this.SetModelStatusFields();

            }


        }

        public void SetParishLong(string param)
        {
            if (this.parishLongStr != param)
            {
                this.parishLongStr = param;

                if (Double.TryParse(this.parishLongStr, out this.parishLongDub))
                {
                    this.isValidParishLong = true;
                }
                else
                {
                    this.isValidParishLong = false;
                    this.SetErrorState("parish long value not valid");
                }


                this.SetModelStatusFields();

            }
        }

        public void SetParishLat(string param)
        {
            if (this.parishLatStr != param)
            {
                this.parishLatStr = param;

                if (Double.TryParse(this.parishLatStr, out this.parishLatDub))
                {
                    this.isValidParishLat = true;
                }
                else
                {
                    this.isValidParishLat = false;
                    this.SetErrorState("parish lat value not valid");
                }


                this.SetModelStatusFields();

            }
        }

        #endregion

        public override void DeleteSelectedRecords()
        {

            if (!IsValidDelete()) return;

            if (this.IsValidEntry)
            {
                ParishsBLL parishsBll = new ParishsBLL();
                foreach (Guid recIdx in this.SelectedRecordIds)
                {
                    parishsBll.DeleteParishById(recIdx);
                }

                Refresh();
              
            }
             
        }

        public override void EditSelectedRecord()
        {

            if (!IsValidEdit()) return;

            if (this.IsValidEntry)
            {
                
                ParishsBLL parishsBll = new ParishsBLL();
                parishsBll.UpdateParish2(this.SelectedRecordId, this.parishName, this.parishRegisterNotes,
                    this.parishRegistersDeposited, this.parishParent, this.parishStartYearInt, parishEndYearInt, parishRegistersCounty,
                    Convert.ToDecimal(this.parishLatDub),Convert.ToDecimal(this.parishLongStr));

                base.EditSelectedRecord();

                Refresh();

            }
            
        }

        public override void InsertNewRecord()
        {

            if (!IsValidInsert()) return;

            if (this.IsValidEntry)
            {
                if (!IsValidEdit()) return;
                ParishsBLL parishsBll = new ParishsBLL();

                this.SetSelectedRecordId(parishsBll.AddParish(this.parishName, this.parishRegisterNotes,
                    this.parishRegistersDeposited, this.parishParent, this.parishStartYearInt,
                    parishRegistersCounty, parishEndYearInt, Convert.ToDecimal(this.parishLatDub), Convert.ToDecimal(this.parishLongDub)));


                Refresh();

            }
             
        }

        public override void Refresh()
        {

            if (!IsvalidSelect()) return;
      
            ParishsBLL parishsBll = new ParishsBLL();

         //   this.parishDataTable = parishsBll.GetParishById(this.SelectedRecordId);

            this.parishEntity = parishsBll.GetParishById2(this.SelectedRecordId);

            if (this.parishEntity != null)
            {
                this.SetParishName(this.parishEntity.ParishName);
                this.SetParishParent(this.parishEntity.ParentParish);
                this.SetParishRegisterNotes(this.parishEntity.ParishNotes);
                this.SetParishRegistersDeposited(this.parishEntity.ParishRegistersDeposited);
                this.SetParishStartYear(this.parishEntity.ParishStartYear.ToString());
                this.SetParishEndYear(this.parishEntity.ParishEndYear.ToString());
                this.SetParishRegistersCounty(this.parishEntity.ParishCounty);
                this.SetParishLat(Convert.ToDouble(this.parishEntity.ParishX).ToString("N6"));
                this.SetParishLong(Convert.ToDouble(this.parishEntity.ParishY).ToString("N6"));
            }


            this.NotifyObservers<ParishsEditorModel>(this);
        }


        public void SetEditorUI()
        {
            throw new NotImplementedException();
        }

        public override void SetModelStatusFields()
        {


            base.SetModelStatusFields();
        }



        public override void SetFromQueryString(string param)
        {
            Debug.WriteLine("editor model SetFromQueryString :" + param);

            NameValueCollection query = HttpUtility.ParseQueryString(param);
            Guid selectedId = Guid.Empty;

            Guid.TryParse(query["id"], out selectedId);

            query.ReadInErrorsAndSecurity(this);

            //if (query.AllKeys.Contains("error"))
            //{
            //    this.SetErrorState(query["error"] ?? "");
            //}

            this.SetSelectedRecordId(selectedId);

          //  this.Refresh();

        }

       
    }
}
