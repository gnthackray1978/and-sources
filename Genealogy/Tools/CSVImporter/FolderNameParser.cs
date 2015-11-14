using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using TDBCore.Types.libs;

namespace CSVImporter
{
    public class FolderNameParser
    {
        private readonly string _rawData = "";
        private int _year = 0;
        private int _yearTo = 0;
        private string _date = "";
        private string _dateTo = "";
        private string _christianName = "";
        private string _surname = "";
        private string _othersideChristianName = "";
        private string _othersideSurname = "";
        private string _location = "";
        private string _description = "";
        private string _type = "";



        public FolderNameParser(string folderName, string parent)
        {
            _rawData = folderName;
            _type = parent;
            ParseDate();
            ParseDescription();
            ParseLocation();
            ParseNames();
        }

        public Source GetSource()
        {
            var plist = new List<Person>
            {
                new Person()
                {
                    ChristianName = _christianName,
                    Surname = _surname,
                    OthersideChristianName = _othersideChristianName,
                    OthersideSurname = _othersideSurname

                }
            };

            if (_othersideChristianName.Length > 0 || _othersideSurname.Length > 0)
            {
                plist.Add(new Person()
                {
                    ChristianName = _othersideChristianName,
                    Surname = _othersideSurname,
                    OthersideChristianName = _christianName,
                    OthersideSurname = _surname
                });
            }
            
            return new Source()
            {
                SourceRef = _rawData.Replace('!',' ').Replace('_',' '),
                From = _date,
                To = _date,
                FromYear = _year,
                ToYear = _year,
                Location = _location,
                PhysicalPath = _rawData,
                Type = _type,
                LinkedPersons = plist
            };
        }

        private void ParseDate()
        {
            var cols =_rawData.Split('_');

            if (cols.Any())
            {
                if (cols[0].Contains("!"))
                {
                    var dates = cols[0].Split('!');

                    _year = dates[0].ToInt32();
                    _date = dates[0];
                    _yearTo = dates[1].ToInt32();
                    _dateTo = dates[1];
                }
                else
                {
                    _year = cols[0].ToInt32();
                    _date = cols[0];
                    _yearTo = cols[0].ToInt32();
                    _dateTo = cols[0];
                }
                
            }

        }

        private void ParseNames()
        {
            var cols = _rawData.Split('_');

            if (cols.Length <= 1) return;

            var nameParts = cols[1].Split('!');

            if(nameParts.Length > 0) _christianName = nameParts[0];
            if(nameParts.Length > 1) _surname = nameParts[1];
            if(nameParts.Length > 2) _othersideChristianName = nameParts[2];
            if(nameParts.Length > 3) _othersideSurname = nameParts[3];
        }

        private void ParseLocation()
        {
            var cols = _rawData.Split('_');

            if (cols.Length > 2)
            {
                _location = cols[2];
            }
        }

        private void ParseDescription()
        {
            var cols = _rawData.Split('_');

            if (cols.Length > 3)
            {
                _description = cols[3];
            }
        }
    }
}
