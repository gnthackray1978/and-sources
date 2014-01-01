using System;
using System.Text.RegularExpressions;

namespace TDBCore.Types.libs
{
    public class DateTools
    {
        public static bool TryParseYear(string param, out int year)
        {

            int retVal = 0;



            Regex regex = new Regex(@"\d\d\d\d");

            Match _match = regex.Match(param);



            if (_match.Success)
            {
                retVal = Convert.ToInt32(_match.Value);
                year = retVal;
                return true;
            }
            else
            {
                year = 0;
                return false;
            }



            
        }
    }
}