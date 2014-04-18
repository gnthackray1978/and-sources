using System;
using System.Collections.Generic;
using TDBCore.EntityModel;
using TDBCore.Types;
using TDBCore.Types.DTOs;

namespace TDBCore.BLL
{
    public interface IPersonDal
    {
        void Delete(Guid deathBirthRecId);
        void Update(ServicePerson person);
        void UpdateDuplicateRefs(Guid person1, Guid person2);
        Guid Insert(ServicePerson person);
        ServicePerson Get(Guid personId);
        IList<ServicePerson> GetByDupeRef(Guid dupeRef);
        List<ServicePerson> GetByFilter(PersonSearchFilter personSearchFilter);
        void UpdateUniqueRefs();
        void MergeDuplicateRecords(Guid personId);
        void UpdateLocationIdsFromParishTable();
        void UpdateDateEstimates();
        List<Guid> DelinkPersons(List<Guid> selectedRecordIds);
        LocationDictionary GetEntryByLocatAndCounty(string locat, string county);
    }
}