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
    public class AdvertisingController : Controller
    {
        private readonly IFinishService _finishService;
        private readonly IRegionService _regionService;
        private readonly IClientService _clientService;
        private readonly IAvailableService _availableService;
        private readonly IVillasAvailablesService _villasAvailableService;
        private readonly IAvailableLandsSevice _landAvailableService;
        private readonly IShopAvailableService _shopAvailableService;
        private readonly IDemandService _demandService;



        public AdvertisingController(IDemandService demandService,IShopAvailableService shopAvailableService, IAvailableLandsSevice landAvailableService, IVillasAvailablesService villasAvailableService, IClientService clientService, IAvailableService availableService, IFinishService finishService, IRegionService regionService)
        {
            _clientService = clientService;
            _availableService = availableService;
            _finishService = finishService;
            _regionService = regionService;
            _villasAvailableService = villasAvailableService;
            _landAvailableService = landAvailableService;
            _shopAvailableService = shopAvailableService;
            _demandService = demandService;
        }
        // GET: Advertising
        public async Task<ActionResult> AdevertisingIndex(string catId)
        {
            AdvertisingVM adVM = new AdvertisingVM()
            {
                Regions =await _availableService.GetRegionsAsync(),
                AvailableCat = int.Parse(catId),
            };
            return View(adVM);
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
            int region = 0;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("region").FirstOrDefault()))
            {
                region = int.Parse(Request.Form.GetValues("region").FirstOrDefault());
            }
            int category = 0;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("catId").FirstOrDefault()))
            {
                category = int.Parse(Request.Form.GetValues("catId").FirstOrDefault());
            }
            DateTime pdate = new DateTime();
            string mydate = Request.Form.GetValues("pdate").FirstOrDefault();
            if (!string.IsNullOrEmpty(mydate))
            {
                pdate = DateTime.ParseExact(mydate, "yyyy-MM-dd", null);
                //pdate = Convert.ToDateTime(mydate);
            }
            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await GetTableData(data, pdate, region, category);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.advertisingVMs,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetTableData(DataTableViewModel tableData, DateTime pdate, int regionid, int category)
        {
            List<AdvertisingVM> advertisingVMs = new List<AdvertisingVM>();
            if (category==Categories.Apartements)
            {
                advertisingVMs =await MapApartementsToAdvertise(pdate, regionid, category);
            }
            if (category == Categories.Lands)
            {
                advertisingVMs = await MapLandToAdvertise(pdate, regionid, category);

            }
            if (category == Categories.Shops)
            {
                advertisingVMs = await MapShopToAdvertise(pdate, regionid, category);

            }
            if (category == Categories.Villas)
            {
                advertisingVMs =await MapVillasToAdvertise(pdate, regionid, category);                
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    advertisingVMs = advertisingVMs.OrderBy(e => e.Available_Id).ToList();
                }
                else
                {
                    advertisingVMs = advertisingVMs.OrderByDescending(e => e.Available_Id).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = advertisingVMs.Count();

            //Paging
            advertisingVMs = advertisingVMs.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.advertisingVMs = advertisingVMs;

            return tableData;
        }

        [NonAction]
        public async Task<List<AdvertisingVM>> MapApartementsToAdvertise(DateTime pdate, int regionId,int catId)
        {
            int userBranchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            // Getting all entity data
            List<AdvertisingVM> advertisingVMs = new List<AdvertisingVM>();
            List<AvailableDto> availablesUnitList =await _availableService.ClientBranchSalesByDateAndRegion(pdate, regionId, userBranchId);
            foreach (AvailableDto availablesUnit in availablesUnitList)
            {
                advertisingVMs.Add(new AdvertisingVM()
                {
                    Available_Id = availablesUnit.PK_AvailableUnits_Id,
                    AvailableCat = catId,
                    RegionName = (await _regionService.FindByID(availablesUnit.tbl_units.FK_Units_Regions_Id)).Region,
                    Space = availablesUnit.tbl_units.Space,
                    Floor = availablesUnit.tbl_units.Floor,
                    Price = availablesUnit.Price,
                    Rooms = availablesUnit.tbl_units.Rooms,
                    Bathrooms = availablesUnit.tbl_units.Bathrooms,
                    Type = (await _finishService.FindByID(availablesUnit.tbl_units.FK_Units_Finishing_Id)).Type,
                });
            }
            return advertisingVMs;
        }

        public async Task<List<AdvertisingVM>> MapVillasToAdvertise(DateTime pdate, int regionId,int catId)
        {
            int userBranchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            // Getting all entity data
            List<AdvertisingVM> advertisingVMs = new List<AdvertisingVM>();
            List<VillasAvailableDto> availables =await _villasAvailableService.ClientBranchSalesByDateAndRegion(pdate, regionId, userBranchId);
            foreach (VillasAvailableDto availablesUnit in availables)
            {
                advertisingVMs.Add(new AdvertisingVM()
                {
                    Available_Id = availablesUnit.PK_VillasAvailables_Id,
                    RegionName = (await _regionService.FindByID(availablesUnit.FK_VillasAvailables_Regions_Id)).Region,
                    AvailableCat=catId,
                    Space = availablesUnit.Space,
                    Price = availablesUnit.Price,
                    Rooms = availablesUnit.Rooms,
                    Bathrooms = availablesUnit.BathRooms,
                    Type = (await _finishService.FindByID(availablesUnit.FK_VillasAvailables_Finishings_Id)).Type,
                });
            }
            return advertisingVMs;
        }

        public async Task<List<AdvertisingVM>> MapLandToAdvertise(DateTime pdate, int regionId, int catId)
        {
            int userBranchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            // Getting all entity data
            List<AdvertisingVM> advertisingVMs = new List<AdvertisingVM>();
            List<AvailableLandsDto> availables = await _landAvailableService.ClientBranchSalesByDateAndRegion(pdate, regionId, userBranchId);
            foreach (AvailableLandsDto availablesUnit in availables)
            {
                advertisingVMs.Add(new AdvertisingVM()
                {
                    Available_Id = availablesUnit.PK_AvailableLands_Id,
                    AvailableCat=catId,
                    RegionName = (await _regionService.FindByID(availablesUnit.FK_AvailabeLands_Regions_RegionId)).Region,
                    Space = availablesUnit.Space,
                    Price = availablesUnit.Price,
                });
            }
            return advertisingVMs;
        }


        public async Task<List<AdvertisingVM>> MapShopToAdvertise(DateTime pdate, int regionId, int catId)
        {
            int userBranchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            // Getting all entity data
            List<AdvertisingVM> advertisingVMs = new List<AdvertisingVM>();
            List<ShopAvailableDto> availables = await _shopAvailableService.ClientBranchSalesByDateAndRegion(pdate, regionId, userBranchId);
            foreach (ShopAvailableDto availablesUnit in availables)
            {
                advertisingVMs.Add(new AdvertisingVM()
                {
                    Available_Id = availablesUnit.PK_ShopAvailable_Id,
                    AvailableCat = catId,
                    RegionName = (await _regionService.FindByID(availablesUnit.FK_ShopAvailable_Regions_Id)).Region,
                    Space = availablesUnit.Space,
                    Price = availablesUnit.Price,
                });
            }
            return advertisingVMs;
        }
        public async Task<ActionResult> GetDetails(int id,int cat)
        {
            if (cat ==Categories.Apartements)
            {
                AvailableDto AvailbleUnit =await _availableService.EditClientSale(id);
                var clientSale = Mapper.Map<AvailableDto, AvailableViewModel>(AvailbleUnit);
                clientSale.Regions = await _availableService.GetRegionById(clientSale.tbl_units.FK_Units_Regions_Id);
                //clientSale.Transactions = _availableService.GetTransById(clientSale.FK_AvaliableUnits_Transaction_TransactionId);
                clientSale.Categories = await _availableService.GetCatsById(clientSale.tbl_units.FK_Units_Categories_Id);
                clientSale.Finishings = await _availableService.GetFinishingsByIdAsync(clientSale.tbl_units.FK_Units_Finishing_Id);
                clientSale.Accessories = await _availableService.GetAccessByIdAsync(clientSale.AccessoriesIds);
                clientSale.Views =await  _availableService.GetViewsByIdAsync(clientSale.tbl_units.FK_Units_Views_Id);
                clientSale.Payments = await _availableService.GetPaymentsIdAsync(clientSale.FK_AvailableUnits_PaymentMethod_Id);
                clientSale.Usages = await _demandService.GetUsagesId(clientSale.FK_AvailableUnits_Usage_Id);
           

                return PartialView("_SaleDetails", clientSale);
            }
            if (cat == Categories.Lands)
            {
                AvailableLandsDto AvailbleUnit = await _landAvailableService.EditAvailableLand(id);
                var availableLandsViewModel = Mapper.Map<AvailableLandsDto, AvailableLandsViewModel>(AvailbleUnit);
                availableLandsViewModel.Regions = await _landAvailableService.GetRegionById(availableLandsViewModel.FK_AvailabeLands_Regions_RegionId);
                availableLandsViewModel.Payments = await _landAvailableService.GetPaymentsId(availableLandsViewModel.FK_AvailableLands_PaymentMethod_Id);
                availableLandsViewModel.Views = await _landAvailableService.GetViewsById(availableLandsViewModel.FK_AvailableLands_Views_ViewId);
                availableLandsViewModel.Transactions = await _landAvailableService.GetTransById(availableLandsViewModel.FK_AvaliableLands_Transactions_TransactionId, null);
                return PartialView("_AvailableLandDetails", availableLandsViewModel);
            }
            if (cat == Categories.Shops)
            {
                ShopAvailableDto AvailbleUnit = await _shopAvailableService.EditAvailableShop(id);
                var availableShopVM = Mapper.Map<ShopAvailableDto, ShopAvailableViewModel>(AvailbleUnit);
                availableShopVM.Regions = await _shopAvailableService.GetRegionById(availableShopVM.FK_ShopAvailable_Regions_Id);
                availableShopVM.Views = await _shopAvailableService.GetViewsById(availableShopVM.AccessoriesIds);
                availableShopVM.Transactions =  await _shopAvailableService.GetTransById(availableShopVM.FK_ShopAvailable_Transactions_Id, null);
                availableShopVM.Payments = await _shopAvailableService.GetPaymentsId(availableShopVM.FK_ShopAvailable_PaymentMethod_Id);
                return PartialView("_AvailableShopDetails", availableShopVM);
            }
            if (cat == Categories.Villas)
            {
                VillasAvailableDto AvailbleUnit = await _villasAvailableService.EditClientSale(id);
                var avaialbleVM = Mapper.Map<VillasAvailableDto, VillsAvailableViewModel>(AvailbleUnit);
                avaialbleVM.Regions = await _villasAvailableService.GetRegionById(avaialbleVM.FK_VillasAvailables_Regions_Id);
                avaialbleVM.Transactions = await _villasAvailableService.GetTransById(avaialbleVM.FK_VillasAvailables_Transactions_Id, null);
                avaialbleVM.Categories = await _villasAvailableService.GetCatsById(avaialbleVM.FK_VillasAvailables_Categories_Id);
                avaialbleVM.Finishings = await _villasAvailableService.GetFinishingsById(avaialbleVM.FK_VillasAvailables_Finishings_Id);
                avaialbleVM.Accessories = await _villasAvailableService.GetAccessById(avaialbleVM.AccessoriesIds);
                avaialbleVM.Views =await _villasAvailableService.GetViewsById(avaialbleVM.FK_VillasAvailables_View_Id);
                avaialbleVM.Payments = await _villasAvailableService.GetPaymentsId(avaialbleVM.FK_VillasAvailables_PaymentMethod_Id);
                avaialbleVM.Usages =await  _villasAvailableService.GetUsagesId(avaialbleVM.FK_VillasAvailables_Usage_Id);

                return PartialView("_VillaSaleDetails", avaialbleVM);
            }
            return Json(null, JsonRequestBehavior.AllowGet);

        }
    }
}