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
    public class ShopDemandQuicSearchController : Controller
    {
        private readonly IShopDemandService _ShopDemandService;
        private readonly IFinishService _finishService;
        private readonly IRegionService _regionService;
        private readonly IClientService _clientService;

        public ShopDemandQuicSearchController(IShopDemandService DemandService, IFinishService finishService, IRegionService regionService, IClientService clientService)
        {
            _ShopDemandService = DemandService;
            _regionService = regionService;
            _finishService = finishService;
            _clientService = clientService;
        }

        // GET: ShopDemandQuicSearch
        public async Task<ActionResult> Index()
        {
            var regions = new ShopDemandViewModel()
            {
                RegionsTo = await _ShopDemandService.GetRegions(),

            };
            return View(regions);
        }

        //ShopDemandQuicSearch/LoadData
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> LoadData()
        {
            var fromDate = new DateTime();
            var toDate = new DateTime();
            DataTableViewModel data = new DataTableViewModel
            {
                Draw = Request.Form.GetValues("draw").FirstOrDefault(),
                Start = Request.Form.GetValues("start").FirstOrDefault(),
                Length = Request.Form.GetValues("length").FirstOrDefault(),
                SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault(),
                SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault(),
                SearchValue = Request.Form.GetValues("search[value]").FirstOrDefault(),
            };

            int regionFrom = 1;
            if (!String.IsNullOrEmpty(Request.Form.GetValues("regionFrom").FirstOrDefault()))
            {
                regionFrom = int.Parse(Request.Form.GetValues("regionFrom").FirstOrDefault());
            }

            int regionTo = 0;
            if (!String.IsNullOrEmpty(Request.Form.GetValues("regionTo").FirstOrDefault()))
            {
                regionTo = int.Parse(Request.Form.GetValues("regionTo").FirstOrDefault());
            }


            int SpaceFrom = 1;
            if (!String.IsNullOrEmpty(Request.Form.GetValues("SpaceFrom").FirstOrDefault()))
            {
                SpaceFrom = int.Parse(Request.Form.GetValues("SpaceFrom").FirstOrDefault());
            }
            if (SpaceFrom == 0)
            { SpaceFrom = 1; }

            int SpaceTo = 0;
            if (!String.IsNullOrEmpty(Request.Form.GetValues("SpaceTo").FirstOrDefault()))
            {
                SpaceTo = int.Parse(Request.Form.GetValues("SpaceTo").FirstOrDefault());
            }


            int PriceFrom = 1;
            if (!String.IsNullOrEmpty(Request.Form.GetValues("PriceFrom").FirstOrDefault()))
            {
                PriceFrom = int.Parse(Request.Form.GetValues("PriceFrom").FirstOrDefault());
            }
            int PriceTo = 0;
            if (!String.IsNullOrEmpty(Request.Form.GetValues("PriceTo").FirstOrDefault()))
            {
                PriceTo = int.Parse(Request.Form.GetValues("PriceTo").FirstOrDefault());
            }



            if (!string.IsNullOrEmpty(Request.Form.GetValues("fromDate").FirstOrDefault()))
            {
                fromDate = DateTime.Parse(Request.Form.GetValues("fromDate").FirstOrDefault());
            }
            else
            {
                //fromDate = DateTime.Now.Date;
                fromDate = new DateTime(2017, 1, 18);
            }

            if (!string.IsNullOrEmpty(Request.Form.GetValues("toDate").FirstOrDefault()))
            {
                toDate = DateTime.Parse(Request.Form.GetValues("toDate").FirstOrDefault()).AddHours(24);
            }
            else
            {
                //toDate = DateTime.Now.Date;
                toDate = DateTime.Now.AddHours(24);
            }
            int Demand = 0;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("Demand").FirstOrDefault()))
            {
                Demand = int.Parse(Request.Form.GetValues("Demand").FirstOrDefault());
            }
            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await GetTableData(data.PageSize, data.Skip, data, regionFrom, regionTo, fromDate, toDate, SpaceFrom, SpaceTo, PriceFrom, PriceTo, Demand);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.ShopDemand,
            }, JsonRequestBehavior.AllowGet);
        }

        private async System.Threading.Tasks.Task<DataTableViewModel> GetTableData(int size, int skip, DataTableViewModel tableData, int regionidFrom, int regionidTo, DateTime fromDate, DateTime toDate, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Demand)
        {
            //string olddate = pdate.ToString("yyyy-MM-dd");
            // Getting all entity data
            List<ShopDemandViewModel> ShopDemand = new List<ShopDemandViewModel>();
            var DemandUnitList = await _ShopDemandService.DemandsByDateAndRegion(fromDate, toDate, regionidFrom, regionidTo, SpaceFrom, SpaceTo, PriceFrom, PriceTo, Demand);
            string GetScaleName(int Scale)
            {

                string scaleName = "";
                switch (Scale)
                {
                    case 1:
                        scaleName = "ميزانان";
                        break;
                    case 2:
                        scaleName = "ميزانان وأرضي";
                        break;
                    case 3:
                        scaleName = "أرضي";
                        break;
                    case 4:
                        scaleName = "الكل";
                        break;
                }
                return scaleName;
            }

            //string GetLicenseName(bool type)
            //{
            //    if (type)
            //    { return "مرخص"; }
            //    return "غير مرخص";

            //}
            //string GetDividerName(bool type)
            //{
            //    if (type)
            //    { return "مقسم"; }
            //    return "غير مقسم";

            //}
            //string GetFurnisherName(bool type)
            //{
            //    if (type)
            //    { return "مجهز"; }
            //    return "غير مجهز";

            //}
            tableData.RecordsTotal = DemandUnitList.Count();
            DemandUnitList = DemandUnitList.Skip(skip).Take(size).ToList();
            foreach (var DemandsUnit in DemandUnitList)
            {
                ShopDemand.Add(new ShopDemandViewModel()
                {

                    PK_ShopDemands_Id = DemandsUnit.PK_ShopDemands_Id,
                    FK_ShopDemands_Clients_ClientId = DemandsUnit.FK_ShopDemands_Clients_ClientId,
                    RegionNameFrom = (await _regionService.FindByID(DemandsUnit.FK_ShopDemands_Regions_FromId)).Region,
                    RegionNameTo = (await _regionService.FindByID(DemandsUnit.FK_ShopDemands_Regions_ToId)).Region,
                    MinPrice = DemandsUnit.MinPrice,
                    MaxPrice = DemandsUnit.MaxPrice,
                    MinSpace = DemandsUnit.MinSpace,
                    MaxSpace = DemandsUnit.MaxSpace,
                    MinBathRooms = DemandsUnit.MinBathRooms,
                    MaxBathRooms = DemandsUnit.MaxBathRooms,

                    ScaleNumber = DemandsUnit.ScaleNumber,
                    Islicense = DemandsUnit.Islicense,
                    IsDivider=DemandsUnit.IsDivider,
                    IsFurnisher=DemandsUnit.IsFurnisher,

                    ScaleName= GetScaleName(DemandsUnit.ScaleNumber),
                    //LicenseName = GetLicenseName(DemandsUnit.Islicense),
                    //DividerName =GetDividerName( DemandsUnit.IsDivider),
                    //Furnisher = GetFurnisherName(DemandsUnit.IsFurnisher),
                    LicenseName = DemandsUnit.Islicense == true ? "مرخص" : "غير مرخص",
                    DividerName = DemandsUnit.IsDivider == true ? "مقسم" : "غير مقسم",
                    Furnisher = DemandsUnit.IsFurnisher == true ? "مجهز" : "غير مجهز",

                });
            }

            ////Search    
            //if (!string.IsNullOrEmpty(region))
            //{

            //}

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    ShopDemand = ShopDemand.OrderBy(e => e.PK_ShopDemands_Id).ToList();
                }
                else
                {
                    ShopDemand = ShopDemand.OrderByDescending(e => e.PK_ShopDemands_Id).ToList();
                }

            }

            ////total number of rows count     
            //tableData.RecordsTotal = ShopDemand.Count();

            ////Paging
            //ShopDemand = ShopDemand.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.ShopDemand = ShopDemand;

            return tableData;
        }


        //ShopDemandQuicSearch/ShopAvailableDemand
        public ActionResult ShopAvailableDemand(string DemandId, string BuyerId)
        {
            ShopAvailableViewModel AvailableAndSalerId = new ShopAvailableViewModel()
            {
                DemandId = DemandId,
                BuyerId = BuyerId,
            };
            ViewBag.Category = Categories.Shops;
            return View(AvailableAndSalerId);
        }
        //ShopDemandQuicSearch/ClientAutoComplete
        public async Task<ActionResult>  ClientAutoComplete(string text)
        {
            var clients = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>(await _clientService.ClientAutoComplete(text));
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
    }
}