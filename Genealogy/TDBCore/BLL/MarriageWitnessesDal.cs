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
            using (var context = new GeneralModelContainer())
            {
                return context.MarriageMapWitness.
                    Where(m => m.Marriages.Marriage_Id == marriageId)
                    .ToList()
                    .Select(
                        p =>
                            p.Persons != null
                                ? new MarriageWitness
                                {
                                    Description = p.WitnessNote,
                                    Person = p.Persons.ToServicePerson()
                                }
                                : null)
                    .ToList();
            }
        }

        public string GetWitnesseStringForMarriage(Guid marriageId)
        {
            using (var context = new GeneralModelContainer())
            {
                return string.Join(" ",
                    context.MarriageMapWitness.Where(m => m.Marriages.Marriage_Id == marriageId)
                        .Select(p => p.Persons.Surname)
                        .ToArray());
            }
        }
  
        public void InsertWitnessesForMarriage(Guid marriageId, IList<MarriageWitness> persons)
        {
            using (var context = new GeneralModelContainer())
            {
                Marriage mToUpDate = context.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

                DeleteWitnessesForMarriage(marriageId);

                foreach (var dupePerson in persons.RemoveDuplicateReferences())
                {
                    _personDal.Delete(dupePerson.Person.PersonId);

                }

                foreach (var personDto in persons)
                {
                    var person = context.Persons.FirstOrDefault(p => p.Person_id == personDto.Person.PersonId);

                    if (person != null)
                        context.MarriageMapWitness.Add(new MarriageMapWitness
                        {
                            Persons = person,
                            Marriages = mToUpDate,
                            WitnessNote = personDto.Description
                        });
                }

                context.SaveChanges();
            }
        }

        public void DeleteWitnessesForMarriage(Guid marriageId)
        {

            using (var context = new GeneralModelContainer())
            {
                foreach (
                    var mmw in
                        context.MarriageMapWitness.Where(m => m.Marriages.Marriage_Id == marriageId).ToList())
                {
                    context.MarriageMapWitness.Remove(mmw);

                    if (mmw.Persons != null)
                    {
                        if (
                            context.MarriageMapWitness.Count(o => o.Persons.Person_id == mmw.Persons.Person_id) ==
                            0)
                            context.Persons.Remove(mmw.Persons);
                    }
                }

                context.SaveChanges();
            }
        }
 


    }
}
