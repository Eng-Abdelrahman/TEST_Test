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
    public class VillasDemandController : Controller
    {

        private readonly IClientService _clientService;
        private readonly IVillasDemandService _demandService;
        private readonly IVillasAvailablesService _availableService;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;
        private IConfirmation _conf;

        public VillasDemandController(ITransService transService, IClientService clientService, ISpecialService specialService, IVillasDemandService demandService, IConfirmation conf, IVillasAvailablesService availableService, IUSerService userService)
        {
            _clientService = clientService;
            _demandService = demandService;
            _conf = conf;
            _availableService = availableService;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
        }


        // GET: VillasDemand/AddDemand
        public async Task<ActionResult> AddDemand()
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            VillaClientDemandViewModel clientDemand = new VillaClientDemandViewModel()
            {
                RegionsFrom = await _demandService.GetRegions(),
                RegionsTo = await _demandService.GetRegions(),
                Finishings = await _demandService.GetFinishings(),
                Usages = await _demandService.GetUsages(),
                Accessories = await _demandService.GetAccess(),
                Views = await _demandService.GetViews(),
                Payments = await _demandService.GetPayments(),
                CreatedAt = DateTime.UtcNow.AddMinutes(120),
                FK_VillasDemands_Categories_Id = Categories.Villas,
            };
            int? userSpecialization = (await _userService.FindUserByID(userId)).Specialization_Id;
            if (userSpecialization.HasValue)
            {
                string specialName = (await _specialService.FindByID(userSpecialization.Value)).Name;
                if (specialName == "بيع")
                {
                    clientDemand.Transactions = await _demandService.GetTrans(specialName);

                }
                else if (specialName == "ايجار")
                {
                    clientDemand.Transactions = await _demandService.GetTrans(specialName);
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
        public async Task<ActionResult> SaveDemand(VillaClientDemandViewModel clientDemandVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            int ClientId = clientDemandVM.PK_Client_Id;
            if (ModelState.IsValid)
            {
                VillasDemandDto clientDemandDto = Mapper.Map<VillaClientDemandViewModel, VillasDemandDto>(clientDemandVM);
                if (ClientId == 0)
                {
                    ClientDto clientDto = Mapper.Map<VillasDemandDto, ClientDto>(clientDemandDto);
                    _conf = await _clientService.SaveClient(clientDto, userId);
                    if (_conf.Valid && _conf.Id > 0)
                    {
                        ClientId = _conf.Id;
                        clientDemandDto.FK_VillasDemands_Clients_ClientId = _conf.Id;
                        _conf = await _demandService.SaveVillaDemand(clientDemandDto, userId, branchId);
                    }
                }
                else
                {
                    //var demandDto = Mapper.Map<VillasDemandDto, VillasDemandDto>(clientDemandDto);
                    clientDemandDto.FK_VillasDemands_Clients_ClientId = ClientId;
                    _conf = await _demandService.SaveVillaDemand(clientDemandDto, userId, branchId);
                }
                if (_conf.Valid)
                {
                    VillasAvailableDemandViewModel matchVM = new VillasAvailableDemandViewModel
                    {
                        Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(ClientId)),
                        Demand = Mapper.Map<VillasDemandDto, VillaClientDemandViewModel>(_conf.VillDemands.FirstOrDefault()),
                        availables = Mapper.Map<List<VillasAvailableDto>, List<VillsAvailableViewModel>>(_conf.VillAvailablesAndExcluded.Item1),
                        SameClientAvailable = Mapper.Map<VillasAvailableDto, VillsAvailableViewModel>(_conf.VillAvailablesAndExcluded.Item2),
                        ExcludedAvailablesForPreviws = Mapper.Map<List<VillasAvailableDto>, List<VillsAvailableViewModel>>(_conf.VillAvailablesAndExcluded.Item3),
                    };
                    Session["AddVillAvailableMatches"] = matchVM;
                    return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> EditVilladDemand(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            VillaDemandViewModel ClientDemand = Mapper.Map<VillasDemandDto, VillaDemandViewModel>(await _demandService.EditVillaDemand(int.Parse(id)));
            ClientDemand.RegionsFrom = await _demandService.GetRegionById(ClientDemand.FK_VillasDemands_Regions_FromId);
            ClientDemand.RegionsTo = await _demandService.GetRegionById(ClientDemand.FK_VillasDemands_Regions_ToId);
            ClientDemand.Finishings = await _demandService.GetFinishingsById(ClientDemand.FinishIds);
            ClientDemand.Accessories = await _demandService.GetAccessById(ClientDemand.AccessoriesIds);
            ClientDemand.Views = await _demandService.GetViewsById(ClientDemand.ViewsIds);
            ClientDemand.Payments = await _demandService.GetPaymentsId(ClientDemand.FK_VillasDemands_PaymentMethod_Id);
            ClientDemand.FK_VillasDemands_Categories_Id = Categories.Villas;
            UserDto sales = (await _userService.FindUserByID(ClientDemand.FK_VillasDemands_Users_SalesId));
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
                    ClientDemand.Transactions = await _demandService.GetTrans(specialName);
                }
            }
            else
            {
                ClientDemand.Transactions = await _demandService.GetTrans(null);
            }
            ClientDemand.TransType = (await _transService.FindByID(ClientDemand.FK_VillasDemands_Transactions_Id)).TransType;
            ClientDemand.Usages = await _demandService.GetUsagesId(ClientDemand.FK_VillasDemands_Usage_Id);
            return View(ClientDemand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateVilladDemand(VillaDemandViewModel demandVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            if (ModelState.IsValid)
            {
                _conf = await _demandService.UpdateVillaDemand(Mapper.Map<VillaDemandViewModel, VillasDemandDto>(demandVM), userId, branchId);
                if (_conf.Valid)
                {
                    VillasAvailableDemandViewModel matchVM = new VillasAvailableDemandViewModel
                    {
                        Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(demandVM.FK_VillasDemands_Clients_ClientId)),
                        Demand = Mapper.Map<VillasDemandDto, VillaClientDemandViewModel>(_conf.VillDemands.FirstOrDefault()),
                        availables = Mapper.Map<List<VillasAvailableDto>, List<VillsAvailableViewModel>>(_conf.VillAvailablesAndExcluded.Item1),
                        SameClientAvailable = Mapper.Map<VillasAvailableDto, VillsAvailableViewModel>(_conf.VillAvailablesAndExcluded.Item2),
                        ExcludedAvailablesForPreviws = Mapper.Map<List<VillasAvailableDto>, List<VillsAvailableViewModel>>(_conf.VillAvailablesAndExcluded.Item3),
                    };
                    Session["UpdateVillAvailableMatches"] = matchVM;
                    return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);

                }

            }

            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteVillaDemand(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;

            _conf = await _demandService.DeleteVillaDemand(int.Parse(id), userId);

            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);
        }
        //VillasDemand/DemandDetails
        public async Task<ActionResult> DemandDetails(int id)
        {
            VillaClientDemandViewModel ClientDemand = Mapper.Map<VillasDemandDto, VillaClientDemandViewModel>(await _demandService.EditVillaDemand(id));
            ClientDemand.RegionsFrom = await _demandService.GetRegionById(ClientDemand.FK_VillasDemands_Regions_FromId);
            ClientDemand.RegionsTo = await _demandService.GetRegionById(ClientDemand.FK_VillasDemands_Regions_ToId);
            ClientDemand.Transactions = await _demandService.GetTransById(ClientDemand.FK_VillasDemands_Transactions_Id, null);
            ClientDemand.Finishings = await _demandService.GetFinishingsById(ClientDemand.FinishIds);
            ClientDemand.Accessories = await _demandService.GetAccessById(ClientDemand.AccessoriesIds);
            ClientDemand.Usages = await _demandService.GetUsagesId(ClientDemand.FK_VillasDemands_Usage_Id);
            ClientDemand.Views = await _demandService.GetViewsById(ClientDemand.ViewsIds);
            ClientDemand.Payments = await _demandService.GetPaymentsId(ClientDemand.FK_VillasDemands_PaymentMethod_Id);
            ClientDemand.FK_VillasDemands_Categories_Id = Categories.Villas;
            return PartialView("_VillaDemandDetails", ClientDemand);
        }

        public ActionResult AvailableMatchesAfterUpdate()
        {
            VillasAvailableDemandViewModel matches = (VillasAvailableDemandViewModel)Session["UpdateVillAvailableMatches"];
            return View(matches);
        }

        public ActionResult AvailableMatchesAfterAdd()
        {
            VillasAvailableDemandViewModel matches = (VillasAvailableDemandViewModel)Session["AddVillAvailableMatches"];
            return View(matches);
        }
        //VillasDemand/SelectAvailables
        public async Task<ActionResult> SelectAvailables(string[] availableIds, string demandId, string buyerId, string[] dates)
        {
            VillasAvailableDemandViewModel matchVM = new VillasAvailableDemandViewModel();
            if (availableIds.Any() && availableIds != null)
            {
                availableIds = availableIds.FirstOrDefault().Split(',');
                foreach (string item in availableIds)
                {
                    VillsAvailableViewModel available = Mapper.Map<VillasAvailableDto, VillsAvailableViewModel>(await _availableService.EditClientSale(int.Parse(item)));
                    available.SellerName = (await _clientService.FindClientByID(available.FK_VillasAvailables_Clients_ClientId)).Name;
                    matchVM.availables.Add(available);
                }
            }
            if (dates.Any() & dates != null)
            {
                dates = dates.FirstOrDefault().Split(',');
                matchVM.Dates = dates.Select(d => DateTime.Parse(d)).ToList();

            }

            if (!string.IsNullOrEmpty(demandId))
            {
                matchVM.Demand = Mapper.Map<VillasDemandDto, VillaClientDemandViewModel>(await _demandService.EditVillaDemand(int.Parse(demandId)));
            }

            if (!string.IsNullOrEmpty(buyerId))
            {
                matchVM.Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(int.Parse(buyerId)));
            }

            return View(matchVM);
        }
        //VillasDemand/GetInstantMatches
        public async Task<ActionResult> GetInstantMatches(string id)
        {
            _conf = await _demandService.GetInstantMatches(int.Parse(id));
            VillasAvailableDemandViewModel matchVM = new VillasAvailableDemandViewModel
            {
                Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(_conf.VillDemands.FirstOrDefault().FK_VillasDemands_Clients_ClientId)),
                Demand = Mapper.Map<VillasDemandDto, VillaClientDemandViewModel>(_conf.VillDemands.FirstOrDefault()),
                availables = Mapper.Map<List<VillasAvailableDto>, List<VillsAvailableViewModel>>(_conf.VillAvailablesAndExcluded.Item1),
                SameClientAvailable = Mapper.Map<VillasAvailableDto, VillsAvailableViewModel>(_conf.VillAvailablesAndExcluded.Item2),
                ExcludedAvailablesForPreviws = Mapper.Map<List<VillasAvailableDto>, List<VillsAvailableViewModel>>(_conf.VillAvailablesAndExcluded.Item3),
            };
            return View("AvailableMatchesAfterAdd", matchVM);
        }

        public async Task<ActionResult> ViewMatchedDemands(string demands)
        {

            List<VillasDemandDto> demandList = !string.IsNullOrEmpty(demands) ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<VillasDemandDto>>(demands) : new List<VillasDemandDto>();
            List<VillaClientDemandViewModel> ViewModelDemands = Mapper.Map<List<VillasDemandDto>, List<VillaClientDemandViewModel>>(demandList);
            foreach (VillaClientDemandViewModel demand in ViewModelDemands)
            {
                demand.BuyerName = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(demand.FK_VillasDemands_Clients_ClientId)).Name;
            }
            return View(ViewModelDemands);
        }

        public async Task<ActionResult> GetMatchesOnTheFly(VillaClientDemandViewModel demandVM)
        {
            List<VillasAvailableDto> availables = await _demandService.GetAvailableMatchesOnTheFly(Mapper.Map<VillaClientDemandViewModel, VillasDemandDto>(demandVM));

            return Json(availables, JsonRequestBehavior.AllowGet);
        }

        //VillasDemand/LoadSalesData
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
            VillaClientDemandViewModel demandVM = !string.IsNullOrEmpty(Request.Form["demandVM"]) ? Newtonsoft.Json.JsonConvert.DeserializeObject<VillaClientDemandViewModel>(Request.Form["demandVM"]) : null;
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
                data = tableData.VillasAvalable,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetAvailableTableData(DataTableViewModel tableData, VillaClientDemandViewModel demandVM)
        {

            // Getting all entity data
            List<VillsAvailableViewModel> clientSales = (demandVM != null) ? Mapper.Map<List<VillasAvailableDto>, List<VillsAvailableViewModel>>(await _demandService.GetAvailableMatchesOnTheFly(Mapper.Map<VillaClientDemandViewModel, VillasDemandDto>(demandVM))) : new List<VillsAvailableViewModel>();

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

            tableData.VillasAvalable = clientSales;

            return tableData;
        }

        public async Task<ActionResult> ClientAutoComplete(string text)
        {
            List<ClientsViewModel> clients = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>(await _clientService.ClientAutoComplete(text));
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
        //ApartmentAvailables/GetClientdetails
        public async Task<ActionResult> GetClientdetails(string id)
        {
            int clientId = int.Parse(id);
            ClientsViewModel clientVM = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(clientId));
            ViewBag.RedirectTo = HttpContext.Items["RedirectUrl"]?.ToString();
            return Json(new { clientList = clientVM }, JsonRequestBehavior.AllowGet);
        }
    }
}