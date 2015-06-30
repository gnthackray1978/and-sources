using System;
using System.Diagnostics;
using System.IO;
using TDBCore.Types.domain.import;

namespace CSVImporter
{
    class Program
    {
        const string Destination = @"G:\Google Drive\familyhist\Images\persons.csv";


        static void Main(string[] args)
        {
            var csImportCsv = new CsImportCsv();

            try
            {
                //csImportCsv.ImportSources(Destination, 1);

                CreatePersonCSV(csImportCsv);

            //    csImportCsv.ImportPersonCSV(Destination);
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
    }
}
