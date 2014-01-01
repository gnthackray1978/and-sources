

namespace ANDServices
{
    public static class UriParishMappings
    {
        //parishs

        public const string GetParishs = "/GetParishs/Select?0={deposited}&1={name}&2={county}&3={page_number}&4={page_size}&5={sort_col}";
        public const string DeleteParishs = "/Delete";
        public const string GetParishsFromLocations = "/GetParishsFromLocations?0={parishLocation}";

        public const string GetParish = "/GetParish?0={parishId}";
        public const string GetParishDetails = "/GetParishDetails?0={parishId}";

        public const string GetSearchResults = "/GetSearchResults?0={parishIds}&1={startYear}&2={endYear}";

        public const string GetParishsTypes = "/GetParishsTypes";

        public const string AddParish = "/Add";

        public const string GetParishCounters = "/GetParishCounters?0={startYear}&1={endYear}";

        public const string GetParishNames = "/GetParishNames?0={parishIds}";
    }
}