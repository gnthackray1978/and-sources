using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using TancWebApp.URIMappings;
using TDBCore.Types.DTOs;

namespace TancWebApp.Interfaces
{
    [ServiceContract]
    public interface ISourceTypes
    {

        //source types

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceTypesMappings.DeleteSourceTypes, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string DeleteSourceTypes(string sourceIds);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceTypesMappings.AddSourceType, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string AddSourceType(string TypeId, string Description, string Order);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceTypesMappings.GetSourceType)]
        ServiceSourceType GetSourceType(string TypeId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceTypesMappings.GetJsonSourceTypes)]
        ServiceSourceTypeObject GetSourceTypes(string description, string page_number, string page_size, string sort_col);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriSourceTypesMappings.GetSourceTypeNames)]
        List<string> GetSourceTypeNames(string typeIds);
    }
}