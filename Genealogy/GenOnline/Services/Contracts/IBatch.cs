using GenOnline.Services.UriMappingConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TancWebApp.Services.UriMappingConstants;
using TDBCore.Types.DTOs;

namespace TancWebApp.Services.Contracts
{
    [ServiceContract]
    public interface IBatch
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = BatchMappings.AddPersons, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string InsertPersons(string sheetUrl);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = BatchMappings.AddMarriages, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string InsertMarriages(string sheetUrl);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = BatchMappings.AddSources, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string InsertSources(string sheetUrl);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = BatchMappings.AddParishs, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string InsertParishs(string sheetUrl);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = BatchMappings.RemoveBatch, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string RemoveBatch(string batchId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = BatchMappings.GetBatches)]
        ServiceBatchObject GetBatch(string batch_ref, string page_number, string page_size, string sort_col);
         
    }
}
