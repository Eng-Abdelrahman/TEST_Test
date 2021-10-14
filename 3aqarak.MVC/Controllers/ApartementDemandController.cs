using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Helpers;
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
    public class ApartementDemandController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IDemandService _demandService;
        private readonly IAvailableService _availableService;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;
        private readonly IRegionService _regionService;
        private readonly IFinishService _finishService;
        private IConfirmation _conf;
        public ApartementDemandController(ITransService transService, IClientService clientService, ISpecialService specialService, IDemandService demandService, IConfirmation conf, IAvailableService availableService, IUSerService userService, IRegionService regionService, IFinishService finishService)
        {
            _clientService = clientService;
            _demandService = demandService;
            _conf = conf;
            _availableService = availableService;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
            _regionService = regionService;
            _finishService = finishService;
        }
        public async Task<ActionResult> AddDemand()
        {
            int catId = Categories.Apartements;
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            MixDemandClientViewModel clientDemand = new MixDemandClientViewModel()
            {
                RegionsFrom =await  _demandService.GetRegions(),
                RegionsTo = await _demandService.GetRegions(),
                Categories = await _demandService.GetCats(),
                Finishings =await _demandService.GetFinishings(),
                Usages =await _demandService.GetUsages(),
                Accessories = await _demandService.GetAccess(),
                Views =await _demandService.GetViews(),
                Payments =await _demandService.GetPayments(),               
                CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                FK_DemandUnits_Categories_Id =catId,
            };
            int? userSpecialization = (await _userService.FindUserByID(userId)).Specialization_Id;
            if (userSpecialization.HasValue)
            {
                string specialName = (await _specialService.FindByID(userSpecialization.Value)).Name;
                if (specialName == "بيع")
                {
                    clientDemand.Transactions =await _demandService.GetTrans(specialName);

                }
                else if (specialName == "ايجار")
                {
                    clientDemand.Transactions =await _demandService.GetTrans(specialName);
                }
            }
            else
            {
                clientDemand.Transactions =await _demandService.GetTrans(null);
            }
            return View(clientDemand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveDemand(MixDemandClientViewModel clientDemandVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            var ClientId =clientDemandVM.PK_Client_Id;
            if (ModelState.IsValid)
            {
                var clientDemandDto = Mapper.Map<MixDemandClientViewModel, MixDemandClientDto>(clientDemandVM);
                if (ClientId==0)
                {
                    var clientDto = Mapper.Map<MixDemandClientDto, ClientDto>(clientDemandDto);
                    _conf = await _clientService.SaveClient(clientDto, userId);
                    if (_conf.Valid && _conf.Id > 0)
                    {
                        ClientId = _conf.Id;
                        var demandDto = Mapper.Map<MixDemandClientDto, DemandDto>(clientDemandDto);
                        demandDto.FK_DemandUnits_Clients_ClientId = _conf.Id;
                        _conf = await _demandService.SaveClientDemand(demandDto, userId, branchId);

                    }
                }
                else
                {
                    var demandDto = Mapper.Map<MixDemandClientDto, DemandDto>(clientDemandDto);
                    demandDto.FK_DemandUnits_Clients_ClientId = ClientId;
                    _conf = await _demandService.SaveClientDemand(demandDto, userId, branchId);
                }
                if (_conf.Valid)
                {
                    DemandAvailableMatchViewModel matchVM = new DemandAvailableMatchViewModel
                    {
                        Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(ClientId)),
                        Demand = Mapper.Map<DemandDto, DemandViewModel>(_conf.Demands.FirstOrDefault()),
                        availables = Mapper.Map<List<AvailableDto>, List<AvailableViewModel>>(_conf.AvailablesAndExcluded.Item1),
                        SameClientAvailable = Mapper.Map<AvailableDto, AvailableViewModel>(_conf.AvailablesAndExcluded.Item2),
                        ExcludedavailablesForPreviws = Mapper.Map<List<AvailableDto>, List<AvailableViewModel>>(_conf.AvailablesAndExcluded.Item3),
                    };
                    Session["AddAvailableMatches"] = matchVM;
                    return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
                }
            }
         
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }       
              
        public async Task<ActionResult> GetMatchesOnTheFly(DemandViewModel demandVM)
        {
            List<AvailableDto> availables = await _demandService.GetAvailableMatchesOnTheFly(Mapper.Map<DemandViewModel, DemandDto>(demandVM));

            return Json(availables, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> LoadSalesData()
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
            DemandViewModel demandVM = !string.IsNullOrEmpty(Request.Form["demandVM"]) ? Newtonsoft.Json.JsonConvert.DeserializeObject<DemandViewModel>(Request.Form["demandVM"]) : null;
            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await GetAvailableTableData(data, demandVM);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.ClientSales,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetAvailableTableData(DataTableViewModel tableData, DemandViewModel demandVM)
        {

            // Getting all entity data
            List<AvailableViewModel> clientSales = (demandVM != null) ? Mapper.Map<List<AvailableDto>, List<AvailableViewModel>>(await _demandService.GetAvailableMatchesOnTheFly(Mapper.Map<DemandViewModel, DemandDto>(demandVM))) : new List<AvailableViewModel>();

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                clientSales = clientSales.Where(e => e.DateString.Contains(tableData.SearchValue)).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    clientSales = clientSales.OrderBy(e => e.DateString).ToList();
                }
                else
                {
                    clientSales = clientSales.OrderByDescending(e => e.DateString).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = clientSales.Count();

            //Paging
            clientSales = clientSales.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.ClientSales = clientSales;

            return tableData;
        }


        public async Task<ActionResult> ClientAutoComplete(string text)
        {
            var clients = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>(await _clientService.ClientAutoComplete(text));
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
        //ApartmentAvailables
        public async Task<ActionResult> GetClientdetailsAsync(string id)
        {
            var clientId = int.Parse(id);
            var clientVM = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(clientId));
            ViewBag.RedirectTo = HttpContext.Items["RedirectUrl"]?.ToString();
            return Json(new { clientList = clientVM }, JsonRequestBehavior.AllowGet);
        }


    }
}