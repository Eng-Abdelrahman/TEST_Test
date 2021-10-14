using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
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
    public class ShopAvailableQuicSearchController : Controller
    {
        private readonly IShopAvailableService _shopAvailableService;
        private readonly IFinishService _finishService;
        private readonly IRegionService _regionService;
        private readonly IClientService _clientService;

        public ShopAvailableQuicSearchController(IShopAvailableService availableService, IFinishService finishService, IRegionService regionService, IClientService clientService)
        {
            _shopAvailableService = availableService;
            _regionService = regionService;
            _finishService = finishService;
            _clientService = clientService;
        }
        // GET: ShopAvailableQuicSearch
        public async Task<ActionResult> Index()
        {
            var regions = new ShopAvailableViewModel()
            {
                Regions = await _shopAvailableService.GetRegions(),

            };
            return View(regions);
        }

        //ShopAvailableQuicSearch/LoadData
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
            int Available = 0;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("Available").FirstOrDefault()))
            {
                Available = int.Parse(Request.Form.GetValues("Available").FirstOrDefault());
            }
            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await GetTableData(data.PageSize, data.Skip, data, regionFrom, regionTo, fromDate, toDate, SpaceFrom, SpaceTo, PriceFrom, PriceTo, Available);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.ShopAvailable,
            }, JsonRequestBehavior.AllowGet);
        }

        private async System.Threading.Tasks.Task<DataTableViewModel> GetTableData(int size, int skip, DataTableViewModel tableData, int regionidFrom, int regionidTo, DateTime fromDate, DateTime toDate, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Available)
        {
            //string olddate = pdate.ToString("yyyy-MM-dd");
            // Getting all entity data
            List<ShopAvailableViewModel> VillasAvailable = new List<ShopAvailableViewModel>();
            var availablesUnitList = await _shopAvailableService.GetAllAveilableByDateAndRegion(fromDate, toDate, regionidFrom, regionidTo, SpaceFrom, SpaceTo, PriceFrom, PriceTo, Available);

            //total number of rows count     
            tableData.RecordsTotal = VillasAvailable.Count();
            availablesUnitList = availablesUnitList.Skip(skip).Take(size).ToList();
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
            foreach (var availablesUnit in availablesUnitList)
            {
                VillasAvailable.Add(new ShopAvailableViewModel()
                {

                    PK_ShopAvailable_Id = availablesUnit.PK_ShopAvailable_Id,
                    RegionName = (await _regionService.FindByID(availablesUnit.FK_ShopAvailable_Regions_Id)).Region,
                    Space = availablesUnit.Space,
                    Price = availablesUnit.Price,
                    BathRooms = availablesUnit.BathRooms,
                   
                    ScaleName = GetScaleName(availablesUnit.ScaleNumber),
                    LicenseName = availablesUnit.Islicense == true ? "مرخص" : "غير مرخص",
                    DividerName = availablesUnit.IsDivider == true ? "مقسم" : "غير مقسم",
                    FurnisherName = availablesUnit.IsFurnished == true ? "مجهز" : "غير مجهز",
                    FK_ShopAvailable_Clients_ClientId=availablesUnit.FK_ShopAvailable_Clients_ClientId,
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
                    VillasAvailable = VillasAvailable.OrderBy(e => e.PK_ShopAvailable_Id).ToList();
                }
                else
                {
                    VillasAvailable = VillasAvailable.OrderByDescending(e => e.PK_ShopAvailable_Id).ToList();
                }

            }

            ////total number of rows count     
            //tableData.RecordsTotal = VillasAvailable.Count();

            //Paging
            //VillasAvailable = VillasAvailable.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.ShopAvailable = VillasAvailable;

            return tableData;
        }


        //ShopAvailableQuicSearch/ShopDemandAvailable
        public ActionResult ShopDemandAvailable(string AvailableId, string selerId)
        {
            ShopDemandViewModel AvailableAndSelarID = new ShopDemandViewModel()
            {
                AvailableId = AvailableId,
                selerId = selerId,
            };
            return View(AvailableAndSelarID);
        }
        //ShopAvailableQuicSearch/ClientAutoComplete
        public async Task<ActionResult> ClientAutoComplete(string text)
        {
            var clients = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>(await _clientService.ClientAutoComplete(text));
            return Json(clients, JsonRequestBehavior.AllowGet);
        }

    }
}