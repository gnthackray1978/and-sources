using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.Interfaces;
using TDBCore.Types.DTOs;
using TDBCore.Types.security;

namespace TDBCore.Types.domain
{
    public class LogSearch
    {
        private readonly ILogDal _logDal;
        readonly ISecurity _security = new NoSecurity();
        public LogSearch(ISecurity security, ILogDal iLogDal)
        {
            _security = security;
            _logDal = iLogDal;
        }

        public void WriteLog(string message, int logType, string source, Exception e)
        {                         
            _logDal.AddLog(new LogDto()
            {
                LogData = message,
                LogDate = DateTime.Now,
                LogType = logType,
                LogSource = source,
                LogException = e!=null ? e.Message : "",
                LogStackTrace = e != null ? e.StackTrace : ""
            });
        }


    }
}
