using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using TDBCore.EntityModel; 
using TDBCore.Types.DTOs;

namespace TDBCore.Types.libs
{

    public static class MyExtensions
    {

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

        public static List<WitnessDto> DeserializeToMarriageWitnesses(this JavaScriptSerializer serializer, string marriageWitnesses, int year,
          string date,
          string location,
          Guid locationId )
        {
            var view = serializer.Deserialize<List<WitnessDto>>(marriageWitnesses);

            foreach(var witness in view)
            {
                witness.Date = date;
                witness.Location = location;
                witness.LocationId = locationId;
                witness.Year = year;

            }

            return view;
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

        public static string GetXName(this List<MarriageWitness> witnesses, int position)
        {
            return witnesses.Count > position && witnesses[position].Person != null
                       ? witnesses[position].Person.ChristianName
                       : "";
        }

        public static string GetXSurname(this List<MarriageWitness> witnesses, int position)
        {
            return witnesses.Count > position && witnesses[position].Person != null
                       ? witnesses[position].Person.Surname
                       : "";
        }

        public static string GetXDescription(this List<MarriageWitness> witnesses, int position)
        {
            return witnesses.Count > position
                       ? witnesses[position].Description
                       : "";
        }

        public static void PopulateServiceMarriage(this List<MarriageWitness> mw, ServiceMarriage marriage)
        {
            marriage.Witness1ChristianName = mw.GetXName(0);
            marriage.Witness1Surname = mw.GetXSurname(0);
            marriage.Witness1Description = mw.GetXDescription(0);
            marriage.Witness2ChristianName = mw.GetXName(1);
            marriage.Witness2Surname = mw.GetXSurname(1);
            marriage.Witness2Description = mw.GetXDescription(1);
            marriage.Witness3ChristianName = mw.GetXName(2);
            marriage.Witness3Surname = mw.GetXSurname(2);
            marriage.Witness3Description = mw.GetXDescription(2);
            marriage.Witness4ChristianName = mw.GetXName(3);
            marriage.Witness4Surname = mw.GetXSurname(3);
            marriage.Witness4Description = mw.GetXDescription(3);
            marriage.Witness5ChristianName = mw.GetXName(4);
            marriage.Witness5Surname = mw.GetXSurname(4);
            marriage.Witness5Description = mw.GetXDescription(4);
            marriage.Witness6ChristianName = mw.GetXName(5);
            marriage.Witness6Surname = mw.GetXSurname(5);
            marriage.Witness6Description = mw.GetXDescription(5);
            marriage.Witness7ChristianName = mw.GetXName(6);
            marriage.Witness7Surname = mw.GetXSurname(6);
            marriage.Witness7Description = mw.GetXDescription(6);
            marriage.Witness8ChristianName = mw.GetXName(7);
            marriage.Witness8Surname = mw.GetXSurname(7);
            marriage.Witness8Description = mw.GetXDescription(7);

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

        public static string ParseToCSV(this List<Guid> str)
        {
            if (str.IsNullOrBelowMinSize()) return "";

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

            string retVal = str.Aggregate("", (current, _int) => current + ("," + _int));

            if (retVal.StartsWith(",")) retVal = retVal.Remove(0, 1);

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

            foreach (var rec in orderedList)
            {
                if (rec.ContainsYearRange(startYear, endYear))
                {
                    isFound = true;
                }
            }


            return isFound;
        }

        public static bool ContainsYearRange(this SourceRecord sourceRecord, int startYearRngToTest, int endYearRngToTest)
        {
            if ((startYearRngToTest >= sourceRecord.YearStart && endYearRngToTest <= sourceRecord.YearEnd))
            {
                return true;
            }
            return false;
        }


        public static IList<ServicePerson> ToServicePersons(this IList<Person> persons)
        {
            var spList = new List<ServicePerson>();

            foreach (Person person in persons)
            {
                spList.Add(person.ToServicePerson());
            }

            return spList;
        }

        //ServiceMarriageObject


        public static ServiceMarriageObject ToServiceMarriageObject(this IList<MarriageResult> marriages, string sortColumn, int pageSize, int pageNumber)
        {
            ServiceMarriageObject smo = new ServiceMarriageObject();


            smo.serviceMarriages = marriages.OrderBy(sortColumn).Select(m => new ServiceMarriageLookup()
                {
                    MarriageId  = m.MarriageId,
                    MaleCName = m.MaleCName,
                    MaleSName = m.MaleSName,
                    FemaleCName = m.FemaleCName,
                    FemaleSName = m.FemaleSName,
                    MarriageDate = m.MarriageYear.ToString(),
                    MarriageLocation = m.MarriageLocation,
                    Witnesses = m.Witnesses,
                    UniqueRef = m.UniqueRef.ToString(),
                    Sources = m.MarriageSource,
                    TotalEvents = m.MarriageTotalEvents.ToString(),
                    LinkedTrees = m.SourceTrees
                }).ToList();

            smo.Total = smo.serviceMarriages.Count;
            
            if (pageSize != 0)
            {
                smo.Batch = pageNumber;
                smo.BatchLength = pageSize;
                smo.serviceMarriages = smo.serviceMarriages.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            }


            return smo;
        }

        public static ServiceSourceTypeObject ToServiceSourceTypeObject(this List<ServiceSourceType> sourceTypes, string sortColumn, int pageSize, int pageNumber)
        {
            var spo = new ServiceSourceTypeObject { serviceSources = sourceTypes.ToList() };

            spo.Total = spo.serviceSources.Count;

            if (pageSize != 0)
            {
                spo.Batch = pageNumber;
                spo.BatchLength = pageSize;
                spo.serviceSources = spo.serviceSources.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            }

            return spo;
        }


        public static SourceAjaxDto ToSourceAjaxDto(this SourceDto sourceDto)
        {

            return  new SourceAjaxDto
            {
                    Files = sourceDto.Files.Select(x=> new FileBasicInfo
                    {
                           Description = x.FileDescription,
                           FileId = x.FileId,
                           Url = x.FileLocation
                        }).ToList(),
                    SourceId = sourceDto.SourceId,
                    Parishs = sourceDto.Parishs.ParseToCSV(),
                    SourceRef = sourceDto.SourceRef,
                    SourceTypes = sourceDto.SourceTypes.ParseToCSV(),
                    SourceNotes = sourceDto.SourceNotes,
                    SourceFileCount = sourceDto.SourceFileCount,
                    SourceDesc = sourceDto.SourceDesc,
                    SourceDateStrTo = sourceDto.SourceDateStrTo,
                    SourceDateStr = sourceDto.SourceDateStr,
                    OriginalLocation = sourceDto.OriginalLocation,
                    IsViewed = sourceDto.IsViewed,
                    IsThackrayFound = sourceDto.IsThackrayFound,
                    IsCopyHeld = sourceDto.IsCopyHeld                   ,
                    ErrorStatus = sourceDto.ErrorStatus
                };
        }


        public static ServiceSourceObject ToServiceSourceObject(this IList<ServiceSource> sources, string sortColumn, int pageSize, int pageNumber)
        {
            var spo = new ServiceSourceObject {serviceSources = sources.ToList()};

            spo.Total = spo.serviceSources.Count;
           
            if (pageSize != 0)
            {
                spo.Batch = pageNumber;
                spo.BatchLength = pageSize;
                spo.serviceSources = spo.serviceSources.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            }

            return spo;
        }

        public static ServicePersonObject ToServicePersonObject(this IList<ServicePerson> persons, string sortColumn, int pageSize, int pageNumber)
        {
            var spo = new ServicePersonObject
            {
                servicePersons = persons.Select(p => new ServicePersonLookUp
                {
                    BirthLocation = p.BirthLocation,
                    BirthYear = p.BirthYear,
                    ChristianName = p.ChristianName,
                    DeathLocation = p.DeathLocation,
                    DeathYear = p.DeathYear,
                    FatherChristianName = p.FatherChristianName,
                    FatherSurname = p.Surname,
                    MotherChristianName = p.MotherChristianName,
                    MotherSurname = p.MotherSurname,
                    PersonId = p.PersonId,
                    Sources = p.Sources,
                    Surname = p.Surname,
                    UniqueReference = p.UniqueReference,
                    Events = p.Events,
                    Spouse = p.Spouse.Trim(),
                    LinkedTrees = p.LinkedTrees,
                    SourceParishName = p.SourceParishName,
                    SourceDateInt = p.SourceDateInt,
                    SourceDateStr = p.SourceDateStr,
                    ReferenceLocation = p.ReferenceLocation,
                    SourceRef = p.SourceRef,
                    SourceId = p.SourceId
                }).ToList()
            };


            spo.Total = spo.servicePersons.Count;
          

            if (pageSize != 0)
            {
                spo.Batch = pageNumber;
                spo.BatchLength = pageSize;
                spo.servicePersons = spo.servicePersons.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            }

            return spo;
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
                UniqueReference = p.UniqueRef.ToString(),
                Events = p.TotalEvents.ToString(),
                Spouse = p.SpouseName.Trim(),
                LinkedTrees = p.OrigMotherSurname
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

        public static ServicePersonObject ToServicePersonObject(this IList<Person> persons)
        {
            ServicePersonObject spo = new ServicePersonObject();


            spo.servicePersons = persons.ToList().Select(p => new ServicePersonLookUp()
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
                UniqueReference = p.UniqueRef.ToString(),
                Events = p.TotalEvents.ToString(),
                Spouse = p.SpouseName.Trim(),
                LinkedTrees = p.OrigMotherSurname
            }).ToList();


            spo.Total = spo.servicePersons.Count;        
            return spo;
        }

        public static ServicePersonObject Paginate(this ServicePersonObject persons, string sortColumn, int pageSize, int pageNumber)
        {
            ServicePersonObject spo = new ServicePersonObject();


            spo.Total = spo.servicePersons.Count;


            if (pageSize != 0)
            {
                spo.Batch = pageNumber;
                spo.BatchLength = pageSize;
                spo.servicePersons = spo.servicePersons.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            }

         


            return spo;
        }
    
        public static ServicePerson ToServicePerson(this Person person)
        {
            
            var sp = new ServicePerson()
                                   {
                                       Baptism = person.BaptismDateStr,
                                       Birth = person.BirthDateStr,
                                       BirthCounty = person.BirthCounty,
                                       BirthLocation = person.BirthLocation,
                                       BirthLocationId = person.BirthLocationId.ToString(),
                                       BirthYear = person.BirthInt,
                                       ChristianName = person.ChristianName,
                                       Death = person.DeathDateStr,
                                       DeathCounty = person.DeathCounty,
                                       DeathLocation = person.DeathLocation,
                                       FatherChristianName = person.FatherChristianName,
                                       FatherOccupation = person.FatherOccupation,
                                       FatherSurname = person.FatherSurname,
                                       LinkedTrees = "SP_only",
                                       MotherChristianName = person.MotherChristianName,
                                       MotherSurname = person.MotherSurname,
                                       Notes = person.Notes,
                                       Occupation = person.Occupation,
                                       PersonId = person.Person_id,
                                       ReferenceDate = person.ReferenceDateStr,
                                       ReferenceLocation = person.ReferenceLocation,
                                       ReferenceLocationId = person.ReferenceLocationId.ToString(),
                                       SourceDescription = person.Source,
                                       Spouse = (person.SpouseName + " " + person.SpouseSurname).ToString().Trim(),
                                       SpouseChristianName = person.SpouseName,
                                       SpouseSurname = person.SpouseSurname,
                                       Surname = person.Surname,
                                       UniqueReference = person.UniqueRef.ToString(),
                                       Events = person.TotalEvents.ToString()
                                       
                                   };

            return sp;
        }

        public static ServiceMarriage ToServiceMarriage(this Marriage marriageRecord)
        {

            var sp = new ServiceMarriage()
                {
                    MarriageDate = marriageRecord.Date,
                    MarriageLocation = marriageRecord.MarriageLocation,
                    LocationCounty = marriageRecord.MarriageCounty,
                    Sources = marriageRecord.Source,
                    MaleCName = marriageRecord.MaleCName,
                    MaleSName = marriageRecord.MaleSName,
                    MaleNotes = marriageRecord.MaleInfo,
                    MaleLocation = marriageRecord.MaleLocation,
                    FemaleCName = marriageRecord.FemaleCName,
                    FemaleSName = marriageRecord.FemaleSName,
                    FemaleNotes = marriageRecord.FemaleInfo,
                    FemaleLocation = marriageRecord.FemaleLocation,
                    FemaleOccupation = marriageRecord.FemaleOccupation,
                    MaleOccupation = marriageRecord.MaleOccupation,
                    IsBanns = marriageRecord.IsBanns.GetValueOrDefault(),
                    IsLicense = marriageRecord.IsLicence.GetValueOrDefault(),
                    IsWidow = marriageRecord.FemaleIsKnownWidow.GetValueOrDefault(),
                    IsWidower = marriageRecord.MaleIsKnownWidower.GetValueOrDefault(),
                    LocationId = marriageRecord.MarriageLocationId.GetValueOrDefault().ToString(),
                    MaleLocationId = marriageRecord.MaleLocationId.GetValueOrDefault().ToString(),
                    FemaleLocationId = marriageRecord.FemaleLocationId.GetValueOrDefault().ToString(),
                    MaleBirthYear = marriageRecord.MaleBirthYear.GetValueOrDefault(),
                    FemaleBirthYear = marriageRecord.FemaleBirthYear.GetValueOrDefault(),
                    MarriageId = marriageRecord.UniqueRef.GetValueOrDefault()                  
                };                  
            return sp;
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
