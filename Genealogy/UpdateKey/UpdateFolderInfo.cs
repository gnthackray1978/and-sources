using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using GedItter.BLL;


namespace UpdateKey
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

    }


    public class KeyType
    {



        public Guid SourceId { get; set; }
        public string SourceRef { get; set; }
        public string SourceDateFrom { get; set; }
        public string SourceDateTo { get; set; }
        public string SourceType { get; set; }
        public string Location { get; set; }
        public string County { get; set; }

        public string SubjectChristianName { get; set; }
        public string SubjectSurname { get; set; }
        public string SubjectOccupation { get; set; }

        public string SubjectRelation { get; set; }
        public string OthersideChristianName { get; set; }
        public string OthersideSurname { get; set; }
        public string OthersideOccupation { get; set; }
        public string PhysicalPath { get; set; }


        public int Date {
            get
            {
                return this.SourceDateTo.ToInt();
            }
        
        }

        public int DateFrom
        {
            get
            {
                return this.SourceDateFrom.ToInt();
            }

        }

        public string WriteString()
        {
            return SourceId.ToString() + "," + SourceRef + "," + SourceType + "," + PhysicalPath + "," + SourceDateFrom + ","
                + SourceDateTo + "," + Location + "," + County + ","
                + SubjectChristianName + "," + SubjectSurname + "," + SubjectOccupation + "," + SubjectRelation +
                "," + OthersideChristianName + "," + OthersideSurname + "," + OthersideOccupation + "," + PhysicalPath;
        }

        public static string WriteHeaderString()
        {
            return "SourceId,sourceRef,Type,PhysicalPath,From,To,Location,County,SubjectChristianName,SubjectSurname,SubjectOccupation" + 
                ",SubjectRelation,OthersideChristianName,OthersideSurname,OthersideOccupation,PhysicalPath";
        }
    }

    public class UpdateFolderInfo
    {
        public string RootPath { get; set; }
        public string CSVPath { get; set; }
        private List<KeyType> keyTypes = new List<KeyType>();

        public void Run()
        {
            CSVPath = RootPath + @"\key.csv";

            DirectoryInfo first = new DirectoryInfo(RootPath);
            ReadCSVKey();
           // ProcessDir(first);

            processCSV();
        }



        private void ReadCSVKey()
        { 
            //"sourceid""Source","Date","dateto","Testator_CName","Testator_SName","Location","Occupation","Relationship","ChristianName","Surname","Occupation",



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
                Console.WriteLine("couldnt find the csv at: " + this.CSVPath);
            }
        }

        private void processCSV() 
        {

            SourceBLL sourceBll = new SourceBLL();
         

            int idx =0;

            // add any missing sources
            while (idx  < this.keyTypes.Count)
            {
                Console.Write("\r" + idx + " of " + this.keyTypes.Count);

                KeyType csvRow = this.keyTypes[idx];

                if (csvRow.SourceRef == "1730ThackerayFelliskirkBond")
                {
                    Debug.WriteLine("");
                }

                if (idx != 0)
                {
                    Guid newSource = csvRow.SourceId;


                    string path = RootPath + csvRow.PhysicalPath;

                    //Path.Combine(RootPath, csvRow.PhysicalPath);

                    DirectoryInfo di = new DirectoryInfo(path);
                    int fileCount = 0;
                    if (di.Exists)
                    {
                        fileCount = di.GetFiles().Count();
                    }


                    if (fileCount > 0)
                    {

                        TDBCore.EntityModel.Source source = null;

                        if (csvRow.SourceId != Guid.Empty)
                            source = sourceBll.FillSourceTableById2(csvRow.SourceId);

                        if (source == null ||
                            csvRow.SourceId == Guid.Empty)
                        {
                            // ok we need to add a source
                            // then we need to update the key with its value.

                            #region add source

                            string sourceDesc = csvRow.SubjectChristianName + " " + csvRow.SubjectSurname + " " + csvRow.SourceType;
                            string sourceOrigin = "";



                            if (csvRow.Location.ToLower().Contains("borthwick"))
                                sourceOrigin = "Borthwick Institute";

                            if (csvRow.Location.ToLower().Contains("durham"))
                                sourceOrigin = "Durham Record Office";

                            if (csvRow.Location.ToLower().Contains("hunsingore"))
                                sourceOrigin = "Hunsingore Peculiar at WYAS Leeds";

                            if (csvRow.Location.ToLower().Contains("huntingdon"))
                                sourceOrigin = "Huntingdon Record Office";

                            if (csvRow.Location.ToLower().Contains("knaresborough"))
                                sourceOrigin = "Knaresborough Peculiar at WYAS Leeds";

                            if (csvRow.Location.ToLower().Contains("london"))
                                sourceOrigin = "LMA";

                            if (csvRow.Location.ToLower().Contains("misc"))
                                sourceOrigin = "Unknown Origin";

                            if (csvRow.Location.ToLower().Contains("nonthackray"))
                                sourceOrigin = "Non Thackray record";

                            newSource = sourceBll.InsertSource2(sourceDesc, sourceOrigin, true, true, true, 1, csvRow.SourceDateFrom, 
                                csvRow.SourceDateTo, csvRow.DateFrom, csvRow.Date, csvRow.SourceRef, fileCount, "");


                            if (newSource != Guid.Empty)
                            {
                                // should we create a info file?

                                // if there is already a info file get rid of it, because its wrong.
                                foreach (FileInfo _file in di.GetFiles())
                                {
                                    if (_file.Extension.ToLower() == ".info")
                                    {
                                        _file.Delete();
                                    }
                                }



                                var sr = File.CreateText(Path.Combine(di.FullName, "info.info"));

                                sr.WriteLine(newSource.ToString());

                                sr.Close();

                                csvRow.SourceId = newSource;

                                sourceBll.ModelContainer.SaveChanges();
                            }
                            else
                            {
                                Debug.WriteLine(csvRow.SourceRef + "source couldnt be added");
                            }

                            #endregion
                        }



                        SourceMappingsBLL sourceMappingsBll = new SourceMappingsBLL();
                        FilesBLL filesBll = new FilesBLL();
                        
                        sourceMappingsBll.DeleteFilesForSource(csvRow.SourceId);

                        List<Guid> filesToAdd = new List<Guid>();

                        if (csvRow.PhysicalPath != null &&
                            csvRow.PhysicalPath != "")
                        {
                            foreach (FileInfo _file in di.GetFiles())
                            {
                                //_file.
                                filesToAdd.Add(filesBll.AddFile2(_file.Name, Path.Combine(csvRow.PhysicalPath, _file.Name), 1, ""));

                                if (_file.Name.ToLower() == "notes.txt")
                                {
                                    string contents = File.ReadAllText(_file.FullName);
                                    if (source != null)
                                    {
                                        source.SourceNotes = contents;


                                        sourceBll.ModelContainer.SaveChanges();
                                    }
                                    else
                                        Debug.WriteLine("didnt  write : " + _file.FullName);
                                }
                            }

                            DirectoryInfo admonDir = new DirectoryInfo(di.FullName + @"\admon");
                            DirectoryInfo willDir = new DirectoryInfo(di.FullName + @"\will");

                            if (admonDir.Exists)
                            {
                                foreach (FileInfo _file in admonDir.GetFiles())
                                {
                                    filesToAdd.Add(filesBll.AddFile2(_file.Name, Path.Combine(csvRow.PhysicalPath, @"\admon\" + _file.Name), 1, ""));
                                }
                            }

                            if (willDir.Exists)
                            {
                                foreach (FileInfo _file in willDir.GetFiles())
                                {
                                    filesToAdd.Add(filesBll.AddFile2(_file.Name, Path.Combine(csvRow.PhysicalPath, @"\will\" + _file.Name), 1, ""));
                                }
                            }

                            if (newSource != Guid.Empty && filesToAdd.Count > 0)
                                sourceMappingsBll.WriteFilesToSource(newSource, filesToAdd, 1);
                            else
                                Debug.WriteLine(csvRow.SourceRef + "no files to add or source is empty");
                        }
                        else
                        {
                            Debug.WriteLine(csvRow.SourceRef + "phy path empty");
                        }
                        //

                    }


                    this.keyTypes[idx] = csvRow;
                }


                idx++;
            }
            // ok so go through adding in images from source locations 
            // into mapping table.

            WriteNewCSV();


        }

        private  void ProcessDir(DirectoryInfo di )
        {
            // read through folders (each folder is taken to be a source)
            // identify folders without info file
            // if there is no info file then we dont have a sourceid and we assume its not in the db
            // - create info file with new source id

            // add folder information and source id into list
            //sort list by date and write out again


            // source unless specified otherwise default image folder is always the first sourcetype name (the lowest id) + sourceref





            Stack<DirectoryInfo> directoryInfos = new Stack<DirectoryInfo>();
            
            directoryInfos.Push(di);
            while (directoryInfos.Count > 0)
            {
                DirectoryInfo currentDir = directoryInfos.Pop();
                List<FileInfo> fileList = new List<FileInfo>(currentDir.GetFiles());
                List<string> result = new List<string>();
                string sourceRef = "";

                var parsedName = parseFolderName(currentDir.Name);

                string date = parsedName.GetKeyValue(0);
                int dateint = date.ToInt();
 

                FileInfo infoFile = null;

                if(fileList.Count >0)
                    infoFile = fileList.FirstOrDefault(o => o.Extension.ToLower() == ".info");

                Guid sourceId = Guid.Empty;

                if (infoFile != null)
                {
                    var lines = File.ReadAllText(infoFile.FullName);
                    if (lines.Length >= 36)
                    {
                        sourceId = lines.Substring(0, 36).ToGuid();
                        //.ReadAllLines(infoFile.FullName);

                        //if (lines.Count() > 0)
                        //    sourceId = lines[0].ToGuid();
                    }
                }
                else
                { 
                    // we dont have an info file.

                }



                string fromDate = "1 Jan " + dateint.ToString();
                string toDate = "31 Dec " + dateint.ToString();

               // DateTime fromDate = new DateTime(dateint,1,1);
             //   DateTime toDate = new DateTime(dateint,12,30);


                string names = parsedName.GetKeyValue(1);
                
                List<string> nameParts = names.Split(' ').ToList();





                List<string> locationParts = parsedName.GetKeyValue(2).Split(' ').ToList();

                string locationStr = locationParts.GetKeyValue(0);
                string county = locationParts.GetKeyValue(1);



                string types = parsedName.GetKeyValue(3);
                string reference = parsedName.GetKeyValue(4);



                switch (types.ToLower())
                {
                    case "will":
                        sourceRef = dateint.ToString()+ nameParts.GetKeyValue(0) + nameParts.GetKeyValue(1) + "Will";
                        break;
                    case "deed":
                        sourceRef = dateint.ToString() + nameParts.GetKeyValue(0) + nameParts.GetKeyValue(1) + "Deed";
                        break;
                    case "bond":
                        sourceRef = dateint.ToString() + nameParts.GetKeyValue(1) + nameParts.GetKeyValue(3) + "Bond";
                        break;
                    default:
                        sourceRef = dateint.ToString() + nameParts.GetKeyValue(0) + nameParts.GetKeyValue(1) + "Misc" + currentDir.Name;
                        break;
                }

                


                if (dateint != 0)
                {
                    // we havent got this source so add it.
                    if (!this.keyTypes.Any(o => o.SourceId == sourceId) || sourceId == Guid.Empty)
                    {

                        keyTypes.Add(new KeyType()
                        {
                            SourceId = sourceId,
                            SourceRef = sourceRef,
                            Location = locationStr,
                            County = county,
                            SourceDateFrom = fromDate,
                            SourceDateTo = toDate,
                            SourceType = types,
                            SubjectChristianName = nameParts.GetKeyValue(0),
                            SubjectSurname = nameParts.GetKeyValue(1),
                            OthersideChristianName = nameParts.GetKeyValue(2),
                            OthersideSurname = nameParts.GetKeyValue(3),
                            PhysicalPath = currentDir.FullName.Substring(this.RootPath.Length)
                             
                        });
                    }
                }


                //foreach (FileInfo fileName in fileList.Where<FileInfo>(_func))
                //{
                     
                //}

                int dircount = di.GetDirectories().Count<DirectoryInfo>();
               
                foreach (DirectoryInfo _di in currentDir.GetDirectories())
                {
                    directoryInfos.Push(_di);
                }
              
               
            }


            WriteNewCSV();

        }

        private void WriteNewCSV()
        {
            var orderedList = this.keyTypes.OrderBy(o => o.Date).ThenBy(p => p.Location);

            Debug.WriteLine(KeyType.WriteHeaderString());

            foreach (var key in orderedList)
            {
                Debug.WriteLine(key.WriteString());
            }
        }


        private List<string> parseFolderName(string name)
        {

            List<string> parts = name.Split('_').ToList();

            int idx =0;
            while(idx < parts.Count)
            {
                parts[idx] = parts[idx].Replace('!', ' ');
                idx++;
            }

            return parts;
        }

    }
}
