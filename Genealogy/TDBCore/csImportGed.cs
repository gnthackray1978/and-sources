using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;


using System.IO;

using System.Diagnostics;
using Gedcom;
using GedcomParser;
using System.Text.RegularExpressions;
using GedItter.BirthDeathRecords.BLL;
using GedItter.MarriageRecords.BLL;
using GedItter.BLL;
 
using TDBCore.BLL;
using TDBCore.Types;
 



namespace GedItter.Tools
{


    public class CsImportGeds
    {

        private string source = "User";
        string _rootPath = "";
        List<string> _filesToImport = new List<string>();

     
        public CsImportGeds()
        {
          
        }

        public void ImportMarriagesGeds(string path0)
        {

            string husbandSName = "";
            string husbandCName = "";
            string wifeSName    = "";
            string wifeCName    = "";

            string tpLocation   = "";
            string marParish    = "";
            string marCounty    = "";
            string marCountry   = "";
            string marDateStr   = "";
            int    marDateInt   = 0;
            string husbParish   = "";
            string wifeParish   = "";
            

            #region set up stuff
       //     FolderBrowserDialog fbd = new FolderBrowserDialog();
        //    fbd.ShowDialog();

            _rootPath = path0;// fbd.SelectedPath;

            _filesToImport = GetFilesRecursive(_rootPath);


            GedcomRecordReader _reader = new GedcomRecordReader();
            Gedcom.GedcomDatabase database = new Gedcom.GedcomDatabase();

            #endregion

            source = "IGI Unusual Spellings";

            foreach (string path in this._filesToImport)
            {

                Debug.WriteLine("importing :" + path);

                if (path.ToLower().Contains("ged"))
                {
                    _reader.ReadGedcom(path);
                    database = _reader.Database;
                    int idx = 0;

                    while (database.Families.Count > idx)
                    {
                        GedcomFamilyRecord gfr = (GedcomFamilyRecord)database.Families[idx];
                        
                        GetMarriageDescription(gfr, 
                            out marDateStr, 
                            out marDateInt, 
                            out tpLocation);

                        ParseAddress(tpLocation, out marParish, out marCounty, out marCountry);


                        if (gfr.Husband != null)
                        {
                            GedcomIndividualRecord giHusb = (GedcomIndividualRecord)database[gfr.Husband];

                            if (giHusb != null)
                            {
                                if (giHusb.Names.Count > 0)
                                {
                                    husbandCName = giHusb.Names[0].Given;
                                    husbandSName = giHusb.Names[0].Surname;
                                }

                                SearchForIndiRes(giHusb, out husbParish, marDateInt);
                            }
                        }

                        if (gfr.Wife != null)
                        {
                            GedcomIndividualRecord giWife = (GedcomIndividualRecord)database[gfr.Wife];

                            if (giWife != null)
                            {
                                if (giWife.Names.Count > 0)
                                {
                                    wifeCName = giWife.Names[0].Given;
                                    wifeSName = giWife.Names[0].Surname;
                                }

                                SearchForIndiRes(giWife, out wifeParish, marDateInt);
                            }
                        }


                        //Debug.WriteLine("cn: "+  husbandCName + " sn:" + husbandSName + " pr:" + husbParish +
                        //                "cn: " + wifeCName + " sn:" + wifeSName + " pr:"+ wifeParish +
                        //                "mp: " + marParish + " cc: " + marCounty + "my:" +marDateInt +
                        //                "mstr:" + marDateStr);

                        MarriagesBLL marriagesBLL = new MarriagesBLL();
                       
                        if (marDateInt > 0)
                        {
                            marriagesBLL.InsertMarriage2(marDateStr,
                                wifeCName,
                                Guid.Empty, "", wifeParish,
                                wifeSName,
                                husbandCName,
                                Guid.Empty,
                                "",
                                husbParish,
                                husbandSName,
                                marCounty,
                                marParish, source,
                                marDateInt,"","",false,false,false,false,1,
                                 new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"),
                                  new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"),
                                  new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"),0,0,Guid.Empty,0,0,"","");
                        }
                        idx++;
                    }
                }
                else
                {

                }
            }
        }

        private void SearchForIndiRes(GedcomIndividualRecord grec, out string place, int marYear)
        {
            place = "";
            int evtYear = 0;
            foreach (GedcomIndividualEvent evt in grec.Events)
            {
                if (evt.EventType == GedcomEvent.GedcomEventType.RESI ||
                    evt.EventType == GedcomEvent.GedcomEventType.RESIFact)
                {
                    #region get event year - if there is one
                    if (evt.Date != null)
                    {
                        if (evt.Date.DateTime1.HasValue)
                        {
                            evtYear = evt.Date.DateTime1.Value.Year;
                        }
                    }
                    #endregion

                   
                    if (marYear >= evtYear)
                    {
                        if (evt.Place != null)
                        {
                            place = evt.Place.Name;
                        }
                    }
                    
                    
                }

            }

            if (place == "")
            {

                if (grec.Birth != null && grec.Birth.Place != null)
                {
                    place = grec.Birth.Place.Name;
                }
            }

            if (place == "")
            {
                foreach (GedcomIndividualEvent evt in grec.Events)
                {
                    if (evt.EventType == GedcomEvent.GedcomEventType.CHR ||
                        evt.EventType == GedcomEvent.GedcomEventType.BAPM)
                    {
                        #region get event year - if there is one
                        if (evt.Date != null)
                        {
                            if (evt.Date.DateTime1.HasValue)
                            {
                                evtYear = evt.Date.DateTime1.Value.Year;
                            }
                        }
                        #endregion

                       
                        if (evt.Place != null)
                        {
                            place = evt.Place.Name;
                        }
                        
                    }

                }
            }

            //grec.Events
        }

        private  void GetMarriageDescription(GedcomFamilyRecord gfrec, 
            out string marDateStr, 
            out int marDateInt, 
            out string location)
        {
            
            marDateStr = "";
            marDateInt = 0;
            location = "";

            foreach (GedcomFamilyEvent gfe in gfrec.Events)
            {
                if (gfe.EventType == GedcomEvent.GedcomEventType.MARR)
                {

                    #region get date info
                    if (gfe.Date != null)
                    {
                        marDateStr = gfe.Date.DateString;

                        // try to get year value from the date time type
                        // its possible this doesnt have anything in
                        // so see if there is a year in the date string 
                        if (gfe.Date.DateTime1.HasValue)
                        {
                            marDateInt = gfe.Date.DateTime1.Value.Year;
                        }
                        else
                        {
                            Regex exp = new Regex("[1-2][0-9][0-9][0-9]", RegexOptions.IgnoreCase);

                            if (marDateStr.Length > 0)
                            {
                                Match m = exp.Match(marDateStr);
                                if (m.Success)
                                {
                                    Int32.TryParse(m.Value, out marDateInt);
                                }
                            }
                        }
                    }
                    #endregion

                    if (gfe.Place != null)
                    {
                        location = gfe.Place.Name;
                    }
                
                }

            }
            
        }


        public string ImportGeds(string folderPath, Guid sourceId)
        {
            _rootPath = folderPath;

            string returnMessage = "";

            if (!File.Exists(folderPath))
            {
                returnMessage = "File " + folderPath +  " does not exist";
                return returnMessage;
            }

            GedcomRecordReader _reader = new GedcomRecordReader();
            GedcomDatabase database = new GedcomDatabase ();
            Debug.WriteLine("importing :" + folderPath);

            bool validDB = true;

            if (folderPath.ToLower().Contains("ged"))
            {
                try
                {
                    _reader.ReadGedcom(folderPath);
                    database = _reader.Database;
                }
                catch (Exception ex)
                {
                    validDB = false;

                    returnMessage += Environment.NewLine + "ERRORS in GED";
                    returnMessage += Environment.NewLine + ex.Message; 
                }

            }
            else
            {
                returnMessage += Environment.NewLine + "problem with file name or path:";
                returnMessage += Environment.NewLine + "does not contain 'ged'";
                returnMessage += Environment.NewLine + folderPath;
                validDB = false;
            }


            if (validDB)
            {

                DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();

                deathsBirthsBLL.DeletePersonsFromSource(sourceId);
                deathsBirthsBLL.DeleteOrphanRelations();

                List<ImportPerson> personList = new List<ImportPerson>();

                try
                {
                    returnMessage += Environment.NewLine + "Creating person records";
                    personList = CreateImportedPersons(database);
                }
                catch (Exception ex1)
                {
                    returnMessage += Environment.NewLine + "ERROR " + ex1.Message;
                    personList = new List<ImportPerson>();
                }

                List<int> sourceMappingIds = new List<int>();
                List<Guid> personIds = new List<Guid>();
                if (personList.Count > 0)
                {
                    returnMessage += Environment.NewLine + "Adding person records to db";
                    SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();

                    try
                    {
                        foreach (var relation in personList)
                        {

                            Guid newSourceId = sourceId;
                            Guid UNK_LOC = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699");
                            Guid personId = deathsBirthsBLL.InsertDeathBirthRecord2(relation.IsMale,
                                                                                        relation.Name, relation.Surname,
                                                                                        relation.BirthParish, relation.BirthStr, relation.BapStr,
                                                                                        relation.DetStr, relation.DeathParish,
                                                                                        relation.FatherCName, relation.FatherSName, relation.MotherCName, relation.MotherSName,
                                                                                        relation.Source,
                                                                                        relation.Notes,
                                                                                        relation.BirthInt, relation.BapInt, relation.DetInt,
                                                                                        relation.BirthCounty, relation.DeathCounty,
                                                                                        relation.Occupation, relation.Ref_loc, relation.Ref_date,
                                                                                        0,
                                                                                        relation.Spouse_cname, relation.Spouse_sname, relation.Father_occupation,
                                                                                        UNK_LOC, 1, UNK_LOC,
                                                                                        UNK_LOC, 1, 1, Guid.Empty, 0, 0, false, false,
                                                                                        relation.XrefId,
                                                                                        relation.FatherXRef,
                                                                                        relation.MotherXRef);

                            if (personId != Guid.Empty)
                            {
                                personIds.Add(personId);



                                sourceMappingIds.Add(sourceMappingBll.Insert(newSourceId, null, null, 1, personId, DateTime.Today.ToShortDateString(), null));

                            }
                        }
                    }
                    catch (Exception ex1)
                    {
                        returnMessage += Environment.NewLine + "ERROR " + ex1.Message;
                        returnMessage += Environment.NewLine + "Rolled back added (" + personIds.Count.ToString() + ") person entries";

                        foreach (int mappingId in sourceMappingIds)
                        {
                            sourceMappingBll.DeleteByMappingId(mappingId);
                        }

                        foreach (Guid person in personIds)
                        {
                            deathsBirthsBLL.DeleteDeathBirthRecord2(person);

                        }
                    }

                    List<ImportRelationship> listRel = new List<ImportRelationship>();
                    List<ImportRelationship> listFMRel = new List<ImportRelationship>();

                    try
                    {
                        listRel = CreateMarriageRelations(sourceId, database);
                        listFMRel = CreateParentList(sourceId, database);
                    }
                    catch (Exception ex1)
                    {
                        listRel = new List<ImportRelationship>();
                        listFMRel = new List<ImportRelationship>();
                        returnMessage += Environment.NewLine + "ERROR " + ex1.Message;
                    }

                    RelationsBLL relationBll = new RelationsBLL();

                    if (listRel.Count > 0)
                    {
                        foreach (var relation in listRel)
                        {
                            if (relationBll.GetRelationByAll2(relation.Person1, relation.Person2, 8).Count() == 0)
                                relationBll.InsertRelation(relation.Person1, relation.Person2, 8, 1);
                        }
                    }
                    else
                    {
                        returnMessage += Environment.NewLine + "no marriage records found ";
                    }

                    if (listFMRel.Count > 0)
                    {
                        foreach (var relation in listFMRel)
                        {
                            if (relation.Type == 2)
                            {
                                if (relationBll.GetRelationByAll2(relation.Person1, relation.Person2, 8).Count() == 0)
                                    relationBll.InsertRelation(relation.Person1, relation.Person2, 2, 1);
                            }
                            if (relation.Type == 4)
                            {
                                if (relationBll.GetRelationByAll2(relation.Person1, relation.Person2, 8).Count() == 0)
                                    relationBll.InsertRelation(relation.Person1, relation.Person2, 4, 1);
                            }
                        }
                    }
                    else
                    {
                        returnMessage += Environment.NewLine + "no parental records found ";
                    }

                    if (listFMRel.Count == 0 && listRel.Count == 0)
                    {

                        returnMessage += Environment.NewLine + "rolled back any previously imported persons data for this source";

                        deathsBirthsBLL.DeletePersonsFromSource(sourceId);
                        deathsBirthsBLL.DeleteOrphanRelations();
                    }

                    CsUtils.UpdateDateEstimates();
                }
                else
                {
                    returnMessage += Environment.NewLine + "no person records found ";
                }
            }
            


            

            return returnMessage;
        }

        private List<ImportRelationship> CreateParentList(Guid sourceId, GedcomDatabase database)
        {
            List<ImportRelationship> listFMRel = new List<ImportRelationship>();

            DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();
         
            IList<TDBCore.EntityModel.Person> _pdt  = deathsBirthsBLL.GetBySourceId2(sourceId).ToList();

            int idx = 0;
            while (database.Individuals.Count > idx)
            {

                GedcomIndividualRecord gInd = (GedcomIndividualRecord)database.Individuals[idx];


                //_pdt[0].OrigSurname

                Guid _personId = Guid.Empty;
                Guid _fatherId = Guid.Empty;
                Guid _motherId = Guid.Empty;

                // get person
                var results = from myRow in _pdt.AsEnumerable()
                              where myRow.OrigSurname == gInd.XRefID && myRow.Surname == gInd.Names[0].Surname
                              select myRow.Person_id;

                foreach (Guid _tpGuid in results)
                    _personId = _tpGuid;





                if (gInd.ChildIn.Count > 0)
                {
                    GedcomFamilyRecord gfr = (GedcomFamilyRecord)database[gInd.ChildIn[0].Family];

                    string dummy = "";
                    string _fatherXRef = "";
                    string _motherXRef = "";

                    FatherName(database, gfr, out dummy, out dummy, out _fatherXRef);

                    MotherName(database, gfr, out dummy, out dummy, out _motherXRef);

                    // get persons father
                    results = from myRow in _pdt.AsEnumerable()
                              where myRow.OrigSurname == _fatherXRef
                              select myRow.Person_id;

                    foreach (Guid _tpGuid in results)
                        _fatherId = _tpGuid;

                    // get persons Mother
                    results = from myRow in _pdt.AsEnumerable()
                              where myRow.OrigSurname == _motherXRef
                              select myRow.Person_id;

                    foreach (Guid _tpGuid in results)
                        _motherId = _tpGuid;

                    //father
                    if (_fatherId != Guid.Empty)
                    {
                        // if (relationBll.GetRelationByAll2(_personId, _fatherId, 8).Count() == 0)
                        //     relationBll.InsertRelation(_personId, _fatherId, 2, 1);

                        listFMRel.Add(new ImportRelationship() { Person1 = _personId, Person2 = _fatherId, Type = 2 });
                    }
                    //mother
                    if (_motherId != Guid.Empty)
                    {
                        //if (relationBll.GetRelationByAll2(_personId, _motherId, 8).Count()==0)
                        //    relationBll.InsertRelation(_personId, _motherId, 4, 1);

                        listFMRel.Add(new ImportRelationship() { Person1 = _personId, Person2 = _motherId, Type = 4 });
                    }

                }


                idx++;
            }

            return listFMRel;
        }

        private List<ImportRelationship> CreateMarriageRelations(Guid sourceId, GedcomDatabase database)
        {

            DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();
            List<ImportRelationship> listRel = new List<ImportRelationship>();

            IList<TDBCore.EntityModel.Person> _pdt = deathsBirthsBLL.GetBySourceId2(sourceId).ToList();
            int idx = 0;
            while (database.Families.Count > idx)
            {
                Guid person1 = Guid.Empty;
                Guid person2 = Guid.Empty;

                Gedcom.GedcomIndividualRecord test = database.Individuals.Where(o => o.XRefID == database.Families[idx].Wife).FirstOrDefault();
                if (test != null)
                {
                    TDBCore.EntityModel.Person pRow = _pdt.Where(p => p.OrigSurname == test.XRefID).Where(p => p.Surname == test.Names[0].Surname).FirstOrDefault();
                    if (pRow != null)
                        person1 = pRow.Person_id;
                }

                Gedcom.GedcomIndividualRecord husb = database.Individuals.Where(o => o.XRefID == database.Families[idx].Husband).FirstOrDefault();
                if (husb != null)
                {
                    TDBCore.EntityModel.Person pRow = _pdt.Where(p => p.OrigSurname == husb.XRefID).Where(p => p.Surname == husb.Names[0].Surname).FirstOrDefault();
                    if (pRow != null)
                        person2 = pRow.Person_id;
                }
                if (person1 != Guid.Empty && person2 != Guid.Empty)
                {
                    listRel.Add(new ImportRelationship() { Person1 = person1, Person2 = person2 });
                }
                idx++;
            }

            return listRel;
        }

        private List<ImportPerson> CreateImportedPersons(GedcomDatabase database)
        {
            int idx = 0;

            List<ImportPerson> personList = new List<ImportPerson>();

            string name = "";
            string surname = "";
            string fatherSName = "";
            string fatherCName = "";
            string motherSName = "";
            string motherCName = "";

            string birthParish = "";
            string birthCounty = "";
            string birthCountry = "";
            int birthInt = 0;
            string birthStr = "";

            int bapInt = 0;
            string bapStr = "";

            string deathCountry = "";
            string deathCounty = "";
            string deathParish = "";
            int detInt = 0;
            string detStr = "";

            string tpDeathPlace = "";
            string tpBirthPlace = "";

            bool isMale = true;
            string motherXRef = "";
            string fatherXRef = "";

            while (database.Individuals.Count > idx)
            {
                GedcomIndividualRecord gInd = (GedcomIndividualRecord)database.Individuals[idx];

                DeathDetails(database, gInd, out tpDeathPlace, out detStr, out detInt);

                ParseAddress(tpDeathPlace, out deathParish, out deathCounty, out deathCountry);

                BirthDetails(database, gInd, out tpBirthPlace, out birthInt, out bapInt, out birthStr, out bapStr);

                ParseAddress(tpBirthPlace, out birthParish, out birthCounty, out birthCountry);

                if (gInd.Sex == GedcomSex.Male)
                    isMale = true;
                else
                    isMale = false;

                if (gInd.Names.Count > 0)
                {
                    name = gInd.Names[0].Given;
                    surname = gInd.Names[0].Surname;
                    //gInd.XRefID
                }

                #region get parent strings
                if (gInd.ChildIn.Count > 0)
                {
                    GedcomFamilyRecord gfr = (GedcomFamilyRecord)database[gInd.ChildIn[0].Family];

                    FatherName(database, gfr, out fatherCName, out fatherSName, out fatherXRef);

                    MotherName(database, gfr, out motherCName, out motherSName, out motherXRef);


                }
                #endregion


                if (bapInt != 0 && birthInt == 0)
                {
                    birthInt = bapInt;
                    birthStr = "Abt" + bapInt.ToString();
                }

                if (birthInt > 0 || detInt > 0)
                {

                    string partName = "";

                    if (surname.Length > 5)
                        partName = surname.Substring(0, 5);
                    else
                        partName = surname;


                    //if in the future i want to merge new imports with existing data
                    //do it from here


                    string occupation = "";
                    string father_occupation = "";
                    string spouse_cname = "";
                    string spouse_sname = "";
                    string ref_date = "";
                    string ref_loc = "";
                    string notes = "";

                    personList.Add(new ImportPerson()
                    {
                        IsMale = isMale,
                        Name = name,
                        Surname = surname,
                        BirthParish = birthParish,
                        BirthStr = birthStr,
                        BapStr = bapStr,
                        DetStr = detStr,
                        DeathParish = deathParish,
                        FatherCName = fatherCName,
                        FatherSName = fatherSName,
                        MotherCName = motherCName,
                        MotherSName = motherSName,
                        Source = source,
                        Notes = notes,
                        BirthInt = birthInt,
                        BapInt = bapInt,
                        DetInt = detInt,
                        BirthCounty = birthCounty,
                        DeathCounty = deathCounty,
                        Occupation = occupation,
                        Ref_loc = ref_loc,
                        Ref_date = ref_date,
                        //0, 
                        Spouse_cname = spouse_cname,
                        Spouse_sname = spouse_sname,
                        Father_occupation = father_occupation,
                        //new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"), 
                        //1,
                        //new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"),
                        //new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"),
                        //1, 1, Guid.Empty, 0, 0, false, false,
                        XrefId = gInd.XRefID,

                        //gInd.XRefID,
                        FatherXRef = fatherXRef,
                        MotherXRef = motherXRef

                    });

                }

                idx++;
            }

            return personList;
        }

        private void ParseAddress(string location,out string  parish,out string county, out string country )
        {
            parish = "";
            county = "";
            country = "";

            string[] addLines = location.Split(',');
            string tp = "";
            #region get address parts

            if (addLines.Length > 0)
            {
                if (addLines.Length == 1)
                {
                    if (!IsCountry(addLines[0], out country) && !IsCounty(addLines[0], out county))
                    {
                        parish = addLines[0];
                    }
                    

                }
                else
                {
                    int commaIdx = 0;
                    foreach (string line in addLines)
                    {
                        
                         tp = "";

                      

                        if (IsCountry(line.Trim(), out tp))
                        {
                            country = tp.Trim();
                            break;
                        }
                        else
                            if (IsCounty(line.Trim(), out tp))
                        {
                            county = tp.Trim();
                        }
                        else
                        {
                            if(commaIdx ==0)
                                parish = line.Trim();
                            else
                                parish = parish + "," + line.Trim();
                        }

                        commaIdx++;
                    }
                }
            }

            #endregion

            tp = parish;
            if (county == "")
            {

                CountyDictionaryBLL countyDictionaryBLL = new CountyDictionaryBLL();

             //  IEnumerable<DsCountyDictionary.CountyDictionaryRow> rowCollection =  dsCountyDictionary.Where(o => o.dictPlace.ToLower().Trim() == tp.ToLower().Trim());

               foreach (var cdr in countyDictionaryBLL.GetDictionary2())
                {
                    county = cdr.dictCounty;
                    break;
                }
            }

        }

        private bool IsCountry(string param, out string country)
        {
            country = "";

            param = param.Trim().ToLower();

            #region waffle in a switch
            switch (param)
            { 
                case "england":
                    country = "England";
                    break;
                case "wales":
                    country = "Wales";
                    break;
                case "scotland":
                    country = "Scotland";
                    break;
                
                case "britain":
                case "united kingdon":
                case "uk":
                case "great britain":
                case "british isles":
                case "the british isles":
                    country = "United Kingdom";
                    break;

                case "usa":
                case "us":
                case "united states":
                case "united states of america":
                case "the united states of america":
                    country = "USA";
                    break;

                case "australia":
                    country = "Australia";
                    break;

                case "new zealand":
                    country = "New Zealand";
                    break;

                case "canada":
                    country = "Canada";
                    break;

                default:
                    break;
            }
            #endregion

            if (country == "")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool IsCounty(string param, out string countyName)
        {

            countyName = "";

            // see if the param is a county if it is correct it!.

            param = param.Trim().ToLower();

            #region waffle
            switch (param)
             {
                 case "bedfordshire": 
                 case "beds":
                    countyName = "Bedfordshire";
                    break;

                 case "berkshire": 
                 case "berks":
                    countyName = "Berkshire";
                    break;

                 case "bristol":
                    countyName = "Bristol";
                    break;

                 case "buckinghamshire": 
                 case "bucks":
                    countyName = "Buckinghamshire";
                    break;
        
                 case "cambridgeshire": 
                 case "cambs":
                    countyName = "Cambridgeshire";
                    break;

                 case "cheshire": 
                 case "ches":
                    countyName = "Cheshire";
                    break;

                 case "city of london":
                 case "london":
                    countyName = "City of London";
                    break;

                 case "cornwall": 
                 case "cornwl":
                    countyName = "Cornwall";
                    break;

                 case "cumbria": 
                 case "cumbri":
                    countyName = "Cumbria";
                    break;


                 case "derbyshire": 
                 case "derbys":
                    countyName = "Derbyshire";
                    break;

                 case "devon":
                    countyName = "Devon";
                    break;

                 case "dorset":
                    countyName = "Dorset";
                    break;

                 case "durham":
                    countyName = "Durham";
                    break;

                 case "east riding of yorkshire": 
                 case "east riding" :
                    countyName = "East Riding of Yorkshire";
                    break;

                 case "east sussex": 
                 case "e susx":
                    countyName = "East Sussex";
                    break;

                 case "essex":
                    countyName = "Essex";
                    break;
  
                 case "gloucestershire": 
                 case "gloucs":
                    countyName = "Gloucestershire";
                    break;
 
                 case "greater london": 
                 
                    countyName = "Greater London";
                    break;
 
                 case "greater manchester": 
                    countyName = "Greater Manchester";
                    break;


                 case "hampshire": 
                 case "hants":
                    countyName = "Hampshire";
                    break;

                 case "herefordshire":
                    countyName = "Herefordshire";
                    break;

                 case "hertfordshire": 
                 case "herts":
                    countyName = "Hertfordshire";
                    break;


                 case "isle of wight":
                    countyName = "Isle of Wight";
                    break;
 
                 case "kent":
                    countyName = "Kent";
                    break;
 
                 case "lancashire": 
                 case "lancs":
                    countyName = "Lancashire";
                    break;
 
                 case "leicestershire": 
                 case "leics":
                    countyName = "Leicestershire";
                    break;
  
                 case "lincolnshire": 
                 case "lincs":
                    countyName = "Lincolnshire";
                    break;
 
                 case "merseyside":
                    countyName = "Merseyside";
                    break;
  
                 case "norfolk": 
                 case "norflk":
                    countyName = "Norfolk";
                    break;
 
                 case "north yorkshire": 
                 case "n york":
                    countyName = "North Yorkshire";
                    break;
  
                 case "northamptonshire": 
                 case "nhants":
                    countyName = "Northamptonshire";
                    break;
  
                 case "northumberland": 
                 case "nthumb":
                    countyName = "Northumberland";
                    break;
                 case "nottinghamshire": 
                 case "notts":
                    countyName = "Nottinghamshire";
                    break;
 
                 case "oxfordshire": 
                 case "oxfds":
                    countyName = "Oxfordshire";
                    break;
 
                 case "rutland":
                    countyName = "Rutland";
                    break;

                 case "shropshire":
                 case "shrops":
                    countyName = "Shropshire";
                    break;

                 case "somerset": 
                 case "somset":
                    countyName = "Somerset";
                    break;

                 case "south yorkshire":
                    countyName = "South Yorkshire";
                    break;

                 case "staffordshire": 
                 case "staffs":
                    countyName = "Staffordshire";
                    break;

                 case "suffolk": 
                 case "sufflk":
                    countyName = "Suffolk";
                    break;

                 case "surrey":
                    countyName = "Suffolk";
                    break;

                 case "tyne and wear":
                    countyName = "Tyne and Wear";
                    break;
 
                 case "warwickshire":
                 case "warks":
                    countyName = "Warwickshire";
                    break;
  
                 case "west midlands":
                    countyName = "West Midlands";
                    break;

                 case "west sussex":
                 case "w sussx":
                    countyName = "West Sussex";
                    break;
  
                 case "west yorkshire":
                    countyName = "West Yorkshire";
                    break;
 
                 case "wiltshire":
                 case "wilts":
                    countyName = "Wiltshire";
                    break;

                 case "worcestershire": 
                 case "worcs":
                    countyName = "Worcestershire";
                    break;
                 
                 case "yorkshire":
                 case "yorks":
                    countyName = "Yorkshire";
                    break;

                 case "huntingdonshire":
                 case "hunts":
                    countyName = "Huntingdonshire";
                    break;
  
                 case "aberdeenshire":
                    countyName = "Aberdeenshire";
                    break;
 
                 case "angus":
                    countyName = "Angus";
                    break;
 
                 case "argyllshire":
                    countyName = "Argyllshire";
                    break;
  
                 case "ayrshire":
                    countyName = "Ayrshire";
                    break;

                 case "banffshire":
                    countyName = "Banffshire";
                    break;
 
                 case "berwickshire":
                    countyName = "Berwickshire";
                    break;
 
                 case "buteshire":
                    countyName = "Buteshire";
                    break;
  
                 case "cromartyshire":
                    countyName = "Cromartyshire";
                    break;
 
                case "caithness":
                    countyName = "Caithness";
                    break;
  
                case "clackmannanshire":
                    countyName = "Clackmannanshire";
                    break;
  
                case "dumfriesshire":
                    countyName = "Dumfriesshire";
                    break;
 
                case "dunbartonshire":
                    countyName = "Dunbartonshire";
                    break;
 
                case "east lothian":
                    countyName = "East Lothian";
                    break;
 
                case "fife":
                    countyName = "East Lothian";
                    break;
 
                case "inverness-shire":
                case "invernessshire":

                    countyName = "Inverness-shire";
                    break;
  
                case "kincardineshire":
                    countyName = "Kincardineshire";
                    break;
  
                case "kinross":
                    countyName = "Kinross";
                    break;
 
                case "kirkcudbrightshire":
                    countyName = "Kirkcudbrightshire";
                    break;
 
                case "lanarkshire":
                    countyName = "Lanarkshire";
                    break;
 
                case "midlothian":
                    countyName = "Midlothian";
                    break;
 
                case "morayshire":
                    countyName = "Morayshire";
                    break;
  
                case "nairnshire":
                    countyName = "Nairnshire";
                    break;
 
                case "orkney":
                    countyName = "Orkney";
                    break;
  
                case "peeblesshire":
                    countyName = "Peeblesshire";
                    break;
 
                case "perthshire":
                    countyName = "Perthshire";
                    break;
 
                case "renfrewshire":
                    countyName = "Renfrewshire";
                    break;
 
                case "ross-shire":
                case "rossshire":
                    countyName = "Ross-shire";
                    break;
 
                case "roxburghshire":
                    countyName = "Roxburghshire";
                    break;
 
                case "selkirkshire":
                    countyName = "Selkirkshire";
                    break;
 
                case "shetland":
                    countyName = "Shetland";
                    break;
 
                case "stirlingshire":
                    countyName = "Shetland";
                    break;
 
                case "sutherland":
                    countyName = "Sutherland";
                    break;
  
                case "west lothian":
                    countyName = "West Lothian";
                    break;
 
                case "wigtownshire":
                    countyName = "Wigtownshire";
                    break;


 
                case "anglesey":
                    countyName = "Anglesey";
                    break;
 
                case "brecknockshire":
                    countyName = "Brecknockshire";
                    break;
  
                case "caernarfonshire":
                    countyName = "Caernarfonshire";
                    break;
 
                case "carmarthenshire":
                    countyName = "Carmarthenshire";
                    break;
 
                case "cardiganshire":
                    countyName = "Cardiganshire";
                    break;
 
                case "denbighshire":
                    countyName = "Denbighshire";
                    break;
 
                case "flintshire":
                    countyName = "Flintshire";
                    break;
 
                case "glamorgan":
                    countyName = "Glamorgan";
                    break;
 
                case "merioneth":
                    countyName = "Merioneth";
                    break;
  
                case "monmouthshire":
                    countyName = "Monmouthshire";
                    break;
  
                case "montgomeryshire":
                    countyName = "Montgomeryshire";
                    break;
 
                case "pembrokeshire":
                    countyName = "Pembrokeshire";
                    break;
 
                case "radnorshire":
                    countyName = "Radnorshire";
                    break;



                case "antrim":
                    countyName = "Antrim";
                    break;

                case "armagh":
                    countyName = "Armagh";
                    break;

                case "down":
                    countyName = "Down";
                    break;

                case "fermanagh":
                    countyName = "Fermanagh";
                    break;

                case "londonderry":
                    countyName = "Londonderry";
                    break;

                case "tyrone":
                    countyName = "Tyrone";
                    break;

                default:
                    break;
             }
            #endregion

            if (countyName == "")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private void DeathDetails(Gedcom.GedcomDatabase database, GedcomIndividualRecord ifr,
            out string place, out string detStr, out int detInt)
        {
            place = "";
            detStr = "";
            detInt = 0;

            if(ifr.Death != null)
            {
                if (ifr.Death.Place != null)
                {
                    place = ifr.Death.Place.Name;
                }

                if (ifr.Death.Date != null)
                {
                    detStr = ifr.Death.Date.DateString;

                    // try to get year value from the date time type
                    // its possible this doesnt have anything in
                    // so see if there is a year in the date string 
                    if (ifr.Death.Date.DateTime1.HasValue)
                    {
                        detInt = ifr.Death.Date.DateTime1.Value.Year;
                    }
                    else
                    {
                        Regex exp = new Regex("[1-2][0-9][0-9][0-9]", RegexOptions.IgnoreCase);

                        if (detStr.Length > 0)
                        {
                            Match m = exp.Match(detStr);
                            if (m.Success)
                            {
                                Int32.TryParse(m.Value, out detInt);
                            }
                        }
                    }
                }

            }

        }

        private void BirthDetails(Gedcom.GedcomDatabase database, GedcomIndividualRecord ifr,
            out string place, 
            out int birthInt, 
            out int bapInt, 
            out string birthStr, 
            out string bapStr)
        {
            place = "";
            bapInt = 0;
            birthInt = 0;
            birthStr = "";
            bapStr = "";

            if(ifr.Names.Count >0 && ifr.Names[0].Surname.Contains("Herbert"))
            {
                Debug.WriteLine("");
            }

            if (ifr.Birth != null)
            {
                #region place
                if (ifr.Birth.Place != null)
                {
                    place = ifr.Birth.Place.Name;

                }
                #endregion

                if (ifr.Birth.Date != null)
                {
                    birthStr = ifr.Birth.Date.DateString;

                    // try to get year value from the date time type
                    // its possible this doesnt have anything in
                    // so see if there is a year in the date string 
                    if (ifr.Birth.Date.DateTime1.HasValue)
                    {
                        birthInt = ifr.Birth.Date.DateTime1.Value.Year;
                    }
                    else
                    {
                        Regex exp = new Regex("[1-2][0-9][0-9][0-9]", RegexOptions.IgnoreCase);

                        if (birthStr.Length > 0)
                        {
                            Match m = exp.Match(birthStr);
                            if (m.Success)
                            {
                                Int32.TryParse(m.Value, out birthInt);
                            }
                        }
                    }
                }

            }
            
            GedcomIndividualEvent gie = ifr.FindEvent(GedcomEvent.GedcomEventType.CHR);

            if (gie != null)
            {
                #region place
                if (gie.Place != null)
                {
                    if (place.Length == 0)
                    {
                        place = gie.Place.Name;
                    }
                }
                #endregion

                if (gie.Date != null)
                {
                    bapStr = gie.Date.DateString;

                    // try to get year value from the date time type
                    // its possible this doesnt have anything in
                    // so see if there is a year in the date string 
                    if (gie.Date.DateTime1.HasValue)
                    {
                        bapInt = gie.Date.DateTime1.Value.Year;
                    }
                    else
                    {
                        Regex exp = new Regex("[1-2][0-9][0-9][0-9]", RegexOptions.IgnoreCase);

                        if (bapStr.Length > 0)
                        {
                            Match m = exp.Match(bapStr);
                            if (m.Success)
                            {
                                Int32.TryParse(m.Value, out bapInt);
                            }
                        }
                    }
                }
            }
            
            GedcomIndividualEvent gie2 = ifr.FindEvent(GedcomEvent.GedcomEventType.BAPM);

            if (gie2 != null)
            {
                #region place
                if (gie2.Place != null)
                {
                    if (place.Length == 0)
                    {
                        place = gie2.Place.Name;
                    }
                }
                #endregion

                if (bapInt == 0 && bapStr.Length == 0)
                {
                    if (gie2.Date != null)
                    {
                        bapStr = gie2.Date.DateString;

                        // try to get year value from the date time type
                        // its possible this doesnt have anything in
                        // so see if there is a year in the date string 
                        if (gie2.Date.DateTime1.HasValue)
                        {
                            bapInt = gie2.Date.DateTime1.Value.Year;
                        }
                        else
                        {
                            Regex exp = new Regex("[1-2][0-9][0-9][0-9]", RegexOptions.IgnoreCase);

                            if (bapStr.Length > 0)
                            {
                                Match m = exp.Match(bapStr);
                                if (m.Success)
                                {
                                    Int32.TryParse(m.Value, out bapInt);
                                }
                            }
                        }
                    } 
                }
            }
           
            

        }

        private void FatherName(Gedcom.GedcomDatabase database, GedcomFamilyRecord gfr,
            out string cName ,out string sName, out string xRef)
        {
            cName = "";
            sName = "";
            xRef = "";

            if (gfr.Husband != null)
            {
                GedcomIndividualRecord father = (GedcomIndividualRecord)database[gfr.Husband];
                if (father != null)
                {
                    if (father.Names.Count > 0)
                    {
                        //Debug.WriteLine("father: " + father.Names[0].Name);
                        cName = father.Names[0].Given;
                        sName = father.Names[0].Surname;
                        xRef = father.XRefID;
                    }
                }
            }
        }

        private void MotherName(Gedcom.GedcomDatabase database, GedcomFamilyRecord gfr,
            out string cName, out string sName, out string xRef)
        {
            cName = "";
            sName = "";
            xRef = "";
            if (gfr.Wife != null)
            {
                GedcomIndividualRecord mother = (GedcomIndividualRecord)database[gfr.Wife];
                if (mother != null)
                {
                    if (mother.Names.Count > 0)
                    {
                        //Debug.WriteLine("father: " + father.Names[0].Name);
                        cName = mother.Names[0].Given;
                        sName = mother.Names[0].Surname;
                        xRef = mother.XRefID;
                    }
                }
            }
        }


        #region get file list -without recursion
        public static List<string> GetFilesRecursive(string b)
        {
            // 1.
            // Store results in the file results list.
            List<string> result = new List<string>();

            // 2.
            // Store a stack of our directories.
            Stack<string> stack = new Stack<string>();

            // 3.
            // Add initial directory.
            stack.Push(b);

            // 4.
            // Continue while there are directories to process
            while (stack.Count > 0)
            {
                // A.
                // Get top directory
                string dir = stack.Pop();

                try
                {
                    // B
                    // Add all files at this directory to the result List.
                    result.AddRange(Directory.GetFiles(dir, "*.*"));

                    // C
                    // Add all directories at this directory.
                    foreach (string dn in Directory.GetDirectories(dir))
                    {
                        stack.Push(dn);
                    }
                }
                catch
                {
                    // D
                    // Could not open the directory
                }
            }
            return result;
        }
        #endregion
    
    
    
    
    
    }
}
