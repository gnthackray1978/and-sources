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
using System.IO;

namespace ParishService
{
    [ServiceContract]
    public interface IParishService
    {
        //parishs


        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriParishMappings.GetParish)]
        ServiceParish GetParish(string parishId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriParishMappings.GetParishs)]
        ServiceParishObject GetParishs(string deposited, string name, string county, string page_number, string page_size, string sort_col);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriParishMappings.DeleteParishs, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string DeleteParishs(string parishIds);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriParishMappings.GetParishsFromLocations)]
        List<ServiceSuperParish> GetParishsFromLocations(string parishLocation);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriParishMappings.GetParishsTypes)]
        List<ServiceParishDataType> GetParishTypes();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriParishMappings.GetParishDetails)]
        ServiceParishDetailObject GetParishDetail(string parishId);

        // public const string GetParishCounters = "/ParishService/GetParishCounters?0={startYear}&1={endYear}";
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriParishMappings.GetParishCounters)]
        List<ServiceParishCounter> GetParishCounters(string startYear, string endYear);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriParishMappings.GetParishNames)]
        List<string> GetParishNames(string parishIds);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriParishMappings.AddParish, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string AddParish(string ParishId, string ParishStartYear, string ParishEndYear,
                                string ParishLat, string ParishLong,
                                string ParishName, string ParishParent,
                                string ParishNote, string ParishCounty, string ParishDeposited);






        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriParishMappings.GetSearchResults)]
        List<ServiceSearchResult> GetSearchResults(string parishIds, string startYear, string endYear);

    }
}