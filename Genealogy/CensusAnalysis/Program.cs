using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using TDBCore.BLL;
using TDBCore.Types.libs;
using WebRipper;
using TDBCore.Types;

namespace CensusAnalysis
{
    public class Program
    {
 

        static void Main(string[] args)
        {
            string filePath = @"C:\Users\george\Documents\Visual Studio 2010\Projects\and-source\and-source\Genealogy\CensusAnalysis\";

            List<Persons> persons = new List<Persons>();

            XmlSerializer deserializer = new XmlSerializer(typeof(List<Persons>));
            TextReader textReader = new StreamReader(filePath + "persons.xml");

            persons = (List<Persons>)deserializer.Deserialize(textReader);

            textReader.Close();

            ParishsBll _parishBll = new ParishsBll();


            var pids = File.ReadAllLines(filePath + @"pids.txt");

            SourceBll sources = new SourceBll();
            SourceMappingsBll sourceMappingsBLL = new SourceMappingsBll();
            SourceMappingParishsBll sourceMappingParishsBLL = new SourceMappingParishsBll();
            DeathsBirthsBll deathsBirthsBLL = new DeathsBirthsBll();

            int idx =0;

            foreach (Persons p in persons) {


                if (pids[idx].ToGuid() != Guid.Empty)
                {
                    string description = "";
                    description += "1841 Census_entry" + Environment.NewLine + Environment.NewLine +
                        "Address " + p.Address + Environment.NewLine +
                        "Civil Parish" + p.Civil_Parish + Environment.NewLine +
                        "County" + p.County + Environment.NewLine +
                        "Municipal Borough" + p.Municipal_Borough + Environment.NewLine +
                        "Registration District" + p.Registration_District + Environment.NewLine +
                        "Page" + p.Page + Environment.NewLine +
                        "Piece" + p.Piece ;

                    Guid newSource = sources.InsertSource2(description, "", false, true, true, 1, "1841", "1841", 1841, 1841, "1841Census-" + p.Civil_Parish, 0, "");

                    if (newSource != Guid.Empty && pids[idx].ToGuid() != Guid.Empty)
                    {
                        sourceMappingsBLL.Insert(newSource, null, null, 1, null, DateTime.Today.ToShortDateString(), 84);
                        sourceMappingParishsBLL.InsertSourceMappingParish2(pids[idx].ToGuid(), newSource, 1);
                        foreach (var _person in p.personList)
                        {
                            var addedPerson = deathsBirthsBLL.CreateBasicPerson(_person.FirstName, _person.Surname, "", _person.BirthYear);
                            sourceMappingsBLL.Insert(newSource, null, null, 1, addedPerson.Person_id, DateTime.Today.ToShortDateString(), null);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("invalid source");                       
                    }
                }
                else
                {
                    Debug.WriteLine("invalid parish");                   
                }
                idx++;
            }

            Console.WriteLine("finished: " + persons.Count.ToString());
            Console.ReadKey();
        }
    }


}
