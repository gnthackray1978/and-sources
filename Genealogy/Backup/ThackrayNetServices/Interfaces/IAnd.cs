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


namespace ANDServices
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




        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsDiag.GetJsonTreePersons)]
        List<ServicePersonLookUp> GetJSONTreePeople(string sourceId, string start, string end);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsDiag.GetJsonTreeSources)]
        ServiceSourceObject GetJSONTreeSources(string description, string pageNumber, string pageSize);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsDiag.SetJsonTreeDefaultPerson, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string SetDefaultTreePerson(Guid sourceId, Guid personId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsDiag.SaveTree, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string SaveNewTree(string sourceId, string fileName, string sourceRef, string sourceDesc, string sourceYear, string sourceYearTo);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsDiag.GetAncTreeDiag)]
        AncestorModel GetAncTreeDiag(string treeId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsDiag.GetTreeDiag)]
        TreeModel GetTreeDiag(string treeId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsDiag.GetTreeDiagPerson)]
        TreeModel GetTreeDiagDefaultPerson(string treeId, string personId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsDiag.DeleteTree, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool DeleteTree(string treeId);



        //files

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsMisc.GetFilesForSource)]
        ServiceFileObject GetFilesForSource(string sourceId, string page_number, string page_size);





        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsEvents.GetEvents)]
        ServiceEventObject GetEvents(string chkIncludeBirths, string chkIncludeDeaths, string chkIncludeWitnesses, string chkIncludeParents, string chkIncludeMarriages, string chkIncludeSpouses, string chkIncludePersonWithSpouses,
                                                string christianName, string surname, string lowerDateRange, string upperDateRange, string location,
                                                string page_number, string page_size, string sort_col);







        // misc




        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsMisc.GetLoggedInUser)]
        string GetLoggedInUser();


      



    }
}
