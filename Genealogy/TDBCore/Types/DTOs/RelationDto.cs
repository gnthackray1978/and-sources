using System;

namespace TDBCore.Types.DTOs
{
    public class RelationDto
    {
        public Guid PersonA { get; set; }
        public Guid PersonB { get; set; }
        public int RelationType { get; set; }
    }
}
