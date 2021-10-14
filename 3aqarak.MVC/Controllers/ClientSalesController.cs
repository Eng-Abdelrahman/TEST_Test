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

    public class ClientSalesController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IAvailableService _availableService;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;
        private readonly IDemandService _demandService;
        private IConfirmation _conf;

        public ClientSalesController(IDemandService demandService, IClientService clientService, ISpecialService specialService, IAvailableService availableService, IConfirmation conf, IUSerService userService, ITransService transService)
        {
            _clientService = clientService;
            _availableService = availableService;
            _conf = conf;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
            _demandService = demandService;
        }
        // GET: ClientSales
        public ActionResult Index()
        {

            return View();
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

            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData =await GetTableData(data);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.Clients,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetTableData(DataTableViewModel tableData)
        {

            // Getting all entity data
            List<ClientsViewModel> entityList = Mapper.Map<List<ClientDto>, List<ClientsViewModel>>(await _clientService.GetClients());

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                entityList = entityList.Where(e => e.Name.Contains(tableData.SearchValue) || e.Mobile.Contains(tableData.SearchValue) || (!string.IsNullOrEmpty(e.Mobile2) && e.Mobile2.Contains(tableData.SearchValue))).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    entityList = entityList.OrderBy(e => e.Name).ToList();
                }
                else
                {
                    entityList = entityList.OrderByDescending(e => e.Name).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = entityList.Count();

            //Paging
            entityList = entityList.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.Clients = entityList;

            return tableData;
        }

        public ActionResult ClientSales(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public async Task<ActionResult> LoadSalesData()
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
            int id = int.Parse(Request.Form.GetValues("id").FirstOrDefault());
            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await GetAvailableTableData(data, id);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.ClientSales,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetAvailableTableData(DataTableViewModel tableData, int id)
        {

            // Getting all entity data
            List<AvailableViewModel> clientSales = Mapper.Map<List<AvailableDto>, List<AvailableViewModel>>(await _availableService.ClientSales(id));
            foreach (AvailableViewModel item in clientSales)
            {
                item.tbl_units = new UnitViewModel
                {
                    FK_Units_Categories_Id = Categories.Apartements
                };
            }

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                clientSales = clientSales.Where(e => e.DateString.Contains(tableData.SearchValue)).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    clientSales = clientSales.OrderBy(e => e.DateString).ToList();
                }
                else
                {
                    clientSales = clientSales.OrderByDescending(e => e.DateString).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = clientSales.Count();

            //Paging
            clientSales = clientSales.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.ClientSales = clientSales;

            return tableData;
        }

        public async Task<ActionResult> AddClientSale(string clientId)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            AvailableViewModel clientSale = new AvailableViewModel()
            {
                FK_AvaliableUnits_Clients_ClientId = int.Parse(clientId),
                ClientName = (await _clientService.FindClientByID(int.Parse(clientId))).Name,
                Regions = await _availableService.GetRegionsAsync(),
                //Transactions = _availableService.GetTrans(),
                Categories = await _availableService.GetCatsAsync(),
                Finishings = await _availableService.GetFinishingsAsync(),
                Accessories = await _availableService.GetAccessAsync(),
                Views = await _availableService.GetViewsAsync(),
                Payments = await _availableService.GetPaymentsAsync(),
                CreatedAt = DateTime.Now,
            };
            int? userSpecialization = (await _userService.FindUserByID(userId)).Specialization_Id;
            if (userSpecialization.HasValue)
            {
                string specialName = (await _specialService.FindByID(userSpecialization.Value)).Name;
                if (specialName == "بيع")
                {
                    clientSale.Transactions = await _availableService.GetTransAsync(specialName);

                }
                else if (specialName == "ايجار")
                {
                    clientSale.Transactions = await _availableService.GetTransAsync(specialName);
                }
            }
            else
            {
                clientSale.Transactions = await _availableService.GetTransAsync(null);
            }
            return View(clientSale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveClientSale(AvailableViewModel saleVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            List<string> images = new List<string>();

            _conf = await _availableService.CheckDuplicateClientSales(Mapper.Map<AvailableViewModel, AvailableDto>(saleVM));
            if (!_conf.Valid)
            {
                //return Json(_conf, JsonRequestBehavior.AllowGet);
                return Json(new { message = "هذا العميل لدية نفس العرض", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                if (Session["ClientSalesImagePathList"] != null)
                {
                    images = (List<string>)Session["ClientSalesImagePathList"];
                }
                _conf = await _availableService.SaveClientSale(Mapper.Map<AvailableViewModel, AvailableDto>(saleVM), userId, images, branchId);
            }
            if (_conf.Valid)
            {
                AvailableDemandMatchViewModel matchVM = new AvailableDemandMatchViewModel
                {
                    Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(saleVM.FK_AvaliableUnits_Clients_ClientId)),
                    Demands = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(_conf.DemandsAndExcluded.Item1),
                    DemandsWithSameClient = Mapper.Map<DemandDto, DemandViewModel>(_conf.DemandsAndExcluded.Item2),
                    DemandsWithPreviews = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(_conf.DemandsAndExcluded.Item3),
                    available = Mapper.Map<AvailableDto, AvailableViewModel>(_conf.availables.FirstOrDefault())
                };
                Session["AddDemandMatches"] = matchVM;
                NotificationHub.showDemandmatchedNotifications(_conf.DemandsAndExcluded.Item4.Select(item=>new MatchedDemandsHelper {PK_DemandUnits_Id= item.PK_DemandUnits_Id,FK_DemandUnits_Users_CreatedBy=item.FK_DemandUnits_Users_CreatedBy }).ToList(),matchVM.available.PK_AvailableUnits_Id);
                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        [DisableHtmlInputs]
        public async Task<ActionResult> EditClientSale(string id)
        {
            if (HttpContext.Items["Disable"] != null && bool.Parse(HttpContext.Items["Disable"].ToString()))
            {
                ViewBag.Disable = true;
            }
            AvailableDto clintSaleDto = await _availableService.EditClientSale(int.Parse(id));
            AvailableViewModel clientSale = Mapper.Map<AvailableDto, AvailableViewModel>(clintSaleDto);
            clientSale.tbl_units = new UnitViewModel();
            clientSale = Mapper.Map<AvailableDto, AvailableViewModel>(clintSaleDto);
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            clientSale.Regions = await _availableService.GetRegionById(clientSale.tbl_units.FK_Units_Regions_Id);
            //clientSale.Transactions = _availableService.GetTransById(clientSale.FK_AvaliableUnits_Transaction_TransactionId);
            clientSale.Categories = await  _availableService.GetCatsById(clientSale.tbl_units.FK_Units_Categories_Id);
            clientSale.Finishings = await _availableService.GetFinishingsByIdAsync(clientSale.tbl_units.FK_Units_Finishing_Id);
            clientSale.Accessories = await _availableService.GetAccessByIdAsync(clientSale.AccessoriesIds);
            clientSale.Views = await _availableService.GetViewsByIdAsync(clientSale.tbl_units.FK_Units_Views_Id);
            clientSale.Payments = await _availableService.GetPaymentsIdAsync(clientSale.FK_AvailableUnits_PaymentMethod_Id);
            clientSale.Usages = await _demandService.GetUsagesId(clientSale.FK_AvailableUnits_Usage_Id);
            UserDto sales = (await _userService.FindUserByID(clientSale.FK_AvaliableUnits_Users_SalesId));
            ViewBag.salesName = sales.FirstName + " " + sales.LastName;
            int? userSpecialization = (await _userService.FindUserByID(userId)).Specialization_Id;
            if (userSpecialization.HasValue)
            {
                string specialName = (await _specialService.FindByID(userSpecialization.Value)).Name;
                if (specialName == "بيع")
                {
                    clientSale.Transactions = await _availableService.GetTransAsync(specialName);

                }
                else if (specialName == "ايجار")
                {
                    clientSale.Transactions = await _availableService.GetTransAsync(specialName);
                }
            }
            else
            {
                clientSale.Transactions = await _availableService.GetTransAsync(null);
            }
            clientSale.TransType = (await _transService.FindByID(clientSale.FK_AvaliableUnits_Transaction_TransactionId)).TransType;
            
            return View(clientSale);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateClientSale(AvailableViewModel saleVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
            List<string> images = new List<string>();
            if (ModelState.IsValid)
            {
                if (Session["ClientSalesImagePathList"] != null)
                {
                    images = (List<string>)Session["ClientSalesImagePathList"];
                    Session["ClientSalesImagePathList"] = null;
                }
                _conf = await _availableService.UpdateClientSale(Mapper.Map<AvailableViewModel, AvailableDto>(saleVM), userId, images, branchId);
            }
            if (_conf.Valid)
            {
                AvailableDemandMatchViewModel matchVM = new AvailableDemandMatchViewModel
                {
                    Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(saleVM.FK_AvaliableUnits_Clients_ClientId)),
                    Demands = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(_conf.DemandsAndExcluded.Item1),
                    DemandsWithSameClient = Mapper.Map<DemandDto, DemandViewModel>(_conf.DemandsAndExcluded.Item2),
                    DemandsWithPreviews = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(_conf.DemandsAndExcluded.Item3),
                    available = Mapper.Map<AvailableDto, AvailableViewModel>(_conf.availables.FirstOrDefault())
                };                
                Session["UpdateDemandMatches"] = matchVM;
                NotificationHub.showDemandmatchedNotifications(_conf.DemandsAndExcluded.Item4.Select(item => new MatchedDemandsHelper{ PK_DemandUnits_Id = item.PK_DemandUnits_Id,FK_DemandUnits_Users_CreatedBy = item.FK_DemandUnits_Users_CreatedBy }).ToList(),matchVM.available.PK_AvailableUnits_Id);
                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
            }
            //System.Threading.Thread.Sleep(7000);
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteClientSale(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;

            _conf = await _availableService.DeleteClientSale(int.Parse(id), userId);

            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaleDetails(int saleId, int demandId = 0)
        {
            AvailableViewModel clientSale = Mapper.Map<AvailableDto, AvailableViewModel>(await _availableService.EditClientSale(saleId));
            ViewBag.demandId = demandId;

            clientSale.Regions = await _availableService.GetRegionById(clientSale.tbl_units.FK_Units_Regions_Id);
            clientSale.Transactions = await _availableService.GetTransById(clientSale.FK_AvaliableUnits_Transaction_TransactionId, null);
            clientSale.Categories = await _availableService.GetCatsById(clientSale.tbl_units.FK_Units_Categories_Id);
            clientSale.Finishings = await _availableService.GetFinishingsByIdAsync(clientSale.tbl_units.FK_Units_Finishing_Id);
            clientSale.Accessories = await _availableService.GetAccessByIdAsync(clientSale.AccessoriesIds);
            clientSale.Views = await _availableService.GetViewsByIdAsync(clientSale.tbl_units.FK_Units_Views_Id);
            clientSale.Payments = await _availableService.GetPaymentsIdAsync(clientSale.FK_AvailableUnits_PaymentMethod_Id);
            return PartialView("_SaleDetails", clientSale);
        }

        public ActionResult DemandMatchesAfterUpdate()
        {
            AvailableDemandMatchViewModel matches = (AvailableDemandMatchViewModel)Session["UpdateDemandMatches"];
            return View(matches);
        }

        public ActionResult DemandMatchesAfterAdd()
        {
           
            AvailableDemandMatchViewModel matches = (AvailableDemandMatchViewModel)Session["AddDemandMatches"];
            return View(matches);
        }
        //ClientSales/GetInstantMatches
        public async Task<ActionResult> GetInstantMatches(string id)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            _conf = await _availableService.GetInstantMatches(int.Parse(id), userId);
            AvailableDemandMatchViewModel matchVM = new AvailableDemandMatchViewModel
            {
                Seller = Mapper.Map<ClientDto, ClientsViewModel>(await _clientService.FindClientByID(_conf.availables.FirstOrDefault().FK_AvaliableUnits_Clients_ClientId)),
                Demands = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(_conf.DemandsAndExcluded.Item1),
                DemandsWithSameClient = Mapper.Map<DemandDto, DemandViewModel>(_conf.DemandsAndExcluded.Item2),
                DemandsWithPreviews = Mapper.Map<List<DemandDto>, List<DemandViewModel>>(_conf.DemandsAndExcluded.Item3),
                available = Mapper.Map<AvailableDto, AvailableViewModel>(_conf.availables.FirstOrDefault())
            };
            NotificationHub.showDemandmatchedNotifications(_conf.DemandsAndExcluded.Item4.Select(item => new MatchedDemandsHelper { PK_DemandUnits_Id = item.PK_DemandUnits_Id, FK_DemandUnits_Users_CreatedBy = item.FK_DemandUnits_Users_CreatedBy }).ToList(), matchVM.available.PK_AvailableUnits_Id);
            return View("DemandMatchesAfterAdd", matchVM);
        }




        #region Upload

        [HttpPost]
        public ActionResult UploadImages()
        {
            try
            {


                // Get all files from Request object
                HttpFileCollectionBase files = Request.Files;

                //HttpPostedFileBase file = files[i];
                _conf = _availableService.SavePhotos(files);
                Session["ClientSalesImagePathList"] = _conf.UrlList;

            }
            catch (Exception)
            {
                _conf.Valid = false;
                _conf.Message = "حدث خطا ما عند حفظ الصور!";
                Session["ClientSalesImagePathList"] = _conf.UrlList;
            }
            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);


        }
        #endregion "Upload"



    }
}