using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using TDBCore.EntityModel;
using TDBCore.Types.libs;

namespace TDBCore.Types.DTOs
{
    public class MarriageWitness
    {
        public Person Person { get; set; }
        public string Description { get; set; }
     
        public static List<MarriageWitness> AddWitnesses(List<WitnessDto> witnessDtos)
        {
            var witnesses = new List<MarriageWitness>();

            foreach (WitnessDto witnessDto in witnessDtos)
            {
                Person witPers3 = new Person();
                MarriageWitness nMarriageWitness = new MarriageWitness();

                witPers3.ReferenceDateInt = witnessDto.Year;
                witPers3.ReferenceDateStr = witnessDto.Date;
                witPers3.ReferenceLocation = witnessDto.Location;
                witPers3.ReferenceLocationId = witnessDto.LocationId;
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
            var marriages = serializer.DeserializeToMarriageWitnesses(witnessDtos, CsUtils.GetDateYear(marriageDate), marriageDate,
                                                                      marriageLocation, locationId);

            foreach (WitnessDto witnessDto in marriages)
            {
                Person witPers3 = new Person();
                MarriageWitness nMarriageWitness = new MarriageWitness();

                witPers3.ReferenceDateInt = witnessDto.Year;
                witPers3.ReferenceDateStr = witnessDto.Date;
                witPers3.ReferenceLocation = witnessDto.Location;
                witPers3.ReferenceLocationId = witnessDto.LocationId;
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