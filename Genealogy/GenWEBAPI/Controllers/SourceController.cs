using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using GenOnline;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.enums;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenWEBAPI.Controllers
{
    public static class UriSourceMappings
    {
        public const string Get1841CensusSources = "1841censussources";
        public const string Get1841CensusPlaces = "1841censusplaces";
        public const string GetSourceNames = "sources/names";
        public const string DeleteSource = "sources/delete";
        public const string GetSources = "sources";                                      
        public const string GetSource = "source";
        public const string AddSource = "source";
        public const string AddPersonTreeSource = "sources/persontree";
        public const string AddMarriageTreeSource = "sources/marriagetree"; 
        public const string RemoveTreeSources = "sources/tree/delete";
    }


    public class ServiceSourceAdd
    {

        public string SourceId { get; set; }
        public string IsCopyHeld { get; set; }
        public string IsThackrayFound { get; set; }
        public string IsViewed { get; set; }
        public string OriginalLocation { get; set; }
        public string SourceDesc { get; set; }
        public string SourceRef { get; set; }
        public string SourceNotes { get; set; }
        public string SourceDateStr { get; set; }
        public string SourceDateStrTo { get; set; }
        public string SourceFileCount { get; set; }
        public string Parishs { get; set; }
        public string SourceTypes { get; set; }
        public string FileIds { get; set; }          
    }

    public class SourceRecordSourceMap
    {
        public string Records { get; set; }
        public string Sources { get; set; }
    }

    public class SourceController : ApiController
    {
        [Route(UriSourceMappings.AddMarriageTreeSource)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AddMarriageSources(SourceRecordSourceMap sourceRecordSourceMap)
        {
            var sourceSearch = new SourceSearch(new Security(new WebUser()));

            string retVal;

            try
            {
                retVal = sourceSearch.AddSources(sourceRecordSourceMap.Records.ParseToGuidList(), sourceRecordSourceMap.Sources.ParseToGuidList(), SourceTypes.Marriage);
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
             
            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }
             
            return Ok(retVal);
        }

        [Route(UriSourceMappings.AddPersonTreeSource)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AddPersonSources(SourceRecordSourceMap sourceRecordSourceMap)
        {
            var sourceSearch = new SourceSearch(new Security(new WebUser()));
            string retVal = "";

            try
            {
                sourceSearch.AddSources(sourceRecordSourceMap.Records.ParseToGuidList(), sourceRecordSourceMap.Sources.ParseToGuidList(), SourceTypes.Person);
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(retVal);
        }

        [Route(UriSourceMappings.RemoveTreeSources)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult RemoveTreeSources(string record)
        {
            string retVal = "";
            var sourceSearch = new SourceSearch(new Security(new WebUser()));

            try
            {
                sourceSearch.RemoveTreeSources(record.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(retVal);
        }

        [Route(UriSourceMappings.Get1841CensusSources)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Get1841CensusSources(Guid sourceId)
        {
            var serviceSourceObject = new ServiceSourceObject();
            var sourceSearch = new SourceSearch(new Security(new WebUser()));

            string retVal = "";
            try
            {

                var ssf = new SourceSearchFilter() { Sources = new List<Guid>() { sourceId } };
                serviceSourceObject = sourceSearch.Search(SourceSearchTypes.Censussource, ssf, new DataShaping() { RecordPageSize = 0 }, new SourceSearchValidator(ssf));

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
                      
            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(serviceSourceObject.CensusSources);
        }

        [Route(UriSourceMappings.GetSource)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetSource(string sourceId)
        {
            
            var ssobj = new SourceDto();

            var sourceSearch = new SourceSearch(new Security(new WebUser()));

            string retVal = "";

            try
            {
                ssobj = sourceSearch.Get(new SourceDto() { SourceId = sourceId.ToGuid() });

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            
            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(ssobj.ToSourceAjaxDto());
        }

        [Route(UriSourceMappings.GetSourceNames)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetSourceNames(string sourceIds)
        {
            var servicesources = new List<string>();

            var sourceSearch = new SourceSearch(new Security(new WebUser()));

            string retVal = "";

            try
            {
                var ssf = new SourceSearchFilter() { Sources = sourceIds.ParseToGuidList() };
                var serviceSourceObject = sourceSearch.Search(SourceSearchTypes.SourceIds, ssf, new DataShaping() { RecordPageSize = 0 }, new SourceSearchValidator(ssf));

                servicesources = serviceSourceObject.serviceSources.Select(p => p.SourceRef).ToList();
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
             
            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(servicesources);
        }

        [Route(UriSourceMappings.GetSources)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetSources(string sourceTypes, string sourceRef, string sourceDesc, string origLoc,
            string dateLb, string toDateLb, string dateUb, string toDateUb, string fileCount, string isThackrayFound,
            string isCopyHeld, string isViewed, string isChecked, string pageNumber, string pageSize, string sortColumn)
        {

            ServiceSourceObject serviceSourceObject = new ServiceSourceObject();

            string retVal = "";
            var ssf = new SourceSearchFilter()
            {
                CensusSources1841 = false,
                CensusPlaces1841 = false,
                CopyHeld = isChecked.ToNullableBool() == true ? isCopyHeld.ToNullableBool() : null,
                ThackrayFound = isChecked.ToNullableBool() == true ? isThackrayFound.ToNullableBool() : null,
                Viewed = isChecked.ToNullableBool() == true ? isViewed.ToNullableBool() : null,
                SourceTypes = sourceTypes.ParseToIntList(),
                Ref = sourceRef ?? "",
                Description = sourceDesc ?? "",
                FromYear = (dateLb.ToInt32() + toDateLb.ToInt32()),
                ToYear = (toDateUb.ToInt32() + dateUb.ToInt32()),
                OriginalLocation = origLoc ?? "",
                FileCount = fileCount ?? "",
                UrStart = dateUb.ToInt32(),
                UrEnd = toDateUb.ToInt32(),
                LrStart = dateLb.ToInt32(),
                LrEnd = toDateLb.ToInt32()
            };


            var sourceSearch = new SourceSearch(new Security(new WebUser()));

            try
            {
                serviceSourceObject = sourceSearch.Search(SourceSearchTypes.Standard, ssf, new DataShaping() { RecordStart = pageNumber.ToInt32(), RecordPageSize = pageSize.ToInt32(), Column = sortColumn }, new SourceSearchValidator(ssf));
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }


            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(serviceSourceObject);
        }

        [Route(UriSourceMappings.DeleteSource)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteSource([FromBody]string sourceId)
        {

            var sourceSearch = new SourceSearch(new Security(new WebUser()));

            var retVal = "";
            try
            {
                sourceSearch.DeleteRecords(new SourceSearchFilter() { Sources = sourceId.ParseToGuidList() });
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }


            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);
        }

        [Route(UriSourceMappings.AddSource)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AddSource(ServiceSourceAdd serviceSourceAdd)
        {
            string retVal = "";

            var jss = new JavaScriptSerializer();

            var obj = jss.Deserialize<dynamic>(serviceSourceAdd.FileIds?? "");

            var tp = new List<ServiceFile>();

            if (obj != null)
                foreach (dynamic item in obj)
                    tp.Add(new ServiceFile(item["url"], item["desc"], item["id"]));

            var ssobj = new SourceDto()
            {
                IsCopyHeld = serviceSourceAdd.IsCopyHeld.ToBool(),
                IsThackrayFound = serviceSourceAdd.IsThackrayFound.ToBool(),
                IsViewed = serviceSourceAdd.IsViewed.ToBool(),
                OriginalLocation = serviceSourceAdd.OriginalLocation,
                SourceDateStr = serviceSourceAdd.SourceDateStr,
                SourceDateStrTo = serviceSourceAdd.SourceDateStrTo,
                SourceDesc = serviceSourceAdd.SourceDesc,
                SourceRef = serviceSourceAdd.SourceRef,
                SourceNotes = serviceSourceAdd.SourceNotes,
                SourceFileCount = serviceSourceAdd.SourceFileCount.ToInt32(),
                SourceId = serviceSourceAdd.SourceId.ToGuid(),
                Files = tp,
                Parishs = serviceSourceAdd.Parishs.ParseToGuidList(),
                SourceTypes = serviceSourceAdd.SourceTypes.ParseToIntList()
            };


            var sourceSearch = new SourceSearch(new Security(new WebUser()));

            try
            {
                sourceSearch.Update(ssobj, new SourceValidator(ssobj));

            }

            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(ssobj.SourceId.ToString());

        }


    }
}
