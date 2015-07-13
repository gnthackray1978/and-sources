using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types.DTOs
{
    public class LogSearch
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int LogType { get; set; }
    }
}
