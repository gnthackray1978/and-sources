using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using GedcomParser;
using TDBCore.BLL;

namespace TDBCore.Types.libs
{
    public class CsUtils
    {
        public class personRecord
        {
            public string cname = "";
            public string sname = "";
            public string date = "";
            public string source = "";
        }


        //public static System.Data.EntityClient.EntityConnection GetConn()
        //{

        //    //Debug.WriteLine(TDBCore.Properties.Settings.Default.ThackrayDBConnectionString);

        //    //Debug.WriteLine(TDBCore.Properties.Settings.Default.YorkshireParishsConnectionString);

        //    //Debug.WriteLine(TDBCore.Properties.Settings.Default.WorkConnString);


        //    SqlConnectionStringBuilder sconStrBuilder = new SqlConnectionStringBuilder(TDBCore.Properties.Settings.Default.ThackrayDBConnectionString);



        //    string entConStr = @"metadata=res://*/EntityModel.GeneralModel.csdl|res://*/EntityModel.GeneralModel.ssdl|res://*/EntityModel.GeneralModel.msl;provider=System.Data.SqlClient;provider connection string="";Data Source=" +
        //                       sconStrBuilder.DataSource + @";Initial Catalog=" + sconStrBuilder.InitialCatalog + @";Integrated Security=" + sconStrBuilder.IntegratedSecurity + @";MultipleActiveResultSets=True"";";

        //    System.Data.EntityClient.EntityConnection connStr = new System.Data.EntityClient.EntityConnection(entConStr);
        //    //  (@"metadata=res://*/CustomSearches.csdl|res://*/CustomSearches.ssdl|res://*/CustomSearches.msl;provider=System.Data.SqlClient;
        //    //provider connection string="";Data Source=GRN-P005718\;Initial Catalog=ThackrayDB;Integrated Security=True;MultipleActiveResultSets=True"";");
            
           
        //    //metadata=res://*/EntityModel.GeneralModel.csdl|res://*/EntityModel.GeneralModel.ssdl|
        //    //res://*/EntityModel.GeneralModel.msl;provider=System.Data.SqlClient;provider connection string=
        //    //&quot;Data Source=GEORGE-PC\SQLEXPRESS;Initial Catalog=ThackrayDB;Integrated Security=True;MultipleActiveResultSets=True&quot;"


        //    return connStr;
        //}



        public static string MakeDateString(string datebapstr,string datebirthstr, string datedeath, string years, string months, string weeks, string days)
        {                
            if (datebapstr == "" && datebirthstr == "" && datedeath != "" &&
                (years != "" || months != "" || weeks != "" || days != ""))
            {
                var deathDate = new DateTime(2100, 1, 1);

                if (!DateTime.TryParse(datedeath, out deathDate))
                {
                    int deathYear = CsUtils.GetDateYear(datedeath);
                    if (deathYear != 0)
                    {
                        deathDate = new DateTime(deathYear, 1, 1);
                    }
                }

                if (deathDate.Year != 2100)
                {
                    int iyears = 0;
                    int imonths = 0;
                    int iweeks = 0;
                    int idays = 0;

                    Int32.TryParse(years, out iyears);
                    Int32.TryParse(months, out imonths);
                    Int32.TryParse(weeks, out iweeks);
                    Int32.TryParse(days, out idays);

                    idays = (iyears*365) + (imonths*28) + (iweeks*7) + idays;

                    var ts = new TimeSpan(idays, 1, 1, 1);


                    DateTime birthDate = deathDate.Subtract(ts);

                    datebirthstr = birthDate.ToString("dd MMM yyyy");

                }

            }


            return datebirthstr;
        }

        public static bool ValidYear(string inputStr)
        {
            bool validYear = false;

            int year = GetDateYear(inputStr);

            if (GetDateYear(inputStr) != 0)
            {
                validYear = CsUtils.ValidYearRange(year);


            }
            

            return validYear;
        }
        
        public static bool ValidYear(string inputStr, out int year)
        {
            bool validYear = false;
            
            year = GetDateYear(inputStr);

            if (year !=0)
            {
                validYear = CsUtils.ValidYearRange(year);

               
            }
            else
            {
                validYear = false;
                
            }

            return validYear;
        }

        public static bool ValidYearRange(int year)
        {
            if (year > 1000 && year < 2100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ValidateDate(string dBap, out string dateString, out int intDate)
        {


            Regex exp = new Regex("[1-2][0-9][0-9][0-9]", RegexOptions.IgnoreCase);
            Match m = exp.Match(dBap);
            if (m.Success)
            {
                Int32.TryParse(m.Value, out intDate);

                DateTime result = DateTime.Today;
                // try to tidy it up, if that cant be done dont worry about it.
                if (DateTime.TryParse(dBap, out result))
                {
                    dateString = result.ToShortDateString();

                }
                else
                {
                    dateString = dBap;
                }



                return true;
            }
            else
            {
                dateString = dBap;
                intDate = 0;
                return false;

            }
        }

        public static int GetDateYear(string _date)
        {

            int retVal = 0;


            //"^[0-9]+$
            Regex regex = new Regex(@"\d\d\d\d+");

            Match _match = regex.Match(_date);



            if (_match.Success)
            {

                retVal = Convert.ToInt32(_match.Value);

            }



            return retVal;

        }

        

        public static void CalcEstDates(int birthInt, int bapInt, int deathInt, out int estBirth, out int estDeath, out bool isEstBirth, out bool isEstDeath, string fatName, string moName)
        {
            estBirth = 0;
            estDeath = 0;
            isEstBirth = false;
            isEstDeath = false;

            if (bapInt > 0)
                estBirth = bapInt;
            else
                estBirth = birthInt;


            estDeath = deathInt;



            if ((bapInt == 0 && birthInt == 0) && deathInt > 0)
            {
                isEstBirth = true;

                //died in childhood average age is 10years
                //
                if (fatName.Length > 0 ||
                    moName.Length > 0)
                {
                    estBirth = deathInt - 10;
                }
                else
                { // normal average life expectancy about 50 years
                    estBirth = deathInt - 50;
                }


            }

            if (deathInt == 0 && (bapInt > 0 || birthInt > 0))
            {
                isEstDeath = true;

                if (bapInt > 0)
                    estDeath = bapInt + 50;
                else
                    estDeath = birthInt + 50;

            }




        }

        public static string MakePercentage(int total, int current)
        {
            decimal percentage = decimal.Zero;

            decimal onepercent = Convert.ToDecimal( 100)/ total ;

            percentage = onepercent * current;


            return percentage.ToString("N4");
        }


        #region junk
        private GedcomRecordReader _reader;
        //private GedcomDatabase _database;
        private GedcomRecordWriter _writer;
        //  private string currentlyOpenFile = "";
      
        List<int> listBaptismYearsAdded = new List<int>();
        List<int> listMarriageYearsAdded = new List<int>();


        public static string Capitalize(string value)
        {
            value = value.ToLower();
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
        }

        public void InsertLoomesSources()
        {
            ParishTranscriptionsBll parishTranscriptionsBll = new ParishTranscriptionsBll();
            //DsParishTranscriptions.ParishTranscriptionDetailsDataTable dtParishTranscriptions = new DsParishTranscriptions.ParishTranscriptionDetailsDataTable();
            
            //  dtParishTranscriptions = parishTranscriptionsBll.GetParishTranscriptionsByTranscriptionId(21377);
            var dtParishTranscriptions = parishTranscriptionsBll.GetParishTranscriptionsByLoomes2();

            foreach (var ptr in dtParishTranscriptions)
            {
                if (ptr.ParishDataString.Contains(","))
                {
                    string[] parts = Regex.Split(ptr.ParishDataString, ",");

                    foreach (string part in parts)
                    {
                        parseLoomesString(ptr.Parish.ParishId,part);
                    }
                }
                else
                {
                    parseLoomesString(ptr.Parish.ParishId, ptr.ParishDataString);
                }



            }

        }

        private void parseLoomesString(Guid paramParishId, string paramLoomStr)
        {
            Regex _reg = new Regex(@"\d\d\d\d");

            MatchCollection _matches = _reg.Matches(paramLoomStr);
            //   DsParishs.ParishsDataTable pdt = new DsParishs.ParishsDataTable();
            ParishsBll parishsBll = new ParishsBll();
            var pdt = parishsBll.GetParishById2(paramParishId);

            if (_matches.Count > 0)
            {
                string sourceFromStr = "1/1/" + _matches[0].Value.Trim();
                string sourceToStr = "31/12/" + _matches[1].Value.Trim();
                int sourceFromInt = Convert.ToInt32(_matches[0].Value);
                int sourceToInt = Convert.ToInt32(_matches[1].Value);

                SourceBll sourcesBll = new SourceBll();
                SourceMappingsBll sourceMappingsBll = new SourceMappingsBll();


                List<Guid> selectedParishIds = new List<Guid>();
                selectedParishIds.Add(paramParishId);
                List<int> selectedSourceTypes = new List<int>();
                selectedSourceTypes.Add(22);

                Guid sourceId = sourcesBll.InsertSource2(pdt.ParishName + " Loomes Marriages", "SOG", false, true, false, 1,
                                                         sourceFromStr, sourceToStr, sourceFromInt, sourceToInt,
                                                         pdt.ParishName + "LM" + sourceFromInt.ToString() + sourceToInt.ToString(),0,"");

                sourceMappingsBll.WriteParishsToSource(sourceId, selectedParishIds, 1);
                sourceMappingsBll.WriteSourceTypesToSource(sourceId, selectedSourceTypes, 1);



                UpdateMissingRecords(paramParishId,sourceFromInt ,sourceToInt );

            }
         
        
        }

        public static bool InDesignMode
        {
            get
            {
                try
                {
                    return System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv";
                }
                catch
                {
                    return false;
                }
            }
        }

        public void UpdateMissingRecords(Guid parishId, int SourceDate, int SourceDateTo)
        {

            ParishRecordsBll parishRecordsBll = new ParishRecordsBll();
           
            MissingParishRecordsBll missingParishRecordsBll = new MissingParishRecordsBll();     
            //BLL.ParishRegSourcesBLL parishRegSourcesBll = new ParishRegSourcesBLL();

            //    DsParishRegisterSources.ParishRegSourcesDataTable parishRegSourcesDT = new DsParishRegisterSources.ParishRegSourcesDataTable();
            //      DsMissingRecords.MissingRecordsDataTable missingRecordsTable = new DsMissingRecords.MissingRecordsDataTable();
          
            //  DsParishRecords.ParishRecordsDataTable prdt = new DsParishRecords.ParishRecordsDataTable();
            //List<ParishRecord> prdt 

           
            //   missingRecordsTable = missingParishRecordsBll.GetMissingRecords(parishId);

            //  if (missingRecordsTable.Count > 0)
            //  {
            foreach (var mRR in missingParishRecordsBll.GetMissingRecords2(parishId))
            {

                if (mRR.RecordType.Trim() == "M")
                {
                    int startYear = mRR.Year.Value;
                    int endYear = mRR.YearEnd.Value;

                    #region code for updating source missing date ranges

                    if ((mRR.Year > SourceDate) && (mRR.YearEnd < SourceDateTo) ||
                        (mRR.Year == SourceDate) && (mRR.YearEnd == SourceDateTo))
                    {
                        missingParishRecordsBll.DeleteMissingRecord2(mRR.MissingRecordId);
                    }
                    else
                    {
                        if ((mRR.Year < SourceDate) && (mRR.YearEnd > SourceDateTo))
                        {
                            endYear = SourceDate;

                            missingParishRecordsBll.UpdateMissingParishRecord2(mRR.MissingRecordId, startYear, endYear);


                            missingParishRecordsBll.InsertMissingParishRecord2(mRR.Parish.ParishId, mRR.DataTypeId.Value, SourceDateTo, mRR.RecordType
                                                                               , mRR.OriginalRegister.Value, mRR.YearEnd.Value);

                        }
                        else
                        {
                            if ((mRR.Year < SourceDate) && (mRR.YearEnd > SourceDate)) endYear = SourceDate;

                            if ((mRR.Year < SourceDateTo) && (mRR.YearEnd > SourceDateTo)) startYear = SourceDateTo;


                            missingParishRecordsBll.UpdateMissingParishRecord2(mRR.MissingRecordId, startYear, endYear);
                        }


                    }

                    #endregion
                }
            }
            //  }

        }

        public void ParseMarriageTable()
        {
            //    DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();
            //    MarriagesBLL marriagesBll = new MarriagesBLL();

            
            //    ParishsBLL parishRecBll = new ParishsBLL();


            //    DsMarriages.MarriagesDataTable marTab = new DsMarriages.MarriagesDataTable();
            // //   DsParishs.ParishsDataTable precTab = new DsParishs.ParishsDataTable();
            
            //    //DsDeathsBirths.PersonsDataTable dtBirthsBirths = new DsDeathsBirths.PersonsDataTable();

            //    //CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
            //    //dtBirthsBirths = deathsBirthsBll.GetDeathsBirths();

            //    //Guid birtlocatId = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699");
            //    Guid marLocatId = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699");

            
            //    //dtBirthsBirths = deathsBirthsBll.GetByLocation(detlocatId, birtlocatId, birthLocat, deathLocat, birthCounty, deathCounty);

            //    string county = "";

            //    marTab = marriagesBll.GetMarByLocat("", county);

            //    foreach (DsMarriages.MarriagesRow mar in marTab)
            //    {


            //        if (mar.MarriageLocationId != marLocatId)
            //        {
            //            mar.Delete();
            //        }


            //    }

            //    marTab.AcceptChanges();



            ////    var precTab = parishRecBll.GetParishs2();

            //    int idx = 0;



            //    foreach (DsMarriages.MarriagesRow mar in marTab)
            //    {
            //        idx++;
            //        decimal percent = (Convert.ToDecimal(100) / Convert.ToDecimal(marTab.Count)) * idx;

            //        //   message.Text = percent.ToString("N2");


            //        if (mar.MarriageLocationId != marLocatId) continue;
            //        if (mar.MarriageLocation.Trim() == "") continue;
                 


            //        Guid locationId = mar.MarriageLocationId;
            //        foreach (var parishRow in parishRecBll.GetParishs2())
            //        {
            //            if (mar.MarriageLocation.ToLower().Contains(parishRow.ParishName.ToLower().Trim()))
            //            {
            //                if (parishRow.ParishCounty.ToLower().Trim() == mar.MarriageCounty.Trim().ToLower())
            //                    locationId = parishRow.ParishId;
            //            }
            //        }

            //        string marLocation = mar.MarriageLocation;

            //        //  update entry


            //        marriagesBll.UpdateMarriageLocat(
            //            mar.Marriage_Id, 
            //            mar.FemaleLocation, 
            //            mar.MaleLocation, 
            //            mar.MarriageCounty,
            //            mar.MarriageLocation,
            //            locationId,
            //            mar.MaleLocationId,
            //            mar.FemaleLocationId);


            //    }



        }



        public void ParsePersonsTable()
        {
            DeathsBirthsBll deathsBirthsBll = new DeathsBirthsBll();
            ParishsBll parishRecBll = new ParishsBll();
            // DsParishs.ParishsDataTable precTab = new DsParishs.ParishsDataTable ();
            //   DsDeathsBirths.PersonsDataTable dtBirthsBirths = new DsDeathsBirths.PersonsDataTable();

            //CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
            //dtBirthsBirths = deathsBirthsBll.GetDeathsBirths();

            Guid birtlocatId = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699");
            Guid detlocatId = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699");

            string birthLocat = "";
            string deathLocat = "";
            string birthCounty = "";
            string deathCounty = "";



            var dtBirthsBirths = deathsBirthsBll.GetByLocation2(detlocatId, birtlocatId, birthLocat, deathLocat, birthCounty, deathCounty).ToList();

            //  precTab = parishRecBll.GetParishs();

            int idx = 0;



            foreach (var person in dtBirthsBirths)
            {
                idx++;
                decimal percent = (Convert.ToDecimal( 100) / Convert.ToDecimal(dtBirthsBirths.Count)) * idx;

                //   message.Text = percent.ToString("N2");

            
                if (person.BirthLocation.Trim() == "") continue;

                Guid locationId = person.BirthLocationId;
                foreach (var parishRow in parishRecBll.GetParishs2())
                {
                    if (person.BirthLocation.ToLower().Contains(parishRow.ParishName.ToLower().Trim()))
                    {
                        locationId = parishRow.ParishId;
                    }
                }

                string birthLocation = person.BirthLocation;
                //  string deathLocation = person.DeathLocation;

                //birthLocation = birthLocation.Replace("Saint Leonard,New Malton", "New Malton St Leonard Parish");
                //birthLocation = birthLocation.Replace("Bland Hill", "Bland Hill, Fewston");
                //birthLocation = birthLocation.Replace("Saint Sampson,York", "York St Sampson");
                //birthLocation = birthLocation.Replace("Hampethwaite,", "Hampsthwaite");
                //birthLocation = birthLocation.Replace("Norwood,", "Norwood,Fewston");
                //birthLocation = birthLocation.Replace("Bilton Ainsty", "Bilton (Ainsty)");
                //birthLocation = birthLocation.Replace("Holy Trinity Micklegate,York", "York Holy Trinity Micklegate");
                //birthLocation = birthLocation.Replace("Witton Le Wear", "");
                //birthLocation = birthLocation.Replace("Saint John Ousebridge,York", "York St John");
                //birthLocation = birthLocation.Replace("0f,York", "York");
                //birthLocation = birthLocation.Replace("Pateleybridge,", "Pateley Bridge");
                //birthLocation = birthLocation.Replace("Snowden,", "Snowden, Weston");
                //birthLocation = birthLocation.Replace("Saint John And Saint Martin,Beverley", "Beverley Saint John OR Saint Mary");
                //birthLocation = birthLocation.Replace("Saint Maurice,York", "York St Maurice");
                //birthLocation = birthLocation.Replace("Saint Mary Bishophill Senior,York", "York St Mary Bishophill the Elder");
                //birthLocation = birthLocation.Replace("Saint Michael-Le-Belfry,York", "York St Michael le Belfrey");
                //birthLocation = birthLocation.Replace("Kildwick", "Kildwick in Craven");
                //birthLocation = birthLocation.Replace("Ryton", "Ryton, Kirby Misperton");
                //birthLocation = birthLocation.Replace("Saint Mary Bishophill Junior,York", "York St Mary Bishophill the Younger");
                //birthLocation = birthLocation.Replace("Brame Lane,", "Fewston");
                //birthLocation = birthLocation.Replace("Saint Helen,York", "York St Helen");
                //birthLocation = birthLocation.Replace("St Mary Bishophill,Jr,York", "York St Mary Bishophill the Younger");
                //birthLocation = birthLocation.Replace("St Martin And St Gregory,York", "York St Martin Coney Street OR St Martin And St Gregory");
                //birthLocation = birthLocation.Replace("St Lawrence,York", "York St Lawrence");
                //birthLocation = birthLocation.Replace("St Michael Le Belfrey,York", "York St Michael le Belfrey");
                //birthLocation = birthLocation.Replace("York,Yorkshire", "York");
                //birthLocation = birthLocation.Replace("St Mary Bishophill Senior,York", "York St Mary Bishophill the Elder");
                //birthLocation = birthLocation.Replace("Saint Olave,York", "York St Olave");
                //birthLocation = birthLocation.Replace("Saint Laurence,York", "York St Lawrence");
                //birthLocation = birthLocation.Replace("Ousebridge'S,York", "York St John");
                //birthLocation = birthLocation.Replace("Norwood,Kewston,", "Norwood, Fewston");
                //birthLocation = birthLocation.Replace("Kirkby,Overblow,", "Kirkby Overblow");
                //birthLocation = birthLocation.Replace("Linton,Yorkshire", "Linton in Craven");
                //birthLocation = birthLocation.Replace("Saint Crux,York", "York St Crux");
                //birthLocation = birthLocation.Replace("Saint Mary Castlegate,York", "York St Mary Castlegate");
                //birthLocation = birthLocation.Replace("Saint Margaret,York", "York St Margaret");
                //birthLocation = birthLocation.Replace("Holy Trinity,Kingston Upon Hull", "");
                //birthLocation = birthLocation.Replace("Brawby", "Brawby, Salton");
                //birthLocation = birthLocation.Replace("Saint Denis,York", "York St Denys with St George United parish");
                //birthLocation = birthLocation.Replace("Saint Michael,New Malton", "New Malton St Michael Parish");
                //birthLocation = birthLocation.Replace("Kirkgate", "Kirkgate, Headingley");
                //birthLocation = birthLocation.Replace("East Keswick", "East Keswick, Harewood");
                //birthLocation = birthLocation.Replace("Harwood", "Harewood");
                //birthLocation = birthLocation.Replace("Baldersby", "Baldersby, Topcliffe");
                //birthLocation = birthLocation.Replace("Cathedral,Manchester", "Manchester, Cathedral");
                //birthLocation = birthLocation.Replace("All Saints,Newcastle Upon Tyne", "Newcastle Upon Tyne, All Saints");
                //birthLocation = birthLocation.Replace("St Sepulchre", "St. Sepulchre without Newgate");
                //birthLocation = birthLocation.Replace("Barrow-In-Furness,", "Barrow In Furness");
                //birthLocation = birthLocation.Replace("Saint Andrew,Holborn", "Holborn, Saint Andrew");
                //birthLocation = birthLocation.Replace("Woodhouse,Near Leids,", "Leeds, Wood House");
                //birthLocation = birthLocation.Replace("Saint Dunstan,Stepney", "Stepney St Dunstan");
                //birthLocation = birthLocation.Replace("Saint Mary-St Marylebone Road,Saint Marylebone", "Marylebone St Mary");
                //birthLocation = birthLocation.Replace("St Giles Cripplegate", "Cripplegate St Giles");



                //
                //
                //birthLocation = birthLocation.Replace("", "");


                //birthLocation = birthLocation.Replace("Of ", "");

                //if (birthLocation.Trim() == "London") birthLocation = "London Unknown";
                //if (birthLocation.Trim() == "York") birthLocation = "York Unknown";
                //if (birthLocation.Trim() == "Newcastle") birthLocation = "Newcastle Unknown";
                //if (birthLocation.Trim() == "Manchester") birthLocation = "Manchester Unknown";

                //  update entry
                deathsBirthsBll.UpdateBirthDeathRecord2(person.Person_id,
                                                        person.IsMale,
                                                        person.ChristianName,
                                                        person.Surname,
                                                        person.BirthLocation,
                                                        person.BirthDateStr,
                                                        person.BaptismDateStr,
                                                        person.DeathDateStr,
                                                        person.ReferenceDateStr,
                                                        person.DeathLocation,
                                                        person.FatherChristianName,
                                                        person.FatherSurname,
                                                        person.MotherChristianName,
                                                        person.MotherSurname,
                                                        person.Notes,
                                                        person.Source,
                                                        person.BapInt,
                                                        person.BirthInt,
                                                        person.DeathInt,
                                                        person.ReferenceDateInt,
                                                        person.ReferenceLocation,
                                                        person.BirthCounty,
                                                        person.DeathCounty,
                                                        person.Occupation,
                                                        person.FatherOccupation,
                                                        person.SpouseName,
                                                        person.SpouseSurname,
                                                        person.UserId,
                                                        locationId,
                                                        person.DeathLocationId,
                                                        person.ReferenceLocationId,
                                                        person.TotalEvents,
                                                        person.EventPriority,
                                                        person.UniqueRef.GetValueOrDefault(),
                                                        person.EstBirthYearInt,
                                                        person.EstDeathYearInt,
                                                        person.IsEstBirth,
                                                        person.IsEstDeath,
                                                        person.OrigSurname,
                                                        person.OrigFatherSurname,
                                                        person.OrigMotherSurname);
            }





        }


        public void ProcessAllParishs()
        {
            ParishsBll brParishRecords = new ParishsBll();
            
            //     DsParishs.ParishsDataTable parishsDataTable = brParishRecords.GetParishs2();
            int idx = 0;
            foreach (var pRow in brParishRecords.GetParishs2())
            {
                try
                {
                    // TidyDataRanges(pRow.ParishId);
                    processParish(pRow.ParishId);

                }
                catch (Exception ex1)
                {
                  
                }
                //

                //   message.Text = "importing: " + idx.ToString() + " of " + parishsDataTable.Count.ToString() + " " + pRow.ParishName;
                idx++;
            }
        }


 
        public static string ExtractChristianAndSurnames(string name, out string cname, out string sname)
        {
            name = name.Trim();
            cname = "";
            sname = "";

            if (name != "")
            {
                int surnameBeginning = name.LastIndexOf(' ');
                if (surnameBeginning > 0)
                {
                    sname = name.Substring(surnameBeginning);

                    cname = name.Substring(0, surnameBeginning);

                    sname = sname.Trim();
                    cname = cname.Trim();
                }
                else
                {
                    sname = name;
                    cname = "";
                }

            }
            return name;
        }



    










        public void StartParishImportation(string parishfolder)
        {
            //   FolderBrowserDialog fbd = new FolderBrowserDialog();

            //   fbd.ShowDialog();

            if (parishfolder == "" || !Directory.Exists(parishfolder)) return;

            DirectoryInfo di = new DirectoryInfo(parishfolder);

            FileInfo[] fileInfos = di.GetFiles();

            int idx = 0;
            foreach (FileInfo _fi in fileInfos)
            {
                //textBox.Text = "importing: " + _fi.Name + " " + idx.ToString() + " of " + fileInfos.Length.ToString();

                try
                {
                    //ImportParish(_fi.FullName);
                    // TidyDataRanges(_fi.FullName);
                    AddMissingDepositsParish(_fi.FullName);
                }
                catch (Exception ex1)
                {
                    LogString(_fi.FullName, ex1);
                }
                idx++;
            }


            //textBox.Text = "finished";
        }

        public void AddMissingDepositsParish(string _path)
        {
            // Application.DoEvents();
            //Application.DoEvents();
            //  \d{4}.*?-.*?\d{4}   extracts date ranges

            listBaptismYearsAdded = new List<int>();
            listMarriageYearsAdded = new List<int>();

            string description = "";
         
            bool foundDepositedRegs = false;
         
    
            Guid _parishId = Guid.Empty;
            string depositedLocation = "";
            string parishName = "";
            string parentParishName = "";
            int startYear = 1536;


            List<string> transcriptList = new List<string>();

            ParishsBll parishsBLL = new ParishsBll();
            ParishTranscriptionsBll parishTranscriptionsBLL = new ParishTranscriptionsBll();

            Match lmatch = null;
            MatchCollection lMatchs = null;
            // string desktopStr = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory).ToString();

            //desktopStr = Path.Combine(desktopStr, "Sample.");

            string[] contents = File.ReadAllLines(_path);
     
            foreach (string line in contents)
            {


                if (line.Contains("Dates and current locations etc. <BR></H3><p>"))
                { //(?<=<H3>).* (?=parish registers)
                    Regex _reg = new Regex(@"(?<=<H3>).*(?=parish registers)|(?<=registers,).*(?=: Dates)");

                    lMatchs = _reg.Matches(line);

                    if (lMatchs.Count > 0)
                    {

                        if (lMatchs.Count > 1)
                        {
                            parentParishName = "Chapelry of " + lMatchs[0].Value.Trim();
                            parishName = lMatchs[1].Value.Replace("Chapelry", "").Trim();
                        }
                        else
                        {
                            parishName = lMatchs[0].Value.Trim();
                        }
                    }

               

                }

              
                if (line.ToLower().Contains("<h4>deposited")) foundDepositedRegs = true;




                depositedLocation = "";
                if (foundDepositedRegs)
                {
                    #region par regs deposited
                    if (line.ToLower().Contains("<h4>deposited"))
                    {
                        Regex _reg = new Regex(@"(?<=\().*(?=\))");

                        Match _match = _reg.Match(line);

                        if (_match.Success)
                            depositedLocation = _match.Value;

                    }

                 

                   
                    #endregion


                }

                //    if(parishName != "" && depositedLocation != "")
                //       MessageBox.Show(parishName, depositedLocation);

                //try
                //{
                var pdt = parishsBLL.GetParishByFilter2(parishName, "", "").FirstOrDefault();

                if (parishName != "" && depositedLocation != "")
                {
                    if (pdt!=null)
                    {
                        parishsBLL.UpdateParish2(pdt.ParishId, pdt.ParishName, pdt.ParishNotes, depositedLocation,
                                                 parentParishName, pdt.ParishStartYear.Value, 0, "Yorkshire", decimal.Zero, decimal.Zero);

                    }
                }
                //}
                //catch (Exception ex1)
                //{
                //    MessageBox.Show(ex1.Message);
                //}




            }

           


          

        }




        public void DownloadFiles(string path)
        {
            //(?<=Y/).*(?=.html)
            //
            string desktopStr = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory).ToString();

            desktopStr = Path.Combine(desktopStr, "Thackray");

            if (!Directory.Exists(desktopStr))
                Directory.CreateDirectory(desktopStr);

            desktopStr = Path.Combine(desktopStr, "Downloads");

            if (!Directory.Exists(desktopStr))
                Directory.CreateDirectory(desktopStr);



            // string path = @"C:\Documents and Settings\GThackray\Desktop\thackray\ainsty.txt";
            //DownloadFile();
            string parishName = "";
            string[] lines = File.ReadAllLines(path);

            int idx = 0;

            foreach (string line in lines)
            {
                Regex _reg = new Regex(@"(?<=Y/).*(?=.html)");
                Match _match = _reg.Match(line);
                if (_match.Success)
                {
                    parishName = _match.Value.Trim() + ".txt";

                    //  textBox.Text = "Downloading " + idx.ToString() + " of " + lines.Length.ToString()
                    //        + " URL: " + line;
                    DownloadFile(line, Path.Combine(desktopStr, parishName));
                }
                idx++;
            }

            // MessageBox.Show("", "Finished");
        }

        public static void DownloadFile(string url, string outFile)
        {

            try
            {
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                // prepare the web page we will be asking for
                HttpWebRequest request = (HttpWebRequest)
                                         WebRequest.Create(url);

                // execute the request
                HttpWebResponse response = (HttpWebResponse)
                                           request.GetResponse();

                // we will read data via the response stream
                Stream resStream = response.GetResponseStream();

                string tempString = null;
                int count = 0;

                do
                {
                    // fill the buffer with data
                    count = resStream.Read(buf, 0, buf.Length);

                    // make sure we read some data
                    if (count != 0)
                    {
                        // translate from bytes to ASCII text
                        tempString = Encoding.ASCII.GetString(buf, 0, count);

                        // continue building the string
                        sb.Append(tempString);
                    }
                }
                while (count > 0); // any more data to read?

                // print out page source
                // Console.WriteLine();

                if (!File.Exists(outFile))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(outFile))
                    {
                        sw.Write(sb.ToString());
                    }
                }

            }
            catch (Exception ex1)
            {
                LogString(url, ex1);
            }
        }

        public static void LogString(string url, Exception ex1)
        {
            //MessageBox.Show(ex1.Message,"Error");

            string desktopStr = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory).ToString();

            desktopStr = Path.Combine(desktopStr, "Thackray");

            if (!Directory.Exists(desktopStr))
                Directory.CreateDirectory(desktopStr);

            desktopStr = Path.Combine(desktopStr, "Downloads");

            if (!Directory.Exists(desktopStr))
                Directory.CreateDirectory(desktopStr);

            desktopStr = Path.Combine(desktopStr, "logfile.txt");

            StreamWriter log;

            if (!File.Exists(desktopStr))
            {
                log = new StreamWriter(desktopStr);
            }
            else
            {
                log = File.AppendText(desktopStr);
            }

            // Write to the file:
            log.WriteLine(DateTime.Now);
            log.WriteLine(url);
            log.WriteLine(ex1.Message);

            log.WriteLine();

            // Close the stream:
            log.Close();
        }





        public void WriteMissingIGIEntries()
        {
            ParishsBll parishsBLL = new ParishsBll();
            //   DsParishs.ParishsDataTable pDataTable = new DsParishs.ParishsDataTable();

            //   pDataTable = parishsBLL.GetParishs2();


            foreach (var pRow in parishsBLL.GetParishs2())
            {
                processParish(pRow.ParishId);
            }



        }


        public void processParish(Guid paramParishid)
        {

            //  paramParishid = new Guid("221ad1d8-c152-459f-90bf-86a1912e3ea8");

            ParishRecordsBll pRecordsBll = new ParishRecordsBll();
            MissingParishRecordsBll missingParishRecordsBll = new MissingParishRecordsBll();
            List<int> bapList = new List<int>();
            List<int> marList = new List<int>();


            //DsParishRecords.ParishRecordsDataTable parishRecordsDataTableMarriages = new DsParishRecords.ParishRecordsDataTable();

            List<TDBCore.EntityModel.ParishRecord> parishRecordsDataTableMarriages = new System.Collections.Generic.List<TDBCore.EntityModel.ParishRecord>();

            // DsParishRecords.ParishRecordsDataTable parishRecordsDataTableBaptisms = new DsParishRecords.ParishRecordsDataTable();

            List<TDBCore.EntityModel.ParishRecord> parishRecordsDataTableBaptisms = new System.Collections.Generic.List<TDBCore.EntityModel.ParishRecord>();


            parishRecordsDataTableBaptisms = pRecordsBll.GetParishRecordsByIdAndRecordType2(paramParishid, "C").ToList();
            parishRecordsDataTableMarriages = pRecordsBll.GetParishRecordsByIdAndRecordType2(paramParishid, "M").ToList();
            int idx = 0;

            if (parishRecordsDataTableBaptisms.Count > 0)
            {
                var bapLst = parishRecordsDataTableBaptisms.OrderBy(o => o.Year);
            

                DeriveListOfRanges(bapLst, bapList);

                bapList.Insert(0, 1550);
                bapList.Add(1840);

                while (idx < (bapList.Count / 2))
                {
                    if ((bapList[idx] - bapList[idx + 1]) < 0) // dont put any negative ranges in there
                        missingParishRecordsBll.InsertMissingParishRecord2(paramParishid, 0, bapList[idx], "C", false, bapList[idx + 1]);
                    idx += 2;
                }
            }
            else
            {
                missingParishRecordsBll.InsertMissingParishRecord2(paramParishid, 0, 1550, "C", false, 1838);
            }

            if (parishRecordsDataTableMarriages.Count > 0)
            {
                //    DataView dvMar = new DataView(parishRecordsDataTableMarriages);
                //    dvMar.Sort = "Year";

                var marLst = parishRecordsDataTableBaptisms.OrderBy(o => o.Year);
                DeriveListOfRanges(marLst, marList);

                marList.Insert(0, 1550);
                marList.Add(1840);

                idx = 0;
                while (idx < (marList.Count / 2))
                {
                    if ((marList[idx] - marList[idx + 1]) < 0) // dont put any negative ranges in there
                        missingParishRecordsBll.InsertMissingParishRecord2(paramParishid, 0, marList[idx], "M", false, marList[idx + 1]);
                    idx += 2;
                }
            }
            else
            {
                missingParishRecordsBll.InsertMissingParishRecord2(paramParishid, 0, 1550, "M", false, 1838);
            }

        }

        public static void DeriveListOfRanges(IOrderedEnumerable<TDBCore.EntityModel.ParishRecord> dvBap, List<int> bapList)
        {
          

            int idx = 0;
            int newX = 0;
            int newY = 0;


            int dvIdx = 0;
            foreach (TDBCore.EntityModel.ParishRecord row in dvBap)
            {
                newX = row.Year.Value; 
                newY = row.YearEnd.Value;                  
                if (dvIdx != 0)
                {
                    if (newX > (bapList[idx + 1] + 1))
                    {
                        bapList.Add(newX);
                        bapList.Add(newY);
                        // move cursor onto new entry
                        idx += 2;
                    }
                    else
                    {   // there is overlap between the ranges
                        // merge them together
                        bapList[idx + 1] = newY;

                    }

                }
                else
                {
                    bapList.Add(newX);
                    bapList.Add(newY);
                }

                dvIdx++;
                
            }

        }

        public void ImportParish(string _path)
        {
            // Application.DoEvents();
            //Application.DoEvents();
            //  \d{4}.*?-.*?\d{4}   extracts date ranges

            listBaptismYearsAdded = new List<int>();
            listMarriageYearsAdded = new List<int>();

            string description = "";
            bool foundBlockQuote = false;
            bool foundDepositedRegs = false;
            bool foundBishTran = false;
            bool foundIGI = false;
            bool foundTranscripts = false;
            Guid _parishId = Guid.Empty;
            string depositedLocation = "";
            string parishName = "";
            string parentParishName = "";
            int startYear = 1536;

            string PRBirths = "";
            string PRMarriages = "";
            string PRBurials = "";
            string bishTran = "";

            string igiBirthPr = "";
            string igiMarriagePr = "";
            string igiBirthBt = "";
            string igiMarriageBt = "";

            List<string> transcriptList = new List<string>();

            ParishsBll parishsBLL = new ParishsBll();
            ParishTranscriptionsBll parishTranscriptionsBLL = new ParishTranscriptionsBll();

            Match lmatch = null;
            MatchCollection lMatchs = null;
            // string desktopStr = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory).ToString();

            //desktopStr = Path.Combine(desktopStr, "Sample.");

            string[] contents = File.ReadAllLines(_path);
            //<h4>Bishop
            foreach (string line in contents)
            {


                if (line.Contains("Dates and current locations etc. <BR></H3><p>"))
                { //(?<=<H3>).* (?=parish registers)
                    Regex _reg = new Regex(@"(?<=<H3>).*(?=parish registers)|(?<=registers,).*(?=: Dates)");

                    lMatchs = _reg.Matches(line);

                    if (lMatchs.Count > 0)
                    {

                        if (lMatchs.Count > 1)
                        {
                            parentParishName = "Chapelry of " + lMatchs[0].Value.Trim();
                            parishName = lMatchs[1].Value.Replace("Chapelry", "").Trim();
                        }
                        else
                        {
                            parishName = lMatchs[0].Value.Trim();
                        }
                    }



                }

                if (line.ToLower().Contains("<blockquote>")) foundBlockQuote = true;
                if (line.ToLower().Contains("</blockquote>")) foundBlockQuote = false;
                if (line.ToLower().Contains("<h4>deposited")) foundDepositedRegs = true;
                if (line.ToLower().Contains("</ul>")) foundDepositedRegs = false;

                if (line.ToLower().Contains("<h4>bishop")) foundBishTran = true;
                if (line.ToLower().Contains("<h4>igi")) foundBishTran = false;

                if (line.ToLower().Contains("<h4>igi")) foundIGI = true;
                if (line.ToLower().Contains("</ul>")) foundIGI = false;

                if (line.ToLower().Contains("<h4>transcripts")) foundTranscripts = true;
                if (line.ToLower().Contains("</ul>")) foundTranscripts = false;

                if (foundBlockQuote)
                    description = description + line;

                description = description.Replace("<blockquote>", "");
                description = description.Replace("</blockquote>", "");

                if (foundDepositedRegs)
                {
                    #region par regs deposited
                    if (line.Contains("<h4>deposited"))
                    {
                        Regex _reg = new Regex(@"(?<=\().*(?=\))");

                        Match _match = _reg.Match(line);

                        if (_match.Success)
                            depositedLocation = _match.Value;

                    }

                    // add to database 
                    try
                    {
                        _parishId = parishsBLL.AddParish(parishName, description, 
                                                         depositedLocation, parentParishName, startYear,"",0,decimal.Zero,decimal.Zero);
                    }
                    catch (Exception ex1)
                    {
                        //MessageBox.Show(ex1.Message);
                    }

                    //if (line.Contains("<li>Baptisms:")|| PRBirths = line;
                    //if (line.Contains("<li>Marriages:")) PRMarriages = line;
                    //PRBurials = line;

                    if (line.Contains("<li>Burials:") ||
                        line.Contains("<li>Baptisms:") ||
                        line.Contains("<li>Marriages:"))
                    {

                        storeLine(_parishId, line, "deposited");
                    }
                    #endregion


                }

                #region bish tran
                if (foundBishTran)
                {
                    bishTran += line;
                }
                #endregion




                #region igi
                if (foundIGI)
                {
                    //   MessageBox.Show(line);


                    if (line.Contains("<li>C"))
                    {
                        #region christenings
                        if (line.Contains("<i>"))
                        {
                            igiBirthPr = line;

                            string nonBTranPart = Regex.Replace(line, @"(<i>).*?(</i>)|<li>|c", "");



                            MatchCollection lmatchc;
                            Regex _reg = new Regex(@"((?<=<i>).*?(?=</i>))");

                            lmatchc = _reg.Matches(line);


                            foreach (Match _match in lmatchc)
                            {
                                storeLine(_parishId, _match.Value, "igibtc");
                            }

                            storeLine(_parishId, nonBTranPart, "igic");
                        }

                        else
                        {
                            igiBirthBt = line;
                            storeLine(_parishId, line, "igic");
                        }
                        #endregion
                    }
                    else
                    {
                        #region marriages
                        if (line.Contains("<i>"))
                        {
                            igiBirthPr = line;

                            string nonBTranPart = Regex.Replace(line, @"(<i>).*?(</i>)|<li>|m", "");

                            MatchCollection lmatchc;
                            Regex _reg = new Regex(@"((?<=<i>).*?(?=</i>))");

                            lmatchc = _reg.Matches(line);

                            foreach (Match _match in lmatchc)
                            {
                                storeLine(_parishId, _match.Value, "igibtm");
                            }

                            storeLine(_parishId, nonBTranPart, "igim");
                        }

                        else
                        {
                            igiBirthBt = line;
                            storeLine(_parishId, line, "igim");
                        }
                        #endregion
                    }

                }
                #endregion



                if (foundTranscripts)
                {
                    if (line.Contains("<li>"))
                    {
                        //  transcriptList.Add(line);
                        try
                        {
                            parishTranscriptionsBLL.InsertTranscription2(_parishId, line.Replace("<li>", ""));
                        }
                        catch (Exception ex1)
                        {
                            //MessageBox.Show(ex1.Message);
                        }
                    }
                }

            }

            if (bishTran != "")
            {
                storeLine(_parishId, bishTran, "deposited");
            }

            List<int> missingBaptismsYears = new List<int>();
            List<int> missingMarriagesYears = new List<int>();

            int yearIdx = 1550;

            while (yearIdx < 1852)
            {
                if (!this.listBaptismYearsAdded.Contains(yearIdx))
                {
                    missingBaptismsYears.Add(yearIdx);
                }

                if (!this.listMarriageYearsAdded.Contains(yearIdx))
                {
                    missingMarriagesYears.Add(yearIdx);
                }

                yearIdx++;
            }


            if (missingBaptismsYears.Count > 0)
            {
                MissingParishRecordsBll missingParishRecordsBLL = new MissingParishRecordsBll();
                foreach (int _year in missingBaptismsYears)
                    missingParishRecordsBLL.InsertMissingParishRecord2(
                        _parishId, 0, _year, "C", false, 0);
            }

            if (missingMarriagesYears.Count > 0)
            {
                MissingParishRecordsBll missingParishRecordsBLL = new MissingParishRecordsBll();
                foreach (int _year in missingMarriagesYears)
                    missingParishRecordsBLL.InsertMissingParishRecord2(
                        _parishId, 0, _year, "M", false, 0);
            }

        }

        private void storeLine(Guid parishId, string line, string lineType)
        {

            // get line type(s)
            // will be the actual deposited registers themselves

            if (lineType == "deposited")
            {
                if (line.ToLower().Contains("baptisms")
                    || line.ToLower().Contains("marriages")
                    || line.ToLower().Contains("burials"))
                {
                    string recordType = "";
                    #region work out baptism, marrage or burial type
                    //  isOriginalRegister = true;
                    recordType = "DPR";
                    //  MessageBox.Show(line);
                    if (line.ToLower().Contains("baptisms"))
                    {
                        recordType = "C";
                    }
                    else if (line.ToLower().Contains("marriages"))
                    {
                        recordType = "M";
                    }
                    else
                    {
                        recordType = "B";
                    }
                    #endregion

                    parishId = ParseStringAndAdd(parishId, line, recordType, 38, true);

                }
                else
                { // bishops transcripts
                    parishId = ParseStringAndAdd(parishId, line, "CMB", 39, true);
                }
            }
            else
            {
                // bool isOriginalRegister = false;
                if (lineType.Contains("igi"))
                {
                    if (lineType == "igim" || lineType == "igic")
                    {
                        if (lineType.Contains("m"))
                            parishId = ParseStringAndAdd(parishId, line, "m", 1, false);
                        else
                            parishId = ParseStringAndAdd(parishId, line, "c", 1, false);
                    }
                    else
                    {
                        if (lineType.Contains("m"))
                            parishId = ParseStringAndAdd(parishId, line, "m", 2, false);
                        else
                            parishId = ParseStringAndAdd(parishId, line, "c", 2, false);

                    }
                }

                else
                { //misc transcript! 
                    ParishTranscriptionsBll parishTranscriptionsBLL = new ParishTranscriptionsBll();
                    try
                    {
                        parishTranscriptionsBLL.InsertTranscription2(parishId, line);
                    }
                    catch (Exception ex1)
                    {
                        //MessageBox.Show(ex1.Message);
                    }
                }
            }


        }

        private Guid ParseStringAndAdd(Guid parishId, string line, string recordType, int typeId, bool isDeposit)
        {




            ParishRecordsBll prbll = new ParishRecordsBll();
            MatchCollection lmatch;
            //Regex _reg = new Regex(@"(\d{4}.*?-.*?\d{4})|(\d{4})");  (\d{4}-\d{4})

            Regex _reg = new Regex(@"(\d{4}-\d{4})");


            lmatch = _reg.Matches(line);

            foreach (Match m in lmatch)
            {
                int startYear = 0;
                int endYear = 0;
                string[] parts = m.Value.Split('-');

                Int32.TryParse(parts[0].Trim(), out  startYear);
                Int32.TryParse(parts[1].Trim(), out  endYear);
                int idx = startYear;
                if (startYear != 0 && endYear != 0)
                {
                    while (idx <= endYear)
                    {

                        try
                        {
                            prbll.InsertParishRecord2(parishId, typeId, idx, recordType, isDeposit, idx);
                        }
                        catch (Exception ex1)
                        {
                            //MessageBox.Show(ex1.Message);
                        }

                        if (!isDeposit)
                        {
                            if (recordType.ToLower().Contains("m"))
                                listMarriageYearsAdded.Add(idx);
                            else
                                listBaptismYearsAdded.Add(idx);
                        }
                        idx++;
                    }
                }
            }

            _reg = new Regex(@"((?<=[^-])\d{4}(?=[^-]))");


            lmatch = _reg.Matches(line);

            foreach (Match m in lmatch)
            {
                int _year = 0;
                Int32.TryParse(m.Value, out  _year);
                try
                {
                    prbll.InsertParishRecord2(parishId, typeId, _year, recordType, isDeposit, _year);
                }
                catch (Exception ex1)
                {
                    //MessageBox.Show(ex1.Message);
                }
                if (!isDeposit)
                {
                    if (recordType.ToLower().Contains("m"))
                        listMarriageYearsAdded.Add(_year);
                    else
                        listBaptismYearsAdded.Add(_year);
                }
            }


            return parishId;
        }

        private void parseTranscript(string transLine)
        {

            //CB 1626-1801; M 1626-1812; TNI; YAS    
            List<string> parts = new List<string>(transLine.Split(';'));
            List<string> sourceList = new List<string>();

            List<string> burialList = new List<string>();
            List<string> marriageList = new List<string>();
            List<string> baptismList = new List<string>();

            #region fill lists
            foreach (string _part in parts)
            {
                Regex _regex = new Regex("[0-9]");
                Match result = _regex.Match(_part);
                if (!result.Success)
                {
                    sourceList.Add(_part.Trim());
                }
                else
                {
                    if (_part.Contains("C"))
                    {
                        baptismList.Add(_part);
                    }
                    if (_part.Contains("B"))
                    {
                        burialList.Add(_part);
                    }
                    if (_part.Contains("M"))
                    {
                        marriageList.Add(_part);
                    }

                }

            }
            #endregion


        }


        public void ImportSourcesFromFile(string folderPath)
        {

            //FolderBrowserDialog fbd = new FolderBrowserDialog();

            //fbd.ShowDialog();

            if (folderPath == "" || !Directory.Exists(folderPath)) return;

            DirectoryInfo di = new DirectoryInfo(folderPath);

            FileInfo[] fileInfos = di.GetFiles();

            SourceBll sourcesBll = new SourceBll();

            int idx = 0;
            foreach (FileInfo _fi in fileInfos)
            {
                List<string> fileLines = new List<string>(File.ReadAllLines(_fi.FullName));
                foreach (string line in fileLines)
                {
                    string sourceName = "";
                    int startYear = 0;
                    DateTime sYear = new DateTime(startYear, 1, 1); ;
                    DateTime eYear = new DateTime(startYear, 1, 1); ;
                    int endYear = 0;
                    string[] parts = line.Split(',');

                    if (parts.Length > 0)
                    {
                        sourceName = parts[0];

                        string[] dateParts = parts[1].Split('-');
                        if (dateParts.Length > 0)
                        {
                            Int32.TryParse(dateParts[0], out startYear);
                            Int32.TryParse(dateParts[1], out endYear);
                        }

                        sYear = new DateTime(startYear, 1, 1);
                        eYear = new DateTime(endYear, 12, 31);

                    }

                    sourcesBll.InsertSource2(sourceName, "ancestry.com", false, true, false, 1, sYear.ToShortDateString(), eYear.ToShortDateString(), startYear, endYear, sourceName,0,"");


                }
            }
        }



   

        #endregion



        private void ExtractDeaths(string path)
        {
            List<string> _allLines = new List<string>(File.ReadAllLines(path));

            var query = from line in _allLines
                        where line.ToCharArray().Where(o => o == '|').Count() == 24

                        let data = line.Split('|')

                        select new

                            {
                                Name = data[0].Replace("Burial of", ""),
                                Piece_Description = data[1].Replace("PD", ""),
                                TNA_Reference = data[2].Replace("TNA Reference", ""),
                                Chapel = data[3].Replace("[Chapel/Registry]", ""),
                                Full_Name = data[4].Replace("Full Name", ""),
                                Date_of_Death = data[5].Replace("Date of Death", ""),
                                Place_of_Death = data[6].Replace("Place of Death", ""),
                                Date_of_Birth = data[7].Replace("Date of Birth", ""),

                                Age = data[9].Replace("Age", ""),
                                Profession = data[10].Replace("Profession", ""),
                                Relation = data[11].Replace("Relation", ""),
                                Description = data[12].Replace("Description", ""),
                                Place_of_Abode = data[13].Replace("Place of Abode", ""),
                                Parish_of_Abode = data[14].Replace("Parish of Abode", ""),
                                County_of_Abode = data[15].Replace("County of Abode", ""),
                                Registration_Date = data[16].Replace("Registration Date", ""),
                                Registration_Town_County = data[17].Replace("Registration Town/County", ""),
                                Ceremony_Performed_by = data[18].Replace("Ceremony Performed by", ""),
                                Husband_Father = data[19].Replace("Husband's/Father's", ""),
                                Husbands_Fathers_Profession = data[20].Replace("Husband's/Father's Profession", ""),
                                Wife_Mother = data[21].Replace("Wife/Mother -", ""),
                                Cause_of_death = data[22].Replace("Cause of death", ""),
                                Grave_Number = data[23].Replace("Grave Number", ""),
                                Undertaker = data[24].Replace("Undertaker", ""),

                            };




            int idx = 0;
            foreach (var team in query)
            {
                //  if (idx == 89)
                Debug.WriteLine(team.TNA_Reference + "," + team.Piece_Description);


                idx++;
            }
        }


        private void ExtractBurials(string path)
        {
            List<string> _allLines = new List<string>(File.ReadAllLines(path));


            int idx = 0;
            foreach (string str in _allLines)
            {
                //if(str.Select(m=> { str.

                if (str.ToCharArray().Where(o => o == '|').Count() != 25)
                {
                    Debug.WriteLine("record: " + idx + " contains " + str.ToCharArray().Where(o => o == '|').Count().ToString());
                    Debug.WriteLine(str);

                }


                idx++;
            }



            var query = from line in _allLines

                        let data = line.Split('|')

                        select new

                            {
                                Name = data[0].Replace("Burial of", ""),
                                Piece_Description = data[1].Replace("PD", ""),
                                TNA_Reference = data[2].Replace("TNA Reference", ""),
                                Chapel = data[3].Replace("[Chapel/Registry]", ""),
                                Full_Name = data[4].Replace("Full Name", ""),
                                Date_of_Burial = data[5].Replace("Date of Burial", ""),
                                Place_of_Burial = data[6].Replace("Place of Burial", ""),
                                Date_of_Birth = data[7].Replace("Date of Birth", ""),
                                Date_of_Death = data[8].Replace("Date of Death", ""),
                                Age = data[9].Replace("Age", ""),
                                Profession = data[10].Replace("Profession", ""),
                                Relation = data[11].Replace("Relation", ""),
                                Description = data[12].Replace("Description", ""),
                                Place_of_Abode = data[13].Replace("Place of Abode", ""),
                                Parish_of_Abode = data[14].Replace("Parish of Abode", ""),
                                County_of_Abode = data[15].Replace("County of Abode", ""),
                                Registration_Date = data[16].Replace("Registration Date", ""),
                                Registration_Town_County = data[17].Replace("Registration Town/County", ""),
                                Ceremony_Performed_by = data[18].Replace("Ceremony Performed by", ""),
                                Husband_Father = data[19].Replace("Husband's/Father's", ""),
                                Husbands_Fathers_Profession = data[20].Replace("Husband's/Father's Profession", ""),
                                Wife_Mother = data[21].Replace("Wife/Mother -", ""),
                                Cause_of_death = data[22].Replace("Cause of death", ""),
                                Grave_Number = data[23].Replace("Grave Number", ""),
                                Undertaker = data[24].Replace("Undertaker", ""),

                            };




            idx = 0;
            foreach (var team in query)
            {
                if (idx == 89)
                    Debug.WriteLine(team.TNA_Reference + "," + team.Piece_Description);


                idx++;
            }
        }


        private void ExtractBirths(string path)
        {
            List<string> _allLines = new List<string>(File.ReadAllLines(path));

            //foreach (string _line in _allLines)
            //{
            //   // Debug.WriteLine(_line.Replace("|", "|\r\n"));
            //    string newLine = _line.Substring(0, _line.LastIndexOf("Grandparent(s)|") + 15);
            //    if (newLine.ToCharArray().Where(o => o == '|').Count() != 31)
            //        Debug.WriteLine("  " +  newLine);
            //    else
            //        Debug.WriteLine(newLine);


            //}

            //int idx = 0;

            var query = from line in _allLines
                        where line.ToCharArray().Where(o => o == '|').Count() == 31

                        let data = line.Split('|')

                        select new

                            {
                                Name = data[0].Replace("Baptism of", ""),
                                Piece_Description = data[1].Replace("PD", ""),
                                TNA_Reference = data[2].Replace("TNA Reference", ""),
                                Chapel = data[3].Replace(@"[Chapel/Registry]", ""),
                                Full_Name = data[4].Replace("Full Name", ""),

                                Date_of_Birth = data[5].Replace("Date of Birth", ""),
                                Place_of_Birth = data[6].Replace("Place of Birth", ""),

                                Place_of_Abode = data[8].Replace("Place of Abode", ""),
                                Parish_of_Abode = data[9].Replace("Parish of Abode", ""),
                                County_of_Abode = data[10].Replace("County of Abode", ""),
                                Registration_Date = data[11].Replace("Registration Date", ""),
                                Registration_Town_County = data[12].Replace("Registration Town/County", ""),
                                Ceremony_Performed_by = data[13].Replace("Ceremony Performed by", ""),



                                //    Godparents = data[14].Replace("Godparents", ""),
                                //    Godfather = data[15].Replace("Godfather", ""),
                                //    Godmother = data[16].Replace("Godmother", ""),
                                Parents = data[14].Replace("Parents", ""),
                                Father = data[15].Replace("Father", ""),
                                Fathers_Profession = data[16].Replace("Father's Profession", ""),
                                Mother = data[17].Replace("Mother", ""),
                                Mothers_Maiden_Name = data[18].Replace("Mother's Maiden Name", ""),
                                Mothers_Parish = data[19].Replace("Mother's Parish", ""),
                                Date_of_Marriage = data[20].Replace("Date of Marriage", ""),
                                Place_of_Marriage = data[21].Replace("Place of Marriage", ""),
                                Maternal_Parents = data[22].Replace("Maternal Parents", ""),
                                Names = data[23].Replace("Name(s)", ""),
                                Profession = data[24].Replace("Profession", ""),
                                TownCounty = data[25].Replace("Town & County", ""),
                                PaternalParents = data[26].Replace("Paternal Parents", ""),
                                Namesx2 = data[27].Replace("Name(s)", ""),
                                Professionx2 = data[28].Replace("Profession", ""),
                                PedigreeChart = data[29].Replace("Pedigree Chart", ""),
                                Grandparentsx1 = data[30].Replace("Grandparent(s)", ""),
                                Grandparentsx2 = data[31].Replace("Grandparent(s)", ""),





                            };




            int idx = 0;
            foreach (var team in query)
            {

                Debug.WriteLine(team.TNA_Reference + "," + team.Piece_Description);


                idx++;
            }
        }


        private void ExtractBaptisms(string path)
        {
            List<string> _allLines = new List<string>(File.ReadAllLines(path));


            //int idx = 0;
            //foreach (string str in _allLines)
            //{
            //    //if(str.Select(m=> { str.

            //    if (str.ToCharArray().Where(o => o == '|').Count() == 39)
            //    {
            //   //     Debug.WriteLine("record: " + idx + " contains " + str.ToCharArray().Where(o => o == '|').Count().ToString());
            //        Debug.WriteLine(str + "|||");

            //    }


            //    idx++;
            //}

            // idx = 0;
            //foreach (string str in _allLines)
            //{
            //    //if(str.Select(m=> { str.

            //       if (str.ToCharArray().Where(o => o == '|').Count() == 40)
            //       {
            //  //  Debug.WriteLine("record: " + idx + " contains " + str.ToCharArray().Where(o => o == '|').Count().ToString());
            //           Debug.WriteLine(str + "||");

            //       }


            //    idx++;
            //}

            // idx = 0;
            //foreach (string str in _allLines)
            //{
            //    //if(str.Select(m=> { str.

            //       if (str.ToCharArray().Where(o => o == '|').Count() == 41)
            //       {
            //  //  Debug.WriteLine("record: " + idx + " contains " + str.ToCharArray().Where(o => o == '|').Count().ToString());
            //           Debug.WriteLine(str + "|");
            //       }


            //    idx++;
            //}


            //idx = 0;
            //foreach (string str in _allLines)
            //{
            //    //if(str.Select(m=> { str.

            //       if (str.ToCharArray().Where(o => o == '|').Count() == 42)
            //       {
            //   // Debug.WriteLine("record: " + idx + " contains " + str.ToCharArray().Where(o => o == '|').Count().ToString());
            //           Debug.WriteLine(str);

            //       }


            //    idx++;
            //}










            var query = from line in _allLines

                        let data = line.Split('|')

                        select new

                            {
                                Name = data[0].Replace("Baptism of", ""),
                                Piece_Description = data[1].Replace("PD", ""),
                                TNA_Reference = data[2].Replace("TNA Reference", ""),
                                Chapel = data[3].Replace(@"[Chapel/Registry]", ""),
                                Full_Name = data[4].Replace("Full Name", ""),

                                Date_of_Baptism = data[5].Replace("Date of Baptism", ""),
                                Place_of_Baptism = data[6].Replace("Place of Baptism", ""),
                                Date_of_Birth = data[7].Replace("Date of Birth", ""),
                                Place_of_Abode = data[8].Replace("Place of Abode", ""),
                                Parish_of_Abode = data[9].Replace("Parish of Abode", ""),
                                County_of_Abode = data[10].Replace("County of Abode", ""),
                                Registration_Date = data[11].Replace("Registration Date", ""),
                                Registration_Town_County = data[12].Replace("Registration Town/County", ""),
                                Ceremony_Performed_by = data[13].Replace("Ceremony Performed by", ""),



                                Godparents = data[14].Replace("Godparents", ""),
                                Godfather = data[15].Replace("Godfather", ""),
                                Godmother = data[16].Replace("Godmother", ""),
                                Parents = data[17].Replace("Parents", ""),
                                Father = data[18].Replace("Father", ""),
                                Fathers_Profession = data[19].Replace("Father's Profession", ""),
                                Mother = data[20].Replace("Mother", ""),
                                Mothers_Maiden_Name = data[21].Replace("Mother's Maiden Name", ""),
                                Mothers_Parish = data[22].Replace("Mother's Parish", ""),
                                Date_of_Marriage = data[23].Replace("Date of Marriage", ""),
                                Place_of_Marriage = data[24].Replace("Place of Marriage", ""),
                                Maternal_Parents = data[25].Replace("Maternal Parents", ""),
                                Names = data[26].Replace("Name(s)", ""),
                                Profession = data[27].Replace("Profession", ""),
                                TownCounty = data[28].Replace("Town & County", ""),
                                PaternalParents = data[29].Replace("Paternal Parents", ""),
                                Namesx2 = data[30].Replace("Name(s)", ""),
                                Professionx2 = data[31].Replace("Profession", ""),
                                PedigreeChart = data[32].Replace("Pedigree Chart", ""),
                                Grandparentsx1 = data[33].Replace("Grandparent(s)", ""),
                                Grandparentsx2 = data[34].Replace("Grandparent(s)", ""),
                                Notes = data[35] + data[36] + data[37] + data[38] + data[39] + data[40] + data[41] + data[42]





                            };




            int idx = 0;
            foreach (var team in query)
            {

                Debug.WriteLine(team.TNA_Reference + "," + team.Piece_Description);


                idx++;
            }
        }



        private void ExtractMarriages(string path)
        {
            List<string> _allLines = new List<string>(File.ReadAllLines(path));






            var query = from line in _allLines

                        let data = line.Split('|')

                        select new

                            {
                                Name = data[0].Replace("Baptism of", ""),
                                Piece_Description = data[1].Replace("PD", ""),
                                TNA_Reference = data[2].Replace("TNA Reference", ""),
                                Chapel = data[3].Replace(@"[Chapel/Registry]", ""),
                                Groom_Name = data[4].Replace("Groom Name", ""),
                                Bride_Name = data[5].Replace("Bride Name", ""),
                                Grooms_Profession = data[6].Replace("Groom's Profession", ""),
                                Date_of_Marriage = data[7].Replace("Date of Marriage", ""),
                                Place_of_Marriage = data[8].Replace("Place of Marriage", ""),
                                Registration_Date = data[9].Replace("Registration Date", ""),
                                Registration_Town_County = data[10].Replace("Registration Town/County", ""),
                                Ceremony_Performed_by = data[11].Replace("Ceremony Performed by", ""),
                                Grooms_Abode = data[12].Replace("Groom's Abode", ""),
                                Brides_Abode = data[13].Replace("Bride's Abode", ""),
                                Grooms_Parents = data[14].Replace("Groom's Parents", ""),
                                Grooms_Father = data[15].Replace("Groom's Father", ""),
                                Grooms_Fathers = data[16].Replace("Groom's Father's", ""),
                                Fathers_Profession = data[17].Replace("Father's Profession", ""),
                                Grooms_Fathers_Abode = data[18].Replace("Groom's Father's Abode", ""),
                                Grooms_Mother = data[19].Replace("Groom's Mother", ""),
                                Grooms_Mothers_Abode = data[20].Replace("Groom's Mother's Abode", ""),
                                Brides_Parents = data[21].Replace("Bride's Parents", ""),


                                Brides_Father = data[22].Replace("Bride's Father", ""),
                                Brides_Fathers_Profession = data[23].Replace("Bride's Father's Profession", ""),
                                Brides_Fathers_Abode = data[24].Replace("Bride's Father's Abode", ""),
                                Brides_Mother = data[25].Replace("Bride's Mother", ""),



                                Brides_Mothers_Adobe = data[26].Replace("Bride's Mother's Adobe", ""),
                                Pedigree_Chart = data[27].Replace("Pedigree Chart", ""),






                            };




            int idx = 0;
            foreach (var team in query)
            {

                Debug.WriteLine(team.TNA_Reference + "," + team.Piece_Description);


                idx++;
            }



        }



        public void ImportSourcedIGI()
        {



            /*
             ROUTINE TO REMOVE ALL DELETED RECORDS FROM DUPLICATE LISTS!
             * 
             
             */

            //  List<personRecord> allRecords = new List<personRecord>();

            //  List<personRecord> pRecords = new List<personRecord>();
            //  List<personRecord> mllRecords = new List<personRecord>();


            //  DirectoryInfo di = new DirectoryInfo(@"C:\Users\george\Desktop\familyhist\IGIExtraction");

            //  foreach (DirectoryInfo innerDir in di.GetDirectories())
            //  {
            //      foreach (FileInfo fileInfo in innerDir.GetFiles())
            //      {
            //          allRecords.AddRange(ReadFile(fileInfo.FullName));
            //      }

            //  }

            //  foreach (personRecord pRec in allRecords)
            //  {
            //      if (pRec.source.Substring(0, 1) == "M")
            //      {
            //          mllRecords.Add(pRec);
            //      }
            //      else
            //      {
            //          pRecords.Add(pRec);
            //      }
            //  }



            //  MarriagesBLL marriagesBLL = new MarriagesBLL();
            //  DsMarriages.MarriagesDataTable mdt = marriagesBLL.GetMarriages();


        



            //////  var filtRows = null;
            //  IEnumerable<DsMarriages.MarriagesRow> filtRows = null;

            //  //filtRows.Where(

            // // 
            


            //      foreach (personRecord _prec in mllRecords)
            //      {

            //          filtRows = mdt.Where(o => o.Date == _prec.date);

            //          foreach (DsMarriages.MarriagesRow pROw in filtRows)
            //          {

            //          }

            //      //  

            //      filtRows = mdt.Where(o => o.IsDeleted == true);

            //  }







            //    //pROw.
            //    //deathsBirthsBll.UpdateSources(pROw.Person_id, "IGI_TRANSCRIPT", pROw.Notes += Environment.NewLine + _prec.source);

            //    List<Guid> peopleToKeep = new List<Guid>();



            //    // get list of unique refs if they exist for the 2 persons


            //    //GetDataByDupeRef(string dupeRef)
            //    dsDBTemp = deathsBirthsBll.GetDeathBirthRecordById(pROw.Person_id);
            //    if (dsDBTemp.Count > 0)
            //        dsDBTemp = deathsBirthsBll.GetDataByDupeRef(dsDBTemp[0].UniqueRef);

            //    foreach (DsDeathsBirths.PersonsRow dupePerson in dsDBTemp)
            //    {
            //        if (pROw.Person_id != dupePerson.Person_id)
            //        {
            //            peopleToKeep.Add(dupePerson.Person_id);
            //        }
            //    }


            //    Guid newRef = Guid.NewGuid();

            //    int evtCount = 1;
            //    foreach (Guid id_ in peopleToKeep)
            //    {
            //        deathsBirthsDLL.UpdateUniqueRefs(id_, newRef.ToString(), peopleToKeep.Count, evtCount);
            //        evtCount++;
            //    }


            //    // records to remove done here

            //    newRef = Guid.NewGuid();
            //    deathsBirthsDLL.UpdateUniqueRefs(pROw.Person_id, newRef.ToString(), 1, 1);



                

           
        }


        private List<personRecord> ReadFile(string path)
        {
            List<string> fileLines = new List<string>(File.ReadAllLines(path));
            List<personRecord> personRecords = new List<personRecord>();
            Regex regex = new Regex(@"\d\d\d\d");

            int idx = -1;
            foreach (string _line in fileLines)
            {

                //  Debug.WriteLine(_line);

                if (_line.Contains("DATE"))
                {
                    if (idx > -1)
                    {
                        Match _match = regex.Match(_line);

                        if (_match.Success)
                            personRecords[idx].date = _match.Value;

                    }

                }

                if (_line.Contains("PAGE Batch"))
                {
                    if (idx > -1)
                    {
                        personRecords[idx].source = _line.Replace(" PAGE Batch #: ", "");
                        personRecords[idx].source = personRecords[idx].source.Substring(1, personRecords[idx].source.Length - 1);
                    }

                }

                if (_line.Contains("1 NAME"))
                {
                    idx++;
                    personRecords.Add(new personRecord());

                    string tp = _line.Replace("1 NAME", "");


                    string[] nameParts = tp.Split('/');

                    if (nameParts.Length > 0)
                        personRecords[idx].cname = nameParts[0].Trim();

                    if (nameParts.Length > 1)
                        personRecords[idx].sname = nameParts[1].Trim();


                }


            }

            personRecords.RemoveAll(o => o.source == "");

            return personRecords;

        }


    }
}