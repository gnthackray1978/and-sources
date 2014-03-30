using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;

//using TDBCore.Datasets.DsSourceTypesTableAdapters;
//using TDBCore.Datasets;

namespace TDBCore.BLL
{
    public class SourceTypesBll : BaseBll
    {
        

        public void DeleteSourceType(int sourceTypeId)
        {
            var record = this.ModelContainer.SourceTypes.FirstOrDefault(o => o.SourceTypeId == sourceTypeId);

            if (record == null) return;
            this.ModelContainer.SourceTypes.Remove(record);
            //this.ModelContainer.SourceTypes.DeleteObject();

            this.ModelContainer.SaveChanges();
        }


        public void DeleteSourceTypes(List<int> sourceTypeIds)
        {
            foreach (var record in sourceTypeIds.Select(sourceTypeId => this.ModelContainer.SourceTypes.FirstOrDefault(o => o.SourceTypeId == sourceTypeId)).Where(record => record != null))
            {
                this.ModelContainer.SourceTypes.Remove(record);
            }

            this.ModelContainer.SaveChanges();
        }

        public int InsertSourceType(string sourceTypeDesc, int userId, int sourceTypeOrder)
        {

            var sourceType = new TDBCore.EntityModel.SourceType();

            sourceType.SourceDateAdded = DateTime.Today.ToShortDateString();
            sourceType.SourceTypeDesc = sourceTypeDesc;
            sourceType.SourceUserAdded = userId;
            sourceType.SourceTypeOrder = sourceTypeOrder;

          //  this.ModelContainer.SourceTypes.Add();
            this.ModelContainer.SourceTypes.Add(sourceType);
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


        //ServiceSourceType
        public ServiceSourceType GetSourceTypeById(int sourceTypeId)
        {
          
            ServiceSourceType sourceTypesDataTable =
                ModelContainer.SourceTypes.Where(o => o.SourceTypeId == sourceTypeId)
                    .Select(p => new ServiceSourceType()
                        {
                            Description = p.SourceTypeDesc,
                            Order = p.SourceTypeOrder,
                            TypeId = p.SourceTypeId
                        }).FirstOrDefault();

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

        public List<int> GetSourceTypeIds(Guid sourceId)
        {
            var sourceTypesDataTable =  ModelContainer.SourceTypes.Where(p => p.SourceMappings.Any(o => o.Source.SourceId == sourceId))
                                                      .Select(s => s.SourceTypeId)
                                                      .ToList();            
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

        public List<ServiceSourceType> GetSourceTypeByFilter(SourceTypeSearchFilter sourceTypeSearchFilter)
        {
            List<ServiceSourceType> sourceTypesDataTable = null;



            if (sourceTypeSearchFilter.Description.Trim() == "%")
            {              
                sourceTypesDataTable = ModelContainer.SourceTypes.OrderBy(s => s.SourceTypeOrder).Select(p=> new ServiceSourceType()
                    {
                        Description = p.SourceTypeDesc,
                        TypeId = p.SourceTypeId,
                        Order = p.SourceTypeOrder,
                        UserId = 0//p.SourceUserAdded.GetValueOrDefault()
                    }).ToList();

            }
            else
            {
                if (sourceTypeSearchFilter.SourceTypeIds.Count > 0)
                {
                    sourceTypesDataTable = ModelContainer.SourceTypes.Where(s => sourceTypeSearchFilter.SourceTypeIds.Contains(s.SourceTypeId)).OrderBy(s => s.SourceTypeOrder).Select(p => new ServiceSourceType()
                    {
                        Description = p.SourceTypeDesc,
                        TypeId = p.SourceTypeId,
                        Order = p.SourceTypeOrder,
                        UserId = 0//p.SourceUserAdded
                    }).ToList();     
                }
                else
                {
                    sourceTypesDataTable = ModelContainer.SourceTypes.Where(o => o.SourceTypeDesc.Contains(sourceTypeSearchFilter.Description)).OrderBy(s => s.SourceTypeOrder).Select(p => new ServiceSourceType()
                    {
                        Description = p.SourceTypeDesc,
                        TypeId = p.SourceTypeId,
                        Order = p.SourceTypeOrder,
                        UserId = 0//p.SourceUserAdded
                    }).ToList();                    
                }

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


        public void UpdateSourceType(ServiceSourceType serviceSourceType)
        {
            //Adapter.Update(sourceTypeDesc, DateTime.Today, userId, sourceTypeId);

            var stype = this.ModelContainer.SourceTypes.FirstOrDefault(o => o.SourceTypeId == serviceSourceType.TypeId);

            if (stype != null)
            {
                stype.SourceTypeDesc = serviceSourceType.Description;
                stype.SourceUserAdded = 1;
                stype.SourceTypeOrder = serviceSourceType.Order;
            }

            this.ModelContainer.SaveChanges();

        }

        public int InsertSourceType(ServiceSourceType serviceSourceType)
        {

            var sourceType = new TDBCore.EntityModel.SourceType();

            sourceType.SourceDateAdded = DateTime.Today.ToShortDateString();
            sourceType.SourceTypeDesc = serviceSourceType.Description;
            sourceType.SourceUserAdded = 1;
            sourceType.SourceTypeOrder = serviceSourceType.Order;

            this.ModelContainer.SourceTypes.Add(sourceType);

            this.ModelContainer.SaveChanges();
            
            return sourceType.SourceTypeId;
        }


    }
}
