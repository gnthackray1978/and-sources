using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using TDBCore.BLL;

using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ToolsCore
{

    public static class Extensions
    {

        public static string GetKeyValue(this string[] str, int key)
        {
            string retVal = "";

            if (str.Length > key)
            {
                retVal = str[key];
            }

            return retVal;
        }

        public static string GetKeyValue(this List<string> str, int key)
        {
            string retVal = "";

            if (str.Count > key)
            {
                retVal = str[key];
            }

            return retVal;
        }

        public static Guid ToGuid(this string str)
        {

            if (str == null) str = "";

            str = str.Trim();


            Guid retVal = Guid.Empty;

            try
            {
                retVal = new Guid(str);
            }
            catch
            {
                retVal = Guid.Empty;
            }



            return retVal;
        }


        public static int ToInt(this string str)
        {

            //string retVal = "";
            int retVal = 0;

            if (str == null) str = "";

            str = str.Trim();


            Regex regex = new Regex(@"\d\d\d\d");

            MatchCollection mc = regex.Matches(str);

            if (mc.Count > 0)
            {
                string number = mc[0].Value;

                Int32.TryParse(number, out retVal);
            }


            return retVal;
        }


        public static FolderDescriptor ToFolderDescriptor(this string str)
        {
            List<string> parts = str.Split('_').ToList();

            FolderDescriptor returnFolderDescriptor = new FolderDescriptor();

            int idx = 0;
            while (idx < parts.Count)
            {
                parts[idx] = parts[idx].Replace('!', ' ');
                idx++;
            }

            returnFolderDescriptor.Date = parts.GetKeyValue(0);

            returnFolderDescriptor.FullName = parts.GetKeyValue(1);

            List<string> locationParts = parts.GetKeyValue(2).Split(' ').ToList();

            returnFolderDescriptor.Location = locationParts.GetKeyValue(0);
            returnFolderDescriptor.LocationCounty = locationParts.GetKeyValue(1);
  
            returnFolderDescriptor.Type = parts.GetKeyValue(3);
             
            returnFolderDescriptor.MiscInner = parts.GetKeyValue(4);

            returnFolderDescriptor.MiscOuter = parts.GetKeyValue(5);

            return returnFolderDescriptor;

        }
    }



    
    public static class Tools
    { 

        public static List<KeyType> ReadCSVKey(string CSVPath)
        {
            //"sourceid""Source","Date","dateto","Testator_CName","Testator_SName","Location","Occupation","Relationship","ChristianName","Surname","Occupation",

            List<KeyType> keyTypes = new List<KeyType>();


            if (File.Exists(CSVPath))
            {
                var lines = File.ReadAllLines(CSVPath).ToList();


                lines.ForEach(l =>
                {
                    var line = l.Split(',');

                    KeyType keyType = new KeyType()
                    {
                        SourceId = line.GetKeyValue(0).ToGuid(),
                        SourceRef = line.GetKeyValue(1),
                        PhysicalPath = line.GetKeyValue(2),
                        SourceType = line.GetKeyValue(3),
                        SourceDateFrom = line.GetKeyValue(4),
                        SourceDateTo = line.GetKeyValue(5),

                        Location = line.GetKeyValue(6),

                        County = line.GetKeyValue(7),

                        SubjectChristianName = line.GetKeyValue(8),
                        SubjectSurname = line.GetKeyValue(9),
                        SubjectOccupation = line.GetKeyValue(10),
                        SubjectRelation = line.GetKeyValue(11),

                        OthersideChristianName = line.GetKeyValue(12),
                        OthersideSurname = line.GetKeyValue(13),
                        OthersideOccupation = line.GetKeyValue(14)


                    };

                    keyTypes.Add(keyType);
                }
                );


                //foreach (var value in values)
                //{
                //    Console.WriteLine(string.Format("Column '{0}', Sum: {1}, Average {2}", value.FirstColumn, value.Values.Sum(), value.Values.Average()));
                //}

            }
            else
            {
                Console.WriteLine("couldnt find the csv at: " + CSVPath);
            }


            return keyTypes;
        }
        
   

        public static string MakeSourceRef(FolderDescriptor fdesc, string misc)
        {
            List<string> nameParts = fdesc.FullName.Split(' ').ToList();
            string sourceRef = "";

            switch (fdesc.Type.ToLower())
            {
                case "will":
                    sourceRef = fdesc.Date + nameParts.GetKeyValue(0) + nameParts.GetKeyValue(1) + "Will";
                    break;
                case "deed":
                    sourceRef = fdesc.Date + nameParts.GetKeyValue(0) + nameParts.GetKeyValue(1) + "Deed";
                    break;
                case "bond":
                    sourceRef = fdesc.Date + nameParts.GetKeyValue(1) + nameParts.GetKeyValue(3) + "Bond";
                    break;
                default:
                    sourceRef = fdesc.Date + nameParts.GetKeyValue(0) + nameParts.GetKeyValue(1) + "Misc" + misc;
                    break;
            }

            return sourceRef;
        }



    }
}
