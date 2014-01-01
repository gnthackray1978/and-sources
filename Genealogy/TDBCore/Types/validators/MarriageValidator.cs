using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.Types.DTOs;
using TDBCore.Types.libs;

namespace TDBCore.Types.validators
{
    public class MarriageValidator : IValidator
    {
        private ServiceMarriage _serviceMarriage = null;

        private string _errorState = "";


        public MarriageValidator (ServiceMarriage serviceMarriage)
        {
            _serviceMarriage = serviceMarriage;

        }

        
      
        public bool ValidEntry()
        {
            if (IsValidMaleCName() && IsValidMaleSName() && IsValidFemaleCName() && IsValidFemaleSName() && IsValidMarriageDate())
            {
                this._errorState = "";
                return true;
            }
            else
            {
                this._errorState = "";
                if (!IsValidMaleCName() || !IsValidMaleSName())
                    this._errorState = this.GetErrors() + " Invalid Male Name";
                if (!IsValidFemaleCName() || !IsValidFemaleSName())
                    this._errorState = this.GetErrors() + " Invalid Female Name";
                if (!IsValidMarriageDate())
                    this._errorState = this.GetErrors() + " Invalid Marriage Date";
                return false;
            }
        }
        public string GetErrors()
        {
            return ValidEntry() ? "" : _errorState;
        }

       
        


        public bool IsValidWitness1()
        {
            if (_serviceMarriage.Witness1ChristianName.Length == 0 && _serviceMarriage.Witness1Surname.Length == 0)
                return false;
            else
                return true;
        }



        public bool IsValidMarriageLocationId()
        {
            if (_serviceMarriage.LocationId.ToGuid() == Guid.Empty)
                return false;
            else
                return true;
        }

        public bool IsValidMaleLocationId()
        {
            if (_serviceMarriage.MaleLocationId.ToGuid() == Guid.Empty)
                return false;
            else
                return true;
        }

        public bool IsValidFemaleLocationId()
        {
            if (_serviceMarriage.MaleLocationId.ToGuid() == Guid.Empty)
                return false;
            else
                return true;
        }
            
        public bool IsValidMaleCName()
        {
            if (_serviceMarriage.MaleCName.Length >= 0 && _serviceMarriage.MaleCName.Length <= 50)
            {
                return true;
            }
            else
            {
                return false;
            }               
        }

        public bool IsValidMaleSName()
        {
            if (_serviceMarriage.MaleSName.Length >= 0 && _serviceMarriage.MaleSName.Length <= 50)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidFemaleCName()
        {
            if (_serviceMarriage.FemaleCName.Length >= 0 && _serviceMarriage.FemaleCName.Length <= 50)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidFemaleSName()
        {
            if (_serviceMarriage.FemaleSName.Length >= 0 && _serviceMarriage.FemaleSName.Length <= 50)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidMarriageDate()
        {          
            int result = 0;
            if (CsUtils.ValidYear(_serviceMarriage.MarriageDate, out result))
            {
                return true;
            }
            else
            {
                return false;
            } 
        }

        public bool IsValidLocation()
        {
            if (_serviceMarriage.MarriageLocation.Length >= 0 && _serviceMarriage.MarriageLocation.Length <= 50)
            {
                return true;
            }
            else
            {
                return false;
            }                
        }

        public bool IsValidMaleLocation()
        {
            if (_serviceMarriage.MaleLocation.Length >= 0 && _serviceMarriage.MaleLocation.Length <= 50)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidFemaleLocation(string femaleLocation)
        {
            if (_serviceMarriage.FemaleLocation.Length >= 0 && _serviceMarriage.FemaleLocation.Length <= 50)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidMarriageCounty(string marriageCounty)
        {
            if (_serviceMarriage.LocationCounty.Length >= 0 && _serviceMarriage.LocationCounty.Length <= 50)
            {
                return true;
            }
            else
            {
                return false;
            }                 
        }

        public bool IsValidSource()
        {
            if (_serviceMarriage.Sources.Length >= 0 && _serviceMarriage.Sources.Length <= 50)
            {
                return true;
            }
            else
            {
                return false;
            }             
        }



        public bool IsValidFemaleInfo()
        {
            if (_serviceMarriage.FemaleNotes.Length >= 0 && _serviceMarriage.FemaleNotes.Length <= 500)
            {
                return true;
            }
            else
            {
                return false;
            }                 
        }

        public bool IsValidMaleInfo()
        {
            if (_serviceMarriage.MaleNotes.Length >= 0 && _serviceMarriage.MaleNotes.Length <= 500)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public bool IsValidMaleOccupation()
        {
            if (_serviceMarriage.MaleOccupation.Length >= 0 && _serviceMarriage.MaleOccupation.Length <= 500)
            {
                return true;
            }
            else
            {
                return false;
            }              
        }

        public bool IsValidFemaleOccupation()
        {
            if (_serviceMarriage.FemaleOccupation.Length >= 0 && _serviceMarriage.FemaleOccupation.Length <= 500)
            {
                return true;
            }
            else
            {
                return false;
            }               
        }

        public bool IsValidFemaleBirthYear()
        {            
            int result;
               
            if (Int32.TryParse(_serviceMarriage.FemaleBirthYear.ToString(), out result))
            {
                if (result > 1300 && result < 2100)
                {
                    return true;                     
                }
                else
                {
                    return false;                         
                }
            }
            else
            {
                return false;                   
            }                       
        }

        public bool IsValidMaleBirthYear()
        {
            int result;

            if (Int32.TryParse(_serviceMarriage.MaleBirthYear.ToString(), out result))
            {
                if (result > 1300 && result < 2100)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }       
        }







      
    }
}
