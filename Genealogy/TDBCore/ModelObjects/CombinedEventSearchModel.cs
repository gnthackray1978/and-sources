using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
////using TDBCore.Datasets;
using GedItter.BirthDeathRecords.BLL;
using System.Collections;
using GedItter.Interfaces;
//using GedItter.DLL;
using GedItter.MarriageRecords.BLL;
using TDBCore.Types;
using System.Diagnostics;
using System.Data.Objects;
using TDBCore.EntityModel;


namespace GedItter.ModelObjects
{
    public class CombinedEventSearchModel : EditorBaseModel<Guid>, ICombinedEventModel
    {
        //todo needs to be expanded to include references

        public static int excepIdx=0;

        private List<SearchEvent> searchEventlist = new List<SearchEvent>();

        private string cName = "will";
        private string sName = "thack";

        private string location = "";
        private string county = "";

        private int startYear = 0;
        private int endYear = 1850;

        //private bool includeMarriageEvents = true;
        //private bool includeParentingEvents = true;
        //private bool includeBirths = true;
        //private bool includeBurials = true;
        private bool isValidUpperDate = false;
        private bool isValidLowerDate = false;
        private bool isValidCName = false;
        private bool isValidSName = false;
        private bool isValidCounty = false;
        private bool isValidLocation = false;
        private bool isValidEventSelection = false;

        EventType filterEventSelection = new EventType();

        DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();
        MarriagesBLL marriagesBll = new MarriagesBLL();

        public CombinedEventSearchModel()
        {
            deathsBirthsBll = new DeathsBirthsBLL();
        }


        #region ICombinedEventModel Members


        #region props

        public bool IsValidCName
        {
            get
            {
                return this.isValidCName;
            }
        }

        public bool IsValidSName
        {
            get
            {
                return this.isValidSName;
            }
        }

        public bool IsValidLocation
        {
            get
            {
                return this.isValidLocation;
            }
        }

        public bool IsValidCounty
        {
            get
            {
                return this.isValidCounty;
            }
        }

        public bool IsValidEventSelection
        {
            get
            {
                return this.isValidEventSelection;
            }
        }

        public bool IsValidUpperDateBound
        {
            get 
            {
                return this.isValidUpperDate;
            }
        }

        public bool IsValidLowerDateBound
        {
            get 
            {
                return this.isValidLowerDate;
            }
        }



        public string FilterUpperDate
        {
            get 
            {
                return this.endYear.ToString();
            }
        }

        public string FilterLowerDate
        {
            get
            {
                return this.startYear.ToString();
            }
        }

        public string FilterCName
        {
            get 
            {
                return this.cName;
            }
        }

        public string FilterSName
        {
            get 
            {
                return this.sName;
            }
        }

        public string FilterLocationCounty
        {
            get
            {
                return this.county; 
            }
        }

        public string FilterLocation
        {
            get 
            {
                return this.location;
            }
        }

        #endregion




        public void SetFilterCName(string param)
        {
            if (this.cName != param)
            {
                this.cName = param;

                SetModelStatusFields();
            }
        }

        public void SetFilterSName(string param)
        {
            if (this.sName != param)
            {
                this.sName = param;

                SetModelStatusFields();
            }
        }

        public void SetFilterLocationCounty(string param)
        {
            if (this.county != param)
            {
                this.county = param;

                SetModelStatusFields();
            }
        }

        public void SetFilterLocation(string param)
        {
            if (this.location != param)
            {
                this.location = param;

                SetModelStatusFields();
            }
        }




        public void SetFilterUpperDate(string param)
        {
            this.isValidUpperDate = true;

            if (param != "")
            {
                if (!Int32.TryParse(param, out this.endYear))
                {
                    this.isValidUpperDate = false;
                    this.endYear = 0;
                }
            }
            else
            {
                this.isValidUpperDate = false;
                this.endYear = 0;
            }
        }

        public void SetFilterLowerDate(string param)
        {
            this.isValidLowerDate = true;

            if (param != "")
            {
                if (!Int32.TryParse(param, out this.startYear))
                {
                    this.isValidLowerDate = false;
                    this.startYear = 0;
                }
            }
            else
            {
                this.isValidLowerDate = false;
                this.startYear = 0;
            }
        }

        public void SetFilterEventSelection(TDBCore.Types.EventType param)
        {
            if (this.filterEventSelection != param)
            {
                this.filterEventSelection = param;

                SetModelStatusFields();
            }

        }





        #endregion
 

        public override void Refresh()
        {
 
            searchEventlist.Clear();



          //  Console.WriteLine("Starting: ");

            IEnumerable<SearchEvent> results = null;

            if (filterEventSelection.Births)
                results = deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.Births)).Select(GetSelect(SearchEventType.Births));


            if (filterEventSelection.PersonsWithSpouses)
            {
                if (results != null)
                    results = results.Union(deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.PersonWithSpouse)).Select(GetSelect(SearchEventType.PersonWithSpouse)));
                else
                    results = deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.PersonWithSpouse)).Select(GetSelect(SearchEventType.PersonWithSpouse));
            }

            if (filterEventSelection.Deaths)
            {
                if (results != null)
                    results = results.Union(deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.Deaths)).Select(GetSelect(SearchEventType.Deaths)));
                else
                    results = deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.Deaths)).Select(GetSelect(SearchEventType.Deaths));
            }


            if (filterEventSelection.Fatherings)
            {
                if (results != null)
                    results = results.Union(deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.Fatherings)).Select(GetSelect(SearchEventType.Fatherings)));
                else
                    results = deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.Fatherings)).Select(GetSelect(SearchEventType.Fatherings));
            }


            if (filterEventSelection.Mothers)
            {
                if (results != null)
                    results = results.Union(deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.Motherings)).Select(GetSelect(SearchEventType.Motherings)));
                else
                    results = deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.Motherings)).Select(GetSelect(SearchEventType.Motherings));
            }

            if (filterEventSelection.References)
            {
                if (results != null)
                    results = results.Union(deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.References)).Select(GetSelect(SearchEventType.References)));
                else
                    results = deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.References)).Select(GetSelect(SearchEventType.References));
            }


            if (filterEventSelection.Spouses)
            {
                if (results != null)
                    results = results.Union(deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.Spouses)).Select(GetSelect(SearchEventType.Spouses)));
                else
                    results = deathsBirthsBll.GetDeathsBirths2().Where(GetWhereP(SearchEventType.Spouses)).Select(GetSelect(SearchEventType.Spouses));
            }

            //// marriage brides


            if (filterEventSelection.MarriageBride)
            {
                if (results != null)
                    results = results.Union(marriagesBll.GetMarriages2().Where(GetWhereM(SearchEventType.MarriageBride)).Select(GetSelectM(SearchEventType.MarriageBride)));
                else
                    results = marriagesBll.GetMarriages2().Where(GetWhereM(SearchEventType.MarriageBride)).Select(GetSelectM(SearchEventType.MarriageBride));
            }

            //// marraige grooms
            if (filterEventSelection.MarriageGroom)
            {
                if (results != null)
                    results = results.Union(marriagesBll.GetMarriages2().Where(GetWhereM(SearchEventType.MarriageGroom)).Select(GetSelectM(SearchEventType.MarriageGroom)));
                else
                    results = marriagesBll.GetMarriages2().Where(GetWhereM(SearchEventType.MarriageGroom)).Select(GetSelectM(SearchEventType.MarriageGroom));
            }

            

            //SimpleTimer _st = new SimpleTimer();


            //_st.StartTimer();

            //Console.WriteLine("Records returned: " + results.Count());

            //Console.WriteLine(_st.EndTimer("1"));

            //_st.StartTimer();

            if (results != null)
            {


                foreach (SearchEvent sr in results)
                {

                    sr.PopulateData();
                    searchEventlist.Add(sr);
                }

            }

            searchEventlist = searchEventlist.OrderBy(o => o.EventYear).ToList();

           // Console.WriteLine(_st.EndTimer("2"));


            this.NotifyObservers<CombinedEventSearchModel>(this);
        }




        public Func<Marriage, SearchEvent> GetSelectM(SearchEventType _set)
        {
            Func<Marriage, SearchEvent> personPred3 = new Func<Marriage, SearchEvent>(cr => new SearchEvent() { EntityObj = cr, EventTypeInt = (int)_set });

            return personPred3;

        }

        public Func<Person, SearchEvent> GetSelect(SearchEventType _set)
        {
            Func<Person, SearchEvent> personPred3 = new Func<Person, SearchEvent>(cr => new SearchEvent() { EntityObj = cr, EventTypeInt = (int)_set });

            return personPred3;

        }

        public Func<Marriage, bool> GetWhereM(SearchEventType _set)
        {



            Func<Marriage, bool> marPred = new Func<Marriage, bool>(p => p.YearIntVal == 1800);

            switch (_set)
            {
 
                case SearchEventType.MarriageBride:
                    marPred = new Func<Marriage, bool>(m => (m.YearIntVal >= this.startYear && m.YearIntVal <= this.endYear) && m.MaleCName.StartsWith(this.FilterCName) && m.MaleSName.StartsWith(this.FilterSName)
                                                        && m.MarriageLocation.Contains(this.FilterLocation) && m.EventPriority == 0);
                    break;
                case SearchEventType.MarriageGroom:
                    marPred = new Func<Marriage, bool>(m => (m.YearIntVal >= this.startYear && m.YearIntVal <= this.endYear) && m.FemaleCName.StartsWith(this.FilterCName) && m.FemaleSName.StartsWith(this.FilterSName)
                                                        && m.MarriageLocation.Contains(this.FilterLocation) && m.EventPriority == 0);
                    break;
     
               
            }

            return marPred;
        }

        public Func<Person, bool> GetWhereP(SearchEventType _set)
        {



            Func<Person, bool> personPred = new Func<Person, bool>(p => p.BapInt == 1800);


            Func<Person, bool> test = new Func<Person, bool>(p => p.BapInt == 1800  );


            switch (_set)
            {
                case SearchEventType.Births:
                    personPred = new Func<Person, bool>(p =>
                        ((p.BapInt >= this.startYear && p.BapInt <= this.endYear) || (p.BirthInt >= this.startYear && p.BirthInt <= this.endYear))
                        && p.ChristianName.StartsWith(this.FilterCName, true, null) && p.Surname.StartsWith(this.FilterSName, true, null) && !p.IsDeleted 
                        && p.BirthLocation.Contains(this.FilterLocation) && p.EventPriority == 1);
                    break;
                case SearchEventType.Deaths:
                    personPred = new Func<Person, bool>(
                        p =>(p.DeathInt >= this.startYear && p.DeathInt <= this.endYear)  &&
                            p.ChristianName.StartsWith(this.FilterCName, true, null) && p.Surname.StartsWith(this.FilterSName, true, null) && !p.IsDeleted
                            && p.DeathLocation.Contains(this.FilterLocation) && p.EventPriority == 1);
                    break;

                case SearchEventType.Fatherings:
                    personPred = new Func<Person, bool>(p =>
                      ((p.BapInt >= this.startYear && p.BapInt <= this.endYear) || (p.BirthInt >= this.startYear && p.BirthInt <= this.endYear))
                       && p.FatherChristianName.StartsWith(this.FilterCName, true, null) && p.FatherSurname.StartsWith(this.FilterSName, true, null) && !p.IsDeleted
                       && p.BirthLocation.Contains(this.FilterLocation) && p.EventPriority == 1);
                    break;
                case SearchEventType.Motherings:

                    personPred = new Func<Person, bool>(p =>
                      ((p.BapInt >= this.startYear && p.BapInt <= this.endYear) || (p.BirthInt >= this.startYear && p.BirthInt <= this.endYear))
                       && p.MotherChristianName.StartsWith(this.FilterCName, true, null) && p.MotherSurname.StartsWith(this.FilterSName, true, null) && !p.IsDeleted
                       && p.BirthLocation.Contains(this.FilterLocation) && p.EventPriority == 1);

                    break;
                case SearchEventType.References:
                    personPred = new Func<Person, bool>(p =>
                      (p.ReferenceDateInt >= this.startYear && p.ReferenceDateInt <= this.endYear)
                      && p.ChristianName.StartsWith(this.FilterCName, true, null) && p.Surname.StartsWith(this.FilterSName, true, null) && !p.IsDeleted
                      && p.ReferenceLocation.Contains(this.FilterLocation) );//&& p.EventPriority == 1
                    break;

                case SearchEventType.Spouses:
                    // ok this is going to be mad!
                    personPred = new Func<Person, bool>(p =>
                        (((p.BapInt >= this.startYear && p.BapInt <= this.endYear) || (p.BirthInt >= this.startYear && p.BirthInt <= this.endYear)) ||
                        (p.DeathInt >= this.startYear && p.DeathInt <= this.endYear))
                        && p.SpouseName.StartsWith(this.FilterCName, true, null) && (p.Surname.StartsWith(this.FilterSName, true, null) || p.SpouseSurname.StartsWith(this.FilterSName, true, null)) 

                        && !p.IsDeleted
                        && (p.BirthLocation.Contains(this.FilterLocation) || p.DeathLocation.Contains(this.FilterLocation)) );// && p.EventPriority == 1
                    break;
                case SearchEventType.PersonWithSpouse:
                    personPred = new Func<Person, bool>(
                        p => (p.DeathInt >= this.startYear && p.DeathInt <= this.endYear) &&
                            p.ChristianName.StartsWith(this.FilterCName, true, null) && p.Surname.StartsWith(this.FilterSName, true, null) && !p.IsDeleted && p.SpouseName != ""
                            && p.DeathLocation.Contains(this.FilterLocation) && p.EventPriority == 1);
                    break;
             
            }



            return personPred;

        }



        public void SetEditorUI()
        {
            throw new NotImplementedException();
        }

        


        public TDBCore.Types.EventType FilterEventSelection
        {
            get 
            {
                return this.filterEventSelection;
            }
        }







        public IList<SearchEvent> SearchEvents
        {
            get 
            {
                return this.searchEventlist;
            }
        }
    }



}
