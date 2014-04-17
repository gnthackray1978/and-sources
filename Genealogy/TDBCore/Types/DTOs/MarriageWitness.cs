using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using TDBCore.EntityModel;
using TDBCore.Types.libs;

namespace TDBCore.Types.DTOs
{
    public class MarriageWitness
    {
        public ServicePerson Person { get; set; }
        public string Description { get; set; }
     
        public static List<MarriageWitness> AddWitnesses(List<WitnessDto> witnessDtos)
        {
            var witnesses = new List<MarriageWitness>();

            foreach (WitnessDto witnessDto in witnessDtos)
            {
                var witPers3 = new ServicePerson();
                var nMarriageWitness = new MarriageWitness();

                witPers3.ReferenceYear = witnessDto.Year;
                witPers3.ReferenceDate = witnessDto.Date;
                witPers3.ReferenceLocation = witnessDto.Location;
                witPers3.ReferenceLocationId = witnessDto.LocationId.ToString();
                witPers3.ChristianName = witnessDto.Name;
                witPers3.Surname = witnessDto.Surname;
                nMarriageWitness.Description = witnessDto.Description;
                nMarriageWitness.Person = witPers3;
                witnesses.Add(nMarriageWitness);
            }

            return witnesses;

        }

        public static List<MarriageWitness> DeSerializeWitnesses(string witnessDtos,string marriageDate,string marriageLocation, Guid locationId)
        {
            var witnesses = new List<MarriageWitness>();

            var serializer = new JavaScriptSerializer();
            var marriages = serializer.DeserializeToMarriageWitnesses(witnessDtos,marriageDate.ParseToValidYear(), marriageDate,
                                                                      marriageLocation, locationId);

            foreach (WitnessDto witnessDto in marriages)
            {
                var witPers3 = new ServicePerson();
                var nMarriageWitness = new MarriageWitness();

                witPers3.ReferenceYear = witnessDto.Year;
                witPers3.ReferenceDate = witnessDto.Date;
                witPers3.ReferenceLocation = witnessDto.Location;
                witPers3.ReferenceLocationId = witnessDto.LocationId.ToString();
                witPers3.ChristianName = witnessDto.Name;
                witPers3.Surname = witnessDto.Surname;
                nMarriageWitness.Description = witnessDto.Description;
                nMarriageWitness.Person = witPers3;
                witnesses.Add(nMarriageWitness);
            }

            return witnesses;

        }

    }
}