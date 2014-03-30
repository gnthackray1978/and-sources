using System;
using System.Linq;
using TDBCore.EntityModel;


namespace TDBCore.BLL
{
    public class MissingParishRecordsBll : BaseBll
    {

       

 

        public void InsertMissingParishRecord2(Guid parishId, int dataType, int year, string type, bool isOriginalReg, int yearEnd)
        {
            //Adapter.InsertReturnValue(parishId, dataType, year, type.Trim(), isOriginalReg, yearEnd);


            Parish _parish = ModelContainer.Parishs.FirstOrDefault(o => o.ParishId == parishId);

            if (_parish != null)
            {
                MissingRecord missingrecord = new MissingRecord();
                missingrecord.Parish = _parish;
                missingrecord.OriginalRegister = isOriginalReg;
                missingrecord.Year = year;
                missingrecord.YearEnd = yearEnd;
                missingrecord.RecordType = type;
                missingrecord.DataTypeId = dataType;

                ModelContainer.MissingRecords.Add(missingrecord);
                ModelContainer.SaveChanges();
            }

           
        }

      

        public void DeleteMissingRecord2(int missingRecId)
        {
    
            var _rec = ModelContainer.MissingRecords.FirstOrDefault(m => m.MissingRecordId == missingRecId);

            if (_rec != null)
            {
                ModelContainer.MissingRecords.Remove(_rec);
            }
        }

    

        public void UpdateMissingParishRecord2(int missingParishRecord, int date, int dateTo)
        {
         //   Adapter.UpdateDates(date, dateTo, missingParishRecord);

            var _rec = ModelContainer.MissingRecords.FirstOrDefault(m => m.MissingRecordId == missingParishRecord);
            
            if (_rec != null)
            {
                _rec.Year = date;
                _rec.YearEnd = dateTo;
                ModelContainer.SaveChanges();
            }
        }

        

        public IQueryable<MissingRecord> GetMissingRecords2(int year)
        {
            return ModelContainer.MissingRecords.Where(m => m.Year == year);
        }

       

        public IQueryable<MissingRecord> GetMissingRecords2(Guid parishId)
        {
            return ModelContainer.MissingRecords.Where(o => o.Parish.ParishId == parishId);
        }

    

        public IQueryable<MissingRecord> GetMissingRecords2(string parishName,
          int startYear, int endYear, bool? includeBaptisms, string deposited)
        {
            string includeBaps = "C";
            string includeMars = "M";

            if (includeBaptisms != null)
            {

                if (includeBaptisms.Value)
                {
                    includeBaps = "C";
                    includeMars = "C";
                }
                else
                {
                    includeBaps = "M";
                    includeMars = "M";
                }

            }


            return ModelContainer.MissingRecords.Where(p=>p.Parish.ParishName.Contains(parishName) && (
                   (startYear <= p.YearEnd && endYear >= p.Year )||(startYear <= p.Year && endYear >= p.YearEnd) ||((startYear <= p.YearEnd && startYear >= p.Year)  ||(endYear >= p.Year && endYear <= p.YearEnd)))   
                   && ((p.RecordType == includeBaps || p.RecordType == includeMars) )&& (p.Parish.ParishRegistersDeposited.Contains(deposited)));

        }
 

        public IQueryable<MissingRecord> GetMissingRecords2(string parishName,
         bool? includeBaptisms, string deposited)
        {
            string includeBaps = "C";
            string includeMars = "M";

            if (includeBaptisms != null)
            {

                if (includeBaptisms.Value)
                {
                    includeBaps = "C";
                    includeMars = "C";
                }
                else
                {
                    includeBaps = "M";
                    includeMars = "M";
                }

            }


            return ModelContainer.MissingRecords.Where(p => p.Parish.ParishName.Contains(parishName)
               
     
             && ((p.RecordType == includeBaps || p.RecordType == includeMars)) && (p.Parish.ParishRegistersDeposited.Contains(deposited)));
        }

    
    }
}
