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
    public class VillasDemandQuicSearchController : Controller
    {
        private readonly IVillasDemandService _villasDemandService;
        private readonly IFinishService _finishService;
        private readonly IRegionService _regionService;
        private readonly IClientService _clientService;

        public VillasDemandQuicSearchController(IVillasDemandService availableService, IFinishService finishService, IRegionService regionService, IClientService clientService)
        {
            _villasDemandService = availableService;
            _regionService = regionService;
            _finishService = finishService;
            _clientService = clientService;
        }
        // GET: VillasDemandQuicSearch
        public async Task< ActionResult> Index()
        {
            var regions = new VillaClientDemandViewModel()
            {
                RegionsTo = await _villasDemandService.GetRegions(),

            };
            return View(regions);
        }

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
                data = tableData.VillasDemand,
            }, JsonRequestBehavior.AllowGet);
        }

        private async System.Threading.Tasks.Task<DataTableViewModel> GetTableData(int size, int skip, DataTableViewModel tableData, int regionidFrom, int regionidTo, DateTime fromDate, DateTime toDate, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Demand)
        {
            //string olddate = pdate.ToString("yyyy-MM-dd");
            // Getting all entity data
            List<VillaClientDemandViewModel> VillasDemand = new List<VillaClientDemandViewModel>();
            var DemandUnitList = await _villasDemandService.DemandsByDateAndRegion(fromDate, toDate, regionidFrom, regionidTo, SpaceFrom, SpaceTo, PriceFrom, PriceTo, Demand);
            tableData.RecordsTotal = DemandUnitList.Count();
            DemandUnitList = DemandUnitList.Skip(skip).Take(size).ToList();
            foreach (var availablesUnit in DemandUnitList)
            {
                VillasDemand.Add(new VillaClientDemandViewModel()
                {

                    PK_VillasDemands_Id = availablesUnit.PK_VillasDemands_Id,
                    FK_VillasDemands_Clients_ClientId = availablesUnit.FK_VillasDemands_Clients_ClientId,
                    RegionNameFrom = (await _regionService.FindByID(availablesUnit.FK_VillasDemands_Regions_FromId)).Region,
                    RegionNameTo = (await _regionService.FindByID(availablesUnit.FK_VillasDemands_Regions_ToId)).Region,
                    MinPrice = availablesUnit.MinPrice,
                    MaxPrice = availablesUnit.MaxPrice,
                    MinSpace = availablesUnit.MinSpace,
                    MaxSpace = availablesUnit.MaxSpace,
                    MinRooms = availablesUnit.MinRooms,
                    MaxRooms = availablesUnit.MaxRooms,
                    MinBathRooms = availablesUnit.MinBathRooms,
                    MaxBathRooms = availablesUnit.MaxBathRooms,
                    MaxAreaSpace = availablesUnit.MaxAreaSpace,
                    MinAreaSpace = availablesUnit.MinAreaSpace,
                });
            }

          
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    VillasDemand = VillasDemand.OrderBy(e => e.PK_VillasDemands_Id).ToList();
                }
                else
                {
                    VillasDemand = VillasDemand.OrderByDescending(e => e.PK_VillasDemands_Id).ToList();
                }

            }

            ////total number of rows count     
            //tableData.RecordsTotal = VillasDemand.Count();

            ////Paging
            //VillasDemand = VillasDemand.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.VillasDemand = VillasDemand;

            return tableData;
        }


        //VillasDemandQuicSearch/VillasAvailableDemand
        public ActionResult VillasAvailableDemand(string DemandId, string BuyerId)
        {
            VillsAvailableViewModel DemandAndBuyerID = new VillsAvailableViewModel()
            {
                DemandId= DemandId,
                BuyerId = BuyerId,
            };
            return View(DemandAndBuyerID);
        }
        //VillasDemandQuicSearch/ClientAutoComplete
        public async Task<ActionResult> ClientAutoComplete(string text)
        {
            var clients = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>(await _clientService.ClientAutoComplete(text));
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
    }
}