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
        readonly IPersonDal _personDal;
        readonly ISourceMappingsDal _sourceMappingsDal;
      //  readonly IRelationsDal _relationsDal;
 
        private readonly ISecurity _security;


        public PersonSearch(ISecurity security)
        {
            _security = security;
            _personDal = new PersonDal();
            _sourceMappingsDal = new SourceMappingsDal();
          //  _relationsDal = new RelationsDal();

        }
        public PersonSearch(ISecurity security, IPersonDal iPersonDal, ISourceMappingsDal iSourceMappingsDal)
        {
            _security = security;
            _personDal = iPersonDal;
            _sourceMappingsDal = iSourceMappingsDal;
            

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
                        tpServicePerson = _personDal.GetByDupeRef(personSearchFilter.ParentId);
                    }
                       
                    break;
                  

                case PersonSearchTypes.Simple:
                    tpServicePerson = _personDal.GetByFilter(personSearchFilter).OrderBy(o => o.BirthYear).ToList();                      
                    break;

                case PersonSearchTypes.IdList:
                    tpServicePerson = _personDal.GetByIdList(personSearchFilter).OrderBy(o => o.BirthYear).ToList();
                    break;
            }



           return tpServicePerson.ToServicePersonObject(shaper.Column, shaper.RecordPageSize,shaper.RecordStart);
        }
        
        public void DeleteRecords(List<Guid> personIds)
        {
            if (!_security.IsValidDelete()) throw new SecurityException("Missing delete permission");

            personIds.ForEach(p => _personDal.Delete(p));           
        }


        public ServicePerson Get(Guid personId)
        {
            var servicePerson = new ServicePerson();

            if (!_security.IsvalidSelect()) throw new SecurityException("Missing select permission");

            if (personId == Guid.Empty) return servicePerson;

            servicePerson = _personDal.Get(personId);

            if (servicePerson != null)
                servicePerson.Sources = _sourceMappingsDal.GetSourceGuidList(personId);

            return servicePerson;
        }

        private void EditSelectedRecord(ServicePerson servicePerson,List<Guid> sourceIds)
        {
            if (!_security.IsValidEdit()) throw new SecurityException("Missing edit permission");

            _personDal.Update(servicePerson);

            _sourceMappingsDal.WritePersonSources2(servicePerson.PersonId, sourceIds, _security.UserId());
        }

        private void InsertNewRecord(ServicePerson servicePerson, List<Guid> sourceIds)
        {
            if (!_security.IsValidInsert()) throw new SecurityException("Missing insert permission");
            
            _personDal.Insert(servicePerson);

            _sourceMappingsDal.WritePersonSources2(servicePerson.PersonId, sourceIds, _security.UserId());
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

      //      _relationsDal.GetRelationsByMapId(persons, 1, _security.UserId()).ForEach(r => _personDal.UpdateDuplicateRefs(r.PersonA, r.PersonB));

            return "";
        }

        public string SetDefaultPersonForTree(Guid param)
        {
            if (!_security.IsValidEdit()) throw new SecurityException("Missing edit permission");

            if (param == Guid.Empty)
            {
                throw new InvalidDataException("You need to select more than source!");
            }

            _sourceMappingsDal.SetDefaultTreePerson(param, param);

            return "";
        }

        public string DelinkPersons(List<Guid> persons)
        {

            if (!_security.IsValidEdit()) throw new SecurityException("Missing edit permission");
          
            if (persons.IsNullOrBelowMinSize(1))
            {
                throw new InvalidDataException("You need to select more than source!");
            }

            _personDal.DelinkPersons(persons);

            return "";
        }

        public void UpdateDateEstimates()
        {
            _personDal.UpdateDateEstimates();
        
             
        }

        public string MergeDuplicates(Guid person)
        {
            if (!_security.IsValidEdit()) return "You dont have permission to edit!";

            if (person != Guid.Empty)
            {
                _personDal.MergeDuplicateRecords(person);                
            }

            return "";
        }
    
        public void UpdateDeletedBirths()
        {
            _personDal.UpdateUniqueRefs();
        }
    
        public void UpdateLocationsFromParishList()
        {
            _personDal.UpdateLocationIdsFromParishTable();
        }
       
 


    }


   
}
