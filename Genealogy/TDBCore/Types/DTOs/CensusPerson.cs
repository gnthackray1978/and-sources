namespace TDBCore.Types.DTOs
{
    public class CensusPerson : ServiceBase
    {
        public int BirthYear { get; set; }
        public string BirthCounty { get; set; }
        public string CName { get; set; }
        public string SName { get; set; }

    }
}