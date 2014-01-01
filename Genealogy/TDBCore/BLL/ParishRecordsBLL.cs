using System;
using System.Linq;
using TDBCore.EntityModel;
////using TDBCore.Datasets;

namespace TDBCore.BLL
{
    public class ParishRecordsBll : BaseBll
    {
    



        public void InsertParishRecord2(Guid parishId, int dataType, int year, string type, bool isOriginalReg, int yearEnd)
        {

            Parish parish = ModelContainer.Parishs.FirstOrDefault(o => o.ParishId == parishId);
            ParishRecordSource prs = ModelContainer.ParishRecordSources.FirstOrDefault(p => p.RecordTypeId == dataType);


            ParishRecord parishrec = new ParishRecord();
            
            parishrec.Parish = parish;
            parishrec.ParishRecordSource = prs;
            parishrec.OriginalRegister = isOriginalReg;
            parishrec.RecordType = type;
            parishrec.Year = year;
            parishrec.YearEnd = yearEnd;

            ModelContainer.ParishRecords.AddObject(parishrec);

            ModelContainer.SaveChanges();


            //parishrecsource.

         //   Adapter.Insert(parishId, dataType, year, type, isOriginalReg, yearEnd);
        }


 

        public IQueryable<ParishRecord> GetParishRecords2()
        {

            return ModelContainer.ParishRecords;
        }

     

        public IQueryable<ParishRecord> GetParishRecordsById2(Guid parishId)
        {
            return ModelContainer.ParishRecords.Where(o => o.Parish.ParishId == parishId);
        }

      

        public IQueryable<ParishRecord> GetParishRecordsByIdAndRecordType2(Guid parishId, string recordType)
        {
            return ModelContainer.ParishRecords.Where(o => o.Parish.ParishId == parishId && o.RecordType == recordType && o.ParishRecordSource.RecordTypeId != 38 && o.ParishRecordSource.RecordTypeId != 39);
        }

        
    }
}
