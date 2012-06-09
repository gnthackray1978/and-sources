using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.EntityModel;
using TDBCore.Types;

namespace TDBCore.BLL
{
    public class MarriageWitnessesBLL : BaseBLL
    {
        public MarriageWitnessesBLL()
        { 
            
        }

        public void MarkWitnessesAsDeleted(Guid marriageId)
        {
            foreach (Person _person in ModelContainer.MarriageMapWitnesses.Where(m => m.Marriage.Marriage_Id == marriageId).Select(p => p.Person))
            {
                _person.IsDeleted = true;
            }
            
            ModelContainer.SaveChanges();
        }

        public List<Person> GetWitnessesForMarriage(Guid marriageId)
        {
            return ModelContainer.MarriageMapWitnesses.Where(m => m.Marriage.Marriage_Id == marriageId).Select(p => p.Person).ToList();
        }

        public string GetWitnesseStringForMarriage(Guid marriageId)
        {           
            return string.Join(" ",ModelContainer.MarriageMapWitnesses.Where(m => m.Marriage.Marriage_Id == marriageId).Select(p => p.Person.ChristianName + " " + p.Person.Surname).ToArray());
        }

        public void UpdateWitnessesForMarriage(int witnessForMarriageId, Guid personId, Guid MarriageId)
        {
            MarriageMapWitness mmw = ModelContainer.MarriageMapWitnesses.Where(m => m.MarriageMapWitnessIndex == witnessForMarriageId).FirstOrDefault();

            Marriage mToUpDate = ModelContainer.Marriages.Where(m => m.Marriage_Id == MarriageId).FirstOrDefault();

            Person pToUpdate = ModelContainer.Persons.Where(p => p.Person_id == personId).FirstOrDefault();

            if (mmw != null && mToUpDate!= null && pToUpdate != null)
            {
                mmw.Marriage = mToUpDate;
                mmw.Person = pToUpdate;

            }

            ModelContainer.SaveChanges();
        }

        public void InsertWitnessesForMarriage(Guid marriageId, IList<Person> persons)
        {
            Marriage mToUpDate = ModelContainer.Marriages.Where(m => m.Marriage_Id == marriageId).FirstOrDefault();

            DeleteWitnessesForMarriage(marriageId);

            foreach (var dupePerson in persons.RemoveDuplicateReferences())
            {
               // Console.WriteLine(dupePerson.ChristianName + " " + dupePerson.Surname);

                ModelContainer.Persons.DeleteObject(dupePerson);

            }

            foreach (Person _person in persons)
            {
                MarriageMapWitness newMMW = new MarriageMapWitness();
                newMMW.Marriage = mToUpDate;
                newMMW.Person = _person;

                ModelContainer.MarriageMapWitnesses.AddObject(newMMW);

            }


            ModelContainer.SaveChanges();

        }

        public void DeleteWitnessesForMarriage(Guid marriageId)
        {
            foreach (var mmw in ModelContainer.MarriageMapWitnesses.Where(m => m.Marriage.Marriage_Id == marriageId).ToList())
            {
                Person _person = mmw.Person;
                ModelContainer.MarriageMapWitnesses.DeleteObject(mmw);

                if(ModelContainer.MarriageMapWitnesses.Where(o=>o.Person.Person_id == _person.Person_id ).Count() == 0)
                    ModelContainer.Persons.DeleteObject(_person);
            }
            
            ModelContainer.SaveChanges();


        }

        public void DeleteWitnessEntriesForMarriage(Guid marriageId)
        {
            foreach (var mmw in ModelContainer.MarriageMapWitnesses.Where(m => m.Marriage.Marriage_Id == marriageId).ToList())
            {
                ModelContainer.MarriageMapWitnesses.DeleteObject(mmw);
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
            else
            {
                Guid returnId = Guid.Empty;
                marriageId = returnId;
                return "";
            }

        }


    }
}
