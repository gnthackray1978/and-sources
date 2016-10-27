using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class SourceTypeService : ISourceTypes
    {

        private readonly SourceTypeSearch _sourceTypeSearch;

        // source types

        public SourceTypeService(ISourceTypesDal isSourceTypesDal,
            ISecurity iSecurity)
        {
            _sourceTypeSearch = new SourceTypeSearch(new Security(new WebUser()),isSourceTypesDal);
        }

        public List<string> GetSourceTypeNames(string typeIds)
        {             
            var sourceTypeSearchFilter = new SourceTypeSearchFilter()
            {
                SourceTypeIds = typeIds.ParseToIntList()
            };

            return _sourceTypeSearch.Search(sourceTypeSearchFilter, new DataShaping(), new SourceTypeSearchValidator(sourceTypeSearchFilter)).serviceSources.Select(p => p.Description).ToList();           
        }

        public ServiceSourceType GetSourceType(string TypeId)
        {
      
            string retVal = "";

            var serviceSourceType = new ServiceSourceType() { TypeId = TypeId.ToInt32() };
            
            try
            {

                serviceSourceType = _sourceTypeSearch.Get(serviceSourceType);

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
               
                serviceSourceType.ErrorStatus = retVal;
            }


            return serviceSourceType;
        }

        public ServiceSourceTypeObject GetSourceTypes(string description, string page_number, string page_size, string sort_col)
        {
           
            string retVal = "";
            var serviceSourceTypeObject = new ServiceSourceTypeObject();

            var sourceTypeSearchFilter = new SourceTypeSearchFilter()
            {
                Description =  description == "" ? "%" : description
            };

            var validator = new SourceTypeSearchValidator(sourceTypeSearchFilter);
            
            try
            {
                serviceSourceTypeObject = _sourceTypeSearch.Search(sourceTypeSearchFilter, new DataShaping() { RecordPageSize = page_size.ToInt32(), RecordStart = page_number.ToInt32() }, validator);           
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += validator.GetErrors();
          
            }


            return serviceSourceTypeObject;
        }

        public string DeleteSourceTypes(string sourceIds)
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
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
             
            }


            return WebHelper.MakeReturn(sourceIds, retVal);
        }

        public string AddSourceType(string TypeId, string Description, string Order)
        {

            string retVal = "";

            var serviceSourceType = new ServiceSourceType()
                {
                    TypeId = TypeId.ToInt32(),
                    Description = Description,
                    Order = Order.ToInt32()
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
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += sourceTypeValidator.GetErrors();
                serviceSourceType.ErrorStatus = retVal;
            }




            return WebHelper.MakeReturn(serviceSourceType.TypeId.ToString(), retVal);
        }

    }
}