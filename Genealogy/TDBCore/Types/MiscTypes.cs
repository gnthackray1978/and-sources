using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

#if! SILVERLIGHT
using GedItter.BLL;
using System.Data.Objects.DataClasses;
using TDBCore.EntityModel;
using System.Diagnostics;
using TDBCore.BLL;
#endif

namespace TDBCore.Types
{


    class MiscTypes
    {
    }


    #if! SILVERLIGHT
    public class SearchEvent
    {
        private string eventDate;

        private int eventYear;

        private SearchEventType eventType;

        private string christianName;

        private string surname;

        private string description;

        private Guid recordId;

        private List<Guid> sourcesList;

        private string sourcesDesc;

        private EntityObject entityObj;

        private string location;

        private string linkDescription;

        private Guid linkId;

        private SearchEventLink searchEventLinkType;




        public SearchEvent()
        {
            eventDate = ""; ;
            eventType = new SearchEventType ();
            christianName = "";
            surname = "";
            description = "";
            recordId = Guid.Empty;
            sourcesList = new List<Guid> ();
            sourcesDesc = "";


        }



        public void PopulateData()
        {
 
            MarriageWitnessesBLL mWits = new MarriageWitnessesBLL();
            Person _person = null;
            Marriage _marriage = null;
            SourceBLL _sources = new SourceBLL();
     
            sourcesList = new List<Guid>();
 
            
            switch (eventType)
            {
                case SearchEventType.Births:
                    _person = (Person)this.entityObj;
                    searchEventLinkType = SearchEventLink.Person;
                    christianName = _person.ChristianName;
                    surname = _person.Surname;
                    recordId = _person.Person_id;
                    linkId = _person.Person_id;

                    location = _person.BirthLocation;

                    if (_person.BapInt > 0)
                    {
                        eventYear = _person.BapInt;
                        eventDate = _person.BaptismDateStr;
                    }
                    else
                    {
                        eventYear = _person.BirthInt; 
                        eventDate = _person.BirthDateStr;
                    }

                    description = _person.ToBirthString();

                    break;
                case SearchEventType.PersonWithSpouse:
                case SearchEventType.Deaths:
             
                    _person = (Person)this.entityObj;
                    searchEventLinkType = SearchEventLink.Person;
                    christianName = _person.ChristianName;
                    surname = _person.Surname;
                    recordId = _person.Person_id;
                    linkId = _person.Person_id;
                    sourcesDesc = _sources.GetPersonSources(recordId, sourcesList);
                    location = _person.DeathLocation;
                    

                    description = _person.ToDeathString();

                    eventYear = _person.DeathInt;
                    eventDate = _person.DeathDateStr;


                    

                    break;
          

                  
                case SearchEventType.Spouses:

                    _person = (Person)this.entityObj;
                    searchEventLinkType = SearchEventLink.Person;


                    christianName = _person.SpouseName;
                    surname = _person.SpouseSurname;
                    description = _person.ToSpouseString();


                    recordId = _person.Person_id;
                    linkId = _person.Person_id;
                    sourcesDesc = _sources.GetPersonSources(recordId, sourcesList);
                    location = _person.DeathLocation;




                    eventYear = _person.DeathInt;
                    eventDate = _person.DeathDateStr;




                    break;

                case SearchEventType.References:
                    _person = (Person)this.entityObj;
                   
                    christianName = _person.ChristianName;
                    surname = _person.Surname;
                    recordId = _person.Person_id;
                    sourcesDesc = _sources.GetPersonSources(recordId, sourcesList);
                    eventDate = _person.ReferenceDateStr;
                    eventYear = _person.ReferenceDateInt;
                    location = _person.ReferenceLocation;


                    description = _person.CheckForMarriage(out linkId);


                    if (description == "")
                    {
                        description = _person.CheckForWills(out linkId);

                        searchEventLinkType = SearchEventLink.Source;

                    }
                    else
                    {
                        searchEventLinkType = SearchEventLink.Marriage;

                    }


                    break;
                case SearchEventType.Fatherings:
                    _person = (Person)this.entityObj;
                   
                    christianName = _person.FatherChristianName;
                    surname = _person.FatherSurname;
                    recordId = _person.Person_id;
                    linkId = _person.Person_id;
                    sourcesDesc = _sources.GetPersonSources(recordId, sourcesList);
                    location = _person.BirthLocation;
                    searchEventLinkType = SearchEventLink.Person;

                    if (_person.BapInt > 0)
                    {
                        eventYear = _person.BapInt;
                        eventDate = _person.BaptismDateStr;
                    }
                    else
                    {
                        eventYear = _person.BirthInt;
                        eventDate = _person.BirthDateStr;
                    }
                    description = _person.ToFatherString();
                   
                    break;

                case SearchEventType.Motherings:
                    _person = (Person)this.entityObj;
                    searchEventLinkType = SearchEventLink.Person;
                    christianName = _person.MotherChristianName;
                    surname = _person.MotherSurname;
                    recordId = _person.Person_id;
                    linkId = _person.Person_id;
                    sourcesDesc = _sources.GetPersonSources(recordId, sourcesList);

                    location = _person.BirthLocation;
                    if (_person.BapInt > 0)
                    {
                        eventYear = _person.BapInt;
                        eventDate = _person.BaptismDateStr;
                    }
                    else
                    {
                        eventYear = _person.BirthInt;
                        eventDate = _person.BirthDateStr;
                    }
                    description = _person.ToMotherString();
                   
                    break;
                case SearchEventType.MarriageBride:
                    _marriage = (Marriage)this.entityObj;
                    searchEventLinkType = SearchEventLink.Marriage;
                    recordId = _marriage.Marriage_Id;
                    linkId = _marriage.Marriage_Id;
                    christianName = _marriage.FemaleCName;
                    surname = _marriage.FemaleSName;
                    eventDate = _marriage.Date;
                    eventYear = _marriage.YearIntVal;
                    location = _marriage.MarriageLocation;
                    description = _marriage.ToBrideDescription();
                    sourcesDesc = _sources.GetMarriageSources(recordId, sourcesList);
                    break;
                case SearchEventType.MarriageGroom:
                    _marriage = (Marriage)this.entityObj;
                    searchEventLinkType = SearchEventLink.Marriage;
                    linkId = _marriage.Marriage_Id;
                     recordId = _marriage.Marriage_Id;
                    christianName = _marriage.MaleCName;
                    surname = _marriage.MaleSName;
                    eventDate = _marriage.Date;
                    eventYear = _marriage.YearIntVal;
                    location = _marriage.MarriageLocation;
                    description = _marriage.ToGroomDescription();
                    sourcesDesc = _sources.GetMarriageSources(recordId, sourcesList);
      

                    break;


            

            }


 
        }

 
        #region props


        public Guid LinkId
        {
            get { return linkId; }
            set { linkId = value; }
        }


        public SearchEventLink SearchEventLinkType
        {
            get { return searchEventLinkType; }
            set { searchEventLinkType = value; }
        }
        

        public string LinkDescription
        {
            get { return linkDescription; }
            set { linkDescription = value; }
        }
        
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        

        public EntityObject EntityObj
        {
            get { return entityObj; }
            set { entityObj = value; }
        }

        public string SourcesDesc
        {
            get { return sourcesDesc; }
            set { sourcesDesc = value; }
        }
        


        public List<Guid> SourcesList
        {
            get { return sourcesList; }
            set { sourcesList = value; }
        }
        


        public Guid RecordId
        {
            get { return recordId; }
            set { recordId = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        

        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        

        public string ChristianName
        {
            get { return christianName; }
            set { christianName = value; }
        }


        public SearchEventType EventType
        {
            get { return eventType; }
            set { eventType = value; }
        }

        /// <summary>
        /// There is some problem with EF4 and enums and linq
        /// So this is a hack until i find a proper soln.
        /// </summary>
        public int EventTypeInt
        {
            get 
            { 
                return (int)eventType; 
            }
            set 
            {
                eventType = (SearchEventType)value; 
            }
        }

        public string EventDate
        {
            get { return eventDate; }
            set { eventDate = value; }
        }

        public int EventYear
        {
            get { return eventYear; }
            set { eventYear = value; }
        }

        #endregion

        public static int GetLinkTypeId(SearchEventLink sel)
        {
            int retTypeId = 0;

            switch (sel)
            {
                case SearchEventLink.Person:
                    retTypeId = 1;
                    break;
                case SearchEventLink.Marriage:
                    retTypeId = 2;
                    break;
                case SearchEventLink.Source:
                    retTypeId = 4;
                    break;
                default:
                    break;
            }


            return retTypeId;
        }

        public static string GetEventString(SearchEventType set)
        {
            string retDesc = "";
            
            switch (set)
            {
                case SearchEventType.Births:
                    retDesc = "Births";
                    break;
                case SearchEventType.Deaths:
                    retDesc = "Deaths";
                    break;
                case SearchEventType.References:
                    retDesc = "References";
                    break;
                case SearchEventType.Fatherings:
                    retDesc = "Fatherings";
                    break;
                case SearchEventType.Motherings:
                    retDesc = "Motherings";
                    break;
                case SearchEventType.MarriageBride:
                    retDesc = "MarriageBride";
                    break;
                case SearchEventType.MarriageGroom:
                    retDesc = "MarriageGroom";
                    break;
                case SearchEventType.MarriageWitnesses:
                    retDesc = "MarriageWitnesses";
                    break;
                case SearchEventType.Spouses:
                    retDesc = "Spouses";
                    break;
                case SearchEventType.PersonWithSpouse:
                    retDesc = "PersonWithSpouse";
                    break;
                
            }

            return retDesc;
        }
    }


    public enum SearchEventLink
    { 
        Person =1,
        Marriage =2,
        Source = 4
    }

    public enum SearchEventType
    { 
        Births = 1,

        Deaths=2,

        References =4,

        Fatherings=8,

        Motherings=16,

        MarriageBride =32,

        MarriageGroom = 64,

        MarriageWitnesses = 128,

        Spouses = 256,

        PersonWithSpouse =512
        
    }



    public class EventType : IEquatable<EventType>
    {
        // could of used a enum to do this
        // but decided this would be simpler 
        // when it comes to implementing the UI
        // the performance loss isnt going to matter much 
        // in this context


        private bool includeBirths;

        private bool includeDeaths;

        private bool includeReferences;

        private bool includeFatherings;

        private bool includeMotherings;

        private bool includeMarriageGroom;

        private bool includeMarriageWitnesses;

        private bool includeMarriageBride;

        private bool includeSpouses;

        private bool includePersonsWithSpouses;


        public EventType()
        { 
        
        }

        public EventType(bool _includeBirths, bool _includeDeaths, bool _includeReferences, bool _includeFatherings,
            bool _includeMotherings, bool _includeMarriageBride, bool _includeMarriageWitnesses, bool _includeMarriageGroom, bool _includeSpouses, bool _includePersonWithSpouses)
        {
            this.includeBirths = _includeBirths;
            this.includeDeaths = _includeDeaths;
            this.includeFatherings = _includeFatherings;
            this.includeMarriageBride = _includeMarriageBride;
            this.includeMarriageGroom = _includeMarriageGroom;
            this.includeMarriageWitnesses = _includeMarriageWitnesses;
            this.includeMotherings = _includeMotherings;
            this.includeReferences = _includeReferences;

            this.includeSpouses = _includeSpouses;
            this.includePersonsWithSpouses = _includePersonWithSpouses;

        }



        public bool PersonsWithSpouses
        {
            get { return includePersonsWithSpouses; }
            set { includePersonsWithSpouses = value; }
        }

        public bool Spouses
        {
            get { return includeSpouses; }
            set { includeSpouses = value; }
        }
        

        public bool MarriageWitness
        {
            get { return includeMarriageWitnesses; }
            set { includeMarriageWitnesses = value; }
        }



        public bool MarriageBride
        {
            get { return includeMarriageBride; }
            set { includeMarriageBride = value; }
        }
        

        public bool MarriageGroom
        {
            get { return includeMarriageGroom; }
            set { includeMarriageGroom = value; }
        }
        
        public bool Mothers
        {
            get { return includeMotherings; }
            set { includeMotherings = value; }
        }
        
        public bool Fatherings
        {
            get { return includeFatherings; }
            set { includeFatherings = value; }
        }
        
        public bool References
        {
            get { return includeReferences; }
            set { includeReferences = value; }
        }

        public bool Deaths
        {
            get { return includeDeaths; }
            set { includeDeaths = value; }
        }

        public bool Births
        {
            get { return includeBirths; }
            set { includeBirths = value; }
        }




        public bool Equals(EventType other)
        {
            bool isEqual = false;


            if (this.includeBirths == other.includeBirths && this.includeDeaths == other.includeDeaths && this.includeReferences == other.includeReferences && this.includeFatherings == other.includeFatherings &&
                this.includeMotherings == other.includeMotherings && this.includeMarriageGroom == other.includeMarriageGroom 
                && this.includeMarriageWitnesses == other.includeMarriageWitnesses && this.includeSpouses == other.includeSpouses && 
                this.includePersonsWithSpouses == other.includePersonsWithSpouses)
            {
                isEqual = true;
            }

            return isEqual;
        }

        public static bool operator ==(EventType evs1, EventType evs2)
        {
            return evs1.Equals(evs2);
        }

        public static bool operator !=(EventType evs1, EventType evs2)
        {
            return (!evs1.Equals(evs2));
        }
    }


    #endif


    public class DateTools
    {
        public static bool TryParseYear(string param, out int year)
        {

            int retVal = 0;



            Regex regex = new Regex(@"\d\d\d\d");

            Match _match = regex.Match(param);



            if (_match.Success)
            {
                retVal = Convert.ToInt32(_match.Value);
                year = retVal;
                return true;
            }
            else
            {
                year = 0;
                return false;
            }



            
        }
    }

    public class YearRange
    {

        int startYear = 0;

        public int StartYear
        {
            get { return startYear; }
            set { startYear = value; }
        }

        int endYear = 0;

        public int EndYear
        {
            get { return endYear; }
            set { endYear = value; }
        }

        public YearRange(int _startYear, int _endYear)
        {
            startYear = _startYear;
            endYear = _endYear;
        }

        public bool ContainsYearRange(int startYearRngToTest, int endYearRngToTest)
        {

            if ((startYearRngToTest >= this.startYear && endYearRngToTest <= this.endYear))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

    }


    public class SimpleTimer
    {
        private long StartTime = 0;
       

        public void StartTimer()
        {
            StartTime = DateTime.Now.Ticks;
        }

        public string EndTimer(string note)
        {
         //   TimeSpan elapsedSpan = new TimeSpan(DateTime.Now.Ticks - StartTime);
            long retVal = (DateTime.Now.Ticks - StartTime) / 10000;

            return note + " " + retVal.ToString() + "ms";

         //   Debug.WriteLine( note + " " + elapsedSpan.Milliseconds.ToString());
        }
    }

}
