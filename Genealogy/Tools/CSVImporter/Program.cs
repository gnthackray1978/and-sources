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

                CreateSourceCSV(csImportCsv);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                 
            }

            Console.WriteLine("finished");
            Console.ReadKey();
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
