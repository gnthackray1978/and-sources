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

    }
}
