using System;
using System.Collections.Generic;
using System.Linq;
 
using TDBCore.BLL;
  
using TDBCore.Types.DTOs;
 
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace TDBCore.Types.domain
{
    public class ParishSearch  
    {
        readonly ParishsDal _parishsDal = new ParishsDal();
     
        private readonly ISecurity _iSecurity;
        private IValidator _validator = new Validator();
  
        public ParishSearch(ISecurity iSecurity)
        {
            _iSecurity = iSecurity;
        }

        public string Delete(string parishIds)
        {
            if (!_iSecurity.IsValidDelete()) return "No delete permission";

            _parishsDal.DeleteParishs(parishIds.ParseToGuidList());
 
            return parishIds;
        }

        public ServiceParishObject StandardSearch(ParishSearchFilter parishSearchFilter,DataShaping shaper, IValidator validator = null)
        {

            if (validator != null)
                _validator = validator;

         
            var serviceParishObject = new ServiceParishObject();

            string retVal = "";

            try
            {
                 
                serviceParishObject.serviceParishs = _parishsDal.GetParishByFilter(parishSearchFilter);



                serviceParishObject.Batch = shaper.RecordStart;
                serviceParishObject.BatchLength = shaper.RecordPageSize;
                serviceParishObject.Total = serviceParishObject.serviceParishs.Count;

                serviceParishObject.serviceParishs = serviceParishObject.serviceParishs.Skip(shaper.RecordStart * shaper.RecordPageSize).Take(shaper.RecordPageSize).ToList();
 
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
             
                serviceParishObject.ErrorStatus = retVal;
            }

            return serviceParishObject;
        }

        public List<string> GetParishNames(ParishSearchFilter parishSearchFilter, IValidator validator = null)
        {
            if (parishSearchFilter.ParishIds.IsNullOrBelowMinSize()) return new List<string>();

            return _parishsDal.GetParishNames(parishSearchFilter.ParishIds);
        }



        public ServiceParish GetParish(Guid parishId)
        {


            return _parishsDal.GetParishById(parishId);
        }

        public void AddParish(ServiceParish serviceParish, IValidator iValidator = null)
        {

            if (iValidator != null)
                _validator = iValidator;

            if (!_validator.ValidEntry()) return;
 
            if (serviceParish.ParishId == Guid.Empty)
            {
                Edit(serviceParish);
            }
            else
            {
                Insert(serviceParish);
            }


        }

        public void Edit(ServiceParish serviceParish)
        {
            if (!_iSecurity.IsValidEdit()) return;

            _parishsDal.UpdateParish(serviceParish);

        }

        public void Insert(ServiceParish serviceParish)
        {
            if (!_iSecurity.IsValidInsert()) return;

            _parishsDal.InsertParish(serviceParish);
        }
    }
}
