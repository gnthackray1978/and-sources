using System;
using TDBCore.Types.filters;
using TDBCore.Types.libs;

namespace TDBCore.Types.validators
{
    public class SourceSearchValidator :IValidator
    {
        readonly SourceSearchFilter _sourceSearchFilter;

        public SourceSearchValidator(SourceSearchFilter sourceSearchFilter)
        {
            _sourceSearchFilter = sourceSearchFilter;
        }

        public bool ValidEntry()
        {
            if (!_sourceSearchFilter.Sources.IsNullOrBelowMinSize(1)) return true;

            var invalidStr = _sourceSearchFilter.Ref.IsNullOrEmpty() &&  _sourceSearchFilter.OriginalLocation.IsNullOrEmpty() &&   _sourceSearchFilter.Description.IsNullOrEmpty();

            if (!invalidStr) return true;
            
            return !(_sourceSearchFilter.FromYear == 0 && _sourceSearchFilter.ToYear== 0);
        }

        public string GetErrors()
        {
            return "";
        }
    }
}
