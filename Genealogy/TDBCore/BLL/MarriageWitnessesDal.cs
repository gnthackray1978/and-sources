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
            return ModelContainer.MarriageMapWitness.
                Where(m => m.Marriages.Marriage_Id == marriageId).ToList().Select(p => p.Persons != null ? new MarriageWitness { Description = p.WitnessNote, Person = p.Persons.ToServicePerson() } : null).ToList();           
        }

        public string GetWitnesseStringForMarriage(Guid marriageId)
        {           
            return string.Join(" ",ModelContainer.MarriageMapWitness.Where(m => m.Marriages.Marriage_Id == marriageId).Select(p => p.Persons.Surname).ToArray());
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
                    ModelContainer.MarriageMapWitness.Add(new MarriageMapWitness { Persons = person, Marriages = mToUpDate, WitnessNote = personDto.Description });
            }
             
            ModelContainer.SaveChanges();

        }

        public void DeleteWitnessesForMarriage(Guid marriageId)
        {
            foreach (var mmw in ModelContainer.MarriageMapWitness.Where(m => m.Marriages.Marriage_Id == marriageId).ToList())
            {       
                ModelContainer.MarriageMapWitness.Remove(mmw);

                if (mmw.Persons != null)
                {
                    if (ModelContainer.MarriageMapWitness.Count(o => o.Persons.Person_id == mmw.Persons.Person_id) == 0)
                        ModelContainer.Persons.Remove(mmw.Persons);
                }
            }
            
            ModelContainer.SaveChanges();
        }
 


    }
}
