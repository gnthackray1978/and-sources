using System;

namespace TDBCore.Types.DTOs
{
    public class CensusPlace : ServiceBase
    {

        public Guid ParishId { get; set; }
        public string PlaceName { get; set; }
        public decimal LocX { get; set; }
        public decimal LocY { get; set; }

    }
}