using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.MVC.Controllers
{
    public class PostController : Controller
    {
        private readonly IUSerService _userService;
        private readonly IClientService _clientService;
        private IConfirmation _conf;
        private readonly IpostsService _postService;
        private readonly IAvailableService _availableService;
        private readonly IDemandService _demandService;
        private readonly IVillasDemandService _VillademandService;
        private readonly ILandsDemandsService _landDemandService;
        private readonly IShopDemandService _shopDemandService;



        private readonly IVillasAvailablesService _villasAvailableService;
        private readonly IAvailableLandsSevice _availableLandService;
        private readonly IShopAvailableService _shopAvailableService;


        public PostController(IShopDemandService shopDemandService, ILandsDemandsService landDemandService, IVillasDemandService villaDemandService, IDemandService demandService, IAvailableService availableService, IShopAvailableService shopAvailable, IAvailableLandsSevice availableLand, IVillasAvailablesService villasAvailable, IConfirmation conf, IUSerService userService, IpostsService postService, IClientService clientService)
        {
            _conf = conf;
            _userService = userService;
            _postService = postService;
            _clientService = clientService;
            _shopAvailableService = shopAvailable;
            _availableLandService = availableLand;
            _villasAvailableService = villasAvailable;
            _availableService = availableService;
            _demandService = demandService;
            _VillademandService = villaDemandService;
            _landDemandService = landDemandService;
            _shopDemandService = shopDemandService;

        }
        // GET: Post     

        public ActionResult Posts()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoadData()
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

            if (!string.IsNullOrEmpty(Request.Form.GetValues("fromDate").FirstOrDefault()))
            {
                fromDate = DateTime.Parse(Request.Form.GetValues("fromDate").FirstOrDefault());
            }
            else
            {
                //fromDate = DateTime.Now.Date;
                fromDate = DateTime.Now.Date;
            }

            if (!string.IsNullOrEmpty(Request.Form.GetValues("toDate").FirstOrDefault()))
            {
                toDate = DateTime.Parse(Request.Form.GetValues("toDate").FirstOrDefault());
            }
            else
            {
                //toDate = DateTime.Now.Date;
                toDate = DateTime.Now.Date;
            }
            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await GetTableData(data, fromDate, toDate);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.Posts,
            }, JsonRequestBehavior.AllowGet);
        }
        private async Task<DataTableViewModel> GetTableData(DataTableViewModel tableData, DateTime fromDate, DateTime toDate)
        {
            List<PostsViewModel> postVMs = new List<PostsViewModel>();
            postVMs = Mapper.Map<List<PostsDto>, List<PostsViewModel>>(await _postService.GetPosts(fromDate, toDate));

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    postVMs = postVMs.OrderBy(e => e.PK_PostId).ToList();
                }
                else
                {
                    postVMs = postVMs.OrderByDescending(e => e.PK_PostId).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = postVMs.Count();

            //Paging
            postVMs = postVMs.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.Posts = postVMs;

            return tableData;
        }
        [HttpGet]
        public async Task<ActionResult> PostDetails(int postId)
        {
            PostsViewModel clientPost = Mapper.Map<PostsDto, PostsViewModel>(await _postService.GetPostDetails(postId));

            return PartialView("_PostDetails", clientPost);

        }
        [HttpPost]
        public async Task<ActionResult> SetPreview(int postId)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            ClientDto client = new ClientDto();
            PostsViewModel clientPost = Mapper.Map<PostsDto, PostsViewModel>(await _postService.GetPostDetails(postId));
            if (clientPost != null)
            {
                _conf = await _clientService.SaveClient(Mapper.Map<PostsViewModel, ClientDto>(clientPost), userId);
                if (_conf.Valid && _conf.Id > 0 && _conf.Clients != null && _conf.Clients.Any())
                {
                    client = _conf.Clients.FirstOrDefault(c => c.PK_Client_Id == _conf.Id);
                    if (clientPost.FK_Posts_Categories_Id == Categories.Apartements)
                    {
                        var saved=await CreateDenand(userId, branchId, client, clientPost);
                        return Json(new { valid = saved, cat = Categories.Apartements }, JsonRequestBehavior.AllowGet);
                    }

                    //this's for Villas
                    else if (clientPost.FK_Posts_Categories_Id == Categories.Villas)
                    {
                        var saved= await CreateVillademand(userId, branchId, client, clientPost);
                        return Json(new { valid = saved, cat = Categories.Villas }, JsonRequestBehavior.AllowGet);
                    }
                    //this's for Land
                    else if (clientPost.FK_Posts_Categories_Id == Categories.Lands)
                    {
                        var saved = await CreateLandDemand(userId, branchId, client, clientPost);
                        return Json(new { valid = saved, cat = Categories.Lands }, JsonRequestBehavior.AllowGet);

                    }
                    //this's for Shop
                    else if (clientPost.FK_Posts_Categories_Id == Categories.Shops)
                    {
                        var saved = CreateShopDemand(userId, branchId, client, clientPost);
                        return Json(new { valid = saved, cat = Categories.Shops }, JsonRequestBehavior.AllowGet);

                    }

                }
            }


            return PartialView("_PostDetails", clientPost);

        }

        private async Task<bool> CreateShopDemand(int userId, int branchId, ClientDto client, PostsViewModel clientPost)
        {
            ShopAvailableDto available = await _shopAvailableService.EditAvailableShop(clientPost.Unit_Id);
            _conf = await _shopDemandService.CreateDemandForAvailable(available, userId, branchId, client.PK_Client_Id, available.Notes);
            if (_conf.Valid)
            {
                ShopDemandsMatchesAfterAddViewModel matchVM = new ShopDemandsMatchesAfterAddViewModel
                {
                    Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(available.FK_ShopAvailable_Clients_ClientId)),
                    ShopDemands = Mapper.Map<List<ShopDemandDto>, List<ShopDemandViewModel>>(_conf.ShopDemands),
                    ShopDemandsWithSameClient = new ShopDemandViewModel(),
                    ShopDemandsWithPreviews = new List<ShopDemandViewModel>(),
                    ShopAvailable = Mapper.Map<ShopAvailableDto, ShopAvailableViewModel>(available),
                    
                };
                matchVM.ShopDemands[0].BuyerName = client.Name;
                Session["AddShopDemandMatches"] = matchVM;
                return true;
            }
            return false;
        }

        private async Task<bool> CreateLandDemand(int userId, int branchId, ClientDto client, PostsViewModel clientPost)
        {
            AvailableLandsDto available = await _availableLandService.EditAvailableLand(clientPost.Unit_Id);
            _conf = await _landDemandService.CreateDemandForAvailable(available, userId, branchId, client.PK_Client_Id, available.Notes);
            if (_conf.Valid)
            {
                LandDemandMatchViewModel matchVM = new LandDemandMatchViewModel
                {
                    Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(available.FK_AvaliableLands_Clients_ClientId)),
                    LandDemands = Mapper.Map<List<LandsDemandsDto>, List<LandsDemandsViewModel>>(_conf.landsDemands),
                    DemandsWithSameClient = new LandsDemandsViewModel(),
                    LandDemandsWithPreviews = new List<LandsDemandsViewModel>(),
                    Landsavailable = Mapper.Map<AvailableLandsDto, AvailableLandsViewModel>(available),
                };
                matchVM.LandDemands[0].BuyerName = client.Name;

                Session["AddLandDemandMatches"] = matchVM;
                return true;

            }
            return false;

        }

        private async Task<bool> CreateVillademand(int userId, int branchId, ClientDto client, PostsViewModel clientPost)
        {
            VillasAvailableDto available =await  _villasAvailableService.EditClientSale(clientPost.Unit_Id);
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

        private async Task<bool> CreateDenand(int userId, int branchId, ClientDto client, PostsViewModel clientPost)
        {
            AvailableDto available =await _availableService.EditClientSale(clientPost.Unit_Id);
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
    }
}