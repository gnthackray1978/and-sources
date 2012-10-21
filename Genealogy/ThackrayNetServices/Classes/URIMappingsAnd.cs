using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANDServices
{
    public static class URIMappingsAND
    {

        //test
        public const string TestLogin = "/TestLogin?0={testParam}";
        public const string TestRest = "/TestRest?0={testParam}";


        //files

        public const string GetFilesForSource = "/Files/Select?0={sourceId}&1={page_number}&2={page_size}";

        //sources

        public const string Get1841CensusSources = "/Sources/Get1841CensusSources?0={sourceId}";

        public const string Get1841CensusPlaces = "/Sources/Get1841CensusPlaces";

        public const string GetSourceNames = "/Sources/GetSourceNames?0={sourceIds}";

        public const string DeleteSource = "/Source/Delete";

        public const string GetSources = "/GetSources/Select?0={sourceTypes}&1={sourceRef}&2={sourceDesc}&3={origLoc}" +
                                                    "&4={dateLB}&5={toDateLB}&6={dateUB}&7={toDateUB}&8={fileCount}&9={isThackrayFound}" +
                                                    "&10={isCopyHeld}&11={isViewed}&12={isChecked}&13={page_number}&14={page_size}&15={sortColumn}";

        public const string GetSource = "/Sources/GetSource?0={sourceId}";

        public const string AddSource = "/Sources/Add";

        //source types

        public const string GetJSONSourceTypes = "/SourceTypes/Select?0={description}&1={page_number}&2={page_size}&3={sort_col}";

        public const string GetSourceType = "/SourceTypes/Id?0={TypeId}";

        public const string DeleteSourceTypes = "/SourceTypes/Delete";

        public const string AddSourceType = "/SourceTypes/Add";

        public const string GetSourceTypeNames = "/SourceTypes/GetNames?0={TypeIds}";

        //person



        public const string UpdateDateEstimates = "/Person/UpdateDates";
        public const string AssignLocations = "/Person/AssignLocats";

        public const string DeletePerson = "/Person/Delete";
        public const string SetPersonRelationship = "/Person/SetRelationship";
        public const string SetPersonDuplicate = "/Person/SetDuplicate";
        public const string MergeSources = "/Person/MergePersons";
        public const string RemoveLinks = "/Person/RemoveLinks";
        public const string AddPerson = "/Person/Add";


        public const string GetPerson = "/GetPerson/Select?0={id}";

        public const string GetPersons = "/GetPersons/Select?0={_parentId}&1={christianName}&2={surname}&3={fatherChristianName}" +
                                            "&4={fatherSurname}&5={motherChristianName}&6={motherSurname}&7={location}&8={county}&9={lowerDate}&10={upperDate}" +
                                            "&11={filterTreeResults}&12={filterIncludeBirths}&13={filterIncludeDeaths}&14={filterSource}&15={spouse}" +
                                            "&16={parishFilter}&17={page_number}&18={page_size}&19={sort_col}";

        public const string GetPersonsCount = "/GetPersonsCount/Select?0={_parentId}&1={christianName}&2={surname}&3={fatherChristianName}" +
                                            "&4={fatherSurname}&5={motherChristianName}&6={motherSurname}&7={location}&8={county}&9={lowerDate}&10={upperDate}" +
                                            "&11={filterTreeResults}&12={filterIncludeBirths}&13={filterIncludeDeaths}&14={filterSource}";


        //marriages



        public const string AddMarriage = "/Marriages/Add";

        public const string GetMarriages = "/Marriages/GetMarriages/Select?0={uniqref}&1={malecname}&2={malesname}&3={femalecname}" +
                                                                 "&4={femalesname}&5={location}&6={lowerDate}&7={upperDate}&8={sourceFilter}" +
                                                                 "&9={parishFilter}&10={page_number}&11={page_size}&12={sort_col}";

        public const string GetMarriage = "/Marriages/GetMarriage/Select?0={id}";




        public const string GetMarriagesCount = "/Marriages/GetMarriagesCount/Select?0={uniqref}&1={malecname}&2={malesname}&3={femalecname}" +
                                                                "&4={femalesname}&5={location}&6={lowerDate}&7={upperDate}&8={sourceFilter}";

        public const string DeleteMarriages = "/Marriages/Delete";
        public const string SetMarriageDuplicate = "/Marriages/SetDuplicate";
        public const string MergeMarriages = "/Marriages/MergeMarriages";
        public const string RemoveMarriageLinks = "/Marriages/RemoveLinks";




        //parishs

        public const string GetParishs = "/Parishs/GetParishs/Select?0={deposited}&1={name}&2={county}&3={page_number}&4={page_size}&5={sort_col}";
        public const string DeleteParishs = "/Parishs/Delete";
        public const string GetParishsFromLocations = "/Parishs/GetParishsFromLocations?0={parishLocation}";

        public const string GetParish = "/Parishs/GetParish?0={parishId}";
        public const string GetParishDetails = "/Parishs/GetParishDetails?0={parishId}";

        public const string GetSearchResults = "/Parishs/GetSearchResults?0={parishIds}&1={startYear}&2={endYear}";

        public const string GetParishsTypes = "/Parishs/GetParishsTypes";

        public const string AddParish = "/Parishs/Add";

        public const string GetParishCounters = "/Parishs/GetParishCounters?0={startYear}&1={endYear}";

        public const string GetParishNames = "/Parishs/GetParishNames?0={parishIds}";

        //events


        public const string GetEvents = "/Events/GetEvents/Select?" +
             "0={chkIncludeBirths}&1={chkIncludeDeaths}&2={chkIncludeWitnesses}&3={chkIncludeParents}&4={chkIncludeMarriages}&5={chkIncludeSpouses}&6={chkIncludePersonWithSpouses}&" +
             "7={christianName}&8={surname}&9={lowerDateRange}&10={upperDateRange}&11={location}" +

            "&12={page_number}&13={page_size}&14={sort_col}";


        //misc

        public const string GetLoggedInUser = "/LoggedInUser";


        public const string AddFile = "/user/Set";





    }

}