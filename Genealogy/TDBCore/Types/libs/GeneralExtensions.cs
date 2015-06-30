using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TDBCore.Types.domain.import;

namespace TDBCore.Types.libs
{
    public static class GeneralExtensions
    {


        public static string Get(this string[] fieldList,IList<CSVField> fields , CSVField field)
        {
            int idx = 0;

            while (idx < fields.Count)
            {
                if (fields[idx] == field) break;
                idx++;
            }


            if (fieldList != null && fieldList.Length > idx) return fieldList[idx];

            return field.ToString("G") + " not present";
        }




        public static bool LazyContains(this string str, string contains)
        {
            return str.Trim().ToLower().Contains(contains.Trim().ToLower());
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }


        public static List<int> ParseToIntList(this string str)
        {
            var selection = new List<int>();

            if (string.IsNullOrEmpty(str)) return selection;

            str.Split(',').ToList().ForEach(s => selection.Add(s.ToInt32()));

            return selection;
        }

        public static List<Guid> ParseToGuidList(this string str)
        {
            var selection = new List<Guid>();

            if (string.IsNullOrEmpty(str)) return selection;

            str.Split(',').ToList().ForEach(s => selection.Add(s.ToGuid()));


            return selection;
        }


        public static int ParseToInt32(this IDictionary<string, string> str, string key, int defaultValue =0)
        {
            key = key.Trim();
            int retVal = defaultValue;

            if (str != null &&
                      str.Count > 0)
            {
                if(str.ContainsKey(key))
                {
                    Int32.TryParse(str[key], out retVal);
                }
            }

            return retVal;
        }

        public static int ParseToValidYear(this string yearVal)
        {

            int retVal = 0;



            var regex = new Regex(@"\d\d\d\d");

            Match match = regex.Match(yearVal);



            if (match.Success)
            {

                retVal = Convert.ToInt32(match.Value);

            }



            return retVal;

        }

        public static string ParseToString(this IDictionary<string, string> str, string key, string defaultValue = "")
        {
            key = key.Trim();
            string retVal = defaultValue;

            if (str != null &&
                      str.Count > 0)
            {
                if (str.ContainsKey(key))
                {

                    retVal = str[key];
                    
                }
            }

            return retVal;
        }

        public static Guid ToGuid(this string str)
        {

            if (str == null) str = "";

            str = str.Trim();


            Guid retVal;

            try
            {
                retVal = new Guid(str);
            }
            catch 
            {
                retVal = Guid.Empty;
            }
            


            return retVal;
        }

        public static Guid SafeFirst(this List<Guid> str)
        {
            return str.IsNullOrBelowMinSize() ? Guid.Empty : str.First();
        }

        public static string ParseToCSV(this List<Guid> str, string seperator = ",")
        {
            if (str.IsNullOrBelowMinSize()) return "";

            string retVal = str.Aggregate("", (current, guid) => current + (seperator + guid.ToString()));

            if (retVal.StartsWith(seperator)) retVal = retVal.Remove(0, 1);

            return retVal;
        }

        public static string ParseToCSV(this List<int> str, string seperator = ",")
        {

            string retVal = str.Aggregate("", (current, _int) => current + (seperator + _int));

            if (retVal.StartsWith(seperator)) retVal = retVal.Remove(0, 1);

            return retVal;
        }


        public static bool ToBool(this string str)
        {
            if (str == null) str = "";

            str = str.Trim();


            bool retVal;

            try
            {
                if (str.ToLower() == "on")
                    str = "true";

                if (str.ToLower() == "off")
                    str = "false";

                retVal = Convert.ToBoolean(str);
            }
            catch
            {
                retVal = false;
            }



            return retVal;
        }

        public static bool? ToNullableBool(this string str)
        {
            if (str == null) return null;

            str = str.Trim();


            bool retVal;

            try
            {
                if (str.ToLower() == "on")
                    str = "true";

                if (str.ToLower() == "off")
                    str = "false";

                retVal = Convert.ToBoolean(str);
            }
            catch
            {
                retVal = false;
            }



            return retVal;
        }

        public static int ToInt32(this String str)
        {

            if (str == null)
                str = "";

            str = str.Trim();
            int retVal;

            Int32.TryParse(str, out retVal);


            return retVal;
        }
        public static double ToDouble(this String str)
        {

            if (str == null) str = "";

            str = str.Trim();
            double retVal;

            Double.TryParse(str, out retVal);


            return retVal;
        }

       
        public static IList<T> RemoveDuplicates<T>(this IList<T> list)
        {
            //TDBCore.Types.EqualityComparer<Person> ec_p = new TDBCore.Types.EqualityComparer<Person>((o1, o2) => o1.ChristianName == o2.ChristianName
            //    && o1.Surname == o2.Surname
            //    && o1.ReferenceDateStr == o2.ReferenceDateStr,
            //    o => (o.ReferenceDateStr.GetHashCode() + o.Surname.GetHashCode() + o.ChristianName.GetHashCode()));

            IList<T> p = new List<T>();
            IList<T> dupes = new List<T>();

            int idx = 0;

            while (idx < list.Count)
            {

                if (!p.Contains(list[idx]))
                {
                    p.Add(list[idx]);
                    idx++;
                }
                else
                {
                    dupes.Add(list[idx]);
                    list.Remove(list[idx]);

                }


            }

            return dupes;

        }

        public static void Remove<T>(this ICollection<T> list, Func<T, bool> predicate)
        {
            var items = list.Where(predicate).ToList();

            foreach (var item in items)
            {
                list.Remove(item);
            }
        }

        public static IList<T> OrderBy<T>(this IList<T> list, String sortBy)
        {
            
            //DESC
            if (list.Count == 0 || sortBy.Trim() == "")
            {
                return list;
            }

            if (sortBy.Contains(" DESC"))
            {
                sortBy = sortBy.Replace(" DESC", "");
                PropertyInfo t = list[0].GetType().GetProperty(sortBy.Trim());
                if(t != null)
                    return list.OrderByDescending(e => t.GetValue(e, null)).ToList();
            }
            else
            {
                //rewrite somehow!
                 
                PropertyInfo t = list[0].GetType().GetProperty(sortBy);
                if (t != null)
                    return list.OrderBy(e => t.GetValue(e, null)).ToList();
                
            }

            return list;
        }
         
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {

            if (list == null) return true;

            if (list.Count == 0) return true;

            return false;
        }
        public static bool IsNullOrBelowMinSize<T>(this List<T> list, int minSize = 1)
        {

            if (list == null) return true;

            if (list.Count < minSize) return true;

            return false;
        }
    }


}
