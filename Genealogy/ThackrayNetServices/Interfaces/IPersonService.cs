using System;
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

namespace PersonService
{
    [ServiceContract]
    public interface IPersonService
    {

        // persons
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriPersonMappings.GetPerson)]
        ServicePerson GetPerson(string id);


        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingsMisc.GetPersons, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriPersonMappings.GetPersons)]
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
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriPersonMappings.SetPersonRelationship, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string SetPersonRelation(string persons, string relationType);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriPersonMappings.DeletePerson, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string DeletePerson(string personId);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriPersonMappings.AddPerson, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
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
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriPersonMappings.SetPersonDuplicate, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string SetDuplicate(string persons);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriPersonMappings.MergeSources, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string MergeSources(string person);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriPersonMappings.RemoveLinks, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string RemoveLink(string person);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriPersonMappings.UpdateDateEstimates)]
        void UpdateDateEstimates();


        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriPersonMappings.AssignLocations)]
        void AssignLocations();


    }
}