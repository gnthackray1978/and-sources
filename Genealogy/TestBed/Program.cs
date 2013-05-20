using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore;
using GedItter.BLL;
using System.Data.Objects;

using System.Data.Objects.DataClasses;
using GedItter.BirthDeathRecords.BLL;
using TDBCore.BLL;
using GedItter.ModelObjects;
using TDBCore.Types;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Reflection;
using GedItter.MarriageRecords.BLL;
using GedItter;
using System.IO;
using System.Text.RegularExpressions;
using TDBCore.EntityModel;
using UnitTests;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using GedItter.BirthDeathRecords;

namespace TestBed
{
   
    class Program
    {

       


        static void Main(string[] args)
        {

            string sources = "";

            List<Guid> sourceList = new List<Guid>();


            sources.Split(',').Where(s => s != null).ToList().ForEach(s => sourceList.Add(s.ToGuid()));



           // ParishsBLL _ParishsBLL = new ParishsBLL();
           // SourceTypesBLL _sourceTypes = new SourceTypesBLL();
           // SourceBLL _sources = new SourceBLL();
           // DeathsBirthsBLL _deathsBirthsBLL = new DeathsBirthsBLL();
           // RelationsBLL _relationsBLL = new RelationsBLL();
           // MarriagesBLL _marriagesBLL = new MarriagesBLL();
           // //ddd1d290-645c-40e2-9f94-21d2f4f672c8
           // Guid sourceId0 = new Guid("8d504873-3d1b-4084-a922-4314a6b8657e");
           // Guid sourceId1 = new Guid("2cd16ede-ef53-44df-8486-0021f57875ad");
           // Guid sourceId2 = new Guid("4f31d528-c940-4240-b974-003b8bc81bd1");
           // Guid parishId = new Guid("7a878c7c-b7e3-408e-8608-8c98a2f2ff1f");//allerthorpe
           // Guid personId = new Guid("4d800222-65fc-42f0-b568-972cde7ce38f");
           // Guid marriageid = new Guid("06573696-412f-4f97-b0ec-9db87a7b3e86");

           // Guid marriageiddoesntexist = new Guid("00000000-0000-0000-0000-000000000000");

           //List<Source> sources =  _sources.FillTreeSources().ToList();



            //foreach (var mar in _marriagesBLL.ModelContainer.GetMarriageByLocation().Where(l=>l.MarriageLocation.Contains("Harewood")))
            //{
            //    Debug.WriteLine("//"+ mar.Date + " " + mar.MarriageLocation + " " + mar.MaleCName + " " + mar.MaleSName + " - " + mar.FemaleCName + " " + mar.FemaleSName + " " + mar.Witness1 + " " + mar.witness2);

            //    //Debug.WriteLine("//_marriagesBLL.UpdateNotes(\"" + mar.marriage_id.ToString() + "\",\"Marriage not found in original source \",\"" + mar.MaleCName + "\",\"" + mar.MaleSName + "\",\"" + mar.FemaleCName + "\",\"" + mar.FemaleSName + "\");");
            //    Debug.WriteLine("_marriagesBLL.ImportMarriageWits(\"" + mar.marriage_id.ToString() + "\",\"\",\"\",\"\",\"\");");
                
            //    Debug.WriteLine("//update Marriages ");
            //    Debug.WriteLine("//set MaleCName = '" + mar.MaleCName+"',FemaleCName = '"+ mar.FemaleCName+"',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''");
            //    Debug.WriteLine("//where Marriage_id = '" +mar.marriage_id.ToString() + "'");
            //}



          //  DeathBirthFilterModel.MergeDuplicateRecords(); 

            //NUnitPersonFilter nUnitPersonFilter = new NUnitPersonFilter();

            //nUnitPersonFilter.SetUpPersonFilter();
            //nUnitPersonFilter.CheckSavedData();
            //nUnitPersonFilter.TestGroupDuplicates();
            //nUnitPersonFilter.TestMergeDuplicates();

            //nUnitPersonFilter.TearDown();


            //NUnitMarriageFilter nunitMarriageFilter = new NUnitMarriageFilter();
            //nunitMarriageFilter.SetUpMarriageFilter();
            //nunitMarriageFilter.CheckControlMultiDelete();
            //nunitMarriageFilter.CheckSavedData();
            //nunitMarriageFilter.TestGroupDuplicates();
            //nunitMarriageFilter.TestMergeDuplicates();

         //   nunitMarriageFilter.TestRemoveDuplicate();

          //  nunitMarriageFilter.TearDown();

            string result = "";
            int n = 37;

            do
            {
                result = ((n % 2 == 0) ? "0" : "1") + result;
                n = n / 2;
            } while (n != 0);

            Console.WriteLine(result);

             Console.WriteLine("finished");
             Console.ReadKey();
        }

    
    
    
    }

  



    public class WillNotesImporter
    {
        string path = @"C:\Users\george\Desktop\familyhist\willnotesImproved_bk4.csv";
      //  string path2 = @"C:\Users\george\Desktop\familyhist\temp.csv";



        string folderRoot = @"C:\Users\george\Desktop\familyhist\Scans_Of_Docs_And_People\Wills";

        public void Import()
        {


            List<string> lineList = new List<string>(System.IO.File.ReadAllLines(path, Encoding.ASCII));

            lineList.RemoveAt(0);

            string[] allLines = lineList.ToArray();//File.ReadAllLines(path);

            var query = from line in allLines

                        let data = line.Split(',')

                        select new _tempObj(data[0],data[1], makeYear(data[2]), data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11], data[12]);

            //{
            //    SourceId = 
            //    SourceDate = 
            //    TestatorCname = 
            //    TestatorSname = 
            //    LocationId = 
            //    Location = 
            //    Occupation = 
            //    //Relationship = 
            //    //PersonId = 
            //    //PersonName = 
            //    //PersonSurname = 
            //    //Notes = 

            //};


            #region  import sourceid file

            //List<string> lineList0 = new List<string>(System.IO.File.ReadAllLines(path2, Encoding.ASCII));

            //lineList0.RemoveAt(0);

            //string[] allLines2 = lineList0.ToArray();//File.ReadAllLines(path);

            //var sourceIdsQry = from line in allLines2

            //let data = line.Split(',')

            //select new

            //{
            //    SourceId = data[0],
            //    SourceRef = data[1],
            //    SourceRefDateFrom = makeYear(data[2]),
            //    SourceRefDateTo = makeYear(data[3]),
            //    NotesDate = makeYear(data[4]),
            //    NotesCName = data[5],
            //    NotesSName = data[6]
                           

            //};

            #endregion

            SourceBLL sourceBll = new SourceBLL();
            SourceMappingParishsBLL sourceMappingParishsBll = new SourceMappingParishsBLL();
            SourceMappingsBLL sourceMappings = new SourceMappingsBLL();
            DeathsBirthsBLL personBll = new DeathsBirthsBLL();
            RelationsBLL relationsBll = new RelationsBLL();

            //List<_tempObj> tempObjs = new List<_tempObj>();

            var resultsList  = query.ToList();


            int idx = 0;

         //   List<string> relationRecord = new List<string>();

            while(idx < resultsList.Count)
            {

                
                string relationStr = resultsList[idx].Relationship.Trim();

                if (relationStr == "")
                    relationStr = "Unknown";

                RelationType _relationType = relationsBll.GetRelationTypes2().Where(r => r.RelationName == relationStr).FirstOrDefault();
 
                Person _person = new Person();

                _person.ReferenceLocation = resultsList[idx].Location;
                _person.ReferenceDateInt = resultsList[idx].SourceDate;
                _person.ReferenceDateStr = resultsList[idx].StrSourceDate;
                _person.ReferenceLocationId = resultsList[idx].LocationId;

                _person.ChristianName = resultsList[idx].PersonName;
                _person.Surname = resultsList[idx].PersonSurname;
                _person.Notes = resultsList[idx].Notes;
              
              
                _person.OrigSurname = "wtentry";

                if (resultsList[idx].Relationship.ToLower().Trim() == "Spouse")
                {
                    _person.SpouseName = resultsList[idx].TestatorCname;
                    _person.SpouseSurname = resultsList[idx].TestatorSname;
                }


                Guid newPerson = personBll.InsertPerson(_person);
                List<Guid> sources = new List<Guid>();
                sources.Add(resultsList[idx].SourceId);

                sourceMappings.WritePersonSources2(newPerson, sources, 1);




                if (_relationType != null)
                {
                    relationsBll.InsertRelation(newPerson, resultsList[idx].PersonId, _relationType.RelationTypeId, 1);
                }

  

                idx++;
            }

             
        }


        private int makeYear(string yearVal)
        {

            int retVal = 0;



            Regex regex = new Regex(@"\d\d\d\d");

            Match _match = regex.Match(yearVal);



            if (_match.Success)
            {

                retVal = Convert.ToInt32(_match.Value);

            }



            return retVal;

        }


        public List<string> ImportFolderSources()
        {

            List<string> result = new List<string>();

           
            Stack<string> stack = new Stack<string>();

            stack.Push(folderRoot);
 
            while (stack.Count > 0)
            {
               
                string dir = stack.Pop();

                try
                {
                    bool isFound = false;
                    foreach (var _file in Directory.GetFiles(dir, "*.*"))
                    {
                        if (_file.ToLower().Contains("info"))
                        {
                            isFound = true;
                        }

                    }


                    if(isFound)
                        result.Add(dir);

                    foreach (string dn in Directory.GetDirectories(dir))
                    {
                        stack.Push(dn);
                    }
                }
                catch
                {
                     
                }
            }

            return result;
        }


        public void ProcessFolder(string _path)
        {
            DirectoryInfo di = new DirectoryInfo(_path);
            Guid sourceId = Guid.Empty;
            string notes = "";


            List<FileInfo> imageFiles = new List<FileInfo>();


            foreach (DirectoryInfo _dinfo in di.GetDirectories())
            {
                if (_dinfo.Name == "Images")
                {
                    imageFiles = _dinfo.GetFiles().ToList();
                }
            }

            
            foreach (FileInfo _finfo in di.GetFiles())
            {
                // get source id
                if (_finfo.Extension.ToLower().Contains("info"))
                {
                    List<string> listLines = System.IO.File.ReadAllLines(_finfo.FullName).ToList();

                    if (listLines.Count > 0)
                    {

                        if (!Guid.TryParse(listLines[0], out sourceId))
                        {

                            sourceId = Guid.Empty;
                        }
                    }
                }


                if (_finfo.Name.ToLower().Contains("notes"))
                {
                    notes  = System.IO.File.ReadAllText(_finfo.FullName);
                }

            }

            // get existing files for source

           
             
            
            Directory.GetFiles(_path);
        }


        private void ImportMarriages()
        {

            MarriagesBLL _marriagesBLL = new MarriagesBLL();


            //#region bardsey
            ////12/11/1776 Bardsey John Furniss - Ann Thackery R Fawcett Surrogate William Wilson of Preston a Farmer
            ////_marriagesBLL.UpdateNotes("c1d7368a-3904-4909-96e4-032147dd6e0d","Marriage not found in original source ","John","Furniss","Ann","Thackery");
            //_marriagesBLL.ImportMarriageWits("c1d7368a-3904-4909-96e4-032147dd6e0d", "Thomas Carr", "Richard Butterfield", "", "");
            ////21/08/1776 Bardsey John Waddington - Sarah Thackray Henry Thackray of Rowley a Farmer H Wright Surrogate
            ////_marriagesBLL.UpdateNotes("dff5eac2-2ebe-459f-96ec-874a96da5dbe","Marriage not found in original source ","John","Waddington","Sarah","Thackray");
            //_marriagesBLL.ImportMarriageWits("dff5eac2-2ebe-459f-96ec-874a96da5dbe", "Henary Thackwray", "Elizabeth Thackwray", "", "");
            ////07/01/1803 Bardsey Richard Farrer - Elizabeth Thackray  
            ////_marriagesBLL.UpdateNotes("d731514e-3f2e-4ea3-8c24-aae561da833a","Marriage not found in original source ","Richard","Farrer","Elizabeth","Thackray");
            //_marriagesBLL.ImportMarriageWits("d731514e-3f2e-4ea3-8c24-aae561da833a", "Francis Thackwray", "Richard Butterfield", "", "");
            ////25 JUL 1814 Bardsey William Thackray - Ann Lowrands  
            ////_marriagesBLL.UpdateNotes("bfe1783e-a8e7-440e-8623-0ce275275a68","Marriage not found in original source ","William","Thackray","Ann","Lowrands");
            //_marriagesBLL.ImportMarriageWits("bfe1783e-a8e7-440e-8623-0ce275275a68", "David Midgeley", "Richard Butterfield", "", "");
            ////01/01/1817 Bardsey William Wright - Isabella Thackwray Richard Fawcett Surrogate Francis Thackwray of Bardsey
            ////_marriagesBLL.UpdateNotes("e8b60bbd-981a-451f-9768-fb257c343959","Marriage not found in original source ","William","Wright","Isabella","Thackwray");
            //_marriagesBLL.ImportMarriageWits("e8b60bbd-981a-451f-9768-fb257c343959", "Abraham Beetham", "Elizabeth Midgeley", "Elizabeth Thackwray", "");

            //#endregion

            ////30 SEP 1765 Harewood Joseph Thackery - Mary Chambers  
            //_marriagesBLL.ImportMarriageWits("87a3d0fc-c81a-4e92-a56c-7fedd500a482", "Thomas Wray", "Major Mawson", "", "");
            ////update Marriages 
            ////set MaleCName = 'Joseph',FemaleCName = 'Mary',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = '87a3d0fc-c81a-4e92-a56c-7fedd500a482'
            ////1 jan 1765 Harewood Joseph Thackerey - Mary Chambers  
            //_marriagesBLL.ImportMarriageWits("a10d1388-9bdd-4e42-9a2a-64e333cb2df2", "Thomas Wray", "Major Mawson", "", "");
            ////update Marriages 
            ////set MaleCName = 'Joseph',FemaleCName = 'Mary',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = 'a10d1388-9bdd-4e42-9a2a-64e333cb2df2'
            ////30 Sep 1765 Harewood Joseph Thackrey - Mary Chambers  
            //_marriagesBLL.ImportMarriageWits("d8da3403-5fe0-4c2c-a08b-169292d810de", "Thomas Wray", "Major Mawson", "", "");
            ////update Marriages 
            ////set MaleCName = 'Joseph',FemaleCName = 'Mary',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = 'd8da3403-5fe0-4c2c-a08b-169292d810de'
            ////1 jan 1770 Harewood Thomas Robertinson - Anne Thackray  
            //_marriagesBLL.ImportMarriageWits("df2bf2e0-9f45-4629-bb39-651926ac160f", "Jonathon Todd", "John Pike", "", "");
            ////update Marriages 
            ////set MaleCName = 'Thomas',FemaleCName = 'Anne',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = 'df2bf2e0-9f45-4629-bb39-651926ac160f'
            ////1 jan 1770 Harewood Thomas Robinson - Anne Thackray  
            //_marriagesBLL.ImportMarriageWits("73c9f3ca-2ce9-4494-aa42-8c76d22d7284", "Jonathon Todd", "John Pike", "", "");
            ////update Marriages 
            ////set MaleCName = 'Thomas',FemaleCName = 'Anne',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = '73c9f3ca-2ce9-4494-aa42-8c76d22d7284'
            ////29 NOV 1770 Harewood Thomas Robinson - Ann Thackaray  
            //_marriagesBLL.ImportMarriageWits("087a7199-ffce-4aef-adc7-9931cb626316", "Jonathon Todd", "John Pike", "", "");
            ////update Marriages 
            ////set MaleCName = 'Thomas',FemaleCName = 'Ann',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = '087a7199-ffce-4aef-adc7-9931cb626316'
            ////29 NOV 1770 Harewood Thomas Robinson - Ann Thackaray  
            //_marriagesBLL.ImportMarriageWits("ca733d1f-a1c7-438d-a5d0-cb50570c2f16", "Jonathon Todd", "John Pike", "", "");
            ////update Marriages 
            ////set MaleCName = 'Thomas',FemaleCName = 'Ann',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = 'ca733d1f-a1c7-438d-a5d0-cb50570c2f16'
            
            ////1 Apr 1771 Harewood John Thackwray - Ann Bullock  
            //_marriagesBLL.ImportMarriageWits("02b433a0-ddb3-42e3-837c-b9948c12bb7f", "Henry Bullock", "William Thackwray", "", "");
            ////update Marriages 
            ////set MaleCName = 'John',FemaleCName = 'Ann',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = '02b433a0-ddb3-42e3-837c-b9948c12bb7f'

    
            ////02 FEB 1789 Harewood James Thackeray - Ann Allenby  
            //_marriagesBLL.ImportMarriageWits("03e3a316-bc37-41d9-94b5-ce827b3a419c", "Thomas Allanby", "William Marston", "", "");
            ////update Marriages 
            ////set MaleCName = 'James',FemaleCName = 'Ann',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = '03e3a316-bc37-41d9-94b5-ce827b3a419c'
            ////02 FEB 1789 Harewood James Thackray - Ann Allanby  
          
            
            
  
          
            
  
            
            
            ////26 Dec 1809 Harewood Major Thackwray - Alice Kaberry  
            //_marriagesBLL.ImportMarriageWits("5ca67715-4e36-4410-977a-d8a8c6a7ed9d", "John Abbott", "Mary Knapton", "Jacob Thackery", "Abraham Thornton");
            ////update Marriages 
            ////set MaleCName = 'Major',FemaleCName = 'Alice',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = '5ca67715-4e36-4410-977a-d8a8c6a7ed9d'
       
            

            ////1 jan 1810 Harewood William Read - Anne Thackwray  
            //_marriagesBLL.ImportMarriageWits("ed0169ce-8b0f-4838-87a3-907dc9849481", "James Brooks", "John Sharp", "", "");
          
            
            ////13 DEC 1813 Harewood Joseph Craven - Elizabeth Thackwray  
            //_marriagesBLL.ImportMarriageWits("589465af-bb60-45b9-9bfe-68457df97907", "JohnX Kirby", "AnnX Thackwray", "James Brookes ", "");
            ////update Marriages 
            ////set MaleCName = 'Joseph',FemaleCName = 'Elizabeth',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = '589465af-bb60-45b9-9bfe-68457df97907'
            
            ////10 FEB 1816 Harewood William Thackwray - Elizabeth Walton  
            //_marriagesBLL.ImportMarriageWits("a0fb79d6-1573-4efd-aa50-932aa1230883", "", "", "", "");
            ////update Marriages 
            ////set MaleCName = 'William',FemaleCName = 'Elizabeth',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = 'a0fb79d6-1573-4efd-aa50-932aa1230883'
           
            ////02/11/1819 Harewood John Thackray - Hannah Knapton Richard Fawcett Surrogate William Thackray - a corn miller
            //_marriagesBLL.ImportMarriageWits("983a81c5-35d9-4cc1-a362-0ba9f7d6f01b", "", "", "", "");
            ////update Marriages 
            ////set MaleCName = 'John',FemaleCName = 'Hannah',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = '983a81c5-35d9-4cc1-a362-0ba9f7d6f01b'
            
            ////03 FEB 1821 Harewood Samuel Bucktrout - Ann Thackwray  
            //_marriagesBLL.ImportMarriageWits("dd346713-1601-4bfe-acd4-7e7d4558f2b3", "", "", "", "");
            ////update Marriages 
            ////set MaleCName = 'Samuel',FemaleCName = 'Ann',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = 'dd346713-1601-4bfe-acd4-7e7d4558f2b3'
            
            ////02 OCT 1827 Harewood William Smith - Ann Thackwray  
            //_marriagesBLL.ImportMarriageWits("19f01e32-808c-471a-9664-4cfb9ba33d5e", "", "", "", "");
            ////update Marriages 
            ////set MaleCName = 'William',FemaleCName = 'Ann',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = '19f01e32-808c-471a-9664-4cfb9ba33d5e'
            
            ////15 OCT 1831 Harewood James Steel - Hannah Thackwray  
            //_marriagesBLL.ImportMarriageWits("e1346868-ff9f-4534-ad89-906d7ab1d09d", "", "", "", "");
            ////update Marriages 
            ////set MaleCName = 'James',FemaleCName = 'Hannah',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = 'e1346868-ff9f-4534-ad89-906d7ab1d09d'
            
            ////29 SEP 1834 Harewood Abraham Lister - Elizabeth Thackray  
            //_marriagesBLL.ImportMarriageWits("4305372e-a5b2-4fa2-ae16-dee2a7cc8743", "", "", "", "");
            ////update Marriages 
            ////set MaleCName = 'Abraham',FemaleCName = 'Elizabeth',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = '4305372e-a5b2-4fa2-ae16-dee2a7cc8743'
            
            ////04 MAY 1835 Harewood Charles Thackwray - Hannah Eastburn  
            //_marriagesBLL.ImportMarriageWits("54d240b7-3ef6-443e-b569-37a4ee993e99", "", "", "", "");
            ////update Marriages 
            ////set MaleCName = 'Charles',FemaleCName = 'Hannah',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            ////where Marriage_id = '54d240b7-3ef6-443e-b569-37a4ee993e99'
            
            ////29 DEC 1840 Harewood William Dodsworth - Ann Thackwray  
            //_marriagesBLL.ImportMarriageWits("63cc8d87-5610-456e-b233-f02c61b36724", "", "", "", "");
            //update Marriages 
            //set MaleCName = 'William',FemaleCName = 'Ann',MaleOccupation = '',FemaleOccupation = '', MaleLocation = ''
            //where Marriage_id = '63cc8d87-5610-456e-b233-f02c61b36724'

        }

    }


    #region temp object
    public class _tempObj
    {
        //public Guid sourceId;
        //public int intSourceDate;
        //public string tcname;
        //public string tsname;
        //public int sourceDate;
        //public int sourceDateTo;
        //public string sourceRef;

        //public _tempObj(Guid _sourceId, int _intSourceDate, string _tcname, string _tsname, int _sourceDate, int _sourceDateTo, string _sourceRef)
        //{
        //    sourceId = _sourceId;
        //    intSourceDate = _intSourceDate;
        //    tcname = _tcname;
        //    tsname = _tsname;
        //    sourceDate = _sourceDate;
        //    sourceDateTo = _sourceDateTo;
        //    sourceRef = _sourceRef;

        //}

             //SourceId = data[0],
             //   SourceDate = makeYear(data[1]),
             //   TestatorCname = data[2],
             //   TestatorSname = data[3],
             //   LocationId = data[4],
             //   Location = data[5],
             //   Occupation = data[6]
                //Relationship = data[7],
                //PersonId = data[8],
                //PersonName = data[9],
                //PersonSurname = data[10],
                //Notes = data[11]

        public _tempObj(string _sourceId,string _strDate, int _sourceDate, string _tcname,
            string _tsname, string _locationId, string _location, string _occupation, string _relationShip,
            string _personId, string _personName, string _personSurname, string _notes)
        {            

            if(! Guid.TryParse(_personId, out personId))
                personId = Guid.Empty;

            if(!Guid.TryParse(_sourceId, out sourceId))
                sourceId = Guid.Empty;

            if(!Guid.TryParse(_locationId, out locationId))
                locationId = Guid.Empty;

            sourceDate = _sourceDate;
            strSourceDate = _strDate;
            testatorCname = _tcname;
            testatorSname = _tsname;

            location = _location;
            occupation = _occupation;
            relationship = _relationShip;

            personName = _personName;
            personSurname = _personSurname;
            notes = _notes;

        }

        public override string ToString()
        {
            return this.Location + "," + this.LocationId.ToString() + "," + this.Notes + "," + this.Occupation + "," + this.PersonId.ToString() + "," + this.PersonName + "," + this.PersonSurname + "," + this.Relationship + "," + this.SourceDate.ToString() + "," + this.SourceId.ToString() + "," + this.StrSourceDate + "," + this.TestatorCname + "," + this.TestatorSname;
        }

        private string strSourceDate;

	    public string StrSourceDate
	    {
		    get { return strSourceDate;}
		    set { strSourceDate = value;}
	    }
	
        private string notes;

        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }
        

        private string personSurname;

        public string PersonSurname
        {
            get { return personSurname; }
            set { personSurname = value; }
        }
        


        private string personName;

        public string PersonName
        {
            get { return personName; }
            set { personName = value; }
        }
        


        private Guid personId;

        public Guid PersonId
        {
            get { return personId; }
            set { personId = value; }
        }
        

        private string relationship;

        public string Relationship
        {
            get { return relationship; }
            set { relationship = value; }
        }
        


        private string occupation;

        public string Occupation
        {
            get { return occupation; }
            set { occupation = value; }
        }
        



        private string location;

        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        


        private Guid locationId;

        public Guid LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }
        


        private Guid sourceId;

        public Guid SourceId
        {
            get { return sourceId; }
            set { sourceId = value; }
        }

        private int sourceDate;

        public int SourceDate
        {
            get { return sourceDate; }
            set { sourceDate = value; }
        }


        private string testatorCname;

        public string TestatorCname
        {
            get { return testatorCname; }
            set { testatorCname = value; }
        }



        private string testatorSname;

        public string TestatorSname
        {
            get { return testatorSname; }
            set { testatorSname = value; }
        }
        




    }

    #endregion
}
