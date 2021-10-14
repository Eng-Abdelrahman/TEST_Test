using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3aqarak.MVC.ViewModels;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Dto;
using AutoMapper;
using System.Threading.Tasks;
using _3aqarak.MVC.Helpers;
using _3aqarak.MVC.Filters;
using _3aqarak.MVC.Hubs;

namespace _3aqarak.MVC.Controllers
{
    public class AvailableLandsController : Controller
    {
        private readonly IAvailableLandsSevice _availableLandsSevice;
        private readonly ILandsDemandsService _landDemandService;
        private readonly IClientService _clientService;
        private IConfirmation _conf;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;
        public AvailableLandsController(IAvailableLandsSevice availableLandsSevice, ILandsDemandsService landDemandService, IConfirmation conf, IClientService clientService, IUSerService userService, ISpecialService specialService, ITransService transService)
        {
            _landDemandService = landDemandService;
            _availableLandsSevice = availableLandsSevice;
            _clientService = clientService;
            _conf = conf;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
        }
        // GET: AvailableLands
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AddAvailableLands()
        {
            int catId = Categories.Lands;
            var availableLandVM = new AvailableLandsViewModel()
            {
                Regions = await _availableLandsSevice.GetRegions(),
                Payments =await  _availableLandsSevice.GetPayments(),
                Views = await _availableLandsSevice.GetViews(),
                Transactions =await _availableLandsSevice.GetTransactions()
            };
            return View(availableLandVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAvailableLands(AvailableLandsViewModel availableLandsViewModel)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            List<string> images = new List<string>();
            var ClientId = availableLandsViewModel.FK_AvaliableLands_Clients_ClientId;
            AvailableLandsDto availableLandsDto = Mapper.Map<AvailableLandsViewModel, AvailableLandsDto>(availableLandsViewModel);

            if (ModelState.IsValid)
            {
                if (ClientId == 0)
                {
                    ClientDto clientDto = Mapper.Map<AvailableLandsDto, ClientDto>(availableLandsDto);
                    clientDto.Address = availableLandsDto.ClientAddress;
                    clientDto.Notes= availableLandsDto.ClientNotes;
                    _conf = await _clientService.SaveClient(clientDto, userId);
                    if (_conf.Valid && _conf.Id > 0)
                    {
                        ClientId = _conf.Id;
                        availableLandsDto.FK_AvaliableLands_Clients_ClientId = _conf.Id;
                        if (Session["AvailableLandsImagePathList"] != null)
                        {
                            images = (List<string>)Session["AvailableLandsImagePathList"];
                        }
                        availableLandsDto.FK_AvailableLands_Categories_CategoryId = Categories.Lands;
                        _conf = await _availableLandsSevice.CheckDuplicateLandSales(availableLandsDto);
                        if (!_conf.Valid)
                        {
                            return Json(new { message = "هذا العميل لدية نفس العرض", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
                        }
                        _conf = await _availableLandsSevice.SaveLandsSale(availableLandsDto, userId, images, branchId);
                    }
                }
                else
                {
                    availableLandsDto.FK_AvaliableLands_Clients_ClientId = ClientId;
                    availableLandsDto.FK_AvailableLands_Categories_CategoryId = Categories.Lands;
                    _conf =await  _availableLandsSevice.CheckDuplicateLandSales(availableLandsDto);
                    if (!_conf.Valid)
                    {
                        return Json(new { message = "هذا العميل لدية نفس العرض", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
                    }
                    _conf = await _availableLandsSevice.SaveLandsSale(availableLandsDto, userId, images, branchId);
                }
                if (_conf.Valid)
                {
                    LandDemandMatchViewModel landDemandMatchVM = new LandDemandMatchViewModel()
                    {
                        Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(ClientId)),
                        Landsavailable = Mapper.Map<AvailableLandsDto, AvailableLandsViewModel>(_conf.LandAvailables.FirstOrDefault()),
                        LandDemands = Mapper.Map<List<LandsDemandsDto>, List<LandsDemandsViewModel>>(_conf.LandsDemandsAndExcluded.Item1),
                        DemandsWithSameClient = Mapper.Map<LandsDemandsDto, LandsDemandsViewModel>(_conf.LandsDemandsAndExcluded.Item2),
                        LandDemandsWithPreviews = Mapper.Map<List<LandsDemandsDto>, List<LandsDemandsViewModel>>(_conf.LandsDemandsAndExcluded.Item3 /*,landDemandMatchVM.Landsavailable.PK_AvailableLands_Id*/)
                    };
                    Session["AddLandDemandMatches"] = landDemandMatchVM;
                    NotificationHub.showLandDemandmatchedNotifications(_conf.LandsDemandsAndExcluded.Item4.Select(item=>new MatchedDemandsHelper {PK_LandsDemands_Id= item.PK_LandsDemands_Id,FK_LandsDemands_Users_CreatedBy= item.FK_LandsDemands_Users_CreatedBy }).ToList(),landDemandMatchVM.Landsavailable.PK_AvailableLands_Id);
                    return Json(new { message = "تم حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);

                }
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetListOfLandDemands(AvailableLandsViewModel availableLandsViewModel)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            (List<LandsDemandsDto>, List<LandsDemandsDto>) landsDemandsDtos = await _availableLandsSevice.GetAvailableLandMatches(Mapper.Map<AvailableLandsViewModel, AvailableLandsDto>(availableLandsViewModel), userId);
            List<LandsDemandsViewModel> landsDemandsVM = Mapper.Map<List<LandsDemandsDto>, List<LandsDemandsViewModel>>(landsDemandsDtos.Item1);
            foreach (var item in landsDemandsVM)
            {
                item.BuyerName = Mapper.Map<ClientDto, ClientsViewModel>( await _clientService.FindClientByID(item.FK_LandsDemands_Clients_ClientId)).Name;
            }
            return Json(new { landsDemandList = landsDemandsVM }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UploadImages()
        {
            try
            {
                // Get all files from Request object
                HttpFileCollectionBase files = Request.Files;

                //HttpPostedFileBase file = files[i];
                _conf = _availableLandsSevice.SavePhotos(files);
                Session["AvailableLandsImagePathList"] = _conf.UrlList;
            }
            catch (Exception)
            {
                _conf.Valid = false;
                _conf.Message = "حدث خطا ما عند حفظ الصور!";
                Session["AvailableLandsImagePathList"] = _conf.UrlList;
            }
            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LandDemandMatchesAfterAdd()
        {
            var LandMatches = (LandDemandMatchViewModel)Session["AddLandDemandMatches"];
            return View(LandMatches);
        }
        //AvailableLands/AvailableLandDetails
        public async Task<ActionResult> AvailableLandDetails(int availableLandId, int landdemandid = 0)
        {
            AvailableLandsViewModel availableLandsViewModel = Mapper.Map<AvailableLandsDto, AvailableLandsViewModel>(await _availableLandsSevice.EditAvailableLand(availableLandId));
            ViewBag.demandId = landdemandid;
            availableLandsViewModel.Regions =await _availableLandsSevice.GetRegionById(availableLandsViewModel.FK_AvailabeLands_Regions_RegionId);
            availableLandsViewModel.Payments = await  _availableLandsSevice.GetPaymentsId(availableLandsViewModel.FK_AvailableLands_PaymentMethod_Id);
            availableLandsViewModel.Views = await _availableLandsSevice.GetViewsById(availableLandsViewModel.FK_AvailableLands_Views_ViewId);
            availableLandsViewModel.Transactions = await _availableLandsSevice.GetTransById(availableLandsViewModel.FK_AvaliableLands_Transactions_TransactionId, null);

            return PartialView("_AvailableLandDetails", availableLandsViewModel);
        }
        //AvailableLands/DeleteAvailableLand
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAvailableLand(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            _conf = await _availableLandsSevice.DeleteLandAvailable(int.Parse(id), userId);
            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);
        }
        //AvailableLands/EditAvailableLand
        [DisableHtmlInputs]
        public async Task<ActionResult> EditAvailableLand(string id)
        {
            if (HttpContext.Items["Disable"] != null && bool.Parse(HttpContext.Items["Disable"].ToString()))
            {
                ViewBag.Disable = true;
            }

            var AvailableLand = Mapper.Map<AvailableLandsDto, LandAvailableForUpdateViewModel>(await _availableLandsSevice.EditAvailableLand(int.Parse(id)));
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            AvailableLand.Regions =await  _availableLandsSevice.GetRegionById(AvailableLand.FK_AvailabeLands_Regions_RegionId);
            AvailableLand.Payments = await _availableLandsSevice.GetPaymentsId(AvailableLand.FK_AvailableLands_PaymentMethod_Id);
            AvailableLand.Views = await _availableLandsSevice.GetViewsById(AvailableLand.FK_AvailableLands_Views_ViewId);
            var sales = (await _userService.FindUserByID(AvailableLand.FK_AvaliableLands_Users_SalesId));
            ViewBag.salesName = sales.FirstName + " " + sales.LastName;
            var userSpecialization =(await _userService.FindUserByID(userId)).Specialization_Id;
            if (userSpecialization.HasValue)
            {
                var specialName = (await _specialService.FindByID(userSpecialization.Value)).Name;
                if (specialName == "بيع")
                {
                    AvailableLand.Transactions = await _availableLandsSevice.GetTrans(specialName);

                }
                else if (specialName == "ايجار")
                {
                    AvailableLand.Transactions = await _availableLandsSevice.GetTrans(specialName);
                }
            }
            else
            {
                AvailableLand.Transactions = await _availableLandsSevice.GetTrans(null);
            }
            AvailableLand.TransType =(await _transService.FindByID(AvailableLand.FK_AvaliableLands_Transactions_TransactionId)).TransType;
            return View(AvailableLand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateAvailableLand(LandAvailableForUpdateViewModel availableLandVM)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            List<string> images = new List<string>();
            if (ModelState.IsValid)
            {
                if (Session["AvailableLandsImagePathList"] != null)
                {
                    images = (List<string>)Session["AvailableLandsImagePathList"];
                    Session["AvailableLandsImagePathList"] = null;
                }
                _conf = await _availableLandsSevice.UpdateAvailableLand(Mapper.Map<LandAvailableForUpdateViewModel, AvailableLandsDto>(availableLandVM), userId, images, branchId);
            }
            if (_conf.Valid)
            {
                LandDemandMatchViewModel landDemandMatchViewModel = new LandDemandMatchViewModel
                {
                    Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(availableLandVM.FK_AvaliableLands_Clients_ClientId)),
                    LandDemands = Mapper.Map<List<LandsDemandsDto>, List<LandsDemandsViewModel>>(_conf.LandsDemandsAndExcluded.Item1),
                    DemandsWithSameClient = Mapper.Map<LandsDemandsDto, LandsDemandsViewModel>(_conf.LandsDemandsAndExcluded.Item2),
                    LandDemandsWithPreviews = Mapper.Map<List<LandsDemandsDto>, List<LandsDemandsViewModel>>(_conf.LandsDemandsAndExcluded.Item3),
                    Landsavailable = Mapper.Map<AvailableLandsDto, AvailableLandsViewModel>(_conf.LandAvailables.FirstOrDefault())
                };
                Session["UpdateLandDemandMatches"] = landDemandMatchViewModel;
                NotificationHub.showLandDemandmatchedNotifications(_conf.LandsDemandsAndExcluded.Item4.Select(item => new MatchedDemandsHelper { PK_LandsDemands_Id = item.PK_LandsDemands_Id, FK_LandsDemands_Users_CreatedBy = item.FK_LandsDemands_Users_CreatedBy }).ToList(), landDemandMatchViewModel.Landsavailable.PK_AvailableLands_Id);
                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LandDemandMatchesAfterUpdate()
        {
            var landMatches = (LandDemandMatchViewModel)Session["UpdateLandDemandMatches"];
            return View(landMatches);
        }

        //Welcome in my part this's Mostafa's part and this's part for load data in Land Demand Quick search 
        //AvailableLands/LoadDemandsData
        public async Task<ActionResult> LoadDemandData()
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
            DataTableViewModel tableData = await GetVillDemandTableData(data, id);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.LandsDemand,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetVillDemandTableData(DataTableViewModel tableData, int id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            //Getting all entity data
            List<LandsDemandsViewModel> clientDemands = Mapper.Map<List<LandsDemandsDto>, List<LandsDemandsViewModel>>((await _landDemandService.LandDemands(id)).Where(a => a.FK_LandsDemands_Users_CreatedBy == userId).ToList());
            foreach (var item in clientDemands)
            {
                item.BuyerName =(await _clientService.FindClientByID(item.FK_LandsDemands_Clients_ClientId)).Name;
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

            tableData.LandsDemand = clientDemands;

            return tableData;
        }

        //AvailableLands/GetInstantMatches
        public async Task<ActionResult> GetInstantMatches(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            _conf = await _availableLandsSevice.GetInstantMatches(int.Parse(id), userId);
            AvailableLandMatcheViewModel matchVM = new AvailableLandMatcheViewModel
            {
                Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(_conf.VillAvailables.FirstOrDefault().FK_VillasAvailables_Clients_ClientId)),
                Demands = Mapper.Map<List<LandsDemandsDto>, List<LandsDemandsViewModel>>(_conf.LandsDemandsAndExcluded.Item1),
                DemandsWithSameClient = Mapper.Map<LandsDemandsDto, LandsDemandsViewModel>(_conf.LandsDemandsAndExcluded.Item2),
                DemandsWithPreviews = Mapper.Map<List<LandsDemandsDto>, List<LandsDemandsViewModel>>(_conf.LandsDemandsAndExcluded.Item3),
                availableLand = Mapper.Map<AvailableLandsDto, AvailableLandsViewModel>(_conf.LandAvailables.FirstOrDefault())
            };
            NotificationHub.showLandDemandmatchedNotifications(_conf.LandsDemandsAndExcluded.Item4.Select(item => new MatchedDemandsHelper { PK_LandsDemands_Id = item.PK_LandsDemands_Id, FK_LandsDemands_Users_CreatedBy = item.FK_LandsDemands_Users_CreatedBy }).ToList(), matchVM.availableLand.PK_AvailableLands_Id);
            return View("LandDemandMatchesAfterAdd", matchVM);
        }
    }
}