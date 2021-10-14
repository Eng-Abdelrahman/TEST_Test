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
    public class DemandQuickSearchController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IDemandService _demandService;
        private readonly IAvailableService _availableService;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;
        private readonly IRegionService _regionService;
        private readonly IFinishService _finishService;
        private IConfirmation _conf;

        public DemandQuickSearchController(ITransService transService, IClientService clientService, ISpecialService specialService, IDemandService demandService, IConfirmation conf, IAvailableService availableService, IUSerService userService, IRegionService regionService, IFinishService finishService)
        {
            _clientService = clientService;
            _demandService = demandService;
            _conf = conf;
            _availableService = availableService;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
            _regionService = regionService;
            _finishService = finishService;
        }
        // GET: DemandQuickSearch
        public async Task<ActionResult> DemandsIndex()
        {

            DemandViewModel regions = new DemandViewModel()
            {
                RegionsFrom = await _demandService.GetRegions(),
                Categories = await _demandService.GetCats(),
            };
            return View(regions);

        }

        public async Task<ActionResult> LoadDemands()
        {
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            DataTableViewModel data = new DataTableViewModel
            {
                Draw = Request.Form.GetValues("draw").FirstOrDefault(),
                Start = Request.Form.GetValues("start").FirstOrDefault(),
                Length = Request.Form.GetValues("length").FirstOrDefault(),
                SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault(),
                SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault(),
                SearchValue = Request.Form.GetValues("search[value]").FirstOrDefault(),
            };

            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;


            int cat = 0;
            //if (!string.IsNullOrEmpty(Request.Form.GetValues("cat").FirstOrDefault()))
            //{
            //    cat = int.Parse(Request.Form.GetValues("cat").FirstOrDefault());
            //}
            int regIdFrom = 1;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("regId").FirstOrDefault()))
            {
                regIdFrom = int.Parse(Request.Form.GetValues("regId").FirstOrDefault());
            }
            int regIdTo = 0;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("regIdTo").FirstOrDefault()))
            {
                regIdTo = int.Parse(Request.Form.GetValues("regIdTo").FirstOrDefault());
            }
            //else
            //{
            //    var number = 9999;
            //    regIdTo = int.Parse(number.ToString());
            //}

            int ElevatorFrom = 0;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("ElevatorFrom").FirstOrDefault()))
            {
                ElevatorFrom = int.Parse(Request.Form.GetValues("ElevatorFrom").FirstOrDefault());
            }
            int ElevatorTo = 0;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("ElevatorTo").FirstOrDefault()))
            {
                ElevatorTo = int.Parse(Request.Form.GetValues("ElevatorTo").FirstOrDefault());
            }
            //else
            //{
            //    var number = 99;
            //    ElevatorTo = int.Parse(number.ToString());
            //}

            int SpaceFrom = 1;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("SpaceFrom").FirstOrDefault()))
            {
                SpaceFrom = int.Parse(Request.Form.GetValues("SpaceFrom").FirstOrDefault());
            }

            int SpaceTo = 0;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("SpaceTo").FirstOrDefault()))
            {
                SpaceTo = int.Parse(Request.Form.GetValues("SpaceTo").FirstOrDefault());
            }
            //else
            //{
            //    var number = 99999999;
            //    SpaceTo = int.Parse(number.ToString());
            //}

            int PriceFrom = 1;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("PriceFrom").FirstOrDefault()))
            {
                PriceFrom = int.Parse(Request.Form.GetValues("PriceFrom").FirstOrDefault());
            }
            int PriceTo = 0;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("PriceTo").FirstOrDefault()))
            {
                PriceTo = int.Parse(Request.Form.GetValues("PriceTo").FirstOrDefault());
            }
            //else
            //{
            //    var number = 99999999;
            //    PriceTo = int.Parse(number.ToString());
            //}

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
            DataTableViewModel tableData = await GetDemandTableData(data.PageSize, data.Skip, data, fromDate, toDate, cat, regIdFrom, regIdTo, ElevatorFrom, ElevatorTo, SpaceFrom, SpaceTo, PriceFrom, PriceTo, Demand);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.DemandGetDataVM,
            }, JsonRequestBehavior.AllowGet);
        }
        private async Task<DataTableViewModel> GetDemandTableData(int size, int skip, DataTableViewModel tableData, DateTime fromDate, DateTime toDate, int cat, int regIdFrom, int regIdTo, int ElevatorFrom, int ElevatorTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Demand)
        {
            List<DemandGetDataViewModel> DemandGetDataViewModel = new List<DemandGetDataViewModel>();
            // Getting all entity data
            //var Demands = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(_demandService.DemandsByDateAndRegion(fromDate,toDate,regId));

            List<DemandDto> Demands = await _demandService.DemandsByDateAndRegion(fromDate, toDate, cat, regIdFrom, regIdTo, ElevatorFrom, ElevatorTo, SpaceFrom, SpaceTo, PriceFrom, PriceTo, Demand);

            //total number of rows count     
            tableData.RecordsTotal = Demands.Count();
            Demands = Demands.Skip(skip).Take(size).ToList();
            foreach (DemandDto demandsUnit in Demands)
            {
                DemandGetDataViewModel.Add(new DemandGetDataViewModel()
                {
                    FK_DemandUnits_Clients_ClientId=demandsUnit.FK_DemandUnits_Clients_ClientId,
                    PK_DemandUnits_Id = demandsUnit.PK_DemandUnits_Id,
                    RegionNameFrom = (await _regionService.FindByID(demandsUnit.FK_DemandUnits_Regions_FromId)).Region,
                    RegionNameTo = (await _regionService.FindByID(demandsUnit.FK_DemandUnits_Regions_ToId)).Region,
                    MinPrice = demandsUnit.MinPrice,
                    MaxPrice = demandsUnit.MaxPrice,
                    MinFloor = demandsUnit.MinFloor,
                    MaxFloor = demandsUnit.MaxFloor,
                    MinSpace = demandsUnit.MinSpace,
                    MaxSpace = demandsUnit.MaxSpace,
                    MinRooms = demandsUnit.MinRooms,
                    MaxRooms = demandsUnit.MaxRooms,
                    MinBathRooms = demandsUnit.MinBathRooms,
                    MaxBathRooms = demandsUnit.MaxBathRooms,
                    NoElevatorsFrom = demandsUnit.NoElevatorsFrom,
                    NoElevatorsTo = demandsUnit.NoElevatorsTo,
                    //Type = _demandService.GetFinishingsById(demandsUnit.FinishIds),
                });
            }

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                Demands = Demands.Where(e => e.DateString.Contains(tableData.SearchValue)).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    Demands = Demands.OrderBy(e => e.DateString).ToList();
                }
                else
                {
                    Demands = Demands.OrderByDescending(e => e.DateString).ToList();
                }

            }

            ////total number of rows count     
            //tableData.RecordsTotal = Demands.Count();

            ////Paging
            //Demands = Demands.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.DemandGetDataVM = DemandGetDataViewModel;

            return tableData;
        }

        //DemandQuickSearch/AvailableDemand
        public ActionResult AvailableDemand(string demandId, string buyerId)
        {
            AvailableViewModel AvailableAndBuyerID = new AvailableViewModel()
            {
                demandId = demandId,
                buyerId = buyerId,
            };
            return View(AvailableAndBuyerID);
        }



        public async Task< ActionResult> ClientAutoComplete(string text)
        {
            var clients = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>(await _clientService.ClientAutoComplete(text));
            return Json(clients, JsonRequestBehavior.AllowGet);
        }

    }
}