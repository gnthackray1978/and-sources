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
    
    public partial class ParishRecordSource
    {
        public ParishRecordSource()
        {
            this.ParishRecords = new HashSet<ParishRecord>();
        }
    
        public int RecordTypeId { get; set; }
        public string RecordTypeName { get; set; }
        public string RecordTypeDescription { get; set; }
    
        public virtual ICollection<ParishRecord> ParishRecords { get; set; }
    }
}
