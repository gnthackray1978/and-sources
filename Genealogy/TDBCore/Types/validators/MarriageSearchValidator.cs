using System;
using TDBCore.Types.filters;
using TDBCore.Types.libs;

namespace TDBCore.Types.validators
{
    public class MarriageSearchValidator : IValidator
    {
        private readonly MarriageSearchFilter _sm;
        private string _errorMessage = "";

        public MarriageSearchValidator(MarriageSearchFilter serviceMarriageLookup)
        {
            _sm = serviceMarriageLookup;
        }

        public bool ValidEntry()
        {

            if (_sm.Source.ToGuid() != Guid.Empty)
            {
                return true;
            }

            if (_sm.UpperDate == 0 && _sm.LowerDate == 0 && _sm.Parish != "")
            {
                return true;
            }
            else
            {
                if ((_sm.MaleCName == "" && _sm.FemaleCName == "" && _sm.MaleSName == "" &&
                     _sm.FemaleSName == "" &&
                     _sm.County == "" && _sm.Location == "" && _sm.FemaleLocation == "" &&
                     _sm.MaleLocation == "" && _sm.Source == "" && _sm.Witness == "") ||
                    (_sm.UpperDate == 0 && _sm.LowerDate == 0))
                {
                    if ((_sm.MaleCName == "" && _sm.FemaleCName == "" && _sm.MaleSName == "" &&
                         _sm.FemaleSName == "" &&
                         _sm.County == "" && _sm.Location == "" && _sm.FemaleLocation == "" &&
                         _sm.MaleLocation == "" && _sm.Source == "" && _sm.Witness == ""))
                    {
                        _errorMessage =
                            "1 of the fields MaleCName,FemaleCName,MaleSName,FemaleSName,County,Location,FemaleLocation,MaleLocation,Source,Witness must have a value";
                    }

                    if (_sm.UpperDate == 0 && _sm.LowerDate == 0)
                    {
                        _errorMessage +=
                            "UpperDate and LowerDate must both be greater than zero";
                    }

                    _errorMessage = "Invalid search params: " + _errorMessage;
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public string GetErrors()
        {
            return _errorMessage;
        }




      
       



    }
}
