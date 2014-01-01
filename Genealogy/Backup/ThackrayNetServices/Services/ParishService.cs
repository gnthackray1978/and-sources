using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Script.Serialization;
using ANDServices;
using TDBCore;
using TDBCore.ControlObjects;
using TDBCore.EntityModel;
using TDBCore.Interfaces;
using TDBCore.Types;
using TDBCore.BLL;
using TDBCore.ModelObjects;
using System.Web.Security;
using System.ServiceModel.Web;
using System.Diagnostics;
using System.Reflection;
using Facebook;
using PersonService = PersonService.PersonService;

namespace ParishService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ParishService : IParishService
    {

        // parishs
        public ServiceParishObject GetParishs(string deposited, string name, string county, string page_number, string page_size, string sort_col)
        {
            ServiceParishObject serviceParishObject = new ServiceParishObject();


            ParishsFilterModel iModel = new ParishsFilterModel();
            ParishsFilterControl iControl = new ParishsFilterControl();
            string retVal = "";
            try
            {

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetParishDeposited(deposited);
                iControl.RequestSetParishCounty(county);
                iControl.RequestSetParishName(name);

                iControl.RequestRefresh();

                if (iModel.ParishEntities != null)
                {

                    serviceParishObject.serviceParishs = iModel.ParishEntities.OrderBy(sort_col).Select(p => new ServiceParish()
                    {
                        ParishId = p.ParishId,
                        ParishName = p.ParishName,
                        ParishDeposited = p.ParishRegistersDeposited,
                        ParishParent = p.ParentParish,
                        ParishStartYear = p.ParishStartYear.GetValueOrDefault(),
                        ParishEndYear = p.ParishEndYear.GetValueOrDefault(),
                        ParishCounty = p.ParishCounty


                    }).ToList();



                    serviceParishObject.Batch = page_number.ToInt32();
                    serviceParishObject.BatchLength = page_size.ToInt32();
                    serviceParishObject.Total = serviceParishObject.serviceParishs.Count;

                    serviceParishObject.serviceParishs = serviceParishObject.serviceParishs.Skip(page_number.ToInt32() * page_size.ToInt32()).Take(page_size.ToInt32()).ToList();

                }
                else
                {
                    serviceParishObject.Batch = page_number.ToInt32();
                    serviceParishObject.BatchLength = page_size.ToInt32();
                    serviceParishObject.Total = 0;
                    serviceParishObject.serviceParishs = new List<ServiceParish>();
                }
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
                serviceParishObject.ErrorStatus = retVal;
            }

            return serviceParishObject;
        }

        public string DeleteParishs(string parishIds)
        {

            ParishsFilterModel iModel = new ParishsFilterModel();
            ParishsFilterControl iControl = new ParishsFilterControl();
            string retVal = "";

            try
            {

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                List<Guid> selection = new List<Guid>();


                string[] persons = parishIds.Split(',');

                foreach (string _person in persons)
                {
                    selection.Add(_person.ToGuid());
                }

                iControl.RequestSetSelectedIds(selection);

                iControl.RequestDelete();
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
            }

            return WebHelper.MakeReturn(iModel.SelectedRecordId.ToString(), retVal);
        }

        public List<ServiceSuperParish> GetParishsFromLocations(string parishLocation)
        {
            ParishsBll parishsBll = new ParishsBll();
            List<ServiceSuperParish> parishs = parishsBll.GetParishsByLocationString(parishLocation).Select(o => new ServiceSuperParish()
            {
                ParishCounty = o.County,
                ParishDeposited = o.Deposited,
                ParishGroupRef = o.groupRef,
                ParishId = o.ParishId,
                ParishLocationCount = o.LocationCount,
                ParishLocationOrder = o.LocationOrder,
                ParishName = o.Name,
                ParishX = o.ParishX,
                ParishY = o.ParishY
            }).ToList();

            foreach (var _parish in parishs.Where(p => p.ParishName == "Cawthorne"))
            {
                Debug.WriteLine(_parish.ParishX + " " + _parish.ParishY);
            }

            return parishs;
        }

        public List<ServiceParishDataType> GetParishTypes()
        {
            ParishsBll parishsBll = new ParishsBll();

            return parishsBll.GetParishTypes().Select(o => new ServiceParishDataType() { DataTypeId = o.dataTypeId, Description = o.description }).ToList();
        }

        public ServiceParishDetailObject GetParishDetail(string parishId)
        {
            ParishsBll parishsBll = new ParishsBll();

            ParishCollection pcoll = parishsBll.GetParishDetail(parishId.ToGuid());
            ServiceParishDetailObject serviceParishDetailObject = new ServiceParishDetailObject();
            //  pcoll.parishDataTypes.AddRange(parishsBll.GetParishTypes());

            //   pcoll.sourceRecords = parishsBll.GetParishSourceRecords(_parish);
            string retVal = "";

            try
            {


                serviceParishDetailObject.serviceParishRecords = pcoll.parishRecords.Select(o => new ServiceParishRecord()
                {
                    DataType = o.dataType,
                    EndYear = o.endYear,
                    ParishId = o.parishId,
                    ParishRecordType = o.parishRecordType,
                    StartYear = o.startYear
                }).ToList();

                serviceParishDetailObject.serviceParishTranscripts = pcoll.parishTranscripts.Select(o => new ServiceParishTranscript()
                {
                    ParishId = o.ParishId,
                    ParishTranscriptRecord = o.ParishTranscriptRecord
                }).ToList();

                //pcoll.sourceRecords = parishsBll.GetParishSourceRecords(_parish);

                serviceParishDetailObject.serviceServiceMapDisplaySource = parishsBll.GetParishSourceRecords(parishId.ToGuid()).Select(s => new ServiceMapDisplaySource()
                {
                    DisplayOrder = s.DisplayOrder,
                    IsCopyHeld = s.IsCopyHeld,
                    IsThackrayFound = s.IsThackrayFound,
                    IsViewed = s.IsViewed,
                    OriginalLocation = s.OriginalLocation,
                    SourceDesc = s.SourceDesc,
                    SourceId = s.SourceId,
                    SourceRef = s.SourceRef,
                    YearEnd = s.YearEnd,
                    YearStart = s.YearStart
                }).ToList();

                string sourceStr = "";

                serviceParishDetailObject.serviceServiceMapDisplaySource.ForEach(p => sourceStr += "," + p.SourceId.ToString());


                //foreach (var record in serviceParishDetailObject.serviceServiceSourceRecord)
                //{
                //    sourceStr += "," + record.SourceId;
                //}

                sourceStr = sourceStr.Substring(1);


                serviceParishDetailObject.PersonCount = GetPersonsCount("", "", "thac", "", "", "", "", "", "", "1400", "1950", "false", "false", "false", sourceStr, "", "");

                serviceParishDetailObject.MarriageCount = GetMarriagesCount("", "", "", "", "", "", "0", "0", sourceStr);
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;

                serviceParishDetailObject.ErrorStatus = retVal;
            }


            return serviceParishDetailObject;
        }

        private int GetPersonsCount(string parentId, string christianName, string surname, string fatherChristianName,
                string fatherSurname, string motherChristianName, string motherSurname, string location, string county,
                string lowerDate, string upperDate, string filterTreeResults, string filterIncludeBirths,
                string filterIncludeDeaths, string filterSource, string spouse,
                string parishFilter)
        {

            ServicePersonObject spo = new ServicePersonObject();

            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();
            string retVal = "";

            try
            {
                iControl.SetModel(iModel);
                iControl.RequestSetUser(WebHelper.GetUser());

                if (parentId.ToGuid() == Guid.Empty)
                {


                    iControl.RequestSetFilterMode(DeathBirthFilterTypes.Simple);
                    iControl.RequestSetSelectedIds(new List<Guid>());


                    PersonSearchFilter personSearchFilter = new PersonSearchFilter()
                    {
                        CName = christianName,
                        Surname = surname,
                        FatherChristianName = fatherChristianName,
                        FatherSurname = fatherSurname,
                        MotherChristianName = motherChristianName,
                        MotherSurname = motherSurname,
                        Location = location,
                        LowerDate = lowerDate.ToInt32(),
                        UpperDate = upperDate.ToInt32(),
                        IsIncludeBirths = filterIncludeBirths.ToBool(),
                        IsIncludeDeaths = filterIncludeDeaths.ToBool(),
                        SpouseChristianName = spouse,
                        SourceString = filterSource,
                        ParishString = parishFilter
                    };

                    iControl.RequestSetSearchFilter(personSearchFilter);



                }
                else
                {
                    iControl.RequestSetSelectedIds(new List<Guid>());
                    iControl.RequestSetParentRecordIds(parentId.ToGuid());

                    iControl.RequestSetFilterMode(DeathBirthFilterTypes.Duplicates);


                }


                iControl.RequestRefresh();
            }
            catch (Exception ex1)
            {
                // swallow error this isnt very important
                retVal = ex1.Message;
            }


            return iModel.PersonsDataTable.Total;

        }


        private int GetMarriagesCount(string uniqref, string malecname, string malesname, string femalecname, string femalesname,
            string location, string lowerDate, string upperDate, string sourceFilter)
        {
            ServiceMarriageObject serviceMarriageObject = new ServiceMarriageObject();

            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();


            try
            {
                iControl.SetModel(iModel);

                Guid parentId = uniqref.ToGuid();

                if (parentId == Guid.Empty)
                {
                    iControl.RequestSetUser(WebHelper.GetUser());

                    iControl.RequestSetFilterMode(MarriageFilterTypes.STANDARD);

                    iControl.RequestSetSelectedIds(new List<Guid>());

                    iControl.RequestSetFilterMaleName((malecname ?? ""), (malesname ?? ""));

                    iControl.RequestSetFilterFemaleName((femalecname ?? ""), (femalesname ?? ""));

                    iControl.RequestSetFilterLocation(location ?? "");

                    iControl.RequestSetFilterMarriageBoundLower(lowerDate ?? "");
                    iControl.RequestSetFilterMarriageBoundUpper(upperDate ?? "");


                    if (sourceFilter != "")
                    {
                        iControl.RequestSetFilterSource(sourceFilter);

                        List<Guid> sources = new List<Guid>();

                        foreach (string _guid in sourceFilter.Split(','))
                        {
                            sources.Add(_guid.ToGuid());
                        }

                        iControl.RequestSetSourceGuidsList(sources);

                    }

                }
                else
                {
                    iControl.RequestSetSelectedIds(new List<Guid>());
                    iControl.RequestSetParentRecordIds(parentId);
                    //hack
                    iControl.RequestSetFilterSource("dupes");
                    iControl.RequestSetIsDataChanged(true);
                    iControl.RequestSetFilterMode(MarriageFilterTypes.DUPLICATES);

                }

                iControl.RequestRefresh();

            }
            catch (Exception ex1)
            {
                //swallow exception
            }



            return iModel.MarriagesTable.Count;
        }




        public List<ServiceSearchResult> GetSearchResults(string parishIds, string startYear, string endYear)
        {

            SourceBll sourceBll = new SourceBll();
            List<ServiceSearchResult> ssresults = new List<ServiceSearchResult>();

            List<string> parishIdLst = parishIds.Split(',').ToList();

            if (parishIdLst.Count > 0 || parishIds != "YORKSHIRE")
            {
                List<uspGetParishSources_Result> parishGroups = sourceBll.GetSourceByParishString(parishIds, startYear.ToInt32(), endYear.ToInt32());//.GroupBy(p=>p.SourceMappingParishId)


                //foreach (var parishGroup in parishGroups)
                //{ 

                //}

                //sort me out!!


                foreach (var parish in parishGroups)
                {

                    ServiceSearchResult ssr = new ServiceSearchResult();
                    ssr.ParishId = parish.SourceMappingParishId.GetValueOrDefault();

                    List<uspGetParishSources_Result> parishResult = parishGroups.Where(ps => ps.SourceMappingParishId == ssr.ParishId).ToList();

                    if (parishResult.Count > 0)
                    {

                        if (parishResult.Exists(pr => pr.MapTypeId == 43))
                        {
                            ssr.IsMarriage = true;
                            ssr.IsBaptism = true;
                            ssr.IsBurial = true;
                        }
                        else
                        {
                            ssr.IsMarriage = parishResult.Exists(pr => pr.MapTypeId == 40);
                            //baptisms
                            ssr.IsBaptism = parishResult.Exists(pr => pr.MapTypeId == 41);
                            //burials
                            ssr.IsBurial = parishResult.Exists(pr => pr.MapTypeId == 42);
                        }
                    }


                    ssresults.Add(ssr);

                }
            }


            return ssresults;

        }

        public List<ServiceParishCounter> GetParishCounters(string startYear, string endYear)
        {
            ParishsBll parishsBll = new ParishsBll();


            return parishsBll.GetParishCounter().Where(p => p.YearStart >= startYear.ToInt32() && p.YearEnd <= endYear.ToInt32()).
                                                Select(o => new ServiceParishCounter()
                                                {
                                                    Counter = o.Count.GetValueOrDefault(),
                                                    EndYear = o.YearEnd.GetValueOrDefault(),
                                                    ParishId = o.ParishId.GetValueOrDefault(),
                                                    ParishName = o.ParishName,
                                                    PX = o.PX.GetValueOrDefault(),
                                                    PY = o.PY.GetValueOrDefault()
                                                }).ToList();
        }

        public List<string> GetParishNames(string parishIds)
        {
            ParishsBll parishsBll = new ParishsBll();

            List<Guid> parishIdLst = new List<Guid>();

            parishIds.Split(',').ToList().ForEach(p => parishIdLst.Add(p.ToGuid()));

            return parishsBll.GetParishs2().Where(p => parishIdLst.Contains(p.ParishId)).Select(s => s.ParishName).ToList();
        }


        public string AddParish(string ParishId, string ParishStartYear, string ParishEndYear,
                                string ParishLat, string ParishLong,
                                string ParishName, string ParishParent,
                                string ParishNote, string ParishCounty, string ParishDeposited)
        {

            WebHelper.WriteParams(ParishId, ParishStartYear, ParishEndYear,
                                  ParishLat, ParishLong,
                                  ParishName, ParishParent,
                                  ParishNote, ParishCounty, ParishDeposited);
            string retVal = "";

            ParishsEditorModel iModel = new ParishsEditorModel();
            ParishEditorControl iControl = new ParishEditorControl();

            try
            {
                if (iModel != null)
                    iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestSetSelectedId(ParishId.ToGuid());
                iControl.RequestSetParishEndYear(ParishEndYear);
                iControl.RequestSetParishStartYear(ParishStartYear);
                iControl.RequestSetParishLat(ParishLat);
                iControl.RequestSetParishLong(ParishLong);
                iControl.RequestSetParishName(ParishName);
                iControl.RequestSetParishParent(ParishParent);
                iControl.RequestSetParishRegisterNotes(ParishNote);
                iControl.RequestSetParishRegistersCounty(ParishCounty);
                iControl.RequestSetParishRegistersDeposited(ParishDeposited);
                iControl.RequestUpdate();
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
            }

            return WebHelper.MakeReturn(iModel.SelectedRecordId.ToString(), retVal);
        }

        public ServiceParish GetParish(string parishId)
        {
            ParishsEditorModel iModel = new ParishsEditorModel();
            ParishEditorControl iControl = new ParishEditorControl();


            ServiceParish parish = new ServiceParish();

            string retVal = "";
            try
            {

                iControl.SetModel(iModel);

                iControl.RequestSetSelectedId(parishId.ToGuid());
                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestRefresh();

                if (iModel.IsValidEntry)
                {
                    parish = new ServiceParish()
                    {
                        ParishCounty = iModel.ParishRegistersCounty,
                        ParishDeposited = iModel.ParishRegistersDeposited,
                        ParishEndYear = iModel.ParishEndYear.ToInt32(),
                        ParishId = parishId.ToGuid(),
                        ParishLat = Convert.ToDouble(iModel.ParishLat),
                        ParishLong = Convert.ToDouble(iModel.ParishLong),
                        ParishName = iModel.ParishName,
                        ParishNote = iModel.ParishRegisterNotes,
                        ParishParent = iModel.ParishParent,
                        ParishStartYear = iModel.ParishStartYear.ToInt32()
                    };
                }
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
                parish.ErrorStatus = retVal;
            }
            return parish;


        }




    }
}