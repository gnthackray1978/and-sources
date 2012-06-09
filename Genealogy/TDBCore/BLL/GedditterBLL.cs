using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gedcom;
//using GedcomDatasets;
using System.Text.RegularExpressions;

using System.Reflection;
using System.ComponentModel;
//using TDBCore.Datasets;
using System.Diagnostics;


namespace GedcomBR
{
    public class GedditterBLL
    {
        private GedcomDatabase _database;

        public static int OLDESTMARRIAGEAGE_MALE = 50;
        public static int OLDESTMARRIAGEAGE_FEMALE = 30;
        public static int YOUNGESTMARRIAGEAGE_MALE = 18;
        public static int YOUNGESTMARRIAGEAGE_FEMALE = 16;
        public static int DEATHAGE_MALE = 90;
        public static int DEATHAGE_FEMALE = 90;


        public GedditterBLL(GedcomDatabase database)
        {
            this._database = database;
        }


        public void GetPedigree(string _ref)
        {
         //   
            GedcomIndividualRecord gInd = (GedcomIndividualRecord)this._database[_ref];
            GedcomIndividualRecord topDog = (GedcomIndividualRecord)this._database[_ref];

            while (topDog.ChildIn.Count > 0)
            {
                GedcomFamilyRecord gfr = (GedcomFamilyRecord)this._database[topDog.ChildIn[0].Family];


                topDog = (GedcomIndividualRecord)this._database[gfr.Husband];
            }

            //MessageBox.Show(topDog.Names[0].Name);


            GedcomFamilyRecord gfr1 = (GedcomFamilyRecord)this._database[topDog.ChildIn[0].Family];

            foreach (string _childStr in gfr1.Children)
            {
                topDog = (GedcomIndividualRecord)this._database[_childStr];

                
            }


            //gInd.ChildIn[0].Family
                
          //  System.Xml.XmlDocument xdox = new System.Xml.XmlDocument();
            
            //System.Xml.XmlNode xmlNode = null;

            //gInd.GenerateXML(xmlNode);

            //GedcomFamilyRecord gfr = gInd.GetFamily();

            //if (gInd.ChildIn.Count > 0)
            //{ 
            
            //}

            // go up the childin properties until there is no father

            // then work our way down and outwards. 









           //PH.DataTree.DTreeBuilder<string> dTreeBuilder = new PH.DataTree.DTreeBuilder<string>();

           //dTreeBuilder.Add("bob");

            //dTreeBuilder.ToTree().Nodes[0].

         //  DTreeBuilder dtBuilder;
            
            //gInd.ChildInFamily(

            
        }

     

        #region update event
        public void UpdateMarriageEvent(Gedcom.GedcomFamilyEvent gEvent,
                                                            GedcomIndividualRecord selectedSpouse,
                                                            List<GedcomIndividualRecord> marriagechildren,
                                                            string indiRec,
                                                            string place,
                                                            Gedcom.GedcomDate date,
                                                            string notes)
        {

            // ok we have the children here
            // remove the children from whatever marriage they are currently attached to
            // then reattach them later.

            int childidx =0;
            foreach (GedcomIndividualRecord _child in marriagechildren)
            {
                childidx = 0;
                while (childidx < _child.ChildIn.Count)
                {
                    GedcomFamilyRecord gfl = (GedcomFamilyRecord)this._database[_child.ChildIn[childidx].Family];

                    gfl.RemoveChild(_child);
                    childidx++;
                }
                //gfl.Children[
            }


            GedcomIndividualRecord gindRec = (GedcomIndividualRecord)this._database[indiRec];

            // do we want to merge this persons existing children into this marriage/

          

            // prompt user which children does the user want to move over to the new relationship


            if (gindRec.Sex == GedcomSex.Male)
            {
                gEvent.FamRecord.ChangeWife(selectedSpouse);
            }
            else
            {
                gEvent.FamRecord.ChangeHusband(selectedSpouse);
            }
  
            // now add children
            childidx = 0;
            while (childidx < marriagechildren.Count)
            {

                gEvent.FamRecord.AddChild(marriagechildren[childidx]);
                childidx++;
            }


            if (gEvent.Place != null)
            {
                gEvent.Place.Name = place;
            }
            else
            {
                gEvent.Place = new GedcomPlace();
                gEvent.Place.Database = this._database;
                gEvent.Place.Name = place;
                gEvent.Place.Level = 1;
                gEvent.Place.ParsingLevel = 1;
            }

            if (gEvent.Date == null)
            {
                gEvent.Date = new GedcomDate(this._database);

                gEvent.Date.Level = 1;
                gEvent.Date.ParsingLevel = 1;
            }
            

            date.Level = gEvent.Date.Level;
            date.ParsingLevel = gEvent.Date.ParsingLevel;

            gEvent.Date = date;


            if (gEvent.Notes.Count > 0)
            {
                gEvent.Notes[0] = notes;
            }
            else
            {
                if (notes != "")
                    gEvent.Notes.Add(notes);
            }

        }



        #endregion


        public void AddMarriageEvent(GedcomIndividualRecord selectedSpouse,
                                                    List<GedcomIndividualRecord> marriagechildren,
                                                    string indiRec,
                                                    string place,
                                                    Gedcom.GedcomDate date,
                                                    string notes)
        {

            // ok we have the children here
            // remove the children from whatever marriage they are currently attached to
            // then reattach them later.
            #region remove children from their families
            int childidx = 0;
            foreach (GedcomIndividualRecord _child in marriagechildren)
            {
                childidx = 0;
                while (childidx < _child.ChildIn.Count)
                {
                    GedcomFamilyRecord gfl = (GedcomFamilyRecord)this._database[_child.ChildIn[childidx].Family];

                    gfl.RemoveChild(_child);
                    childidx++;
                }

            }
            #endregion


            GedcomIndividualRecord gindRec = (GedcomIndividualRecord)this._database[indiRec];


            GedcomFamilyRecord newFamRec = new GedcomFamilyRecord(this._database, gindRec, selectedSpouse);


            newFamRec.ParsingLevel = 1;
            newFamRec.Level = 1;

            // now add children
            childidx = 0;
            while (childidx < marriagechildren.Count)
            {
                newFamRec.AddChild(marriagechildren[childidx]);
                childidx++;
            }


            GedcomFamilyEvent gfe = newFamRec.AddNewEvent(GedcomEvent.GedcomEventType.MARR);
            gfe.Level = 1;
            gfe.ParsingLevel = 1;

            gfe.Place = new GedcomPlace();
            gfe.Place.Database = this._database;
            gfe.Place.Name = place;
            gfe.Place.ParsingLevel = 1;
            gfe.Place.Level = 1;

            if (date != null)
            {
                date.Level = 1;
                date.ParsingLevel = 1;

                gfe.Date = date;
            }
            else
            {
                gfe.Date = new GedcomDate();
                gfe.Database = this._database;
                gfe.Date.Level = 1;

                gfe.ParsingLevel = 1;
            }

            if (notes != "")
                gfe.Notes.Add(notes);
            

        }



        #region update event
        public void UpdateEvent(Gedcom.GedcomIndividualEvent gEvent, 
                                                            string place, 
                                                            Gedcom.GedcomDate date, 
                                                            string notes)
        {

            if (gEvent.Place != null)
            {
                gEvent.Place.Name = place;
            }
            else
            {
                gEvent.Place = new GedcomPlace();
                gEvent.Place.Database = this._database;

            }

            date.Level = gEvent.Date.Level;
            date.ParsingLevel = gEvent.Date.ParsingLevel;

            gEvent.Date = date;

           
                if (gEvent.Notes.Count > 0)
                {
                    gEvent.Notes[0] = notes;
                }
                else
                {
                    if(notes != "")
                        gEvent.Notes.Add(notes);
                }

        }



        #endregion

        #region add event
        public void AddEvent(Gedcom.GedcomIndividualRecord gRec, Gedcom.GedcomEvent.GedcomEventType gRecType,
                                                          string place,
                                                          Gedcom.GedcomDate date,
                                                          string notes
            )
        {


            GedcomIndividualEvent gEvent = new GedcomIndividualEvent();

            gEvent.Database = this._database;
            gEvent.EventType = gRecType;
            gEvent.Level = 1;
            gEvent.ParsingLevel = 1;
 
            gEvent.Place = new GedcomPlace();
            gEvent.Place.Database = this._database;
            gEvent.Place.Level = 2;
            gEvent.Place.ParsingLevel = 2;
            gEvent.Place.Name = place;

            

            gEvent.Date = date;

            gEvent.Date.Level = 2;
            gEvent.Date.ParsingLevel = 2;


            if (gEvent.Notes.Count > 0)
            {
                gEvent.Notes[0] = notes;
            }
            else
            {
                if (notes != "")
                    gEvent.Notes.Add(notes);
            }

            gRec.Events.Add(gEvent);


        }

        #endregion

        #region add unrelated person
        public void AddUnrelatedPerson(Gedcom.GedcomDatabase gDatabase,
            string name,
            Gedcom.GedcomDate dob,
            string dobLocation,
            Gedcom.GedcomDate dod,
            string dodLocation,
            Gedcom.GedcomDate bapDate,
            string bapLocation,
            Gedcom.GedcomDate occDate,
            string occLocation,
            string occDesc,
            Gedcom.GedcomDate resDate,
            string resLocation,
            bool isMale)
        {

            try
            {
                Gedcom.GedcomIndividualEvent gBirthRec = null;
                if (gDatabase == null) return;

                Gedcom.GedcomIndividualRecord gIndividual = new GedcomIndividualRecord(gDatabase);



                #region date based events

                if (resDate != null)
                {
                    gBirthRec = new GedcomIndividualEvent();
                    genNewEvent(gDatabase, resDate, resLocation, gBirthRec);
                    gBirthRec.EventType = GedcomEvent.GedcomEventType.RESI;
                    gIndividual.Events.Add(gBirthRec);
                }

                if (occDate != null)
                {
                    gBirthRec = new GedcomIndividualEvent();
                    genNewEvent(gDatabase, occDate, occLocation, gBirthRec);

                    gBirthRec.Notes.Add(occDesc);
                    gBirthRec.EventType = GedcomEvent.GedcomEventType.OCCUFact;
                    gIndividual.Events.Add(gBirthRec);
                }

                if (bapDate != null)
                {
                    gBirthRec = new GedcomIndividualEvent();
                    genNewEvent(gDatabase, bapDate, bapLocation, gBirthRec);
                    gBirthRec.EventType = GedcomEvent.GedcomEventType.BAPM;
                    gIndividual.Events.Add(gBirthRec);
                }

                if (dod != null)
                {
                    gBirthRec = new GedcomIndividualEvent();
                    genNewEvent(gDatabase, dod, dodLocation, gBirthRec);
                    gBirthRec.EventType = GedcomEvent.GedcomEventType.DEAT;
                    gIndividual.Events.Add(gBirthRec);
                }


                if (dob != null)
                {
                    gBirthRec = new GedcomIndividualEvent();
                    genNewEvent(gDatabase, dob, dobLocation, gBirthRec);
                    gBirthRec.EventType = GedcomEvent.GedcomEventType.BIRT;
                    gIndividual.Events.Add(gBirthRec);
                }

                #endregion


                Gedcom.GedcomName _gName = new GedcomName();
                _gName.Database = gDatabase;
                _gName.Level = 1;
                _gName.ParsingLevel = 1;

                FitNameInGedcomNameField(_gName, name);



                if (gIndividual.Names.Count > 0)
                    gIndividual.Names[0] = _gName;
                else
                    gIndividual.Names.Add(_gName);

          //      gIndividual.Database = gDatabase;
                gIndividual.Level = 1;
                gIndividual.ParsingLevel = 1;

                if (isMale)
                    gIndividual.Sex = GedcomSex.Male;
                else
                    gIndividual.Sex = GedcomSex.Female;
                
             //   gDatabase.Individuals.Add(gIndividual);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message);
                //MessageBox.Show(ex1.Message);
            }
        }

        #endregion

        #region static methods


        public static void FitNameInGedcomNameField(Gedcom.GedcomName _gname, string nameStr)
        {
            nameStr = nameStr.Trim().Replace("  ", " ");
            nameStr = nameStr.Replace("   ", " ");

            string[] nameParts = nameStr.Split(' ');


            //_gname.Given = 
            int idx = 0;
            string forenamePart = "";
            while (idx < nameParts.Length - 1)
            {
                forenamePart += nameParts[idx];
                idx++;
            }

            _gname.Given = forenamePart;

            _gname.Surname = nameParts[nameParts.Length - 1];
        }

        private static void genNewEvent(Gedcom.GedcomDatabase gDatabase, Gedcom.GedcomDate dob, string dobLocation, Gedcom.GedcomIndividualEvent gBirthRec)
        {
            gBirthRec.Database = gDatabase;
            gBirthRec.Level = 1;
            gBirthRec.ParsingLevel = 1;
            gBirthRec.Date = dob;
            gBirthRec.Place = genNewPlace(gDatabase, dobLocation);
        }

        private static GedcomPlace genNewPlace(Gedcom.GedcomDatabase gDatabase, string dobLocation)
        {
            Gedcom.GedcomPlace gPlace = new GedcomPlace();
            gPlace.Level = 1;
            gPlace.ParsingLevel = 1;
            gPlace.Database = gDatabase;
            gPlace.Name = dobLocation;

            return gPlace;
        }


        //public static bool validateDate(TextBox tb, out GedcomDate gedcomDate, ErrorProvider epDate)
        //{
        //    bool _validEventDate = false;
        //    GedcomDate.GedcomDatePeriod _eventDatePeriod;
        //    DateTime _eventDate = new DateTime(1, 1, 1);
        //    string outputdate = "";

        //    _eventDatePeriod = GedcomDate.GedcomDatePeriod.Exact;

        //    GedditterBLL.GetDateInfo(out _eventDatePeriod, out outputdate, tb.Text);

        //    int year = 0;

        //    if (outputdate.Length == 4)
        //    {
        //        if (Int32.TryParse(outputdate, out year))
        //        {
        //            _eventDate = new DateTime(year, 1, 1);
        //            outputdate = _eventDate.ToShortDateString();


        //        }

        //    }

        //    if (DateTime.TryParse(outputdate, out _eventDate))
        //    {
        //        _validEventDate = true;
        //        epDate.SetError(tb, "");
        //    }
        //    else
        //    {
        //        _validEventDate = false;
        //        epDate.SetError(tb, "Invalid Date");
        //    }



        //    string dateString = "";
        //    #region make period string
        //    switch (_eventDatePeriod)
        //    {
        //        //case GedcomDate.GedcomDatePeriod.Exact:
        //        //    dateString = "Ex. ";
        //        //    break;
        //        case GedcomDate.GedcomDatePeriod.After:
        //            dateString = "AFT. ";
        //            break;
        //        case GedcomDate.GedcomDatePeriod.Before:
        //            dateString = "BEF. ";
        //            break;
        //        //case GedcomDate.GedcomDatePeriod.Between:
        //        //    dateString = "Bet. ";
        //        //    break;
        //        //case GedcomDate.GedcomDatePeriod.About:
        //        //    dateString = "Abt. ";
        //        //    break;
        //        //case GedcomDate.GedcomDatePeriod.Calculated:
        //        //    dateString = "Cal. ";
        //        //    break;
        //        case GedcomDate.GedcomDatePeriod.Estimate:
        //            dateString = "EST. ";
        //            break;
        //        //case GedcomDate.GedcomDatePeriod.Interpretation:
        //        //    dateString = "Int. ";
        //        //    break;
        //        //case GedcomDate.GedcomDatePeriod.Range:
        //        //    dateString = "Rng. ";
        //        //    break;
        //        //default:
        //        //    break;
        //    }

        //    #endregion

        //    gedcomDate = new GedcomDate();

        //    gedcomDate.Level = 1;
        //    gedcomDate.ParsingLevel = 1;
        //    if (_eventDate.Month == 1 && _eventDate.Day == 1)
        //    {
        //        dateString = dateString + _eventDate.Year.ToString();
        //        // this._currentGedDate.Time = this._eventDate.Year.ToString();
        //    }
        //    else
        //    {
        //        dateString = dateString + _eventDate.ToString("d MMM yyyy");
        //        // this._currentGedDate.Time = this._eventDate.ToString("d MMM yyyy");
        //    }



        //    gedcomDate.ParseDateString(dateString);

        //    return _validEventDate;
        //}




        #region get marriage dates
        public static void GetMarriageDates(GedcomIndividualRecord gindRec, 
            out DateTime mLower, out DateTime mUpper, 
            out DateTime bLower, out DateTime bUpper,
            out DateTime dLower, out DateTime dUpper,
            out DateTime youngestchildAge,
            out DateTime oldestChildAge,
            out string marriageLocation,
            out string birthLocation,
            out string childData,
            out string spouseCName1,
            out string spouseCName2,
            out string spouseCName3,
            out string spouseSName1,
            out string spouseSName2,
            out string spouseSName3,   
  
            out string spouseRef1,
            out string spouseRef2,
            out string spouseRef3
                    )
        {
            mLower = new DateTime(1, 1, 1);
            mUpper = new DateTime(1, 1, 1);
            bLower = new DateTime(1, 1, 1);
            bUpper = new DateTime(1, 1, 1);
            dLower = new DateTime(1, 1, 1);
            dUpper = new DateTime(1, 1, 1);
            youngestchildAge = new DateTime(1, 1, 1);
            oldestChildAge = new DateTime(1, 1, 1);
            spouseCName1="";
            spouseCName2="";
            spouseCName3="";
            spouseSName1="";
            spouseSName2="";
            spouseSName3="";  
            spouseRef1="";
            spouseRef2="";
            spouseRef3 = "";
            marriageLocation = "";
            birthLocation = "";
            childData = "";
            //gindRec.Birth.Date.DatePeriod == GedcomDate.GedcomDatePeriod.
            //DateTime workingdateTime = GetDateFromGedDate(gindRec.mar);
            GedcomFamilyEvent firstMarrEvt = null;
            

            //gindRec.Birth.Date.DatePeriod == GedcomDate.GedcomDatePeriod.

            if (gindRec.Birth == null) return;
            DateTime workingdateTime = GetDateFromGedDate(gindRec.Birth.Date);

            #region if we have estimate or exact date etc - we'll take that as being real for arguments sake and return it

            if (gindRec.Birth.Date == null) return;
            
            switch (gindRec.Birth.Date.DatePeriod)
            {
                case GedcomDate.GedcomDatePeriod.Range:
                case GedcomDate.GedcomDatePeriod.Interpretation:
                case GedcomDate.GedcomDatePeriod.Exact:
                case GedcomDate.GedcomDatePeriod.Calculated:
                case GedcomDate.GedcomDatePeriod.About:
                case GedcomDate.GedcomDatePeriod.Estimate:
                    bLower = workingdateTime;
                    bUpper = workingdateTime;
                    break;
            }

            GedcomIndividualEvent christeningEvt = null;

                
            christeningEvt = gindRec.FindEvent(GedcomEvent.GedcomEventType.CHR);


            if (christeningEvt == null)
            {
                christeningEvt = gindRec.FindEvent(GedcomEvent.GedcomEventType.BAPM);
            }

            if (christeningEvt != null)
            {
                workingdateTime = GetDateFromGedDate(christeningEvt.Date);
                birthLocation = christeningEvt.Place.Name;
                bLower = workingdateTime;
                bUpper = workingdateTime;

            }

            #endregion
       


            // look for marriage
            #region find first marriage if existing




            GedcomFamilyRecord gedFamRec0 = gindRec.GetFamily();

            if (gedFamRec0 != null)
            {
                GedcomRecordList<GedcomFamilyEvent> famEvents = gedFamRec0.Events;

                foreach (GedcomFamilyEvent gfe in famEvents)
                {
                    if (gfe.EventType == GedcomEvent.GedcomEventType.MARR)
                    {
                        if (firstMarrEvt == null)
                        {
                            firstMarrEvt = gfe;
                        }
                        else
                        {
                            if (GetDateFromGedDate(firstMarrEvt.Date) >= GetDateFromGedDate(gfe.Date))
                            {
                                firstMarrEvt = gfe;
                            }
                        }
                    }
                }
            }

            //foreach (GedcomIndividualEvent gfevent in gindRec.Events)
            //{


            //    if (gfevent.EventType == GedcomEvent.GedcomEventType.MARR)
            //    {
            //        if (firstMarrEvt == null)
            //        {
            //            firstMarrEvt = gfevent;
            //        }
            //        else
            //        {
            //            if (GetDateFromGedDate(firstMarrEvt.Date) >= GetDateFromGedDate(gfevent.Date))
            //            {
            //                firstMarrEvt = gfevent;
            //            }
            //        }
            //    }


            //}
            #endregion

            if (firstMarrEvt != null)
            {
                // we found a marriage
                mLower = GetDateFromGedDate(firstMarrEvt.Date);
                mUpper = mLower;

                
                // we havent worked out the birth year yet so try to do that. 
                if (bLower.Year == 1 && bUpper.Year == 1)
                {
                    if (gindRec.Sex == GedcomSex.Female)
                    {
                        bUpper = mLower.AddYears(-(YOUNGESTMARRIAGEAGE_FEMALE));
                        bLower = mLower.AddYears(-(OLDESTMARRIAGEAGE_FEMALE));
                    }
                    if (gindRec.Sex == GedcomSex.Male)
                    {
                        bUpper = mLower.AddYears(-(YOUNGESTMARRIAGEAGE_MALE));
                        bLower = mLower.AddYears(-(OLDESTMARRIAGEAGE_MALE));
                    }
                }


            }
            // cant find a marriage 
            // try to calculate it based on the births of any children
            // also try to calculate the birth from the birth of any children
            else
            {
                // ok we dont have a marriage date or a birth date, but we do have children
                // which can happen quite a lot
                // look for children

                // children seem to be stored as a list of 'xrefs' which then you presumably look up somehow

                // as so i think
                GedcomFamilyRecord gedFamRec = gindRec.GetFamily();
                
                //GedcomIndividualRecord firstChild = null;
                childData = "";

                #region record data based on children

                if (gedFamRec != null && gedFamRec.Children.Count > 0)
                {
                    DateTime oldestChildBirth = new DateTime(1, 1, 1);
                    DateTime youngestChildBirth = new DateTime(1, 1, 1);

                    #region get earliest and latest childbirths

                    foreach (string childRef in gedFamRec.Children)
                    {


                        GedcomIndividualRecord indi = (GedcomIndividualRecord)gindRec.Database[gedFamRec.Children[0]];

                        string dateString = "";
                        if (indi.Birth != null &&indi.Birth.Date != null)
                            dateString = indi.Birth.Date.DateString;

                        childData = childData + indi.Names[0].Given + dateString + ",";
                        if (oldestChildBirth.Year == 1)
                        {
                            if (indi.Birth != null && indi.Birth.Date != null)
                                oldestChildBirth = GetDateFromGedDate(indi.Birth.Date);
                        }
                        else if (GetDateFromGedDate(indi.Birth.Date) <= oldestChildBirth)
                        {
                            if (indi.Birth != null && indi.Birth.Date != null)
                                oldestChildBirth = GetDateFromGedDate(indi.Birth.Date);
                        }


                    }



                    foreach (string childRef in gedFamRec.Children)
                    {
                        GedcomIndividualRecord indi = (GedcomIndividualRecord)gindRec.Database[gedFamRec.Children[0]];

                        if (youngestChildBirth.Year == 1)
                        {
                            if (indi.Birth != null && indi.Birth.Date != null)
                             youngestChildBirth = GetDateFromGedDate(indi.Birth.Date);
                        }
                        else if (GetDateFromGedDate(indi.Birth.Date) >= youngestChildBirth)
                        {
                            if (indi.Birth != null && indi.Birth.Date != null)
                             youngestChildBirth = GetDateFromGedDate(indi.Birth.Date);
                        }
                    }
                    #endregion


                    // have we already recorded the marriage and birth 

                    if (mLower.Year == 1 && mUpper.Year == 1)
                    {
                        mLower = youngestChildBirth;
                        mUpper = oldestChildBirth;

                        if (gindRec.Sex == GedcomSex.Female)
                        {
                            if ((youngestChildBirth.Year - OLDESTMARRIAGEAGE_FEMALE)>0)
                                mLower = youngestChildBirth.AddYears(-(OLDESTMARRIAGEAGE_FEMALE));
                        }
                        if (gindRec.Sex == GedcomSex.Male)
                        {
                            if ((youngestChildBirth.Year - OLDESTMARRIAGEAGE_MALE) > 0)
                                mLower = youngestChildBirth.AddYears(-(OLDESTMARRIAGEAGE_MALE));

                        }

                        // if we dont already have a birth 
                        if (bLower.Year == 1 && bUpper.Year == 1)
                        {
                            // we cant base the birth on the marriage dates because they are just guesses themselves!!
                            bUpper = oldestChildBirth.AddYears(-(YOUNGESTMARRIAGEAGE_FEMALE));
                            bLower = oldestChildBirth.AddYears(-(OLDESTMARRIAGEAGE_FEMALE));
                        }
                    }

                    youngestchildAge = youngestChildBirth;
                    oldestChildAge = oldestChildBirth;

                }
                #endregion

            }

            //
            
            // ok if after all this crap we still havent got a birth range, use any default after and before values

            if (bLower.Year == 1 && bUpper.Year == 1)
            {

                switch (gindRec.Birth.Date.DatePeriod)
                {
                    case GedcomDate.GedcomDatePeriod.Before:
                        bUpper = GetDateFromGedDate(gindRec.Birth.Date);
                        bLower = bUpper.AddYears(-75);
                        break;

                    case GedcomDate.GedcomDatePeriod.After:
                        bLower = GetDateFromGedDate(gindRec.Birth.Date);
                        bUpper = bLower.AddYears(75);
                        break;

                }

             //   bUpper = bLower;

                if (gindRec.Birth.Place != null)
                {
                    birthLocation = gindRec.Birth.Place.Name; 
                }
            }


            GetMarriageDetails(gindRec, 0, out spouseCName1, out spouseSName1, marriageLocation, out spouseRef1);
            GetMarriageDetails(gindRec, 1, out spouseCName2, out spouseSName2, marriageLocation, out spouseRef2);
            GetMarriageDetails(gindRec, 2, out spouseCName3, out spouseSName3, marriageLocation, out spouseRef3);

            #region deaths
            if (gindRec.Sex == GedcomSex.Female)
            {
                dLower = bLower.AddYears(DEATHAGE_FEMALE);
                dUpper = bUpper.AddYears(DEATHAGE_FEMALE);
            }
            else
            {
                dLower = bLower.AddYears(DEATHAGE_MALE);
                dUpper = bUpper.AddYears(DEATHAGE_MALE);
            }
            #endregion

        }




        public static void GetMarriageDetails(GedcomIndividualRecord gindRec, int marriageIdx,

            out string givenName, out string surname,  string marriageLocation, out string spouseRef)
        {
            GedcomDatabase gDB = gindRec.Database;
            GedcomRecordList<GedcomFamilyLink> gfl = gindRec.SpouseIn;
            givenName = "";
            surname = "";
            
            spouseRef = "";

            if (gfl.Count <= marriageIdx || gfl.Count ==0) return;


            GedcomFamilyRecord fam = null;
            GedcomIndividualRecord wife = null;


            wife = (GedcomIndividualRecord)gDB[gfl[marriageIdx].Indi];
            fam = gDB[gfl[marriageIdx].Family] as GedcomFamilyRecord;

            string marriageDate = "";


            if (fam.Marriage != null)
            {
                if (fam.Marriage.Date != null)
                {
                    marriageDate = "_" + fam.Marriage.Date.DateString;
                }

                if (fam.Marriage.Place != null)
                {
                    marriageLocation = marriageLocation + "," + fam.Marriage.Place.Name;
                }
            }



            givenName = wife.Names[0].Given;

            surname = wife.Names[0].Surname + marriageDate;


            spouseRef = wife.XRefID;
        }

        #endregion


        #region get residence info
        public static void GetResidences(GedcomIndividualRecord gindRec,
            out DateTime residence1Date,
            out DateTime residence2Date,
            out DateTime residence3Date,
            out string residence1,
            out string residence2,
            out string residence3)
        { 
             residence1Date = new DateTime (1,1,1);
             residence2Date = new DateTime (1,1,1);
             residence3Date = new DateTime (1,1,1);
             residence1= string.Empty;
             residence2= string.Empty;
             residence3 = string.Empty;

             List<GedcomIndividualEvent> gevents = new List<GedcomIndividualEvent>();
             int idx = 0;
             foreach (GedcomIndividualEvent gevent in gindRec.Events)
             {
                 if (gevent.EventType == GedcomEvent.GedcomEventType.RESIFact || gevent.EventType == GedcomEvent.GedcomEventType.RESI)
                 {
                     gevents.Add(gevent);
                     idx++;
                 }

                 if (idx > 2) break;

             }

             if (gevents.Count > 0)
             {
                 if (gevents[0].Date != null && gevents[0].Date.DateTime1.HasValue)
                     residence1Date = gevents[0].Date.DateTime1.Value;
                 if (gevents[0].Place != null)
                    residence1 = gevents[0].Place.Name;
             }

             if (gevents.Count > 1)
             {
                 if (gevents[1].Date != null && gevents[1].Date.DateTime1.HasValue)
                     residence2Date = gevents[1].Date.DateTime1.Value;
                 
                 if (gevents[1].Place != null)
                    residence2 = gevents[1].Place.Name;
             }

             if (gevents.Count > 2)
             {
                 if (gevents[2].Date != null && gevents[2].Date.DateTime1.HasValue)
                     residence3Date = gevents[2].Date.DateTime1.Value;
                 
                 if (gevents[2].Place != null)
                    residence3 = gevents[2].Place.Name;
             }
        }

        #endregion


        #region date functions
        public static void GetDateInfo(out Gedcom.GedcomDate.GedcomDatePeriod period, out string date, string test)
        {
            period = GedcomDate.GedcomDatePeriod.Exact;

            date = "";
            // lets not spend too much time on this eh :P
            MatchCollection mColl = Regex.Matches(test.ToLower(), @"(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|\d).*");
             

            //about before est aft
            test = test.ToLower();
            if (Regex.IsMatch(test,"abt|about")) period = GedcomDate.GedcomDatePeriod.About;
            if (Regex.IsMatch(test, "bef")) period = GedcomDate.GedcomDatePeriod.Before;
            if (Regex.IsMatch(test, "aft")) period = GedcomDate.GedcomDatePeriod.After;
            if (Regex.IsMatch(test, "est")) period = GedcomDate.GedcomDatePeriod.Estimate;
            if (Regex.IsMatch(test, "ex")) period = GedcomDate.GedcomDatePeriod.Exact;

            foreach (Match m in mColl)
            {
                date = m.ToString();
            }




        }

        public static string FormattedDate(GedcomDate gcDate)
        {
            string retVal = "";
            DateTime tp = GetDateFromGedDate(gcDate);

            if (tp.Month == 1 && tp.Day == 1)
            {
                retVal = tp.Year.ToString();
            }
            else
            {
                if (tp.Year != 1)
                {
                    retVal = tp.ToShortDateString();
                }
            }

            return retVal;
        }

        public static DateTime GetDateFromGedDate(GedcomDate gcDate)
        {
            DateTime retVal = new DateTime(1,1,1);
            if (gcDate != null)
            {

                if (gcDate.DateTime1.HasValue)
                {
                    return gcDate.DateTime1.Value;
                }

                if (gcDate.DateTime2.HasValue)
                {
                    return gcDate.DateTime2.Value;
                }


                if (!DateTime.TryParse(gcDate.Date1, out retVal))
                {
                    DateTime.TryParse(gcDate.Date2, out retVal);
                }

            }

            return retVal;
        }

        #endregion

        #endregion
    }

    public class MiscHelper
    {

        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }



    }
}
