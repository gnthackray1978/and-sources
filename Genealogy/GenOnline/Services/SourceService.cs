﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Script.Serialization;
using GenOnline.Helpers;
using GenOnline.Services.Contracts;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.enums;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenOnline.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class SourceService : ISourceService
    {

        public string AddMarriageSources(string record, string sources)
        {
            var iModel = new SourceSearch(new Security(new WebUser()));
            string retVal = "";

            try
            {
                iModel.AddSources(record.ParseToGuidList(), sources.ParseToGuidList(), SourceTypes.Marriage);
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            return WebHelper.MakeReturn(record, retVal);
        }

        public string AddPersonSources(string record, string sources)
        {
            var iModel = new SourceSearch(new Security(new WebUser()));
            string retVal = "";

            try
            {
                iModel.AddSources(record.ParseToGuidList(), sources.ParseToGuidList(),SourceTypes.Person);
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            return WebHelper.MakeReturn(record, retVal);
        }

        public string RemoveTreeSources(string record)
        {
            string retVal = "";
            var iModel = new SourceSearch(new Security(new WebUser()));

            try
            {
                iModel.RemoveTreeSources(record.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            return WebHelper.MakeReturn(record, retVal);
        }
     

        public List<CensusSource> Get1841CensusSources(Guid sourceId)
        {
            var serviceSourceObject = new ServiceSourceObject();                     
            var iModel = new SourceSearch(new Security(new WebUser()));

            string retVal = "";
            try
            {             
              
                var ssf = new SourceSearchFilter() { Sources = new List<Guid>() { sourceId } };
                serviceSourceObject = iModel.Search(SourceSearchTypes.Censussource, ssf,new DataShaping(){RecordPageSize = 0},new SourceSearchValidator(ssf));

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
         
                serviceSourceObject.ErrorStatus = retVal;
            }


            return serviceSourceObject.CensusSources;
        }


        public SourceAjaxDto GetSource(string sourceId)
        {

            var ssobj = new SourceDto();

            var iModel = new SourceSearch(new Security(new WebUser()));
          
            string retVal = "";
              
            try
            {
               ssobj = iModel.Get(new SourceDto(){SourceId = sourceId.ToGuid()});
                 
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                ssobj.ErrorStatus += retVal;

            }

            return ssobj.ToSourceAjaxDto();
        }

        public List<string> GetSourceNames(string sourceIds)
        {
            var serviceSourceObject = new ServiceSourceObject();   
                      
            var iModel = new SourceSearch(new Security(new WebUser()));

            string retVal = "";
            try
            {               
                var ssf = new SourceSearchFilter() { Sources = sourceIds.ParseToGuidList() };
                serviceSourceObject = iModel.Search(SourceSearchTypes.SourceIds, ssf, new DataShaping(){RecordPageSize = 0}, new SourceSearchValidator(ssf));
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                 
                serviceSourceObject.ErrorStatus = retVal;
            }

            return serviceSourceObject.serviceSources.Select(p => p.SourceRef).ToList();
        }





        public ServiceSourceObject GetSources(string sourceTypes, string sourceRef, string sourceDesc, string origLoc,
            string dateLB, string toDateLB, string dateUB, string toDateUB, string fileCount, string isThackrayFound,
            string isCopyHeld, string isViewed, string isChecked, string page_number, string page_size, string sortColumn)
        {

            var serviceSourceObject = new ServiceSourceObject();   

            string retVal = "";
            var ssf = new SourceSearchFilter()
                {
                    CensusSources1841 = false,
                    CensusPlaces1841 = false,
                    CopyHeld = isChecked.ToNullableBool() == true ? isCopyHeld.ToNullableBool() : null,
                    ThackrayFound = isChecked.ToNullableBool() == true ? isThackrayFound.ToNullableBool() : null,
                    Viewed = isChecked.ToNullableBool() == true ? isViewed.ToNullableBool() : null,
                    SourceTypes = sourceTypes.ParseToIntList(),
                    Ref = sourceRef,
                    Description = sourceDesc,
                    FromYear= (dateLB.ToInt32() + toDateLB.ToInt32()),
                    ToYear = (toDateUB.ToInt32() + dateUB.ToInt32()),
                    OriginalLocation = origLoc,
                    FileCount = fileCount,
                    UrStart = dateUB.ToInt32(),
                    UrEnd = toDateUB.ToInt32(),
                    LrStart = dateLB.ToInt32(),
                    LrEnd = toDateLB.ToInt32()                    
                };

             
            var iModel = new SourceSearch(new Security(new WebUser()));

            try
            {
                serviceSourceObject= iModel.Search(SourceSearchTypes.Standard, ssf, new DataShaping() { RecordStart = page_number.ToInt32(), RecordPageSize = page_size.ToInt32(), Column = sortColumn }, new SourceSearchValidator(ssf));
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;

                serviceSourceObject.ErrorStatus = retVal;
            }


            return serviceSourceObject; 
        }

        public string DeleteSource(string sourceId)
        {
    
      
            var iModel = new SourceSearch(new Security(new WebUser()));

            var retVal = "";
            try
            {

                iModel.DeleteRecords(new SourceSearchFilter() { Sources = sourceId.ParseToGuidList() });

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
             
            }



            return WebHelper.MakeReturn(sourceId, retVal);
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

           

            var jss = new JavaScriptSerializer();

            var obj = jss.Deserialize<dynamic>(fileIds);

            var tp = new List<ServiceFile>();
            
            if (obj != null)
                foreach (dynamic item in obj)
                    tp.Add(new ServiceFile(item["url"],item["desc"],item["id"]));
                                              
            var ssobj = new SourceDto()
                {
                    IsCopyHeld = isCopyHeld.ToBool(),
                    IsThackrayFound = isThackrayFound.ToBool(),
                    IsViewed = isViewed.ToBool(),
                    OriginalLocation = originalLocation,
                    SourceDateStr = sourceDateStr,
                    SourceDateStrTo = sourceDateStrTo,
                    SourceDesc = sourceDesc,
                    SourceRef = sourceRef,
                    SourceNotes = sourceNotes,
                    SourceFileCount = sourceFileCount.ToInt32(), 
                    SourceId = sourceId.ToGuid() ,
                    Files = tp,
                    Parishs = parishs.ParseToGuidList(),
                    SourceTypes = sourceTypes.ParseToIntList()
                };


            var iModel = new SourceSearch(new Security(new WebUser()));

            try
            {
                iModel.Update(ssobj, new SourceValidator(ssobj));

            }

            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            return WebHelper.MakeReturn(ssobj.SourceId.ToString(), retVal);

        }


    }
}