using System;
using System.Collections.Generic;

namespace TDBCore.BLL
{
    public interface ISourceMappingParishsDal
    {
        List<Guid> GetParishIds(Guid sourceId);
        int? InsertSourceMappingParish2(Guid sourceMappingParishId, Guid sourceMappingSourceId, int? userId);
    }
}