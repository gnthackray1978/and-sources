using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using TDBCore.BLL;
using ToolsCore;


namespace UpdateKey
{

    public class UpdateFolderInfo
    {
        public string RootPath { get; set; }
        public string CSVPath { get; set; }
        private List<KeyType> keyTypes = new List<KeyType>();

        public void Run()
        {
            CSVPath = RootPath + @"\key.csv";

            DirectoryInfo first = new DirectoryInfo(RootPath);

            keyTypes = Tools.ReadCSVKey(this.CSVPath);

            processCSV();
        }

 
        private void processCSV() 
        {

            SourceBll sourceBll = new SourceBll();
         

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


                        Tools.RefreshSourceFiles(di, csvRow, source);


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
              //  string sourceRef = "";

                FolderDescriptor fdesc = currentDir.Name.ToFolderDescriptor();

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

                List<string> nameParts = fdesc.FullName.Split(' ').ToList();

           


                if (fdesc.Date.Length == 4)
                {
                    // we havent got this source so add it.
                    if (!this.keyTypes.Any(o => o.SourceId == sourceId) || sourceId == Guid.Empty)
                    {

                        keyTypes.Add(new KeyType()
                        {
                            SourceId = sourceId,
                            SourceRef = Tools.MakeSourceRef(fdesc,di.Name),
                            Location = fdesc.Location,
                            County = fdesc.LocationCounty,
                            SourceDateFrom = fdesc.LowerDateRange(),
                            SourceDateTo = fdesc.UpperDateRange(),
                            SourceType = fdesc.Type,
                            
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
