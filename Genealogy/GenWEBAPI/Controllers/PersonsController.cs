using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using GenOnline;
using GenOnline.Helpers;
using TDBCore.BLL;
using TDBCore.Types;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.enums;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenWEBAPI.Controllers
{
    public static class UriPersonMappings
    {
        //person


        
        public const string UpdateDateEstimates = "persons/updatedateestimates";
        public const string AssignLocations = "persons/assignlocations";        
        public const string SetPersonRelationship = "persons/setrelations";
        public const string SetPersonDuplicate = "persons/setduplicate";
        public const string MergeSources = "persons/mergepersons";
        public const string RemoveLinks = "persons/removelinks";

        public const string DeletePerson = "person/delete";
        public const string AddPerson = "person/add";
        public const string GetPerson = "person";
        public const string GetPersons = "persons";

    }

    public class ServicePersonData
    {
        public ServicePersonAdd servicePersonAdd { get; set; }

        public string sources { get; set; }
         
    }

    public class PersonsController : ApiController
    {

        private readonly PersonSearch _personSearch;

        public PersonsController(IPersonDal iPersonDal,            
            ISourceMappingsDal iSourceMappingsDal,
            ISecurity iSecurity)
        {
            _personSearch = new PersonSearch(iSecurity, iPersonDal, iSourceMappingsDal);
        }

        [Route(UriPersonMappings.AssignLocations)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AssignLocations()
        {
            string retVal = "";

            try
            {
                _personSearch.UpdateLocationsFromParishList();
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);
        }

        [Route(UriPersonMappings.UpdateDateEstimates)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult UpdateDateEstimates()
        {
            string retVal = "";

            try
            {
                _personSearch.UpdateDateEstimates();
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);
        }

        [Route(UriPersonMappings.SetPersonDuplicate)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult SetDuplicate(string persons)
        {
            string retVal = "";
          
            try
            {
                retVal = _personSearch.SetDuplicateRelation(persons.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);
        }

        [Route(UriPersonMappings.MergeSources)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult MergeSources(string person)
        {
            string retVal = "";

           
            try
            {
                retVal = _personSearch.MergeDuplicates(person.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);
        }

        [Route(UriPersonMappings.RemoveLinks)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult RemoveLink(string person)
        {
            string retVal = "";

            
            try
            {
                retVal = _personSearch.DelinkPersons(person.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);
        }

        [Route(UriPersonMappings.SetPersonRelationship)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult SetPersonRelation(string persons, string relationType)
        {
            string retVal = "";
            
            try
            {
                _personSearch.SetDuplicateRelation(persons.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);
        }

        [Route(UriPersonMappings.DeletePerson)]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeletePerson(string personId)
        {
            string retVal = "";

           
            try
            {
                _personSearch.DeleteRecords(personId.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(true);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriPersonMappings.AddPerson)]
        [HttpPost]
        public IHttpActionResult AddPerson(ServicePersonData servicePersonData)
        {


            string retVal = "";

            var personSearch = new PersonSearch(new Security(new WebUser()));

            var servicePerson = servicePersonData.servicePersonAdd.AsServicePerson();

            try
            {
                personSearch.Save(servicePerson, servicePersonData.sources.ParseToGuidList(), new PersonValidator(servicePerson));
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(servicePerson.PersonId);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriPersonMappings.GetPerson)]
        [HttpGet]
        public IHttpActionResult GetPerson(string id)
        {
            string retVal = "";

            var sp = new ServicePerson();
           
            try
            {
                sp = _personSearch.Get(id.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
                sp.ErrorStatus = "Exception: " + ex1.Message;
            }

            sp.ErrorStatus = retVal;

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            if (sp.PersonId == Guid.Empty)
            {
                return Content(HttpStatusCode.NotFound, id);
            }

            return Ok(sp);
            
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriPersonMappings.GetPersons)]
        [HttpGet]
        public IHttpActionResult GetPersons(string parentId,
            string christianName,
            string surname,
            string fatherChristianName,
            string fatherSurname,
            string motherChristianName,
            string motherSurname,
            string location,
            string county,
            string lowerDate,
            string upperDate,
            string filterTreeResults,
            string filterIncludeBirths,
            string filterIncludeDeaths,
            string filterIncludeSources,
            string sourceFilter,
            string spouse,
            string parishFilter,

            string pno,
            string psize,
            string sortcol)
        {
            string retVal = "";
         
            var spo = new ServicePersonObject();

            var personSearch = new PersonSearch(new Security(new WebUser()));

            try
            {

                if (parentId.ToGuid() == Guid.Empty)
                {

                    var searchParams = new PersonSearchFilter
                    {
                        CName = christianName ?? "",
                        Surname = surname ?? "",
                        FatherChristianName = fatherChristianName ?? "",
                        FatherSurname = fatherSurname ?? "",
                        MotherChristianName = motherChristianName ?? "",
                        MotherSurname = motherSurname ?? "",
                        Location = location ?? "",
                        LowerDate = lowerDate.ToInt32(),
                        UpperDate = upperDate.ToInt32(),
                        IsIncludeBirths = filterIncludeBirths.ToBool(),
                        IsIncludeDeaths = filterIncludeDeaths.ToBool(),
                        IsIncludeSources = filterIncludeSources.ToBool(),
                        SpouseChristianName = spouse ?? "",
                        SourceString = sourceFilter ?? "",
                        ParishString = parishFilter ?? ""
                    };


                    spo = personSearch.Search(PersonSearchTypes.Simple, searchParams,
                                  new DataShaping
                                  {
                                      RecordStart = pno.ToInt32(),
                                      RecordPageSize = psize.ToInt32()
                                  }, new PersonSearchValidator(searchParams));
                }
                else
                {

                    spo = personSearch.Search(PersonSearchTypes.Duplicates, new PersonSearchFilter { ParentId = parentId.ToGuid() },
                                        new DataShaping
                                        {
                                            RecordStart = 0,
                                            RecordPageSize = 25
                                        });

                }


            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;

                spo.ErrorStatus = retVal;
            }



            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }




            return Ok(spo);
        }

    }
}

