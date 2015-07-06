

namespace GenOnline.Services.UriMappingConstants
{

    public static class UriMappingMarriage
    {
        //marriages
        public const string AddMarriage = "/Add";
        public const string GetMarriages = "/Get/Select?0={uniqref}&1={malecname}&2={malesname}&3={femalecname}" +
                                           "&4={femalesname}&5={location}&6={lowerDate}&7={upperDate}&8={sourceFilter}" +
                                           "&9={parishFilter}&10={MarriageWitness}&11={page_number}&12={page_size}&13={sort_col}";

        public const string GetMarriage = "/Marriage?0={id}";
        public const string DeleteMarriages = "/Delete";
        public const string SetMarriageDuplicate = "/SetDuplicate";
        public const string MergeMarriages = "/MergeMarriages";
        public const string RemoveMarriageLinks = "/RemoveLinks";
        public const string ReorderMarriages = "/ReorderMarriages";
        public const string SwitchSpouses = "/SwitchSpouses";

    }
}