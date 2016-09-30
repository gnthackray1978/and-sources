using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using GenOnline.Helpers;
using GenOnline.Services.UriMappingConstants;
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
    //test comment

    public class TestController2 : ApiController
    {

        private readonly MarriageSearch _marriageSearch;

        public TestController2(IMarriagesDal iMarriagesDal,
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
    }
}