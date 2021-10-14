using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Helpers;
using _3aqarak.MVC.Hubs;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.MVC.Controllers
{
    public class ApartmentAvailablesController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IAvailableService _availableService;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;
        private readonly IFinishService _finishService;
        private readonly IRegionService _regionService;
        private readonly IDemandService _demandService;
        private IConfirmation _conf;

        public ApartmentAvailablesController(IDemandService demandService, IClientService clientService, ISpecialService specialService, IAvailableService availableService, IConfirmation conf, IUSerService userService, ITransService transService, IFinishService finishService, IRegionService regionService)
        {
            _clientService = clientService;
            _availableService = availableService;
            _conf = conf;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
            _finishService = finishService;
            _regionService = regionService;
            _demandService = demandService;
        }

        // GET: ApartmentAvailables
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public async Task<ActionResult> AddClientSale()
        {
            int catId = Categories.Apartements;
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            MixedAvailableClientViewModel clientSale = new MixedAvailableClientViewModel()
            {
                Regions =await  _availableService.GetRegionsAsync(),
                Categories =await _availableService.GetCatsAsync(),
                Finishings =await  _availableService.GetFinishingsAsync(),
                Accessories =await _availableService.GetAccessAsync(),
                Views =await _availableService.GetViewsAsync(),
                Payments =await _availableService.GetPaymentsAsync(),
                CreatedAt = DateTime.UtcNow.AddMinutes(120),
                Usages =await _demandService.GetUsages(),

            };
            clientSale.tbl_units = new UnitViewModel() { FK_Units_Categories_Id = catId };
            
            int? userSpecialization = (await _userService.FindUserByID(userId)).Specialization_Id;
            if (userSpecialization.HasValue)
            {
                string specialName = (await _specialService.FindByID(userSpecialization.Value)).Name;
                if (specialName == "بيع")
                {
                    clientSale.Transactions = await _availableService.GetTransAsync(specialName);

                }
                else if (specialName == "ايجار")
                {
                    clientSale.Transactions = await _availableService.GetTransAsync(specialName);
                }
            }
            else
            {
                clientSale.Transactions = await _availableService.GetTransAsync(null);
            }
            return View(clientSale);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveClientSale(MixedAvailableClientViewModel availableClientVM )
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            var ClientId = availableClientVM.FK_AvaliableUnits_Clients_ClientId;
            List<string> images = new List<string>();
            if (ModelState.IsValid)
            {
                if (Session["ClientSalesImagePathList"] != null)
                {
                    images = (List<string>)Session["ClientSalesImagePathList"];
                }
                var clientAvailableDto = Mapper.Map<MixedAvailableClientViewModel, MixedAvailableClientDto>(availableClientVM);
                if (ClientId == 0)
                {
                    var AvailableclientDto = Mapper.Map<MixedAvailableClientDto, ClientDto>(clientAvailableDto);
                    _conf = await _clientService.SaveClient(AvailableclientDto, userId);
                    if (_conf.Valid && _conf.Id > 0)
                    {
                        ClientId = _conf.Id;
                        var AvailableDto = Mapper.Map<MixedAvailableClientDto, AvailableDto>(clientAvailableDto);
                        AvailableDto.FK_AvaliableUnits_Clients_ClientId = _conf.Id;
                        _conf =await  _availableService.CheckDuplicateClientSales(AvailableDto);
                        if (!_conf.Valid)
                        {
                            return Json(new { message = "هذا العميل لدية نفس العرض", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
                        }                      
                        _conf = await _availableService.SaveClientSale(AvailableDto, userId, images, branchId);                       
                    }
                }
                else
                {
                    var AvailableDto = Mapper.Map<MixedAvailableClientDto, AvailableDto>(clientAvailableDto);
                    AvailableDto.FK_AvaliableUnits_Clients_ClientId = ClientId;
                    _conf = await _availableService.CheckDuplicateClientSales(AvailableDto);
                    if (!_conf.Valid)
                    {
                        return Json(new { message = "هذا العميل لدية نفس العرض", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);                  
                    }
                    _conf = await _availableService.SaveClientSale(AvailableDto, userId, images, branchId);
                    
                }
                if (_conf.Valid)
                {
                    AvailableDemandMatchViewModel matchVM = new AvailableDemandMatchViewModel
                    {
                        Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(ClientId)),
                        available = Mapper.Map<AvailableDto, AvailableViewModel>(_conf.availables.FirstOrDefault()),
                        Demands = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(_conf.DemandsAndExcluded.Item1),
                        DemandsWithPreviews = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(_conf.DemandsAndExcluded.Item3),
                        DemandsWithSameClient = Mapper.Map<DemandDto, DemandViewModel>(_conf.DemandsAndExcluded.Item2),
                    };
                    Session["AddDemandMatches"] = matchVM;
                    NotificationHub.showDemandmatchedNotifications(_conf.DemandsAndExcluded.Item4.Select(item=> new MatchedDemandsHelper { PK_DemandUnits_Id = item.PK_DemandUnits_Id, FK_DemandUnits_Users_CreatedBy = item.FK_DemandUnits_Users_CreatedBy}).ToList(), matchVM.available.PK_AvailableUnits_Id);
                    return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);

                }              
            }

            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> LoadDemandsData()
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
            int id = !string.IsNullOrEmpty(Request.Form.GetValues("id").FirstOrDefault()) ? int.Parse(Request.Form.GetValues("id").FirstOrDefault()) : 0;
            
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await GetDemandTableData(data, id);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.ClientDemands,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetDemandTableData(DataTableViewModel tableData, int id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            // Getting all entity data
            List<DemandViewModel> clientDemands = Mapper.Map<List<DemandDto>, List<DemandViewModel>>((await _demandService.ClientDemands(id)).Where(a => a.FK_DemandUnits_Users_CreatedBy==userId).ToList());
            foreach (var item in clientDemands)
            {
                item.BuyerName = (await _clientService.FindClientByID(item.FK_DemandUnits_Clients_ClientId)).Name;
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    clientDemands = clientDemands.OrderBy(e => e.DateString).ToList();
                }
                else
                {
                    clientDemands = clientDemands.OrderByDescending(e => e.DateString).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = clientDemands.Count();

            //Paging
            clientDemands = clientDemands.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.ClientDemands = clientDemands;

            return tableData;
        }

        [HttpPost]
        public async Task<ActionResult> GetDemansList(AvailableViewModel availableViewModel)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            (List<DemandDto>, List<DemandDto>) demandlist = await _availableService.GetAvailableMatches(Mapper.Map<AvailableViewModel, AvailableDto>(availableViewModel), userId);
            List<DemandViewModel> demands = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(demandlist.Item1);

            foreach (var demand in demands)
            {
                demand.ClientName = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(demand.FK_DemandUnits_Clients_ClientId)).Name;
            }
            return Json(new { demanList = demands }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ClientAutoComplete(string text)
        {
            var clients = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>(await _clientService.ClientAutoComplete(text));
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
        //ApartmentAvailables/GetClientdetails
        public async Task<ActionResult> GetClientdetails(string id)
        {
            var clientId = int.Parse(id);
            var clientVM = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(clientId));
            ViewBag.RedirectTo = HttpContext.Items["RedirectUrl"]?.ToString();
            return Json(new { clientList = clientVM }, JsonRequestBehavior.AllowGet);
        }
    }
}