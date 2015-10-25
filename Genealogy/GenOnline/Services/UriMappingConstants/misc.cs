namespace GenOnline.Services.UriMappingConstants
{
    public static class UriMappingsMisc
    {
        //files
        public const string UploadFile = "/Upload{fileName}";

        public const string GetFilesForSource = "/Files/Select?0={sourceId}&1={page_number}&2={page_size}";
        //misc
        public const string GetLoggedInUserId = "/LoggedInUserId";
       
        public const string GetLoggedInUserName = "/LoggedInUserName";

        public const string DoNothing = "/DoNothing";
        
        public const string AddFile = "/user/Set";

        public const string GetEvents = "/TotalEvents/GetEvents/Select?" +
                                        "0={chkIncludeBirths}&1={chkIncludeDeaths}&2={chkIncludeWitnesses}&3={chkIncludeParents}&4={chkIncludeMarriages}&5={chkIncludeSpouses}&6={chkIncludePersonWithSpouses}&" +
                                        "7={christianName}&8={surname}&9={lowerDateRange}&10={upperDateRange}&11={location}" +

                                        "&12={page_number}&13={page_size}&14={sort_col}";
    }
}