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
    
    public partial class Log
    {
        public int LogId { get; set; }
        public System.DateTime LogDate { get; set; }
        public int LogType { get; set; }
        public string LogData { get; set; }
        public string LogException { get; set; }
        public string LogStackTrace { get; set; }
        public string LogSource { get; set; }
    }
}
