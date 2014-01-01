using System;

namespace TDBCore.Types.DTOs
{
    public class WitnessDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public Guid LocationId { get; set; }
    }
}