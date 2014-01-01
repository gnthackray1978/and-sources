

using TDBCore.Types.filters;

namespace TDBCore.Types.validators
{
    public class SourceTypeSearchValidator :IValidator
    {
        private readonly SourceTypeSearchFilter _sourceTypeSearchFilter;
        private string _errorRecord;

        public SourceTypeSearchValidator(SourceTypeSearchFilter sourceTypeSearchFilter)
        {
            _sourceTypeSearchFilter = sourceTypeSearchFilter;
        }

        public bool ValidEntry()
        {
            if (_sourceTypeSearchFilter.Description!=null &&  _sourceTypeSearchFilter.Description.Length >= 250)
            {
                _errorRecord = "Description too long it must be less than 250 chars!";
                return false;
            }

            return true;
        }

        public string GetErrors()
        {
            return _errorRecord;
        }
    }
}
