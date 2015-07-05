 

namespace TancWebApp.Services.UriMappingConstants
{
    public static class UriSourceMappings
    {
        public const string Get1841CensusSources = "/Get1841CensusSources?0={sourceId}";

        public const string Get1841CensusPlaces = "/Get1841CensusPlaces";

        public const string GetSourceNames = "/GetSourceNames?0={sourceIds}";

        public const string DeleteSource = "/Delete";

        public const string GetSources = "/Select?0={sourceTypes}&1={sourceRef}&2={sourceDesc}&3={origLoc}" +
                                         "&4={dateLB}&5={toDateLB}&6={dateUB}&7={toDateUB}&8={fileCount}&9={isThackrayFound}" +
                                         "&10={isCopyHeld}&11={isViewed}&12={isChecked}&13={page_number}&14={page_size}&15={sortColumn}";

        public const string GetSource = "/GetSource?0={sourceId}";

        public const string AddSource = "/Add";

        public const string AddPersonTreeSource = "/AddPersonTreeSource";
        public const string AddMarriageTreeSource = "/AddMarriageTreeSource";

        public const string RemoveTreeSources = "/RemoveTreeSources";
    }
}