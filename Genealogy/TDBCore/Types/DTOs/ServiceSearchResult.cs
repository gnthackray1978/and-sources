using System;

namespace TDBCore.Types.DTOs
{
    public class ServiceSearchResult : ServiceBase
    {
        public Guid ParishId { get; set; }
        public bool IsBaptism { get; set; }
        public bool IsMarriage { get; set; }
        public bool IsBurial { get; set; }

    }
}