using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.Types.DTOs;
using TDBCore.Types.libs;

namespace TDBCore.Types.validators
{
    public class SourceValidator : IValidator
    {
        private readonly SourceDto _sourceAjaxDto;
        private string _errorString;

        public SourceValidator(SourceDto sourceAjaxDto)
        {
            _sourceAjaxDto = sourceAjaxDto;

            Validate();
        }


        public bool ValidEntry()
        {
            if (IsValidSourceDate && IsValidSourceDateTo && IsValidSourceDescription)
            {
                _errorString = "";
                return true;
            }

            _errorString = "";

            if (!IsValidSourceDate)
                _errorString +=" Invalid Source Date";
            if (!IsValidSourceDateTo)
                _errorString +=" Invalid Source Date To";
            if (!IsValidSourceDescription)
                _errorString +=" Invalid Source Desc.";

            return false;
        }


        public string GetErrors()
        {
            return _errorString;
        }

    
        bool _isValidSourceDate;
        bool _isValidSourceDateTo;
        bool _isValidSourceDescription;
        bool _isValidSourceOriginalLocation;
        bool _isValidSourceRef;
  

    
        public bool IsValidSourceDate
        {
            get
            {
                return _isValidSourceDate;

            }
        }

        public bool IsValidSourceDateTo
        {
            get
            {
                return _isValidSourceDateTo;
            }
        }

        public bool IsValidSourceDescription
        {
            get
            {
                return _isValidSourceDescription;
            }
        }

        public bool IsValidSourceOriginalLocation
        {
            get
            {
                return _isValidSourceOriginalLocation;
            }
        }

        public bool IsValidSourceRef
        {
            get
            {
                return _isValidSourceRef;
            }
        }

        

        private void Validate()
        {
            _isValidSourceDate = CsUtils.ValidYear(_sourceAjaxDto.SourceDateStr);

            _isValidSourceDateTo = CsUtils.ValidYear(_sourceAjaxDto.SourceDateStrTo);

            _isValidSourceDescription = !string.IsNullOrWhiteSpace(_sourceAjaxDto.SourceDesc);

            _isValidSourceOriginalLocation = !string.IsNullOrWhiteSpace(_sourceAjaxDto.OriginalLocation);

            _isValidSourceRef = !string.IsNullOrWhiteSpace(_sourceAjaxDto.SourceRef);


        }
    }
}
