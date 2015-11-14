using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TDBCore.Types.domain.import;
using TDBCore.Types.libs;

namespace CSVImporter
{
    public class SourcePersonReader
    {
        List<Source> _sources = new List<Source>();
        private string _googlePath = "";

        public List<Source> Sources
        {
            get { return _sources; }
        }

        public SourcePersonReader(string path)
        {
            _googlePath = path;
        }

        public void ReadSources()
        {

            ImportSourcePersonsCSVFromFile(_googlePath);
        }


        private void ImportSourcePerson(string[] allLines)
        {
            ////SourceId	sourceRef	PhysicalPath	Type	From	To	Location	County
            foreach (var l in allLines)
            {
                var cols = l.Split(',').ToList();


                var path = cols[2];

                try
                {
                    var dirName = new DirectoryInfo(path).Name;
                    path = dirName;
                }
                catch (Exception e)
                {
                     Debug.WriteLine(e.Message);
                }


                if (!_sources.Exists(s => s.PhysicalPath == path))
                {
                    _sources.Add(new Source()
                    {
                        SourceId = cols[0].ToGuid(),
                        SourceRef = cols[1],
                        PhysicalPath = path,
                        Type = cols[3],
                        From = cols[4],
                        FromYear = cols[4].ParseToValidYear(),
                        To = cols[5],
                        ToYear = cols[5].ParseToValidYear(),
                        Location = cols[6] + " " + cols[7]

                    });
                }

                var idx = _sources.FindIndex(o => o.PhysicalPath == path);

                //SubjectChristianName	SubjectSurname	SubjectOccupation	SubjectRelation	OthersideChristianName	OthersideSurname	OthersideOccupation



                _sources[idx].LinkedPersons.Add(new Person()
                {
                    ChristianName = cols[8],
                    Surname = cols[9],
                    Occupation = cols[10],
                    Relation = cols[11],
                    OthersideChristianName = cols[12],
                    OthersideSurname = cols[13],
                    OthersideOccupation = cols[14]
                });

            }
        }

        private void ImportSourcePersonsCSVFromFile(string path)
        {
            string csv = new WebClient().DownloadString(path);

            var lineList = csv.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            lineList.RemoveAt(0);

            ImportSourcePerson(lineList.ToArray());
        }

        public void UpdateSources(Dictionary<string,Guid> keys)
        {

            foreach (var s in _sources.Where(s=>s.SourceId == Guid.Empty))
            {
                if (keys.ContainsKey(s.PhysicalPath))
                {
                    s.SourceId = keys[s.PhysicalPath];
                    Debug.WriteLine("updating " + s.PhysicalPath);
                }
            }

        }

        public void AddMissingSources(Dictionary<string, Guid> keys,Dictionary<string, string> types)
        {
            //make list of everything thats not in there already.


            foreach (var k in keys)
            {
                if (_sources.Exists(o => o.SourceId == k.Value)) continue;

      

                var fnp = new FolderNameParser(k.Key,types[k.Key]);
                var newSource = fnp.GetSource();

                newSource.SourceId = k.Value;

                _sources.Add(newSource);
            }
        }

        public List<string> DumpToCSV()
        {
            var csvRows = new List<string>();

            foreach (var s in _sources)
            {

                if (s.LinkedPersons.Count > 0)
                {
                    csvRows.AddRange(s.LinkedPersons.Select(p => s.ToCSVString(true) + p.ToCSVString(false)));
                }
                else
                {
                    csvRows.Add(s.ToCSVString(true) + ",,,,,,");
                }
            }

            return csvRows;
        }


    }
}
