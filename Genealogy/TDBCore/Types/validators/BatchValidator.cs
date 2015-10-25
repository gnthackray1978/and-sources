using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types.validators
{
    public class BatchValidator : IValidator
    {
        public bool ValidEntry()
        {
            return true;
        }

        public string GetErrors()
        {
            return "";
        }
    }
}
