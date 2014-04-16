using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TDBCore.Types.libs
{
    public class DateTools
    {

        public static int CalcDeathInt(string deathInt, string deathStr)
        {
            int retVal = 0;

            var regex = new Regex(@"\d\d\d\d");

            var match = regex.Match(deathStr);

            if (match.Success)
            {

                retVal = Convert.ToInt32(match.Value);
            }

            match = regex.Match(deathInt);
            if (match.Success)
            {

                retVal = Convert.ToInt32(match.Value);
            }

            return retVal;
        }

        public static int CalcMarriageBirthYear(string date, string marriageDate)
        {

            int retVal = 0;
            var regex = new Regex(@"\d\d\d\d");
            var match = regex.Match(marriageDate);

            if (match.Success)
            {
                retVal = Convert.ToInt32(match.Value);
            }

            int age;

            Int32.TryParse(date, out age);

            if (age > 0)
            {
                retVal = retVal - age;
            }

            return retVal;

        }

        public static int CalcBirthInt(string birthStr, string alternateBirth,string referenceDate, string ageYear, string ageMonth, string ageWeek, string strDeathDate)
        {

            var regex = new Regex(@"\d\d\d\d");

            if (strDeathDate == "1698 11 27")
            {
                Debug.WriteLine("");
            }


            if (birthStr == "" && alternateBirth != "") birthStr = alternateBirth;

            if (strDeathDate == "" && referenceDate != "") strDeathDate = referenceDate;



            var match = regex.Match(birthStr);

            if (!match.Success)
            {

                #region we dont have a birth string supplied so try to calc. it

                int year = ageYear.ToInt32();

                int month = ageMonth.ToInt32();

                int weeks = ageMonth.ToInt32();


                bool isValidBirthYear = year > 0;

                bool isValidMonths = month > 0;

                bool isValidWeeks = weeks > 0;





                match = regex.Match(strDeathDate);



                if (match.Success && (isValidBirthYear || isValidMonths || isValidWeeks))
                {

                    DateTime deathDate;

                    DateTime birthDate = DateTime.Today;



                    bool isValidDeathDate = DateTime.TryParse(strDeathDate, out deathDate);



                    if (!isValidDeathDate)

                        deathDate = new DateTime(Convert.ToInt32(match.Value), 1, 1);





                    if (isValidWeeks)
                    {

                        int days = weeks * 7;



                        birthDate = deathDate.AddDays(days - (days * 2));

                    }

                    if (isValidMonths)
                    {



                        birthDate = deathDate.AddMonths(month - (month * 2));

                    }

                    if (isValidBirthYear)
                    {

                        birthDate = deathDate.AddYears(year - (year * 2));

                    }





                    return birthDate.Year;

                }

                #endregion

            }

            else
            {
                DateTime origBirthDate;

                return !DateTime.TryParse(birthStr, out origBirthDate) ? match.Value.ToInt32() : origBirthDate.Year;
            }



            return 0;

        }

        public static string CalcBirthStr(string mainBirthStr, string alternateBirth, string referenceDate, string ageYear, string ageMonth, string ageWeek, string strDeathDate)
        {

            var regex = new Regex(@"\d\d\d\d");


            if (mainBirthStr == "" && alternateBirth != "")
            {
                mainBirthStr = alternateBirth;
            }

            if (strDeathDate == "" && referenceDate != "") strDeathDate = referenceDate;



            if (mainBirthStr == "" && strDeathDate == "")
            {

                Match match0 = regex.Match(alternateBirth);



                if (match0.Success)
                {

                    mainBirthStr = "Abt" + Convert.ToInt32(match0.Value);

                }



            }


            Match match = regex.Match(mainBirthStr);


            if (!match.Success)
            {

                #region we dont have a birth string supplied so try to calc. it

                int year = ageYear.ToInt32();

                int month = ageMonth.ToInt32();

                int weeks = ageMonth.ToInt32();


                bool isValidBirthYear = year >0;

                bool isValidMonths = month >0;

                bool isValidWeeks = weeks >0;





                match = regex.Match(strDeathDate);



                if (match.Success && (isValidBirthYear || isValidMonths || isValidWeeks))
                {

                    DateTime deathDate;

                    DateTime birthDate = DateTime.Today;



                    bool isValidDeathDate = DateTime.TryParse(strDeathDate, out deathDate);



                    if (!isValidDeathDate)

                        deathDate = new DateTime(Convert.ToInt32(match.Value), 1, 1);





                    if (isValidWeeks)
                    {

                        int days = weeks * 7;



                        birthDate = deathDate.AddDays(days - (days * 2));

                    }

                    if (isValidMonths)
                    {



                        birthDate = deathDate.AddMonths(month - (month * 2));

                    }

                    if (isValidBirthYear)
                    {

                        birthDate = deathDate.AddYears(year - (year * 2));

                    }





                    return birthDate.ToString("dd-MMM-yyyy");

                }

                #endregion

            }

            else
            {
                DateTime origBirthDate;
                bool isValidBirthDate = DateTime.TryParse(mainBirthStr, out origBirthDate);
                if (!isValidBirthDate)
                    return mainBirthStr;

                return origBirthDate.ToString("dd-MMM-yyyy");
            }



            return "";



        }

        public static string MakeDateString(string datebapstr,string datebirthstr, string datedeath, string years, string months, string weeks, string days)
        {                
            if (datebapstr == "" && datebirthstr == "" && datedeath != "" &&
                (years != "" || months != "" || weeks != "" || days != ""))
            {
                var deathDate = new DateTime(2100, 1, 1);

                if (!DateTime.TryParse(datedeath, out deathDate))
                {
                    int deathYear = datedeath.ParseToValidYear();
                    if (deathYear != 0)
                    {
                        deathDate = new DateTime(deathYear, 1, 1);
                    }
                }

                if (deathDate.Year != 2100)
                {
                    int iyears = 0;
                    int imonths = 0;
                    int iweeks = 0;
                    int idays = 0;

                    Int32.TryParse(years, out iyears);
                    Int32.TryParse(months, out imonths);
                    Int32.TryParse(weeks, out iweeks);
                    Int32.TryParse(days, out idays);

                    idays = (iyears*365) + (imonths*28) + (iweeks*7) + idays;

                    var ts = new TimeSpan(idays, 1, 1, 1);


                    DateTime birthDate = deathDate.Subtract(ts);

                    datebirthstr = birthDate.ToString("dd MMM yyyy");

                }

            }


            return datebirthstr;
        }

        public static bool ValidYear(string inputStr)
        {
            bool validYear = false;

            int year = inputStr.ParseToValidYear();

            if (inputStr.ParseToValidYear() != 0)
            {
                validYear = ValidYearRange(year);


            }
            

            return validYear;
        }

        public static bool ValidYearRange(int year)
        {
            if (year > 1000 && year < 2100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void CalcEstDates(int birthInt, int bapInt, int deathInt, out int estBirth, out int estDeath, out bool isEstBirth, out bool isEstDeath, string fatName, string moName)
        {
            estBirth = 0;
            estDeath = 0;
            isEstBirth = false;
            isEstDeath = false;

            if (bapInt > 0)
                estBirth = bapInt;
            else
                estBirth = birthInt;


            estDeath = deathInt;



            if ((bapInt == 0 && birthInt == 0) && deathInt > 0)
            {
                isEstBirth = true;

                //died in childhood average age is 10years
                //
                if (fatName.Length > 0 ||
                    moName.Length > 0)
                {
                    estBirth = deathInt - 10;
                }
                else
                { // normal average life expectancy about 50 years
                    estBirth = deathInt - 50;
                }


            }

            if (deathInt == 0 && (bapInt > 0 || birthInt > 0))
            {
                isEstDeath = true;

                if (bapInt > 0)
                    estDeath = bapInt + 50;
                else
                    estDeath = birthInt + 50;

            }




        }
    }
}