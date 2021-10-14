using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.ViewModels;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Controllers
{
    
    public class FellowupCallController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IFellowCallService _callService;
        private IConfirmation _conf;

        public FellowupCallController( IClientService clientService,IFellowCallService callService)
        {
            _clientService = clientService;
            _callService = callService;
        }
        public ActionResult Index()
        {

            return View();
        }
        // GET: FellowupCall
        [HttpPost]
        public async Task<ActionResult> LoadData()
        {
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
            DataTableViewModel tableData = await GetTableData(data);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.Clients,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetTableData(DataTableViewModel tableData)
        {

            // Getting all entity data
            List<ClientsViewModel> entityList = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>(await _clientService.GetClients());

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                entityList = entityList.Where(e => e.Name.Contains(tableData.SearchValue) || e.Mobile.Contains(tableData.SearchValue) || (!string.IsNullOrEmpty(e.Mobile2) && e.Mobile2.Contains(tableData.SearchValue))).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    entityList = entityList.OrderBy(e => e.Name).ToList();
                }
                else
                {
                    entityList = entityList.OrderByDescending(e => e.Name).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = entityList.Count();

            //Paging
            entityList = entityList.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.Clients = entityList;

            return tableData;
        }

        [HttpGet]
        public async Task<ActionResult> RegisterCall(int id)
        {
            var client = await _clientService.FindClientByID(id);
            var callVM = new FellowupCallViewModel();
            callVM.ClientName = client.Name;
            callVM.PhoneNumber1 = client.Mobile;
            callVM.PhoneNumber2 = client.Mobile2??client.Mobile;
            callVM.ClientId= id;       

            return PartialView("_FellowupCall", callVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> savecall(FellowupCallViewModel callVM)
        {
            var validCall = false;
            if (callVM.DateTime < DateTime.UtcNow.AddMinutes(120))
            {
                return Json(new { valid = validCall, message = " لقد ادخلت تاريخااو توقيتا قبل تاريخ اليوم او التوقيت الحالي" }, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                var callDto = Mapper.Map<FellowupCallViewModel, FellowCallDto>(callVM);
                callDto.ClientId = callVM.ClientId;
                callDto.Notes = callVM.Notes;
                validCall = await _callService.SaveFellowupCall(callDto,userId);
                if (validCall)
                {
                    return Json(new { valid = validCall, message = " تم الحفظ بنجاح" }, JsonRequestBehavior.AllowGet);

                }
            }
            return Json(new { valid = validCall, message = " لم يتم الحفظ بنجاح" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FellowupCallsIndex(string calls)
        {

            var callList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FellowCallDto>>(calls);
            return View(Mapper.Map<List<FellowCallDto>, List<FellowupCallViewModel>>(callList));
        }

        public async Task<ActionResult> ConfirmCall(int id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var confirmed = await _callService.ConfirmCall(id, userId);
            return Json(confirmed, JsonRequestBehavior.AllowGet); ;
        }
        public async Task<ActionResult> CancellCall(int id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var confirmed = await _callService.CancellCall(id, userId);
            return Json(confirmed, JsonRequestBehavior.AllowGet); ;
        }


    }
}