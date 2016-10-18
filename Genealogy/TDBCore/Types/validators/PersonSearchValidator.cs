
using System.Linq;
using TDBCore.Types.libs;

namespace TDBCore.Types.validators
{
    public class PersonSearchValidator:IValidator
    {
        private readonly PersonSearchFilter _sp;
        private string _errorMessage = "";

        public PersonSearchValidator(PersonSearchFilter personSearchFilter)
        {
            _sp = personSearchFilter;
        }

        public bool ValidEntry()
        {
            if (_sp.Ids.Any()) return true;

            if (_sp.Location.IsNullOrEmpty() && _sp.SourceString.IsNullOrEmpty() && _sp.CName.IsNullOrEmpty() && _sp.Surname.IsNullOrEmpty() &&
                _sp.MotherChristianName.IsNullOrEmpty() && _sp.MotherSurname.IsNullOrEmpty() && _sp.FatherChristianName.IsNullOrEmpty() && _sp.FatherSurname.IsNullOrEmpty()
                && _sp.ParishString.IsNullOrEmpty())
            {
                _errorMessage = "Nothing searched for";
                return false;
            }
            return true;
        }

        public string GetErrors()
        {
            return _errorMessage;
        }

        public bool IsValidSearchUpperBound {
            get {
                return _sp.UpperDate > 1000 && _sp.UpperDate < 2100;
            }
        }

        public bool IsValidSearchLowerBound => _sp.LowerDate > 1000 && _sp.LowerDate < 2100;
    }
}
