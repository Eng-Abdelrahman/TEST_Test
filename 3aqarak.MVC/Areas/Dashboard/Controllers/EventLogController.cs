using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Areas.Dashboard.ViewModels;
using _3aqarak.MVC.Filters;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Areas.Dashboard.Controllers
{
    [IsAdminFilter]
    public class EventLogController : Controller
    {
        private readonly IEventLogService _EventLogService;
        private IConfirmation _conf;

        public EventLogController(IEventLogService EventLogService, IConfirmation conf)
        {
            _EventLogService = EventLogService;
            _conf = conf;
        }
        // GET: Dashboard/EventLog
        public ActionResult Index()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> LoadData()
        {
            var fromDate = new DateTime();
            var toDate = new DateTime();
            DataTableViewModel data = new DataTableViewModel
            {
                Draw = Request.Form.GetValues("draw").FirstOrDefault(),
                Start = Request.Form.GetValues("start").FirstOrDefault(),
                Length = Request.Form.GetValues("length").FirstOrDefault(),
                SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault(),
                SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault(),
                SearchValue = Request.Form.GetValues("search[value]").FirstOrDefault(),
            };

            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            string Event= "All";
            if (Request.Form.GetValues("Event").First() == "0")
            { Event= "Insert".ToString(); }
            else if (Request.Form.GetValues("Event").First() == "1")
            { Event = "Delete".ToString(); }
            if (Request.Form.GetValues("Event").First() == "2")
            { Event = "Update".ToString(); }

            //int User = Request.Form.GetValues("type") != null ? int.Parse(Request.Form.GetValues("type")[0]) : 0;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("fromDate").FirstOrDefault()))
            {
                fromDate = DateTime.Parse(Request.Form.GetValues("fromDate").FirstOrDefault());
            }
            else
            {
                //fromDate = DateTime.Now.Date;
                fromDate = new DateTime(2017, 1, 18);
            }

            if (!string.IsNullOrEmpty(Request.Form.GetValues("toDate").FirstOrDefault()))
            {
                toDate = DateTime.Parse(Request.Form.GetValues("toDate").FirstOrDefault()).AddHours(24);
            }
            else
            {
                //toDate = DateTime.Now.Date;
                toDate = DateTime.Now.AddHours(24);
            }
            DataTableViewModel tableData = await GetEventLogData(data, Event, fromDate, toDate);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.EventLog,
            }, JsonRequestBehavior.AllowGet);
        }

        private async System.Threading.Tasks.Task<DataTableViewModel> GetEventLogData(DataTableViewModel tableData, string Event, DateTime fromDate, DateTime toDate)
        {

            // Getting all entity data
            List<EventLogViewModel> entityList = Mapper.Map<List<EventLogDto>, List<EventLogViewModel>>(await _EventLogService.FindByID(fromDate.Date, toDate, Event));

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                entityList = entityList.Where(e => e.DateString.Contains(tableData.SearchValue) || e.EventType.Contains(tableData.SearchValue) || e.UserName.Contains(tableData.SearchValue)).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    entityList = entityList.OrderBy(e => e.UserName).ToList();
                }
                else
                {
                    entityList = entityList.OrderByDescending(e => e.UserName).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = entityList.Count();

            //Paging
            entityList = entityList.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.EventLog = entityList;

            return tableData;
        }

    }
}