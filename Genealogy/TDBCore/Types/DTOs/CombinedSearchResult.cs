//using System;
//using System.Collections.Generic;
//using System.Data.Entity.Core.Objects.DataClasses;
//using TDBCore.BLL;
//using TDBCore.EntityModel;
//using TDBCore.Types.enums;

//namespace TDBCore.Types.DTOs
//{
//    public class CombinedSearchResult
//    {
//        private string eventDate;

//        private int eventYear;

//        private SearchEventType eventType;

//        private string christianName;

//        private string surname;

//        private string description;

//        private Guid recordId;

//        private List<Guid> sourcesList;

//        private string sourcesDesc;

//        private EntityObject entityObj;

//        private string location;

//        private string linkDescription;

//        private Guid linkId;

//        private SearchEventLink searchEventLinkType;




//        public CombinedSearchResult()
//        {
//            eventDate = ""; ;
//            eventType = new SearchEventType ();
//            christianName = "";
//            surname = "";
//            description = "";
//            recordId = Guid.Empty;
//            sourcesList = new List<Guid> ();
//            sourcesDesc = "";


//        }



//        public void PopulateData()
//        {
 
//            MarriageWitnessesBll mWits = new MarriageWitnessesBll();
//            Person _person = null;
//            Marriage _marriage = null;
//            SourceBll _sources = new SourceBll();
     
//            sourcesList = new List<Guid>();
 
            
//            switch (eventType)
//            {
//                case SearchEventType.Births:
//                    _person = (Person)this.entityObj;
//                    searchEventLinkType = SearchEventLink.Person;
//                    christianName = _person.ChristianName;
//                    surname = _person.Surname;
//                    recordId = _person.Person_id;
//                    linkId = _person.Person_id;

//                    location = _person.BirthLocation;

//                    if (_person.BapInt > 0)
//                    {
//                        eventYear = _person.BapInt;
//                        eventDate = _person.BaptismDateStr;
//                    }
//                    else
//                    {
//                        eventYear = _person.BirthInt; 
//                        eventDate = _person.BirthDateStr;
//                    }

//                    description = _person.ToBirthString();

//                    break;
//                case SearchEventType.PersonWithSpouse:
//                case SearchEventType.Deaths:
             
//                    _person = (Person)this.entityObj;
//                    searchEventLinkType = SearchEventLink.Person;
//                    christianName = _person.ChristianName;
//                    surname = _person.Surname;
//                    recordId = _person.Person_id;
//                    linkId = _person.Person_id;
//                    sourcesDesc = _sources.GetPersonSources(recordId, sourcesList);
//                    location = _person.DeathLocation;
                    

//                    description = _person.ToDeathString();

//                    eventYear = _person.DeathInt;
//                    eventDate = _person.DeathDateStr;


                    

//                    break;
          

                  
//                case SearchEventType.Spouses:

//                    _person = (Person)this.entityObj;
//                    searchEventLinkType = SearchEventLink.Person;


//                    christianName = _person.SpouseName;
//                    surname = _person.SpouseSurname;
//                    description = _person.ToSpouseString();


//                    recordId = _person.Person_id;
//                    linkId = _person.Person_id;
//                    sourcesDesc = _sources.GetPersonSources(recordId, sourcesList);
//                    location = _person.DeathLocation;




//                    eventYear = _person.DeathInt;
//                    eventDate = _person.DeathDateStr;




//                    break;

//                case SearchEventType.References:
//                    _person = (Person)this.entityObj;
                   
//                    christianName = _person.ChristianName;
//                    surname = _person.Surname;
//                    recordId = _person.Person_id;
//                    sourcesDesc = _sources.GetPersonSources(recordId, sourcesList);
//                    eventDate = _person.ReferenceDateStr;
//                    eventYear = _person.ReferenceDateInt;
//                    location = _person.ReferenceLocation;


//                    description = _person.CheckForMarriage(out linkId);


//                    if (description == "")
//                    {
//                        description = _person.CheckForWills(out linkId);

//                        searchEventLinkType = SearchEventLink.Source;

//                    }
//                    else
//                    {
//                        searchEventLinkType = SearchEventLink.Marriage;

//                    }


//                    break;
//                case SearchEventType.Fatherings:
//                    _person = (Person)this.entityObj;
                   
//                    christianName = _person.FatherChristianName;
//                    surname = _person.FatherSurname;
//                    recordId = _person.Person_id;
//                    linkId = _person.Person_id;
//                    sourcesDesc = _sources.GetPersonSources(recordId, sourcesList);
//                    location = _person.BirthLocation;
//                    searchEventLinkType = SearchEventLink.Person;

//                    if (_person.BapInt > 0)
//                    {
//                        eventYear = _person.BapInt;
//                        eventDate = _person.BaptismDateStr;
//                    }
//                    else
//                    {
//                        eventYear = _person.BirthInt;
//                        eventDate = _person.BirthDateStr;
//                    }
//                    description = _person.ToFatherString();
                   
//                    break;

//                case SearchEventType.Motherings:
//                    _person = (Person)this.entityObj;
//                    searchEventLinkType = SearchEventLink.Person;
//                    christianName = _person.MotherChristianName;
//                    surname = _person.MotherSurname;
//                    recordId = _person.Person_id;
//                    linkId = _person.Person_id;
//                    sourcesDesc = _sources.GetPersonSources(recordId, sourcesList);

//                    location = _person.BirthLocation;
//                    if (_person.BapInt > 0)
//                    {
//                        eventYear = _person.BapInt;
//                        eventDate = _person.BaptismDateStr;
//                    }
//                    else
//                    {
//                        eventYear = _person.BirthInt;
//                        eventDate = _person.BirthDateStr;
//                    }
//                    description = _person.ToMotherString();
                   
//                    break;
//                case SearchEventType.MarriageBride:
//                    _marriage = (Marriage)this.entityObj;
//                    searchEventLinkType = SearchEventLink.Marriage;
//                    recordId = _marriage.Marriage_Id;
//                    linkId = _marriage.Marriage_Id;
//                    christianName = _marriage.FemaleCName;
//                    surname = _marriage.FemaleSName;
//                    eventDate = _marriage.Date;
//                    eventYear = _marriage.YearIntVal;
//                    location = _marriage.MarriageLocation;
//                    description = _marriage.ToBrideDescription();
//                    sourcesDesc = _sources.GetMarriageSources(recordId, sourcesList);
//                    break;
//                case SearchEventType.MarriageGroom:
//                    _marriage = (Marriage)this.entityObj;
//                    searchEventLinkType = SearchEventLink.Marriage;
//                    linkId = _marriage.Marriage_Id;
//                    recordId = _marriage.Marriage_Id;
//                    christianName = _marriage.MaleCName;
//                    surname = _marriage.MaleSName;
//                    eventDate = _marriage.Date;
//                    eventYear = _marriage.YearIntVal;
//                    location = _marriage.MarriageLocation;
//                    description = _marriage.ToGroomDescription();
//                    sourcesDesc = _sources.GetMarriageSources(recordId, sourcesList);
      

//                    break;


            

//            }


 
//        }

 
//        #region props


//        public Guid LinkId
//        {
//            get { return linkId; }
//            set { linkId = value; }
//        }


//        public SearchEventLink SearchEventLinkType
//        {
//            get { return searchEventLinkType; }
//            set { searchEventLinkType = value; }
//        }
        

//        public string LinkDescription
//        {
//            get { return linkDescription; }
//            set { linkDescription = value; }
//        }
        
//        public string Location
//        {
//            get { return location; }
//            set { location = value; }
//        }
        

//        public EntityObject EntityObj
//        {
//            get { return entityObj; }
//            set { entityObj = value; }
//        }

//        public string SourcesDesc
//        {
//            get { return sourcesDesc; }
//            set { sourcesDesc = value; }
//        }
        


//        public List<Guid> SourcesList
//        {
//            get { return sourcesList; }
//            set { sourcesList = value; }
//        }
        


//        public Guid RecordId
//        {
//            get { return recordId; }
//            set { recordId = value; }
//        }

//        public string Description
//        {
//            get { return description; }
//            set { description = value; }
//        }
        

//        public string Surname
//        {
//            get { return surname; }
//            set { surname = value; }
//        }
        

//        public string ChristianName
//        {
//            get { return christianName; }
//            set { christianName = value; }
//        }


//        public SearchEventType EventType
//        {
//            get { return eventType; }
//            set { eventType = value; }
//        }

//        /// <summary>
//        /// There is some problem with EF4 and enums and linq
//        /// So this is a hack until i find a proper soln.
//        /// </summary>
//        public int EventTypeInt
//        {
//            get 
//            { 
//                return (int)eventType; 
//            }
//            set 
//            {
//                eventType = (SearchEventType)value; 
//            }
//        }

//        public string EventDate
//        {
//            get { return eventDate; }
//            set { eventDate = value; }
//        }

//        public int EventYear
//        {
//            get { return eventYear; }
//            set { eventYear = value; }
//        }

//        #endregion

//        public static int GetLinkTypeId(SearchEventLink sel)
//        {
//            int retTypeId = 0;

//            switch (sel)
//            {
//                case SearchEventLink.Person:
//                    retTypeId = 1;
//                    break;
//                case SearchEventLink.Marriage:
//                    retTypeId = 2;
//                    break;
//                case SearchEventLink.Source:
//                    retTypeId = 4;
//                    break;
//                default:
//                    break;
//            }


//            return retTypeId;
//        }

//        public static string GetEventString(SearchEventType set)
//        {
//            string retDesc = "";
            
//            switch (set)
//            {
//                case SearchEventType.Births:
//                    retDesc = "Births";
//                    break;
//                case SearchEventType.Deaths:
//                    retDesc = "Deaths";
//                    break;
//                case SearchEventType.References:
//                    retDesc = "References";
//                    break;
//                case SearchEventType.Fatherings:
//                    retDesc = "Fatherings";
//                    break;
//                case SearchEventType.Motherings:
//                    retDesc = "Motherings";
//                    break;
//                case SearchEventType.MarriageBride:
//                    retDesc = "MarriageBride";
//                    break;
//                case SearchEventType.MarriageGroom:
//                    retDesc = "MarriageGroom";
//                    break;
//                case SearchEventType.MarriageWitnesses:
//                    retDesc = "MarriageWitnesses";
//                    break;
//                case SearchEventType.Spouses:
//                    retDesc = "Spouses";
//                    break;
//                case SearchEventType.PersonWithSpouse:
//                    retDesc = "PersonWithSpouse";
//                    break;
                
//            }

//            return retDesc;
//        }
//    }
//}