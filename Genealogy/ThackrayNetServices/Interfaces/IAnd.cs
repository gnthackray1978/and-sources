using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;
using System.ServiceModel.Activation;
using TDBCore.Types;
using TDBCore.ModelObjects;
using ANDServices;


namespace ANDServices
{
    [ServiceContract]
    public interface IAnd
    {

        //diags




        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsDiag.GetJSONTreePersons)]
        List<ServicePersonLookUp> GetJSONTreePeople(string sourceId, string start, string end);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsDiag.GetJSONTreeSources)]
        ServiceSourceObject GetJSONTreeSources(string description, string page_number, string page_size);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsDiag.SetJSONTreeDefaultPerson, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string SetDefaultTreePerson(Guid sourceId, Guid personId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsDiag.SaveTree, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string SaveNewTree(string sourceId, string fileName, string sourceRef, string sourceDesc, string sourceYear, string sourceYearTo);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsDiag.GetAncTreeDiag)]
        AncestorModel GetAncTreeDiag(string treeId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsDiag.GetTreeDiag)]
        TreeModel GetTreeDiag(string treeId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsDiag.GetTreeDiagPerson)]
        TreeModel GetTreeDiagDefaultPerson(string treeId, string personId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsDiag.DeleteTree, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool DeleteTree(string treeId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.TestLogin)]
        string TestLogin(string testParam);

        //files

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetFilesForSource)]
        ServiceFileObject GetFilesForSource(string sourceId, string page_number, string page_size);

        //source types

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.DeleteSourceTypes, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool DeleteSourceTypes(string sourceIds);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.AddSourceType, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string AddSourceType(string TypeId, string Description, string Order);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetSourceType)]
        ServiceSourceType GetSourceType(string TypeId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetJSONSourceTypes)]
        ServiceSourceTypeObject GetSourceTypes(string description, string page_number, string page_size, string sort_col);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetSourceTypeNames)]
        List<string> GetSourceTypeNames(string typeIds);


        //sources
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.AddSource, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string AddSource(string sourceId,
                                   string isCopyHeld,
                                   string isThackrayFound,
                                   string isViewed,
                                   string originalLocation,
                                   string sourceDesc,
                                   string sourceRef,
                                   string sourceNotes,
                                   string sourceDateStr,
                                   string sourceDateStrTo,
                                   string sourceFileCount,
                                   string parishs,
                                   string sourceTypes,
                                   string fileIds);


        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetSource)]
        ServiceFullSource GetSource(string sourceId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetSourceNames)]
        List<string> GetSourceNames(string sourceIds);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetSources)]
        ServiceSourceObject GetSources(string sourceTypes,
            string sourceRef,
            string sourceDesc,
            string origLoc,
            string dateLB,
            string toDateLB,
            string dateUB,
            string toDateUB,
            string fileCount,
            string isThackrayFound,
            string isCopyHeld,
            string isViewed,
            string isChecked,
            string page_number, string page_size, string sortColumn);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.DeleteSource, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool DeleteSource(string sourceId);



        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.Get1841CensusPlaces)]
        List<CensusPlace> Get1841CensusPlaces();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.Get1841CensusSources)]
        List<CensusSource> Get1841CensusSources(Guid sourceId);


        // persons
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetPerson)]
        ServicePerson GetPerson(string id);


        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetPersons, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetPersons)]
        ServicePersonObject GetPersons(string _parentId,
                                                string christianName,
                                                string surname,
                                                string fatherChristianName,
                                                string fatherSurname,
                                                string motherChristianName,
                                                string motherSurname,
                                                string location,
                                                string county,
                                                string lowerDate,
                                                string upperDate,
                                                string filterTreeResults,
                                                string filterIncludeBirths,
                                                string filterIncludeDeaths,
                                                string filterSource,
                                                string spouse,
                                                string parishFilter,
                                                string page_number,
                                                string page_size,
                                                string sort_col);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetPersonsCount)]
        int GetPersonsCount(string _parentId,
                        string christianName,
                        string surname,
                        string fatherChristianName,
                        string fatherSurname,
                        string motherChristianName,
                        string motherSurname,
                        string location,
                        string county,
                        string lowerDate,
                        string upperDate,

                        string filterTreeResults,
                        string filterIncludeBirths,
                        string filterIncludeDeaths,
                        string filterSource);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.SetPersonRelationship, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string SetPersonRelation(string persons, string relationType);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.DeletePerson, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string DeletePerson(string personId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.AddPerson, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string AddPerson(
            string personId,
            string birthparishId,
            string deathparishId,
            string referenceparishId,
            string sources,
            string christianName,
            string surname,
            string fatherchristianname,
            string fathersurname,
            string motherchristianname,
            string mothersurname,
            string source,
            string ismale,
            string occupation,
            string datebirthstr,
            string datebapstr,
            string birthloc,
            string birthcounty,
            string datedeath,
            string deathloc,
            string deathcounty,
            string notes,
            string refdate,
            string refloc,
            string fatheroccupation,
            string spousesurname,
            string spousechristianname,
            string years,
            string months,
            string weeks,
            string days


            );

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.SetPersonDuplicate, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string SetDuplicate(string persons);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.MergeSources, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string MergeSources(string person);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.RemoveLinks, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string RemoveLink(string person);




        //marriages


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.AddMarriage, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string AddMarriage(
                    string FemaleLocationId,
                    string LocationId,
                    string MaleLocationId,
                    string SourceDescription,
                    string Sources,
                    string MarriageId,
                    string IsBanns,
                    string IsLicense,
                    string IsWidow,
                    string IsWidower,
                    string FemaleBirthYear,
                    string FemaleCName,
                    string FemaleLocation,
                    string FemaleNotes,
                    string FemaleOccupation,
                    string FemaleSName,
                    string LocationCounty,
                    string MaleBirthYear,
                    string MaleCName,
                    string MaleLocation,
                    string MaleNotes,
                    string MaleOccupation,
                    string MaleSName,
                    string MarriageDate,
                    string MarriageLocation,

                    string Witness1ChristianName,
                    string Witness1Surname,
                    string Witness2ChristianName,
                    string Witness2Surname,
                    string Witness3ChristianName,
                    string Witness3Surname,
                    string Witness4ChristianName,
                    string Witness4Surname
            );

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetMarriage)]
        ServiceMarriage GetMarriage(string id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetMarriages)]
        ServiceMarriageObject GetMarriages(string uniqref,
                                                string malecname,
                                                string malesname,
                                                string femalecname,
                                                string femalesname,
                                                string location,
                                                string lowerDate,
                                                string upperDate,
                                                string sourceFilter,
                                                string parishFilter,
                                                string page_number,
                                                string page_size,
                                                string sort_col);




        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetMarriagesCount)]
        int GetMarriagesCount(string uniqref,
                                                string malecname,
                                                string malesname,
                                                string femalecname,
                                                string femalesname,
                                                string location,
                                                string lowerDate,
                                                string upperDate,
                                                string sourceFilter);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.DeleteMarriages, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool DeleteMarriages(string marriageIds);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.SetMarriageDuplicate, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool SetMarriageDuplicate(string marriages);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.MergeMarriages, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool MergeMarriage(string marriage);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.RemoveMarriageLinks, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool RemoveMarriageLink(string marriage);


        //combined events 


        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetEvents)]
        ServiceEventObject GetEvents(string chkIncludeBirths, string chkIncludeDeaths, string chkIncludeWitnesses, string chkIncludeParents, string chkIncludeMarriages, string chkIncludeSpouses, string chkIncludePersonWithSpouses,
                                                string christianName, string surname, string lowerDateRange, string upperDateRange, string location,
                                                string page_number, string page_size, string sort_col);


        //parishs


        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetParish)]
        ServiceParish GetParish(string parishId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetParishs)]
        ServiceParishObject GetParishs(string deposited, string name, string county, string page_number, string page_size, string sort_col);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.DeleteParishs, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string DeleteParishs(string parishIds);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetParishsFromLocations)]
        List<ServiceSuperParish> GetParishsFromLocations(string parishLocation);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetParishsTypes)]
        List<ServiceParishDataType> GetParishTypes();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetParishDetails)]
        ServiceParishDetailObject GetParishDetail(string parishId);

        // public const string GetParishCounters = "/Parishs/GetParishCounters?0={startYear}&1={endYear}";
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetParishCounters)]
        List<ServiceParishCounter> GetParishCounters(string startYear, string endYear);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetParishNames)]
        List<string> GetParishNames(string parishIds);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.AddParish, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string AddParish(string ParishId, string ParishStartYear, string ParishEndYear,
                                string ParishLat, string ParishLong,
                                string ParishName, string ParishParent,
                                string ParishNote, string ParishCounty, string ParishDeposited);






        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetSearchResults)]
        List<ServiceSearchResult> GetSearchResults(string parishIds, string startYear, string endYear);




        // misc




        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.GetLoggedInUser)]
        string GetLoggedInUser();


        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.UpdateDateEstimates)]
        void UpdateDateEstimates();


        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = URIMappingsAND.AssignLocations)]
        void AssignLocations();



    }
}
