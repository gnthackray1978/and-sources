
namespace TancWebApp.URIMappings
{
    public static class UriMappingsEvents
    {
        public const string GetEvents = "/TotalEvents/GetEvents/Select?" +
                                        "0={chkIncludeBirths}&1={chkIncludeDeaths}&2={chkIncludeWitnesses}&3={chkIncludeParents}&4={chkIncludeMarriages}&5={chkIncludeSpouses}&6={chkIncludePersonWithSpouses}&" +
                                        "7={christianName}&8={surname}&9={lowerDateRange}&10={upperDateRange}&11={location}" +

                                        "&12={page_number}&13={page_size}&14={sort_col}";
    }
}