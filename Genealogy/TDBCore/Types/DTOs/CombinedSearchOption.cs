using System;

namespace TDBCore.Types.DTOs
{
    public class CombinedSearchOption : IEquatable<CombinedSearchOption>
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


        public CombinedSearchOption()
        { 
        
        }

        public CombinedSearchOption(bool _includeBirths, bool _includeDeaths, bool _includeReferences, bool _includeFatherings,
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




        public bool Equals(CombinedSearchOption other)
        {
            var isEqual = this.includeBirths == other.includeBirths && this.includeDeaths == other.includeDeaths && this.includeReferences == other.includeReferences && this.includeFatherings == other.includeFatherings &&
                           this.includeMotherings == other.includeMotherings && this.includeMarriageGroom == other.includeMarriageGroom 
                           && this.includeMarriageWitnesses == other.includeMarriageWitnesses && this.includeSpouses == other.includeSpouses && 
                           this.includePersonsWithSpouses == other.includePersonsWithSpouses;


            return isEqual;
        }

        public static bool operator ==(CombinedSearchOption evs1, CombinedSearchOption evs2)
        {
            return evs1.Equals(evs2);
        }

        public static bool operator !=(CombinedSearchOption evs1, CombinedSearchOption evs2)
        {
            return (!evs1.Equals(evs2));
        }
    }
}
