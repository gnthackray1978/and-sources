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
    
    public partial class SourceMappingParish
    {
        public int SourceMappingParishsRowId { get; set; }
        public Nullable<System.DateTime> SourceMappingDateAdded { get; set; }
        public Nullable<int> SourceMappingUser { get; set; }
    
        public virtual Parish Parish { get; set; }
        public virtual Source Source { get; set; }
    }
}
