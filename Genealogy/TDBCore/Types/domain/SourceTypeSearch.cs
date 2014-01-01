using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.BLL;
 
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace TDBCore.Types.domain
{
    public class SourceTypeSearch //: EditorBaseModel<int> 
    {
        readonly SourceTypesBll _sourceTypesBll = new SourceTypesBll();
        
        readonly ISecurity _security = new NoSecurity();
     


        public SourceTypeSearch(ISecurity security)
        {
            _security = security;
        }

      

        public ServiceSourceTypeObject Search(SourceTypeSearchFilter sourceTypeSearchFilter,DataShaping shaper, SourceTypeSearchValidator validator =null)
        {
            if (!_security.IsvalidSelect()) return new ServiceSourceTypeObject();


            if (validator != null && !validator.ValidEntry()) return new ServiceSourceTypeObject();

            return _sourceTypesBll.GetSourceTypeByFilter(sourceTypeSearchFilter).ToServiceSourceTypeObject(shaper.Column, shaper.RecordPageSize, shaper.RecordStart);
        }

        public void DeleteRecords(SourceTypeSearchFilter sourceTypeSearchFilter)
        {
            if (!_security.IsValidDelete()) return;

            _sourceTypesBll.DeleteSourceTypes(sourceTypeSearchFilter.SourceTypeIds);
        }


        public ServiceSourceType Get(ServiceSourceType serviceSourceType)
        {
            if (!_security.IsvalidSelect()) return new ServiceSourceType();

            return _sourceTypesBll.GetSourceTypeById(serviceSourceType.TypeId);
        }

        private void Edit(ServiceSourceType serviceSourceType)
        {
            if (!_security.IsValidEdit()) return;

        
            _sourceTypesBll.UpdateSourceType(serviceSourceType);

        }

        private void Insert(ServiceSourceType serviceSourceType)
        {

            if (!_security.IsValidInsert()) return;

        
            _sourceTypesBll.InsertSourceType(serviceSourceType);

        }


        public void Update(ServiceSourceType serviceSourceType, SourceTypeValidator validator)
        {
            if (!validator.ValidEntry()) return;

            if (serviceSourceType.TypeId == 0)
            {
                Insert(serviceSourceType);
            }
            else
            {
                Edit(serviceSourceType);
            }

        }

    }
}
