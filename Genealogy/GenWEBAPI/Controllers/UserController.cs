using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using GenOnline.Helpers;

namespace GenWEBAPI.Controllers
{
    public static class UriMappingsMisc
    {
       
        //misc
        public const string GetLoggedInUserId = "userid";

        public const string GetLoggedInUserName = "username";
 

 
    }

    public class UserController : ApiController
    {
        [Route(UriMappingsMisc.GetLoggedInUserName)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public string GetLoggedInUserName()
        {
            string user;

            try
            {
                user = WebHelper.GetUserName();
            }
            catch (Exception e)
            {
                user = e.Message;

            }

            return user;
        }

        [Route(UriMappingsMisc.GetLoggedInUserId)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public string GetLoggedInUserId()
        {
            string user;

            try
            {
                user = WebHelper.GetUser();
            }
            catch (Exception e)
            {
                user = e.Message;

            }

            return user;
        }

    }



}
