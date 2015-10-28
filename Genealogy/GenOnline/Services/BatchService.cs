using GenOnline;
using GenOnline.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using TancWebApp.Services.Contracts;
using TDBCore.BLL;
using TDBCore.Interfaces;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace TancWebApp.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class BatchService : IBatch
    {

        private readonly BatchSearch _batchSearch;

        public BatchService(ISourceTypesDal isSourceTypesDal,
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


        public string InsertPersons(string sheetUrl)
        {
            var newBatch = Guid.Empty;
            
            try
            {
                newBatch = _batchSearch.ImportPersonCSVFromGoogle(sheetUrl);
            }
            catch (Exception e) {
                 WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                 WebOperationContext.Current.OutgoingResponse.StatusDescription = e.Message;
            }

            return WebHelper.MakeJSONReturn(newBatch.ToString(), newBatch != Guid.Empty);
        }

        public string InsertMarriages(string sheetUrl)
        {            
            var newBatch = Guid.Empty;
           
            try
            {
                newBatch = _batchSearch.ImportMarriageCSVFromGoogle(sheetUrl);
            }
            catch (Exception e) {
                 WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                 WebOperationContext.Current.OutgoingResponse.StatusDescription = e.Message;
            }

            return WebHelper.MakeJSONReturn(newBatch.ToString(), newBatch != Guid.Empty);
        }


        public string InsertSources(string sheetUrl)
        {
            var newBatch = Guid.Empty;
         

            try
            {
                newBatch = _batchSearch.ImportSourceCSVFromGoogle(sheetUrl);
            }
            catch (Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = e.Message;
            }

            return WebHelper.MakeJSONReturn(newBatch.ToString(), newBatch != Guid.Empty);
        }

        public string InsertParishs(string sheetUrl)
        {
            var newBatch = Guid.Empty;
            

            try
            {
                newBatch = _batchSearch.ImportParishFromGoogle(sheetUrl);
            }
            catch (Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = e.Message;
            }

            return WebHelper.MakeJSONReturn(newBatch.ToString(), newBatch != Guid.Empty );
        }

        public string RemoveBatch(string batchId)
        {
            try
            {
                _batchSearch.RemoveBatch(batchId.ToGuid());
            }
            catch (Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = e.Message;
            }

            return WebHelper.MakeJSONReturn(batchId.ToString(), true);
        }

        public ServiceBatchObject GetBatch(string batch_ref,string page_number, string page_size, string sort_col)
        {

            var serviceBatchObject = new ServiceBatchObject();

            var batchSearchFilter = new BatchSearchFilter();

            var validator = new BatchValidator();

            batchSearchFilter.Ref = batch_ref;

            try
            {
                serviceBatchObject = _batchSearch.Search(batchSearchFilter, new DataShaping() { RecordPageSize = page_size.ToInt32(), RecordStart = page_number.ToInt32() }, validator);
            }
            catch (Exception e)
            {
                if (WebOperationContext.Current == null) return serviceBatchObject;

                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = e.Message;
            }

            return serviceBatchObject;
        }
    }
}