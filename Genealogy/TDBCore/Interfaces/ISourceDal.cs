using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;

namespace TDBCore.BLL
{
    public interface ISourceDal
    {
        List<CensusSource> Get1841CensuSources(Guid sourceId);
        string MakeSourceString(Guid person);
        List<Person> GetPersonsForSource(Guid sourceId);
        Guid InsertSource(SourceDto sourceAjaxDto);
        void UpdateSource(SourceDto sourceAjaxDto);
        void DeleteSource2(Guid sourceId);
        List<ServiceSource> FillSourceTableBySourceIds(List<Guid> ssf);
        List<ServiceSource> FillSourceTableByFilter(SourceSearchFilter ssf);
        List<ServiceSearchResult> GetSourceByParishString(string parishs, int startYear, int endYear);
        List<SourceRecord> GetParishSourceRecords(Guid parishId);
        Source FillSourceTableById2(Guid sourceId);
        SourceDto GetSource(Guid sourceId);
        IQueryable<Source> FillSourceTableByPersonOrMarriageId2(Guid recordId);
        List<ServiceSource> FillTreeSources(SourceSearchFilter description);
        string GetSourcesRef(Guid recordId);
    }
}