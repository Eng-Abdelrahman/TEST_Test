using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using _3aqarak.BLL.Dto;
using System.Threading.Tasks;

namespace _3aqarak.MVC.Controllers
{
    public class LandsDemandQuickSearchController : Controller
    {
        private readonly ILandsDemandsService _LandsDemandService;
        private readonly IFinishService _finishService;
        private readonly IRegionService _regionService;
        private readonly IClientService _clientService;

        public LandsDemandQuickSearchController(ILandsDemandsService demandService, IFinishService finishService, IRegionService regionService, IClientService clientService)
        {
            _LandsDemandService = demandService;
            _regionService = regionService;
            _finishService = finishService;
            _clientService = clientService;
        }
        // GET: LandsDemandQuickSearch
        public async Task<ActionResult> Index()
        {
            var regions = new LandsDemandsViewModel()
            {
                RegionsTo = await _LandsDemandService.GetRegions(),

            };
            return View(regions);
        }
        //LandsDemandQuickSearch/LoadData
        [HttpPost]
        public async Task<ActionResult> LoadData()
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
                data = tableData.LandsDemand,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetTableData(int size, int skip, DataTableViewModel tableData, int regionidFrom, int regionidTo, DateTime fromDate, DateTime toDate, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Demand)
        {
            //string olddate = pdate.ToString("yyyy-MM-dd");
            // Getting all entity data
            List<LandsDemandsViewModel> LandsDemand = new List<LandsDemandsViewModel>();
            var DemandUnitList =await _LandsDemandService.DemandsByDateAndRegion(fromDate, toDate, regionidFrom, regionidTo, SpaceFrom, SpaceTo, PriceFrom, PriceTo, Demand);

            string GetTypeName(bool type)
            {
                if (type)
                { return "كردون مباني"; }
                return "أرض زراعية";

            }
            tableData.RecordsTotal = DemandUnitList.Count();
            DemandUnitList = DemandUnitList.Skip(skip).Take(size).ToList();
            foreach (var DemandUnit in DemandUnitList)
            {
                LandsDemand.Add(new LandsDemandsViewModel()
                {

                    PK_LandsDemands_Id = DemandUnit.PK_LandsDemands_Id,
                    FK_LandsDemands_Clients_ClientId = DemandUnit.FK_LandsDemands_Clients_ClientId,
                    RegionNameFrom = (await _regionService.FindByID(DemandUnit.FK_LandsDemands_Regions_FromId)).Region,
                    RegionNameTo = (await _regionService.FindByID(DemandUnit.FK_LandsDemands_Regions_ToId)).Region,
                    MinPrice = DemandUnit.MinPrice,
                    MaxPrice = DemandUnit.MaxPrice,
                    MinSpace = DemandUnit.MinSpace,
                    MaxSpace = DemandUnit.MaxSpace,
                    TypeName = GetTypeName(DemandUnit.Type),

                });
            }


            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    LandsDemand = LandsDemand.OrderBy(e => e.PK_LandsDemands_Id).ToList();
                }
                else
                {
                    LandsDemand = LandsDemand.OrderByDescending(e => e.PK_LandsDemands_Id).ToList();
                }

            }

            ////total number of rows count     
            //tableData.RecordsTotal = LandsDemand.Count();

            ////Paging
            //LandsDemand = LandsDemand.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.LandsDemand = LandsDemand;

            return tableData;
        }


        //LandsDemandQuickSearch/LandAvailableDemand
        public ActionResult LandAvailableDemand(string DemandId, string BuyerId)
        {
            AvailableLandsViewModel DemandAndBuyerID = new AvailableLandsViewModel()
            {
                DemandId = DemandId,
                BuyerId = BuyerId,
            };
            return View(DemandAndBuyerID);
        }
        //LandsDemandQuickSearch/ClientAutoComplete
        public async Task< ActionResult> ClientAutoComplete(string text)
        {
            var clients = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>(await _clientService.ClientAutoComplete(text));
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
    }
}