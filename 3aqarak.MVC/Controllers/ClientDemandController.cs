using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Helpers;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.MVC.Controllers
{
    public class ClientDemandController : Controller
    {
        // GET: ClientDemand

        private readonly IClientService _clientService;
        private readonly IDemandService _demandService;
        private readonly IAvailableService _availableService;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;
        private IConfirmation _conf;

        public ClientDemandController(ITransService transService, IClientService clientService, ISpecialService specialService, IDemandService demandService, IConfirmation conf, IAvailableService availableService, IUSerService userService)
        {
            _clientService = clientService;
            _demandService = demandService;
            _conf = conf;
            _availableService = availableService;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
        }
        // GET: ClientDemands
        public ActionResult Index()
        {

            return View();
        }

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

        public ActionResult ClientDemands(string id)
        {
            ViewBag.Id = id;
            return View();
        }
        // ClientDemands/LoadDemandsData
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
            int id = int.Parse(Request.Form.GetValues("id").FirstOrDefault());
            //Paging Size (10,20,50,100)
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

            // Getting all entity data
            List<DemandViewModel> clientDemands = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(await _demandService.ClientDemands(id));

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                clientDemands = clientDemands.Where(e => e.DateString.Contains(tableData.SearchValue)).ToList();
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

        public async Task<ActionResult> AddClientDemand(string clientId)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            DemandViewModel clientDemand = new DemandViewModel()
            {
                FK_DemandUnits_Clients_ClientId = int.Parse(clientId),
                ClientName = (await _clientService.FindClientByID(int.Parse(clientId))).Name,
                RegionsFrom =await  _demandService.GetRegions(),
                RegionsTo =await  _demandService.GetRegions(),
                Categories = await _demandService.GetCats(),
                Finishings =await  _demandService.GetFinishings(),
                Accessories =await _demandService.GetAccess(),
                Views =await _demandService.GetViews(),
                Payments =await _demandService.GetPayments(),
                CreatedAt = DateTime.Now,
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
                clientDemand.Transactions = await _demandService.GetTrans(null);
            }
            return View(clientDemand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveClientDemand(DemandViewModel demandVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;

            if (ModelState.IsValid)
            {
                _conf = await _demandService.SaveClientDemand(Mapper.Map<DemandViewModel, DemandDto>(demandVM), userId, branchId);
            }
            if (_conf.Valid)
            {
                DemandAvailableMatchViewModel matchVM = new DemandAvailableMatchViewModel
                {
                    Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(demandVM.FK_DemandUnits_Clients_ClientId)),
                    Demand = Mapper.Map<DemandDto, DemandViewModel>(_conf.Demands.FirstOrDefault()),
                    availables = Mapper.Map<List<AvailableDto>, List<AvailableViewModel>>(_conf.AvailablesAndExcluded.Item1),
                    SameClientAvailable = Mapper.Map<AvailableDto, AvailableViewModel>(_conf.AvailablesAndExcluded.Item2),
                    ExcludedavailablesForPreviws = Mapper.Map<List<AvailableDto>, List<AvailableViewModel>>(_conf.AvailablesAndExcluded.Item3)
                };
                Session["AddAvailableMatches"] = matchVM;
                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditClientDemand(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            DemandViewModel ClientDemand = Mapper.Map<DemandDto, DemandViewModel>(await _demandService.EditClientDemand(int.Parse(id)));
            ClientDemand.RegionsFrom = await _demandService.GetRegionById(ClientDemand.FK_DemandUnits_Regions_FromId);
            ClientDemand.RegionsTo = await _demandService.GetRegionById(ClientDemand.FK_DemandUnits_Regions_ToId);
            ClientDemand.Categories = await _demandService.GetCatsById(ClientDemand.FK_DemandUnits_Categories_Id);
            ClientDemand.Finishings =await _demandService.GetFinishingsById(ClientDemand.FinishIds);
            ClientDemand.Accessories = await _demandService.GetAccessById(ClientDemand.AccessoriesIds);
            ClientDemand.Views =await _demandService.GetViewsById(ClientDemand.ViewsIds);
            ClientDemand.Payments = await _demandService.GetPaymentsId(ClientDemand.FK_DemandUnits_PaymentMethod_Id);
            ClientDemand.Usages = await _demandService.GetUsagesId(ClientDemand.FK_DemandUnits_Usage_Id);
            UserDto sales = (await _userService.FindUserByID(ClientDemand.FK_DemandUnits_Users_Sales));
            ViewBag.salesName = sales.FirstName + " " + sales.LastName;
            int? userSpecialization = (await _userService.FindUserByID(userId)).Specialization_Id;
            if (userSpecialization.HasValue)
            {
                string specialName = (await _specialService.FindByID(userSpecialization.Value)).Name;
                if (specialName == "بيع")
                {
                    ClientDemand.Transactions = await _demandService.GetTrans(specialName);

                }
                else if (specialName == "ايجار")
                {
                    ClientDemand.Transactions =await  _demandService.GetTrans(specialName);
                }
            }
            else
            {
                ClientDemand.Transactions =await _demandService.GetTrans(null);
            }
            ClientDemand.TransType = (await _transService.FindByID(ClientDemand.FK_DemandUnits_Transactions_Id)).TransType;
            return View(ClientDemand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateClientDemand(DemandViewModel demandVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            if (ModelState.IsValid)
            {
                _conf = await _demandService.UpdateClientDemand(Mapper.Map<DemandViewModel, DemandDto>(demandVM), userId, branchId);            }
            if (_conf.Valid)
            {
                DemandAvailableMatchViewModel matchVM = new DemandAvailableMatchViewModel
                {
                    Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(demandVM.FK_DemandUnits_Clients_ClientId)),
                    Demand = Mapper.Map<DemandDto, DemandViewModel>(_conf.Demands.FirstOrDefault()),
                    availables = Mapper.Map<List<AvailableDto>, List<AvailableViewModel>>(_conf.AvailablesAndExcluded.Item1),
                    SameClientAvailable = Mapper.Map<AvailableDto, AvailableViewModel>(_conf.AvailablesAndExcluded.Item2),
                    ExcludedavailablesForPreviws = Mapper.Map<List<AvailableDto>, List<AvailableViewModel>>(_conf.AvailablesAndExcluded.Item3)
                };
                Session["UpdateAvailableMatches"] = matchVM;
                
                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteClientDemand(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;

            _conf = await _demandService.DeleteClientDemand(int.Parse(id), userId);

            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DemandDetails(int id)
        {
            DemandViewModel ClientDemand = Mapper.Map<DemandDto, DemandViewModel>(await _demandService.EditClientDemand(id));
            ClientDemand.RegionsFrom = await _demandService.GetRegionById(ClientDemand.FK_DemandUnits_Regions_FromId);
            ClientDemand.RegionsTo = await _demandService.GetRegionById(ClientDemand.FK_DemandUnits_Regions_ToId);
            ClientDemand.Transactions = await _demandService.GetTransById(ClientDemand.FK_DemandUnits_Transactions_Id, null);
            ClientDemand.Categories = await _demandService.GetCatsById(ClientDemand.FK_DemandUnits_Categories_Id);
            ClientDemand.Finishings =await _demandService.GetFinishingsById(ClientDemand.FinishIds);
            ClientDemand.Accessories =await _demandService.GetAccessById(ClientDemand.AccessoriesIds);
            ClientDemand.Views = await _demandService.GetViewsById(ClientDemand.ViewsIds);
            ClientDemand.Payments =await _demandService.GetPaymentsId(ClientDemand.FK_DemandUnits_PaymentMethod_Id);
            return PartialView("_DemandDetails", ClientDemand);
        }

        public ActionResult AvailableMatchesAfterUpdate()
        {
            DemandAvailableMatchViewModel matches = (DemandAvailableMatchViewModel)Session["UpdateAvailableMatches"];
            return View(matches);
        }

        public ActionResult AvailableMatchesAfterAdd()
        {
            DemandAvailableMatchViewModel matches = (DemandAvailableMatchViewModel)Session["AddAvailableMatches"];
            return View(matches);
        }

        public async Task<ActionResult> SelectAvailables(string[] availableIds, string demandId, string buyerId, string[] dates)
        {
            DemandAvailableMatchViewModel matchVM = new DemandAvailableMatchViewModel();
            if (availableIds.Any() && availableIds != null)
            {
                availableIds = availableIds.FirstOrDefault().Split(',');
                foreach (string item in availableIds)
                {
                    AvailableViewModel available = Mapper.Map<AvailableDto, AvailableViewModel>(await _availableService.EditClientSale(int.Parse(item)));
                    available.SellerName = (await _clientService.FindClientByID(available.FK_AvaliableUnits_Clients_ClientId)).Name;
                    matchVM.availables.Add(available);
                }
            }
            if (dates.Any() && dates != null)
            {
                dates = dates.FirstOrDefault().Split(',');
                matchVM.Dates = dates.Select(d => DateTime.Parse(d)).ToList();

            }

            if (!string.IsNullOrEmpty(demandId))
            {
                matchVM.Demand = Mapper.Map<DemandDto, DemandViewModel>(await _demandService.EditClientDemand(int.Parse(demandId)));
            }

            if (!string.IsNullOrEmpty(buyerId))
            {
                matchVM.Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(int.Parse(buyerId)));
            }

            return View(matchVM);
        }

        public async Task<ActionResult> GetInstantMatches(string id)
        {
            _conf = await _demandService.GetInstantMatches(int.Parse(id));
            DemandAvailableMatchViewModel matchVM = new DemandAvailableMatchViewModel
            {
                Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(_conf.Demands.FirstOrDefault().FK_DemandUnits_Clients_ClientId)),
                Demand = Mapper.Map<DemandDto, DemandViewModel>(_conf.Demands.FirstOrDefault()),
                ExcludedavailablesForPreviws = Mapper.Map<List<AvailableDto>, List<AvailableViewModel>>(_conf.AvailablesAndExcluded.Item3),
                SameClientAvailable = Mapper.Map<AvailableDto, AvailableViewModel>(_conf.AvailablesAndExcluded.Item2),
                availables = Mapper.Map<List<AvailableDto>, List<AvailableViewModel>>(_conf.AvailablesAndExcluded.Item1)
            };
            return View("AvailableMatchesAfterAdd", matchVM);
        }

        public async Task<ActionResult> ViewMatchedDemands(string demands)
        {

            List<DemandDto> demandList = !string.IsNullOrEmpty(demands) ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<DemandDto>>(demands) : new List<DemandDto>();
            List<DemandViewModel> ViewModelDemands = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(demandList);
            foreach (DemandViewModel demand in ViewModelDemands)
            {
                demand.BuyerName = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(demand.FK_DemandUnits_Clients_ClientId)).Name;
            }
            return View(ViewModelDemands);
        }
    }
}
