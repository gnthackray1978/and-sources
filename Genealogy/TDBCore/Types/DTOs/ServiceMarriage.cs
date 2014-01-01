using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    public class ServiceMarriage : ServiceMarriageLookup
    {
 
        public string Priority { get; set; }

       // public string UserId { get; set; }


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
}