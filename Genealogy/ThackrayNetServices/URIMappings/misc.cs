using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANDServices
{
    public static class UriMappingsMisc
    {

        //test
        public const string TestLogin = "/TestLogin?0={testParam}";
        public const string TestRest = "/TestRest?0={testParam}";


        //files
        public const string UploadFile = "/Upload{fileName}";

        public const string GetFilesForSource = "/Files/Select?0={sourceId}&1={page_number}&2={page_size}";




        //misc

        public const string GetLoggedInUser = "/LoggedInUser";


        public const string AddFile = "/user/Set";
    }
}