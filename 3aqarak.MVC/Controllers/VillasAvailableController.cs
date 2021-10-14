using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Filters;
using _3aqarak.MVC.Helpers;
using _3aqarak.MVC.Hubs;
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
    public class VillasAvailableController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IVillasAvailablesService _villasAvailableService;
        private readonly IVillasDemandService _villasDemandService;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;
        private readonly IFinishService _finishService;
        private readonly IRegionService _regionService;
        private IConfirmation _conf;

        public VillasAvailableController(IClientService clientService, ISpecialService specialService, IVillasDemandService VillasDemand, IVillasAvailablesService availableService, IConfirmation conf, IUSerService userService, ITransService transService, IFinishService finishService, IRegionService regionService)
        {
            _clientService = clientService;
            _villasAvailableService = availableService;
            _villasDemandService = VillasDemand;
            _conf = conf;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
            _finishService = finishService;
            _regionService = regionService;

        }
        // GET: VillasAvailable
        public async Task<ActionResult> AddVillasClientSalse()
        {
            int catId = Categories.Villas;
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            MixedVillasAvailableViewModel clientSale = new MixedVillasAvailableViewModel()
            {
                Regions = await _villasAvailableService.GetRegions(),
                Categories = await _villasAvailableService.GetCats(),
                Finishings = await _villasAvailableService.GetFinishings(),
                Accessories = await _villasAvailableService.GetAccess(),
                Views = await _villasAvailableService.GetViews(),
                Payments = await _villasAvailableService.GetPayments(),
                CreatedAt = DateTime.UtcNow.AddMinutes(120),
                Usages = await _villasAvailableService.GetUsages(),
            };
            clientSale.FK_VillasAvailables_Categories_Id = catId;
            int? userSpecialization = (await _userService.FindUserByID(userId)).Specialization_Id;
            if (userSpecialization.HasValue)
            {
                string specialName = (await _specialService.FindByID(userSpecialization.Value)).Name;
                if (specialName == "بيع")
                {
                    clientSale.Transactions = await _villasAvailableService.GetTrans(specialName);

                }
                else if (specialName == "ايجار")
                {
                    clientSale.Transactions = await _villasAvailableService.GetTrans(specialName);
                }
            }
            else
            {
                clientSale.Transactions = await _villasAvailableService.GetTrans(null);
            }
            return View(clientSale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveClientSale(MixedVillasAvailableViewModel availableClientVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            List<string> images = new List<string>();
            int ClientId = availableClientVM.FK_VillasAvailables_Clients_ClientId;
            
            if (ModelState.IsValid)
            {
                if (Session["VillasClientSalesImagePathList"] != null)
                {
                    images = (List<string>)Session["VillasClientSalesImagePathList"];
                }
                MixedVillasAvailableDto clientAvailableDto = Mapper.Map<MixedVillasAvailableViewModel, MixedVillasAvailableDto>(availableClientVM);
                if (ClientId == 0)
                {
                    ClientDto clientDto = Mapper.Map<MixedVillasAvailableDto, ClientDto>(clientAvailableDto);
                    //IConfirmation savedClient = await _clientService.SaveClient(clientDto, userId);
                    _conf = await _clientService.SaveClient(clientDto, userId);
                    if (_conf.Valid && _conf.Id > 0)
                    {
                        ClientId = _conf.Id;
                        clientAvailableDto.FK_VillasAvailables_Clients_ClientId = ClientId;
                        VillasAvailableDto availableDto = Mapper.Map<MixedVillasAvailableDto, VillasAvailableDto>(clientAvailableDto);
                        _conf = await _villasAvailableService.CheckDuplicateClientSales(availableDto);
                        if (!_conf.Valid)
                        {
                            return Json(new { message = "هذا العميل لدية نفس العرض", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
                        }
                        _conf = await _villasAvailableService.SaveClientSale(availableDto, userId, images, branchId);
                    }

                }
                else
                {
                    clientAvailableDto.FK_VillasAvailables_Clients_ClientId = ClientId;
                    VillasAvailableDto availableDto = Mapper.Map<MixedVillasAvailableDto, VillasAvailableDto>(clientAvailableDto);
                    _conf = await _villasAvailableService.CheckDuplicateClientSales(availableDto);
                    if (!_conf.Valid)
                    {
                        return Json(new { message = "هذا العميل لدية نفس العرض", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
                    }
                    _conf = await _villasAvailableService.SaveClientSale(availableDto, userId, images, branchId);
                }
                if (_conf.Valid)
                {
                    VillasAvailableDemandViewModel matchVM = new VillasAvailableDemandViewModel
                    {
                        Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(ClientId)),
                        available = Mapper.Map<VillasAvailableDto, VillsAvailableViewModel>(_conf.VillAvailables.FirstOrDefault()),
                        Demands = Mapper.Map<List<VillasDemandDto>, List<VillaClientDemandViewModel>>(_conf.VillDemandsAndExcluded.Item1),
                        DemandsWithPreviews = Mapper.Map<List<VillasDemandDto>, List<VillaClientDemandViewModel>>(_conf.VillDemandsAndExcluded.Item3),
                        DemandsWithSameClient = Mapper.Map<VillasDemandDto, VillaClientDemandViewModel>(_conf.VillDemandsAndExcluded.Item2),
                    };
                    Session["AddVillDemandMatches"] = matchVM;
                    NotificationHub.showVillaDemandmatchedNotifications(_conf.VillDemandsAndExcluded.Item4.Select(item=> new MatchedDemandsHelper { PK_VillasDemands_Id = item.PK_VillasDemands_Id, FK_VillasDemands_Users_CreatedBy = item.FK_VillasDemands_Users_CreatedBy }).ToList(),matchVM.available.PK_VillasAvailables_Id);
                    return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);

                }
            }

            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);

        }

        //*********************************** from here 
        //VillasAvailable/EditClientSale
        [DisableHtmlInputs]
        public async Task<ActionResult> EditClientSale(string id)
        {
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;          
            if (HttpContext.Items["Disable"] != null && bool.Parse(HttpContext.Items["Disable"].ToString()))
            {
                ViewBag.Disable = true;
            }
            VillsAvailableViewModel clientSale = Mapper.Map<VillasAvailableDto, VillsAvailableViewModel>(await _villasAvailableService.EditClientSale(int.Parse(id)));
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            clientSale.Regions = await _villasAvailableService.GetRegionById(clientSale.FK_VillasAvailables_Regions_Id);
            clientSale.Categories = await _villasAvailableService.GetCatsById(clientSale.FK_VillasAvailables_Categories_Id);
            clientSale.Finishings = await _villasAvailableService.GetFinishingsById(clientSale.FK_VillasAvailables_Finishings_Id);
            clientSale.Accessories = await _villasAvailableService.GetAccessById(clientSale.AccessoriesIds);
            clientSale.Views = await _villasAvailableService.GetViewsById(clientSale.FK_VillasAvailables_View_Id);
            clientSale.Payments = await _villasAvailableService.GetPaymentsId(clientSale.FK_VillasAvailables_PaymentMethod_Id);
            UserDto sales = (await _userService.FindUserByID(clientSale.FK_VillasAvailables_Users_SalesId));
            ViewBag.salesName = sales.FirstName + " " + sales.LastName;
            int? userSpecialization = (await _userService.FindUserByID(userId)).Specialization_Id;
            if (userSpecialization.HasValue)
            {
                string specialName = (await _specialService.FindByID(userSpecialization.Value)).Name;
                if (specialName == "بيع")
                {
                    clientSale.Transactions = await _villasAvailableService.GetTrans(specialName);

                }
                else if (specialName == "ايجار")
                {
                    clientSale.Transactions = await _villasAvailableService.GetTrans(specialName);
                }
            }
            else
            {
                clientSale.Transactions = await _villasAvailableService.GetTrans(null);
            }
            clientSale.Type = (await _transService.FindByID(clientSale.FK_VillasAvailables_Transactions_Id)).TransType;
            clientSale.Usages = await _villasAvailableService.GetUsagesId(clientSale.FK_VillasAvailables_Usage_Id);
            return View(clientSale);
        }

        //VillasAvailable/UpdateClientSale
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateClientSale(VillsAvailableViewModel saleVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            int catId = Categories.Villas;
            saleVM.FK_VillasAvailables_Categories_Id = catId;
            List<string> images = new List<string>();
            if (ModelState.IsValid)
            {
                if (Session["VillasClientSalesImagePathList"] != null)
                {
                    images = (List<string>)Session["VillasClientSalesImagePathList"];
                    Session["VillasClientSalesImagePathList"] = null;
                }
                _conf = await _villasAvailableService.UpdateClientSale(Mapper.Map<VillsAvailableViewModel, VillasAvailableDto>(saleVM), userId, images, branchId);
            }
            if (_conf.Valid)
            {
                VillasAvailableDemandViewModel matchVM = new VillasAvailableDemandViewModel
                {
                    Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(saleVM.FK_VillasAvailables_Clients_ClientId)),
                    Demands = Mapper.Map<List<VillasDemandDto>, List<VillaClientDemandViewModel>>(_conf.VillDemandsAndExcluded.Item1),
                    DemandsWithSameClient = Mapper.Map<VillasDemandDto, VillaClientDemandViewModel>(_conf.VillDemandsAndExcluded.Item2),
                    DemandsWithPreviews = Mapper.Map<List<VillasDemandDto>, List<VillaClientDemandViewModel>>(_conf.VillDemandsAndExcluded.Item3),
                    available = Mapper.Map<VillasAvailableDto, VillsAvailableViewModel>(_conf.VillAvailables.FirstOrDefault())
                };
                MatchesForPreviewsHelper.AddVillasAvailableMatches = matchVM;
                Session["UpdateVillDemandMatches"] = matchVM;
                NotificationHub.showVillaDemandmatchedNotifications(_conf.VillDemandsAndExcluded.Item4.Select(item => new MatchedDemandsHelper { PK_VillasDemands_Id = item.PK_VillasDemands_Id, FK_VillasDemands_Users_CreatedBy = item.FK_VillasDemands_Users_CreatedBy }).ToList(), matchVM.available.PK_VillasAvailables_Id);
                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }
        //VillasAvailable/DeleteClientSale
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteClientSale(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;

            _conf = await _villasAvailableService.DeleteClientSale(int.Parse(id), userId);

            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);
        }

        //VillasAvailable/SaleDetails
        public async Task<ActionResult> SaleDetails(int saleId, int demandId = 0)
        {
            VillsAvailableViewModel clientSale = Mapper.Map<VillasAvailableDto, VillsAvailableViewModel>(await _villasAvailableService.EditClientSale(saleId));
            ViewBag.demandId = demandId;

            clientSale.Regions = await _villasAvailableService.GetRegionById(clientSale.FK_VillasAvailables_Regions_Id);
            clientSale.Transactions = await _villasAvailableService.GetTransById(clientSale.FK_VillasAvailables_Transactions_Id, null);
            clientSale.Categories = await _villasAvailableService.GetCatsById(clientSale.FK_VillasAvailables_Categories_Id);
            clientSale.Finishings = await _villasAvailableService.GetFinishingsById(clientSale.FK_VillasAvailables_Finishings_Id);
            clientSale.Accessories = await _villasAvailableService.GetAccessById(clientSale.AccessoriesIds);
            clientSale.Views = await _villasAvailableService.GetViewsById(clientSale.FK_VillasAvailables_View_Id);
            clientSale.Payments = await _villasAvailableService.GetPaymentsId(clientSale.FK_VillasAvailables_PaymentMethod_Id);
            clientSale.Usages = await _villasAvailableService.GetUsagesId(clientSale.FK_VillasAvailables_Usage_Id);

            // convert it to normal view 

            return PartialView("_SaleDetails", clientSale);

        }
        //VillasAvailable/DemandMatchesAfterUpdate
        public ActionResult DemandMatchesAfterUpdate()
        {
            ViewBag.Category = Categories.Villas;
            //var matches = MatchesForPreviewsHelper.UpdateDemandMatches;
            VillasAvailableDemandViewModel matches = (VillasAvailableDemandViewModel)Session["UpdateVillDemandMatches"];
            return View(matches);
        }
        //VillasAvailable/DemandMatchesAfterAdd
        public ActionResult DemandMatchesAfterAdd()
        {
            //ViewBag.Category = Categories.Villas;
            //var matches = MatchesForPreviewsHelper.AddDemandMatches;
            VillasAvailableDemandViewModel matches = (VillasAvailableDemandViewModel)Session["AddVillDemandMatches"];
            return View(matches);
        }
        //VillasAvailable/GetInstantMatches
        public async Task<ActionResult> GetInstantMatches(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            _conf = await _villasAvailableService.GetInstantMatches(int.Parse(id), userId);
            VillasAvailableDemandViewModel matchVM = new VillasAvailableDemandViewModel
            {
                Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(_conf.VillAvailables.FirstOrDefault().FK_VillasAvailables_Clients_ClientId)),
                Demands = Mapper.Map<List<VillasDemandDto>, List<VillaClientDemandViewModel>>(_conf.VillDemandsAndExcluded.Item1),
                DemandsWithSameClient = Mapper.Map<VillasDemandDto, VillaClientDemandViewModel>(_conf.VillDemandsAndExcluded.Item2),
                DemandsWithPreviews = Mapper.Map<List<VillasDemandDto>, List<VillaClientDemandViewModel>>(_conf.VillDemandsAndExcluded.Item3),
                available = Mapper.Map<VillasAvailableDto, VillsAvailableViewModel>(_conf.VillAvailables.FirstOrDefault())
            };
           NotificationHub.showVillaDemandmatchedNotifications(_conf.VillDemandsAndExcluded.Item4.Select(item=> new MatchedDemandsHelper { PK_VillasDemands_Id = item.PK_VillasDemands_Id, FK_VillasDemands_Users_CreatedBy = item.FK_VillasDemands_Users_CreatedBy }).ToList(),matchVM.available.PK_VillasAvailables_Id);
            return View("DemandMatchesAfterAdd", matchVM);
        }

        //VillasAvailable/GetDemansList
        [HttpPost]
        public async Task<ActionResult> GetDemandsList(VillsAvailableViewModel availableViewModel)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            (List<VillasDemandDto>, List < VillasDemandDto >) availabelist = await _villasAvailableService.GetAvailableMatches(Mapper.Map<VillsAvailableViewModel, VillasAvailableDto>(availableViewModel), userId);
            List<VillaClientDemandViewModel> demands = Mapper.Map<List<VillasDemandDto>, List<VillaClientDemandViewModel>>(availabelist.Item1);
            return Json(new { demanList = demands }, JsonRequestBehavior.AllowGet);
        }

        //VillasAvailable/UploadImages
        [HttpPost]
        public ActionResult UploadImages()
        {
            try
            {


                // Get all files from Request object
                HttpFileCollectionBase files = Request.Files;

                //HttpPostedFileBase file = files[i];
                _conf = _villasAvailableService.SavePhotos(files);
                Session["VillasClientSalesImagePathList"] = _conf.UrlList;

            }
            catch (Exception)
            {
                _conf.Valid = false;
                _conf.Message = "حدث خطا ما عند حفظ الصور!";
                Session["VillasClientSalesImagePathList"] = _conf.UrlList;
            }
            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);


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
            DataTableViewModel tableData = await GetVillDemandTableData(data, id);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.VillasClientDemand,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetVillDemandTableData(DataTableViewModel tableData, int id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            // Getting all entity data
            List<VillaDemandViewModel> clientDemands = Mapper.Map<List<VillasDemandDto>, List<VillaDemandViewModel>>((await _villasDemandService.villaDemands(id)).Where(a => a.FK_VillasDemands_Users_CreatedBy == userId).ToList());
            foreach (VillaDemandViewModel item in clientDemands)
            {
                item.BuyerName = (await _clientService.FindClientByID(item.FK_VillasDemands_Clients_ClientId)).Name;
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

            tableData.VillasClientDemand = clientDemands;

            return tableData;
        }

    }
}