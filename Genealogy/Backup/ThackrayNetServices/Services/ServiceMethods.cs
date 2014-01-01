using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Script.Serialization;
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


namespace ANDServices
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class APIServices : IAnd
    {

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


        public void Upload(string fileName, Stream stream)
        {

            //MultipartParser parser = new MultipartParser(stream);
            //if (parser.Success)
            //{
            //    // Save the file
            //   // SaveFile(parser.Filename, parser.ContentType, parser.FileContents);

            //   System.IO.File.WriteAllBytes("C:\\Temp\\" + fileName, parser.FileContents);
            //}

            //FileStream fileToupload = new FileStream("C:\\Temp\\" + fileName, FileMode.Create);

            //byte[] bytearray = new byte[10000];
            //int bytesRead, totalBytesRead = 0;
            //do
            //{
            //    bytesRead = stream.Read(bytearray, 0, bytearray.Length);
            //    totalBytesRead += bytesRead;
            //} while (bytesRead > 0);

            //fileToupload.Write(bytearray, 0, bytearray.Length);
            //fileToupload.Close();
            //fileToupload.Dispose();

        }


        // tree related methods

        public ServiceSourceObject GetJSONTreeSources(string description, string pageNumber, string pageSize)
        {
            ISourceFilterModel iModel = new SourceFilterModel();
            ISourceFilterControl iControl = new SourceFilterControl();
            string retVal= "";

            try
            {
                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetFilterSourceDescription(description ?? "");

                iControl.RequestSetRecordStart(pageNumber.ToInt32());

                iControl.RequestSetRecordPageSize(pageSize.ToInt32());

                iControl.RequestSetFilterIncludeDefaultPerson("true");

                iControl.RequestSetFilterMode(SourceFilterTypes.TREESOURCES);

                iControl.RequestRefresh();
            }
            catch (Exception ex1)
            {
                retVal = "Exception: "+ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
                iModel.SourcesDataTable.ErrorStatus = retVal;
            }


            return iModel.SourcesDataTable; 
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

                iControl.RequestSetFilterMode(DeathBirthFilterTypes.Tree);

                iControl.RequestSetSelectedIds(new List<Guid>() { sourceId.ToGuid()});

                iControl.RequestSetUser(WebHelper.GetUser());
             
                iControl.RequestSetSearchFilter(new PersonSearchFilter()
                {
                    LowerDate = start.ToInt32(),
                    UpperDate = end.ToInt32()
                });

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
            TreeBll treeBLL = new TreeBll();

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


            SourceMappingsBll sourceMappingsBll = new SourceMappingsBll();

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
            SourceBll _sources = new SourceBll();


            return _sources.DeleteTree(treeId.ToGuid());
        }









        // events

        public ServiceEventObject GetEvents(string chkIncludeBirths, string chkIncludeDeaths, string chkIncludeWitnesses, string chkIncludeParents, string chkIncludeMarriages, string chkIncludeSpouses, string chkIncludePersonWithSpouses,
            string christianName, string surname, string lowerDateRange, string upperDateRange, string location,
            string page_number, string page_size, string sort_col)
        {
            ServiceEventObject serviceParishObject = new ServiceEventObject();

            CombinedEventSearchModel iModel = new CombinedEventSearchModel();
            CombinedEventSearchControl iControl = new CombinedEventSearchControl();
            string retVal = "";

            try
            {
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




        // misc

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

       

       



   

        public ServiceFileObject GetFilesForSource(string sourceId, string page_number, string page_size)
        {
            ServiceFileObject sfo = new ServiceFileObject();

            FilesBll filesBLL = new FilesBll();

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




