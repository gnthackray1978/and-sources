using System;
using System.Collections.Generic;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;

namespace TDBCore.Interfaces
{
    public interface ISourceTypesDal
    {
        void DeleteSourceTypes(List<int> sourceTypeIds);
        ServiceSourceType GetSourceTypeById(int sourceTypeId);
        IEnumerable<SourceType> GetSourceTypeBySourceId2(Guid sourceId);
        List<int> GetSourceTypeIds(Guid sourceId);
        List<ServiceSourceType> GetSourceTypeByFilter(SourceTypeSearchFilter sourceTypeSearchFilter);
        void UpdateSourceType(ServiceSourceType serviceSourceType);
        int InsertSourceType(ServiceSourceType serviceSourceType);
    }
}