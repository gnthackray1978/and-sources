

/*
   yes i know extension methods arent types!
 * but is a tidy looking place to put them 
 * even if its not strictly speaking correct
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using TDBCore.Types;

#if! SILVERLIGHT
using TDBCore.EntityModel;
using TDBCore.BLL;
using System.Collections.Specialized;
using GedItter.Interfaces;
using GedItter.BLL;
#endif

namespace TDBCore.Types
{

    public static class MyExtensions
    {

        //public static string ToCSVString<T>(this List<T> list)
        //{
        //    return string.Join(",", list.ConvertAll<string>(delegate(T i) { return i.ToString(); }).ToArray());
        //}
        #if! SILVERLIGHT
        public static void ReadInErrorsAndSecurity<T>(this NameValueCollection query, IDBRecordModel<T> baseModel)
        {
            if (query.AllKeys.Contains("error"))
            {
                baseModel.SetErrorState(query["error"] ?? "");
            }

            if (query.AllKeys.Contains("permission"))
            {
                baseModel.SetPermissionState(query["permission"] ?? "");
            }


        }
#endif
        public static TreePerson GetTreePerson(this List<List<TreePerson>> treeList, Guid personId)
        {
            TreePerson retPerson = null;

            foreach (List<TreePerson> ltp in treeList)
            {
                foreach (TreePerson tp in ltp)
                {
                    if (tp.PersonId == personId)
                    {
                        return tp;
                    }
                }
            }

            return retPerson;
        }

        public static int FirstIndexOfFamily(this List<TreePerson> treeList, Guid personId, Guid fatherId, Guid motherId)
        {

            int idx =-1;


            if (treeList.Count > 0 && treeList.Count(tl=>tl.PersonId == personId
                               && tl.FatherId == fatherId
                               && tl.MotherId == motherId) > 0)
            {

                idx = 0;

                while (idx < treeList.Count)
                {

                    if (treeList[idx].PersonId == personId
                               && treeList[idx].FatherId == fatherId
                               && treeList[idx].MotherId == motherId)
                    {
                        break;
                    }

                    idx++;
                }


            }


            return idx;
        }


        public static int FirstIndexOfPerson(this List<TreePerson> treeList, Guid personId)
        {

            int idx = -1;


            if (treeList.Count > 0 && treeList.Count(tl => tl.PersonId == personId) > 0)
            {

                idx = 0;

                while (idx < treeList.Count
                               && treeList[idx].PersonId != personId)
                {
                    idx++;
                }


            }


            return idx;
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

        public static string ParseToString(this IDictionary<string, string> str, string key, string defaultValue = "")
        {
            key = key.Trim();
            string retVal = defaultValue;

            if (str != null &&
                      str.Count > 0)
            {
                if (str.ContainsKey(key))
                {

                    retVal = str[key].ToString();
                    
                }
            }

            return retVal;
        }

        public static Guid ToGuid(this string str)
        {

            if (str == null) str = "";

            str = str.Trim();


            Guid retVal = Guid.Empty;

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


        public static string ParseToCSV(this List<Guid> str)
        {

            string retVal = "";
            foreach (Guid _guid in str)
            {
                retVal += "," + _guid.ToString();
            }

            if (retVal.StartsWith(",")) retVal = retVal.Remove(0, 1);

            return retVal;
        }

        public static string ParseToCSV(this List<int> str)
        {

            string retVal = "";
            foreach (int _int in str)
            {
                retVal += "," + _int.ToString();
            }

            if (retVal.StartsWith(",")) retVal = retVal.Remove(0, 1);

            return retVal;
        }


        public static bool ToBool(this string str)
        {
            if (str == null) str = "";

            str = str.Trim();


            bool retVal = false;

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
            int retVal =0;

            Int32.TryParse(str, out retVal);


            return retVal;
        }

        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static bool AlmostEquals(this double double1, double double2, double precision)
        {
            return (Math.Abs(double1 - double2) <= precision);
        }

        /// <summary>
        /// RETURNS list of DUPLICATE items
        /// </summary>
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
                PropertyInfo _t = list[0].GetType().GetProperty(sortBy.Trim());
                if(_t != null)
                    return list.OrderByDescending(e => _t.GetValue(e, null)).ToList();
            }
            else
            {
                //rewrite somehow!
                 
                PropertyInfo _t = list[0].GetType().GetProperty(sortBy);
                if (_t != null)
                    return list.OrderBy(e => _t.GetValue(e, null)).ToList();
                
            }

            return list;
        }

        public static bool ContainsYearRange(this IEnumerable<SourceRecord> list, int startYear, int endYear)
        {
            int idx = 0;

            bool isFound = false;



            List<YearRange> orderedList = list.OrderBy(sr => sr.YearStart).Select(y=> new YearRange(y.YearStart,y.YearEnd)).ToList();

            while ((idx + 1) < orderedList.Count)
            {
                if ((orderedList[idx + 1].StartYear <= orderedList[idx].EndYear) ||
                    (orderedList[idx + 1].StartYear == orderedList[idx].EndYear + 1))
                {

                    if (orderedList[idx].EndYear < orderedList[idx + 1].EndYear)
                    {
                        orderedList[idx].EndYear = orderedList[idx + 1].EndYear;
                    }

                    orderedList.RemoveAt(idx + 1);
                }
                else
                {
                    idx++;
                }
            }

            foreach (var _rec in orderedList)
            {
                if (_rec.ContainsYearRange(startYear, endYear))
                {
                    isFound = true;
                }
            }


            return isFound;
        }

        public static bool ContainsYearRange(this SourceRecord _sourceRecord, int startYearRngToTest, int endYearRngToTest)
        {
      
            if ((startYearRngToTest >= _sourceRecord.YearStart && endYearRngToTest <= _sourceRecord.YearEnd))
            {
                return true;
            }
            else
            {
                return false;
            }

          
        }


        public static ServicePersonObject ToServicePersonObject(this IList<Person> persons, string sortColumn, int pageSize, int pageNumber)
        {
            ServicePersonObject spo = new ServicePersonObject();


            spo.servicePersons = persons.ToList().OrderBy(sortColumn).Select(p => new ServicePersonLookUp()
            {
                BirthLocation = p.BirthLocation,
                BirthYear = p.BirthInt,
                ChristianName = p.ChristianName,
                DeathLocation = p.DeathLocation,
                DeathYear = p.DeathInt,
                FatherChristianName = p.FatherChristianName,
                FatherSurname = p.Surname,
                MotherChristianName = p.MotherChristianName,
                MotherSurname = p.MotherSurname,
                PersonId = p.Person_id,
                Sources = p.Source,
                Surname = p.Surname,
                XREF = p.UniqueRef.ToString(),
                Events = p.TotalEvents.ToString(),
                Spouse = p.SpouseName
            }).ToList();


           spo.Total = spo.servicePersons.Count;


           if (pageSize != 0 )
           {
               spo.Batch = pageNumber;
               spo.BatchLength = pageSize;
               spo.servicePersons = spo.servicePersons.Skip(pageNumber * pageSize).Take(pageSize).ToList();
           }

            return spo;
        }

    }


}
