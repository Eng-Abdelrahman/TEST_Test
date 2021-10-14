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
    public class VillasAvailableQuicSearchController : Controller
    {
        private readonly IVillasAvailablesService _villasAvailableService;
        private readonly IFinishService _finishService;
        private readonly IRegionService _regionService;
        private readonly IClientService _clientService;
        private IConfirmation _conf;
        private readonly IVillasDemandService _VillademandService;

        public VillasAvailableQuicSearchController(IVillasDemandService villaDemandService, IConfirmation conf, IVillasAvailablesService availableService, IFinishService finishService, IRegionService regionService, IClientService clientService)
        {
            _villasAvailableService = availableService;
            _regionService = regionService;
            _finishService = finishService;
            _clientService = clientService;
            _conf = conf;
            _VillademandService = villaDemandService;
        }
        // GET: VillasAvailableQuicSearch
        public async Task<ActionResult> Index()
        {
            var regions = new VillsAvailableViewModel()
            {
                Regions = await _villasAvailableService.GetRegions(),
                Payments = await _villasAvailableService.GetPaymentsAsync(),
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

            string VillaNumber = null;
            if (!String.IsNullOrEmpty(Request.Form.GetValues("VillaNumber").FirstOrDefault()))
            {
                VillaNumber = Request.Form.GetValues("VillaNumber").FirstOrDefault();
            }

            string GroupNumber = null;
            if (!String.IsNullOrEmpty(Request.Form.GetValues("GroupNumber").FirstOrDefault()))
            {
                GroupNumber = Request.Form.GetValues("GroupNumber").FirstOrDefault();
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
            DataTableViewModel tableData = await GetTableData(data.PageSize, data.Skip, data, regionFrom, regionTo, fromDate, toDate, SpaceFrom, SpaceTo, PriceFrom, PriceTo, Payments, BasisOfInstallmentDropdown
                , OverFrom, OverTo, RemainingFrom, RemainingTo, YearOfInstallmentFrom, YearOfInstallmentTo, VillaNumber, GroupNumber, Available);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.VillasAvalable,
            }, JsonRequestBehavior.AllowGet);
        }

        private async System.Threading.Tasks.Task<DataTableViewModel> GetTableData(int size, int skip, DataTableViewModel tableData, int regionidFrom, int regionidTo,
            DateTime fromDate, DateTime toDate, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo, int Payments, int BasisOfInstallmentDropdown,
            int OverFrom, int OverTo, int RemainingFrom, int RemainingTo,
            int YearOfInstallmentFrom, int YearOfInstallmentTo, string VillaNumber, string GroupNumber,int Available)
        {
            //string olddate = pdate.ToString("yyyy-MM-dd");
            // Getting all entity data
            List<VillsAvailableViewModel> VillasAvailable = new List<VillsAvailableViewModel>();
            var availablesUnitList = await _villasAvailableService.GetAllAveilableByDateAndRegion(fromDate, toDate, regionidFrom, regionidTo, SpaceFrom, SpaceTo, PriceFrom, PriceTo, Payments, BasisOfInstallmentDropdown
                , OverFrom, OverTo, RemainingFrom, RemainingTo, YearOfInstallmentFrom, YearOfInstallmentTo, VillaNumber, GroupNumber, Available);
            //total number of rows count     
            tableData.RecordsTotal = VillasAvailable.Count();
            availablesUnitList = availablesUnitList.Skip(skip).Take(size).ToList();
            foreach (var availablesUnit in availablesUnitList)
            {
                VillasAvailable.Add(new VillsAvailableViewModel()
                {

                    PK_VillasAvailables_Id = availablesUnit.PK_VillasAvailables_Id,
                    RegionName = (await _regionService.FindByID(availablesUnit.FK_VillasAvailables_Regions_Id)).Region,
                    Space = availablesUnit.Space,
                    AreaSpace = availablesUnit.AreaSpace,
                    Price = availablesUnit.Price,
                    Rooms = availablesUnit.Rooms,
                    BathRooms = availablesUnit.BathRooms,
                    NoOfElevators = availablesUnit.NoOfElevators,
                    Type = (await _finishService.FindByID(availablesUnit.FK_VillasAvailables_Finishings_Id)).Type,
                    FK_VillasAvailables_Clients_ClientId=availablesUnit.FK_VillasAvailables_Clients_ClientId,
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
                    VillasAvailable = VillasAvailable.OrderBy(e => e.PK_VillasAvailables_Id).ToList();
                }
                else
                {
                    VillasAvailable = VillasAvailable.OrderByDescending(e => e.PK_VillasAvailables_Id).ToList();
                }

            }

            ////total number of rows count     
            //tableData.RecordsTotal = VillasAvailable.Count();

            //Paging
            //VillasAvailable = VillasAvailable.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.VillasAvalable = VillasAvailable;

            return tableData;
        }


        //VillasAvailableQuicSearch/SetPreview
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
                    if (postModel.FK_Posts_Categories_Id == Categories.Villas)
                    {
                        var saved = await CreateVillademand(userId, branchId, client, postModel);
                        return Json(new { valid = saved, cat = Categories.Villas }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return PartialView("_PostDetails", postModel);
        }
        private async Task<bool> CreateVillademand(int userId, int branchId, ClientDto client, PostsViewModel clientPost)
        {
            VillasAvailableDto available = await _villasAvailableService.EditClientSale(clientPost.Unit_Id);
            _conf = await _VillademandService.CreateDemandForAvailable(available, userId, branchId, client.PK_Client_Id, available.Notes);
            if (_conf.Valid)
            {
                VillasAvailableDemandViewModel matchVM = new VillasAvailableDemandViewModel
                {
                    Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(available.FK_VillasAvailables_Clients_ClientId)),
                    Demands = Mapper.Map<List<VillasDemandDto>, List<VillaClientDemandViewModel>>(_conf.VillDemands),
                    DemandsWithSameClient = new VillaClientDemandViewModel(),
                    DemandsWithPreviews = new List<VillaClientDemandViewModel>(),
                    available = Mapper.Map<VillasAvailableDto, VillsAvailableViewModel>(available),
                };
                matchVM.Demands[0].BuyerName = client.Name;

                Session["AddVillDemandMatches"] = matchVM;
                return true;
            }
            return false;

        }

        //VillasAvailableQuicSearch/VillasDemandAvailable
        public ActionResult VillasDemandAvailable(string AvailableId, string selerId)
        {
            VillaDemandViewModel AvailableAndSelarID = new VillaDemandViewModel()
            {
                AvailableId=AvailableId,
                selerId=selerId,
            };
            return View(AvailableAndSelarID);
        }
        //VillasAvailableQuicSearch/ClientAutoComplete
        public async Task<ActionResult> ClientAutoComplete(string text)
        {
            var clients = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>( await _clientService.ClientAutoComplete(text));
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
    }
}