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
    public class ShopDemandController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IShopDemandService _demandService;
        //don't forget change here from Villas to Shop
        private readonly IShopAvailableService _availableService;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;
        private IConfirmation _conf;

        public ShopDemandController(ITransService transService, IClientService clientService, ISpecialService specialService, IShopDemandService demandService, IConfirmation conf, IShopAvailableService availableService, IUSerService userService)
        {
            _clientService = clientService;
            _demandService = demandService;
            _conf = conf;
            _availableService = availableService;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
        }
        // GET: ShopDemand/AddShopDemand
        public async Task<ActionResult> AddShopDemand()
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            ShopDemandViewModel clientDemand = new ShopDemandViewModel()
            {
                RegionsFrom = await _demandService.GetRegions(),
                RegionsTo = await _demandService.GetRegions(),
                Usages = await _demandService.GetUsages(),
                Accessories = await _demandService.GetAccess(),
                Views = await _demandService.GetViews(),
                Payments = await _demandService.GetPayments(),
                CreatedAt = DateTime.UtcNow.AddMinutes(120),
                FK_ShopDemands_Categories_Id = Categories.Shops,
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
        //ShopDemand/SaveDemand
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveDemand(ShopDemandViewModel clientDemandVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            int ClientId = clientDemandVM.PK_Client_Id;
            if (ModelState.IsValid)
            {
                ShopDemandDto clientDemandDto = Mapper.Map<ShopDemandViewModel, ShopDemandDto>(clientDemandVM);
                if (ClientId == 0)
                {
                    ClientDto clientDto = Mapper.Map<ShopDemandDto, ClientDto>(clientDemandDto);
                    _conf = await _clientService.SaveClient(clientDto, userId);
                    if (_conf.Valid && _conf.Id > 0)
                    {
                        ClientId = _conf.Id;
                        clientDemandDto.FK_ShopDemands_Clients_ClientId = _conf.Id;
                        _conf = await _demandService.SaveShopDemand(clientDemandDto, userId, branchId);
                    }
                }
                else
                {
                    //var demandDto = Mapper.Map<ShopDemandDto, ShopDemandDto>(clientDemandDto);
                    clientDemandDto.FK_ShopDemands_Clients_ClientId = ClientId;
                    _conf = await _demandService.SaveShopDemand(clientDemandDto, userId, branchId);
                }
                if (_conf.Valid)
                {
                    //*** look here we need to change this to Shop Available Demand *****************************************
                    ShopAvailableDemandViewModel matchVM = new ShopAvailableDemandViewModel
                    {
                        Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(ClientId)),
                        Demand = Mapper.Map<ShopDemandDto, ShopDemandViewModel>(_conf.ShopDemands.FirstOrDefault()),
                        availables = Mapper.Map<List<ShopAvailableDto>, List<ShopAvailableViewModel>>(_conf.ShopAvailableAndExcluded.Item1),
                        SameClientAvailable = Mapper.Map<ShopAvailableDto, ShopAvailableViewModel>(_conf.ShopAvailableAndExcluded.Item2),
                        ExcludedAvailablesForPreviws = Mapper.Map<List<ShopAvailableDto>, List<ShopAvailableViewModel>>(_conf.ShopAvailableAndExcluded.Item3),
                    };
                    Session["AddShopAvailableMatches"] = matchVM;
                    return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        //ShopDemand/AvailableMatchesAfterUpdate

        public ActionResult AvailableMatchesAfterUpdate()
        {
            ShopAvailableDemandViewModel matches = (ShopAvailableDemandViewModel)Session["UpdateShopAvailableMatches"];
            return View(matches);
        }
        //ShopDemand/AvailableMatchesAfterAdd

        public ActionResult AvailableMatchesAfterAdd()
        {
            ShopAvailableDemandViewModel matches = (ShopAvailableDemandViewModel)Session["AddShopAvailableMatches"];
            return View(matches);
        }

        //ShopDemand/EditClientDemand
        public async Task<ActionResult> EditClientDemand(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            ShopDemandForUpdateViewModel ClientDemand = Mapper.Map<ShopDemandDto, ShopDemandForUpdateViewModel>(await _demandService.EditShopDemand(int.Parse(id)));
            ClientDemand.RegionsFrom = await _demandService.GetRegionById(ClientDemand.FK_ShopDemands_Regions_FromId);
            ClientDemand.RegionsTo = await _demandService.GetRegionById(ClientDemand.FK_ShopDemands_Regions_ToId);
            ClientDemand.Accessories = await _demandService.GetAccessById(ClientDemand.AccessoriesIds);
            ClientDemand.Views = await _demandService.GetViewsById(ClientDemand.ViewsIds);
            ClientDemand.Payments = await _demandService.GetPaymentsId(ClientDemand.FK_ShopDemands_PaymentMethod_Id);
            ClientDemand.FK_ShopDemands_Categories_Id = Categories.Shops;
            UserDto sales = (await _userService.FindUserByID(ClientDemand.FK_ShopDemands_Users_SalesId));
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
            ClientDemand.TransType = (await _transService.FindByID(ClientDemand.FK_ShopDemands_Transactions_Id)).TransType;
            ClientDemand.Usages = await _demandService.GetUsagesId(ClientDemand.FK_ShopDemands_Usage_Id);
            return View(ClientDemand);
        }
        //ShopDemand/UpdateClientDemand
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateClientDemand(ShopDemandForUpdateViewModel demandVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            
            if (ModelState.IsValid)
            {
                _conf = await _demandService.UpdateShopDemand(Mapper.Map<ShopDemandForUpdateViewModel, ShopDemandDto>(demandVM), userId, branchId);
            }
            if (_conf.Valid)
            {
                ShopAvailableDemandViewModel matchVM = new ShopAvailableDemandViewModel
                {
                    //Buyer = Mapper.Map<ClientDto, ClientsViewModel>(_clientService.FindClientByID(_conf.Id)),
                    Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(demandVM.FK_ShopDemands_Clients_ClientId)),
                    Demand = Mapper.Map<ShopDemandDto, ShopDemandViewModel>(_conf.ShopDemands.FirstOrDefault()),
                    availables = Mapper.Map<List<ShopAvailableDto>, List<ShopAvailableViewModel>>(_conf.ShopAvailableAndExcluded.Item1),
                    SameClientAvailable = Mapper.Map<ShopAvailableDto, ShopAvailableViewModel>(_conf.ShopAvailableAndExcluded.Item2),
                    ExcludedAvailablesForPreviws = Mapper.Map<List<ShopAvailableDto>, List<ShopAvailableViewModel>>(_conf.ShopAvailableAndExcluded.Item3),
                };
                Session["UpdateShopAvailableMatches"] = matchVM;
                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        //ShopDemand/DeleteClientDemand
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteClientDemand(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;

            _conf = await _demandService.DeleteShopDemand(int.Parse(id), userId);

            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);
        }
        //ShopDemand/DemandDetails
        public async Task<ActionResult> DemandDetails(int id)
        {
            ShopDemandViewModel ClientDemand = Mapper.Map<ShopDemandDto, ShopDemandViewModel>(await _demandService.EditShopDemand(id));
            ClientDemand.RegionsFrom = await _demandService.GetRegionById(ClientDemand.FK_ShopDemands_Regions_FromId);
            ClientDemand.RegionsTo = await _demandService.GetRegionById(ClientDemand.FK_ShopDemands_Regions_ToId);
            ClientDemand.Transactions = await _demandService.GetTransById(ClientDemand.FK_ShopDemands_Transactions_Id, null);
            ClientDemand.Accessories = await _demandService.GetAccessById(ClientDemand.AccessoriesIds);
            ClientDemand.Usages = await _demandService.GetUsagesId(ClientDemand.FK_ShopDemands_Usage_Id);
            ClientDemand.Views = await _demandService.GetViewsById(ClientDemand.ViewsIds);
            ClientDemand.Payments = await _demandService.GetPaymentsId(ClientDemand.FK_ShopDemands_PaymentMethod_Id);
            ClientDemand.FK_ShopDemands_Categories_Id = Categories.Shops;
            return PartialView("_ShopDemandDetails", ClientDemand);
           
        }

        //ShopDemand/ClientAutoComplete
        public async Task<ActionResult> ClientAutoComplete(string text)
        {
            List<ClientsViewModel> clients = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>(await _clientService.ClientAutoComplete(text));
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
        //ShopDemand/GetClientdetails
        public async Task<ActionResult> GetClientdetails(string id)
        {
            int clientId = int.Parse(id);
            ClientsViewModel clientVM = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(clientId));
            ViewBag.RedirectTo = HttpContext.Items["RedirectUrl"]?.ToString();
            return Json(new { clientList = clientVM }, JsonRequestBehavior.AllowGet);
        }

        //ShopDemand/SelectAvailables
        public async Task<ActionResult> SelectAvailables(string[] availableIds, string demandId, string buyerId, string[] dates)
        {
            ShopAvailableDemandViewModel matchVM = new ShopAvailableDemandViewModel();
            if (availableIds.Any() && availableIds != null)
            {
                availableIds = availableIds.FirstOrDefault().Split(',');
                foreach (string item in availableIds)
                {
                    ShopAvailableViewModel available = Mapper.Map<ShopAvailableDto, ShopAvailableViewModel>(await _availableService.EditAvailableShop(int.Parse(item)));
                    available.SellerName = (await _clientService.FindClientByID(available.FK_ShopAvailable_Clients_ClientId)).Name;
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
                matchVM.Demand = Mapper.Map<ShopDemandDto, ShopDemandViewModel>(await _demandService.EditShopDemand(int.Parse(demandId)));
            }

            if (!string.IsNullOrEmpty(buyerId))
            {
                matchVM.Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(int.Parse(buyerId)));
            }

            return View(matchVM);
        }


        //ShopDemand/LoadSalesData
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
            ShopDemandViewModel demandVM = !string.IsNullOrEmpty(Request.Form["demandVM"]) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ShopDemandViewModel>(Request.Form["demandVM"]) : null;
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
                data = tableData.ShopAvailable,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetAvailableTableData(DataTableViewModel tableData, ShopDemandViewModel demandVM)
        {

            // Getting all entity data
            List<ShopAvailableViewModel> clientSales = (demandVM != null) ? Mapper.Map<List<ShopAvailableDto>, List<ShopAvailableViewModel>>(await _demandService.GetAvailableMatchesOnTheFly(Mapper.Map<ShopDemandViewModel, ShopDemandDto>(demandVM))) : new List<ShopAvailableViewModel>();

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

            tableData.ShopAvailable = clientSales;

            return tableData;
        }

        //ShopDemand/GetInstantMatches
        public async Task<ActionResult> GetInstantMatches(string id)
        {
            _conf = await _demandService.GetInstantMatches(int.Parse(id));
            ShopAvailableDemandViewModel matchVM = new ShopAvailableDemandViewModel
            {
                Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(_conf.ShopDemands.FirstOrDefault().FK_ShopDemands_Clients_ClientId)),
                Demand = Mapper.Map<ShopDemandDto, ShopDemandViewModel>(_conf.ShopDemands.FirstOrDefault()),
                availables = Mapper.Map<List<ShopAvailableDto>, List<ShopAvailableViewModel>>(_conf.ShopAvailableAndExcluded.Item1),
                SameClientAvailable = Mapper.Map<ShopAvailableDto, ShopAvailableViewModel>(_conf.ShopAvailableAndExcluded.Item2),
                ExcludedAvailablesForPreviws = Mapper.Map<List<ShopAvailableDto>, List<ShopAvailableViewModel>>(_conf.ShopAvailableAndExcluded.Item3),
            };
            return View("AvailableMatchesAfterAdd", matchVM);
        }


    }
}