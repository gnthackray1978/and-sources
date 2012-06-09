using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Text.RegularExpressions;

using GedItter.BLL;
//using TDBCore.Datasets;
using GedItter.BirthDeathRecords.BLL;
using GedItter.MarriageRecords.BLL;
using System.Diagnostics;

namespace GedItter
{
    public class csFileFormatHelper
    {
        string rootPath = "";
        string currentFileBeingProcesssed = "";
        string currentParish = "";
        string currentFileContents = "";
        Guid currentSourceId = Guid.Empty;
        bool isCancelled = false;
        bool isWriting = true;
        string testOutPut = "";


     



        private string utilRenameBurialIndexFiles(string lastName)
        {
            string path = @"C:\Users\george\Desktop\tpwork\burials\BaptismIndex";
            lastName = lastName.Replace(" ", "");
            lastName = lastName.Replace(",", "");
            path = Path.Combine(path, lastName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = lastName + DateTime.Now.Ticks.ToString() + ".html";

            File.WriteAllText(Path.Combine(path, fileName), currentFileContents);
            return lastName;
        }


        private string utilGetBIPatternWithReplace(string pattern)
        {
            string retVal = "";

            Regex regEx = new Regex(pattern + "</td>.*?</td>");
            Match match = regEx.Match(currentFileContents);

            if (match.Success)
            {
                retVal = Regex.Replace(match.Value, "<.*?>", "");
                retVal = Regex.Replace(retVal, pattern, "");

            }

            return retVal;
        }

        private List<string> utilGetAllMatchesAsList()
        {
            List<string> retVal = new List<string>();
            string retStr = "";
            Regex regEx = new Regex(@"(?<=(td class=""bodytext"">)).*?(:)");

            MatchCollection matchs = regEx.Matches(currentFileContents);

            foreach (Match _match in matchs)
            {
                if (!retVal.Contains(_match.Value))
                {
                    retStr += "," + _match.Value;
                    retVal.Add(_match.Value);
                }
            }

            return retVal;
        }


        #region process my own text files

        

        //private string ProcessCustomFileFormat(FileInfo file, string fileContent)
        //{
        //    utilCheckForAndRemove(ref fileContent, "the ");
        //    utilCheckForAndRemove(ref fileContent, " by ");


        //    currentSourceId = AddSource(fileContent);

        //    //   MessageBox.Show(currentSourceId.ToString());

        //    currentParish = utilCheckForAndReturn(fileContent, @"(?<=<parish name>).*?(?=</parish name>)");



        //    #region process file content
        //    MatchCollection mcollection = null;

        //    if (fileContent.Length > 1)
        //    {
        //        currentFileBeingProcesssed = file.DirectoryName;
        //        // marriages
        //        Regex _reg = new Regex(@"(?<=<[Mm]arriage>).*?(?=</[Mm]arriage>)", RegexOptions.Singleline);

        //        mcollection = _reg.Matches(fileContent);

        //        foreach (Match _match in mcollection)
        //        {
        //            if (!isCancelled)
        //                this.handleMarriage(_match.Value);
        //        }

        //        //baptisms
        //        _reg = new Regex(@"(?<=<[Bb]aptism>).*?(?=</[Bb]aptism>)", RegexOptions.Singleline);

        //        mcollection = _reg.Matches(fileContent);

        //        foreach (Match _match in mcollection)
        //        {
        //            if (!isCancelled)
        //                this.handleBaptism(_match.Value);
        //        }

        //        //burials 
        //        _reg = new Regex(@"(?<=<[Bb]urial>).*?(?=</[Bb]urial>)", RegexOptions.Singleline);

        //        mcollection = _reg.Matches(fileContent);

        //        foreach (Match _match in mcollection)
        //        {
        //            if (!isCancelled)
        //                this.handleBurial(_match.Value);
        //        }



        //    }


        //    #endregion
        //    return fileContent;
        //}



        private Guid AddSource(string fileContent)
        {
            Guid sourceId = Guid.Empty;

            SourceBLL sourceBll = new SourceBLL();

            if (fileContent == "") 
                return sourceId;


            try
            {
                string dateSTart = "";
                string dateEnd = "";

                bool thackrayFound = Convert.ToBoolean(utilCheckForAndReturn(fileContent, @"(?<=<thackrayFound>).*?(?=</thackrayFound>)"));

                int startDate = Convert.ToInt32(utilCheckForAndReturn(fileContent, @"(?<=<start>).*?(?=</start>)"));
                int endDate = Convert.ToInt32(utilCheckForAndReturn(fileContent, @"(?<=<end>).*?(?=</end>)"));
                string parishName = utilCheckForAndReturn(fileContent, @"(?<=<parish name>).*?(?=</parish name>)");


                DateTime startDateDT = new DateTime(startDate, 1, 1);
                DateTime endDateDT = new DateTime(endDate, 12, 31);

                string parishRef = parishName + startDate.ToString() + endDate.ToString();

                IList<TDBCore.EntityModel.Source> sourcesDataTable = sourceBll.FillSourceTableBySourceRef2(parishRef).ToList();


                if (sourcesDataTable.Count > 0)
                    return sourcesDataTable[0].SourceId;


                ParishsBLL parishsBll = new ParishsBLL();

                var pdt = parishsBll.GetParishByNameFilter2(parishName.Trim()).FirstOrDefault();
                Guid parishId = Guid.Empty;
                if (pdt!= null)
                {
                    parishId = pdt.ParishId;
                }
                else
                {
                    parishId = parishsBll.AddParish(parishName.Trim(), "", "", "", 1550,"",0,decimal.Zero,decimal.Zero);
                }
                sourceId = sourceBll.InsertSource2(parishName + " Parish Register Transcript", "Ancestry.co.uk", true, true, thackrayFound, 1, startDateDT.ToShortDateString(), endDateDT.ToShortDateString(), startDate, endDate, parishRef,0,"");






                SourceMappingParishsBLL smpBll = new SourceMappingParishsBLL();
                SourceMappingsBLL smbll = new SourceMappingsBLL();

                smbll.Insert(sourceId, null, null, 1, null, DateTime.Today.ToShortDateString(), 1);

                smpBll.InsertSourceMappingParish2(parishId, sourceId, 1);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message);
                //MessageBox.Show(ex1.Message);
            }

            return sourceId;
        }


        #region cycle lines 
        private static void CycleLines(List<FileInfo> files)
        {
            //files[0].DirectoryName
            foreach (FileInfo file in files)
            {

                List<string> lines = new List<string>(File.ReadAllLines(file.FullName));

                if (!file.Name.Contains("txt")) continue;

                int idx = 0;
                string findString = "";
                while (idx < lines.Count)
                {

                    //if (!line.Contains("<"))
                    //{
                    //lines[idx] = Regex.Replace(lines[idx], "(^|[^<])[Mm]arriage($|[^>])", "<marriage>");
                    //lines[idx] = Regex.Replace(lines[idx], "(^|[^<])[Bb]aptism($|[^>])", "<baptism>");
                    //lines[idx] = Regex.Replace(lines[idx], "(^|[^<])[Bb]urial($|[^>])", "<burial>");

                    //}
                    //(^|[^<])[Bb]urial($|[^>])
                    if ((lines[idx].ToLower().Trim() == "<marriage>"))
                    {

                        lines.Insert(idx + 1, "   <witness1> </witness1");
                        lines.Insert(idx + 1, "   <witness2> </witness2");
                        lines.Insert(idx + 1, "   <witness3> </witness3");
                        lines.Insert(idx + 1, "   <witness4> </witness4");
                        lines.Insert(idx + 1, "   <witness5> </witness5");

                        lines.Insert(idx + 1, "   <banns> </banns");
                        lines.Insert(idx + 1, "   <lic> </lic");                        
                        
                        lines.Insert(idx+1,   "   <maleTrade> </maleTrade>");
                        lines.Insert(idx + 1, "   <maleLocation> </maleLocation>");
                        lines.Insert(idx + 1, "   <femaleLocation> </femaleLocation>");
                      
                        lines.Insert(idx + 1, "   <femaleSName> </femaleSName>");
                        lines.Insert(idx + 1, "   <femaleCName> </femaleCName");
                        lines.Insert(idx + 1, "   <maleSName> </maleSName>");
                        lines.Insert(idx + 1, "   <maleCName> </maleCName>");
                        lines.Insert(idx + 1, "   <marriageLocation> </marriageLocation");
                        lines.Insert(idx + 1, "   <marriageDate> </marriageDate");

                        idx += 17;

                    }


                    //if ((lines[idx].ToLower().Trim() == "<marriage>" ||
                    //    lines[idx].ToLower().Trim() == "<baptism>" ||
                    //    lines[idx].ToLower().Trim() == "<burial>") &&
                    //    findString != "")
                    //{
                    //    lines.Insert(idx - 1, findString);
                    //    // the lines array has grown by 1 because we inserted something into it
                    //    // so the current index is going to be pointing to the wrong entry
                    //    // correct it
                    //    idx++;
                    //}


                    //if (lines[idx].ToLower().Trim() == "<marriage>") findString = "</marriage>";
                    //if (lines[idx].ToLower().Trim() == "<baptism>") findString = "</baptism>";
                    //if (lines[idx].ToLower().Trim() == "<burial>") findString = "</burial>";
                    idx++;
                }

                //if (findString != "")
                //{
                //    lines.Insert(lines.Count, findString);
                //}


                file.Delete();
                //Application.DoEvents();

                File.WriteAllLines(file.FullName, lines.ToArray());
            }
        }
        #endregion



//        private void handleMarriage(string marriageContent)
//        {
//            bool isBtp = false;
//            bool isBanns = false;
//            bool isLic = false;
//            bool isOtp =false;
//            bool isWidower = false;
//            bool isWidow =false;
//            bool isSpinster =false;
//            bool isBach = false;

//            string marriageDate = "";
//            string spareDate = "";


//            string maleCName = "";
//            string maleSName = "";

//            string femaleCName = "";
//            string femaleSName = "";

//            string femaleLocation = "";
//            string femaleOccupation = "";

//            string maleLocation = "";
//            string maleOccupation = "";

//            List<string> witnessCollection = new List<string>();

//            string originalContent = marriageContent;
//                int marriageYearInt = 0;
//                int marriageYearIntSpare = 0;

//            currentFileContents = marriageContent;
//            string notes = utilCheckForAndReturn(  marriageContent, @"(?<=\[).*?(?=\])");

//            if (notes != "")
//            {
//                utilCheckForAndRemove(ref marriageContent, @"(\[).*?(\])");

//            }
//            string outString = "";

//            try
//            {

//                #region handle marriage parties
//                string date = "";

//                bool maleCantWrite = false;
//                bool femaleCantWrite = false;

//                MatchCollection mcollection = null;

//                isSpinster = utilCheckForAndRemove(ref marriageContent, "<spin>");
//                isWidow = utilCheckForAndRemove(ref marriageContent, "<widow>");
//                isWidower = utilCheckForAndRemove(ref marriageContent, "<widower>");
//                isBanns = utilCheckForAndRemove(ref marriageContent, "<banns>");
//                isLic = utilCheckForAndRemove(ref marriageContent, "<lic>");
//                isBtp = utilCheckForAndRemove(ref marriageContent, "<btp>");
//                isBach = utilCheckForAndRemove(ref marriageContent, "<bach>");
//                isOtp = utilCheckForAndRemove(ref marriageContent, "<otp>");


//                utilCheckForAndRemoveDate(ref marriageContent, out marriageDate, out spareDate,out marriageYearInt, out marriageYearIntSpare);



//                Regex regEx = new Regex(@"(?<=[Ww]it:).*($|\r|\n)", RegexOptions.Singleline);

//                Match _matchTp = regEx.Match(marriageContent);

//                if (_matchTp.Success)
//                {
//                    if (_matchTp.Value.Contains(',') || _matchTp.Value.Contains(" and "))
//                    {

//                        witnessCollection.AddRange(Regex.Split(_matchTp.Value, " and |,"));
//                    }
//                    else
//                    {
//                        witnessCollection.Add(_matchTp.Value);
//                    }
//                }

//                utilCheckForAndRemove(ref marriageContent, "([Ww]it:).*($|\r|\n)");

//                if (this.isCancelled) return;

//                regEx = new Regex(@"(^.*(?= and ))|((?<= and ).*$)", RegexOptions.Singleline);
//                mcollection = regEx.Matches(marriageContent);


//                if (!marriageContent.Contains('+'))
//                {
//                    //FrmImporterDialog frmImporterDialog = new FrmImporterDialog(true);
//                    //frmImporterDialog.Caption = this.currentParish+ " :Mark which entry is the man with a + " ;
//                    //frmImporterDialog.Result = marriageContent;
                  
//                    //frmImporterDialog.Notes = this.currentFileContents;
//                    //frmImporterDialog.ShowDialog();
//                  //  this.isCancelled = frmImporterDialog.IsCancelled;

//                  //  marriageContent = frmImporterDialog.Result;
//                }


//                foreach (Match _match in mcollection)
//                {
//                    // male
//                    string workingTemp = _match.Value;

//                    if (this.isCancelled) break;

//                    if (workingTemp.Contains('+'))
//                    {
//                        workingTemp = workingTemp.Replace("+", "");
//                        workingTemp = workingTemp.Replace(",", "");
//                        maleCantWrite = utilCheckForAndRemove(ref workingTemp, "<cantWrite>");
//                        utilSetLocation(ref maleLocation, ref workingTemp);

//                        utilSetOccupation(ref maleOccupation, ref workingTemp);

//                        // ok so in theory we should just be left with a name!
//                        //  processName(workingTemp, out maleCName, out maleSName);
//                        utilSetDoubleName(ref maleCName, ref maleSName, ref workingTemp);
//                    }
//                    else
//                    {
//                        // get locations
//                        workingTemp = workingTemp.Replace(",", "");
//                        femaleCantWrite = utilCheckForAndRemove(ref workingTemp, "<cantWrite>");
//                        utilSetLocation(ref femaleLocation, ref workingTemp);

//                        utilSetOccupation(ref femaleOccupation, ref workingTemp);


//                        // ok so in theory we should just be left with a name!
//                        //  processName(workingTemp, out femaleCName, out femaleSName);
//                        utilSetDoubleName(ref femaleCName, ref femaleSName, ref workingTemp);
//                    }


//                }
//                #endregion

//            }
//            catch (Exception ex1)
//            {
//                Debug.WriteLine(ex1.Message);
//                //MessageBox.Show(ex1.Message);
//            }


//            outString = "";
//         //   outString = Environment.NewLine + "<MARRIAGE>" + Environment.NewLine +

//             //  GetTag("ParishName", currentParish, true) +
//             //  GetTag("MarriageDate", marriageDate, true) +
//            //   GetTag("MarriagePartyMaleCName", maleCName, true) +
//            //   GetTag("MarriagePartyMaleSName", maleSName, true) +
//            //   GetTag("MarriagePartyFemaleCName", femaleCName, true) +
//            //   GetTag("MarriagePartyFemaleSName", femaleSName, true) +

//            //   GetTag("MarriageMaleOccupation", maleOccupation, true) +
//            //   GetTag("MarriageMaleLocation", maleLocation, true) +
//            //   GetTag("MarriageFemaleOccupation", femaleOccupation, true) +
//            //   GetTag("MarriageFemaleLocation", femaleLocation, true) +

//            //   GetTag("MarriageFemaleIsWidow", isWidow.ToString(), true) +
//            outString += GetTag("MarriageFemaleSpinster", isSpinster.ToString(), true) +
//              // GetTag("MarriageMaleIsWidower", isWidower.ToString(), true) +
//               GetTag("MarriageIsBtp", isBtp.ToString(), true) +
//             //  GetTag("MarriageIsBanns", isBanns.ToString(), true) +
//            //   GetTag("MarriageIsLicense", isLic.ToString(), true) +
//               GetTag("MarriageIsOTP", isOtp.ToString(), true) +
//               GetTag("MarriageNotes", notes, true);


//            int witIdx = 0;

//            string wit1 = "";
//            string wit2 = "";
//            string wit3 = "";
//            string wit4 = "";


//            string witNotesStr = "";
//            foreach (string wit in witnessCollection)
//            {
//                witNotesStr += GetTag("Witness" + witIdx.ToString(), wit, true);


//                if (witIdx == 0)
//                    wit1 = witnessCollection[0];

//                if (witIdx == 1)
//                    wit2 = witnessCollection[1];
//                if (witIdx == 2)
//                    wit3 = witnessCollection[2];
//                if (witIdx == 3)
//                    wit4 = witnessCollection[3];

//                 witIdx++;               
//            }


//            if (witnessCollection.Count > 4)
//            {
//                outString += witNotesStr;
//            }



//            if (maleCName == "" || maleSName == "" || femaleCName == "" || femaleSName == "")
//            {
//                //MessageBox.Show(originalContent, this.currentParish);
//            }


//           // outString += Environment.NewLine + "</MARRIAGE>";



//            if (isWriting)
//            {
//                MarriagesBLL marriageBll = new MarriagesBLL();

//                Guid marriageId = marriageBll.InsertMarriage(marriageDate, femaleCName, Guid.Empty, "", femaleLocation, femaleSName, maleCName, Guid.Empty, outString, maleLocation,
//                    maleSName, "Yorkshire", currentParish, "", wit1, wit2, wit3, wit4, marriageYearInt, maleOccupation, femaleOccupation,
//                    isLic, isBanns, isWidow, isWidower, 1, 
//                    new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"), 
//                    new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"),
//                    new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"),0,0,"",0,0);

//                SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();

//                sourceMappingBll.Insert(this.currentSourceId, null, marriageId, 1, null, DateTime.Today.ToShortDateString(), null);

//            }
//            testOutPut += outString;
//        }

//        private void handleBaptism(string baptismContent)
//        {
//            string cName = "";
//            string sName = "";
//            bool isMale = false;
//            string occupation = "";
//            string location = "";
//            string fatherCName = "";
//            string fatherSName = "";
//            string fatherOccupation = "";
//            string fatherLocation = "";
//            bool fatherIsWidower = false;
//            string motherCName = "";
//            string motherSName = "";
//            string motherOccupation = "";
//            string motherLocation = "";
//            bool motherIsWidow = false;
//            string baptismDate = "";
//            string birthDate = "";

//            bool isBastard = false;

//            string originalContent = baptismContent;
//            int baptismYearLower = 0;
//            int baptismYearUpper = 0;
          

//            string notes = utilCheckForAndReturn(baptismContent, @"(?<=\[).*?(?=\])");
//            if (notes != "")
//            {
//                utilCheckForAndRemove(ref baptismContent, @"(\[).*?(\])");

//            }
 
//            currentFileContents = baptismContent;

//            try
//            {

//                #region fill out variables

//                utilCheckForAndRemoveDate(ref baptismContent, out birthDate, out baptismDate, out baptismYearLower, out baptismYearUpper);

//                if (baptismContent.Contains("son"))
//                {
//                    isMale = true;
//                }
//                else
//                {
//                    isMale = false;
//                }

//                string[] bParts = Regex.Split(baptismContent, "son|daughter");

//                if (bParts.Length > 0 && bParts[0] != "")
//                {
//                    cName = utilCheckValidChristianName(bParts[0]);


//                }

//                if (bParts.Length > 1)
//                {
//                    string[] birthParts = new string[2];
//                    int bIdx = 0;

//                    if (bParts[1].Contains(" and "))
//                    {
//                        // 2 parents recorded

//                        birthParts = Regex.Split(bParts[1], " and ");

//                        while (bIdx < birthParts.Length)
//                        {
//                            if (birthParts[bIdx].Contains('+'))
//                            {
//                                #region handle father

//                                utilSetLocation(ref fatherLocation, ref birthParts[bIdx]);
//                                utilSetOccupation(ref fatherOccupation, ref birthParts[bIdx]);
//                                fatherIsWidower = utilCheckForAndRemove(ref birthParts[bIdx], "<widower>");

//                                utilSetDoubleName(ref fatherCName, ref fatherSName, ref birthParts[bIdx]);
//                                sName = fatherSName;

//                                #endregion
//                            }
//                            else
//                            {
//                                #region mother

//                                utilSetLocation(ref motherLocation, ref birthParts[bIdx]);

//                                utilSetOccupation(ref motherOccupation, ref birthParts[bIdx]);

//                                motherIsWidow = utilCheckForAndRemove(ref birthParts[bIdx], "<widow>");

//                                utilSetDoubleName(ref motherCName, ref motherSName, ref birthParts[bIdx]);

//                                #endregion
//                            }

//                            bIdx++;
//                        }


//                    }
//                    else
//                    {
//                        // 1 parent recorded

//                        if (bParts[1].Contains('+'))
//                        {
//                            #region handle father

//                            utilSetLocation(ref fatherLocation, ref bParts[1]);

//                            utilSetOccupation(ref fatherOccupation, ref bParts[1]);


//                            fatherIsWidower = utilCheckForAndRemove(ref bParts[1], "<widower>");

//                            utilSetDoubleName(ref fatherCName, ref fatherSName, ref bParts[1]);

//                            sName = fatherSName;

//                            #endregion
//                        }
//                        else
//                        {
//                            #region single mother
//                            //motherOccupation = utilCheckForAndReturn(bParts[1], @"#.*?#");
//                            //utilCheckForAndRemove(ref bParts[1], @"#.*?#");

//                            //motherLocation = utilCheckForAndReturn(bParts[1], @";.*?;");
//                            //utilCheckForAndRemove(ref bParts[1], @";.*?;");



//                            utilSetLocation(ref motherLocation, ref bParts[1]);

//                            utilSetOccupation(ref motherOccupation, ref bParts[1]);


//                            motherIsWidow = utilCheckForAndRemove(ref bParts[1], "<widow>");

//                            utilSetDoubleName(ref motherCName, ref motherSName, ref bParts[1]);

//                            //if (bParts[1].Trim().Contains(" "))
//                            //{
//                            //    string[] nameParts = bParts[1].Split(' ');

//                            //    if (nameParts.Length == 2)
//                            //    {
//                            //        motherCName = nameParts[0];
//                            //        motherSName = nameParts[1];
//                            //    }

//                            //}
//                            //else
//                            //{
//                            //    motherCName = bParts[1].Trim();
//                            //}

//                            sName = motherSName;
//                            isBastard = true;
//                            #endregion
//                        }
//                    }




//                }
//                #endregion

//            }
//            catch (Exception ex1)
//            {
//                //MessageBox.Show(ex1.Message);
//            }

//            testOutPut = "";//+= Environment.NewLine + "<BAPTISM>" + Environment.NewLine+
//              //  GetTag("ParishName", currentParish, true) +
//             //   GetTag("BirthDate", birthDate, true) +
//             //   GetTag("BaptismDate", baptismDate, true) +
//             //   GetTag("IsMale", isMale.ToString(), true) +
//              //  GetTag("CName", cName, true) +
//             //   GetTag("SName", sName, true) +

//             //   GetTag("FatherCName", fatherCName, true) +
//            //    GetTag("FatherSName", fatherSName, true) +

//           //     GetTag("MotherCName", motherCName, true) +
//            //    GetTag("MotherSName", motherSName, true) +

//             //   GetTag("FatherOccupation", fatherOccupation, true) +
//             //   GetTag("FatherLocation", fatherLocation, true) +

//            testOutPut += GetTag("MotherOccupation", motherOccupation, true) +
//           GetTag("MotherLocation", motherLocation, true) +

//           GetTag("IsBastard", isBastard.ToString(), true) +

//           GetTag("Widower", fatherIsWidower.ToString(), true) +
//           GetTag("Widow", motherIsWidow.ToString(), true) +
//           GetTag("Notes", notes, true);// +Environment.NewLine + "</BAPTISM>";

            
//                if (cName == "" && sName == "")
//                {
//                    //MessageBox.Show(originalContent, this.currentParish);
//                }

//                DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();
 
//                if (isWriting)
//                {
//                    Guid personId = deathsBirthsBLL.InsertDeathBirthRecord(isMale, cName, sName, 
//                        utilFormatLocation(fatherLocation), birthDate, baptismDate, "", "", fatherCName, fatherSName
//                        , motherCName, motherSName, "", testOutPut, baptismYearLower, baptismYearUpper,
//                        0, "Yorkshire", "", "", "", "", 0, "", "", fatherOccupation,
//                        new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"), 1,
//                        new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"),
//                        new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"),0,0,"",0,0,false,false);


//                    SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();

//                    sourceMappingBll.Insert(this.currentSourceId, null, null,1,personId, DateTime.Today.ToShortDateString(), null);


//                }


//                //Environment.NewLine + "Birth: " + birthDate + Environment.NewLine +
//                //"Baptism: " + baptismDate + Environment.NewLine +
//                //cName + " " + sName + Environment.NewLine +
//                //"Father name " + Environment.NewLine +
//                //fatherCName + " " + fatherSName + Environment.NewLine +
//                //"Mother name " + Environment.NewLine +
//                //motherCName + " " + motherSName + Environment.NewLine +

//                //"Father Occupation: "  + fatherOccupation + Environment.NewLine +
//                //"Father Location: "  + fatherLocation + Environment.NewLine +
//                //"Mother Location: " + motherLocation + Environment.NewLine +
//                //"Mother Occupation: "  + motherOccupation + Environment.NewLine +

//                //"Is Male: " + isMale.ToString() + Environment.NewLine +
//                //"Is Bastard: " + isBastard.ToString() + Environment.NewLine +
//                //"Father Widower: " + fatherIsWidower.ToString() + Environment.NewLine +
//                //"Mother Widow: " + motherIsWidow.ToString() + Environment.NewLine + Environment.NewLine + "Original Content " + Environment.NewLine +
//                //originalContent + Environment.NewLine + Environment.NewLine;


                
//        }

//        private void handleBurial(string burialContent)
//        {
//            bool deceasedIsMale = false;
//            bool deceasedIsWidow = false;
//            bool deceasedIsWidower = false;
//            bool deceasedIsBach = false;
//            bool isSpin = false;
//            string deceasedOccupation = "";
//            string deceasedCName = "";
//            string deceasedSName = "";
//            string deceasedAge = "";
//            string deceasedDateOfDeath = "";
//            int deceasedDeathYearLower = 0;
//            int deceasedDeathYearUpper = 0;
//            string deceasedLocation = "";
//            string spareDate = "";

//            string deceasedSpouseCName = "";
//            string deceasedSpouseSName = "";
//            string deceasedSpouseLocation = "";
//            string deceasedSpouseOccupation = "";
//            bool deceasedSpouseIsMale = true;
//            bool deceasedSpouseIsWidow = false;
//            bool deceasedSpouseIsWidower = false;
          

//            bool motherIsWidow = false;
//            bool fatherIsWidower = false;

//            string fatherCName = "";
//            string fatherSName = "";
//            string motherCName = "";
//            string motherSName = "";



//            string fatherLocation = "";
//            string motherLocation = "";

//            string fatherOccupation = "";
//            string motherOccupation = "";

//            string notes = utilCheckForAndReturn(burialContent, @"(?<=\[).*?(?=\])");
//            if (notes != "")
//            {
//                utilCheckForAndRemove(ref burialContent, @"(\[).*?(\])");

//            }
//            // get date of death
//            utilCheckForAndRemoveDate(ref burialContent, out deceasedDateOfDeath, out spareDate,out deceasedDeathYearLower, out deceasedDeathYearUpper);
            
//            // work out age
//            string ageYears = "";
//            string ageMonths = "";
//            string ageWeeks = "";

//            string originalContent = burialContent;
            
//            // ADD see bish hill sen
//            //????????????
//            // widow of???????
//            //
//            this.currentFileContents = burialContent;

//            //(?<=aged)((\s|\d)*)(w|m|w).*
//            isSpin = utilCheckForAndRemove(ref burialContent, "<spin>");
//            deceasedIsBach = utilCheckForAndRemove(ref burialContent, "<bach>");

//            burialContent = Regex.Replace(burialContent, "of ", " ");

//            string birthStr = "";
//            int birthInt = 0;

//            try
//            {

//                #region get age

//                DateTime dateOfBurial = DateTime.Parse(deceasedDateOfDeath);
//                DateTime calcDateOfBirth = DateTime.Today;

//                if (utilCheckForAndReturn(burialContent, @"(?<=aged)((\s|\d)*)(y|m|w).*") != "")
//                {
//                    ageYears = utilCheckForAndReturn(burialContent, @"(?<=aged).*(?=y)");
//                    ageMonths = utilCheckForAndReturn(burialContent, @"(?<=aged).*(?=m)");
//                    ageWeeks = utilCheckForAndReturn(burialContent, @"(?<=aged).*(?=w)");
//                }
//                else
//                {
//                    ageYears = utilCheckForAndReturn(burialContent, @"(?<=aged)((\s|\d)*)");
//                }

//                if (ageYears != "")
//                {
//                    int years = Convert.ToInt32(ageYears.Trim());
//                    calcDateOfBirth = dateOfBurial.AddYears(0 - years);

//                    birthStr = String.Format("{0:d MMM yyyy}", calcDateOfBirth);// calcDateOfBirth.ToShortDateString();
//                    birthInt = calcDateOfBirth.Year;


//                }

//                if (ageMonths != "")
//                {
//                    int months = Convert.ToInt32(ageMonths.Trim());
//                    calcDateOfBirth = dateOfBurial.AddMonths(0 - months);
//                    birthStr = String.Format("{0:d MMM yyyy}", calcDateOfBirth);// calcDateOfBirth.ToShortDateString();
//                    birthInt = calcDateOfBirth.Year;
//                }

//                if (ageWeeks != "")
//                {
//                    int weeks = Convert.ToInt32(ageWeeks.Trim());
//                    calcDateOfBirth = dateOfBurial.AddDays(0 - (7 * weeks));
//                    birthStr = String.Format("{0:d MMM yyyy}", calcDateOfBirth);// calcDateOfBirth.ToShortDateString();
//                    birthInt = calcDateOfBirth.Year;
//                }




//                utilCheckForAndRemove(ref burialContent, @"aged((\s|\d)*)");
//                burialContent = Regex.Replace(burialContent, "weeks", "");
//                burialContent = Regex.Replace(burialContent, "months", "");

//                #endregion

//                if (burialContent.Contains(" son ") || burialContent.Contains(" daughter "))
//                {
//                    if (burialContent.Contains(" son "))
//                    {
//                        deceasedIsMale = true;

//                    }
//                    else
//                    {
//                        deceasedIsMale = false;

//                    }


//                    string[] childDeceasedParts = Regex.Split(burialContent, " son|daughter ");

//                    if (childDeceasedParts.Length == 2)
//                    {
//                        #region deceased was child

//                        // childs name
                   

//                         deceasedCName =  utilCheckValidChristianName (childDeceasedParts[0]);
//                        // now deal with parents 
//                        string tpParentString = childDeceasedParts[1];

//                        if (tpParentString.Contains(" and "))
//                        {
//                            string[] marriageParts = Regex.Split(tpParentString, " and ");
//                            int idxMarriage = 0;
//                            while(idxMarriage < marriageParts.Length)
//                            {
//                                if (marriageParts[idxMarriage].Contains('-'))
//                                {
//                                    #region handle mother
 
//                                    utilSetLocation(ref motherLocation, ref marriageParts[idxMarriage]);
//                                    utilSetOccupation(ref motherOccupation, ref marriageParts[idxMarriage]);

//                                    motherIsWidow = utilCheckForAndRemove(ref marriageParts[idxMarriage], "<widow>");
 
//                                    utilSetDoubleName(ref motherCName, ref motherSName, ref marriageParts[idxMarriage]);

//                                            deceasedSName = deceasedSpouseSName;
//                                            deceasedLocation = motherLocation;

//                                    #endregion
//                                }
//                                else
//                                {
//                                    #region handle father
 
//                                    utilSetLocation(ref fatherLocation, ref marriageParts[idxMarriage]);
//                                    utilSetOccupation(ref fatherOccupation, ref marriageParts[idxMarriage]);

//                                    fatherIsWidower = utilCheckForAndRemove(ref marriageParts[idxMarriage], "<widower>");
 
//                                    utilSetDoubleName(ref fatherCName, ref fatherSName, ref marriageParts[idxMarriage]);

//                                            deceasedSName = deceasedSpouseSName;
//                                            deceasedLocation = fatherLocation;

//                                    #endregion
//                                }
//                                idxMarriage++;
//                            }
//                        }
//                        else
//                        { 
//                            // only 1 parent 
                             
                              
//                            if (tpParentString.Contains('-'))
//                            {
 
//                                utilSetLocation(ref motherLocation, ref tpParentString);
//                                utilSetOccupation(ref motherOccupation, ref tpParentString);

//                                motherIsWidow = utilCheckForAndRemove(ref tpParentString, "<widow>");
 
//                                utilSetDoubleName(ref motherCName, ref motherSName, ref tpParentString);

//                                deceasedSName = deceasedSpouseSName;
//                                deceasedLocation = motherLocation;

//                            }
//                            else
//                            { 
 
//                                utilSetLocation(ref fatherLocation, ref tpParentString);
//                                utilSetOccupation(ref fatherOccupation, ref tpParentString);

//                                fatherIsWidower = utilCheckForAndRemove(ref tpParentString, "<widower>");
 
//                                utilSetDoubleName(ref fatherCName, ref fatherSName, ref tpParentString);
                               
//                            }

//                        }

//                        #endregion
//                    }

//                }
//                else
//                {

//                    if (burialContent.Contains(" wife ") | burialContent.Contains("<widow>"))
//                    {
//                        string[] wifeDeceasedParts = Regex.Split(burialContent, " wife|<widow> ");

//                        if (wifeDeceasedParts.Length == 2)
//                        {
//                            #region deceased was married
//                            deceasedCName = wifeDeceasedParts[0];

//                            string tpDecPart2 = wifeDeceasedParts[1];

//                            utilSetLocation(ref deceasedSpouseLocation, ref tpDecPart2);
//                            utilSetOccupation(ref deceasedSpouseOccupation, ref tpDecPart2);

//                            deceasedSpouseIsWidow = utilCheckForAndRemove(ref tpDecPart2, "<widow>");
//                            deceasedSpouseIsWidower = utilCheckForAndRemove(ref tpDecPart2, "<widower>");

//                            utilCheckForAndRemove(ref tpDecPart2, @" and ");

//                            deceasedSpouseIsMale = true;


//                            utilSetDoubleName(ref deceasedSpouseCName, ref deceasedSpouseSName, ref tpDecPart2);



//                            #endregion
//                        }
//                        else
//                        {
//                            if (wifeDeceasedParts.Length == 1)
//                            {
//                                deceasedIsWidow = utilCheckForAndRemove(ref wifeDeceasedParts[0], "<widow>");
//                                deceasedIsWidower = utilCheckForAndRemove(ref wifeDeceasedParts[0], "<widower>");
//                                utilSetLocation(ref deceasedLocation, ref wifeDeceasedParts[0]);
//                                utilSetOccupation(ref deceasedOccupation, ref wifeDeceasedParts[0]);

//                                if (wifeDeceasedParts[0].Contains('+'))
//                                {
//                                    deceasedIsMale = true;
//                                }
//                                else
//                                {

//                                    deceasedIsMale = false;
//                                }

//                                utilSetDoubleName(ref deceasedCName, ref deceasedSName, ref wifeDeceasedParts[0]);
//                            }
//                            else
//                            {
                                
//                            }

//                        }
//                    }
//                    else
//                    {
//                        #region standard death

//                        deceasedIsWidow = utilCheckForAndRemove(ref burialContent, "<widow>");
//                        deceasedIsWidower = utilCheckForAndRemove(ref burialContent, "<widower>");

//                        deceasedIsWidower = utilCheckForAndRemove(ref burialContent, " and ");


//                        utilSetLocation(ref deceasedLocation, ref burialContent);
//                        utilSetOccupation(ref deceasedOccupation, ref burialContent);
 
//                        if (burialContent.Contains('+'))
//                        {
//                            deceasedIsMale = true;
//                        }
//                        else
//                        {

//                            deceasedIsMale = false;
//                        }
 
//                        utilSetDoubleName(ref deceasedCName, ref deceasedSName, ref burialContent);


//                        #endregion

//                    }
//                }

//            }
//            catch (Exception ex1)
//            {
//                //MessageBox.Show(originalContent + Environment.NewLine +  ex1.Message, this.currentFileBeingProcesssed);
//            }


//            if (deceasedSName == "")
//            {
//                if (fatherSName != "")
//                    deceasedSName = fatherSName;

//                if (fatherSName == "" && motherSName != "")
//                    deceasedSName = motherSName;
//            }


//            if (deceasedLocation == "")
//            {
//                if (fatherLocation != "")
//                    deceasedLocation = fatherLocation;

//                if (fatherLocation == "" && motherLocation != "")
//                    deceasedLocation = motherLocation;
//            }


//             string tempDisplay = "";
//           //  tempDisplay = tempDisplay + Environment.NewLine +
                 
//               //  Environment.NewLine + "<BURIAL>" + Environment.NewLine +
//               //  GetTag("ParishName", currentParish, true) +
//         //    GetTag("DateOfDeath", deceasedDateOfDeath, true) +
//          //   GetTag("DeceasedCName", deceasedCName, true) +
//          //   GetTag("DeceasedSName", deceasedSName, true) +
//             tempDisplay += GetTag("DeceasedBach", deceasedIsBach.ToString(), true) +
//             GetTag("DeceasedWidow", deceasedIsWidow.ToString(), true) +
//             GetTag("DeceasedWidower", deceasedIsWidower.ToString(), true) +

//             GetTag("DeceasedAge", deceasedAge, true) +
//           //  GetTag("DeceasedOccupation", deceasedOccupation, true) +
//           //  GetTag("DeceasedLocation", deceasedLocation, true) +
//           //  GetTag("DeceasedIsMale", deceasedIsMale.ToString(), true) +

//        // /// //   GetTag("FatherCName", fatherCName, true) +
////GetTag("FatherSName", fatherSName, true) +
//          //   GetTag("FatherLocation", fatherLocation, true) +
//          //   GetTag("FatherOccupation", fatherOccupation, true) +
//             GetTag("FatherIsWidower", fatherIsWidower.ToString(), true) +


//          //   GetTag("MotherCName", motherCName, true) +
//         //    GetTag("MotherSName", motherSName, true) +
//             GetTag("MotherLocation", motherLocation, true) +
//             GetTag("MotherOccupation", motherOccupation, true) +
//             GetTag("MotherIsWidow", motherIsWidow.ToString(), true) +

//           //  GetTag("DeceasedSpouseCName", deceasedSpouseCName, true) +
//           //  GetTag("DeceasedSpouseSName", deceasedSpouseSName, true) +
//             GetTag("DeceasedSpouseLocation", deceasedSpouseLocation, true) +
//             GetTag("DeceasedSpouseOccupation", deceasedSpouseOccupation, true) +
//             GetTag("DeceasedSpouseIsWidow", deceasedSpouseIsWidow.ToString(), true) +
//             GetTag("DeceasedSpouseIsWidower", deceasedSpouseIsWidower.ToString(), true) +
//             GetTag("DeceasedSpouseIsMale", deceasedSpouseIsMale.ToString(), true) +
//             GetTag("Notes", notes, true);// +Environment.NewLine + "</BURIAL>";


//             DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();


          


//             if (isWriting)
//             {
//                Guid personId= deathsBirthsBLL.InsertDeathBirthRecord(
//                     deceasedIsMale,
//                     deceasedCName,
//                     deceasedSName, "",
//                     birthStr,
//                     birthStr,
//                     deceasedDateOfDeath,
//                     utilFormatLocation(deceasedLocation),
//                     fatherCName,
//                     fatherSName,
//                     motherCName,
//                     motherSName,
//                     "",
//                     tempDisplay,
//                     birthInt,
//                     birthInt,
//                     deceasedDeathYearLower,
//                     "", "Yorkshire", deceasedOccupation, "", "", 0, 
//                     deceasedSpouseCName, 
//                     deceasedSpouseSName, 
//                     fatherOccupation,
//                     new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"), 1, 
//                     new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"),
//                     new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699"),0,0,"",0,0,false,false);



//                 SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();

//                 sourceMappingBll.Insert(this.currentSourceId, null, null, 1, personId, DateTime.Today.ToShortDateString(), null);
//             }
//             testOutPut += tempDisplay;

//             if (deceasedCName == "" && deceasedSName == "")
//             {
//                 //MessageBox.Show(originalContent, this.currentParish);
//             }


//        }

        #endregion


        #region util functions
        private string utilFormatLocation(string deceasedLocation)
        {


            if ((deceasedLocation.Trim().ToLower() != currentParish.Trim().ToLower()) && deceasedLocation.Trim() != "")
            {
                deceasedLocation = deceasedLocation + ", " + currentParish;
            }
            else
            {
                if (deceasedLocation == "")
                {
                    deceasedLocation =  currentParish;
                }
            }


            return deceasedLocation;
        }




        private void utilGetFileList(List<FileInfo> files, string getFileList)
        {
            //FolderBrowserDialog fbd = new FolderBrowserDialog();
           // fbd.ShowDialog();

            rootPath = getFileList;


            DirectoryInfo dinfo = new DirectoryInfo(rootPath);

            DirectoryInfo[] folderList = dinfo.GetDirectories("*.*", System.IO.SearchOption.AllDirectories);

            foreach (DirectoryInfo di in folderList)
            {
                files.AddRange(di.GetFiles());

            }



            files.AddRange(dinfo.GetFiles());
        }


        private string GetTag(string tagName, string tagContent, bool addNewLine)
        {
            if (tagContent.Trim() == "" || tagContent.ToLower() == "true")
            {
                if (addNewLine)
                    return "    <" + tagName + ">" + tagContent + "</" + tagName + ">" + Environment.NewLine;
                else
                    return "    <" + tagName + ">" + tagContent + "</" + tagName + ">";
            }
            else
            {
                return "";
            }
        }
  


        private void utilSetDoubleName(ref string cName, ref string sName, ref string source)
        {

            source = Regex.Replace(source, @"of ", " ");
            source = Regex.Replace(source, @"\s+", " ");
            source = source.Replace("+", "");
            if (source.Trim().Contains(" "))
            {
                string[] nameParts = source.Trim().Split(' ');

                if (nameParts.Length == 2)
                {
                    cName = nameParts[0];
                    sName = nameParts[1];
                }

            }
            else
            {
                cName = source.Trim();
            }
        }



        private void utilSetOccupation(ref string maleOccupation, ref string workingTemp)
        {


            maleOccupation = utilCheckForAndReturn(workingTemp, @"#.*?#");
            utilCheckForAndRemove(ref maleOccupation, "of ");
            utilCheckForAndRemove(ref workingTemp, @"#.*?#");
            
            workingTemp = workingTemp.Replace("#", "");

        }

        private void utilSetLocation(ref string maleLocation, ref string workingTemp)
        {


            maleLocation = utilCheckForAndReturn(workingTemp, @";.*?;");
            utilCheckForAndRemove(ref maleLocation, "of ");

            utilCheckForAndRemove(ref workingTemp, @";.*?;");


            maleLocation = maleLocation.Replace(";", "").Trim();

            

        }


        private string utilCheckValidChristianName(string cName)
        {
            string retVal = "";

            if (cName.Trim().Contains(' '))
            {
                
                //FrmImporterDialog frmImporterDialog = new FrmImporterDialog(true);
                //frmImporterDialog.Result = cName;
                //frmImporterDialog.Caption = this.currentParish;
                //frmImporterDialog.Notes = this.currentFileContents;
                //frmImporterDialog.ShowDialog();

                //this.isCancelled = frmImporterDialog.IsCancelled;
                
                //retVal = frmImporterDialog.Result;

            }
            else
            {
                retVal = cName;
            }
            return retVal;
        }


        /// <summary>
        /// looks for all instancse of dates , parses them and then deletes them from the original string
        /// </summary>
        /// <param name="marriageContent"></param>
        /// <param name="dateLower"></param>
        /// <param name="dateUpper"></param>
        private void utilCheckForAndRemoveDate(ref string marriageContent, out string dateLower, out string dateUpper, 
            out int dateIntLower, out int dateIntUpper)
        {
            Regex regEx = new Regex(@"(\r|\n|^).*?\d{4}.*?(\r|\n|$)", RegexOptions.Singleline);
            MatchCollection mcollection = null;
            mcollection = regEx.Matches(marriageContent);
            List<DateTime> dateList = new List<DateTime>();
            List<string> strList = new List<string>();
            dateLower = "";
            dateUpper = "";

            dateIntLower = 0;
            dateIntUpper = 0;

            foreach (Match match in mcollection)
            {
                DateTime tpDate = DateTime.Today;
                string date = "";

                date = utilconvertDateStr(match.Value, out tpDate);

                if (tpDate != DateTime.Today)
                {
                    dateList.Add(tpDate);
                    strList.Add(date);
                }
            }

            if (dateList.Count > 1)
            {
                if (DateTime.Compare(dateList[0], dateList[1]) > 0)
                {
                    dateUpper = strList[0];
                    dateIntUpper = dateList[0].Year;
                    dateIntLower = dateList[1].Year;
                    dateLower = strList[1];
                }
                else
                {
                    dateUpper = strList[1];
                    dateLower = strList[0];

                    dateIntUpper = dateList[1].Year;
                    dateIntLower = dateList[0].Year;
                }
            }
            else
            {
                if (strList.Count > 0)
                {
                    dateLower = strList[0];
                    dateIntLower = dateList[0].Year;
                }

            }

            marriageContent = Regex.Replace(marriageContent, @".*?\d{4}.*", "",RegexOptions.Multiline);


        }


        /// <summary>
        /// attempts to parse a datestring and return a datetime and a string
        /// if it has problems it should prompt user
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="outDate"></param>
        /// <returns></returns>
        private string utilconvertDateStr(string dateStr, out DateTime outDate)
        {
            string retVal = "";
            DateTime result;
            outDate = DateTime.Today;


            string yearVal = utilCheckForAndReturn(dateStr, @"\d\d\d\d");
            // if the year is stuck at the front then that will cause us problems 
            // fix it
            if (utilCheckForAndRemove(ref dateStr, @"^\d\d\d\d"))
            {
                dateStr = dateStr.Trim() + " " + yearVal;
            }


            if (DateTime.TryParse(dateStr, out result))
            {
                retVal = String.Format("{0:d MMM yyyy}", result);// calcDateOfBirth.ToShortDateString();
             

              //  retVal = result.ToShortDateString();


                outDate = result;
            }
            else
            {
                // its just the year probably
                if (dateStr.Trim().Length == 4)
                {
                    retVal = utilconvertDateStr("1 Jan "+dateStr.Trim(), out outDate);
                }
                else
                {

                    if (!this.isCancelled)
                    {
                        //FrmImporterDialog frmImporterDialog = new FrmImporterDialog(true);
                        //frmImporterDialog.Result = dateStr;
                        //frmImporterDialog.Caption = this.currentParish;
                        //frmImporterDialog.Notes = this.currentFileContents;
                        //frmImporterDialog.ShowDialog();
                        //this.isCancelled = frmImporterDialog.IsCancelled;

                        //retVal = utilconvertDateStr(frmImporterDialog.Result, out outDate);
                    }
                }
            }


            return retVal;
        }

        /// <summary>
        /// returns all instances of a pattern in a string, doesnt delete anything!
        /// </summary>
        /// <param name="marriageContent"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private string utilCheckForAndReturn(string marriageContent, string pattern)
        {
            string retVal = "";
            marriageContent = marriageContent.Trim();
            Regex regEx = new Regex(pattern, RegexOptions.Singleline);

            Match _match = regEx.Match(marriageContent);

            if (_match.Success) retVal = _match.Value;

            return retVal;
        }
        /// <summary>
        /// check for the existence of a pattern and if we find it then return true and
        /// then delete all instancse of that pattern
        /// </summary>
        /// <param name="marriageContent"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private bool utilCheckForAndRemove(ref string marriageContent, string pattern)
        {
            bool isFound = false;

            marriageContent = marriageContent.Trim();
            Regex regEx = new Regex(pattern, RegexOptions.Singleline);

            Match _match = regEx.Match(marriageContent);

            if (_match.Success)
            {
                isFound = true;
                marriageContent = Regex.Replace(marriageContent, pattern, "", RegexOptions.Singleline);
            }

            return isFound;
        }




        #endregion


    }
}
