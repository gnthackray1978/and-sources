using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.EntityModel;

namespace TDBCore.Types
{
    public class ServiceBase
    {
        public string ErrorStatus { get; set; }
    }



    //used with google maps
    public class ServiceMapDisplaySource
    {
        public Guid SourceId { get; set; }
        public string SourceRef { get; set; }
        public string SourceDesc { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string OriginalLocation { get; set; }
        public bool IsCopyHeld { get; set; }
        public bool IsViewed { get; set; }
        public bool IsThackrayFound { get; set; }
        //public List<int> sourceTypes;
        public int DisplayOrder { get; set; }
    }

    public class ServiceFullSource
    {


        public Guid SourceId { get; set; }
        public string SourceRef { get; set; }
        public string SourceDesc { get; set; }
        public string SourceNotes { get; set; }

       // public int SourceDate { get; set; }
       // public int SourceDateTo { get; set; }

        public int SourceFileCount { get; set; }

        public string SourceDateStr { get; set; }
        public string SourceDateStrTo { get; set; }

        public string OriginalLocation { get; set; }

        public bool IsCopyHeld { get; set; }
        public bool IsViewed { get; set; }
        public bool IsThackrayFound { get; set; }

        public string Parishs { get; set; }

        public string SourceTypes { get; set; }

        public string FileIds { get; set; }

    }

    public class ServiceSource
    {
        public Guid SourceId { get; set; }
        public Guid DefaultPerson { get; set; }
        public int SourceYear { get; set; }
        public int SourceYearTo { get; set; }
        public string SourceRef { get; set; }
        public string SourceDesc { get; set; }


    }

    public class CensusPlace
    {

        public Guid ParishId { get; set; }
        public string PlaceName { get; set; }
        public decimal LocX { get; set; }
        public decimal LocY { get; set; }

    }


    public class CensusSource
    {
        public Guid SourceId { get; set; }
        public List<CensusPerson> attachedPersons { get; set; }
        public int CensusYear { get; set; }
        public string CensusRef { get; set; }
        public string CensusDesc { get; set; }

        public string Address { get; set; }
        public string Civil_Parish { get; set; }
        public string County { get; set; }
        public string Municipal_Borough { get; set; }
        public string Registration_District { get; set; }
        public string Page { get; set; }
        public string Piece { get; set; }

   


    }

    public class CensusPerson
    {
        public int BirthYear { get; set; }
        public string BirthCounty { get; set; }
        public string CName { get; set; }
        public string SName { get; set; }

    }




    public class ServiceSourceObject
    {
        public List<ServiceSource> serviceSources { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }





    public class ServiceFileObject
    {
        public List<ServiceFile> serviceFiles { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }


    public class ServiceFile
    {
        public Guid FileId { get; set; }
        public string FileDescription { get; set; }
        public string FileLocation { get; set; }
        public string FileThumbLocation { get; set; }
    }



    public class ServicePersonLookUp : ServiceBase
    {
        public Guid PersonId { get; set; }
        public string ChristianName { get; set; }
        public string Surname { get; set; }
        public string FatherChristianName { get; set; }
        public string FatherSurname { get; set; }
        public string MotherChristianName { get; set; }
        public string MotherSurname { get; set; }
        public int DeathYear { get; set; }
        public int BirthYear { get; set; }
        public string BirthLocation { get; set; }
        public string DeathLocation { get; set; }
        public string XREF { get; set; }
        public string Sources { get; set; }
        public string Events { get; set; }
        public string Spouse { get; set; }
    }

    public class ServicePerson : ServicePersonLookUp
    {
        public string Baptism { get; set; }
        public string Birth { get; set; }
        public string Death { get; set; }
        public string BirthCounty { get; set; }
        public string DeathCounty { get; set; }
        public string BirthLocationId { get; set; }
        public string DeathLocationId { get; set; }
        public string ReferenceLocationId { get; set; }
        public string ReferenceLocation { get; set; }
        public string ReferenceDate { get; set; }
        public string SourceDescription { get; set; }
        public string SpouseChristianName { get; set; }
        public string SpouseSurname { get; set; }
        public string FatherOccupation { get; set; }
        public string Occupation { get; set; }
        public string Notes { get; set; }
        public string IsMale { get; set; }
       

    }




    public class ServiceMarriageLookup :ServiceBase
    {
        public Guid MarriageId { get; set; }
        public string MaleCName { get; set; }
        public string MaleSName { get; set; }
        public string FemaleCName { get; set; }
        public string FemaleSName { get; set; }
        public string MarriageDate { get; set; }
        public string MarriageLocation { get; set; }
        public string Witnesses { get; set; }
        public string XREF { get; set; }
        public string Sources { get; set; }
        public string Events { get; set; }

    }

    public class ServiceMarriage : ServiceMarriageLookup
    {
        public string SourceDescription { get; set; }

        public string Witness1ChristianName { get; set; }
        public string Witness1Surname { get; set; }

        public string Witness2ChristianName { get; set; }
        public string Witness2Surname { get; set; }

        public string Witness3ChristianName { get; set; }
        public string Witness3Surname { get; set; }

        public string Witness4ChristianName { get; set; }
        public string Witness4Surname { get; set; }

        public string LocationCounty { get; set; }
        public string LocationId { get; set; }

        public string MaleLocation { get; set; }
        public string MaleLocationId { get; set; }

        public string FemaleLocation { get; set; }
        public string FemaleLocationId { get; set; }

        public int MaleBirthYear { get; set; }
        public int FemaleBirthYear { get; set; }

        public string MaleOccupation { get; set; }
        public string FemaleOccupation { get; set; }

        public string MaleNotes { get; set; }
        public string FemaleNotes { get; set; }

        public bool IsWidow { get; set; }
        public bool IsWidower { get; set; }
        public bool IsBanns { get; set; }
        public bool IsLicense { get; set; }

    }

    public class ServiceMarriageObject : ServiceBase
    {
        public List<ServiceMarriageLookup> serviceMarriages { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }



    public class ServiceSourceType
    {
        public int TypeId { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }


    }

    public class ServiceSourceTypeObject
    {
        public List<ServiceSourceType> serviceSources { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }



    public class ServicePersonObject : ServiceBase
    {
        public List<ServicePersonLookUp> servicePersons { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }


    public class ServiceParish
    {
        public Guid ParishId { get; set; }
        public string ParishName { get; set; }
        public string ParishDeposited { get; set; }
        public string ParishParent { get; set; }
        public string ParishCounty { get; set; } 
        public int ParishStartYear { get; set; }
        public int ParishEndYear { get; set; }
        public double ParishLat { get; set; }
        public double ParishLong { get; set; }
        public string ParishNote { get; set; }

    }

    public class ServiceSuperParish
    {
        public Guid ParishId { get; set; }//public Guid ParishId;
        public string ParishName { get; set; }//public string Name;
        public string ParishDeposited { get; set; }//public string Deposited;
        public string ParishCounty { get; set; } //public string County;
        public double ParishX { get; set; }
        public double ParishY { get; set; }
        public int ParishLocationCount { get; set; }
        public int ParishLocationOrder { get; set; }
        public string ParishGroupRef { get; set; }
    }

    public class ServiceParishDataType
    {
        public int DataTypeId { get; set; }
        public string Description { get; set; }
    }


    public class ServiceParishObject
    {
        public List<ServiceParish> serviceParishs { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }


    public class ServiceParishDetailObject
    {
        public List<ServiceParishRecord> serviceParishRecords { get; set; }
        public List<ServiceParishTranscript> serviceParishTranscripts { get; set; }
        public List<ServiceMapDisplaySource> serviceServiceMapDisplaySource { get; set; }

        public int MarriageCount {get;set;}
        public int PersonCount { get; set; }


    }




    public class ServiceParishRecord
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int DataType { get; set; }
        public string ParishRecordType { get; set; }
        public Guid ParishId { get; set; }
    }


    public class ServiceParishTranscript
    {
        public Guid ParishId { get; set; }
        public string ParishTranscriptRecord { get; set; }
    }


    public class ServiceSearchResult
    {
        public Guid ParishId { get; set; }
        public bool IsBaptism { get; set; }
        public bool IsMarriage { get; set; }
        public bool IsBurial { get; set; }

    }

    public class ServiceParishCounter
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public Guid ParishId { get; set; }
        public string ParishName { get; set; }

        public int Counter { get; set; }

        public decimal PX {get; set; }
        public decimal PY { get; set; }
    }
    
    public class ServiceEvent
    {
        public int EventDate { get; set; }

        public string EventDescription { get; set; }
        public string EventChristianName { get; set; }
        public string EventSurname { get; set; }
        public string EventLocation { get; set; }
        public string EventText { get; set; }


        public Guid EventId { get; set; }
        public Guid LinkId { get; set; }
        public int LinkTypeId { get; set; }
    }


    public class ServiceEventObject
    {
        public List<ServiceEvent> serviceEvents { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }


}



//<asp:Label ID="lnkDate" runat="server" Text='<%# Eval("EventDate") %>'  />
                         
//<asp:LinkButton ID="lnkEventType" runat="server"  CommandArgument="Edit" CommandName="Edit"><%# GetEvent() %></asp:LinkButton>
                        
//<asp:Label ID="lblCName" runat="server" Text='<%# Eval("ChristianName") %>'  />
                          
//<asp:Label ID="lblSName" runat="server" Text='<%# Eval("Surname") %>' />
                         
//<asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location") %>'  />
                       
//<asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' />