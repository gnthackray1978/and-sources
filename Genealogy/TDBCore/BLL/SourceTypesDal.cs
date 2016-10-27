using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using TDBCore.Interfaces;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;

namespace TDBCore.BLL
{
    public class SourceTypesDal : BaseBll, ISourceTypesDal
    {               
        public void DeleteSourceTypes(List<int> sourceTypeIds)
        {
            using (var context = new GeneralModelContainer())
            {
                var st = context.SourceTypes.ToList();

                foreach (
                    var record in
                        sourceTypeIds.Select(
                            sourceTypeId =>
                                st.FirstOrDefault(o => o.SourceTypeId == sourceTypeId))
                            .Where(record => record != null))
                {
                    context.SourceTypes.Remove(record);
                }

                context.SaveChanges();
            }
        }

        //ServiceSourceType
        public ServiceSourceType GetSourceTypeById(int sourceTypeId)
        {
            using (var context = new GeneralModelContainer())
            {
                var sourceTypesDataTable =
                    context.SourceTypes.Where(o => o.SourceTypeId == sourceTypeId)
                        .Select(p => new ServiceSourceType
                        {
                            Description = p.SourceTypeDesc,
                            Order = p.SourceTypeOrder,
                            TypeId = p.SourceTypeId
                        }).FirstOrDefault();

                return sourceTypesDataTable;

            }
        }
         
        public IEnumerable<SourceType> GetSourceTypeBySourceId2(Guid sourceId)
        {
            using (var context = new GeneralModelContainer())
            {
                IEnumerable<SourceType> sourceTypesDataTable = from c in context.SourceTypes
                    where c.SourceMappings.Any(o => o.Source.SourceId == sourceId)
                    select c;


                return sourceTypesDataTable;
            }
        }

        public List<int> GetSourceTypeIds(Guid sourceId)
        {
            using (var context = new GeneralModelContainer())
            {
                var sourceTypesDataTable =
                    context.SourceTypes.Where(p => p.SourceMappings.Any(o => o.Source.SourceId == sourceId))
                        .Select(s => s.SourceTypeId)
                        .ToList();
                return sourceTypesDataTable;
            }

        }
         

        public List<ServiceSourceType> GetSourceTypeByFilter(SourceTypeSearchFilter sourceTypeSearchFilter)
        {
            using (var context = new GeneralModelContainer())
            {
                List<ServiceSourceType> sourceTypesDataTable;
                if (sourceTypeSearchFilter.Description.Trim() == "%")
                {
                    sourceTypesDataTable =
                        context.SourceTypes.OrderBy(s => s.SourceTypeOrder).Select(p => new ServiceSourceType
                        {
                            Description = p.SourceTypeDesc,
                            TypeId = p.SourceTypeId,
                            Order = p.SourceTypeOrder,
                            UserId = 0 //p.SourceUserAdded.GetValueOrDefault()
                        }).ToList();

                }
                else
                {
                    if (sourceTypeSearchFilter.SourceTypeIds.Count > 0)
                    {
                        sourceTypesDataTable =
                            context.SourceTypes.Where(
                                s => sourceTypeSearchFilter.SourceTypeIds.Contains(s.SourceTypeId))
                                .OrderBy(s => s.SourceTypeOrder)
                                .Select(p => new ServiceSourceType
                                {
                                    Description = p.SourceTypeDesc,
                                    TypeId = p.SourceTypeId,
                                    Order = p.SourceTypeOrder,
                                    UserId = 0 //p.SourceUserAdded
                                }).ToList();
                    }
                    else
                    {
                        sourceTypesDataTable =
                            context.SourceTypes.Where(
                                o => o.SourceTypeDesc.Contains(sourceTypeSearchFilter.Description))
                                .OrderBy(s => s.SourceTypeOrder)
                                .Select(p => new ServiceSourceType
                                {
                                    Description = p.SourceTypeDesc,
                                    TypeId = p.SourceTypeId,
                                    Order = p.SourceTypeOrder,
                                    UserId = 0 //p.SourceUserAdded
                                }).ToList();
                    }

                }

                return sourceTypesDataTable;
            }
        }
 


        public void UpdateSourceType(ServiceSourceType serviceSourceType)
        {
            //Adapter.Update(sourceTypeDesc, DateTime.Today, userId, sourceTypeId);
            using (var context = new GeneralModelContainer())
            {
                var stype = context.SourceTypes.FirstOrDefault(o => o.SourceTypeId == serviceSourceType.TypeId);

                if (stype != null)
                {
                    stype.SourceTypeDesc = serviceSourceType.Description;
                    stype.SourceUserAdded = 1;
                    stype.SourceTypeOrder = serviceSourceType.Order;
                }

                context.SaveChanges();
            }
        }

        public int InsertSourceType(ServiceSourceType serviceSourceType)
        {
            using (var context = new GeneralModelContainer())
            {
                var sourceType = new SourceType
                {
                    SourceDateAdded = DateTime.Today.ToShortDateString(),
                    SourceTypeDesc = serviceSourceType.Description,
                    SourceUserAdded = 1,
                    SourceTypeOrder = serviceSourceType.Order
                };

                context.SourceTypes.Add(sourceType);

                context.SaveChanges();

                return sourceType.SourceTypeId;
            }
        }


    }
}
