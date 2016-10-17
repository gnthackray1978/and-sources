using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using GenOnline.Helpers;
using TDBCore.BLL;
using TDBCore.Interfaces;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenWEBAPI.Controllers
{
    public static class UriParishMappings
    {
        //person


        public const string Get1841Census = "1841census";
        public const string GetParishs = "parishs";

        public const string DeleteParishs = "parishs/delete";
        public const string GetParishsFromLocations = "parishlocations"; //"/GetParishsFromLocations?0={parishLocation}";

        public const string GetParish = "parish";// "/GetParish?0={parishId}";
        public const string GetParishDetails = "parishdetail";//"/GetParishDetails?0={parishId}";


        public const string GetSearchResults = "parishpresence";//"/GetSearchResults?0={parishIds}&1={startYear}&2={endYear}";

        public const string GetParishsTypes = "parishtypes";
       
        public const string AddParish = "parish";

        public const string GetParishCounters = "parishcounter";// "/GetParishCounters?0={startYear}&1={endYear}";

        public const string GetParishNames = "parishnames";// "/GetParishNames?0={parishIds}";

    }


    public class ParishController : ApiController
    {
        private readonly ParishSearch _parishSearch;
        private readonly MapDataSources _mapDataSources;
        private readonly TDBCore.Types.domain.LogSearch _logSearch;


        public ParishController(ISecurity iSecurity, IParishsDal iParishsDal, ILogDal iLogDal)
        {
            _logSearch = new TDBCore.Types.domain.LogSearch(iSecurity, iLogDal);
            _parishSearch = new ParishSearch(iSecurity, iParishsDal);
            _mapDataSources = new MapDataSources(new NoSecurity());
        }

        [Route(UriParishMappings.Get1841Census)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Get1841CensusPlaces()
        {
            List<CensusPlace> censusData = new List<CensusPlace>();

            string retVal = "";

            try
            {
                censusData = _mapDataSources.Get1841CensusPlaces();
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }
           
            return Ok(censusData);             
        }


        [Route(UriParishMappings.GetParishs)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // parishs
        public IHttpActionResult GetParishs(string deposited, string name, string county, string pno, string psize, string sortcol)
        {
            //string pno, string psize, string sortcol = ""
            string retVal = "";

            var psf = new ParishSearchFilter
            {
                County = county ?? "",
                Deposited = deposited?? "",
                Name = name??""
            };

            var result = new ServiceParishObject();

            try
            {
                result = _parishSearch.StandardSearch(psf, new DataShaping() { RecordPageSize = psize.ToInt32(), RecordStart = pno.ToInt32(), Column = sortcol });
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(result);
        }


        [Route(UriParishMappings.DeleteParishs)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteParishs([FromBody] string parishIds)
        {
            var retVal = "";
            var errorMessage = "";

            try
            {
                retVal = _parishSearch.Delete(parishIds);
            }
            catch (Exception ex1)
            {
                errorMessage = ex1.Message;
            }

            if (errorMessage != "")
            {
                return Content(HttpStatusCode.BadRequest, errorMessage);
            }

            return Ok(retVal);
        }

        [Route(UriParishMappings.GetParishNames)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetParishNames(string parishIds)
        {
            var parishs = new List<string>();

            string retVal = "";

            try
            {
                var psf = new ParishSearchFilter()
                {
                    ParishIds = parishIds.ParseToGuidList()
                };

                parishs = _parishSearch.GetParishNames(psf);
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(parishs);
        }

        [Route(UriParishMappings.AddParish)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AddParish(ServiceParishAdd serviceParishAdd)
        {

       

            string retVal = "";



            var sp = new ServiceParish
            {
                ParishId = serviceParishAdd.ParishId.ToGuid(),
                ParishStartYear = serviceParishAdd.ParishStartYear.ToInt32(),
                ParishDeposited = serviceParishAdd.ParishDeposited ?? "",
                ParishEndYear = serviceParishAdd.ParishEndYear.ToInt32(),
                ParishLat = serviceParishAdd.ParishLat.ToDouble(),
                ParishLong = serviceParishAdd.ParishLong.ToDouble(),
                ParishName = serviceParishAdd.ParishName ?? "",
                ParishNote = serviceParishAdd.ParishNote ??"",
                ParishParent = serviceParishAdd.ParishParent??"",
                ParishCounty = serviceParishAdd.ParishCounty ??""
            };

            try
            {
                _parishSearch.AddParish(sp, new ParishValidator { ServiceParish = sp });
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(sp.ParishId);
        }


        [Route(UriParishMappings.GetParish)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetParish(string id)
        {
            var parish = new ServiceParish();
            string retVal = "";
            try
            {
                parish = _parishSearch.GetParish(id.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(parish.ParishId);
        }


        [Route(UriParishMappings.GetParishsFromLocations)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetParishsFromLocations(string parishLocation)
        {
            string retVal = "";

            var results = new List<ServiceSuperParish>();

            try
            {
                results = _mapDataSources.GetParishsFromLocations(new ParishSearchFilter { Location = parishLocation }); ;
            }
            catch (Exception exception)
            {
                // _logSearch.WriteLog("GetParishsFromLocations", 1, WebHelper.GetRequestIp(), exception);
                retVal = "Exception: " + exception.Message;
            }
             
            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(results);
        }

        [Route(UriParishMappings.GetParishsTypes)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetParishTypes()
        {
            //  Stopwatch stopWatch = new Stopwatch();
            //  stopWatch.Start();

            //  _logSearch.WriteLog("GetParishTypes", 1, WebHelper.GetRequestIp(), null);
            string retVal = "";
            var results = new List<ServiceParishDataType>();

            try
            {
                results = _mapDataSources.GetParishTypes();
            }
            catch (Exception exception)
            {
                //    _logSearch.WriteLog("GetParishTypes", 1, WebHelper.GetRequestIp(), exception);
                retVal = "Exception: " + exception.Message;
            }

            //  stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            //   TimeSpan ts = stopWatch.Elapsed;

            //   _logSearch.WriteLog("GetParishTypes finished in " + ts.TotalMilliseconds, 1, WebHelper.GetRequestIp(), null);

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(results);
        }

        [Route(UriParishMappings.GetSearchResults)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetSearchResults(string parishIds, string startYear, string endYear)
        {
            string retVal = "";

            List<ServiceSearchResult> results = new List<ServiceSearchResult>();

            try
            {
                results = _mapDataSources.GetSearchResults(new ParishSearchFilter
                {
                    ParishIds = parishIds.ParseToGuidList(),
                    DateFrom = startYear.ToInt32(),
                    DateTo = endYear.ToInt32()
                });
            }
            catch (Exception exception)
            {
                //    _logSearch.WriteLog("GetParishTypes", 1, WebHelper.GetRequestIp(), exception);
                retVal = "Exception: " + exception.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(results);
        }

        [Route(UriParishMappings.GetParishCounters)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetParishCounters(string startYear, string endYear)
        {
            string retVal = "";

            var results = new List<ServiceParishCounter>();

            try
            {
                results = _mapDataSources.GetParishCounters(new ParishSearchFilter
                {
                    DateFrom = startYear.ToInt32(),
                    DateTo = endYear.ToInt32()
                });

            }
            catch (Exception exception)
            {
                //    _logSearch.WriteLog("GetParishTypes", 1, WebHelper.GetRequestIp(), exception);
                retVal = "Exception: " + exception.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(results);

        }

        [Route(UriParishMappings.GetParishDetails)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetParishDetail(string parishId)
        {
            //_logSearch.WriteLog("GetParishDetail", 1, WebHelper.GetRequestIp(), null);
           // Stopwatch stopWatch = new Stopwatch();
           // stopWatch.Start();

            var serviceParishDetailObject = new ServiceParishDetailObject();
            string retVal = "";

            try
            {
                serviceParishDetailObject = _mapDataSources.GetParishDetail(new ParishSearchFilter { ParishIds = new List<Guid> { parishId.ToGuid() } });
            }
            catch (Exception ex1)
            {
               // _logSearch.WriteLog("GetParishDetail", 1, WebHelper.GetRequestIp(), ex1);
                retVal = "Exception: " + ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }
            // stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            //TimeSpan ts = stopWatch.Elapsed;

            // _logSearch.WriteLog("GetParishDetail finished in " + ts.TotalMilliseconds, 1, WebHelper.GetRequestIp(), null);
            return Ok(serviceParishDetailObject);
        }




    }
}
