using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using ANDServices;
using TDBCore.Types;
using TDBCore.Types.DTOs;

namespace AndServices.Interfaces
{
    [ServiceContract]
    public interface ISourceService
    {


        //sources
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceMappings.AddSource, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
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
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceMappings.GetSource)]
        SourceAjaxDto GetSource(string sourceId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceMappings.GetSourceNames)]
        List<string> GetSourceNames(string sourceIds);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceMappings.GetSources)]
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
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceMappings.DeleteSource, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string DeleteSource(string sourceId);
      
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceMappings.Get1841CensusSources)]
        List<CensusSource> Get1841CensusSources(Guid sourceId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceMappings.AddMarriageTreeSource, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string AddMarriageSources(string record, string sources);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceMappings.AddPersonTreeSource, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string AddPersonSources(string record, string sources);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceMappings.RemoveTreeSources, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string RemoveTreeSources(string record);
    }
}