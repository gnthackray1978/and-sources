using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using GedItter.BLL;
using ToolsCore;
using System.Diagnostics;
using TDBCore.EntityModel;
using TDBCore.BLL;


namespace UpdateSources
{
    public class UpdateSources
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

            SourceBLL sourceBll = new SourceBLL();


           // int idx = 0;

            var groupedKey = this.keyTypes.GroupBy(o => o.SourceId);


            foreach (var group in groupedKey)
            { 
               

                //.SourceId
                KeyType csvRow = group.First();

                if (csvRow.Location == "Location") continue;
                
                    
                    Guid newSource = csvRow.SourceId;


                string path = RootPath + csvRow.PhysicalPath;

                //Path.Combine(RootPath, csvRow.PhysicalPath);

                DirectoryInfo di = new DirectoryInfo(path);
                int fileCount = 0;
                if (di.Exists)
                {
                    fileCount = di.GetFiles().Count();
                }


                TDBCore.EntityModel.Source source = null;

                if (csvRow.SourceId != Guid.Empty)
                    source = sourceBll.FillSourceTableById2(csvRow.SourceId);




                if (csvRow.SourceId != Guid.Empty)
                {
                    // get list of files for source
                 //   Tools.RefreshSourceFiles(di, csvRow, source);


                }

                RelationsBLL relationsBLL = new RelationsBLL();             
                SourceMappingsBLL sourceMappingsBLL = new SourceMappingsBLL();

                List<Person> persons = sourceBll.GetPersonsForSource(csvRow.SourceId);
                // list of persons for this source

                List<RelationType> relationTypes = relationsBLL.GetRelationTypes2().ToList();

                List<Relation> relations = new List<Relation>();

                foreach (Person _person in persons)
                {
                    relations.AddRange(relationsBLL.GetRelationByChildOrParent(_person.Person_id).ToList()); ;


                }

                

                // so we've got the list of persons thats stored 
                // compare it to whats in the key

                //sourceMappingsBLL.






                //



            }

            //while (idx < this.keyTypes.Count)
            //{
            //    Console.Write("\r" + idx + " of " + this.keyTypes.Count);

            //    KeyType csvRow = this.keyTypes[idx];
 

            //    if (idx != 0)
            //    {



            //        if (fileCount > 0)
            //        {

            //            // read source record
            //            // extract all need information
                         


            //            TDBCore.EntityModel.Source source = null;

            //            if (csvRow.SourceId != Guid.Empty)
            //                source = sourceBll.FillSourceTableById2(csvRow.SourceId);

            //            if ( csvRow.SourceId != Guid.Empty)
            //            {
            //                // get list of files for source
            //                Tools.RefreshSourceFiles(di, csvRow, source);                           

                        
            //            }
                        
            //            // so now we need list of persons.





            //            // list of relations for the source
                         


            //        }


            //        this.keyTypes[idx] = csvRow;
            //    }


            //    idx++;
            //}
            //// ok so go through adding in images from source locations 
            //// into mapping table.

        


        }

 


    }


}
