using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using GenOnline.Helpers;
using TDBCore.BLL;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.enums;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenWEBAPI.Controllers
{
    public static class UriMappingMarriage
    {
        //marriages
        public const string AddMarriage = "/marriage";

        public const string GetMarriages = "/marriages";

        public const string GetMarriage = "/marriage";

        public const string DeleteMarriages = "/marriage/delete";

        public const string SetMarriageDuplicate = "/marriage/createduplicate";
        public const string MergeMarriages = "/marriage/mergemarriages";
        public const string RemoveMarriageLinks = "/marriage/removelinks";
        public const string ReorderMarriages = "/marriage/reorder";
        public const string SwitchSpouses = "/marriage/switchspouses";

    }
    public class MyClass
    {
        public int Id { get; set; }

        public string description { get; set; }
    }
    public class LoginInfo
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class TestController : ApiController
    {
        private readonly MarriageSearch _marriageSearch;

        public TestController(IMarriagesDal iMarriagesDal,
            IMarriageWitnessesDal iMarriageWitnessesDal,
            ISourceDal iSourceDal,
            ISourceMappingsDal iSourceMappingsDal,
            IPersonDal iPersonDal,
            ISecurity iSecurity)
        {
            _marriageSearch = new MarriageSearch(iSecurity,
                iMarriagesDal,
                iMarriageWitnessesDal, iSourceDal, iSourceMappingsDal, iPersonDal);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // [Route("/Marriage?0={id}")]
        [Route("api/test")]
        [HttpPost]
        public IHttpActionResult GetSomeRows(LoginInfo loginInfo)
        {
            var a = "";
            try
            {
                a = loginInfo.username;
            }
            catch (Exception e)
            {
                a = e.Message;
            }

            var r = new List<MyClass>
            {
                new MyClass {Id = 1 , description = a}
            };

            return Ok(r);
        }




        [Route(UriMappingMarriage.GetMarriage)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetMarriage(string id)
        {

            // ahouls use search function here     
            string retVal = "";

            var serviceMarriage = new ServiceMarriage();

            try
            {

                serviceMarriage = _marriageSearch.Get(id.ToGuid());

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }


            serviceMarriage.ErrorStatus = retVal;

            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }
            
            return Ok(serviceMarriage);
        }



        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.GetMarriages)]
        [HttpGet]
        public IHttpActionResult GetMarriages(string uniqref, string malecname, string malesname, string femalecname,
            string femalesname, string location, string lowerDate, string upperDate, string sourceFilter, string parishFilter, string marriageWitness,
            string pno, string psize, string sortcol)
        {
            //var iModel = new MarriageSearch(new Security(new WebUser()));


            var serviceMarriageObject = new ServiceMarriageObject();

            string retVal = "";

            try
            {

                Guid parentId = uniqref.ToGuid();


                if (parentId == Guid.Empty)
                {
                    var marriageFilter = new MarriageSearchFilter()
                    {
                        MaleCName = malecname,
                        MaleSName = malesname,
                        FemaleCName = femalecname,
                        FemaleSName = femalesname,
                        Location = location,
                        LowerDate = lowerDate.ToInt32(),
                        UpperDate = upperDate.ToInt32(),
                        Witness = marriageWitness,
                        Parish = parishFilter,
                        Source = sourceFilter,
                        ParentId = Guid.Empty
                    };

                    var marriageValidation = new MarriageSearchValidator(marriageFilter);


                    serviceMarriageObject = _marriageSearch.Search(MarriageFilterTypes.Standard, marriageFilter,
                                  new DataShaping()
                                  {
                                      Column = sortcol,
                                      RecordPageSize = psize.ToInt32(),
                                      RecordStart = pno.ToInt32()
                                  }, marriageValidation);


                }
                else
                {
                    serviceMarriageObject = _marriageSearch.Search(MarriageFilterTypes.Duplicates, new MarriageSearchFilter() { ParentId = parentId },
                                  new DataShaping()
                                  {
                                      Column = sortcol,
                                      RecordPageSize = psize.ToInt32(),
                                      RecordStart = 0
                                  });

                }

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
           
            if (retVal != "")
            {
                return Content(HttpStatusCode.BadRequest, retVal);
            }

            return Ok(serviceMarriageObject);
            
        }



        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.DeleteMarriages)]
        [HttpPost]
        public IHttpActionResult DeleteMarriages(string marriageIds)
        {
            string retVal = "";

            try
            {
                _marriageSearch.DeleteRecords(marriageIds.ParseToGuidList());
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
        [Route(UriMappingMarriage.SetMarriageDuplicate)]
        [HttpPost]
        public IHttpActionResult SetMarriageDuplicate(string marriages)
        {
            string retVal = "";

            try
            {
                _marriageSearch.SetSelectedDuplicateMarriage(marriages.ParseToGuidList());
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
        [Route(UriMappingMarriage.RemoveMarriageLinks)]
        [HttpPost]
        public string RemoveMarriageLink(string marriage)
        {
            // var iModel = new MarriageSearch(new Security(new WebUser()));    
            string retVal = "";

            try
            {
                _marriageSearch.SetRemoveSelectedFromDuplicateList(marriage.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            return WebHelper.MakeReturn(marriage, retVal);

        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.ReorderMarriages)]
        [HttpPost]
        public string ReorderMarriages(string marriage)
        {
            //  var iModel = new MarriageSearch(new Security(new WebUser()));

            string retVal = "";

            try
            {
                _marriageSearch.SetReorderDupes(marriage.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            return WebHelper.MakeReturn(marriage, retVal);

        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.SwitchSpouses)]
        [HttpPost]
        public string SwitchSpouses(string marriage)
        {

            string retVal = "";

            try
            {
                _marriageSearch.SwitchSpouses(marriage.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            return WebHelper.MakeReturn(marriage, retVal);
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.MergeMarriages)]
        [HttpPost]
        public string MergeMarriage(string marriage)
        {

            string retVal = "";

            try
            {
                _marriageSearch.SetMergeSources(marriage.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            return WebHelper.MakeReturn(marriage, retVal);
        }



        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route(UriMappingMarriage.AddMarriage)]
        [HttpPost]
        public string AddMarriage(ServiceMarriage serviceMarriage,string sources, string marriageWitnesses)
        {



            //WebHelper.WriteParams(FemaleLocationId, LocationId, MaleLocationId, SourceDescription, Sources, MarriageId, IsBanns, IsLicense, IsWidow, IsWidower,
            //         FemaleBirthYear, FemaleCName, FemaleLocation, FemaleNotes, FemaleOccupation, FemaleSName, LocationCounty, MaleBirthYear, MaleCName,
            //         MaleLocation, MaleNotes, MaleOccupation, MaleSName, MarriageDate, MarriageLocation);

            string retVal = "";

            //var serviceMarriage = new ServiceMarriage
            //{
            //    MarriageId = MarriageId.ToGuid(),
            //    MarriageDate = MarriageDate,
            //    MaleCName = MaleCName,
            //    MaleSName = MaleSName,
            //    FemaleCName = FemaleCName,
            //    FemaleSName = FemaleSName,
            //    MaleNotes = MaleNotes,
            //    FemaleNotes = FemaleNotes,
            //    MarriageLocation = MarriageLocation,
            //    LocationId = LocationId,
            //    LocationCounty = LocationCounty,
            //    MaleLocation = MaleLocation,
            //    FemaleLocation = FemaleLocation,
            //    IsBanns = IsBanns.ToBool(),
            //    IsLicense = IsLicense.ToBool(),
            //    IsWidow = IsWidow.ToBool(),
            //    IsWidower = IsWidower.ToBool(),
            //    MaleOccupation = MaleOccupation,
            //    FemaleOccupation = FemaleOccupation,
            //    MaleBirthYear = MaleBirthYear.ToInt32(),
            //    FemaleBirthYear = FemaleBirthYear.ToInt32(),
            //    SourceDescription = SourceDescription
            //};

            try
            {
                _marriageSearch.Save(serviceMarriage, sources.ParseToGuidList(), MarriageWitness.DeSerializeWitnesses(marriageWitnesses, serviceMarriage), new MarriageValidator(serviceMarriage));
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            return WebHelper.MakeReturn(serviceMarriage.MarriageId.ToString(), retVal);

        }
    }
}