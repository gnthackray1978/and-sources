using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using TDBCore.BLL;
 
using TDBCore.Types.DTOs;
 
using TDBCore.Types.enums;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace TDBCore.Types.domain
{
    public class PersonSearch 
    {
        readonly DeathsBirthsBll _deathsBirthsBll = new DeathsBirthsBll();
        readonly SourceMappingsBll _sourceMappingsBll = new SourceMappingsBll();
        readonly RelationsBll _relationsBll = new RelationsBll();
 
        private readonly ISecurity _security;


        public PersonSearch(ISecurity security)
        {
            _security = security;

        }



        public ServicePersonObject Search(PersonSearchTypes filterMode, PersonSearchFilter personSearchFilter, DataShaping shaper, IValidator validator = null)
        {
           
            if (validator != null && !validator.ValidEntry()) throw new InvalidDataException(validator.GetErrors());

            if (!_security.IsvalidSelect()) throw new SecurityException("Missing select permission");
        
            IList<ServicePerson> tpServicePerson = new List<ServicePerson>();

            switch (filterMode)
            {

                case PersonSearchTypes.Duplicates:

                    if (personSearchFilter.ParentId != Guid.Empty)
                    {
                        tpServicePerson = _deathsBirthsBll.GetDataByDupeRef(personSearchFilter.ParentId);
                    }
                       
                    break;
                  

                case PersonSearchTypes.Simple:

                    tpServicePerson = _deathsBirthsBll.GetFilterSimple2(personSearchFilter).OrderBy(o => o.BirthYear).ToList();
                      
                    break;

            }



           return tpServicePerson.ToServicePersonObject(shaper.Column, shaper.RecordPageSize,shaper.RecordStart);
        }
        
        public void DeleteRecords(List<Guid> personIds)
        {
            if (!_security.IsValidDelete()) throw new SecurityException("Missing delete permission");

            personIds.ForEach(p => _deathsBirthsBll.DeleteDeathBirthRecord2(p));           
        }


        public ServicePerson Get(Guid personId)
        {
            var servicePerson = new ServicePerson();

            if (!_security.IsvalidSelect()) throw new SecurityException("Missing select permission");

            if (personId == Guid.Empty) return servicePerson;

            servicePerson = _deathsBirthsBll.GetDeathBirthRecordById(personId);

            if (servicePerson != null)
                servicePerson.Sources = _sourceMappingsBll.GetSourceGuidList(personId);

            return servicePerson;
        }

        private void EditSelectedRecord(ServicePerson servicePerson,List<Guid> sourceIds)
        {
            if (!_security.IsValidEdit()) throw new SecurityException("Missing edit permission");

            _deathsBirthsBll.UpdateBirthDeathRecord(servicePerson);

            _sourceMappingsBll.WritePersonSources2(servicePerson.PersonId, sourceIds, _security.UserId());
        }

        private void InsertNewRecord(ServicePerson servicePerson, List<Guid> sourceIds)
        {
            if (!_security.IsValidInsert()) throw new SecurityException("Missing insert permission");
            
            _deathsBirthsBll.InsertDeathBirthRecord(servicePerson);

            _sourceMappingsBll.WritePersonSources2(servicePerson.PersonId, sourceIds, _security.UserId());
        }

        public void Save(ServicePerson servicePerson,List<Guid> sourceIds,  IValidator validator = null)
        {
            if (validator != null && !validator.ValidEntry()) throw new InvalidDataException(validator.GetErrors());

            if (servicePerson.PersonId == Guid.Empty)
            {
                InsertNewRecord(servicePerson, sourceIds);
            }
            else
            {
                EditSelectedRecord(servicePerson, sourceIds);
            }

        }

     

        public string SetDuplicateRelation(List<Guid> persons)
        {

            if (!_security.IsValidEdit()) throw new SecurityException("Missing edit permission");

            if (persons.IsNullOrBelowMinSize(2))
            {
                throw new InvalidDataException("You need to select more than source!");
                 
            }

            _relationsBll.GetRelationsByMapId(persons, 1, _security.UserId()).ForEach(r => _deathsBirthsBll.UpdateDuplicateRefs2(r.PersonA, r.PersonB));

            return "";
        }

        public string SetDefaultPersonForTree(Guid param)
        {
            if (!_security.IsValidEdit()) throw new SecurityException("Missing edit permission");

            if (param == Guid.Empty)
            {
                throw new InvalidDataException("You need to select more than source!");
            }

            _sourceMappingsBll.SetDefaultTreePerson(param, param);

            return "";
        }

        public string DelinkPersons(List<Guid> persons)
        {

            if (!_security.IsValidEdit()) throw new SecurityException("Missing edit permission");
          
            if (persons.IsNullOrBelowMinSize(1))
            {
                throw new InvalidDataException("You need to select more than source!");
            }

            _deathsBirthsBll.DelinkPersons(persons);

            return "";
        }

        public void UpdateDateEstimates()
        {
            _deathsBirthsBll.UpdateDateEstimates();
        
             
        }

        public string MergeDuplicates(Guid person)
        {
            if (!_security.IsValidEdit()) return "You dont have permission to edit!";

            if (person != Guid.Empty)
            {
                _deathsBirthsBll.MergeDuplicateRecords(person);                
            }

            return "";
        }
    
        public void UpdateDeletedBirths()
        {
            _deathsBirthsBll.UpdateDeletedBirths();
        }
    
        public void UpdateLocationsFromParishList()
        {
            _deathsBirthsBll.UpdateLocationIdsFromParishTable();
        }
       
 


    }


   
}
