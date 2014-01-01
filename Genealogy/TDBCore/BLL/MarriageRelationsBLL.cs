using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;


namespace TDBCore.BLL
{
    public class MarriageRelationsBll : BaseBll
    {

        public int InsertRelation(Guid marriageA, Guid marriageB, int relationTypeId, int userId)
        {

            Marriage marriagea = ModelContainer.Marriages.FirstOrDefault(o => o.Marriage_Id == marriageA);
            Marriage marriageb = ModelContainer.Marriages.FirstOrDefault(o => o.Marriage_Id == marriageB);
            RelationType relationType = ModelContainer.RelationTypes.FirstOrDefault(o => o.RelationTypeId == relationTypeId);

            if (marriagea != null && marriageb != null && relationType != null)
            {
                MarriageRelation relation = new MarriageRelation();

                relation.MarriageA = marriagea;
                relation.MarriageB = marriageb;
                relation.RelationType = relationType;
                relation.UserId = userId;
                relation.DateAdded = DateTime.Today;

                ModelContainer.MarriageRelations.AddObject(relation);
                ModelContainer.SaveChanges();

                return relation.MarriageRelationId;
            }
            else
            {
                ErrorCondition = "Couldnt insert marriage relation";
                return 0;
            }



        }


        public IQueryable<MarriageRelation> GetRelationsById2(int relationMapId)
        {
            return ModelContainer.MarriageRelations.Where(o => o.MarriageRelationId == relationMapId);

        }


        public void DeleteMarriageRelation(int marriageRelationId)
        {
            var _record = ModelContainer.MarriageRelations.FirstOrDefault(mr => mr.MarriageRelationId == marriageRelationId);

            if (_record != null)
            {
                ModelContainer.MarriageRelations.DeleteObject(_record);
                ModelContainer.SaveChanges();
            }

        }

        public MarriageRelation GetMarriageRelationByEitherParty(Guid marriageId, int typeId)
        {

            var _marriageRelation = ModelContainer.MarriageRelations.FirstOrDefault(m => (m.MarriageA.Marriage_Id == marriageId || m.MarriageB.Marriage_Id == marriageId) && m.RelationType.RelationTypeId == typeId);

            return _marriageRelation;

        }

        public MarriageRelation GetMarriageRelationByBothParties(Guid marriage1, Guid marriage2)
        {
            return ModelContainer.MarriageRelations.Where(o => (o.MarriageA.Marriage_Id == marriage1 && o.MarriageB.Marriage_Id == marriage2)
                || (o.MarriageB.Marriage_Id == marriage1 && o.MarriageA.Marriage_Id == marriage2)).FirstOrDefault();
        }



        public void DeleteMarriageRelationByMarriages(List<Guid> SelectedRecordIds)
        {
            if (SelectedRecordIds.Count > 0)
            {


                Guid marriageA = SelectedRecordIds[0];

                if (SelectedRecordIds.Count == 1)
                {
                    var relationMaps = this.GetMarriageRelationByEitherParty(marriageA, 1);


                    if (relationMaps != null)
                    {
                        this.DeleteMarriageRelation(relationMaps.MarriageRelationId);
                    }

                }
                else
                {
                    foreach (Guid selectedPerson in SelectedRecordIds)
                    {
                        var relationship = this.GetMarriageRelationByBothParties(marriageA, selectedPerson);

                        if (relationship != null)
                        {
                            this.DeleteMarriageRelation(relationship.MarriageRelationId);
                        }
                    }

                }
            }
        }


    }
}