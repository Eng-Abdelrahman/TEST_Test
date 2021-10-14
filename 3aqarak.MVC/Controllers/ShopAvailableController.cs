using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using System.Threading.Tasks;
using _3aqarak.MVC.Hubs;
using _3aqarak.MVC.Filters;
using _3aqarak.MVC.Helpers;

namespace _3aqarak.MVC.Controllers
{
    public class ShopAvailableController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;
        private readonly IShopDemandService _demandService;
        private readonly IShopAvailableService _shopAvailableService;
        private readonly IShopDemandService _shopDemandService;
        private IConfirmation _conf;

        public ShopAvailableController(IClientService clientService, IUSerService userService, ISpecialService specialService, ITransService transService, IShopDemandService demandService, IShopAvailableService shopAvailableService, IShopDemandService shopDemandService)
        {
            _clientService = clientService;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
            _demandService = demandService;
            _shopAvailableService = shopAvailableService;
            _shopDemandService = shopDemandService;
        }
        // GET: ShopAvailable
        public ActionResult Index()
        {
            return View();
        }
        //ShopAvailable/LoadShopDemandsData
        public async Task<ActionResult> LoadShopDemandsData()
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
            DataTableViewModel tableData = await GetShopDemandTableData(data, id);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.ShopDemand,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetShopDemandTableData(DataTableViewModel tableData, int id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            // Getting all entity data
            List<ShopDemandViewModel> ShopDemands = Mapper.Map<List<ShopDemandDto>, List<ShopDemandViewModel>>(_shopDemandService.ShopClientDemands(id).Where(a => a.FK_ShopDemands_Users_CreatedBy == userId).ToList());
            foreach (var item in ShopDemands)
            {
                item.BuyerName = (await _clientService.FindClientByID(item.FK_ShopDemands_Clients_ClientId)).Name;
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    ShopDemands = ShopDemands.OrderBy(e => e.DateString).ToList();
                }
                else
                {
                    ShopDemands = ShopDemands.OrderByDescending(e => e.DateString).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = ShopDemands.Count();

            //Paging
            ShopDemands = ShopDemands.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.ShopDemand = ShopDemands;

            return tableData;
        }

        public async Task<ActionResult> AddShopAvailable()
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            ShopAvailableViewModel shopAvailableViewModel = new ShopAvailableViewModel
            {
                Regions = await _shopAvailableService.GetRegions(),
                Usages = await _shopAvailableService.GetUsages(),
                Accessories = await _shopAvailableService.GetAccess(),
                Views = await _shopAvailableService.GetViews(),
                Payments = await _shopAvailableService.GetPayments(),
                Transactions = await _shopAvailableService.GetTransactions(),
                FK_ShopAvailable_Categories_Id = Categories.Shops,
            };
            int? userSpecialization = (await _userService.FindUserByID(userId)).Specialization_Id;
            if (userSpecialization.HasValue)
            {
                string specialName = (await _specialService.FindByID(userSpecialization.Value)).Name;
                if (specialName == "بيع")
                {
                    shopAvailableViewModel.Transactions = await _shopAvailableService.GetTrans(specialName);

                }
                else if (specialName == "ايجار")
                {
                    shopAvailableViewModel.Transactions = await _shopAvailableService.GetTrans(specialName);
                }
            }
            else
            {
                shopAvailableViewModel.Transactions = await _shopAvailableService.GetTrans(null);
            }
            return View(shopAvailableViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveShopAvailable(ShopAvailableViewModel shopAvailableVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            var ClientId = shopAvailableVM.FK_ShopAvailable_Clients_ClientId;
            List<string> images = new List<string>();
            if (ModelState.IsValid)
            {
                var shopAvailableDto = Mapper.Map<ShopAvailableViewModel, ShopAvailableDto>(shopAvailableVM);
                if (ClientId == 0)
                {
                    var clientDto = Mapper.Map<ShopAvailableDto, ClientDto>(shopAvailableDto);
                    clientDto.Address = shopAvailableVM.ClientAddress;
                    clientDto.Notes = shopAvailableVM.ClientNotes;
                    _conf = await _clientService.SaveClient(clientDto, userId);
                    if (_conf.Valid && _conf.Id > 0)
                    {
                        ClientId = _conf.Id;
                        shopAvailableDto.FK_ShopAvailable_Clients_ClientId = _conf.Id;
                        if (Session["ShopAvailableImagePathList"] != null)
                        {
                            images = (List<string>)Session["ShopAvailableImagePathList"];
                        }
                        shopAvailableDto.FK_ShopAvailable_Categories_Id = Categories.Shops;
                        _conf = await _shopAvailableService.CheckDuplicateClientShopAvailable(shopAvailableDto);
                        if (!_conf.Valid)
                        {
                            return Json(new { message = "هذا العميل لدية نفس العرض", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
                        }
                        _conf = await _shopAvailableService.SaveShopAvailable(shopAvailableDto, userId, images, branchId);
                    }
                }
                else
                {
                    shopAvailableDto.FK_ShopAvailable_Clients_ClientId = ClientId;
                    _conf = await _shopAvailableService.CheckDuplicateClientShopAvailable(shopAvailableDto);
                    if (!_conf.Valid)
                    {
                        return Json(new { message = "هذا العميل لدية نفس العرض", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
                    }
                    _conf = await _shopAvailableService.SaveShopAvailable(shopAvailableDto, userId, images, branchId);
                }
                if (_conf.Valid)
                {
                    ShopDemandsMatchesAfterAddViewModel shopDemandsMatchesAfterAddVM = new ShopDemandsMatchesAfterAddViewModel
                    {
                        Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(ClientId)),
                        ShopAvailable = Mapper.Map<ShopAvailableDto, ShopAvailableViewModel>(_conf.ShopAvailables.FirstOrDefault()),
                        ShopDemands = Mapper.Map<List<ShopDemandDto>, List<ShopDemandViewModel>>(_conf.ShopDemandsAndExcluded.Item1),
                        ShopDemandsWithPreviews = Mapper.Map<List<ShopDemandDto>, List<ShopDemandViewModel>>(_conf.ShopDemandsAndExcluded.Item3),
                        ShopDemandsWithSameClient = Mapper.Map<ShopDemandDto, ShopDemandViewModel>(_conf.ShopDemandsAndExcluded.Item2),
                    };
                    Session["AddShopDemandMatches"] = shopDemandsMatchesAfterAddVM;
                    NotificationHub.showShopDemandmatchedNotifications(_conf.ShopDemandsAndExcluded.Item4.Select(item=>new MatchedDemandsHelper {PK_ShopDemands_Id= item.PK_ShopDemands_Id,FK_ShopDemands_Users_CreatedBy=item.FK_ShopDemands_Users_CreatedBy }).ToList(), shopDemandsMatchesAfterAddVM.ShopAvailable.PK_ShopAvailable_Id);
                    return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }
        //ShopAvailable/EditShopAvailable
        [DisableHtmlInputs]
        public async Task<ActionResult> EditShopAvailable(string id)
        {
            if (HttpContext.Items["Disable"] != null && bool.Parse(HttpContext.Items["Disable"].ToString()))
            {
                ViewBag.Disable = true;
            }
            var ShopAvailable = Mapper.Map<ShopAvailableDto, ShopAvailableViewModel>(await _shopAvailableService.EditAvailableShop(int.Parse(id)));
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            ShopAvailable.Regions = await _shopAvailableService.GetRegionById(ShopAvailable.FK_ShopAvailable_Regions_Id);
            ShopAvailable.Accessories = await _shopAvailableService.GetAccessById(ShopAvailable.AccessoriesIds);
            ShopAvailable.Views = await _shopAvailableService.GetViewsById(ShopAvailable.ViewsIds);
            ShopAvailable.Payments = await _shopAvailableService.GetPaymentsId(ShopAvailable.FK_ShopAvailable_PaymentMethod_Id);
            ShopAvailable.FK_ShopAvailable_Categories_Id = Categories.Shops;
            ShopAvailable.Usages = await _shopAvailableService.GetUsagesId(ShopAvailable.FK_ShopAvailable_Usage_Id);
            var sales = (await _userService.FindUserByID(ShopAvailable.FK_ShopAvailable_Users_SalesId));
            ShopAvailable.salesName = sales.FirstName + " " + sales.LastName;
            var userSpecialization = (await _userService.FindUserByID(userId)).Specialization_Id;
            if (userSpecialization.HasValue)
            {
                var specialName =  (await _specialService.FindByID(userSpecialization.Value)).Name;
                if (specialName == "بيع")
                {
                    ShopAvailable.Transactions = await _shopAvailableService.GetTrans(specialName);

                }
                else if (specialName == "ايجار")
                {
                    ShopAvailable.Transactions = await _shopAvailableService.GetTrans(specialName);
                }
            }
            else
            {
                ShopAvailable.Transactions = await _shopAvailableService.GetTrans(null);
            }
            ShopAvailable.TransType = (await _transService.FindByID(ShopAvailable.FK_ShopAvailable_Transactions_Id)).TransType;
            return View(_shopAvailableService);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateShopAvailable(ShopAvailableViewModel shopAvailableViewModel)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            List<string> images = new List<string>();
            if (ModelState.IsValid)
            {
                if (Session["AvailableShopImagePathList"] != null)
                {
                    images = (List<string>)Session["AvailableShopImagePathList"];
                    Session["AvailableShopImagePathList"] = null;
                }
                _conf = await _shopAvailableService.UpdateAvailableLand(Mapper.Map<ShopAvailableViewModel, ShopAvailableDto>(shopAvailableViewModel), userId, images, branchId);
            }
            if (_conf.Valid)
            {
                ShopDemandsMatchesAfterAddViewModel shopDemandsMatchesAfterAddVM = new ShopDemandsMatchesAfterAddViewModel
                {
                    Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(shopAvailableViewModel.FK_ShopAvailable_Clients_ClientId)),
                    ShopAvailable = Mapper.Map<ShopAvailableDto, ShopAvailableViewModel>(_conf.ShopAvailables.FirstOrDefault()),
                    ShopDemands = Mapper.Map<List<ShopDemandDto>, List<ShopDemandViewModel>>(_conf.ShopDemandsAndExcluded.Item1),
                    ShopDemandsWithPreviews = Mapper.Map<List<ShopDemandDto>, List<ShopDemandViewModel>>(_conf.ShopDemandsAndExcluded.Item3),
                    ShopDemandsWithSameClient = Mapper.Map<ShopDemandDto, ShopDemandViewModel>(_conf.ShopDemandsAndExcluded.Item2),
                };
                Session["UpdateShopDemandMatches"] = shopDemandsMatchesAfterAddVM;
                NotificationHub.showShopDemandmatchedNotifications(_conf.ShopDemandsAndExcluded.Item4.Select(item => new MatchedDemandsHelper { PK_ShopDemands_Id = item.PK_ShopDemands_Id, FK_ShopDemands_Users_CreatedBy = item.FK_ShopDemands_Users_CreatedBy }).ToList(), shopDemandsMatchesAfterAddVM.ShopAvailable.PK_ShopAvailable_Id);
                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShopDemandsMatchesAfterUpdate()
        {
            var matchVM = (ShopDemandsMatchesAfterAddViewModel)Session["UpdateShopDemandMatches"];
            return View(matchVM);
        }
        public ActionResult ShopDemandsMatchesAfterAdd()
        {
            var matchVM = (ShopDemandsMatchesAfterAddViewModel)Session["AddShopDemandMatches"];
            return View(matchVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAvailableShop(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            _conf = await _shopAvailableService.DeleteShopAvailable(int.Parse(id), userId);
            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);
        }
        //ShopAvailable/AvailableShopDetails
        public async Task<ActionResult> AvailableShopDetails(int availableShopId, int shopDemandId = 0)
        {
            var availableShopVM = Mapper.Map<ShopAvailableDto, ShopAvailableViewModel>(await _shopAvailableService.EditAvailableShop(availableShopId));
            ViewBag.shopDemandId = shopDemandId;
            availableShopVM.Regions = await _shopAvailableService.GetRegionById(availableShopVM.FK_ShopAvailable_Regions_Id);
            availableShopVM.Views = await _shopAvailableService.GetViewsById(availableShopVM.AccessoriesIds);
            availableShopVM.Transactions = await _shopAvailableService.GetTransById(availableShopVM.FK_ShopAvailable_Transactions_Id, null);
            availableShopVM.Payments = await _shopAvailableService.GetPaymentsId(availableShopVM.FK_ShopAvailable_PaymentMethod_Id);
            return PartialView("_AvailableShopDetails", availableShopVM);
        }

        [HttpPost]
        public ActionResult UploadImages()
        {
            try
            {
                // Get all files from Request object
                HttpFileCollectionBase files = Request.Files;

                //HttpPostedFileBase file = files[i];
                _conf = _shopAvailableService.SavePhotos(files);
                Session["AvailableShopImagePathList"] = _conf.UrlList;
            }
            catch (Exception)
            {
                _conf.Valid = false;
                _conf.Message = "حدث خطا ما عند حفظ الصور!";
                Session["AvailableShopImagePathList"] = _conf.UrlList;
            }
            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);
        }

        //
        public async Task<ActionResult> LoadShopDemandMatchesData()
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
            ShopAvailableViewModel demandVM = !string.IsNullOrEmpty(Request.Form["ShopAvailableVM"]) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ShopAvailableViewModel>(Request.Form["ShopAvailableVM"]) : null;
            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await GetAvailableTableDataAsync(data, demandVM);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.ClientSales,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetAvailableTableDataAsync(DataTableViewModel tableData, ShopAvailableViewModel shopAvailableViewModel)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            // Getting all entity data
            List<ShopDemandViewModel> shopDemands = (shopAvailableViewModel != null) ? Mapper.Map<List<ShopDemandDto>, List<ShopDemandViewModel>>((await _shopAvailableService.GetShopAvailableMatches(Mapper.Map<ShopAvailableViewModel, ShopAvailableDto>(shopAvailableViewModel), userId)).Item1) : new List<ShopDemandViewModel>();
            foreach (var item in shopDemands)
            {
                item.DateString = item.CreatedAt.ToShortDateString();
                item.ShortDescription = await _shopDemandService.GetShortDescAndDate(Mapper.Map<ShopDemandViewModel, ShopDemandDto>(item));
            }
            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                shopDemands = shopDemands.Where(e => e.DateString.Contains(tableData.SearchValue)).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    shopDemands = shopDemands.OrderBy(e => e.DateString).ToList();
                }
                else
                {
                    shopDemands = shopDemands.OrderByDescending(e => e.DateString).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = shopDemands.Count();

            //Paging
            shopDemands = shopDemands.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.ShopDemand = shopDemands;

            return tableData;
        }




        public async Task<ActionResult> GetInstantMatches(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            _conf = await _shopAvailableService.GetInstantMatches(int.Parse(id), userId);
            var matchVM = new ShopDemandsMatchesAfterAddViewModel();
            matchVM.Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(_conf.ShopAvailables.FirstOrDefault().FK_ShopAvailable_Clients_ClientId));
            matchVM.ShopDemands = Mapper.Map<List<ShopDemandDto>, List<ShopDemandViewModel>>(_conf.ShopDemandsAndExcluded.Item1);
            matchVM.ShopDemandsWithSameClient = Mapper.Map<ShopDemandDto, ShopDemandViewModel>(_conf.ShopDemandsAndExcluded.Item2);
            matchVM.ShopDemandsWithPreviews = Mapper.Map<List<ShopDemandDto>, List<ShopDemandViewModel>>(_conf.ShopDemandsAndExcluded.Item3);
            matchVM.ShopAvailable = Mapper.Map<ShopAvailableDto, ShopAvailableViewModel>(_conf.ShopAvailables.FirstOrDefault());
            NotificationHub.showShopDemandmatchedNotifications(_conf.ShopDemandsAndExcluded.Item4.Select(item => new MatchedDemandsHelper { PK_ShopDemands_Id = item.PK_ShopDemands_Id, FK_ShopDemands_Users_CreatedBy = item.FK_ShopDemands_Users_CreatedBy }).ToList(), matchVM.ShopAvailable.PK_ShopAvailable_Id);
            return View("DemandMatchesAfterAdd", matchVM);
        }
    }
}