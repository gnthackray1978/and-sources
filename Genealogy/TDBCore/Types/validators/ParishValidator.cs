using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.Types.DTOs;

namespace TDBCore.Types.validators
{
    public class ParishValidator :IValidator
    {
        public ServiceParish ServiceParish { get; set; }

        private string _errorState = "";

        public bool ValidEntry()
        {
            return ValidCounty() && ValidDeposited() && ValidName();
        }

        public string GetErrors()
        {
            return _errorState;
        }

        //500
        public bool ValidName()
        {
            return ServiceParish.ParishName.Length >= 3 && ServiceParish.ParishName.Length <= 500;
        }

        //50
        public bool ValidCounty()
        {
            return ServiceParish.ParishCounty.Length >= 3 && ServiceParish.ParishCounty.Length <= 50;
        }
        //100
        public bool ValidDeposited()
        {
            return ServiceParish.ParishDeposited.Length >= 3 && ServiceParish.ParishDeposited.Length <= 100;
                          
        }

    }
}
