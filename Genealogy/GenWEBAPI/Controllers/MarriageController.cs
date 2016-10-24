using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using GenOnline.Helpers;
using TDBCore.BLL;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.enums;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenWEBAPI.Controllers
{
    public static class UriMappingMarriage
    {
        //marriages
        public const string AddMarriage = "addmarriage";
        public const string GetMarriages = "marriages";
        public const string GetFilteredMarriages = "filteredmarriages";
        public const string GetMarriage = "marriage";
        public const string DeleteMarriages = "marriage/delete";
        public const string SetMarriageDuplicate = "marriages/createduplicate";
        public const string MergeMarriages = "marriages/mergemarriages";
        public const string RemoveMarriageLinks = "marriages/removelinks";
        public const string ReorderMarriages = "marriages/reorder";
        public const string SwitchSpouses = "marriages/switchspouses";

    }


    public class ServiceMarriageData
    {
        public ServiceMarriage serviceMarriage { get; set; }

        public string sources { get; set; }

        public List<ServiceWitness> marriageWitnesses { get; set; }
    }

    public class MarriageController : ApiController
    {
        private readonly MarriageSearch _marriageSearch;

        public MarriageController(IMarriagesDal iMarriagesDal,
            IMarriageWitnessesDal iMarriageWitnessesDal,
            ISourceDal iSourceDal,
            ISourceMappingsDal iSourceMappingsDal,
            IPersonDal iPersonDal,
            ISecurity iSecurity)
        {
            _marriageSearch = new MarriageSearch(iSecurity,
                iMarriagesDal,
                iMarriageWitnessesDal, iSourceDal, iSourceMappingsDal, iPersonDal);
        }


        [Route(UriMappingMarriage.GetMarriage)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetMarriage(string id)
        {

            // ahouls use search function here     
            string retVal = "";

            var serviceMarriage = new ServiceMarriage();

            try
            {

                serviceMarriage = _marriageSearch.Get(id.ToGuid());

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }


            serviceMarriage.ErrorStatus = retVal;

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            if (serviceMarriage.MarriageId == Guid.Empty)
            {
                return Content(HttpStatusCode.NotFound, id);
            }

            return Ok(serviceMarriage);
        }

        [Route(UriMappingMarriage.GetMarriages)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetMarriages(string ids)
        {
            
            var marriageFilter = new MarriageSearchFilter()
            {
                Ids = ids.ParseToGuidList()
            };
            var marriageValidation = new MarriageSearchValidator(marriageFilter);
            // ahouls use search function here     
            string retVal = "";

            var serviceMarriageObject = new ServiceMarriageObject();

            try
            {

                serviceMarriageObject = _marriageSearch.Search(MarriageFilterTypes.IdList, marriageFilter,
                                  new DataShaping()
                                  {
                                      Column = "",
                                      RecordPageSize = 25,
                                      RecordStart = 0
                                  }, marriageValidation);

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
 
            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            

            return Ok(serviceMarriageObject );
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.GetFilteredMarriages)]
        [HttpGet]
        public IHttpActionResult GetMarriages(string uniqref, string malecname, string malesname, string femalecname,
            string femalesname, string location, string lowerDate, string upperDate, string sourceFilter, string parishFilter, string marriageWitness,
            string pno, string psize, string sortcol = "")
        {
 


            var serviceMarriageObject = new ServiceMarriageObject();

            string retVal = "";

            try
            {

                Guid parentId = uniqref.ToGuid();


                if (parentId == Guid.Empty)
                {
                    var marriageFilter = new MarriageSearchFilter()
                    {
                        Ids = new List<Guid>(),
                        MaleCName = malecname ?? "",
                        MaleSName = malesname ?? "",
                        FemaleCName = femalecname ?? "",
                        FemaleSName = femalesname ?? "",
                        Location = location ?? "",
                        LowerDate = lowerDate.ToInt32(),
                        UpperDate = upperDate.ToInt32(),
                        Witness = marriageWitness ?? "",
                        Parish = parishFilter ?? "",
                        Source = sourceFilter ?? "",
                        ParentId = Guid.Empty
                    };

                    var marriageValidation = new MarriageSearchValidator(marriageFilter);


                    serviceMarriageObject = _marriageSearch.Search(MarriageFilterTypes.Standard, marriageFilter,
                                  new DataShaping()
                                  {
                                      Column = sortcol ?? "",
                                      RecordPageSize = psize.ToInt32(),
                                      RecordStart = pno.ToInt32()
                                  }, marriageValidation);


                }
                else
                {
                    serviceMarriageObject = _marriageSearch.Search(MarriageFilterTypes.Duplicates, new MarriageSearchFilter() { ParentId = parentId },
                                  new DataShaping()
                                  {
                                      Column = sortcol ?? "",
                                      RecordPageSize = psize.ToInt32(),
                                      RecordStart = 0
                                  });

                }

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
           
            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

           


            return Ok(serviceMarriageObject);
            
        }



        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.DeleteMarriages)]
        [HttpPost]
        public IHttpActionResult DeleteMarriages([FromBody]string marriageIds)
        {
            string retVal = "";

            try
            {
                _marriageSearch.DeleteRecords(marriageIds.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);
        }



        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.SetMarriageDuplicate)]
        [HttpPost]
        public IHttpActionResult SetMarriageDuplicate([FromBody]string marriages)
        {
            string retVal = "";

            try
            {
                _marriageSearch.SetSelectedDuplicateMarriage(marriages.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);

        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.RemoveMarriageLinks)]
        [HttpPost]
        public IHttpActionResult RemoveMarriageLink([FromBody]string marriage)
        {
            // var iModel = new MarriageSearch(new Security(new WebUser()));    
            string retVal = "";

            try
            {
                _marriageSearch.SetRemoveSelectedFromDuplicateList(marriage.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);

        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.ReorderMarriages)]
        [HttpPost]
        public IHttpActionResult ReorderMarriages([FromBody]string marriage)
        {
            //  var iModel = new MarriageSearch(new Security(new WebUser()));

            string retVal = "";

            try
            {
                _marriageSearch.SetReorderDupes(marriage.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);

        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.SwitchSpouses)]
        [HttpPost]
        public IHttpActionResult SwitchSpouses([FromBody]string marriage)
        {

            string retVal = "";

            try
            {
                _marriageSearch.SwitchSpouses(marriage.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.MergeMarriages)]
        [HttpPost]
        public IHttpActionResult MergeMarriage([FromBody]string marriage)
        {

            string retVal = "";

            try
            {
                _marriageSearch.SetMergeSources(marriage.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);
        }



        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.AddMarriage)]
        [HttpPost]        
        public IHttpActionResult AddMarriage(ServiceMarriageData marriageData)
        {

            ServiceMarriage serviceMarriage = marriageData.serviceMarriage;
            
            string retVal = "";
            
            try
            {
                var tp = MarriageWitness.FormatWitnessCollection(marriageData.marriageWitnesses, marriageData.serviceMarriage);

                _marriageSearch.Save(serviceMarriage, marriageData.sources.ParseToGuidList(), tp, new MarriageValidator(serviceMarriage));

            

            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }



            return Ok(serviceMarriage.MarriageId);

        }
    }
}