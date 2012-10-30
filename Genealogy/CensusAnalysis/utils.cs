using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WebRipper
{

    public class Importer
    {

        private string folderPath =

            @"C:\Users\george\Documents\Visual Studio 2010\Projects\WebRipper\WebRipper\htmlpages\";

        private List<Persons> personsRecord = new List<Persons>();

        public List<Persons> PersonsRecord
        {
            get { return personsRecord; }
            set { personsRecord = value; }
        }



        public void Import()
        {

            var di = new DirectoryInfo(folderPath);


            int idx =0;
            foreach (var file in di.GetFiles().Where(o => o.Extension == ".html"))
            {
                //Debug.WriteLine(idx.ToString());
                var newPerson = FillPersons(file.Name);


                Debug.WriteLine(idx);

                if (!personsRecord.ContainsEx(newPerson))
                    personsRecord.Add(newPerson);

                idx++;
             //   Debug.WriteLine(personsRecord.Count.ToString());
            }

           
        }



        private Persons FillPersons(string fileName)
        {

            string contents = File.ReadAllText(folderPath + fileName);



            string names_pattern = @"<DIV class=""clear component table"".*?<section class=copy>";



            Regex regex = new Regex(names_pattern, RegexOptions.Singleline);

            var names = regex.Match(contents);



       //     Debug.WriteLine(names);



            Persons _newPersons = new Persons();





            AddPersonsData(contents, _newPersons);

            AddPersonData(names.Value, _newPersons);



            //   personsRecord.Add(_newPersons);



            return _newPersons;



            // personsRecord.Contains(_newPersons, )



        }



        private static void AddPersonData(string names, Persons _newPersons)
        {

            string row_pattern = @"<TR.*?</TR>";
        
            string col_pattern = @"(?<= (<TD class=third>) |(<TD class=half>)|(<TD>)|(<TD class=narrower>)).*?(?=</TD>)";



            Regex regex = new Regex(row_pattern, RegexOptions.Singleline);

            var rows = regex.Matches(names);





            foreach (Match _match in rows)
            {

                regex = new Regex(col_pattern, RegexOptions.Singleline);

                var cols = regex.Matches(_match.Value);

                if (cols.Count > 0)
                {

                    Person _person = new Person();



                    var nameparts = cols[0].Value.Split(',');



                    if (nameparts.Length > 0)
                    {

                        _person.FirstName = cols[0].Value.Replace(nameparts[0], "").Replace(",", "").Trim();

                        _person.Surname = nameparts[0].Trim();

                    }

                    else
                    {

                        _person.Surname = cols[0].Value;

                    }



                    _person.Sex = cols[1].Value;

                    int tp = 0;

                    Int32.TryParse(cols[2].Value, out tp);

                    _person.Age = tp;

                    Int32.TryParse(cols[3].Value, out tp);

                    _person.BirthYear = tp;

                    _person.BirthCounty = cols[5].Value;

                    _newPersons.personList.Add(_person);

                }

            }





        }



        private static void AddPersonsData(string contents, Persons _newPersons)
        {



            string addelements_pattern = @"<P>.*?</P>";



            string add_pattern = @"<section class=copy>.*?</section>";



            Regex regex;

            regex = new Regex(add_pattern, RegexOptions.Singleline);

            var add = regex.Match(contents).Value;



            regex = new Regex(addelements_pattern, RegexOptions.Singleline);

            var addelements = regex.Matches(add);

            foreach (Match _match in addelements)
            {

                string type_pattern = @"(?<=<SPAN class=detail>).*?(?=:</SPAN>)";



                regex = new Regex(type_pattern, RegexOptions.Singleline);



                var elementType = regex.Match(_match.Value).Value;



                string rec_pattern = @"(?<=<SPAN class=record>).*?(?=</SPAN>)";



                regex = new Regex(rec_pattern, RegexOptions.Singleline);



                var recValue = regex.Match(_match.Value).Value;

                //Debug.WriteLine(elementType.Value);



                if (elementType == "Address") _newPersons.Address = recValue.Trim();

                if (elementType == "County") _newPersons.County = recValue.Trim();

                if (elementType == "Piece") _newPersons.Piece = recValue.Trim();

                if (elementType == "Book/Folio") _newPersons.Book = recValue.Trim();

                if (elementType == "Page") _newPersons.Page = recValue.Trim();

                if (elementType == "Registration District") _newPersons.Registration_District = recValue.Trim();

                if (elementType == "Civil Parish") _newPersons.Civil_Parish = recValue.Trim();

                if (elementType == "Municipal Borough") _newPersons.Municipal_Borough = recValue.Trim();

            }

        }

    }

    public class Persons
    {

        public string Address { get; set; }

        public string County { get; set; }

        public string Piece { get; set; }

        public string Book { get; set; }



        public string Page { get; set; }

        public string Registration_District { get; set; }

        public string Civil_Parish { get; set; }

        public string Municipal_Borough { get; set; }





        public List<Person> personList = new List<Person>();





        public string HashCode()
        {

            var elements = personList.OrderBy(o => o.Age).ThenBy(p => p.FirstName).ThenBy(q => q.Surname);

         

            string nameshash = elements.Aggregate("", (current, _p) => current + (_p.Age + _p.BirthYear + _p.FirstName.PadRight(2,'x').Substring(0, 2) + _p.Surname.Substring(0, 3)));



            nameshash += Address.Substring(0, 3) + County.Substring(0, 3) + Registration_District.Substring(0, 3) +

                         Civil_Parish.Substring(0, 3);



            return nameshash;

        }

    }

    public class Person
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int BirthYear { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string BirthCounty { get; set; }
    }

    public static class MyExtensions
    {
        public static bool ContainsEx(this List<Persons> plist, Persons newPersons)
        {
            foreach (var persons in plist)
            {

                Debug.WriteLine(persons.GetHashCode());

                if (persons.HashCode() == newPersons.HashCode())
                {
                    Debug.WriteLine(persons.GetHashCode());
                    return true;
                }

            }
            return false;

        }

    }  

}
