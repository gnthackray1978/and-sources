using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;

namespace TDBCore.BLL
{
    public interface ISourceMappingsDal
    {
        bool SetDefaultTreePerson(Guid sourceId, Guid personId);
        int Insert(Guid? sourceId, Guid? fileId, Guid? marriageId, int userId, Guid? personId, string dateAdded, int? mapTypeId);
        int UpdateDefaultPerson(int mappingId, Guid sourceId, Guid personId);
        void WriteMarriageSources(Guid recordId, List<Guid> selectedSourceGuids, int userId);
        void WritePersonSources2(Guid recordId, List<Guid> selectedSourceGuids, int userId);
        void WriteParishsToSource(Guid sourceRecordId, List<Guid> parishIdList, int userId);
        void WriteFilesIdsToSource(Guid sourceId, List<Guid> fileIdList, int userId);
        void WriteFilesToSource(Guid sourceId, List<ServiceFile> fileIdList, int userId);
        void WriteSourceTypesToSource(Guid sourceId, List<int> sourceTypeIdList, int userId);
        void WriteSourceMappings2(Guid recordId, IList<Guid> selectedSourceGuids, int userId, bool isMarriage);
        void DeleteFilesForSource(Guid sourceId);
        void DeleteByMappingId(int mappingId);
        void DeleteByMapTypeIdAndSourceId(Guid sourceId, int mapTypeId);
        void DeleteByFileIdAndSourceId(Guid? sourceId, Guid? fileId);
        void DeleteBySourceIdMarriageIdOrPersonId(Guid? sourceId, Guid? recordId);
        void DeleteSourcesForPersonOrMarriage(Guid recordId);
        void DeleteSourcesForPersonOrMarriage(Guid recordId, int? mapTypeId);
        IEnumerable<SourceMapping> GetBySourceIdAndMapTypeId2(Guid? sourceId, int? mapTypeId);
        IEnumerable<SourceMapping> GetByFileIdAndSourceId2(Guid? sourceId, Guid? fileId);
        IEnumerable<SourceMapping> GetByPersonOrMarriageIdAndSourceId2(Guid? sourceId, Guid? recordId);
        IEnumerable<SourceMapping> GetByMarriageIdOrPersonId2(Guid? recordId);
        string GetSourceGuidList(Guid? recordId);
        IEnumerable<SourceMapping> GetSourceMappingsWithFiles(Guid? recordId);
        IEnumerable<SourceMapping> GetBySourceTypesBySourceId2(Guid? recordId);
    }
}