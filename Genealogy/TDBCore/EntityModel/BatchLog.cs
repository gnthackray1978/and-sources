//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TDBCore.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class BatchLog
    {
        public System.Guid Id { get; set; }
        public System.DateTime TimeRun { get; set; }
        public Nullable<System.Guid> PersonId { get; set; }
        public Nullable<System.Guid> MarriageId { get; set; }
        public Nullable<System.Guid> SourceId { get; set; }
        public Nullable<System.Guid> ParishId { get; set; }
        public System.Guid BatchId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Ref { get; set; }
        public string GoogleSheet { get; set; }
    }
}
