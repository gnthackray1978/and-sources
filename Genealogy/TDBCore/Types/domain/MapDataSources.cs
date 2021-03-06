﻿using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.BLL;
using TDBCore.Interfaces;
using TDBCore.Types.DTOs;
 
using TDBCore.Types.enums;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;

namespace TDBCore.Types.domain
{
    public class MapDataSources 
    {
        readonly ISourceDal _sourceDal = new SourceDal();
        readonly ISourceMappingsDal _sourceMappingsDal = new SourceMappingsDal();
        readonly IPersonDal _personDal = new PersonDal();   
        readonly IParishsDal _parishsDal = new ParishsDal();
        readonly ISecurity _security = new NoSecurity();
        readonly IMarriagesDal _iMarriagesDal = new MarriagesDal();
        readonly IMarriageWitnessesDal _marriageWitnessesDal = new MarriageWitnessesDal(); 

        readonly MarriageSearch _marriageSearch;
        readonly PersonSearch _personSearch;

        public MapDataSources(ISecurity security)
        {
            _security = security;

            _marriageSearch = new MarriageSearch(new NoSecurity(), _iMarriagesDal, _marriageWitnessesDal, _sourceDal, _sourceMappingsDal, _personDal);
            _personSearch = new PersonSearch(new NoSecurity());

        }

        public List<CensusPlace> Get1841CensusPlaces()
        {
            if (!_security.IsvalidSelect()) return new List<CensusPlace>();

            return _parishsDal.Get1841Census();


        }



        public List<ServiceSuperParish> GetParishsFromLocations(ParishSearchFilter parishSearchFilter)
        {

            if (!_security.IsvalidSelect()) return new List<ServiceSuperParish>();

            var parishs = _parishsDal.GetParishsByLocationString(parishSearchFilter.Location).Select(o => new ServiceSuperParish
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
             

            return parishs;
        }

        public List<ServiceParishDataType> GetParishTypes()
        {           
            if (!_security.IsvalidSelect()) return new List<ServiceParishDataType>();



            return _parishsDal.GetParishTypes().Select(o => new ServiceParishDataType { DataTypeId = o.dataTypeId, Description = o.description }).ToList();
        }

        public List<ServiceSearchResult> GetSearchResults(ParishSearchFilter parishSearchFilter)
        { //|| parishIds != "YORKSHIRE"
            var ssresults = new List<ServiceSearchResult>();


            if (!_security.IsvalidSelect()) return new List<ServiceSearchResult>();

            if (parishSearchFilter.ParishIds.Count > 0 && _security.IsvalidSelect())
            {
                _sourceDal.GetSourceByParishString(parishSearchFilter.ParishIds.ParseToCSV(), parishSearchFilter.DateFrom, parishSearchFilter.DateTo);
            }


            return ssresults;
        }

        public List<ServiceParishCounter> GetParishCounters(ParishSearchFilter parishSearchFilter)
        {

            if (!_security.IsvalidSelect()) return new List<ServiceParishCounter>();

            return _parishsDal.GetParishCounter().Where(p => p.YearStart >= parishSearchFilter.DateFrom && p.YearEnd <= parishSearchFilter.DateTo).
                                                Select(o => new ServiceParishCounter
                                                {
                                                    Counter = o.Count.GetValueOrDefault(),
                                                    EndYear = o.YearEnd.GetValueOrDefault(),
                                                    ParishId = o.ParishId.GetValueOrDefault(),
                                                    ParishName = o.ParishName,
                                                    PX = o.PX.GetValueOrDefault(),
                                                    PY = o.PY.GetValueOrDefault()
                                                }).ToList();
        }



        public ServiceParishDetailObject GetParishDetail(ParishSearchFilter parishSearchFilter)
        {           
            var serviceParishDetailObject = new ServiceParishDetailObject();

            if (!_security.IsvalidSelect()) return serviceParishDetailObject;

          
            var parishDetails = _parishsDal.GetParishDetail(parishSearchFilter.ParishIds.SafeFirst());
         
            serviceParishDetailObject.serviceParishRecords = parishDetails.parishRecords.Select(o => new ServiceParishRecord
            {
                DataType = o.dataType,
                EndYear = o.endYear,
                ParishId = o.parishId,
                ParishRecordType = o.parishRecordType,
                StartYear = o.startYear
            }).ToList();
          
            serviceParishDetailObject.serviceParishTranscripts = parishDetails.parishTranscripts.Select(o => new ServiceParishTranscript
            {
                ParishId = o.ParishId,
                ParishTranscriptRecord = o.ParishTranscriptRecord
            }).ToList();
          
            serviceParishDetailObject.serviceServiceMapDisplaySource = _sourceDal.GetParishSourceRecords(parishSearchFilter.ParishIds.SafeFirst()).Select(s => new ServiceMapDisplaySource
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

       
            var sourceStr =
                serviceParishDetailObject.serviceServiceMapDisplaySource.Select(p => p.SourceId).ToList().ParseToCSV();
          
            serviceParishDetailObject.PersonCount = GetPersonsCount("", "", "thac", "", "", "", "", "", "", "1400", "1950", "false", "false", "false", sourceStr, "", "");
       
            serviceParishDetailObject.MarriageCount = GetMarriagesCount("", "", "", "", "", "", "0", "0", sourceStr);

      
            return serviceParishDetailObject;
        }





        private int GetPersonsCount(string parentId, string christianName, string surname, string fatherChristianName,
                string fatherSurname, string motherChristianName, string motherSurname, string location, string county,
                string lowerDate, string upperDate, string filterTreeResults, string filterIncludeBirths,
                string filterIncludeDeaths, string filterSource, string spouse,
                string parishFilter)
        {

            var spo = new ServicePersonObject();

            

            string retVal = "";

            try
            {

              
                if (parentId.ToGuid() == Guid.Empty)
                {

                     
                   

                    var personSearchFilter = new PersonSearchFilter
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



                    spo = _personSearch.Search(PersonSearchTypes.Simple, personSearchFilter, new DataShaping());


                }
                else
                {
                    spo = _personSearch.Search(PersonSearchTypes.Duplicates, new PersonSearchFilter() { ParentId = parentId.ToGuid() }, new DataShaping());
                }

                 
            }
            catch (Exception ex1)
            {
              
                spo.ErrorStatus += ex1.Message;
            }


            return spo.Total;
        }


        private int GetMarriagesCount(string uniqref, string malecname, string malesname, string femalecname, string femalesname,
            string location, string lowerDate, string upperDate, string sourceFilter)
        {
            
            var serviceMarriageObject = new ServiceMarriageObject();
           
            try
            {

                serviceMarriageObject = _marriageSearch.Search(MarriageFilterTypes.Standard, new MarriageSearchFilter
                    {
                        MaleCName = malecname,
                        MaleSName = malesname,
                        FemaleCName = femalecname,
                        FemaleSName = femalesname,
                        Location = location,
                        LowerDate = lowerDate.ToInt32(),
                        UpperDate = upperDate.ToInt32(),
                        Source = sourceFilter

                    }, new DataShaping() { Column = "MarriageDate" });


            }
            catch (Exception ex1)
            {           
                serviceMarriageObject.ErrorStatus = "Exception: " + ex1.Message;
            }
          

            return serviceMarriageObject.serviceMarriages.Count;
        }


    }
}
