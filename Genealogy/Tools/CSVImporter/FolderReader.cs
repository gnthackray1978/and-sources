using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TDBCore.Types.libs;

namespace CSVImporter
{
    public class FolderReader
    {
        private string _rootPath = "";
        private Dictionary<string, Guid> _folderIndex = new Dictionary<string, Guid>();
        private Dictionary<string,string> _folderTypeIndex = new Dictionary<string, string>();

        public Dictionary<string, Guid> Index
        {
            get { return _folderIndex; }
        }

        public Dictionary<string, string> FolderTypeIndex
        {
            get { return _folderTypeIndex; }
        }


        public FolderReader(string rootPath)
        {
            _rootPath = rootPath;
        }

        public void ReadFolders()
        {
            var di = new DirectoryInfo(_rootPath);

            var folders = di.EnumerateDirectories("*", SearchOption.AllDirectories);

            foreach (var currentFolder in folders)
            {
                
                var infoFiles = currentFolder.EnumerateFiles("*.info");

                if (infoFiles.Any())
                {
                    foreach (var infoFile in infoFiles)
                    {
                        var contents = File.ReadAllLines(infoFile.FullName);

                        if (contents.Length > 0)
                        {
                            Guid fid = contents.First().ToGuid();
                            if (!_folderIndex.ContainsKey(currentFolder.Name))
                            {
                                _folderIndex.Add(currentFolder.Name, fid);
                                _folderTypeIndex.Add(currentFolder.Name, currentFolder.Parent.Name);

                            }
                            else
                            {
                                Debug.WriteLine(currentFolder.Name + " already added");
                            }
                        }
                        else
                        {
                            Debug.WriteLine(currentFolder.Name + " badly formed info file");
                        }


                    }
                }
                else
                {
                    Debug.WriteLine(currentFolder.FullName + " no info file");

                    if(currentFolder.FullName.Contains("_"))
                    {
                   //     File.WriteAllText(currentFolder.FullName + "\\info.info", Guid.NewGuid().ToString());
                    }
                }

            }

        }
    }
}
