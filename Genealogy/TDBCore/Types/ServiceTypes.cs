﻿using System;
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
    public class ServiceMapDisplaySource :ServiceBase
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

    public class ServiceFullSource : ServiceBase
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

    public class ServiceSource : ServiceBase
    {
        public Guid SourceId { get; set; }
        public Guid DefaultPerson { get; set; }
        public int SourceYear { get; set; }
        public int SourceYearTo { get; set; }
        public string SourceRef { get; set; }
        public string SourceDesc { get; set; }


    }

    public class CensusPlace : ServiceBase
    {

        public Guid ParishId { get; set; }
        public string PlaceName { get; set; }
        public decimal LocX { get; set; }
        public decimal LocY { get; set; }

    }

    public class CensusSource : ServiceBase
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

    public class CensusPerson : ServiceBase
    {
        public int BirthYear { get; set; }
        public string BirthCounty { get; set; }
        public string CName { get; set; }
        public string SName { get; set; }

    }

    public class ServiceSourceObject : ServiceBase
    {
        public List<ServiceSource> serviceSources { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }

    public class ServiceFileObject : ServiceBase
    {
        public List<ServiceFile> serviceFiles { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }

    public class ServiceFile : ServiceBase
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
        public string LinkedTrees { get; set; }
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
        public string Witness1Description { get; set; }

        public string Witness2ChristianName { get; set; }
        public string Witness2Surname { get; set; }
        public string Witness2Description { get; set; }

        public string Witness3ChristianName { get; set; }
        public string Witness3Surname { get; set; }
        public string Witness3Description { get; set; }

        public string Witness4ChristianName { get; set; }
        public string Witness4Surname { get; set; }
        public string Witness4Description { get; set; }

        public string Witness5ChristianName { get; set; }
        public string Witness5Surname { get; set; }
        public string Witness5Description { get; set; }

        public string Witness6ChristianName { get; set; }
        public string Witness6Surname { get; set; }
        public string Witness6Description { get; set; }

        public string Witness7ChristianName { get; set; }
        public string Witness7Surname { get; set; }
        public string Witness7Description { get; set; }

        public string Witness8ChristianName { get; set; }
        public string Witness8Surname { get; set; }
        public string Witness8Description { get; set; }


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

    public class ServiceSourceType : ServiceBase
    {
        public int TypeId { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }


    }

    public class ServiceSourceTypeObject : ServiceBase
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

    public class ServiceParish : ServiceBase
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

    public class ServiceSuperParish : ServiceBase
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

    public class ServiceParishDataType : ServiceBase
    {
        public int DataTypeId { get; set; }
        public string Description { get; set; }
    }

    public class ServiceParishObject : ServiceBase
    {
        public List<ServiceParish> serviceParishs { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }

    public class ServiceParishDetailObject : ServiceBase
    {
        public List<ServiceParishRecord> serviceParishRecords { get; set; }
        public List<ServiceParishTranscript> serviceParishTranscripts { get; set; }
        public List<ServiceMapDisplaySource> serviceServiceMapDisplaySource { get; set; }

        public int MarriageCount {get;set;}
        public int PersonCount { get; set; }


    }

    public class ServiceParishRecord : ServiceBase
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int DataType { get; set; }
        public string ParishRecordType { get; set; }
        public Guid ParishId { get; set; }
    }

    public class ServiceParishTranscript : ServiceBase
    {
        public Guid ParishId { get; set; }
        public string ParishTranscriptRecord { get; set; }
    }

    public class ServiceSearchResult : ServiceBase
    {
        public Guid ParishId { get; set; }
        public bool IsBaptism { get; set; }
        public bool IsMarriage { get; set; }
        public bool IsBurial { get; set; }

    }

    public class ServiceParishCounter : ServiceBase
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public Guid ParishId { get; set; }
        public string ParishName { get; set; }

        public int Counter { get; set; }

        public decimal PX {get; set; }
        public decimal PY { get; set; }
    }

    public class ServiceEvent : ServiceBase
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

    public class ServiceEventObject : ServiceBase
    {
        public List<ServiceEvent> serviceEvents { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }


}

 