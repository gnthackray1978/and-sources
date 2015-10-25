using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types.DTOs
{
    public class BatchDto
    {
        public Guid Id { get; set; }

        public Guid BatchId { get; set; }

        public DateTime TimeRun { get; set; }

        public Guid? PersonId { get; set; }

        public Guid? MarriageId { get; set; }

        public Guid? SourceId { get; set; }

        public Guid? ParishId { get; set; }

        public string Ref { get; set; }

        public bool IsDeleted { get; set; }
    }
}
