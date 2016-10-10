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

    public class ServicePersonAdd : ServicePersonLookUp
    {
        public string personGuid { get; set; }

        public string datebapstr { get; set; }
        public string datebirthstr { get; set; }
        public string datedeath { get; set; }
        public string years { get; set; }
        public string months { get; set; }
        public string weeks { get; set; }
        public string days { get; set; }

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