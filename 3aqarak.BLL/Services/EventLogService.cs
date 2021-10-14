using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace _3aqarak.BLL.Services
{
    public class EventLogService : IEventLogService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpServerUtilityBase _server;
        public EventLogService(IUnitOfWork uow, IConfirmation conf, HttpServerUtilityBase server)
        {
            _uow = uow;
            _conf = conf;
            _server = server;
        }
        public async Task<List<EventLogDto>> FindByID(DateTime from, DateTime to, string Event)
        {
            var EventLogData = new List<EventLogDto>();
            if (Event == "Insert")
            {

                EventLogData =(await _uow.EventLogsRepo.FindAsync(h => h.EventType == Event  && (h.Date >= from && h.Date <= to ))).Select(h => new EventLogDto()
                {
                    PK_Event_Id = h.PK_Event_Id,
                    UserName = h.UserName,
                    DateString = h.Date.ToString("dd/MM/yyyy hh:mm tt"),
                    EventType = h.EventType,
                    TableName = h.TableName,
                    Description = h.Description
                }).ToList();

            }
            else if (Event == "Delete")
            {
                EventLogData =(await _uow.EventLogsRepo.FindAsync(h => h.EventType == Event  && (h.Date >= from && h.Date <= to))).Select(h => new EventLogDto()
                {
                    PK_Event_Id = h.PK_Event_Id,
                    UserName = h.UserName,
                    DateString = h.Date.ToString("dd/MM/yyyy hh:mm tt"),
                    EventType = h.EventType,
                    TableName = h.TableName,
                    Description = h.Description
                }).ToList();
            }
            else if (Event == "Update")
            {
                EventLogData = (await _uow.EventLogsRepo.FindAsync(h => h.EventType == Event && (h.Date >= from.Date && h.Date <= to))).Select(h => new EventLogDto()
                {
                    PK_Event_Id = h.PK_Event_Id,
                    UserName = h.UserName,
                    DateString = h.Date.ToString("dd/MM/yyyy hh:mm tt"),
                    EventType = h.EventType,
                    TableName=h.TableName,
                    Description = h.Description
                }).ToList();
            }
            else
            {
                EventLogData =(await _uow.EventLogsRepo.FindAsync(h => (h.Date >= from.Date && h.Date <= to))).Select(h => new EventLogDto()
                {
                    PK_Event_Id = h.PK_Event_Id,
                    UserName = h.UserName,
                    DateString = h.Date.ToString("dd/MM/yyyy hh:mm tt"),
                    EventType = h.EventType,
                    TableName = h.TableName,
                    Description = h.Description
                }).ToList();
            }

            return EventLogData;
        }

        public async Task<List<EventLogDto>> GetEventLog()
        {

            var EventLog = await _uow.EventLogsRepo.GetAllAsync();
            if (EventLog.Any() && EventLog != null)
            {
                return Mapper.Map<List<tbl_EventLogs>, List<EventLogDto>>(EventLog.ToList());
            }
            return new List<EventLogDto>();
        }
    }
}
