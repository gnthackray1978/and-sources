using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using TDBCore.BLL;
using TDBCore.EntityModel;
using TDBCore.Types.domain.import;
using TDBCore.Types.DTOs;

namespace TDBCore.Types.libs
{
    public static class DomainExtensions
    {

      
        public static string SourceDto(this CSVField self, SourceDto sourceDto)
        {
            switch (self)
            {               
                case CSVField.SourceDate:
                    return sourceDto.SourceDateStr;                    
                case CSVField.SourceDateTo:
                    return sourceDto.SourceDateStrTo;    
                case CSVField.SourceRef:
                    return sourceDto.SourceRef;
                case CSVField.SourceParish:
                    return sourceDto.Parishs.ParseToCSV("|");
                case CSVField.SourceType:
                    return sourceDto.SourceTypes.ParseToCSV("|");
                case CSVField.SourceDesc:
                    return sourceDto.SourceDesc;
                case CSVField.SourceOrigLocat:
                    return sourceDto.OriginalLocation;
                case CSVField.IsCopyHeld:
                    return sourceDto.IsCopyHeld.ToString();
                case CSVField.IsViewed:
                    return sourceDto.IsViewed.ToString();
                case CSVField.IsThackrayFound:
                    return sourceDto.IsThackrayFound.ToString();
                case CSVField.Notes:
                    return sourceDto.SourceNotes;
                case CSVField.SourceId:
                    return sourceDto.SourceId.ToString();
            }

            return "";
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

        public static IList<ServicePerson> ToServicePersons(this IList<Person> persons)
        {
            return persons.Select(person => person.ToServicePerson()).ToList();
        }

        public static ServiceMarriageObject ToServiceMarriageObject(this IList<MarriageResult> marriages, string sortColumn, int pageSize, int pageNumber)
        {
            var smo = new ServiceMarriageObject
            {
                serviceMarriages = marriages.OrderBy(sortColumn).Select(m => new ServiceMarriageLookup
                {
                    MarriageId = m.MarriageId,
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
                    LinkedTrees = m.SourceTrees,
                    Notes = m.Notes
                }).ToList()
            };


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

        public static ServiceBatchObject ToServiceBatchObject(this List<BatchDto> batchList, string sortColumn, int pageSize, int pageNumber)
        {
            var sbo = new ServiceBatchObject { batchContents = batchList.ToList() };

            sbo.Total = sbo.batchContents.Count;

            if (pageSize != 0)
            {
                sbo.Batch = pageNumber;
                sbo.BatchLength = pageSize;
                sbo.batchContents = sbo.batchContents.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            }

            return sbo;
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
       
        public static ServicePerson ToServicePerson(this Person person)
        {
            
            var sp = new ServicePerson()
            {
                Baptism = person.BaptismDateStr,
                Birth = person.BirthDateStr,
                BirthCounty = person.BirthCounty,
                BirthLocation = person.BirthLocation,
                BirthLocationId = person.BirthLocationId.ToString(),
                BirthYear = person.BirthInt == 0 ? person.BapInt : person.BirthInt,
                ChristianName = person.ChristianName,
                DeathYear = person.DeathInt,
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
                Events = person.TotalEvents.ToString(),
                SourceParishName = "",
                SourceDateInt = 0,
                SourceDateStr = "",
                SourceRef = ""
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


        public static void MergeInto(this Person _person, Person newPerson, SourceDal sourceDal)
        {
         
            

            Guid dummyLocation = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699");

            if (_person.SpouseSurname == "")
                _person.SpouseSurname = newPerson.SpouseSurname;

            if (_person.SpouseName == "")
                _person.SpouseName = newPerson.SpouseName;


            if ((_person.ReferenceLocationId == dummyLocation || _person.ReferenceLocationId == Guid.Empty) &&
                newPerson.ReferenceLocationId != dummyLocation && newPerson.ReferenceLocationId != Guid.Empty)
                _person.ReferenceLocationId = newPerson.ReferenceLocationId;

            if ((_person.DeathLocationId == dummyLocation || _person.ReferenceLocationId == Guid.Empty) && 
                newPerson.DeathLocationId != dummyLocation && newPerson.ReferenceLocationId != Guid.Empty)
                _person.DeathLocationId = newPerson.DeathLocationId;

            if ((_person.BirthLocationId == dummyLocation || _person.ReferenceLocationId == Guid.Empty) &&
                newPerson.BirthLocationId != dummyLocation && newPerson.ReferenceLocationId != Guid.Empty)
                _person.BirthLocationId = newPerson.BirthLocationId;



            if (newPerson.ReferenceLocation != "" && _person.ReferenceLocation == "")
                _person.ReferenceLocation = newPerson.ReferenceLocation;

            if (newPerson.ReferenceDateStr != "" && _person.ReferenceDateStr == "")
                _person.ReferenceDateStr = newPerson.ReferenceDateStr;



            if (newPerson.Occupation != "" && _person.Occupation == "")
            {
                if (_person.Occupation.Trim() == "")
                {
                    _person.Occupation = newPerson.Occupation;
                }
                else
                {
                    _person.Occupation += " " + newPerson.Occupation;
                }
            }


            if (newPerson.Notes != "")
            {
                if (_person.Notes.Trim() == "")
                {
                    _person.Notes = newPerson.Notes;
                }
                else
                {
                    _person.Notes += " " + newPerson.Notes;
                }

            }

            if (newPerson.MotherSurname != "" && _person.MotherSurname == "")
                _person.MotherSurname = newPerson.MotherSurname;

            if (newPerson.MotherChristianName != "" && _person.MotherChristianName == "")
                _person.MotherChristianName = newPerson.MotherChristianName;

            if (newPerson.FatherChristianName != "" && _person.FatherChristianName == "")
                _person.FatherChristianName = newPerson.FatherChristianName;

            if (newPerson.FatherOccupation != "" && _person.FatherOccupation == "")
                _person.FatherOccupation = newPerson.FatherOccupation;

            if (newPerson.DeathCounty != "" && _person.DeathCounty == "")
                _person.DeathCounty = newPerson.DeathCounty;

            if (newPerson.DeathDateStr != "" && _person.DeathDateStr == "")
                _person.DeathDateStr = newPerson.DeathDateStr;

            if (newPerson.ReferenceDateInt > 0 && _person.ReferenceDateInt == 0)
                _person.ReferenceDateInt = newPerson.ReferenceDateInt;

            if (newPerson.DeathInt > 0 && _person.DeathInt == 0)
                _person.DeathInt = newPerson.DeathInt;

            if (newPerson.BirthInt > 0 && _person.BirthInt == 0)
                _person.BirthInt = newPerson.BirthInt;

            if (newPerson.BapInt > 0 && _person.BapInt == 0)
                _person.BapInt = newPerson.BapInt;


            if (newPerson.DeathLocation != "" && _person.DeathLocation == "")
                _person.DeathLocation = newPerson.DeathLocation;

            if (newPerson.BirthCounty != "" && _person.BirthCounty == "")
                _person.BirthCounty = newPerson.BirthCounty;

            if (newPerson.BirthDateStr != "" && _person.BirthDateStr == "")
                _person.BirthDateStr = newPerson.BirthDateStr;

            if (newPerson.BirthLocation != "" && _person.BirthLocation == "")
                _person.BirthLocation = newPerson.BirthLocation;

            if (newPerson.BaptismDateStr != "" && _person.BaptismDateStr == "")
                _person.BaptismDateStr = newPerson.BaptismDateStr;

            _person.IsMale = newPerson.IsMale;

         
            string source = _person.Source + Environment.NewLine + sourceDal.MakeSourceString(newPerson.Person_id);


            // if(source.Length >49)
            _person.Source = "Multiple sources";
            //  else
            //     _person.Source = source;




            int estBYear = 0;
            int estDYear =0;
            bool isEstBYear =false;
            bool isEstDYear = false;

            DateTools.CalcEstDates(_person.BirthInt, _person.BapInt, _person.DeathInt, out estBYear, out estDYear, out isEstBYear, out isEstDYear, _person.FatherChristianName, _person.MotherChristianName);

            _person.EstBirthYearInt = estBYear;
            _person.EstDeathYearInt = estDYear;
            _person.IsEstBirth = isEstBYear;
            _person.IsEstDeath = isEstDYear;

        }



        public static IEnumerable<MarriageWitness> RemoveDuplicateReferences(this IList<MarriageWitness> list)
        {
            var ecP = new EqualityComparer<MarriageWitness>((o1, o2) => o1.Person.ChristianName == o2.Person.ChristianName
                                                                                                                             && o1.Person.Surname == o2.Person.Surname
                                                                                                                             && o1.Person.ReferenceDate == o2.Person.ReferenceDate,
                o => (o.Person.ReferenceDate.GetHashCode() + o.Person.Surname.GetHashCode() + o.Person.ChristianName.GetHashCode()));

            IList<MarriageWitness> p = new List<MarriageWitness>();
            IList<MarriageWitness> dupes = new List<MarriageWitness>();

            int idx = 0;

            while (idx < list.Count)
            {

                if (!p.Contains(list[idx], ecP))
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

        public static void MergeInto(this Marriage marriage, Marriage newMarriage)
        {

            var dummyGuid = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699");

            if(marriage.FemaleId.GetValueOrDefault() == Guid.Empty 
               && newMarriage.FemaleId.GetValueOrDefault() == Guid.Empty)
                marriage.FemaleId = Guid.Empty;

            if (marriage.MaleId.GetValueOrDefault() == Guid.Empty
                && newMarriage.MaleId.GetValueOrDefault() == Guid.Empty)
                marriage.MaleId = Guid.Empty;

            if (marriage.Date == "" && newMarriage.Date != "")
                marriage.Date = newMarriage.Date;

            if(marriage.FemaleBirthYear == 0 && newMarriage.FemaleBirthYear >0)
                marriage.FemaleBirthYear = newMarriage.FemaleBirthYear;
          
            if (marriage.YearIntVal == 0 && newMarriage.YearIntVal > 0)
                marriage.YearIntVal = newMarriage.YearIntVal;

            if (marriage.MaleBirthYear == 0 && newMarriage.MaleBirthYear > 0)
                marriage.MaleBirthYear = newMarriage.MaleBirthYear;


            if (marriage.FemaleCName == "" && newMarriage.FemaleCName != "")
                marriage.FemaleCName = newMarriage.FemaleCName;

            if (marriage.FemaleInfo == "" && newMarriage.FemaleInfo != "")
                marriage.FemaleInfo = newMarriage.FemaleInfo;

            if ((marriage.MaleLocationId.GetValueOrDefault() == dummyGuid || marriage.MaleLocationId.GetValueOrDefault() == Guid.Empty)
                && (newMarriage.MaleLocationId.GetValueOrDefault() != dummyGuid && newMarriage.MaleLocationId.GetValueOrDefault() != Guid.Empty))
                marriage.MaleLocationId = newMarriage.MaleLocationId;

            if ((marriage.MarriageLocationId.GetValueOrDefault() == dummyGuid || marriage.MarriageLocationId.GetValueOrDefault() == Guid.Empty)
                && (newMarriage.MarriageLocationId.GetValueOrDefault() != dummyGuid && newMarriage.MarriageLocationId.GetValueOrDefault() != Guid.Empty))
                marriage.MarriageLocationId = newMarriage.MarriageLocationId;

            if ((marriage.FemaleLocationId.GetValueOrDefault() == dummyGuid || marriage.FemaleLocationId.GetValueOrDefault() == Guid.Empty)
                && (newMarriage.FemaleLocationId.GetValueOrDefault() != dummyGuid && newMarriage.FemaleLocationId.GetValueOrDefault() != Guid.Empty))
                marriage.FemaleLocationId = newMarriage.FemaleLocationId;

            if (newMarriage.IsBanns.GetValueOrDefault())
                marriage.IsBanns = newMarriage.IsBanns;

            if (newMarriage.IsLicence.GetValueOrDefault())
                marriage.IsLicence = newMarriage.IsLicence;

            if (newMarriage.FemaleIsKnownWidow.GetValueOrDefault())
                marriage.FemaleIsKnownWidow = newMarriage.FemaleIsKnownWidow;

            if (newMarriage.MaleIsKnownWidower.GetValueOrDefault())
                marriage.MaleIsKnownWidower = newMarriage.MaleIsKnownWidower;

            if (marriage.FemaleOccupation == "" && newMarriage.FemaleOccupation != "")
                marriage.FemaleOccupation = newMarriage.FemaleOccupation;

            if (marriage.FemaleSName == "" && newMarriage.FemaleSName != "")
                marriage.FemaleSName = newMarriage.FemaleSName;

            if (marriage.MaleCName == "" && newMarriage.MaleCName != "")
                marriage.MaleCName = newMarriage.MaleCName;

            if (marriage.MaleInfo == "" && newMarriage.MaleInfo != "")
                marriage.MaleInfo = newMarriage.MaleInfo;

            if (marriage.FemaleLocation == "" && newMarriage.FemaleLocation != "")
                marriage.FemaleLocation = newMarriage.FemaleLocation;

            if (marriage.MaleLocation == "" && newMarriage.MaleLocation != "")
                marriage.MaleLocation = newMarriage.MaleLocation;

            if (marriage.MaleOccupation == "" && newMarriage.MaleOccupation != "")
                marriage.MaleOccupation = newMarriage.MaleOccupation;

            if (marriage.MaleSName == "" && newMarriage.MaleSName != "")
                marriage.MaleSName = newMarriage.MaleSName;

            if (marriage.MarriageCounty == "" && newMarriage.MarriageCounty != "")
                marriage.MarriageCounty = newMarriage.MarriageCounty;

            if (marriage.MarriageLocation == "" && newMarriage.MarriageLocation != "")
                marriage.MarriageLocation = newMarriage.MarriageLocation;

            if (marriage.OrigFemaleSurname == "" && newMarriage.OrigFemaleSurname != "")
                marriage.OrigFemaleSurname = newMarriage.OrigFemaleSurname;

            if (marriage.OrigMaleSurname == "" && newMarriage.OrigMaleSurname != "")
                marriage.OrigMaleSurname = newMarriage.OrigMaleSurname;

            if (marriage.Source == "" && newMarriage.Source != "")
                marriage.Source = newMarriage.Source;




        }

     }
}