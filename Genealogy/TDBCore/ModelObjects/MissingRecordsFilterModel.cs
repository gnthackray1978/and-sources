using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
////using TDBCore.Datasets;
using TDBCore.EntityModel;

namespace GedItter.ModelObjects
{
    public class MissingRecordsFilterModel : EditorBaseModel<int>, IMissingRecordsModel
    {

       // TDBCore.Datasets.DsMissingRecords.MissingRecordsDataTable missingRecordsDataTable = new TDBCore.Datasets.DsMissingRecords.MissingRecordsDataTable();

        IList<MissingRecord> missingRecordsList = new List<MissingRecord>();


        int startDateInt = 0;
        string startDate = "";
        int endDateInt = 0;
        string endDate = "";
        string parishName = "";
        string parishDeposited = "";
        bool isValidStartYear = false;
        bool isValidEndYear = false;
        bool isValidParishLong = true;
        bool isValidParishLat = true;
        bool isValidDistance = true;

        bool? includeBaptisms = null;

        string originLatStr = "0";
        double originLat;

        string originLongStr = "0";
        double originLong;

        double distance =   50;
        string distanceStr = "0";

        Guid originParishId = Guid.Empty;


        #region IMissingRecords Members

      

        public IList<MissingRecord> MissingRecordsList 
        {
            get
            {
                return this.missingRecordsList;
            }
        }


        public string StartDate
        {
            get 
            {
                return this.startDate;
            }
        }

        public string EndDate
        {
            get 
            {
                return this.endDate;
            }
        }

        public string ParishName
        {
            get 
            {
                return this.parishName;
            }
        }

        public void SetParishName(string param)
        {
            if (this.parishName != param)
            {
                this.parishName = param;
                this.SetModelStatusFields();
            }
            
        }

        public void SetStartDate(string param)
        {
            if (this.startDate != param)
            {
                this.startDate = param;

                this.isValidStartYear = Int32.TryParse(this.startDate, out this.startDateInt);

                this.SetModelStatusFields();
            }
        }

        public void SetEndDate(string param)
        {
            if (this.endDate != param)
            {
                this.endDate = param;

                this.isValidEndYear = Int32.TryParse(this.endDate, out this.endDateInt);

                this.SetModelStatusFields();
            }
        }
        #endregion

        public bool? IncludeBaptisms
        {
            get 
            {
                return this.includeBaptisms;
            }
        }


        public double OriginLat
        {
            get
            {
                return this.originLat;
            }
        }

        public double OriginLong
        {
            get
            {
                return this.originLong;
            }
        }

        public double Distance
        {
            get
            {
                return this.distance;
            }
        }


        public Guid OriginParishId
        {
            get 
            {
                return this.originParishId;
            }
        }

        private void SetLocationFromParishId()
        {
            BLL.ParishsBLL parishsBll = new GedItter.BLL.ParishsBLL();
            var pdt = parishsBll.GetParishById2(this.originParishId);

            if (pdt != null)
            {
                this.SetOriginLet(pdt.ParishX.Value.ToString("N6"));
                this.SetOriginLong(pdt.ParishY.Value.ToString("N6"));

               // this.originLat = Convert.ToDouble();
               // this.originLong = Convert.ToDouble(pdt[0].ParishY);
            }
        }

        public void SetOriginParish(List<Guid> param)
        {
            if (param != null && param.Count >0)
            {
                SetOriginParish(param[0]);
            }
        }

        public void SetOriginParish(Guid param)
        {
            if (this.originParishId != param)
            {
                this.originParishId = param;

                this.SetLocationFromParishId();
                this.SetModelStatusFields();
            }
        }

        public void SetOriginLet(string param)
        {
            if (this.originLatStr != param)
            {

                this.isValidParishLat = Double.TryParse(param, out this.originLat);


                this.originLatStr = param;
                this.SetModelStatusFields();
            }
        }

        public void SetOriginLong(string param)
        {
            if (this.originLongStr != param)
            {
                this.isValidParishLong = Double.TryParse(param, out this.originLong);

                this.originLongStr = param;
                this.SetModelStatusFields();
            }
        }

        public void SetDistance(string param)
        {
            if (this.distanceStr != param)
            {
                this.distanceStr = param;

                this.isValidDistance = Double.TryParse(this.distanceStr, out this.distance);


                this.SetModelStatusFields();
            }
        }

        public void SetIncludeBaptisms(bool? param)
        {
            if (this.includeBaptisms != param)
            {
                this.includeBaptisms = param;
                this.SetModelStatusFields();
            }
        }


        public void SetIncludeBaptisms(bool paramBap, bool paramMar)
        {
            if (paramBap && paramMar)
            {
                this.includeBaptisms = null;
            }
            else
            {
                if (paramBap)
                    this.includeBaptisms = true;
                else
                    this.includeBaptisms = false;
            }
        }

        #region validation props
        public bool IsValidDistance
        {
            get
            {
                return this.isValidDistance;
            }
        }

        public bool IsValidOriginLong
        {
            get
            {
                return this.isValidParishLong;
            }
        }

        public bool IsValidOriginLat
        {
            get
            {
                return this.isValidParishLat;
            }
        }

        public bool IsValidStartYear
        {
            get 
            {
                return this.isValidStartYear;
            }
        }

        public bool IsValidEndYear
        {
            get 
            {
                return this.isValidEndYear;
            }
        }

        #endregion


        //public void AddObserver(IMissingRecordsView paramView)
        //{
        //    aList.Add(paramView);
        //}

        //public void RemoveObserver(IMissingRecordsView paramView)
        //{
        //    aList.Remove(paramView);
        //}

        //public void NotifyObservers()
        //{
        //    foreach (IMissingRecordsView view in aList)
        //    {
        //        view.Update(this);
        //    }
        //}

        public void SetModelStatusFields()
        {
            base.SetModelStatusFields();
        }

        public override void Refresh()
        {

            if (!IsvalidSelect()) return;

            BLL.MissingParishRecordsBLL missingRecordsBll = new GedItter.BLL.MissingParishRecordsBLL();
            
            double lat1 = this.originLat;
            double lon1 = this.originLong;

            double lat2 = 52.889993;
            double lon2 = -0.453554;



            double d = GeoCodeCalc.CalcDistance(lat1, lon1, lat2, lon2);
           
            

            if (this.startDateInt == 0 && this.endDateInt == 0)
            {
                this.missingRecordsList = missingRecordsBll.GetMissingRecords2(this.parishName,
                    this.includeBaptisms, this.parishDeposited).ToList();
            }
            else
            {
                this.missingRecordsList = missingRecordsBll.GetMissingRecords2(this.parishName,
                    this.startDateInt, this.endDateInt, this.includeBaptisms, this.parishDeposited).ToList();
            }

            if (distance > 0)
            {
                int idx = 0;

                while (idx < this.missingRecordsList.Count)
                {
                    lat2 = Convert.ToDouble(this.missingRecordsList[idx].Parish.ParishX);
                    lon2 = Convert.ToDouble(this.missingRecordsList[idx].Parish.ParishY);

                    this.missingRecordsList[idx].Parish.Distance = Convert.ToInt32(GeoCodeCalc.CalcDistance(lat1, lon1, lat2, lon2));

                //    if (this.missingRecordsDataTable[idx].Distance < this.distance)
                 //       this.missingRecordsDataTable[idx].Delete();

                    idx++;
                }
            //    this.missingRecordsDataTable.AcceptChanges();

            }

            //  handle distancse seperately here
            // easier to do this by code than by sql


            this.NotifyObservers<MissingRecordsFilterModel>(this);
        }

        public void SetEditorUI()
        {
            throw new NotImplementedException();
        }







      

        public string ParishDeposited
        {
            get
            {
                return this.parishDeposited;
            }
        }

        public void SetParishDeposited(string param)
        {
            if (this.parishDeposited != param)
            {
                this.parishDeposited = param;
                this.SetModelStatusFields();
            }
        }

       
    }
}
