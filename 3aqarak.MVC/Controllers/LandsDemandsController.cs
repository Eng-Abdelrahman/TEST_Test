using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Helpers;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
namespace _3aqarak.MVC.Controllers
{
    public class LandsDemandsController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ILandsDemandsService _landsDemandsService;
        private readonly IAvailableLandsSevice _availableLandsSevice;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;

        private IConfirmation _conf;

        public LandsDemandsController(IClientService clientService, IConfirmation conf, ILandsDemandsService landsDemandsService, IAvailableLandsSevice availableLandsSevice, IUSerService userService, ISpecialService specialService, ITransService transService)
        {
            _clientService = clientService;
            _landsDemandsService = landsDemandsService;
            _conf = conf;
            _availableLandsSevice = availableLandsSevice;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
        }
        // GET: LandsDemands
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AddLandDemand()
        {
            int catId = Categories.Lands;
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            LandsDemandsViewModel landDemandMatchVM = new LandsDemandsViewModel
            {
                RegionsFrom = await _landsDemandsService.GetRegions(),
                RegionsTo = await _landsDemandsService.GetRegions(),
                Payments =await  _landsDemandsService.GetPayments(),
                Transactions = await _landsDemandsService.GetTransactions(),
                Views = await _landsDemandsService.GetViews(),
                FK_LandsDemands_Categories_Id = catId,
                CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
            };
            return View(landDemandMatchVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveLandDemand(LandsDemandsViewModel landsDemandsViewModel)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            var ClientId = landsDemandsViewModel.PK_Client_Id;
            LandsDemandsDto landsDemandsDto = Mapper.Map<LandsDemandsViewModel, LandsDemandsDto>(landsDemandsViewModel);
            if (ModelState.IsValid)
            {
                var clientDto = Mapper.Map<LandsDemandsViewModel, ClientDto>(landsDemandsViewModel);
                if (ClientId == 0)
                {
                    _conf = await _clientService.SaveClient(clientDto, userId);
                    if (_conf.Valid && _conf.Id > 0)
                    {
                        ClientId = _conf.Id;
                        landsDemandsDto.FK_LandsDemands_Clients_ClientId = _conf.Id;
                        landsDemandsDto.FK_LandsDemands_Categories_Id = Categories.Lands;
                        _conf = await _landsDemandsService.SaveLandsDemand(landsDemandsDto, userId, branchId);
                    }
                }
                else
                {
                    landsDemandsDto.FK_LandsDemands_Clients_ClientId = ClientId;
                    landsDemandsDto.FK_LandsDemands_Categories_Id = Categories.Lands;
                    _conf = await _landsDemandsService.SaveLandsDemand(landsDemandsDto, userId, branchId);
                }
                if (_conf.Valid)
                {
                    AvailableLandMatcheViewModel matchVM = new AvailableLandMatcheViewModel
                    {
                        Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(ClientId)),
                        landDemand = Mapper.Map<LandsDemandsDto, LandsDemandsViewModel>(_conf.landsDemands.FirstOrDefault()),
                        availableLands = Mapper.Map<List<AvailableLandsDto>, List<AvailableLandsViewModel>>(_conf.LandsAvailableAndExcluded.Item1),
                        availableLandWithSameClient = Mapper.Map<AvailableLandsDto, AvailableLandsViewModel>(_conf.LandsAvailableAndExcluded.Item2),
                        excludedAvailablesLandForPreviews = Mapper.Map<List<AvailableLandsDto>, List<AvailableLandsViewModel>>(_conf.LandsAvailableAndExcluded.Item3)
                    };
                    Session["AddAvailableLandMatch"] = matchVM;
                    return Json(new { message = "تم حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AvailableLandMatchesAfterAdd()
        {
            AvailableLandMatcheViewModel landDemandMatchVM = (AvailableLandMatcheViewModel)Session["AddAvailableLandMatch"];
            ViewBag.Category = Categories.Lands;
            return View(landDemandMatchVM);
        }
        //LandsDemands/EditeLandDemand

        public async Task<ActionResult> EditeLandDemand(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            LandDemandForUpdateViewModel landsDemandsViewModel = Mapper.Map<LandsDemandsDto, LandDemandForUpdateViewModel>(await _landsDemandsService.EditLandDemand(int.Parse(id)));
            landsDemandsViewModel.RegionsFrom =await _landsDemandsService.GetRegionById(landsDemandsViewModel.FK_LandsDemands_Regions_FromId);
            landsDemandsViewModel.RegionsTo = await _landsDemandsService.GetRegionById(landsDemandsViewModel.FK_LandsDemands_Regions_ToId);
            landsDemandsViewModel.Payments = await _landsDemandsService.GetPaymentsId(landsDemandsViewModel.FK_LandsDemands_PaymentMethod_Id);
            landsDemandsViewModel.Views = await _landsDemandsService.GetViewsById(landsDemandsViewModel.ViewsIds);
            UserDto sales =await  _userService.FindUserByID(landsDemandsViewModel.FK_LandsDemands_Users_SalesId);
            landsDemandsViewModel.SalseName = sales.FirstName + " " + sales.LastName;
            int? userSpecialization =(await _userService.FindUserByID(userId)).Specialization_Id;
            if (userSpecialization.HasValue)
            {
                string specialName = (await _specialService.FindByID(userSpecialization.Value)).Name;
                if (specialName == "بيع")
                {
                    landsDemandsViewModel.Transactions = await _landsDemandsService.GetTrans(specialName);

                }
                else if (specialName == "ايجار")
                {
                    landsDemandsViewModel.Transactions = await _landsDemandsService.GetTrans(specialName);
                }

            }
            else
            {
                landsDemandsViewModel.Transactions = await _landsDemandsService.GetTrans(null);
            }
            landsDemandsViewModel.TransType =(await _transService.FindByID(landsDemandsViewModel.FK_LandsDemands_Transactions_Id)).TransType;
            landsDemandsViewModel.Transactions =await _landsDemandsService.GetTransById(landsDemandsViewModel.FK_LandsDemands_Transactions_Id, landsDemandsViewModel.TransType);

            return View(landsDemandsViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateLandDemand(LandDemandForUpdateViewModel landsDemandVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            if (ModelState.IsValid)
            {
                _conf = await _landsDemandsService.UpdateClientDemand(Mapper.Map<LandDemandForUpdateViewModel, LandsDemandsDto>(landsDemandVM), userId, branchId);
            }
            if (_conf.Valid)
            {
                AvailableLandMatcheViewModel availableLandMatcheViewModel = new AvailableLandMatcheViewModel
                {
                    Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(landsDemandVM.FK_LandsDemands_Clients_ClientId)),
                    landDemand = Mapper.Map<LandsDemandsDto, LandsDemandsViewModel>(_conf.landsDemands.FirstOrDefault()),
                    availableLands = Mapper.Map<List<AvailableLandsDto>, List<AvailableLandsViewModel>>(_conf.LandsAvailableAndExcluded.Item1),
                    availableLandWithSameClient = Mapper.Map<AvailableLandsDto, AvailableLandsViewModel>(_conf.LandsAvailableAndExcluded.Item2),
                    excludedAvailablesLandForPreviews = Mapper.Map<List<AvailableLandsDto>, List<AvailableLandsViewModel>>(_conf.LandsAvailableAndExcluded.Item3)
                };
                Session["UpdateAvailableLandMatches"] = availableLandMatcheViewModel;
                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }
        //LandsDemands/LoadAvailableLandData
        public async Task<ActionResult> LoadAvailableLandData()
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
            LandsDemandsViewModel landsDemandsVM = !string.IsNullOrEmpty(Request.Form["landDemandVm"]) ? Newtonsoft.Json.JsonConvert.DeserializeObject<LandsDemandsViewModel>(Request.Form["landDemandVm"]) :null;
            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await GetAvailableTableData(data, landsDemandsVM);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.AvailableLands,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetAvailableTableData(DataTableViewModel tableData, LandsDemandsViewModel demandVM)
        {

            // Getting all entity data
            if (demandVM != null)
            {
                demandVM.FK_LandsDemands_Categories_Id = Categories.Lands;

            }
            List<AvailableLandsViewModel> availableLands = (demandVM != null) ? Mapper.Map<List<AvailableLandsDto>, List<AvailableLandsViewModel>>(await _landsDemandsService.GetLandAvailableMatchOnFly(Mapper.Map<LandsDemandsViewModel, LandsDemandsDto>(demandVM))) : new List<AvailableLandsViewModel>();

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                availableLands = availableLands.Where(e => e.Name.Contains(tableData.SearchValue)).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    availableLands = availableLands.OrderBy(e => e.Name).ToList();
                }
                else
                {
                    availableLands = availableLands.OrderByDescending(e => e.Name).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = availableLands.Count();

            //Paging
            availableLands = availableLands.Skip(tableData.Skip).Take(tableData.PageSize).ToList();
            foreach (var item in availableLands)
            {
                item.DateString = item.CreatedAt.ToShortDateString();
                item.ShortDescription = item.Description;
            }
            tableData.AvailableLands = availableLands;

            return tableData;
        }

        public async Task<ActionResult> GetMatchesOnFly(LandsDemandsViewModel landsDemandsViewModel)
        {
            List<AvailableLandsDto> availableLandsDtos = await _landsDemandsService.GetLandAvailableMatchOnFly(Mapper.Map<LandsDemandsViewModel, LandsDemandsDto>(landsDemandsViewModel));
            return Json(availableLandsDtos, JsonRequestBehavior.AllowGet);
        }
        //LandsDemands/SelectLandAvailables
        public async Task<ActionResult> SelectLandAvailables(string[] availableIds, string demandId, string buyerId, string[] dates)
        {
            AvailableLandMatcheViewModel availableLandMatcheViewModel = new AvailableLandMatcheViewModel();
            if (availableIds.Any())
            {
                foreach (var item in availableIds)
                {
                    AvailableLandsViewModel availableLandsViewModel = Mapper.Map<AvailableLandsDto, AvailableLandsViewModel>(await _availableLandsSevice.EditAvailableLand(int.Parse(item)));
                    availableLandsViewModel.SellerName = (await _clientService.FindClientByID(availableLandsViewModel.FK_AvaliableLands_Clients_ClientId)).Name;
                    availableLandMatcheViewModel.availableLands.Add(availableLandsViewModel);
                }
            }
            if (dates.Any() && dates != null)
            {
                dates = dates.FirstOrDefault().Split(',');
                availableLandMatcheViewModel.Dates = dates.Select(d => DateTime.Parse(d)).ToList();
            }
            if (!string.IsNullOrEmpty(demandId))
            {
                availableLandMatcheViewModel.landDemand = Mapper.Map<LandsDemandsDto, LandsDemandsViewModel>(await _landsDemandsService.EditLandDemand(int.Parse(demandId)));
            }
            if (!string.IsNullOrEmpty(buyerId))
            {
                availableLandMatcheViewModel.Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(int.Parse(buyerId)));
            }
            return View(availableLandMatcheViewModel);
        }

        public ActionResult availableLandMatchesAfterUpdate()
        {
            ViewBag.Category = Categories.Apartements;
            AvailableLandMatcheViewModel availableLandMatcheViewModel = (AvailableLandMatcheViewModel)Session["UpdateAvailableLandMatches"];
            return View(availableLandMatcheViewModel);
        }
        //LandsDemands/LandDemandDetails
        public async Task<ActionResult> LandDemandDetails(int landDemandId)
        {
            LandsDemandsViewModel landDemandMatchVM = Mapper.Map<LandsDemandsDto, LandsDemandsViewModel>(await _landsDemandsService.EditLandDemand(landDemandId));
            landDemandMatchVM.RegionsFrom = await _landsDemandsService.GetRegionById(landDemandMatchVM.FK_LandsDemands_Regions_FromId);
            landDemandMatchVM.RegionsTo = await _landsDemandsService.GetRegionById(landDemandMatchVM.FK_LandsDemands_Regions_ToId);
            landDemandMatchVM.Transactions = await _landsDemandsService.GetTransById(landDemandMatchVM.FK_LandsDemands_Transactions_Id, null);
            landDemandMatchVM.Views = await _landsDemandsService.GetViews();
            landDemandMatchVM.Payments = await _landsDemandsService.GetPaymentsId(landDemandMatchVM.FK_LandsDemands_PaymentMethod_Id);
            return PartialView("_LandDemandDetails", landDemandMatchVM);
        }
        //LandsDemands/DeleteLandDemand
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteLandDemand(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            _conf = await _landsDemandsService.DeleteLandDemand(int.Parse(id), userId);
            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SelectAvailables(string[] availableIds, string demandId, string buyerId, string[] dates)
        {
            AvailableLandMatcheViewModel matchVM = new AvailableLandMatcheViewModel();
            if (availableIds.Any() && availableIds != null)
            {
                availableIds = availableIds.FirstOrDefault().Split(',');
                foreach (string item in availableIds)
                {
                    AvailableLandsViewModel available = Mapper.Map<AvailableLandsDto, AvailableLandsViewModel>(await _availableLandsSevice.EditAvailableLand(int.Parse(item)));
                    available.SellerName = (await _clientService.FindClientByID(available.FK_AvaliableLands_Clients_ClientId)).Name;
                    matchVM.availableLands.Add(available);
                }
            }
            if (dates.Any() && dates != null)
            {
                dates = dates.FirstOrDefault().Split(',');
                matchVM.Dates = dates.Select(d => DateTime.Parse(d)).ToList();

            }

            if (!string.IsNullOrEmpty(demandId))
            {
                matchVM.landDemand = Mapper.Map<LandsDemandsDto, LandsDemandsViewModel>(await _landsDemandsService.EditLandDemand(int.Parse(demandId)));
            }

            if (!string.IsNullOrEmpty(buyerId))
            {
                matchVM.Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(int.Parse(buyerId)));
            }

            return View(matchVM);
        }

        //LandsDemands/GetInstantMatches
        public async Task<ActionResult> GetInstantMatches(string id)
        {
            _conf = await _landsDemandsService.GetInstantMatches(int.Parse(id));
   
            AvailableLandMatcheViewModel matchVM = new AvailableLandMatcheViewModel
            {
                
                Buyer = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(_conf.landsDemands.FirstOrDefault().FK_LandsDemands_Clients_ClientId)),
                landDemand = Mapper.Map<LandsDemandsDto, LandsDemandsViewModel>(_conf.landsDemands.FirstOrDefault()),
                availableLands = Mapper.Map<List<AvailableLandsDto>, List<AvailableLandsViewModel>>(_conf.LandsAvailableAndExcluded.Item1),
                availableLandWithSameClient = Mapper.Map<AvailableLandsDto, AvailableLandsViewModel>(_conf.LandsAvailableAndExcluded.Item2),
                excludedAvailablesLandForPreviews = Mapper.Map<List<AvailableLandsDto>, List<AvailableLandsViewModel>>(_conf.LandsAvailableAndExcluded.Item3),
            };
            return View("AvailableLandMatchesAfterAdd", matchVM);
        }

    }
}