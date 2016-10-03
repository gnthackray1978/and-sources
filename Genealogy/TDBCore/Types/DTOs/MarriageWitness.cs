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

        public static List<MarriageWitness> DeSerializeWitnesses(string witnessDtos, ServiceMarriage marriage)
        {
            var witnesses = new List<MarriageWitness>();

            var serializer = new JavaScriptSerializer();
            var marriages = serializer.DeserializeToMarriageWitnesses(witnessDtos, marriage.MarriageDate.ParseToValidYear(), marriage.MarriageDate,
                                                                      marriage.MarriageLocation, marriage.LocationId.ToGuid());

            foreach (WitnessDto witnessDto in marriages)
            {
                var person = new ServicePerson();
                var nMarriageWitness = new MarriageWitness();

                person.ReferenceYear = witnessDto.Year;
                person.ReferenceDate = witnessDto.Date;
                person.ReferenceLocation = witnessDto.Location;
                person.ReferenceLocationId = witnessDto.LocationId.ToString();
                person.ChristianName = witnessDto.Name;
                person.Surname = witnessDto.Surname;
                person.OthersideChristianName = "";
                person.OthersideSurname = "";
                person.OthersideRelationship = "";
                person.Notes = "Witness to marriage of " + marriage.MaleSName + " and " + marriage.FemaleSName + " " + marriage.MarriageDate + " at " + marriage.MarriageLocation;


                nMarriageWitness.Description = witnessDto.Description;
                nMarriageWitness.Person = person;
                witnesses.Add(nMarriageWitness);
            }

            return witnesses;

        }


        public static List<MarriageWitness> FormatWitnessCollection(List<ServiceWitness> serviceWitnesses , ServiceMarriage marriage)
        {
            var witnesses = new List<MarriageWitness>();

            var serializer = new JavaScriptSerializer();
            //var marriages = serializer.DeserializeToMarriageWitnesses(witnessDtos, marriage.MarriageDate.ParseToValidYear(), marriage.MarriageDate,
             //                                                         marriage.MarriageLocation, marriage.LocationId.ToGuid());

            foreach (var witnessDto in serviceWitnesses)
            {
                var person = new ServicePerson();
                var nMarriageWitness = new MarriageWitness();

                person.ReferenceYear = marriage.MarriageDate.ParseToValidYear();
                person.ReferenceDate = marriage.MarriageDate;
                person.ReferenceLocation = marriage.MarriageLocation;
                person.ReferenceLocationId = marriage.LocationId;
                person.ChristianName = witnessDto.Name;
                person.Surname = witnessDto.Surname;
                person.OthersideChristianName = "";
                person.OthersideSurname = "";
                person.OthersideRelationship = "";
                person.Notes = "Witness to marriage of " + marriage.MaleSName + " and " + marriage.FemaleSName + " " + marriage.MarriageDate + " at " + marriage.MarriageLocation;


                nMarriageWitness.Description = witnessDto.Description;
                nMarriageWitness.Person = person;
                witnesses.Add(nMarriageWitness);
            }

            return witnesses;

        }

    }
}