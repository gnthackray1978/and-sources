using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using GedItter.BirthDeathRecords.BLL;
using GedItter.BLL;
using GedItter.Tools;
using TDBCore.EntityModel;
using TDBCore.Types;
using TDBCore.BLL;
using GedItter.ModelObjects;
using GedItter.ControlObjects;
using GedItter.BirthDeathRecords;
using GedItter.MarriageRecords;
using TDBCore.ModelObjects;
using System.Web.Security;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Diagnostics;
using System.Reflection;

using System.Web;
using Facebook;
using System.Xml.Linq;
using GedItter.Interfaces;
using GedItter;
 

namespace ANDServices
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class APIServices : IAnd
    {

        //[WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "{userName}?cb={callbackName}")]       
        //[OperationContract]    
        //[JSONPBehavior(callback = "callbackName")]    
        //public string GetDataJSONCallback(string userName, string callbackName)
        //{
        //    return userName + “: I was called by WCF!”;
        //} 

        public APIServices()
        {
            //
            // TODO: Add constructor logic here
            //
        }

    
        public string TestLogin(string testParam)
        {
            string retVal = "could not get login ";
            string token = WebOperationContext.Current.IncomingRequest.Headers["fb"];

            if (token != null && token.Length != 0)
            {

                Facebook.FacebookClient fbc = new FacebookClient(token);

                var me2 = (IDictionary<string, object>)fbc.Get("/me");


                if (me2.ContainsKey("name"))
                {
                    retVal = (string)me2["name"];

                }

                Debug.WriteLine("user id" + WebHelper.GetUser());
            }


            return retVal;
        }

        // tree related methods

        public ServiceSourceObject GetJSONTreeSources(string description, string page_number, string page_size)
        {
            ISourceFilterModel iModel = new SourceFilterModel();
            ISourceFilterControl iControl = new SourceFilterControl();

            iControl.SetModel(iModel);

            iControl.RequestSetUser(WebHelper.GetUser());

            iControl.RequestSetFilterSourceDescription(description ?? "");

            iControl.RequestSetRecordStart(page_number.ToInt32());

            iControl.RequestSetRecordPageSize(page_size.ToInt32());

            iControl.RequestSetFilterIncludeDefaultPerson("true");

            iControl.RequestSetFilterMode(SourceFilterTypes.TREESOURCES);

            iControl.RequestRefresh();

            return iModel.SourcesDataTable;//.ToServiceSourceObject("", page_number.ToInt32(), page_size.ToInt32(),true); 
        }

        public string SetDefaultTreePerson(Guid sourceId, Guid personId)
        {

            string retVal = "";

            IDeathBirthFilterModel iModel = new DeathBirthFilterModel();
            IDeathBirthFilterControl iControl = new DeathBirthFilterControl();
            List<Guid> selection = new List<Guid>();

            try
            {
               
                selection.Add(personId);

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedIds(selection);

                iControl.RequestSetDefaultPersonForTree(sourceId);
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

            return retVal;
        }



        public List<ServicePersonLookUp> GetJSONTreePeople(string sourceId, string start, string end)
        {

            string retVal = "";

            ServicePersonObject spo = new ServicePersonObject();


            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();

            try
            {
                iControl.SetModel(iModel);

                iControl.RequestSetFilterMode(DeathBirthFilterTypes.TREE);


                iControl.RequestSetSelectedIds(new List<Guid>() { sourceId.ToGuid()});


                iControl.RequestSetUser(WebHelper.GetUser());


                iControl.RequestSetFilterLowerBirth(start ?? "");
                iControl.RequestSetFilterUpperBirth(end ?? "");

                iControl.RequestRefresh();


                spo = iModel.PersonsDataTable;
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;

                spo.ErrorStatus = retVal;
            }

            return spo.servicePersons;
        }





        public string SaveNewTree(string sourceId, string fileName, string sourceRef, string sourceDesc, string sourceYear, string sourceYearTo)
        {
            TreeBLL treeBLL = new TreeBLL();

            return treeBLL.SaveNewTree(sourceId, fileName, sourceRef, sourceDesc, sourceYear, sourceYearTo);
        }

        public AncestorModel GetAncTreeDiag(string treeId)
        {
            AncestorModel iModel = new AncestorModel();


            iModel.SetFather(treeId.ToGuid());
            iModel.Refresh();


            return iModel;
        }

        public TreeModel GetTreeDiag(string treeId)
        {
            TreeModel iModel = new TreeModel();


            SourceMappingsBLL sourceMappingsBll = new SourceMappingsBLL();

            var sourceMap = sourceMappingsBll.GetBySourceIdAndMapTypeId2(treeId.ToGuid(), 39).FirstOrDefault();

            Guid fatherId = Guid.Empty;

            if (sourceMap != null)
            {
                if (sourceMap.Person != null && sourceMap.Person.Person_id != null)
                    fatherId = sourceMap.Person.Person_id;

            }


            //new Guid("6B153C9C-DBA2-431B-8AEC-2D540E7D5F5B")

            iModel.SetFather(fatherId);
            iModel.Refresh();

            return iModel;
        }

        public TreeModel GetTreeDiagDefaultPerson(string treeId, string personId)
        {
            TreeModel iModel = new TreeModel();


            //SourceMappingsBLL sourceMappingsBll = new SourceMappingsBLL();

            //var sourceMap = sourceMappingsBll.GetBySourceIdAndMapTypeId2(treeId.ToGuid(), 39).FirstOrDefault();

            //Guid fatherId = Guid.Empty;

            //if (sourceMap != null)
            //{
            //    if (sourceMap.Person != null && sourceMap.Person.Person_id != null)
            //        fatherId = sourceMap.Person.Person_id;

            //}


            //new Guid("6B153C9C-DBA2-431B-8AEC-2D540E7D5F5B")

            iModel.SetFather(personId.ToGuid());
            iModel.Refresh();

            return iModel;
        }


        public bool DeleteTree(string treeId)
        {
            SourceBLL _sources = new SourceBLL();


            return _sources.DeleteTree(treeId.ToGuid());
        }





        // sources methods

        public List<CensusPlace> Get1841CensusPlaces()
        {
            SourceBLL sourceBll = new SourceBLL();
            return sourceBll.Get1841Census();
        }

        public List<CensusSource> Get1841CensusSources(Guid sourceId)
        {
            SourceBLL sourceBll = new SourceBLL();
            return sourceBll.Get1841CensuSources(sourceId);
        }


        public ServiceFullSource GetSource(string sourceId)
        {

            ServiceFullSource ssobj = new ServiceFullSource();

            var iModel = new SourceEditorModel();
            var iControl = new SourceEditorControl();


            iControl.SetModel(iModel);

            iControl.RequestSetSelectedIds(sourceId.ToGuid());

            iControl.RequestSetUser(WebHelper.GetUser());

            iControl.RequestRefresh();

            ssobj = new ServiceFullSource()
            {
                IsCopyHeld = iModel.IsCopyHeld.GetValueOrDefault(),
                IsThackrayFound = iModel.IsThackrayFound.GetValueOrDefault(),
                IsViewed = iModel.IsViewed.GetValueOrDefault(),
                OriginalLocation = iModel.SourceOriginalLocation,
                SourceDesc = iModel.SourceDescription,
                SourceId = iModel.SelectedRecordId,
                SourceRef = iModel.SourceRef,
                SourceNotes = iModel.SourceNotes,
                SourceDateStr = iModel.SourceDateStr,
                SourceDateStrTo = iModel.SourceDateToStr,
                Parishs = iModel.SourceParishs.ParseToCSV(),
                SourceTypes = iModel.SourceTypeIdList.ParseToCSV(),
                FileIds = iModel.SourceFileIds.ParseToCSV(),
                SourceFileCount = iModel.SourceFileCount.ToInt32()

            };

            return ssobj;
        }

        public List<string> GetSourceNames(string sourceIds)
        {

            // the results need to be returned in the same order as the list of 
            // ids used to look them up.

            SourceBLL sourceBll = new SourceBLL();

            List<Guid> sourceIdLst = new List<Guid>();
            List<string> returnList = new List<string>();

            sourceIds.Split(',').ToList().ForEach(p => sourceIdLst.Add(p.ToGuid()));

            List<Source> unsortedList = sourceBll.FillSources().Where(s => sourceIdLst.Contains(s.SourceId)).ToList();

            if (unsortedList.Count > 0)
            {
                foreach (Guid source in sourceIdLst)
                {
                    returnList.Add(unsortedList.First(o => o.SourceId == source).SourceRef);
                }
            }
            return returnList;


        }

        public ServiceSourceObject GetSources(string sourceTypes, string sourceRef, string sourceDesc, string origLoc,
            string dateLB, string toDateLB, string dateUB, string toDateUB, string fileCount, string isThackrayFound,
            string isCopyHeld, string isViewed, string isChecked, string page_number, string page_size, string sortColumn)
        {
            ISourceFilterModel iModel = new SourceFilterModel();
            ISourceFilterControl iControl = new SourceFilterControl();

            iControl.SetModel(iModel);

            iControl.RequestSetUser(WebHelper.GetUser());

            iControl.RequestSetFilterSourceTypeList(SourceFilterModel.GetSourceTypeList(sourceTypes));
            iControl.RequestSetFilterSourceRef(sourceRef);
            iControl.RequestSetFilterSourceDescription(sourceDesc ?? "");
            iControl.RequestSetFilterSourceOriginalLocation(origLoc ?? "");
            iControl.RequestSetFilterSourceDateLowerBound(dateLB ?? "");
            iControl.RequestSetFilterSourceToDateLowerBound(toDateLB ?? "");
            iControl.RequestSetFilterSourceDateUpperBound(dateUB ?? "");
            iControl.RequestSetFilterSourceToDateUpperBound(toDateUB ?? "");
            iControl.RequestSetFilterSourceFileCount((fileCount ?? ""), true);
            iControl.RequestSetFilterIsThackrayFound(isThackrayFound.ToBool(), isChecked.ToBool());
            iControl.RequestSetFilterIsCopyHeld(isCopyHeld.ToBool(), isChecked.ToBool());
            iControl.RequestSetFilterIsViewed(isViewed.ToBool(), isChecked.ToBool());

         
            iControl.RequestSetRecordStart(page_number.ToInt32());
            iControl.RequestSetRecordPageSize(page_size.ToInt32());


            iControl.RequestRefresh();
           
            return iModel.SourcesDataTable;//.ToServiceSourceObject(sortColumn, page_number.ToInt32(), page_size.ToInt32()); 
        }

        public bool DeleteSource(string sourceId)
        {
            List<Guid> sourceIdGuids = new List<Guid>();
            string[] sourceIds = sourceId.Split(',');

            foreach (string _str in sourceIds)
            {
                sourceIdGuids.Add(_str.ToGuid());
            }

            SourceFilterModel iModel = new SourceFilterModel();
            SourceFilterControl iControl = new SourceFilterControl();


            iControl.SetModel(iModel);
            iControl.RequestSetUser(WebHelper.GetUser());

            iControl.RequestSetSelectedIds(sourceIdGuids);

            iControl.RequestDelete();

            return true;
        }

        public string AddSource(string sourceId,
                                   string isCopyHeld,
                                   string isThackrayFound,
                                   string isViewed,
                                   string originalLocation,
                                   string sourceDesc,
                                   string sourceRef,
                                   string sourceNotes,
                                   string sourceDateStr,
                                   string sourceDateStrTo,
                                   string sourceFileCount,
                                   string parishs,
                                   string sourceTypes,
                                   string fileIds)
        {
            string retVal = "";

            var iModel = new SourceEditorModel();
            var iControl = new SourceEditorControl();

            List<Guid> parishsList = new List<Guid>();
            List<int> sourceTypeList = new List<int>();

            if (parishs != null)
                parishs.Split(',').Where(s => s != null).ToList().ForEach(s => parishsList.Add(s.ToGuid()));

            if (sourceTypes != null)
                sourceTypes.Split(',').Where(s => s != null).ToList().ForEach(s => sourceTypeList.Add(s.ToInt32()));

            try
            {
                if (iModel != null)
                    iControl.SetModel(iModel);

                //     iControl.RequestSetUserId(
                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedId(sourceId.ToGuid());
                iControl.RequestRefresh();

                iControl.RequestSetIsCopyHeld(isCopyHeld.ToBool());
                iControl.RequestSetIsThackrayFound(isThackrayFound.ToBool());
                iControl.RequestSetIsViewed(isViewed.ToBool());

                iControl.RequestSetSourceOriginalLocation(originalLocation);

                iControl.RequestSetSourceDescription(sourceDesc);
                iControl.RequestSetSourceRef(sourceRef);

                iControl.RequestSetSourceNotes(sourceNotes);

                iControl.RequestSetSourceDateStr(sourceDateStr);
                iControl.RequestSetSourceDateToStr(sourceDateStrTo);

                iControl.RequestSetSourceFileCount(sourceFileCount);

                iControl.RequestSetSourceParishs(parishsList);

                iControl.RequestSetSourceTypeIdList(sourceTypeList);

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


            return MakeReturn(iModel.SelectedRecordId.ToString(), retVal);

        }



        // source types

        public List<string> GetSourceTypeNames(string typeIds)
        {

            // the results need to be returned in the same order as the list of 
            // ids used to look them up.          
            SourceTypesBLL sourceTypesBll = new SourceTypesBLL();
            List<int> typeIdLst = new List<int>();

            typeIds.Split(',').ToList().ForEach(p => typeIdLst.Add(p.ToInt32()));

            return sourceTypesBll.GetSourceTypesFromIdList(typeIdLst);
        }

        public ServiceSourceType GetSourceType(string TypeId)
        {
            SourceTypeEditorModel iModel = new SourceTypeEditorModel();
            SourceTypeEditorControl iControl = new SourceTypeEditorControl();
            ServiceSourceType ssTO = new ServiceSourceType();

            iControl.SetModel(iModel);

            iControl.RequestSetUser(WebHelper.GetUser());

            iControl.RequestSetSelectedId(TypeId.ToInt32());

            iControl.RequestRefresh();

            if (iModel.IsValidEntry)
            {
                ssTO = new ServiceSourceType() { Description = iModel.SourceTypeDesc, Order = iModel.SourceTypeOrder, TypeId = TypeId.ToInt32() };
            }

            return ssTO;
        }

        public ServiceSourceTypeObject GetSourceTypes(string description, string page_number, string page_size, string sort_col)
        {
            SourceTypeFilterModel iModel = new SourceTypeFilterModel();
            SourceTypeFilterControl iControl = new SourceTypeFilterControl();
            ServiceSourceTypeObject ssTO = new ServiceSourceTypeObject();

            iControl.SetModel(iModel);

            iControl.RequestSetSourceTypesDescrip(description);

            iControl.RequestSetUser(WebHelper.GetUser());

            iControl.RequestRefresh();

            ssTO.serviceSources = iModel.SourceTypesDataTable.Select(s => new ServiceSourceType()
            {
                Description = s.SourceTypeDesc,
                TypeId = s.SourceTypeId,
                Order = s.SourceTypeOrder
            }).ToList();

            ssTO.Batch = page_number.ToInt32();
            ssTO.BatchLength = page_size.ToInt32();
            ssTO.Total = ssTO.serviceSources.Count;

            ssTO.serviceSources = ssTO.serviceSources.Skip(page_number.ToInt32() * page_size.ToInt32()).Take(page_size.ToInt32()).ToList();

            return ssTO;
        }

        public bool DeleteSourceTypes(string sourceIds)
        {
            List<int> sourceTypeIds = new List<int>();

            sourceIds.Split(',').ToList().ForEach(id => sourceTypeIds.Add(id.ToInt32()));

            SourceTypeFilterModel iModel = new SourceTypeFilterModel();
            SourceTypeFilterControl iControl = new SourceTypeFilterControl();


            iControl.SetModel(iModel);
            iControl.RequestSetUser(WebHelper.GetUser());
            iControl.RequestSetSelectedIds(sourceTypeIds);

            iControl.RequestDelete();

            return true;
        }

        public string AddSourceType(string TypeId, string Description, string Order)
        {

            WriteParams(Description, Order);

            string retVal = "";

            SourceTypeEditorModel iModel = new SourceTypeEditorModel();
            SourceTypeEditorControl iControl = new SourceTypeEditorControl();

            try
            {
                if (iModel != null)
                    iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedId(TypeId.ToInt32());
                iControl.RequestRefresh();



                iControl.RequestSetSourceTypeDesc(Description);
                iControl.RequestSetSourceTypeOrder(Order);

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


            return MakeReturn(iModel.SelectedRecordId.ToString(), retVal);



        }


        // events

        public ServiceEventObject GetEvents(string chkIncludeBirths, string chkIncludeDeaths, string chkIncludeWitnesses, string chkIncludeParents, string chkIncludeMarriages, string chkIncludeSpouses, string chkIncludePersonWithSpouses,
            string christianName, string surname, string lowerDateRange, string upperDateRange, string location,
            string page_number, string page_size, string sort_col)
        {
            ServiceEventObject serviceParishObject = new ServiceEventObject();

            CombinedEventSearchModel iModel = new CombinedEventSearchModel();
            CombinedEventSearchControl iControl = new CombinedEventSearchControl();
            iControl.SetModel(iModel);

            iControl.RequestSetUser(WebHelper.GetUser());

            EventType _eventType = new EventType(chkIncludeBirths.ToBool(),
                                                    chkIncludeDeaths.ToBool(),
                                                    chkIncludeWitnesses.ToBool(),
                                                    chkIncludeParents.ToBool(),
                                                    chkIncludeParents.ToBool(),
                                                    chkIncludeMarriages.ToBool(),
                                                    chkIncludeWitnesses.ToBool(),
                                                    chkIncludeMarriages.ToBool(),
                                                    chkIncludeSpouses.ToBool(),
                                                    chkIncludePersonWithSpouses.ToBool());


            iControl.RequestSetFilterCName(christianName);
            iControl.RequestSetFilterSName(surname);

            iControl.RequestSetFilterEventSelection(_eventType);
            iControl.RequestSetFilterLocation(location);
            iControl.RequestSetFilterLowerDate(lowerDateRange);
            iControl.RequestSetFilterUpperDate(upperDateRange);




            iControl.RequestRefresh();

            if (iModel.SearchEvents != null)
            {
                serviceParishObject.serviceEvents = iModel.SearchEvents.OrderBy(sort_col).Select(p => new ServiceEvent()
                {
                    EventChristianName = p.ChristianName,
                    EventSurname = p.Surname,
                    LinkId = p.LinkId,
                    EventLocation = p.Location,
                    EventId = p.RecordId,
                    EventDate = p.EventYear,
                    EventDescription = SearchEvent.GetEventString(p.EventType),
                    EventText = p.Description,
                    LinkTypeId = SearchEvent.GetLinkTypeId(p.SearchEventLinkType)
                }).ToList();

                serviceParishObject.Batch = page_number.ToInt32();
                serviceParishObject.BatchLength = page_size.ToInt32();
                serviceParishObject.Total = serviceParishObject.serviceEvents.Count;

                serviceParishObject.serviceEvents = serviceParishObject.serviceEvents.Skip(page_number.ToInt32() * page_size.ToInt32()).Take(page_size.ToInt32()).ToList();
            }
            else
            {
                serviceParishObject.Batch = page_number.ToInt32();
                serviceParishObject.BatchLength = page_size.ToInt32();
                serviceParishObject.Total = 0;
                serviceParishObject.serviceEvents = new List<ServiceEvent>();
            }

            return serviceParishObject;
        }




        // misc

        public void AssignLocations()
        {
            DeathBirthFilterModel.UpdateLocationIdsFromParishTable();
        }

        public void UpdateDateEstimates()
        {
            DeathBirthFilterModel deathBirthFilterModel = new DeathBirthFilterModel();
            DeathBirthFilterControl deathBirthFilterControl = new DeathBirthFilterControl();
            deathBirthFilterControl.SetModel(deathBirthFilterModel);

            deathBirthFilterControl.RequestUpdateDateEstimates();
        }

        public string GetLoggedInUser()
        {

            MembershipUser muser = null;
            string returnValue = "";

            try
            {
                muser = Membership.GetUser();
                returnValue = muser.UserName;

            }
            catch (Exception ex1)
            {
                returnValue = "Guest,Error: " + ex1.Message;
            }


            return returnValue;
        }

        public void WriteParams(params string[] parameters)
        {

            StackTrace stackTrace = new StackTrace();
            MethodBase method = stackTrace.GetFrame(1).GetMethod();


            List<string> names = method.GetParameters().Select(s => s.Name).ToList();

            int idx = 0;

            foreach (var p in parameters)
            {

                Debug.WriteLine(names[idx] + " " + p);
                idx++;
            }


        }

        private string MakeReturn(string recordId, string error)
        {
            return "Id=" + recordId + "&Error=" + error;
        }



        // parishs
        public ServiceParishObject GetParishs(string deposited, string name, string county, string page_number, string page_size, string sort_col)
        {
            ServiceParishObject serviceParishObject = new ServiceParishObject();


            ParishsFilterModel iModel = new ParishsFilterModel();
            ParishsFilterControl iControl = new ParishsFilterControl();


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

            return MakeReturn(iModel.SelectedRecordId.ToString(), retVal);
        }

        public List<ServiceSuperParish> GetParishsFromLocations(string parishLocation)
        {
            ParishsBLL parishsBll = new ParishsBLL();
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
            ParishsBLL parishsBll = new ParishsBLL();

            return parishsBll.GetParishTypes().Select(o => new ServiceParishDataType() { DataTypeId = o.dataTypeId, Description = o.description }).ToList();
        }

        public ServiceParishDetailObject GetParishDetail(string parishId)
        {
            ParishsBLL parishsBll = new ParishsBLL();

            ParishCollection pcoll = parishsBll.GetParishDetail(parishId.ToGuid());

            //  pcoll.parishDataTypes.AddRange(parishsBll.GetParishTypes());

            //   pcoll.sourceRecords = parishsBll.GetParishSourceRecords(_parish);


            ServiceParishDetailObject serviceParishDetailObject = new ServiceParishDetailObject();

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



            serviceParishDetailObject.PersonCount = GetPersonsCount("", "", "thac", "", "", "", "", "", "", "1400", "1950", "false", "false", "false", sourceStr);




            serviceParishDetailObject.MarriageCount = GetMarriagesCount("", "", "", "", "", "", "0", "0", sourceStr);


            return serviceParishDetailObject;
        }

        public List<ServiceSearchResult> GetSearchResults(string parishIds, string startYear, string endYear)
        {

            SourceBLL sourceBll = new SourceBLL();
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
            ParishsBLL parishsBll = new ParishsBLL();


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
            ParishsBLL parishsBll = new ParishsBLL();

            List<Guid> parishIdLst = new List<Guid>();

            parishIds.Split(',').ToList().ForEach(p => parishIdLst.Add(p.ToGuid()));

            return parishsBll.GetParishs2().Where(p => parishIdLst.Contains(p.ParishId)).Select(s => s.ParishName).ToList();
        }


        public string AddParish(string ParishId, string ParishStartYear, string ParishEndYear,
                                string ParishLat, string ParishLong,
                                string ParishName, string ParishParent,
                                string ParishNote, string ParishCounty, string ParishDeposited)
        {

            WriteParams(ParishId, ParishStartYear, ParishEndYear,
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

            return MakeReturn(iModel.SelectedRecordId.ToString(), retVal);
        }

        public ServiceParish GetParish(string parishId)
        {
            ParishsEditorModel iModel = new ParishsEditorModel();
            ParishEditorControl iControl = new ParishEditorControl();


            ServiceParish parish = new ServiceParish();

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

            return parish;


        }





        // persons



        public string SetDuplicate(string persons)
        {

            string retVal = "";

            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();

            List<Guid> selection = new List<Guid>();

            try
            {
                persons.Split(',').ToList().ForEach(s => selection.Add(s.ToGuid()));

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedIds(selection);

                iControl.RequestSetRelationTypeId(1);
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

            return MakeReturn(persons, retVal);
        }


        public string MergeSources(string person)
        {
            string retVal = "";

            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();

            try
            {
                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedId(person.ToGuid());

                iControl.RequestMergeDuplicates();
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


            return MakeReturn(person, retVal);
        }

        public string RemoveLink(string person)
        {
            string retVal = "";

            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();
            List<Guid> selection = new List<Guid>();

            try
            {

                person.Split(',').ToList().ForEach(s => selection.Add(s.ToGuid()));

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedIds(selection);

                iControl.RequestRemoveRelationType();

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

            return MakeReturn(person, retVal);
        }

        public string SetPersonRelation(string persons, string relationType)
        {
            string retVal = "";

            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();
            List<Guid> selection = new List<Guid>();

            try
            {
                persons.Split(',').ToList().ForEach(s => selection.Add(s.ToGuid()));

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedIds(selection);

                iControl.RequestSetRelationTypeId(relationType.ToInt32());
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

            return MakeReturn(persons, retVal);
        }

        public string DeletePerson(string personId)
        {
            string retVal = "";

            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();
            List<Guid> selection = new List<Guid>();

            try
            {

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                personId.Split(',').ToList().ForEach(s => selection.Add(s.ToGuid()));

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



            return MakeReturn(personId, retVal);
        }

        public string AddPerson(string personId, string birthparishId, string deathparishId, string referenceparishId, string sources, string christianName, string surname, string fatherchristianname,
            string fathersurname, string motherchristianname, string mothersurname,
            string source, string ismale, string occupation, string datebirthstr,
            string datebapstr, string birthloc, string birthcounty, string datedeath,
            string deathloc, string deathcounty, string notes, string refdate,
            string refloc, string fatheroccupation, string spousesurname, string spousechristianname, string years, string months, string weeks, string days)
        {
            string retVal = "";

            WriteParams(birthparishId, deathparishId, referenceparishId, sources, christianName, surname, fatherchristianname,
             fathersurname, motherchristianname, mothersurname,
             source, ismale, occupation, datebirthstr,
             datebapstr, birthloc, birthcounty, datedeath,
             deathloc, deathcounty, notes, refdate,
             refloc, fatheroccupation, spousesurname, spousechristianname, years, months, weeks, days);


            if (datebapstr == "" && datebirthstr == "" && datedeath !="" && (years !="" || months !="" || weeks != "" || days !=""))
            {
                DateTime deathDate = new DateTime(2100, 1, 1);

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

                    idays = (iyears * 365) + (imonths * 28) + (iweeks * 7) + idays;

                    TimeSpan ts = new TimeSpan(idays,1,1,1);


                    DateTime birthDate = deathDate.Subtract(ts);

                    datebirthstr = birthDate.ToString("dd MMM yyyy");

                }
            }



            IDeathBirthEditorModel iModel = null;
            IDeathBirthEditorControl iControl = null;

            iModel = new DeathBirthEditorModel();
            iControl = new DeathBirthEditorControl();

            try
            {


                if (iModel != null)
                    iControl.SetModel(iModel);

                List<Guid> sourceList = new List<Guid>();

                if (sources != null)
                    sources.Split(',').Where(s => s != null).ToList().ForEach(s => sourceList.Add(s.ToGuid()));

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedId(personId.ToGuid());

                iControl.RequestSetSourceGuidsList(sourceList);

                iControl.RequestSetEditorChristianName(christianName);

                iControl.RequestSetEditorSurnameName(surname);


                iControl.RequestSetEditorFatherChristianName(fatherchristianname);

                iControl.RequestSetEditorFatherSurname(fathersurname);

                iControl.RequestSetEditorMotherChristianName(motherchristianname);

                iControl.RequestSetEditorMotherSurname(mothersurname);

                iControl.RequestSetEditorSource(source);

                if (ismale.ToBool())
                    iControl.RequestSetEditorIsMale(true);
                else
                    iControl.RequestSetEditorIsMale(false);

                iControl.RequestSetEditorOccupation(occupation);

                iControl.RequestSetEditorDateBirthString(datebirthstr);

                iControl.RequestSetEditorDateBapString(datebapstr);

                iControl.RequestSetEditorBirthLocation(birthloc);

                iControl.RequestSetEditorBirthCountyLocation(birthcounty);

                iControl.RequestSetEditorDateDeathString(datedeath);

                iControl.RequestSetEditorDeathLocation(deathloc);

                iControl.RequestSetEditorDeathCountyLocation(deathcounty);

                iControl.RequestSetEditorNotes(notes);

                iControl.RequestSetEditorReferenceDate(refdate);

                iControl.RequestSetEditorFatherOccupation(fatheroccupation);

                iControl.RequestSetEditorSpouseSName(spousesurname);

                iControl.RequestSetEditorSpouseCName(spousechristianname);

                iControl.RequestSetEditorReferenceLocation(refloc);

                iControl.RequestSetEditorBirthLocationId(birthparishId.ToGuid());

                iControl.RequestSetEditorDeathLocationId(deathparishId.ToGuid());

                iControl.RequestSetEditorReferenceLocationId(referenceparishId.ToGuid());

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


            return MakeReturn(iModel.SelectedRecordId.ToString(), retVal);
        }


        public int GetPersonsCount(string _parentId, string christianName, string surname, string fatherChristianName,
            string fatherSurname, string motherChristianName, string motherSurname, string location, string county,
            string lowerDate, string upperDate, string filterTreeResults, string filterIncludeBirths, string filterIncludeDeaths, string filterSource)
        {
            Guid parentId = _parentId.ToGuid();

            ServicePersonObject spo = new ServicePersonObject();

            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();
            iControl.SetModel(iModel);
            iControl.RequestSetUser(WebHelper.GetUser());

            if (parentId == Guid.Empty)
            {


                iControl.RequestSetFilterMode(DeathBirthFilterTypes.SIMPLE);
                iControl.RequestSetSelectedIds(new List<Guid>());



                iControl.RequestSetFilterCName(christianName ?? "");
                iControl.RequestSetFilterSName(surname ?? "");
                iControl.RequestSetFilterFatherCName(fatherChristianName ?? "");
                iControl.RequestSetFilterFatherSName(fatherSurname ?? "");
                iControl.RequestSetFilterMotherCName(motherChristianName ?? "");
                iControl.RequestSetFilterMotherSName(motherSurname ?? "");
                iControl.RequestSetFilterLocation(location ?? "");
                iControl.RequestSetFilterLowerBirth(lowerDate ?? "");
                iControl.RequestSetFilterUpperBirth(upperDate ?? "");
                iControl.RequestSetFilterTreeResults(filterTreeResults.ToBool());
                iControl.RequestSetFilterIsIncludeBirths(filterIncludeBirths.ToBool());
                iControl.RequestSetFilterIsIncludeDeaths(filterIncludeDeaths.ToBool());




                if (filterSource != "")
                {
                    iControl.RequestSetFilterSource(filterSource);

                    List<Guid> sources = new List<Guid>();

                    foreach (string _guid in filterSource.Split(','))
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

                iControl.RequestSetFilterMode(DeathBirthFilterTypes.DUPLICATES);


            }


            iControl.RequestRefresh();



            return iModel.PersonsDataTable.Total;

        }


        public ServicePerson GetPerson(string id)
        {
            string retVal = "";

            IDeathBirthEditorModel iModel = new DeathBirthEditorModel();
            IDeathBirthEditorControl iControl = new DeathBirthEditorControl();

            ServicePerson sp = new ServicePerson();

            try
            {

                iControl.SetModel(iModel);
                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestSetSelectedId(id.ToGuid());

                iControl.RequestRefresh();



                sp.Baptism = iModel.EditorDateBapString;
                sp.Birth = iModel.EditorDateBirthString;
                sp.BirthCounty = iModel.EditorBirthCountyLocation;
                sp.BirthLocation = iModel.EditorBirthLocation;
                sp.BirthLocationId = iModel.EditorBirthLocationId.ToString();
                sp.BirthYear = iModel.EstBirthInt;
                sp.ChristianName = iModel.EditorChristianName;
                sp.Death = iModel.EditorDateDeathString;
                sp.DeathCounty = iModel.EditorDeathCountyLocation;
                sp.DeathLocation = iModel.EditorDeathLocation;
                sp.DeathLocationId = iModel.EditorDeathLocationId.ToString();
                sp.DeathYear = iModel.EstDeathInt;
                sp.FatherChristianName = iModel.EditorFatherChristianName;
                sp.FatherSurname = iModel.EditorFatherSurname;
                sp.FatherOccupation = iModel.EditorFatherOccupation;
                sp.IsMale = iModel.EditorIsMale.ToString();
                sp.MotherChristianName = iModel.EditorMotherChristianName;
                sp.MotherSurname = iModel.EditorMotherSurname;
                sp.Notes = iModel.EditorNotes;
                sp.Occupation = iModel.EditorOccupation;
                sp.PersonId = iModel.SelectedRecordId;
                sp.ReferenceLocation = iModel.EditorReferenceLocation;
                sp.ReferenceLocationId = iModel.EditorReferenceLocationId.ToString();
                sp.ReferenceDate = iModel.EditorReferenceDateString;
                sp.SourceDescription = iModel.EditorSource;
                sp.Sources = iModel.SourceGuidListAsString;
                sp.SpouseChristianName = iModel.EditorSpouseCName;
                sp.SpouseSurname = iModel.EditorSpouseSName;
                sp.Surname = iModel.EditorSurnameName;
                sp.XREF = "";

            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;

                sp.ErrorStatus = retVal;
            }



            return sp;
        }

        public ServicePersonObject GetPersons(string _parentId,
            string christianName,
            string surname,
            string fatherChristianName,
            string fatherSurname,
            string motherChristianName,
            string motherSurname,
            string location,
            string county,
            string lowerDate,
            string upperDate,
            string filterTreeResults,
            string filterIncludeBirths,
            string filterIncludeDeaths,
            string filterSource,
            string spouse,
            string parishFilter,

            string page_number,
            string page_size,
            string sort_col)
        {
            string retVal = "";

            Guid parentId = _parentId.ToGuid();

            ServicePersonObject spo = new ServicePersonObject();
            ParishsBLL parishsBll = new ParishsBLL();
            Guid parishId = parishFilter.ToGuid();
            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();

            try
            {
                iControl.SetModel(iModel);

                if (parishId != Guid.Empty)
                {

                    filterSource = "";

                    parishsBll.GetParishSourceRecords(parishId).ForEach(p => filterSource += "," + p.SourceId.ToString());

                    filterSource = filterSource.Substring(1);
                }

                if (parentId == Guid.Empty)
                {


                    iControl.RequestSetFilterMode(DeathBirthFilterTypes.SIMPLE);
                    iControl.RequestSetSelectedIds(new List<Guid>());
                    iControl.RequestSetUser(WebHelper.GetUser());
                    int pageNo = page_number.ToInt32();

                    iControl.RequestSetFilterCName(christianName ?? "");
                    iControl.RequestSetFilterSName(surname ?? "");
                    iControl.RequestSetFilterFatherCName(fatherChristianName ?? "");
                    iControl.RequestSetFilterFatherSName(fatherSurname ?? "");
                    iControl.RequestSetFilterMotherCName(motherChristianName ?? "");
                    iControl.RequestSetFilterMotherSName(motherSurname ?? "");
                    iControl.RequestSetFilterLocation(location ?? "");
                    iControl.RequestSetFilterLowerBirth(lowerDate ?? "");
                    iControl.RequestSetFilterUpperBirth(upperDate ?? "");
                    iControl.RequestSetFilterTreeResults(filterTreeResults.ToBool());
                    iControl.RequestSetFilterIsIncludeBirths(filterIncludeBirths.ToBool());
                    iControl.RequestSetFilterIsIncludeDeaths(filterIncludeDeaths.ToBool());
                    iControl.RequestSetFilterSpouseCName(spouse);

                    iControl.RequestSetRecordStart(page_number.ToInt32());

                    iControl.RequestSetRecordPageSize(page_size.ToInt32());

                    if (filterSource != "")
                    {
                        iControl.RequestSetFilterSource(filterSource);

                        List<Guid> sources = new List<Guid>();

                        foreach (string _guid in filterSource.Split(','))
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

                    iControl.RequestSetFilterMode(DeathBirthFilterTypes.DUPLICATES);


                }


                iControl.RequestRefresh();


                //spo.servicePersons = iModel.PersonsDataTable.OrderBy(sort_col).Select(p => new ServicePersonLookUp()
                //{
                //    BirthLocation = p.BirthLocation,
                //    BirthYear = p.BirthInt,
                //    ChristianName = p.ChristianName,
                //    DeathLocation = p.DeathLocation,
                //    DeathYear = p.DeathInt,
                //    FatherChristianName = p.FatherChristianName,
                //    FatherSurname = p.Surname,
                //    MotherChristianName = p.MotherChristianName,
                //    MotherSurname = p.MotherSurname,
                //    PersonId = p.Person_id,
                //    Sources = p.Source,
                //    Surname = p.Surname,
                //    XREF = p.UniqueRef.ToString(),
                //    Events = p.TotalEvents.ToString()
                //}).ToList();


                //spo.Batch = page_number.ToInt32();
                //spo.BatchLength = page_size.ToInt32();
                //spo.Total = spo.servicePersons.Count;

                //spo.servicePersons = spo.servicePersons.Skip(page_number.ToInt32() * page_size.ToInt32()).Take(page_size.ToInt32()).ToList();


                spo = iModel.PersonsDataTable;
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;

                spo.ErrorStatus = retVal;
            }

            return spo;
        }


        // marriages

        public ServiceMarriage GetMarriage(string id)
        {
            MarriagesEditorModel iModel = new MarriagesEditorModel();
            MarriagesEditorControl iControl = new MarriagesEditorControl();

            iControl.SetModel(iModel);

            iControl.RequestSetUser(WebHelper.GetUser());

            iControl.RequestSetSelectedIds(id.ToGuid());

            iControl.RequestRefresh();

            ServiceMarriage serviceMarriage = new ServiceMarriage()
            {
                FemaleBirthYear = iModel.EditorFemaleBirthYear.ToInt32(),
                FemaleCName = iModel.EditorFemaleCName,
                FemaleLocation = iModel.EditorFemaleLocation,
                FemaleLocationId = iModel.EditorFemaleLocationId.ToString(),
                FemaleNotes = iModel.EditorFemaleInfo,
                FemaleOccupation = iModel.EditorFemaleOccupation,
                FemaleSName = iModel.EditorFemaleSName,
                IsBanns = iModel.EditorIsBanns,
                IsLicense = iModel.EditorIsLicence,
                IsWidow = iModel.EditorIsWidow,
                IsWidower = iModel.EditorIsWidower,
                LocationCounty = iModel.EditorMarriageCounty,
                LocationId = iModel.EditorMarriageLocationId.ToString(),
                MaleBirthYear = iModel.EditorMaleBirthYear.ToInt32(),
                MaleCName = iModel.EditorMaleCName,
                MaleLocation = iModel.EditorMaleLocation,
                MaleLocationId = iModel.EditorMaleLocationId.ToString(),
                MaleNotes = iModel.EditorMaleInfo,
                MaleOccupation = iModel.EditorMaleOccupation,
                MaleSName = iModel.EditorMaleSName,
                MarriageDate = iModel.EditorDateMarriageString,
                MarriageId = iModel.SelectedRecordId,
                MarriageLocation = iModel.EditorLocation,
                Sources = iModel.SourceGuidListAsString,
                SourceDescription = iModel.EditorSource,
                Witness1ChristianName = iModel.EditorWitnessCName1,
                Witness1Surname = iModel.EditorWitness1,
                Witness2ChristianName = iModel.EditorWitnessCName2,
                Witness2Surname = iModel.EditorWitness2,
                Witness3ChristianName = iModel.EditorWitnessCName3,
                Witness3Surname = iModel.EditorWitness3,
                Witness4ChristianName = iModel.EditorWitnessCName4,
                Witness4Surname = iModel.EditorWitness4
            };

            return serviceMarriage;
        }

        public ServiceMarriageObject GetMarriages(string uniqref, string malecname, string malesname, string femalecname,
            string femalesname, string location, string lowerDate, string upperDate, string sourceFilter, string parishFilter, string page_number, string page_size, string sort_col)
        {

            //var temp = WebOperationContext.Current.IncomingRequest;
            ////var temp = OperationContext.Current.RequestContext.RequestMessage.Headers;

            //foreach (var header in temp.Headers)
            //{
            //    Debug.WriteLine(header);
            //}


            ServiceMarriageObject serviceMarriageObject = new ServiceMarriageObject();
            ParishsBLL parishsBll = new ParishsBLL();
            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();


            iControl.SetModel(iModel);

            iControl.RequestSetUser(WebHelper.GetUser());

            ////if (query.AllKeys.Contains("error"))
            ////{
            ////    this.SetErrorState(query["error"] ?? "");
            ////}

            Guid parishId = parishFilter.ToGuid();

            if (parishId != Guid.Empty)
            {

                sourceFilter = "";

                parishsBll.GetParishSourceRecords(parishId).ForEach(p => sourceFilter += "," + p.SourceId.ToString());

                sourceFilter = sourceFilter.Substring(1);
            }


            Guid parentId = uniqref.ToGuid();

            if (parentId == Guid.Empty)
            {

                iControl.RequestSetFilterMode(MarriageFilterTypes.STANDARD);

                iControl.RequestSetSelectedIds(new List<Guid>());

                //    marriagesFilterControl.RequestSetSelectedRecordIds(new List<Guid>());


                //   int pageNo = page_number.ToInt32();

                //    marriagesFilterControl.RequestSetRecordStart(pageNo);



                iControl.RequestSetFilterMaleName((malecname ?? ""), (malesname ?? ""));


                iControl.RequestSetFilterFemaleName((femalecname ?? ""), (femalesname ?? ""));


                iControl.RequestSetFilterLocation(location ?? "");

                //     marriagesFilterControl.RequestSetFilterLocationCounty(county ?? "");


                iControl.RequestSetFilterMarriageBoundLower(lowerDate ?? "");
                iControl.RequestSetFilterMarriageBoundUpper(upperDate ?? "");




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

            iControl.RequestRefresh();

            if (sort_col.Contains("MarriageDate DESC"))
            {
                sort_col = "MarriageYear DESC";
            }
            else if (sort_col.Contains("MarriageDate"))
            {
                sort_col = "MarriageYear";
            }


            serviceMarriageObject.serviceMarriages = iModel.MarriagesTable.OrderBy(sort_col).Select(p => new ServiceMarriageLookup()
            {
                Events = p.MarriageTotalEvents.ToString(),
                FemaleCName = p.FemaleCName,
                FemaleSName = p.FemaleSName,
                MaleCName = p.MaleCName,
                MaleSName = p.MaleSName,
                MarriageDate = p.MarriageYear.ToString(),
                MarriageId = p.MarriageId,
                MarriageLocation = p.MarriageLocation,
                Sources = p.MarriageSource,
                Witnesses = p.Witnesses,
                XREF = p.UniqueRef.ToString()

            }).ToList();


            serviceMarriageObject.Batch = page_number.ToInt32();
            serviceMarriageObject.BatchLength = page_size.ToInt32();
            serviceMarriageObject.Total = serviceMarriageObject.serviceMarriages.Count;

            serviceMarriageObject.serviceMarriages = serviceMarriageObject.serviceMarriages.Skip(page_number.ToInt32() * page_size.ToInt32()).Take(page_size.ToInt32()).ToList();


            return serviceMarriageObject;
        }

        public string DeleteMarriages(string marriageIds)
        {
            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();
            string retVal = "";

            try
            {
                iControl.SetModel(iModel);

                List<Guid> selection = new List<Guid>();

                marriageIds.Split(',').ToList().ForEach(p => selection.Add(p.ToGuid()));


                iControl.RequestSetSelectedIds(selection);
                iControl.RequestSetUser(WebHelper.GetUser());
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

            return MakeReturn(marriageIds, retVal);
        }

        public string SetMarriageDuplicate(string marriages)
        {
            //RequestSetSelectedDuplicateMarriage
            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();

            string retVal = "";

            try
            {
                iControl.SetModel(iModel);

                List<Guid> selection = new List<Guid>();


                string[] persons = marriages.Split(',');

                foreach (string _person in persons)
                {
                    selection.Add(_person.ToGuid());
                }

                iControl.RequestSetSelectedIds(selection);
                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestSetSelectedDuplicateMarriage();

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

            return MakeReturn(marriages, retVal);

        }

        public string RemoveMarriageLink(string marriage)
        {

            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();

            string retVal = "";

            try
            {
                iControl.SetModel(iModel);

                List<Guid> selection = new List<Guid>();


                string[] persons = marriage.Split(',');

                foreach (string _person in persons)
                {
                    selection.Add(_person.ToGuid());
                }

                iControl.RequestSetSelectedIds(selection);

                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestSetRemoveSelectedFromDuplicateList();

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

            return MakeReturn(marriage, retVal);

        }

        public string MergeMarriage(string marriage)
        {
            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();

            string retVal = "";

            try
            {
                iControl.SetModel(iModel);

                iControl.RequestSetSelectedId(marriage.ToGuid());
                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestSetMergeSources();

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

            return MakeReturn(marriage, retVal);
        }

        public int GetMarriagesCount(string uniqref, string malecname, string malesname, string femalecname, string femalesname,
            string location, string lowerDate, string upperDate, string sourceFilter)
        {
            ServiceMarriageObject serviceMarriageObject = new ServiceMarriageObject();

            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();


            iControl.SetModel(iModel);

            ////if (query.AllKeys.Contains("error"))
            ////{
            ////    this.SetErrorState(query["error"] ?? "");
            ////}

            Guid parentId = uniqref.ToGuid();

            if (parentId == Guid.Empty)
            {
                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetFilterMode(MarriageFilterTypes.STANDARD);

                iControl.RequestSetSelectedIds(new List<Guid>());

                //    marriagesFilterControl.RequestSetSelectedRecordIds(new List<Guid>());


                //   int pageNo = page_number.ToInt32();

                //    marriagesFilterControl.RequestSetRecordStart(pageNo);



                iControl.RequestSetFilterMaleName((malecname ?? ""), (malesname ?? ""));


                iControl.RequestSetFilterFemaleName((femalecname ?? ""), (femalesname ?? ""));


                iControl.RequestSetFilterLocation(location ?? "");

                //     marriagesFilterControl.RequestSetFilterLocationCounty(county ?? "");


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





            return iModel.MarriagesTable.Count;
        }

        public string AddMarriage(
                    string FemaleLocationId, string LocationId, string MaleLocationId, string SourceDescription, string Sources, string MarriageId, string IsBanns, string IsLicense, string IsWidow, string IsWidower,
                    string FemaleBirthYear, string FemaleCName, string FemaleLocation, string FemaleNotes, string FemaleOccupation, string FemaleSName, string LocationCounty, string MaleBirthYear, string MaleCName,
                    string MaleLocation, string MaleNotes, string MaleOccupation, string MaleSName, string MarriageDate, string MarriageLocation,
                    string Witness1ChristianName, string Witness1Surname, string Witness2ChristianName, string Witness2Surname, string Witness3ChristianName, string Witness3Surname, string Witness4ChristianName, string Witness4Surname)
        {


            WriteParams(FemaleLocationId, LocationId, MaleLocationId, SourceDescription, Sources, MarriageId, IsBanns, IsLicense, IsWidow, IsWidower,
                     FemaleBirthYear, FemaleCName, FemaleLocation, FemaleNotes, FemaleOccupation, FemaleSName, LocationCounty, MaleBirthYear, MaleCName,
                     MaleLocation, MaleNotes, MaleOccupation, MaleSName, MarriageDate, MarriageLocation,
                     Witness1ChristianName, Witness1Surname, Witness2ChristianName, Witness2Surname, Witness3ChristianName, Witness3Surname, Witness4ChristianName, Witness4Surname);

            string retVal = "";
            IMarriageEditorModel iModel = new MarriagesEditorModel();
            IMarriageEditorControl iControl = new MarriagesEditorControl();

            try
            {
                if (iModel != null)
                    iControl.SetModel(iModel);

                List<Guid> sourceList = new List<Guid>();

                if (Sources != null)
                    Sources.Split(',').ToList().ForEach(s => sourceList.Add(s.ToGuid()));

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedId(MarriageId.ToGuid());
                iControl.RequestSetSourceGuidsList(sourceList);

                iControl.RequestSetEditorMarriageDate(MarriageDate);
                iControl.RequestSetEditorSource(SourceDescription);
                iControl.RequestSetEditorMaleName(MaleCName, MaleSName);

                iControl.RequestSetEditorFemaleName(FemaleCName, FemaleSName);

                iControl.RequestSetEditorMaleInfo(MaleNotes);
                iControl.RequestSetEditorFemaleInfo(FemaleNotes);
                iControl.RequestSetEditorLocation(MarriageLocation);
                iControl.RequestSetEditorMarriageCounty(LocationCounty);
                iControl.RequestSetEditorMaleLocation(MaleLocation);
                iControl.RequestSetEditorFemaleLocation(FemaleLocation);

                iControl.RequestSetEditorWitness1(Witness1Surname);
                iControl.RequestSetEditorWitness1CName(Witness1ChristianName);

                iControl.RequestSetEditorWitness2(Witness2Surname);
                iControl.RequestSetEditorWitness2CName(Witness2ChristianName);

                iControl.RequestSetEditorWitness3(Witness3Surname);
                iControl.RequestSetEditorWitness3CName(Witness3ChristianName);

                iControl.RequestSetEditorWitness4(Witness4Surname);
                iControl.RequestSetEditorWitness4CName(Witness4ChristianName);

                iControl.RequestSetEditorIsWidow(IsWidow.ToBool());
                iControl.RequestSetEditorIsWidower(IsWidower.ToBool());
                iControl.RequestSetEditorIsBanns(IsBanns.ToBool());
                iControl.RequestSetEditorIsLicence(IsLicense.ToBool());

                iControl.RequestSetEditorMaleOccupation(MaleOccupation);
                iControl.RequestSetEditorFemaleOccupation(FemaleOccupation);
                iControl.RequestSetEditorMaleBirthYear(MaleBirthYear);
                iControl.RequestSetEditorFemaleBirthYear(FemaleBirthYear);
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


            return MakeReturn(iModel.SelectedRecordId.ToString(), retVal);

        }




        //files

        public ServiceFileObject GetFilesForSource(string sourceId, string page_number, string page_size)
        {
            ServiceFileObject sfo = new ServiceFileObject();

            FilesBLL filesBLL = new FilesBLL();

            var filesMappedDataTable = filesBLL.GetFilesByParentId2(sourceId.ToGuid());



            sfo.serviceFiles = sfo.serviceFiles.Select(f => new ServiceFile()
            {
                FileDescription = f.FileDescription,
                FileId = f.FileId,
                FileLocation = f.FileLocation,
                FileThumbLocation = f.FileThumbLocation
            }).ToList();


            sfo.Batch = page_number.ToInt32();
            sfo.BatchLength = page_size.ToInt32();
            sfo.Total = sfo.serviceFiles.Count;

            sfo.serviceFiles = sfo.serviceFiles.Skip(page_number.ToInt32() * page_size.ToInt32()).Take(page_size.ToInt32()).ToList();


            return sfo;
        }




       
    }





}




