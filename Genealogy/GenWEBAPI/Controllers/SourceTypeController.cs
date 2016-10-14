using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using GenOnline;
using GenOnline.Helpers;
using TDBCore.BLL;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenWEBAPI.Controllers
{
    public static class UriSourceTypesMappings
    {
        //source types

        public const string GetJsonSourceTypes = "sourcetypes";//"/Select?0={description}&1={page_number}&2={page_size}&3={sort_col}";

        public const string GetSourceType = "sourcetype";// "/Id?0={TypeId}";

        public const string DeleteSourceTypes = "sourcetypes/delete";//"/Delete";

        public const string AddSourceType = "sourcetype";// "/Add";

        public const string GetSourceTypeNames = "sourcetypenames"; //"/GetNames?0={TypeIds}";
    }

    public class ServiceTypeAdd
    {
        //string typeId, string description, string order

        public string TypeId { get; set; }

        public string Description { get; set; }

        public string Order { get; set; }



    }

    public class SourceTypeController : ApiController
    {

        private readonly SourceTypeSearch _sourceTypeSearch;

        // source types

        public SourceTypeController(ISourceTypesDal isSourceTypesDal,
            ISecurity iSecurity)
        {
            _sourceTypeSearch = new SourceTypeSearch(new Security(new WebUser()), isSourceTypesDal);
        }


        [Route(UriSourceTypesMappings.GetSourceTypeNames)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetSourceTypeNames(string typeIds)
        {
            string retVal = "";

            var sourceTypeList = new List<string>();

            var sourceTypeSearchFilter = new SourceTypeSearchFilter()
            {
                SourceTypeIds = typeIds.ParseToIntList()
            };

            try
            {
                sourceTypeList = _sourceTypeSearch.Search(sourceTypeSearchFilter, new DataShaping(), new SourceTypeSearchValidator(sourceTypeSearchFilter)).serviceSources.Select(p => p.Description).ToList();
            }

            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(sourceTypeList);
        }


        [Route(UriSourceTypesMappings.GetSourceType)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetSourceType(string typeId)
        {

            string retVal = "";

            var serviceSourceType = new ServiceSourceType() { TypeId = typeId.ToInt32() };
           
            try
            {
                serviceSourceType = _sourceTypeSearch.Get(serviceSourceType);
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }


            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(serviceSourceType);
        }


        [Route(UriSourceTypesMappings.GetJsonSourceTypes)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetSourceTypes(string description, string pageNumber, string pageSize, string sortCol)
        {

            string retVal = "";
            var serviceSourceTypeObject = new ServiceSourceTypeObject();

            var sourceTypeSearchFilter = new SourceTypeSearchFilter()
            {
                Description = description == "" ? "%" : description
            };

            var validator = new SourceTypeSearchValidator(sourceTypeSearchFilter);

            try
            {
                serviceSourceTypeObject = _sourceTypeSearch.Search(sourceTypeSearchFilter, new DataShaping() { RecordPageSize = pageSize.ToInt32(), RecordStart = pageNumber.ToInt32() }, validator);
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            
            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(serviceSourceTypeObject);
        }

        [Route(UriSourceTypesMappings.DeleteSourceTypes)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteSourceTypes([FromBody]string sourceIds)
        {
            string retVal = "";

            try
            {
                 _sourceTypeSearch.DeleteRecords(new SourceTypeSearchFilter()
                {
                    SourceTypeIds = sourceIds.ParseToIntList()
                });
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

        [Route(UriSourceTypesMappings.AddSourceType)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AddSourceType(ServiceTypeAdd serviceTypeAdd)
        {

            string retVal = "";

            var serviceSourceType = new ServiceSourceType()
            {
                TypeId = serviceTypeAdd.TypeId.ToInt32(),
                Description = serviceTypeAdd.Description,
                Order = serviceTypeAdd.Order.ToInt32()
            };

            var sourceTypeValidator = new SourceTypeValidator(serviceSourceType);

            try
            {
                _sourceTypeSearch.Update(serviceSourceType, sourceTypeValidator);
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }


            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            //return WebHelper.MakeReturn(serviceSourceType.TypeId.ToString(), retVal);

            return Ok(true);
        }


    }
}
