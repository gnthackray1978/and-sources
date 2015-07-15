using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TDBCore.EntityModel;
using TDBCore.Interfaces;
using TDBCore.Types.DTOs;

namespace TDBCore.BLL
{
    public class LogDal : BaseBll, ILogDal
    {
        public int AddLog(LogDto log)
        {            
            var mylog = new Log
            {
                LogData =  log.LogData,
                LogDate = log.LogDate,
                LogType = log.LogType,
                LogException = log.LogException,
                LogSource = log.LogSource,
                LogStackTrace = log.LogStackTrace
            };

            ModelContainer.Log.Add(mylog);

            ModelContainer.SaveChanges();

            return mylog.LogId;
        }

        public List<LogDto> GetLogsByParams(LogSearch searchOptions)
        {


            return
                ModelContainer.Log.Where(
                    o =>
                        o.LogDate > searchOptions.From && o.LogDate < searchOptions.To &&
                        o.LogType == searchOptions.LogType).Select(a => new LogDto
                        {
                            LogData = a.LogData,
                            LogId = a.LogId,
                            LogType = a.LogType,
                            LogDate = a.LogDate,
                            LogException = a.LogException,
                            LogSource = a.LogSource,
                            LogStackTrace = a.LogStackTrace
                        }).ToList();

        }
    }
}
