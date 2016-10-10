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
using TDBCore.Interfaces;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenWEBAPI.Controllers
{
    public static class BatchMappings
    {
        public const string GetBatches = "batches";//"/GetBatches?0={batch_ref}&1={page_number}&2={page_size}&3={sort_col}";

        public const string AddPersons = "batchperson";//"/AddPersons";

        public const string AddMarriages = "batchmarriage";// "/AddMarriages";

        public const string AddParishs = "batchparishs";// "/AddParishs";

        public const string AddSources = "batchsources";// "/AddSources";

        public const string RemoveBatch = "batch/delete";//"/RemoveBatch";
    }

    public class BatchController : ApiController
    {
        private readonly BatchSearch _batchSearch;

        public BatchController(ISourceTypesDal isSourceTypesDal,
                            ISecurity iSecurity,
                            IParishsDal parishsDal,
                            ISourceMappingsDal sourceMappingDal,
                            IPersonDal personDal,
                            ISourceDal sourceDal,
                            ISourceMappingParishsDal sourceMappingParishDal,
                            ISourceMappingsDal sourceMappingsDal,
                            IMarriagesDal marriagesDal,
                            IMarriageWitnessesDal mwits,
                            IBatchDal iBatch)
        {

            _batchSearch = new BatchSearch(new Security(new WebUser()),
                iBatch, parishsDal, sourceMappingDal, personDal, sourceDal, sourceMappingParishDal, sourceMappingsDal, marriagesDal, mwits);

        }

        [Route(BatchMappings.AddPersons)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult InsertPersons(string sheetUrl, string batchRef)
        {
            var newBatch = Guid.Empty;
            string retVal = "";

            try
            {
                newBatch = _batchSearch.ImportPersonCSVFromGoogle(sheetUrl, batchRef);
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(newBatch);
        }

        [Route(BatchMappings.AddMarriages)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult InsertMarriages(string sheetUrl, string batchRef)
        {
            var newBatch = Guid.Empty;
            string retVal = "";

            try
            {
                newBatch = _batchSearch.ImportMarriageCSVFromGoogle(sheetUrl, batchRef);
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(newBatch);
        }

        [Route(BatchMappings.AddSources)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult InsertSources(string sheetUrl, string batchRef)
        {
            var newBatch = Guid.Empty;
            string retVal = "";

            try
            {
                newBatch = _batchSearch.ImportSourceCSVFromGoogle(sheetUrl, batchRef);
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(newBatch);
        }

        [Route(BatchMappings.AddParishs)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult InsertParishs(string sheetUrl, string batchRef)
        {
            var newBatch = Guid.Empty;
            string retVal = "";

            try
            {
                newBatch = _batchSearch.ImportParishFromGoogle(sheetUrl);
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(newBatch);
        }

        [Route(BatchMappings.RemoveBatch)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult RemoveBatch(string batchId)
        {
            string retVal = "";
            bool success = true;

            try
            {
                success = _batchSearch.RemoveBatch(batchId.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(success);
        }

        [Route(BatchMappings.GetBatches)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetBatch(string batchRef, string pageNumber, string pageSize, string sortCol)
        {
            string retVal = "";
            var serviceBatchObject = new ServiceBatchObject();

            var batchSearchFilter = new BatchSearchFilter();

            var validator = new BatchValidator();

            batchSearchFilter.Ref = batchRef;

            try
            {
                serviceBatchObject = _batchSearch.Search(batchSearchFilter, new DataShaping() { RecordPageSize = pageSize.ToInt32(), RecordStart = pageNumber.ToInt32() }, validator);
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(serviceBatchObject);
        }
    }
}
