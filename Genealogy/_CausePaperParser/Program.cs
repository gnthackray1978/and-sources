using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _CausePaperParser
{

    public class PersonRec
    {
        public string surname { get; set; }
        public string cname { get; set; }

        public string role { get; set; }
        public string employment { get; set; }

        public string details { get; set; }

        public string location { get; set; }

        public string age { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var text  = File.ReadAllLines(@"content.txt");

            var person = new PersonRec();
            var personContainer = new List<PersonRec>();

            foreach (var l in text.Where(l=>l.Trim()!= ""))
            {

                var participant = GetTag(@"Participant", l);

                if (participant != "")
                {
                    person = new PersonRec();
                    personContainer.Add(person);
                    person.location = "";

                    var nameParts = participant.Split(' ').Where(p => p.Trim() != "").ToList();

                    if (nameParts.Count > 0) person.cname = nameParts[0];
                    if (nameParts.Count > 1) person.surname = nameParts[1];

                }


                var role = GetTag(@"Role", l);
                if (role != "") person.role = role;

                var employment = GetTag(@"Employment", l);
                if (employment != "") person.employment = employment;

                var details = GetTag(@"Details", l);
                if (details != "")
                {
                    person.details = details;

                    person.age = GetAge(person.details);

                    var dets = person.details.Split(';');

                    if (dets.Length > 2)
                    {
                        person.employment = dets[2].Trim();

                        person.details = person.details.Replace(person.employment, "");
                    }

                    if (person.age != "")
                    {
                        person.details = person.details.Replace(person.age, "");
                    }

                    if (person.details!= null)
                        person.details = person.details.Replace(";", "");
                }

                var location = GetTag(@"Location", l);
                if (location != "") person.location = location;                
            }

            foreach (var p in personContainer)
            {
                Debug.WriteLine(p.cname + "\t" + p.surname + "\t" + p.details + "\t" + p.employment + "\t" + p.location.Replace("(YorkshireEastRiding)", "") + "\t" + p.role + "\t" + p.age);
            }

            Console.WriteLine(personContainer.Count.ToString());
           // Console.ReadKey();
        }

        static string GetTag(string searchStr, string input)
        {

            var makePattern = @"(?<=" + searchStr + ":).*";

            var regex = new Regex(makePattern);

            var match = regex.Match(input);

            if (match.Success)
            {
                return match.Value;
            }
            
            return "";
            
        }

        static string GetAge(string input)
        {

            const string makePattern = @"\d\d";

            var regex = new Regex(makePattern);

            var match = regex.Match(input);

            if (match.Success)
            {
                return match.Value;
            }

            return "";

        }

    }
}
