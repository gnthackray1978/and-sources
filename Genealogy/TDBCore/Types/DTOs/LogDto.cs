using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types.DTOs
{
    public class LogDto
    {
        public int LogId { get; set; }
        public DateTime LogDate { get; set; }
        public int LogType { get; set; }

        public string LogData { get; set; }

        public string LogStackTrace { get; set; }

        public string LogSource { get; set; }

        public string LogException { get; set; }

    }
}
