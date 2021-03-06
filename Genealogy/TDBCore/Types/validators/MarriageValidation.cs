﻿namespace TDBCore.Types.validators
{
    public class MarriageValidation
    {
        public bool IsValidMarriageDate { get; set; }
        public bool IsValidMaleName { get; set; }
        public bool IsValidMaleSurname { get; set; }
        public bool IsValidFemaleName { get; set; }
        public bool IsValidFemaleSurname { get; set; }
        public bool IsValidLocation { get; set; }
        public bool IsValidMaleLocation { get; set; }
        public bool IsValidFemaleLocation { get; set; }
        public bool IsValidMaleInfo { get; set; }
        public bool IsValidFemaleInfo { get; set; }
        public bool IsValidMarriageCounty { get; set; }
        public bool IsValidSource { get; set; }
        public bool IsValidWitnesses { get; set; }

        public bool IsValidMaleOccupation { get; set; }
        public bool IsValidFemaleOccupation { get; set; }
        public bool IsValidFemaleBirthYear { get; set; }
        public bool IsValidMaleBirthYear { get; set; }


        public bool IsValidOriginalName { get; set; }
        public bool IsValidOriginalFemaleName { get; set; }


        public bool IsValidEntry { get; set; }
    }
}
