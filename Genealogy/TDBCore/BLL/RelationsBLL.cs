using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.Types;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;
using TDBCore.Types.libs;


namespace TDBCore.BLL
{
    public class RelationsBll : BaseBll
    {




     

        public List<GetAncestors_Result> GetAncestors(Guid personId)
        {
            List<GetAncestors_Result> ancestors = ModelContainer.GetAncestors(personId).ToList();

            if (ancestors == null) ancestors = new List<GetAncestors_Result>();

            return ancestors;
        }

        public List<GetDescendants_Result> GetDescendants(Guid personId)
        {
            List<GetDescendants_Result> descendants = ModelContainer.GetDescendants(personId).ToList();

            if (descendants == null) descendants = new List<GetDescendants_Result>();

            return descendants;
        }


        public List<GetDescendantSpouses_Result> GetDescendantSpouses(Guid personId)
        {
            List<GetDescendantSpouses_Result> descendants = ModelContainer.GetDescendantSpouses(personId).ToList();

            if (descendants == null) descendants = new List<GetDescendantSpouses_Result>();
       //     uvw_ParentMapChildren
            return descendants;
        }

 
     


        public void DeleteRelationMapping(List<Guid> SelectedRecordIds)
        {
            if (SelectedRecordIds.Count > 0)
            {
                RelationsBll relationsBll = new RelationsBll();

                Guid personA = SelectedRecordIds[0];

                if (SelectedRecordIds.Count == 1)
                {
                    var relationMaps = relationsBll.GetRelationByChildOrParent(personA, 1).FirstOrDefault();

                    if (relationMaps != null)
                    {
                        relationsBll.DeleteRelationMapping(relationMaps.RelationId);
                    }

                }
                else
                {
                    foreach (Guid selectedPerson in SelectedRecordIds)
                    {
                        var relationship = relationsBll.GetRelationByPersons2(personA, selectedPerson).FirstOrDefault();

                        if (relationship != null)
                        {
                            relationsBll.DeleteRelationMapping(relationship.RelationId);
                        }
                    }

                }
            }
        }



        public List<RelationDto> GetRelationsByMapId(List<Guid> ids, int relationTypeId, int userId)
        {

            var relationDtos = new List<RelationDto>();


            if (relationTypeId == 1 && !ids.IsNullOrBelowMinSize(2))
            {
                foreach (int relMapId in InsertRelations(ids, relationTypeId, userId))
                {
                    relationDtos.AddRange(GetRelationsById2(relMapId).ToList().Select(rrow => new RelationDto() {PersonA = rrow.PersonA.Person_id, PersonB = rrow.PersonB.Person_id}));
                }
            }

            return relationDtos;
        }
 

        public List<int> InsertRelations(List<Guid> ids, int relationTypeId, int userId)
        {
            return ids.IsNullOrEmpty() ? new List<int>() : ids.Select(t => InsertRelation(ids[0], t, relationTypeId, userId)).ToList();
        }


        public IQueryable<RelationType> GetRelationTypes2()
        {


            return ModelContainer.RelationTypes;
        }

      

        public IQueryable<Relation> GetRelationsByType2(int relationTypeId)
        {

            return ModelContainer.Relations.Where(o => o.RelationType.RelationTypeId == relationTypeId);
        }

       

        public IQueryable<Relation> GetRelationsById2(int relationMapId)
        {
            return ModelContainer.Relations.Where(o => o.RelationId == relationMapId);

        }

     

        public IQueryable<Relation> GetRelationByParent2(Guid parentId, int typeId)
        {
        

            return ModelContainer.Relations.Where(o => o.PersonB.Person_id == parentId && o.RelationType.RelationTypeId == typeId);
        }

       

        public IQueryable<Relation> GetRelationByChild2(Guid childId, int typeId)
        {
            return ModelContainer.Relations.Where(o => o.PersonA.Person_id == childId && o.RelationType.RelationTypeId == typeId);
       
        }

        public IQueryable<Relation> GetRelationByChildOrParent(Guid personId, int typeId)
        {
            return ModelContainer.Relations.Where(o => (o.PersonA.Person_id == personId || o.PersonB.Person_id == personId) && 
                o.RelationType.RelationTypeId == typeId);

        }

        public IQueryable<Relation> GetRelationByChildOrParent(Guid personId)
        {
            return ModelContainer.Relations.Where(o => (o.PersonA.Person_id == personId || o.PersonB.Person_id == personId) );

        }
       

        public IQueryable<Relation> GetRelationByPersons2(Guid person1, Guid person2)
        {
            return ModelContainer.Relations.Where(o => (o.PersonA.Person_id == person1 && o.PersonB.Person_id == person2) 
                || (o.PersonB.Person_id == person1 && o.PersonA.Person_id == person2));
        }

     

        public IQueryable<Relation> GetRelationByAll2(Guid person1, Guid person2, int typeId)
        {

//            WHERE        (PersonA = @personA) AND (PersonB = @personB) AND (RelationType = @relationType) 
//OR
//                         (PersonA = @personB) AND (PersonB = @personA) AND (RelationType = @relationType)

            return GetRelationByPersons2(person1, person2).Where(o => o.RelationType.RelationTypeId == typeId);
            
            //return ModelContainer.Relations.Where(o => o.PersonA.Person_id == person1 && o.PersonB.Person_id == person2 && o.RelationType.RelationTypeId == typeId);               

        }

       

        public IQueryable<Relation> GetRelationByParents2(Guid person1)
        {
      //      var test = ModelContainer.Relations.Include(i=>i,


            return ModelContainer.Relations.Where(o => o.PersonA.Person_id == person1 && (o.RelationType.RelationTypeId == 4 || o.RelationType.RelationTypeId ==2));


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

            Person persona = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == personA);
            Person personb = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == personB);
            RelationType relationType = ModelContainer.RelationTypes.FirstOrDefault(o => o.RelationTypeId == relationTypeId);

            if (persona != null && personb != null && relationType != null)
            {
                Relation relation = new Relation();

                relation.PersonA = persona;
                relation.PersonB = personb;
                relation.RelationType = relationType;
                relation.UserId = userId;

                ModelContainer.Relations.Add(relation);
                ModelContainer.SaveChanges();

                return relation.RelationId;
            }
            else
            {
                ErrorCondition = "Couldnt insert relation";
                return 0;
            }


            
        }


        /// <summary>
        /// finds duplicate relations
        /// </summary>
        public void FindDuplicateRelations()
        {
            var groups = from r in ModelContainer.Relations group r.RelationId by new { r.PersonA.Person_id, person2 = r.PersonB.Person_id, r.RelationType.RelationTypeId }
                             into g where g.Count() > 1 select new { g.Key.Person_id, g.Key.person2, g.Key.RelationTypeId, count = g.Count() } ;


            foreach (var _group in groups)
            { 
                Console.WriteLine(_group.count);

            }
        }
    
    }
}
