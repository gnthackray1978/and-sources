using System;

namespace TDBCore.Types.DTOs
{
    public class ServicePerson : ServicePersonLookUp
    {

        public ServicePerson()
        {
            BirthLocationId = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699").ToString();
            DeathLocationId = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699").ToString();
            ReferenceLocationId = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699").ToString();
        }

        public string Baptism { get; set; }
        public string Birth { get; set; }
        public string Death { get; set; }
        public string BirthCounty { get; set; }
        public string DeathCounty { get; set; }
        public string BirthLocationId { get; set; }
        public string DeathLocationId { get; set; }
        public string ReferenceLocationId { get; set; }      
        public string ReferenceDate { get; set; }
 
        public string SourceDescription { get; set; }
        public string SpouseChristianName { get; set; }
        public string SpouseSurname { get; set; }
        public string FatherOccupation { get; set; }
        public string Occupation { get; set; }
        public string Notes { get; set; }
        public string IsMale { get; set; }
        

    }
}