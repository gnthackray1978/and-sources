using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using GenOnline.Helpers;
using GenOnline.Services.Contracts;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenOnline.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ParishService : IParishService
    {
        public List<CensusPlace> Get1841CensusPlaces()
        {
       
            var mapDataSources = new MapDataSources(new NoSecurity());

            return mapDataSources.Get1841CensusPlaces();
        }
        // parishs
        public ServiceParishObject GetParishs(string deposited, string name, string county, string page_number, string page_size, string sort_col)
        {
            var parishSearch = new ParishSearch(new Security(WebHelper.GetUser()));
          
            var psf =new ParishSearchFilter
            {
                County = county,
                Deposited = deposited,
                Name = name
            };

 

            return parishSearch.StandardSearch(psf, new DataShaping(){RecordPageSize = page_size.ToInt32() , RecordStart = page_number.ToInt32()});
        }

        public string DeleteParishs(string parishIds)
        {

         
            string retVal = "";
            var parishSearch = new ParishSearch(new Security(WebHelper.GetUser()));
            
            try
            {
                retVal = parishSearch.Delete(parishIds);
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
            var parishSearch = new ParishSearch(new Security(WebHelper.GetUser()));
      
            return parishSearch.GetParishNames(new ParishSearchFilter
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

            var pe = new ParishSearch(new Security(WebHelper.GetUser()));



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
                pe.AddParish(sp, new ParishValidator { ServiceParish = sp });
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
                var pe = new ParishSearch(new Security(WebHelper.GetUser()));
                parish = pe.GetParish(parishId.ToGuid());
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
            var mapDataSources = new MapDataSources(new Security(WebHelper.GetUser()));
                        
            return mapDataSources.GetParishsFromLocations(new ParishSearchFilter { Location = parishLocation });
        }

        public List<ServiceParishDataType> GetParishTypes()
        {
            var mapDataSources = new MapDataSources(new NoSecurity());

            return mapDataSources.GetParishTypes();
        }

        public List<ServiceSearchResult> GetSearchResults(string parishIds, string startYear, string endYear)
        {
            var mapDataSources = new MapDataSources(new Security(WebHelper.GetUser()));
                       
            return mapDataSources.GetSearchResults(new ParishSearchFilter
            {
                ParishIds = parishIds.ParseToGuidList(),
                DateFrom = startYear.ToInt32(),
                DateTo = endYear.ToInt32()
            });
        }

        public List<ServiceParishCounter> GetParishCounters(string startYear, string endYear)
        {
            var mapDataSources = new MapDataSources(new Security(WebHelper.GetUser()));
              
            return mapDataSources.GetParishCounters(new ParishSearchFilter
            {
                DateFrom = startYear.ToInt32(),
                DateTo = endYear.ToInt32()
            });
        }


        public ServiceParishDetailObject GetParishDetail(string parishId)
        {
            var parishSearch = new MapDataSources(new Security(WebHelper.GetUser()));
            var serviceParishDetailObject = new ServiceParishDetailObject();
            string retVal = "";

            try
            {  
                serviceParishDetailObject = parishSearch.GetParishDetail(new ParishSearchFilter { ParishIds = new List<Guid> { parishId.ToGuid() } });
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
    }
}