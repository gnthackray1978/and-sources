using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;

namespace TDBCore.BLL
{
    public class SourceTypesDal : BaseBll, ISourceTypesDal
    {               
        public void DeleteSourceTypes(List<int> sourceTypeIds)
        {
            foreach (var record in sourceTypeIds.Select(sourceTypeId => ModelContainer.SourceTypes.FirstOrDefault(o => o.SourceTypeId == sourceTypeId)).Where(record => record != null))
            {
                ModelContainer.SourceTypes.Remove(record);
            }

            ModelContainer.SaveChanges();
        }

        //ServiceSourceType
        public ServiceSourceType GetSourceTypeById(int sourceTypeId)
        {
          
            ServiceSourceType sourceTypesDataTable =
                ModelContainer.SourceTypes.Where(o => o.SourceTypeId == sourceTypeId)
                    .Select(p => new ServiceSourceType
                    {
                            Description = p.SourceTypeDesc,
                            Order = p.SourceTypeOrder,
                            TypeId = p.SourceTypeId
                        }).FirstOrDefault();

            return sourceTypesDataTable;


        }
         
        public IQueryable<SourceType> GetSourceTypeBySourceId2(Guid sourceId)
        {
            IQueryable<SourceType> sourceTypesDataTable = from c in ModelContainer.SourceTypes where c.SourceMappings.Any(o => o.Source.SourceId == sourceId) select c;


            return sourceTypesDataTable;
        }

        public List<int> GetSourceTypeIds(Guid sourceId)
        {
            var sourceTypesDataTable =  ModelContainer.SourceTypes.Where(p => p.SourceMappings.Any(o => o.Source.SourceId == sourceId))
                                                      .Select(s => s.SourceTypeId)
                                                      .ToList();            
            return sourceTypesDataTable;

        }
         

        public List<ServiceSourceType> GetSourceTypeByFilter(SourceTypeSearchFilter sourceTypeSearchFilter)
        {
            List<ServiceSourceType> sourceTypesDataTable;



            if (sourceTypeSearchFilter.Description.Trim() == "%")
            {              
                sourceTypesDataTable = ModelContainer.SourceTypes.OrderBy(s => s.SourceTypeOrder).Select(p=> new ServiceSourceType
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
                    sourceTypesDataTable = ModelContainer.SourceTypes.Where(s => sourceTypeSearchFilter.SourceTypeIds.Contains(s.SourceTypeId)).OrderBy(s => s.SourceTypeOrder).Select(p => new ServiceSourceType
                    {
                        Description = p.SourceTypeDesc,
                        TypeId = p.SourceTypeId,
                        Order = p.SourceTypeOrder,
                        UserId = 0//p.SourceUserAdded
                    }).ToList();     
                }
                else
                {
                    sourceTypesDataTable = ModelContainer.SourceTypes.Where(o => o.SourceTypeDesc.Contains(sourceTypeSearchFilter.Description)).OrderBy(s => s.SourceTypeOrder).Select(p => new ServiceSourceType
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
 


        public void UpdateSourceType(ServiceSourceType serviceSourceType)
        {
            //Adapter.Update(sourceTypeDesc, DateTime.Today, userId, sourceTypeId);

            var stype = ModelContainer.SourceTypes.FirstOrDefault(o => o.SourceTypeId == serviceSourceType.TypeId);

            if (stype != null)
            {
                stype.SourceTypeDesc = serviceSourceType.Description;
                stype.SourceUserAdded = 1;
                stype.SourceTypeOrder = serviceSourceType.Order;
            }

            ModelContainer.SaveChanges();

        }

        public int InsertSourceType(ServiceSourceType serviceSourceType)
        {

            var sourceType = new SourceType
            {
                SourceDateAdded = DateTime.Today.ToShortDateString(),
                SourceTypeDesc = serviceSourceType.Description,
                SourceUserAdded = 1,
                SourceTypeOrder = serviceSourceType.Order
            };

            ModelContainer.SourceTypes.Add(sourceType);

            ModelContainer.SaveChanges();
            
            return sourceType.SourceTypeId;
        }


    }
}
