using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.Types.DTOs;

namespace TDBCore.Interfaces
{
    public interface ILogDal
    {
        int AddLog(LogDto log);
        List<LogDto> GetLogsByDate(LogSearch searchOptions);
    }
}
