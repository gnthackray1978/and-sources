using System;
using System.Collections.Generic;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;

namespace TDBCore.BLL
{
    public interface IMarriagesDal
    {
        Guid ReorderMarriages(Guid marriageId);
        string SwapSpouses(List<Guid> marriageIds);
        List<Guid> GetDeletedMarriages();
        Guid GetMarriageUniqueRef(Guid marriageId);
        IList<Guid> GetMarriageUniqueRefs(List<Guid> marriageIds, bool returnEmpty = false);
        ServiceMarriage GetMarriageById2(Guid marriageId);
        IList<Guid> GetMarriageIdsByUniqueRef(Guid uniqueRef);
        IList<MarriageResult> GetDataByUniqueRef(Guid uniqueRef);
        IList<Guid> GetDataByDupeRefByMarriageId(Guid marriageId);
        List<MarriageResult> GetFilteredMarriages(MarriageSearchFilter m);
        void MergeMarriages(Guid marriageToMergeIntoId, Guid marriageToMergeId);
        Guid InsertMarriage(ServiceMarriage sm);

        void UpdateMarriageUniqRef(Guid marriageId,
            Guid uniqueRef,
            int totalEvents,
            int eventPriority);

        void UpdateMarriage(ServiceMarriage serviceMarriage);
        void DeleteMarriageTemp2(Guid marriageId);
    }
}