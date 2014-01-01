using System;

namespace TDBCore.Types.DTOs
{
    public class ServiceParishTranscript : ServiceBase
    {
        public Guid ParishId { get; set; }
        public string ParishTranscriptRecord { get; set; }
    }
}