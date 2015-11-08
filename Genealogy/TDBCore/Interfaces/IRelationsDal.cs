using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;

namespace TDBCore.BLL
{
    public interface IRelationsDal
    {
        void DeleteRelationMapping(List<Guid> selectedRecordIds);
        List<RelationDto> GetRelationsByMapId(List<Guid> ids, int relationTypeId, int userId);
        List<int> InsertRelations(List<Guid> ids, int relationTypeId, int userId);
        IQueryable<RelationTypes> GetRelationTypes2();
        IQueryable<Relations> GetRelationsByType2(int relationTypeId);
        IQueryable<Relations> GetRelationsById2(int relationMapId);
        IQueryable<Relations> GetRelationByChildOrParent(Guid personId, int typeId);
        IQueryable<Relations> GetRelationByChildOrParent(Guid personId);
        IQueryable<Relations> GetRelationByPersons2(Guid person1, Guid person2);
        void DeleteRelationMapping(int relationMappingId);
        int InsertRelation(Guid personA, Guid personB, int relationTypeId, int userId);
    }
}