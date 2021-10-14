using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IEventLogService
    {
        Task<List<EventLogDto>> GetEventLog();
        Task<List<EventLogDto>> FindByID(DateTime from, DateTime to, string Event);
    }
}
