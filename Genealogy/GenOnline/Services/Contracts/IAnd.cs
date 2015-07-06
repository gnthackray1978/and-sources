using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using GenOnline.Services.UriMappingConstants;
using TDBCore.Types.DTOs;

namespace GenOnline.Services.Contracts
{
    [ServiceContract]
    public interface IAnd
    {
        //test
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsMisc.TestLogin)]
        string TestLogin(string testParam);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsMisc.UploadFile)]
        void Upload(string fileName, Stream stream);

        //diags
        
 


        //files

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsMisc.GetFilesForSource)]
        ServiceFileObject GetFilesForSource(string sourceId, string page_number, string page_size);





        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsMisc.GetEvents)]
        ServiceEventObject GetEvents(string chkIncludeBirths, string chkIncludeDeaths, string chkIncludeWitnesses, string chkIncludeParents, string chkIncludeMarriages, string chkIncludeSpouses, string chkIncludePersonWithSpouses,
                                                string christianName, string surname, string lowerDateRange, string upperDateRange, string location,
                                                string page_number, string page_size, string sort_col);







        // misc




        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsMisc.GetLoggedInUser)]
        string GetLoggedInUser();


      



    }
}
