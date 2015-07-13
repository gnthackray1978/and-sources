

namespace GenOnline.Services.UriMappingConstants
{


    public static class UriPersonMappings
    {
        //person


        public const string Test = "/Test";
        public const string UpdateDateEstimates = "/UpdateDates";
        public const string AssignLocations = "/AssignLocats";

        public const string DeletePerson = "/Delete";
        public const string SetPersonRelationship = "/SetRelationship";
        public const string SetPersonDuplicate = "/SetDuplicate";
        public const string MergeSources = "/MergePersons";
        public const string RemoveLinks = "/RemoveLinks";
        public const string AddPerson = "/Add";


        public const string GetPerson = "/Person?0={id}";

        public const string GetPersons = "/Get/Select?0={_parentId}&1={christianName}&2={surname}&3={fatherChristianName}" +
                                            "&4={fatherSurname}&5={motherChristianName}&6={motherSurname}&7={location}&8={county}&9={lowerDate}&10={upperDate}" +
                                            "&11={filterTreeResults}&12={filterIncludeBirths}&13={filterIncludeDeaths}&14={filterIncludeSources}&15={filterSource}&16={spouse}" +
                                            "&17={parishFilter}&18={page_number}&19={page_size}&20={sort_col}";
 
    }
}