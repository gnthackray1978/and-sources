using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;
using TDBCore.Types.libs;


namespace TDBCore.BLL
{
    public class RelationsDal : BaseBll, IRelationsDal
    {        
        public void DeleteRelationMapping(List<Guid> selectedRecordIds)
        {
            if (selectedRecordIds.Count <= 0) return;

            var personA = selectedRecordIds[0];

            if (selectedRecordIds.Count == 1)
            {
                var relationMaps = GetRelationByChildOrParent(personA, 1).FirstOrDefault();

                if (relationMaps != null)
                {
                    DeleteRelationMapping(relationMaps.RelationId);
                }
            }
            else
            {
                foreach (var relationship in selectedRecordIds.Select(selectedPerson => GetRelationByPersons2(personA, selectedPerson)
                    .FirstOrDefault()).Where(relationship => relationship != null))
                {
                    DeleteRelationMapping(relationship.RelationId);
                }
            }
        }



        public List<RelationDto> GetRelationsByMapId(List<Guid> ids, int relationTypeId, int userId)
        {

            var relationDtos = new List<RelationDto>();
            /*REMOVED BECAUSE THOUGHT ON UNYUSE*/

            if (relationTypeId == 1 && !ids.IsNullOrBelowMinSize(2))
            {
                foreach (int relMapId in InsertRelations(ids, relationTypeId, userId))
                {
                    relationDtos.AddRange(GetRelationsById2(relMapId).ToList().Select(rrow => new RelationDto { PersonA = rrow.PersonsA.Person_id, PersonB = rrow.PersonsB.Person_id }));
                }
            }

            return relationDtos;
        }
 

        public List<int> InsertRelations(List<Guid> ids, int relationTypeId, int userId)
        {
            return ids.IsNullOrEmpty() ? new List<int>() : ids.Select(t => InsertRelation(ids[0], t, relationTypeId, userId)).ToList();
        }


        public IQueryable<RelationTypes> GetRelationTypes2()
        {


            return ModelContainer.RelationTypes;
        }

      

        public IQueryable<Relations> GetRelationsByType2(int relationTypeId)
        {

            return ModelContainer.Relations.Where(o => o.RelationTypes.RelationTypeId == relationTypeId);
        }

       

        public IQueryable<Relations> GetRelationsById2(int relationMapId)
        {
            return ModelContainer.Relations.Where(o => o.RelationId == relationMapId);

        }
         
        public IQueryable<Relations> GetRelationByChildOrParent(Guid personId, int typeId)
        {
            return ModelContainer.Relations.Where(o => (o.PersonsA.Person_id == personId || o.PersonsB.Person_id == personId) && 
                o.RelationTypes.RelationTypeId == typeId);

        }

        public IQueryable<Relations> GetRelationByChildOrParent(Guid personId)
        {
            return ModelContainer.Relations.Where(o => (o.PersonsA.Person_id == personId || o.PersonsB.Person_id == personId) );

        }
       

        public IQueryable<Relations> GetRelationByPersons2(Guid person1, Guid person2)
        {
            return ModelContainer.Relations.Where(o => (o.PersonsA.Person_id == person1 && o.PersonsB.Person_id == person2) 
                || (o.PersonsB.Person_id == person1 && o.PersonsA.Person_id == person2));
        }

      


        public void DeleteRelationMapping(int relationMappingId)
        {
            var relMap = GetRelationsByType2(relationMappingId).FirstOrDefault();

            if (relMap != null)
            {
                ModelContainer.Relations.Remove(relMap);

                ModelContainer.SaveChanges();
            }
        }



        public int InsertRelation(Guid personA, Guid personB, int relationTypeId, int userId)
        {

            var persona = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == personA);
            var personb = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == personB);
            var relationType = ModelContainer.RelationTypes.FirstOrDefault(o => o.RelationTypeId == relationTypeId);

            if (persona != null && personb != null && relationType != null)
            {
                var relation = new Relations
                {
                    PersonsA = persona,
                    PersonsB = personb,
                    RelationTypes = relationType,
                    UserId = userId
                };

                ModelContainer.Relations.Add(relation);
                ModelContainer.SaveChanges();

                return relation.RelationId;
            }

           
            return 0;
        }
         
    
    }
}
