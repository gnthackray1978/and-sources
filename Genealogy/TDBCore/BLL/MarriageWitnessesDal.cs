using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;
using TDBCore.Types.libs;

namespace TDBCore.BLL
{
    public class MarriageWitnessesDal : BaseBll, IMarriageWitnessesDal
    {
        private readonly PersonDal _personDal;

        public MarriageWitnessesDal()
        {
            _personDal = new PersonDal();
        }


        public List<MarriageWitness> GetWitnessesForMarriage(Guid marriageId)
        {
            return ModelContainer.MarriageMapWitnesses.
                Where(m => m.Marriage.Marriage_Id == marriageId).ToList().Select(p => p.Person != null ? new MarriageWitness { Description = p.WitnessNote, Person = p.Person.ToServicePerson() } : null).ToList();           
        }

        public string GetWitnesseStringForMarriage(Guid marriageId)
        {           
            return string.Join(" ",ModelContainer.MarriageMapWitnesses.Where(m => m.Marriage.Marriage_Id == marriageId).Select(p => p.Person.Surname).ToArray());
        }
  
        public void InsertWitnessesForMarriage(Guid marriageId, IList<MarriageWitness> persons)
        {
            Marriage mToUpDate = ModelContainer.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

            DeleteWitnessesForMarriage(marriageId);

            foreach (var dupePerson in persons.RemoveDuplicateReferences())
            {
                _personDal.Delete(dupePerson.Person.PersonId);
                  
            }
           
            foreach (var personDto in persons)
            {
                var person = ModelContainer.Persons.FirstOrDefault(p=>p.Person_id== personDto.Person.PersonId);

                if(person!=null)
                    ModelContainer.MarriageMapWitnesses.Add(new MarriageMapWitness { Person = person, Marriage = mToUpDate, WitnessNote = personDto.Description });
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
 


    }
}
