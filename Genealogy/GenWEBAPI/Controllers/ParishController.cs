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

        public const string DeleteParishs = "/Delete";
        public const string GetParishsFromLocations = "/GetParishsFromLocations?0={parishLocation}";

        public const string GetParish = "/GetParish?0={parishId}";
        public const string GetParishDetails = "/GetParishDetails?0={parishId}";

        public const string GetSearchResults = "/GetSearchResults?0={parishIds}&1={startYear}&2={endYear}";

        public const string GetParishsTypes = "/GetParishsTypes";

        

        public const string AddParish = "/Add";

        public const string GetParishCounters = "/GetParishCounters?0={startYear}&1={endYear}";

        public const string GetParishNames = "/GetParishNames?0={parishIds}";

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
        public IHttpActionResult GetParishs(string deposited, string name, string county, string pageNumber, string pageSize, string sortCol)
        {
            var psf = new ParishSearchFilter
            {
                County = county,
                Deposited = deposited,
                Name = name
            };

            ServiceParishObject result = _parishSearch.StandardSearch(psf, new DataShaping() { RecordPageSize = pageSize.ToInt32(), RecordStart = pageNumber.ToInt32() });

            

            return Ok(result);
        }

        public string DeleteParishs(string parishIds)
        {
            var retVal = "";
            try
            {
                retVal = _parishSearch.Delete(parishIds);
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;

            }

            return WebHelper.MakeReturn(parishIds, retVal);
        }

        public List<string> GetParishNames(string parishIds)
        {
            return _parishSearch.GetParishNames(new ParishSearchFilter
            {
                ParishIds = parishIds.ParseToGuidList()
            });
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



            var sp = new ServiceParish
            {
                ParishId = ParishId.ToGuid(),
                ParishStartYear = ParishStartYear.ToInt32(),
                ParishDeposited = ParishDeposited,
                ParishEndYear = ParishEndYear.ToInt32(),
                ParishLat = ParishLat.ToDouble(),
                ParishLong = ParishLong.ToDouble(),
                ParishName = ParishName,
                ParishNote = ParishNote,
                ParishParent = ParishParent,
                ParishCounty = ParishCounty
            };

            try
            {
                _parishSearch.AddParish(sp, new ParishValidator { ServiceParish = sp });
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            return WebHelper.MakeReturn(sp.ParishId.ToString(), retVal);
        }



        public ServiceParish GetParish(string parishId)
        {
            var parish = new ServiceParish();
            string retVal = "";
            try
            {
                parish = _parishSearch.GetParish(parishId.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                parish.ErrorStatus = retVal;
            }
            return parish;
        }


        public List<ServiceSuperParish> GetParishsFromLocations(string parishLocation)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            _logSearch.WriteLog("GetParishsFromLocations: " + parishLocation, 1, WebHelper.GetRequestIp(), null);

            var results = new List<ServiceSuperParish>();

            try
            {
                results = _mapDataSources.GetParishsFromLocations(new ParishSearchFilter { Location = parishLocation }); ;
            }
            catch (Exception exception)
            {
                _logSearch.WriteLog("GetParishsFromLocations", 1, WebHelper.GetRequestIp(), exception);
            }


            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            _logSearch.WriteLog("GetParishsFromLocations finished in " + ts.TotalMilliseconds, 1, WebHelper.GetRequestIp(), null);
            return results;
        }

        public List<ServiceParishDataType> GetParishTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            _logSearch.WriteLog("GetParishTypes", 1, WebHelper.GetRequestIp(), null);

            var results = new List<ServiceParishDataType>();

            try
            {
                results = _mapDataSources.GetParishTypes();
            }
            catch (Exception exception)
            {
                _logSearch.WriteLog("GetParishTypes", 1, WebHelper.GetRequestIp(), exception);
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            _logSearch.WriteLog("GetParishTypes finished in " + ts.TotalMilliseconds, 1, WebHelper.GetRequestIp(), null);

            return results;
        }

        public List<ServiceSearchResult> GetSearchResults(string parishIds, string startYear, string endYear)
        {
            return _mapDataSources.GetSearchResults(new ParishSearchFilter
            {
                ParishIds = parishIds.ParseToGuidList(),
                DateFrom = startYear.ToInt32(),
                DateTo = endYear.ToInt32()
            });
        }

        public List<ServiceParishCounter> GetParishCounters(string startYear, string endYear)
        {
            return _mapDataSources.GetParishCounters(new ParishSearchFilter
            {
                DateFrom = startYear.ToInt32(),
                DateTo = endYear.ToInt32()
            });
        }


        public ServiceParishDetailObject GetParishDetail(string parishId)
        {
            _logSearch.WriteLog("GetParishDetail", 1, WebHelper.GetRequestIp(), null);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var serviceParishDetailObject = new ServiceParishDetailObject();
            string retVal = "";

            try
            {
                serviceParishDetailObject = _mapDataSources.GetParishDetail(new ParishSearchFilter { ParishIds = new List<Guid> { parishId.ToGuid() } });
            }
            catch (Exception ex1)
            {
                _logSearch.WriteLog("GetParishDetail", 1, WebHelper.GetRequestIp(), ex1);
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;

                serviceParishDetailObject.ErrorStatus = retVal;
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            _logSearch.WriteLog("GetParishDetail finished in " + ts.TotalMilliseconds, 1, WebHelper.GetRequestIp(), null);
            return serviceParishDetailObject;
        }




    }
}
