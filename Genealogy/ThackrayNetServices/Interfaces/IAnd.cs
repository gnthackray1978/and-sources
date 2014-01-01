﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;
using System.ServiceModel.Activation;
using TDBCore.Types;
using ANDServices;
using System.IO;
using TDBCore.Types.DTOs;


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
