using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;

namespace TDBCore.BLL
{
    public interface ISourceTypesDal
    {
        void DeleteSourceTypes(List<int> sourceTypeIds);
        ServiceSourceType GetSourceTypeById(int sourceTypeId);
        IQueryable<SourceType> GetSourceTypeBySourceId2(Guid sourceId);
        List<int> GetSourceTypeIds(Guid sourceId);
        List<ServiceSourceType> GetSourceTypeByFilter(SourceTypeSearchFilter sourceTypeSearchFilter);
        void UpdateSourceType(ServiceSourceType serviceSourceType);
        int InsertSourceType(ServiceSourceType serviceSourceType);
    }
}