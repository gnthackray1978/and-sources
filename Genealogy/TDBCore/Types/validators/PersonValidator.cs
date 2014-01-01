using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.Types.DTOs;
using TDBCore.Types.libs;

namespace TDBCore.Types.validators
{
    public class PersonValidator :IValidator
    {
        public ServicePerson ServicePerson { get; set; }
        private string _errorState = "";




        private bool ValidateString(string param, int min = 0, int max = 150)
        {
            return param.Length >= min
                       && param.Length <= max;

        }

        private bool ValidateGuidAsString(string param)
        {
            return param.ToGuid() != Guid.Empty;
        }

        private bool ValidateDate(string param)
        {
        
            return CsUtils.ValidYear(param); ;
        }


        public PersonValidator (ServicePerson servicePerson)
        {
            ServicePerson = servicePerson;

        }

        public bool ValidEntry()
        {
            return IsValidBirthEntry || IsValidDeathEntry || IsValidReferenceEntry;
        }

        public string GetErrors()
        {

            return ValidEntry() ? "" : _errorState;
            
        }

        

        


        public bool IsValidBirthEntry
        {
            get
            {
                if (IsValidName && IsValidSurname && (IsValidBirthDate || IsValidBapDate) && IsValidBirthLocation)
                    return true;
                
                SetErrorState("InvalidBirthEntry");

                return false;
            }
        }
        public bool IsValidDeathEntry
        {
            get
            {

                if (IsValidName && IsValidSurname && IsValidDeathDate && IsValidDeathLocation)
                    return true;
                
                SetErrorState("InvalidDeathEntry");

                return false;
            }
        }
        public bool IsValidReferenceEntry
        {
            get
            {
                if (IsValidName && IsValidSurname && IsValidReferenceDate && IsValidReferenceLocation)
                    return true;
                
                SetErrorState("InvalidReferenceEntry");

                return false;
            }
        }

        private void SetErrorState(string param)
        {
            if (_errorState == param) return;

            if (_errorState.Length < 200)
                _errorState += " " + param;
        }

        public bool IsValidSpouseCName 
        {
            get
            {
                return ValidateString(ServicePerson.SpouseChristianName);                
            }
        }
    
        public bool IsValidSpouseSName
        {
            get
            {
                return ValidateString(ServicePerson.SpouseSurname);
            }
        }

        public bool IsValidFatherOccupation
        {
            get
            {
                return ValidateString(ServicePerson.FatherOccupation);
            }
        }

        public bool IsValidBirthLocationId
        {
            get
            {
                return ValidateGuidAsString(ServicePerson.BirthLocationId);
            }
        }

        public bool IsValidDeathLocationId  
        {
            get
            {
                return ValidateGuidAsString(ServicePerson.DeathLocationId);
            }
        }

        public bool IsValidReferenceLocationId
        {
            get
            {
                return ValidateGuidAsString(ServicePerson.ReferenceLocationId);
            }
        }

        public bool IsValidOccupation
        {
            get
            {
                return ValidateString(ServicePerson.Occupation);
            }
        }

        public bool IsValidSurname
        {
            get
            {
                return ValidateString(ServicePerson.Surname);
            }
        }

        public bool IsValidFatherChristianName
        {
            get
            {
                return ValidateString(ServicePerson.FatherChristianName);
            }
        }

        public bool IsValidFatherSurname
        {
            get
            {
                return ValidateString(ServicePerson.FatherSurname);
            }
        }

        public bool IsValidMotherChristianName
        {
            get
            {
                return ValidateString(ServicePerson.MotherChristianName);
            }
        }

        public bool IsValidMotherSurname
        {
            get
            {
                return ValidateString(ServicePerson.MotherSurname);
            }
        }

        public bool IsValidBirthCountyLocation
        {
            get
            {
                return ValidateString(ServicePerson.BirthCounty);
            }
        }

        public bool IsValidDeathCountyLocation
        {
            get
            {
                return ValidateString(ServicePerson.DeathCounty);
            }
        }

        public bool IsValidBirthLocation
        {
            get
            {
                return ValidateString(ServicePerson.BirthLocation);
            }
        }

        public bool IsValidSource
        {
            get
            {
                return ValidateString(ServicePerson.Sources,0,50);
            }
        }

        public bool IsValidNotes
        {
            get
            {
                return ValidateString(ServicePerson.Notes, 0, 8000);
            }
        }

        public bool IsValidUniqueRef
        {
            get
            {
                //todo no validation on unique refs
                return true;
            }
        }

        public bool IsValidOriginalName
        {
            get
            {
                //todo no validation on original name
                return true;
            }
        }

        public bool IsValidOriginalFatherName
        {
            get
            {
                //todo no validation on original father name
                return true;
            }
        }

        public bool IsValidOriginalMotherName
        {
            get
            {
                //todo no validation on original mother name
                return true;
            }
        }

        public bool IsValidName
        {
            get
            {
                return ValidateString(ServicePerson.ChristianName);
            }
        }

        public bool IsValidDeathLocation
        {
            get
            {
                return ValidateString(ServicePerson.DeathLocation);
            }
        }



        public bool IsValidReferenceLocation
        {
            get
            {
                return ValidateString(ServicePerson.ReferenceLocation);
            }
        }




        public bool IsValidBapDate
        {
            get
            {
                return ValidateDate(ServicePerson.Baptism);
            }
        }

        public bool IsValidDeathDate
        {
            get
            {
                return ValidateDate(ServicePerson.Death);
            }
        }

        public bool IsValidBirthDate
        {
            get
            {
                return ValidateDate(ServicePerson.Birth);
            }
        }

        public bool IsValidReferenceDate
        {
            get
            {
                return ValidateDate(ServicePerson.ReferenceDate);
            }
        }






        
    }
}
