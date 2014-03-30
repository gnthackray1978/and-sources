using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using TDBCore.Types;
using TDBCore.Types.DTOs;

namespace TDBCore.BLL
{
    public class MarriageWitnessesBll : BaseBll
    {

        public void MarkWitnessesAsDeleted(Guid marriageId)
        {
            foreach (Person person in ModelContainer.MarriageMapWitnesses.Where(m => m.Marriage.Marriage_Id == marriageId).Select(p => p.Person))
            {
                person.IsDeleted = true;
            }
            
            ModelContainer.SaveChanges();
        }

        public List<MarriageWitness> GetWitnessesForMarriage(Guid marriageId)
        {
            return ModelContainer.MarriageMapWitnesses.Where(m => m.Marriage.Marriage_Id == marriageId).Select(p => new MarriageWitness { Description = p.WitnessNote, Person = p.Person }).ToList();           
        }

        public string GetWitnesseStringForMarriage(Guid marriageId)
        {           
            return string.Join(" ",ModelContainer.MarriageMapWitnesses.Where(m => m.Marriage.Marriage_Id == marriageId).Select(p => p.Person.Surname).ToArray());
        }

        public List<Guid> GetMarriagesByWitnessName(string name)
        {
            return ModelContainer.MarriageMapWitnesses.Where(m => m.Person.Surname.Contains(name)).Select(p => p.Marriage.Marriage_Id).ToList();
        }

        public void UpdateWitnessesForMarriage(int witnessForMarriageId, Guid personId, Guid marriageId)
        {
            MarriageMapWitness mmw = ModelContainer.MarriageMapWitnesses.FirstOrDefault(m => m.MarriageMapWitnessIndex == witnessForMarriageId);

            Marriage mToUpDate = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

            Person pToUpdate = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == personId);

            if (mmw != null && mToUpDate!= null && pToUpdate != null)
            {
                mmw.Marriage = mToUpDate;
                mmw.Person = pToUpdate;
                
            }

            ModelContainer.SaveChanges();
        }

        public void InsertWitnessesForMarriage(Guid marriageId, IList<MarriageWitness> persons)
        {
            Marriage mToUpDate = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

            DeleteWitnessesForMarriage(marriageId);

            foreach (var dupePerson in persons.RemoveDuplicateReferences())
            {
                ModelContainer.Persons.Remove(dupePerson.Person);
            }
           
            foreach (var person in persons)
            {
                ModelContainer.MarriageMapWitnesses.Add(new MarriageMapWitness { Person = person.Person, Marriage = mToUpDate, WitnessNote = person.Description });
            }
             
            ModelContainer.SaveChanges();

        }

        public void DeleteWitnessesForMarriage(Guid marriageId)
        {
            foreach (var mmw in ModelContainer.MarriageMapWitnesses.Where(m => m.Marriage.Marriage_Id == marriageId).ToList())
            {       
                ModelContainer.MarriageMapWitnesses.Remove(mmw);

                if (mmw.Person != null)
                {
                    if (ModelContainer.MarriageMapWitnesses.Count(o => o.Person.Person_id == mmw.Person.Person_id) == 0)
                        ModelContainer.Persons.Remove(mmw.Person);
                }
            }
            
            ModelContainer.SaveChanges();


        }

        public void DeleteWitnessEntriesForMarriage(Guid marriageId)
        {
            foreach (var mmw in ModelContainer.MarriageMapWitnesses.Where(m => m.Marriage.Marriage_Id == marriageId).ToList())
            {
                ModelContainer.MarriageMapWitnesses.Remove(mmw);
            }

            ModelContainer.SaveChanges();


        }

        public string GetWitnessMarriageDesc(Guid personId, out Guid marriageId)
        {
            Marriage _m = ModelContainer.MarriageMapWitnesses.Where(m => m.Person.Person_id == personId).Select(s => s.Marriage).FirstOrDefault();


            if (_m != null)
            {
                marriageId = _m.Marriage_Id;
                return _m.ToGeneralDescription();
            }
            Guid returnId = Guid.Empty;
            marriageId = returnId;
            return "";
        }


    }
}
