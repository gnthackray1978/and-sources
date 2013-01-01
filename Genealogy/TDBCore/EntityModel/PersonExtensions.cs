using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.BLL;
using TDBCore.Types;
using GedItter.BLL;
using GedItter.BirthDeathRecords.BLL;
using GedItter;


namespace TDBCore.EntityModel
{
    public static class PersonExtensions
    {




        public static void MergeInto(this Person _person, Person newPerson)
        {
            DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();

            Guid dummyLocation = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699");

            if (_person.SpouseSurname == "")
                _person.SpouseSurname = newPerson.SpouseSurname;

            if (_person.SpouseName == "")
                _person.SpouseName = newPerson.SpouseName;


            if ((_person.ReferenceLocationId == dummyLocation || _person.ReferenceLocationId == Guid.Empty) &&
                newPerson.ReferenceLocationId != dummyLocation && newPerson.ReferenceLocationId != Guid.Empty)
                _person.ReferenceLocationId = newPerson.ReferenceLocationId;

            if ((_person.DeathLocationId == dummyLocation || _person.ReferenceLocationId == Guid.Empty) && 
                newPerson.DeathLocationId != dummyLocation && newPerson.ReferenceLocationId != Guid.Empty)
                _person.DeathLocationId = newPerson.DeathLocationId;

            if ((_person.BirthLocationId == dummyLocation || _person.ReferenceLocationId == Guid.Empty) &&
                newPerson.BirthLocationId != dummyLocation && newPerson.ReferenceLocationId != Guid.Empty)
                _person.BirthLocationId = newPerson.BirthLocationId;



            if (newPerson.ReferenceLocation != "" && _person.ReferenceLocation == "")
                _person.ReferenceLocation = newPerson.ReferenceLocation;

            if (newPerson.ReferenceDateStr != "" && _person.ReferenceDateStr == "")
                _person.ReferenceDateStr = newPerson.ReferenceDateStr;



            if (newPerson.Occupation != "" && _person.Occupation == "")
            {
                if (_person.Occupation.Trim() == "")
                {
                    _person.Occupation = newPerson.Occupation;
                }
                else
                {
                    _person.Occupation += " " + newPerson.Occupation;
                }
            }


            if (newPerson.Notes != "")
            {
                if (_person.Notes.Trim() == "")
                {
                    _person.Notes = newPerson.Notes;
                }
                else
                {
                    _person.Notes += " " + newPerson.Notes;
                }

            }

            if (newPerson.MotherSurname != "" && _person.MotherSurname == "")
                _person.MotherSurname = newPerson.MotherSurname;

            if (newPerson.MotherChristianName != "" && _person.MotherChristianName == "")
                _person.MotherChristianName = newPerson.MotherChristianName;

            if (newPerson.FatherChristianName != "" && _person.FatherChristianName == "")
                _person.FatherChristianName = newPerson.FatherChristianName;

            if (newPerson.FatherOccupation != "" && _person.FatherOccupation == "")
                _person.FatherOccupation = newPerson.FatherOccupation;

            if (newPerson.DeathCounty != "" && _person.DeathCounty == "")
                _person.DeathCounty = newPerson.DeathCounty;

            if (newPerson.DeathDateStr != "" && _person.DeathDateStr == "")
                _person.DeathDateStr = newPerson.DeathDateStr;

            if (newPerson.ReferenceDateInt > 0 && _person.ReferenceDateInt == 0)
                _person.ReferenceDateInt = newPerson.ReferenceDateInt;

            if (newPerson.DeathInt > 0 && _person.DeathInt == 0)
                _person.DeathInt = newPerson.DeathInt;

            if (newPerson.BirthInt > 0 && _person.BirthInt == 0)
                _person.BirthInt = newPerson.BirthInt;

            if (newPerson.BapInt > 0 && _person.BapInt == 0)
                _person.BapInt = newPerson.BapInt;


            if (newPerson.DeathLocation != "" && _person.DeathLocation == "")
                _person.DeathLocation = newPerson.DeathLocation;

            if (newPerson.BirthCounty != "" && _person.BirthCounty == "")
                _person.BirthCounty = newPerson.BirthCounty;

            if (newPerson.BirthDateStr != "" && _person.BirthDateStr == "")
                _person.BirthDateStr = newPerson.BirthDateStr;

            if (newPerson.BirthLocation != "" && _person.BirthLocation == "")
                _person.BirthLocation = newPerson.BirthLocation;

            if (newPerson.BaptismDateStr != "" && _person.BaptismDateStr == "")
                _person.BaptismDateStr = newPerson.BaptismDateStr;

            _person.IsMale = newPerson.IsMale;

            string source = _person.Source + Environment.NewLine + deathsBirthsBll.MakeSourceString(newPerson.Person_id);


           // if(source.Length >49)
                _person.Source = "Multiple sources";
          //  else
           //     _person.Source = source;




            int estBYear = 0;
            int estDYear =0;
            bool isEstBYear =false;
            bool isEstDYear = false;

            CsUtils.CalcEstDates(_person.BirthInt, _person.BapInt, _person.DeathInt, out estBYear, out estDYear, out isEstBYear, out isEstDYear, _person.FatherChristianName, _person.MotherChristianName);

            _person.EstBirthYearInt = estBYear;
            _person.EstDeathYearInt = estDYear;
            _person.IsEstBirth = isEstBYear;
            _person.IsEstDeath = isEstDYear;

        }

        public static IEnumerable<Person> RemoveDuplicateReferences(this IList<Person> list)
        {
            TDBCore.Types.EqualityComparer<Person> ec_p = new TDBCore.Types.EqualityComparer<Person>((o1, o2) => o1.ChristianName == o2.ChristianName
                && o1.Surname == o2.Surname
                && o1.ReferenceDateStr == o2.ReferenceDateStr,
                o => (o.ReferenceDateStr.GetHashCode() + o.Surname.GetHashCode() + o.ChristianName.GetHashCode()));

            IList<Person> p = new List<Person>();
            IList<Person> dupes = new List<Person>();

            int idx = 0;

            while (idx < list.Count)
            {

                if (!p.Contains(list[idx], ec_p))
                {
                    p.Add(list[idx]);
                    idx++;
                }
                else
                {
                    dupes.Add(list[idx]);
                    list.Remove(list[idx]);

                }


            }

            return dupes;

        }

        public static List<string> CheckForRelations(this Person _person)
        {
            RelationsBLL rbll = new RelationsBLL();
            string retStr = "";
            List<string> relations = new List<string>();

            foreach (var rel in rbll.GetRelationByChildOrParent(_person.Person_id).GroupBy(g=>g.RelationType.RelationName))
            {
              
                retStr = "";
                foreach(var group in rel)
                {
                    if (group.PersonA.Person_id == _person.Person_id)
                    {
                        retStr = " " + group.PersonB.ChristianName + " " + group.PersonB.Surname;
                       // rel.PersonB
                    }
                    else
                    {
                        retStr = " " + group.PersonA.ChristianName + " " + group.PersonA.Surname;
                    }


                }

                retStr = rel.Key + retStr;
                relations.Add(retStr);

                //rel.RelationType.RelationName
            }

            return relations;
        }



        public static string CheckForWills(this Person _person, out Guid sourceId)
        {
            SourceBLL sourecBll = new SourceBLL();
            string description = "";

            Guid willSource = Guid.Empty;

            foreach (SourceMapping sm in _person.SourceMappings)
            {
                //  Debug.WriteLine(sm.Source.SourceRef);
                // 1,2, 11-33
                foreach (SourceMapping sm2 in sm.Source.SourceMappings)
                {
                    if (sm2.SourceType != null)
                    {
                        if (sm2.SourceType.SourceTypeId == 1 || sm2.SourceType.SourceTypeId == 2
                            || (sm2.SourceType.SourceTypeId >= 11 && sm2.SourceType.SourceTypeId <= 33))
                        {
                            description = sm2.SourceType.SourceTypeDesc + " " + sm2.Source.SourceRef + ":";

                            willSource = sm2.Source.SourceId;
                        }
                    }
                }
            }


            int idx = 0;
            foreach (Person _person0 in sourecBll.GetPersonsForSource(willSource))
            {
                if (_person0 != null)
                {
                    if (idx != 0)
                        description += ",";

                    description += _person0.ChristianName + " " + _person0.Surname;
                    idx++;
                }
               
                
            }

            sourceId = willSource;
            return description;
        }

        public static string CheckForMarriage(this Person _person, out Guid marriageId)
        {
            string description = "";

            MarriageWitnessesBLL mwitbll = new MarriageWitnessesBLL();

            description = mwitbll.GetWitnessMarriageDesc(_person.Person_id,out marriageId);


            return description;
        }



        public static string ToBirthString(this Person _person)
        {
            string description = "";

            if(_person.FatherChristianName != "" && _person.MotherChristianName != "")
                description = " Child of " + _person.FatherChristianName + " " + _person.FatherSurname + " and " + _person.MotherChristianName + " " + _person.MotherSurname;

            if (_person.FatherChristianName != "" && _person.MotherChristianName == "")
                description = " Child of " + _person.FatherChristianName + " " + _person.FatherSurname;

            if (_person.FatherChristianName == "" && _person.MotherChristianName != "")
                description = " Child of " + _person.MotherChristianName + " " + _person.MotherSurname;



            return description;
        }

        public static string ToSpouseString(this Person _person)
        {
            string description = "";


            description = " Spouse of " + _person.ChristianName + " " + _person.Surname;



            return description;
        }

        public static string ToDeathString(this Person _person)
        {
            string description = "";

            if (_person.FatherChristianName != "" && _person.MotherChristianName != "")
                description = " Child of " + _person.FatherChristianName + " " + _person.FatherSurname + " and " + _person.MotherChristianName + " " + _person.MotherSurname;

            if (_person.FatherChristianName != "" && _person.MotherChristianName == "")
                description = " Child of " + _person.FatherChristianName + " " + _person.FatherSurname;

            if (_person.FatherChristianName == "" && _person.MotherChristianName != "")
                description = " Child of " + _person.MotherChristianName + " " + _person.MotherSurname;

            if (_person.BirthInt > 0 || _person.BapInt > 0)
            {
                if (_person.BapInt > 0)
                {
                    description = "Born: " + _person.BaptismDateStr + " " + description;
                }
                else
                {
                    description = "Born: " + _person.BirthDateStr + " " + description;
                }

            }

            if (_person.Occupation != "")
                description += " Occupation: " + _person.Occupation;


            if(_person.SpouseName != "")
                description += " Spouse: " + _person.SpouseName + " " + _person.SpouseSurname;

            return description.Trim();
        }

        public static string ToMotherString(this Person _person)
        {
            string description = "";

            if (_person.FatherChristianName != "")
            {
                description = " And " + _person.FatherChristianName + " Parents of " + _person.ChristianName;
            }
            else
            {
                description = " Mother of " + _person.ChristianName;
            }

            return description;
        }

        public static string ToFatherString(this Person _person)
        {
            string description = "";

            if (_person.MotherChristianName != "")
            {
                description = " And " + _person.MotherChristianName + " Parents of " + _person.ChristianName;
            }
            else
            {
                description = " Father of " + _person.ChristianName;
            }

            return description;
        }
    }
}
