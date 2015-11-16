using System;
using System.Diagnostics;
using System.IO;
using TDBCore.Types.domain.import;

namespace CSVImporter
{
    class Program
    {
        const string Destination = @"E:\GoogleDrive\familyhist\datadump\sors.csv";


        static void Main(string[] args)
        {
            var csImportCsv = new CsImportCsv();

            try
            {

                UpdateKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                 
            }

            Console.WriteLine("finished");
            Console.ReadKey();
        }


        private static void UpdateKey()
        {
            var folderReader = new FolderReader(@"E:\GoogleDrive\familyhist\CategorizedRecords");

            folderReader.ReadFolders();

            var sourcePersonReader = new SourcePersonReader(@"https://docs.google.com/spreadsheets/d/10pxOz7dY1-67jxNI-hLqYcVcwn35gTcGUF3F0xygW8o/pub?output=csv");

            sourcePersonReader.ReadSources();

            sourcePersonReader.UpdateSources(folderReader.Index);

            sourcePersonReader.AddMissingSources(folderReader.Index,folderReader.FolderTypeIndex);

            var newFile = sourcePersonReader.DumpToCSV();

            File.WriteAllLines(@"E:\GoogleDrive\familyhist\datadump\output.csv", newFile);
        }

        private static void CorrectKey()
        {
            var folderReader = new FolderReader(@"E:\GoogleDrive\familyhist\CategorizedRecords");

            folderReader.ReadFolders();

            var sourcePersonReader = new SourcePersonReader(@"https://docs.google.com/spreadsheets/d/10pxOz7dY1-67jxNI-hLqYcVcwn35gTcGUF3F0xygW8o/pub?output=csv");

            sourcePersonReader.ReadSources();

            sourcePersonReader.UpdateSources(folderReader.Index);

            sourcePersonReader.AddMissingSources(folderReader.Index, folderReader.FolderTypeIndex);




            var newFile = sourcePersonReader.DumpToCSV();

            File.WriteAllLines(@"E:\GoogleDrive\familyhist\datadump\output.csv", newFile);
        }



        private static void CreateSourceCSV(CsImportCsv csImportCsv)
        {
            


            File.WriteAllText(Destination, csImportCsv.CreateSourceCSV());

            if (File.Exists(Destination))
            {
                Process.Start("explorer.exe", Destination);
            }
        }

        private static void CreatePersonCSV(CsImportCsv csImportCsv)
        {



            File.WriteAllText(Destination, csImportCsv.CreatePersonCSV());

            if (File.Exists(Destination))
            {
                Process.Start("explorer.exe", Destination);
            }
        }

        private static void CreateMarriageCSV(CsImportCsv csImportCsv)
        {



            File.WriteAllText(Destination, csImportCsv.CreateMarCSV());

            if (File.Exists(Destination))
            {
                Process.Start("explorer.exe", Destination);
            }
        }
    }
}
