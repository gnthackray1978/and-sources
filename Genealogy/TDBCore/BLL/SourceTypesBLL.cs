using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using TDBCore.Datasets.DsSourceTypesTableAdapters;
//using TDBCore.Datasets;
using TDBCore.BLL;

namespace GedItter.BLL
{
    public class SourceTypesBLL : BaseBLL
    {
       
        public void DeleteSourceType(int sourceTypeId)
        {
            var record = this.ModelContainer.SourceTypes.Where(o => o.SourceTypeId == sourceTypeId).FirstOrDefault();

            if (record != null)
            {
                this.ModelContainer.SourceTypes.DeleteObject(record);

                this.ModelContainer.SaveChanges();
            }

        }
 
        public int InsertSourceType(string sourceTypeDesc, int userId, int sourceTypeOrder)
        {

            TDBCore.EntityModel.SourceType sourceType = new TDBCore.EntityModel.SourceType();

            sourceType.SourceDateAdded = DateTime.Today.ToShortDateString();
            sourceType.SourceTypeDesc = sourceTypeDesc;
            sourceType.SourceUserAdded = userId;
            sourceType.SourceTypeOrder = sourceTypeOrder;

            this.ModelContainer.SourceTypes.AddObject(sourceType);

            this.ModelContainer.SaveChanges();
            //object i = Adapter.InsertSource(sourceTypeDesc, DateTime.Today, userId);

            //int retVal = 0;

            //Int32.TryParse(i.ToString(), out retVal);

            return sourceType.SourceTypeId;
        }
 
        public void UpdateSourceType(string sourceTypeDesc, int userId, int sourceTypeId, int sourceTypeOrder)
        {
            //Adapter.Update(sourceTypeDesc, DateTime.Today, userId, sourceTypeId);

            var stype = this.ModelContainer.SourceTypes.FirstOrDefault(o => o.SourceTypeId == sourceTypeId);

            if (stype != null)
            {
                stype.SourceTypeDesc = sourceTypeDesc;
                stype.SourceUserAdded = userId;
                stype.SourceTypeOrder = sourceTypeOrder;
            }

            this.ModelContainer.SaveChanges();

        }
 



        public IQueryable<TDBCore.EntityModel.SourceType> GetSourceTypeForObjectDataSource2(string sourceTypeId)
        {
            IQueryable<TDBCore.EntityModel.SourceType> sourceTypesDataTable = null;

            sourceTypesDataTable = this.ModelContainer.SourceTypes;

            return sourceTypesDataTable;
        }
         
        public IQueryable<TDBCore.EntityModel.SourceType> GetSourceTypeById2(int sourceTypeId)
        {
            IQueryable<TDBCore.EntityModel.SourceType> sourceTypesDataTable = null;

            sourceTypesDataTable = this.ModelContainer.SourceTypes.Where(o => o.SourceTypeId == sourceTypeId); //Adapter.GetDataBySourceTypeId(sourceTypeId);

            return sourceTypesDataTable;


        }
         

        public IQueryable<TDBCore.EntityModel.SourceType> GetSourceTypeBySourceId2(Guid sourceId)
        {


            IQueryable<TDBCore.EntityModel.SourceType> sourceTypesDataTable = null;

  
            sourceTypesDataTable = from c in ModelContainer.SourceTypes where c.SourceMappings.Any(o => o.Source.SourceId == sourceId) select c;

   
            return sourceTypesDataTable;
            
        }
         

        public IQueryable<TDBCore.EntityModel.SourceType> GetSourceTypeByFilter2(string desc, int userId)
        {
            IQueryable<TDBCore.EntityModel.SourceType> sourceTypesDataTable = null;

            if (desc == "%")
            {
                sourceTypesDataTable = ModelContainer.SourceTypes.OrderBy(s => s.SourceTypeOrder); 
            }
            else
            {
                sourceTypesDataTable = ModelContainer.SourceTypes.Where(o => o.SourceTypeDesc.Contains(desc)).OrderBy(s => s.SourceTypeOrder);            
            }

            return sourceTypesDataTable;
        }


        public List<string> GetSourceTypesFromIdList(List<int> typeIdList)
        {
            List<string> returnList = new List<string>();

            var sourceTypesDataTable = ModelContainer.SourceTypes.Where(s => typeIdList.Contains(s.SourceTypeId)).ToList();

            if (typeIdList.Count > 0)
            {
                foreach (int typeId in typeIdList)
                {
                    returnList.Add(sourceTypesDataTable.First(o => o.SourceTypeId == typeId).SourceTypeDesc);
                }
            }

            return returnList;
        }
    
    }
}
