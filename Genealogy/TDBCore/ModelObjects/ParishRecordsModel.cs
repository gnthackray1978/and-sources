using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using GedItter.BLL;
//using TDBCore.Datasets;

namespace TDBCore.ModelObjects
{
    public class ParishRecordsModel : IParishRecordsModel
    {

        int startYear = 0;
        int endYear = 0;
        string strStartYear;
        string strEndYear;
        //bool isValidStartYear;
        bool isValidEndYear;

        string parishName = "";
        string parishDeposited = "";
        string parishCounty = "";
        bool isBaptism;
        bool isMarriage;
        bool isBurials;

        bool isValidOrigDist;
        bool isValidOrigLong;
        bool isValidOrigLat;


        string stroriginDistance;
        string stroriginLong ;
        string stroriginLat;

        double originDistance = 0;
        double originLong = 0;
        double originLat = 0;

        List<Types.ParishDataType> parishDataTypes = new List<Types.ParishDataType>();
        List<Types.ParishRecord> parishRecords = new List<Types.ParishRecord>();
        List<Types.SilverParish> parishs = new List<Types.SilverParish>();


        public bool IsValidStartYear
        {
            get 
            {
                if (startYear > 1000)
                    return true;
                else
                    return false;
            }
        }

        public bool IsValidEndYear
        {
            get 
            {
                if (endYear > 1000)
                    return true;
                else
                    return false;
            }
        }

        public string StartDate
        {
            get 
            {
                return this.startYear.ToString();
            }
        }

        public string EndDate
        {
            get 
            {
                return this.endYear.ToString();
            }
        }

        public string ParishName
        {
            get 
            {
                return this.parishName;
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

        public bool Baptisms
        {
            get 
            {
                return this.isBaptism;
            }
        }

        public bool Marriages
        {
            get 
            {
                return this.isMarriage;
            }
        }

        public bool Deaths
        {
            get 
            {
                return this.isBurials;
            }
        }

        public void SetBaptisms(bool param)
        {
            if (param != isBaptism)
                isBaptism = param;
        }

        public void SetMarriages(bool param)
        {
            if (param != isMarriage)
                isMarriage = param;
        }

        public void SetDeaths(bool param)
        {
            if (param != isBurials)
                isBurials = param;
        }

        public void SetParishCounty(string param)
        {
            if (parishCounty != param)
                parishCounty = param;
        }

        public void SetParishDeposited(string param)
        {
            if (parishDeposited != param)
                parishDeposited = param;
        }

        public void SetParishName(string param)
        {
            if (parishName != param)
                parishName = param;
        }

        public void SetStartDate(string param)
        {
            if (this.strStartYear != param)
            {
                int _birthYear = 0;
                if (Int32.TryParse(param, out _birthYear))
                {
                    //this.isValidStartYear = true;
                    this.startYear = _birthYear;
                    this.strStartYear = param;
                }
                else
                {
                  //  this.isValidStartYear = false;
                }

            //    this.isDataChanged = true;
            //    this.SetModelStatusFields();
            }
        }

        public void SetEndDate(string param)
        {
            if (this.strEndYear != param)
            {
                int _birthYear = 0;
                if (Int32.TryParse(param, out _birthYear))
                {
                    this.isValidEndYear = true;
                    this.endYear = _birthYear;
                    this.strEndYear = param;
                }
                else
                {
                    this.isValidEndYear = false;
                }

                //    this.isDataChanged = true;
                //    this.SetModelStatusFields();
            }
        }

        public List<Types.ParishDataType> ParishDataTypes
        {
            get 
            {
                return this.parishDataTypes;
            }
        }

        public List<Types.ParishRecord> ParishRecords
        {
            get 
            {
                return this.parishRecords;
            }
        }

        public List<Types.SilverParish> Parishs
        {
            get
            {
                return this.parishs;
            }
        }


        public void NotifyObservers()
        {
            
        }

        public void AddObserver(IParishRecordsView paramView)
        {
           
        }

        public void RemoveAllObservers()
        {
             
        }

        public void RemoveObserver(IParishRecordsView paramView)
        {
           
        }


        public double OriginDistance
        {
            get 
            {
                return this.originDistance;
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



        public void SetOriginDistance(string param)
        {
            
            if (this.stroriginDistance != param)
            {
                double _birthYear = 0;
                if (Double.TryParse(param, out _birthYear))
                {
                    this.isValidOrigDist = true;
                    this.originDistance = _birthYear;
                    this.stroriginDistance = param;
                }
                else
                {
                    this.isValidOrigDist = false;
                }

                //    this.isDataChanged = true;
                //    this.SetModelStatusFields();
            }
        }

        public void SetOriginLat(string param)
        {
            if (this.stroriginLat != param)
            {
                double _birthYear = 0;
                if (Double.TryParse(param, out _birthYear))
                {
                    this.isValidOrigLat = true;
                    this.originLat = _birthYear;
                    this.stroriginLat = param;
                }
                else
                {
                    this.isValidOrigLat = false;
                }

                //    this.isDataChanged = true;
                //    this.SetModelStatusFields();
            }
        }

        public void SetOriginLong(string param)
        {
            if (this.stroriginLong != param)
            {
                double _birthYear = 0;
                if (Double.TryParse(param, out _birthYear))
                {
                    this.isValidOrigLong = true;
                    this.originLong = _birthYear;
                    this.stroriginLong = param;
                }
                else
                {
                    this.isValidOrigLong = false;
                }

                //    this.isDataChanged = true;
                //    this.SetModelStatusFields();
            }
        }


        public void Refresh()
        {

            // need to pupulate these 3

            //List<Types.ParishDataType> parishDataTypes = new List<Types.ParishDataType>();
            //List<Types.ParishRecord> parishRecords = new List<Types.ParishRecord>();
            //List<Types.Parish> parishs = new List<Types.Parish>();



        }


        private void GetParishData(Guid parishId, int start, int end)
        {
            ParishRecordsBLL parishRecordsbll = new ParishRecordsBLL();

            var parishRecordsDataTable = parishRecordsbll.GetParishRecordsById2(parishId);



            foreach (var prr in parishRecordsDataTable)
            { 
            //prr.
            }



        }

    }
}
