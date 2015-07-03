namespace TancWebApp.URIMappings
{
    public static class UriSourceTypesMappings
    {
        //source types

        public const string GetJsonSourceTypes = "/Select?0={description}&1={page_number}&2={page_size}&3={sort_col}";

        public const string GetSourceType = "/Id?0={TypeId}";

        public const string DeleteSourceTypes = "/Delete";

        public const string AddSourceType = "/Add";

        public const string GetSourceTypeNames = "/GetNames?0={TypeIds}";
    }
}