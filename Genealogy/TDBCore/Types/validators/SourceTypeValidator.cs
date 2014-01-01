using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.Types.DTOs;

namespace TDBCore.Types.validators
{
    public class SourceTypeValidator : IValidator
    {
        private readonly ServiceSourceType _serviceSourceType;


        bool _isValidDescription;

        private string _errorString;

        public SourceTypeValidator(ServiceSourceType serviceFullSource)
        {
            _serviceSourceType = serviceFullSource;

            Validate();
        }

        public bool ValidDescription
        {
            get { return _isValidDescription; }
        }


        public bool ValidEntry()
        {
            if (ValidDescription)
            {
                _errorString = "";
                return true;
            }

            _errorString = "";

            if (!ValidDescription)
                _errorString += " Invalid Description";
           

            return false;
        }

        public string GetErrors()
        {
            return _errorString;
        }

        private void Validate()
        {
            _isValidDescription = _serviceSourceType.Description.Length > 0 &&
                                  _serviceSourceType.Description.Length < 250;
        
        }



    }
}
