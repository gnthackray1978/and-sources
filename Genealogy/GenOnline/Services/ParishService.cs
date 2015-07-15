using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.Threading;
using System.Web;
using GenOnline.Helpers;
using GenOnline.Services.Contracts;
using TDBCore.BLL;
using TDBCore.Interfaces;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenOnline.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ParishService : IParishService
    {
        private readonly ParishSearch _parishSearch;
        private readonly MapDataSources _mapDataSources;
        private readonly TDBCore.Types.domain.LogSearch _logSearch;

        public ParishService(ISecurity iSecurity, IParishsDal iParishsDal, ILogDal iLogDal)
        {
            _logSearch = new TDBCore.Types.domain.LogSearch(iSecurity,iLogDal);
            _parishSearch = new ParishSearch(iSecurity,iParishsDal);
            _mapDataSources = new MapDataSources(new NoSecurity());
        }


        public List<CensusPlace> Get1841CensusPlaces()
        {                 
            return _mapDataSources.Get1841CensusPlaces();
        }
        // parishs
        public ServiceParishObject GetParishs(string deposited, string name, string county, string page_number, string page_size, string sort_col)
        {          
            var psf =new ParishSearchFilter
            {
                County = county,
                Deposited = deposited,
                Name = name
            };

            return _parishSearch.StandardSearch(psf, new DataShaping() { RecordPageSize = page_size.ToInt32(), RecordStart = page_number.ToInt32() });
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

            _logSearch.WriteLog("GetParishsFromLocations: " + parishLocation, 1, WebHelper.GetRequestIp(),null);

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