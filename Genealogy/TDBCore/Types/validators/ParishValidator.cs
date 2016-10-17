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
            if (ServiceParish.ParishName.Length >= 3 && ServiceParish.ParishName.Length <= 500)
            {
                return true;
            }

            _errorState += Environment.NewLine + "Invalid ParishName Value";

            return false;
        }

        //50
        public bool ValidCounty()
        {

            if (ServiceParish.ParishCounty.Length >= 3 && ServiceParish.ParishCounty.Length <= 50)
            {
                return true;
            }

            _errorState += Environment.NewLine + "Invalid ParishCounty Value";

            return false;

        }
        //100
        public bool ValidDeposited()
        {
           
            if (ServiceParish.ParishDeposited.Length >= 3 && ServiceParish.ParishDeposited.Length <= 100)
            {
                return true;
            }


            _errorState += Environment.NewLine + "Invalid ParishDeposited Value";

            return false;
        }

    }
}
