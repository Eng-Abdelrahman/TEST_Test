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
    public class AvailableQuickSearchController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IAvailableService _availableService;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;
        private readonly IFinishService _finishService;
        private readonly IRegionService _regionService;
        private readonly IDemandService _demandService;
        private IConfirmation _conf;

        public AvailableQuickSearchController(IDemandService demandService, IClientService clientService, ISpecialService specialService, IAvailableService availableService, IConfirmation conf, IUSerService userService, ITransService transService, IFinishService finishService, IRegionService regionService)
        {
            _clientService = clientService;
            _availableService = availableService;
            _conf = conf;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
            _finishService = finishService;
            _regionService = regionService;
            _demandService = demandService;
        }

       
        // GET: AvailableQuickSearch
        public async Task<ActionResult> Index()
        {

            
                var regions = new AvailableViewModel()
                {
                    Regions = await _availableService.GetRegionsAsync(),
                    Categories = await _availableService.GetCatsAsync(),
                    Payments = await _availableService.GetPaymentsAsync(),
                };
                return View(regions);
            

        }

        [HttpPost]
        public async Task<ActionResult> LoadData()
        {
            try
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
                int cat = 0;
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

                int ElevatorFrom = 0;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("ElevatorFrom").FirstOrDefault()))
                {
                    ElevatorFrom = int.Parse(Request.Form.GetValues("ElevatorFrom").FirstOrDefault());
                }
                int ElevatorTo = 0;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("ElevatorTo").FirstOrDefault()))
                {
                    ElevatorTo = int.Parse(Request.Form.GetValues("ElevatorTo").FirstOrDefault());
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

                int Payments = 0;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("Payments").FirstOrDefault()))
                {
                    Payments = int.Parse(Request.Form.GetValues("Payments").FirstOrDefault());
                }

                int BasisOfInstallmentDropdown = 0;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("BasisOfInstallmentDropdown").FirstOrDefault()))
                {
                    BasisOfInstallmentDropdown = int.Parse(Request.Form.GetValues("BasisOfInstallmentDropdown").FirstOrDefault());
                }

                int OverFrom = 1;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("OverFrom").FirstOrDefault()))
                {
                    OverFrom = int.Parse(Request.Form.GetValues("OverFrom").FirstOrDefault());
                }
                int OverTo = 0;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("OverTo").FirstOrDefault()))
                {
                    OverTo = int.Parse(Request.Form.GetValues("OverTo").FirstOrDefault());
                }

                int RemainingFrom = 1;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("RemainingFrom").FirstOrDefault()))
                {
                    RemainingFrom = int.Parse(Request.Form.GetValues("RemainingFrom").FirstOrDefault());
                }
                int RemainingTo = 0;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("RemainingTo").FirstOrDefault()))
                {
                    RemainingTo = int.Parse(Request.Form.GetValues("RemainingTo").FirstOrDefault());
                }

                int YearOfInstallmentFrom = 1;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("YearOfInstallmentFrom").FirstOrDefault()))
                {
                    YearOfInstallmentFrom = int.Parse(Request.Form.GetValues("YearOfInstallmentFrom").FirstOrDefault());
                }
                int YearOfInstallmentTo = 0;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("YearOfInstallmentTo").FirstOrDefault()))
                {
                    YearOfInstallmentTo = int.Parse(Request.Form.GetValues("YearOfInstallmentTo").FirstOrDefault());
                }

                string FlatNumber = null;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("FlatNumber").FirstOrDefault()))
                {
                    FlatNumber = Request.Form.GetValues("FlatNumber").FirstOrDefault();
                }
                string ApartmentNumber = null;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("ApartmentNumber").FirstOrDefault()))
                {
                    ApartmentNumber = Request.Form.GetValues("ApartmentNumber").FirstOrDefault();
                }

                string GroupNumber = null;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("GroupNumber").FirstOrDefault()))
                {
                    GroupNumber = Request.Form.GetValues("GroupNumber").FirstOrDefault();
                }
                int Available_Id = 0;
                if (!String.IsNullOrEmpty(Request.Form.GetValues("availableNumber").FirstOrDefault()))
                {
                    Available_Id = int.Parse(Request.Form.GetValues("availableNumber").FirstOrDefault());
                }

                //Paging Size (10,20,50,100)
                data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
                data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
                data.RecordsTotal = 0;
                DataTableViewModel tableData = await GetTableData(data.PageSize, data.Skip,
                    data, regionFrom, cat, regionTo, fromDate, toDate, ElevatorFrom, ElevatorTo, SpaceFrom, SpaceTo, PriceFrom, PriceTo, Payments, BasisOfInstallmentDropdown
                    , OverFrom, OverTo, RemainingFrom, RemainingTo, YearOfInstallmentFrom, YearOfInstallmentTo, FlatNumber, ApartmentNumber, GroupNumber, Available_Id);
                return Json(new
                {
                    draw = tableData.Draw,
                    recordsTotal = tableData.RecordsTotal,
                    recordsFiltered = tableData.RecordsTotal,
                    data = tableData.advertisingVMs,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                string message=ex.Message;
            }
            return Json(new { });
        }

        private async Task<DataTableViewModel> GetTableData(int size,int skip,DataTableViewModel tableData, int regionidFrom,int cat, int regionidTo, DateTime fromDate,
            DateTime toDate ,int ElevatorFrom, int ElevatorTo, int SpaceFrom,int SpaceTo, int PriceFrom,int PriceTo,
            int Payments ,int BasisOfInstallmentDropdown,int OverFrom ,int OverTo ,int RemainingFrom ,int RemainingTo ,
            int YearOfInstallmentFrom ,int YearOfInstallmentTo,string FlatNumber ,string ApartmentNumber ,string GroupNumber,int Available_Id)
        {
            //string olddate = pdate.ToString("yyyy-MM-dd");
            // Getting all entity data
            List<AdvertisingVM> advertisingVMs = new List<AdvertisingVM>();
            var availablesUnitList =await _availableService.GetAllAveilableByDateAndRegion( fromDate, toDate,cat,regionidFrom, regionidTo, ElevatorFrom, ElevatorTo, SpaceFrom,SpaceTo,PriceFrom,PriceTo, Payments, BasisOfInstallmentDropdown, OverFrom, OverTo, RemainingFrom, RemainingTo, YearOfInstallmentFrom, YearOfInstallmentTo, FlatNumber, ApartmentNumber, GroupNumber, Available_Id);
            //total number of rows count     
            tableData.RecordsTotal = availablesUnitList.Count();
            availablesUnitList = availablesUnitList.Skip(0).Take(5).ToList();

            //    .Select( (a) =>
            //new AdvertisingVM()
            //{

            //    ClientId = a.FK_AvaliableUnits_Clients_ClientId,
            //    Available_Id = a.PK_AvailableUnits_Id,
            //    RegionName =  (await _regionService.FindByID(a.tbl_units.FK_Units_Regions_Id)).Region,
            //    Space = a.tbl_units.Space,
            //    Floor = a.tbl_units.Floor,
            //    Price = a.Price,
            //    Rooms = a.tbl_units.Rooms,
            //    Bathrooms = a.tbl_units.Bathrooms,
            //    NoOfElevator = a.NoOfElevators,
            //    Type = (await _finishService.FindByID( a.tbl_units.FK_Units_Finishing_Id)).Type,
            //}).ToList();          
            foreach (var availablesUnit in availablesUnitList)
            {
                advertisingVMs.Add(new AdvertisingVM()
                {

                    ClientId = availablesUnit.FK_AvaliableUnits_Clients_ClientId,
                    Available_Id = availablesUnit.PK_AvailableUnits_Id,
                    RegionName = (await _regionService.FindByID(availablesUnit.tbl_units.FK_Units_Regions_Id)).Region,
                    Space = availablesUnit.tbl_units.Space,
                    Floor = availablesUnit.tbl_units.Floor,
                    Price = availablesUnit.Price,
                    Rooms = availablesUnit.tbl_units.Rooms,
                    Bathrooms = availablesUnit.tbl_units.Bathrooms,
                    NoOfElevator = availablesUnit.NoOfElevators,
                    Type = (await _finishService.FindByID(availablesUnit.tbl_units.FK_Units_Finishing_Id)).Type,
                });
            }

            ////Search    
            //if (!string.IsNullOrEmpty(region))
            //{

            //}
            //Search    

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

           

            //Paging
            //advertisingVMs = advertisingVMs.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.advertisingVMs = advertisingVMs;

            return tableData;
        }
        //AvailableQuickSearch/SetPreview
        [HttpPost]
        public async Task<ActionResult> SetPreview(PostsViewModel postModel)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            ClientDto client = new ClientDto();
            //PostsViewModel clientPost = Mapper.Map<PostsDto, PostsViewModel>(await _postService.GetPostDetails(postId));
            if (postModel != null)
            {
                _conf = await _clientService.SaveClient(Mapper.Map<PostsViewModel, ClientDto>(postModel), userId);
                if (_conf.Valid && _conf.Id > 0 && _conf.Clients != null && _conf.Clients.Any())
                {
                    client = _conf.Clients.FirstOrDefault(c => c.PK_Client_Id == _conf.Id);
                    if (postModel.FK_Posts_Categories_Id == Categories.Apartements)
                    {
                        var saved = await CreateDenand(userId, branchId, client, postModel);
                        return Json(new { valid = saved, cat = Categories.Apartements }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return PartialView("_PostDetails", postModel);
        }

        private async Task<bool> CreateDenand(int userId, int branchId, ClientDto client, PostsViewModel clientPost)
        {
            AvailableDto available = await _availableService.EditClientSale(clientPost.Unit_Id);
            _conf = await _demandService.CreateDemandForAvailable(available, userId, branchId, client.PK_Client_Id, clientPost.Notes);
            if (_conf.Valid)
            {
                AvailableDemandMatchViewModel matchVM = new AvailableDemandMatchViewModel
                {
                    Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(available.FK_AvaliableUnits_Clients_ClientId)),
                    Demands = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(_conf.Demands),
                    DemandsWithSameClient = new DemandViewModel(),
                    DemandsWithPreviews = new List<DemandViewModel>(),
                    available = Mapper.Map<AvailableDto, AvailableViewModel>(available),
                };
                matchVM.Demands[0].BuyerName = client.Name;

                Session["AddDemandMatches"] = matchVM;
                return true;
            }
            return false;
        }

        public ActionResult  DemandAvailable(string AvailableId, string selerId )
        {
            
            var AvailableIdAndSelarId = new DemandViewModel()
            {
                AvailableId = AvailableId,
                SelarId=selerId,
            };
            return View(AvailableIdAndSelarId);
        }
        public async Task<ActionResult> ClientAutoComplete(string text)
        {
            var clients = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>(await _clientService.ClientAutoComplete(text));
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
    }
}