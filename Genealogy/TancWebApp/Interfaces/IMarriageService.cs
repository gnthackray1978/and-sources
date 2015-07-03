using System.ServiceModel;
using System.ServiceModel.Web;
using TancWebApp.URIMappings;
using TDBCore.Types.DTOs;

namespace TancWebApp.Interfaces
{
    [ServiceContract]
    public interface IMarriageService
    {

        //marriages


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingMarriage.AddMarriage, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
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
                    string MarriageWitnesses
         
            );

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingMarriage.GetMarriage)]
        ServiceMarriage GetMarriage(string id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingMarriage.GetMarriages)]
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
                                                string marriageWitness,
                                                string page_number,
                                                string page_size,
                                                string sort_col);





        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingMarriage.DeleteMarriages, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string DeleteMarriages(string marriageIds);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingMarriage.SetMarriageDuplicate, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string SetMarriageDuplicate(string marriages);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingMarriage.MergeMarriages, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string MergeMarriage(string marriage);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingMarriage.RemoveMarriageLinks, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string RemoveMarriageLink(string marriage);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingMarriage.ReorderMarriages, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string ReorderMarriages(string marriage);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = UriMappingMarriage.SwitchSpouses, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string SwitchSpouses(string marriage);

 
    }
}