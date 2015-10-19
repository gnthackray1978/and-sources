using System;

namespace TDBCore.Types.DTOs
{
    public class MarriageResult
    {
        public Guid MarriageId { get; set; }

        public Guid UniqueRef { get; set; }

        public Guid UniqueRefStr { get; set; }

        public int MarriageYear { get; set; }

        public string FemaleSName { get; set; }

        public string FemaleCName { get; set; }

        public string MaleSName { get; set; }

        public string MaleCName { get; set; }

        public string MarriageLocation { get; set; }

        public string MarriageSource { get; set; }

        public string Witnesses { get; set; }

        public string SourceTrees { get; set; }

        public int MarriageTotalEvents { get; set; }

        public string Notes { get; set; }

    }
}
