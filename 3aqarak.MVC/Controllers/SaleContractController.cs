using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Filters;
using _3aqarak.MVC.Helpers;
using _3aqarak.MVC.Structs;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace _3aqarak.MVC.Controllers
{
    [IsBranchManager]
    public class SaleContractController : Controller
    {
        // GET: SaleContract

        private readonly IClientService _clientService;
        private readonly ISaleContractService _saleService;
        private readonly IAvailableService _availableService;
        private readonly IDemandService _demandService;
        private readonly IPreviewService _previewService;
        private readonly IexpectedContractService _expectedService;
        private readonly IStaticService _staticService;
        private readonly ITransService _transService;
        private readonly IAccountingService _accountingService;
        private readonly IVillasAvailablesService _villasAvailableService;
        private readonly IAvailableLandsSevice _availableLandService;
        private readonly IShopAvailableService _shopAvailableService;
        private readonly IVillasDemandService _villasDemandService;
        private readonly IShopDemandService _shopDemandService;
        private readonly ILandsDemandsService _landDemandService;
        private IConfirmation _conf;

        public SaleContractController( ILandsDemandsService landDemand, IShopDemandService shopDemand, IVillasDemandService villasDemand, IShopAvailableService shopAvailable, IAvailableLandsSevice availableLand, IVillasAvailablesService villasAvailable, ITransService transService, IAccountingService accountingService, IexpectedContractService expectedService, IPreviewService previewService, IDemandService demandService, IStaticService staticService, IClientService clientService, IConfirmation conf, IAvailableService availableService, ISaleContractService saleService)
        {
            _villasDemandService = villasDemand;
            _shopDemandService = shopDemand;
            _landDemandService = landDemand;
            _shopAvailableService = shopAvailable;
            _availableLandService = availableLand;
            _villasAvailableService = villasAvailable;
            _clientService = clientService;
            _conf = conf;
            _availableService = availableService;
            _saleService = saleService;
            _expectedService = expectedService;
            _staticService = staticService;
            _demandService = demandService;
            _previewService = previewService;
            _transService = transService;
            _accountingService = accountingService;
        }
        // GET: RentContract
        public async Task<ActionResult> Index()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var salecontractVM = new SaleContractViewModel()
            {
                Cats = await _saleService.GetCats(),
            };
            return View(salecontractVM);
        }

        [HttpPost]
        public async Task<ActionResult> LoadData()
        {
            DateTime? fromDate = new DateTime();
            DateTime? toDate = new DateTime();
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
                fromDate = null;
            }

            if (!string.IsNullOrEmpty(Request.Form.GetValues("toDate").FirstOrDefault()))
            {
                toDate = DateTime.Parse(Request.Form.GetValues("toDate").FirstOrDefault());
            }
            else
            {
                toDate = null;
            }
            var catId = !string.IsNullOrEmpty(Request.Form["catId"]) ? int.Parse(Request.Form["catId"]) : 0;

            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await GetTableData(data, fromDate, toDate,catId);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.SaleContracts,
                dateProblem = false,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetTableData(DataTableViewModel tableData, DateTime? from, DateTime? to,int catId)
        {
            var userName = ((UserDto)Session["User"]).FirstName + " " + ((UserDto)Session["User"]).LastName;
            // Getting all entity data
            List<SaleContractViewModel> entityList = Mapper.Map<List<SaleHeaderDto>, List<SaleContractViewModel>>(await _saleService.GetContracts(from, to,catId));
            foreach (var item in entityList)
            {
                item.BuyerName = (await _clientService.FindClientByID(item.FK_SalesHeaders_Clients_BuyerId)).Name;
                item.SellerName = (await _clientService.FindClientByID(item.FK_SalesHeaders_Clients_SellerId)).Name;
                item.EmpName = userName;
                item.DateString = item.Date.ToShortDateString();
                if (item.IsInstallable)
                {
                    item.Installement = "تقسيــط";
                }
                else
                {
                    item.Installement = "كــاش";
                }

            }

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                entityList = entityList.Where(e => e.BuyerName.Contains(tableData.SearchValue) || e.SellerName.Contains(tableData.SearchValue) || e.BuyerId.Contains(tableData.SearchValue) || e.SellerId.Contains(tableData.SearchValue)).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    entityList = entityList.OrderBy(e => e.PK_SalesHeaders_Id).ToList();
                }
                else
                {
                    entityList = entityList.OrderByDescending(e => e.PK_SalesHeaders_Id).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = entityList.Count();

            //Paging
            entityList = entityList.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.SaleContracts = entityList;

            return tableData;
        }




        public async Task<ActionResult> AddWaiverContract(string id, string type, string install, string isareement)
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var expected = await _expectedService.FindByID(int.Parse(id));
            var buyer = await _clientService.FindClientByID(expected.FK_ExpectContract_Clients_Buyer);
            var seller = await _clientService.FindClientByID(expected.FK_ExpectContract_Clients_Seller);
            var userName = ((UserDto)Session["User"]).FirstName + " " + ((UserDto)Session["User"]).LastName;
            var transaction = await GetTransIdByCategory(expected.CategoryId, seller.PK_Client_Id, expected.AvailableUnits_Id);
            if (transaction.TransCode != 1)
            {
                await UpdateAvailableTransaction(expected.CategoryId, expected.AvailableUnits_Id);
                await UpdateDemandTransaction(expected.CategoryId, expected.FK_ExpectedContract_Preview_Id);
            }
            var category = expected.CategoryId;
            var newContract = new SaleContractViewModel();
            newContract.TypeOfContract = GetContractType(bool.Parse(isareement));
            newContract.EmpName = userName;
            newContract.FK_SalesHeaders_Users_EmpId = ((UserDto)Session["User"]).PK_Users_Id;
            newContract.FK_SalesHeaders_Clients_BuyerId = expected.FK_ExpectContract_Clients_Buyer;
            newContract.FK_SalesHeaders_Clients_SellerId = expected.FK_ExpectContract_Clients_Seller;
            newContract.AvailableUnits_Id = expected.AvailableUnits_Id;
            newContract.AvailableCat = expected.CategoryId;
            var preview = await _previewService.FindById(expected.FK_ExpectedContract_Preview_Id);
            newContract.DemandUnits_Id = preview.DemandUnit_Id;
            newContract.DemandCat = preview.Category_Id;
            newContract.BuyerName = buyer.Name;
            newContract.SellerName = seller.Name;
            newContract.SellerAddress = seller.Address;
            newContract.SellerId = seller.IdNumber;
            newContract.BuyerId = buyer.IdNumber;
            newContract.ReservDeposit = expected.Deposit;
            newContract.BuyerAddress = buyer.Address;
            newContract.Basis = await _saleService.GetPaymentBasis();
            var contractType = type == "rent" ? TranscatTypes.Rental : TranscatTypes.Sale;
            if (contractType == TranscatTypes.Sale)
            {
                newContract.ReservDeposit = expected.Deposit;
            }
            var transId = (await _transService.GetTrans()).Where(t => t.TransCode == contractType).FirstOrDefault().PK_Transactions_Id;
            newContract.DetailContent = (await _staticService.GetTemplates()).Where(t => t.FK_StaticContract_Categories_CatId == category && t.FK_StaticContract_Transaction_Transid == transId).FirstOrDefault()?.ContactContent;
            if (Boolean.Parse(install))
            {
                ViewBag.IsInstallable = true;
            }
            else
            {
                ViewBag.IsInstallable = false;
            }
            Session["expected"] = expected;
            return View(newContract);
        }



        private string GetContractType(bool type)
        {
            if (type)
            { return "عقد إتفاق"; }
            return "إيصال عربون";

        }
        public async Task<ActionResult> AddContract(string id, string type, string install,string isareement)
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var expected = await _expectedService.FindByID(int.Parse(id));
            var buyer =await  _clientService.FindClientByID(expected.FK_ExpectContract_Clients_Buyer);
            var seller = await _clientService.FindClientByID(expected.FK_ExpectContract_Clients_Seller);
            var userName = ((UserDto)Session["User"]).FirstName + " " + ((UserDto)Session["User"]).LastName;
            var transaction = await GetTransIdByCategory(expected.CategoryId, seller.PK_Client_Id, expected.AvailableUnits_Id);
            if (transaction.TransCode != 1)
            {
                await UpdateAvailableTransaction(expected.CategoryId, expected.AvailableUnits_Id);
                await UpdateDemandTransaction(expected.CategoryId, expected.FK_ExpectedContract_Preview_Id);
            } 
            var category = expected.CategoryId;
            var newContract = new SaleContractViewModel();
            newContract.TypeOfContract = GetContractType(bool.Parse(isareement));
            newContract.EmpName = userName;
            newContract.FK_SalesHeaders_Users_EmpId = ((UserDto)Session["User"]).PK_Users_Id;
            newContract.FK_SalesHeaders_Clients_BuyerId = expected.FK_ExpectContract_Clients_Buyer;
            newContract.FK_SalesHeaders_Clients_SellerId = expected.FK_ExpectContract_Clients_Seller;
            newContract.AvailableUnits_Id = expected.AvailableUnits_Id;
            newContract.AvailableCat = expected.CategoryId;
            var preview = await _previewService.FindById(expected.FK_ExpectedContract_Preview_Id);
            newContract.DemandUnits_Id = preview.DemandUnit_Id;
            newContract.DemandCat = preview.Category_Id;
            newContract.BuyerName = buyer.Name;
            newContract.SellerName = seller.Name;
            newContract.SellerAddress = seller.Address;
            newContract.SellerId = seller.IdNumber;
            newContract.BuyerId = buyer.IdNumber;
            newContract.ReservDeposit = expected.Deposit;
            newContract.BuyerAddress = buyer.Address;
            newContract.Basis = await _saleService.GetPaymentBasis();
            var contractType = type == "rent" ? TranscatTypes.Rental : TranscatTypes.Sale;
            if (contractType == TranscatTypes.Sale)
            {
                newContract.ReservDeposit = expected.Deposit;
            }
            var transId = (await _transService.GetTrans()).Where(t => t.TransCode == contractType).FirstOrDefault().PK_Transactions_Id;
            newContract.DetailContent = (await _staticService.GetTemplates()).Where(t => t.FK_StaticContract_Categories_CatId == category && t.FK_StaticContract_Transaction_Transid == transId).FirstOrDefault()?.ContactContent;
            if (Boolean.Parse(install))
            {
                ViewBag.IsInstallable = true;
            }
            else
            {
                ViewBag.IsInstallable = false;
            }
            Session["expected"] = expected;
            return View(newContract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveContract(SaleContractViewModel contractVM)
        {

            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            
            if (contractVM.DateOfFirstInstall.HasValue && contractVM.DateOfNextInstall.HasValue && (contractVM.DateOfFirstInstall >= contractVM.DateOfNextInstall))
            {
                return Json(new { valid = false, message = "تاريخ القسط القادم لا بد  ان يكون اكبر من تاريخ اول قسط!" }, JsonRequestBehavior.AllowGet);
            }
            Regex regex = new Regex("^([0-9]{14})$");
            if (!regex.IsMatch(contractVM.SellerId) || !regex.IsMatch(contractVM.BuyerId))
            {
                return Json(new { valid = false, message = "الرقم القومي لايمكن ان يقل او يزيد عن 14 رقم!" }, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                if (Session["SaleContractImagePathList"] != null)
                {
                    contractVM.ContractUrl = ((List<string>)Session["SaleContractImagePathList"])[0];
                    Session["SaleContractImagePathList"] = null;
                }
                //if (contractVM.TotalAmount != contractVM.PaidAmount + (contractVM.ReservDeposit + contractVM.DueAmount) && contractVM.PaidAmount != contractVM.TotalAmount - (contractVM.PaidAmount + contractVM.ReservDeposit))
                //{
                //    _conf.Valid = false;
                //    _conf.Message = "حدث خطأ";
                //    return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);
                //}
                //var user = GetUsetId(contractVM.DemandUnits_Id, contractVM.DemandCat);
                _conf = await _saleService.SaveContract(Mapper.Map<SaleContractViewModel, SaleHeaderDto>(contractVM), Mapper.Map<SaleContractViewModel, SaleDetailsDto>(contractVM), userId);
                if (_conf.Valid)
                {
                    var expected = (ExpectedContractDto)Session["expected"];
                    var expectClose = await _expectedService.CloseExpected(expected.PK_ExpectContracts_Id, userId);
                    var demandClose = await CloseDemand(contractVM.DemandUnits_Id, contractVM.DemandCat, userId);
                    var AvailableClose = await CloseAvailable(expected.AvailableUnits_Id, expected.CategoryId, userId);
                }
            }
            return Json(new { valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditContract(string id)
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var contractId = int.Parse(id);
            var contractVM = Mapper.Map<SaleHeaderDto, SaleContractViewModel>(await _saleService.FindHeaderByID(contractId));
            contractVM.BuyerName = (await _clientService.FindClientByID(contractVM.FK_SalesHeaders_Clients_BuyerId)).Name;
            contractVM.SellerName = (await _clientService.FindClientByID(contractVM.FK_SalesHeaders_Clients_SellerId)).Name;
            var userName = ((UserDto)Session["User"]).FirstName + " " + ((UserDto)Session["User"]).LastName;
            if (contractVM.FK_SalesHeaders_PaymentBasis_Id.HasValue)
            {
                contractVM.Basis = await _saleService.GetPaymentBasisId(contractVM.FK_SalesHeaders_PaymentBasis_Id.Value);
            }
            contractVM.DetailContent = (await _saleService.FindDetailByID(contractVM.PK_SalesHeaders_Id)).DetailContent;
            if (contractVM.IsInstallable)
            {
                ViewBag.IsInstallable = true;
            }
            else
            {
                ViewBag.IsInstallable = false;
            }
            return View(contractVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateContract(SaleContractViewModel contractVM)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            if (contractVM.DateOfFirstInstall.HasValue && contractVM.DateOfNextInstall.HasValue && (contractVM.DateOfFirstInstall >= contractVM.DateOfNextInstall))
            {
                return Json(new { valid = false, message = "تاريخ القسط القادم لا بد  ان يكون اكبر من تاريخ اول قسط!" }, JsonRequestBehavior.AllowGet);
            }

            Regex regex = new Regex("^([0-9]{14})$");
            if (!regex.IsMatch(contractVM.SellerId) || !regex.IsMatch(contractVM.BuyerId))
            {
                return Json(new { valid = false, message = "الرقم القومي لايمكن ان يقل او يزيد عن 14 رقم!" }, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                if (Session["SaleContractImagePathList"] != null)
                {
                    contractVM.ContractUrl = ((List<string>)Session["SaleContractImagePathList"])[0];
                    Session["SaleContractImagePathList"] = null;
                }
                //var use = GetUsetId(contractVM.DemandUnits_Id, contractVM.DemandCat);
                _conf.Valid = await _saleService.UpdateContract(Mapper.Map<SaleContractViewModel, SaleHeaderDto>(contractVM), Mapper.Map<SaleContractViewModel, SaleDetailsDto>(contractVM), userId);

            }
            return Json(new { valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteContract(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var contract = await _saleService.FindHeaderByID(int.Parse(id));
            var valid = await _saleService.DeleteContract(int.Parse(id), userId);
            if (valid)
            {
                var demandReleases = await ReleaseDemand(contract.DemandUnits_Id, contract.DemandCat, userId);
                var AvailableReleased = await ReleaseAvailable(contract.AvailableUnits_Id, contract.AvailableCat, userId);
                var deletedCompCommission = await _saleService.DeleteRelatedCompCommission(contract.PK_SalesHeaders_Id);
                var deletedEmpCommission = await _saleService.DeleteRelatedEmpCommission(contract.PK_SalesHeaders_Id);
            }

            return Json(valid, JsonRequestBehavior.AllowGet);

        }


        #region Upload

        [HttpPost]
        public ActionResult UploadImages()
        {
            try
            {
                if (Session["SaleContractImagePathList"] != null && ((List<string>)Session["SaleContractImagePathList"]).Count > 0)
                {
                    var path = ((List<string>)Session["SaleContractImagePathList"])[0];

                    var photo = Directory
                             .GetFiles(Server.MapPath("/Assets/Img/SaleContractImages"), path.Split('/')[3], SearchOption.AllDirectories)
                             .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                    }

                    Session["SaleContractImagePathList"] = null;
                }
                // Get all files from Request object
                HttpFileCollectionBase files = Request.Files;
                if (files.Count < 1)
                {
                    return Json(new { valid = false, message = "لابد من تحميل صورة العقد" }, JsonRequestBehavior.AllowGet);
                }
                //HttpPostedFileBase file = files[i];
                _conf = _saleService.SavePhotos(files);
                Session["SaleContractImagePathList"] = _conf.UrlList;

            }
            catch (Exception ex)
            {
                var errmsg = ex.Message;
                _conf.Valid = false;
                _conf.Message = "حدث خطا ما عند حفظ الصور!";
                Session["SaleContractImagePathList"] = _conf.UrlList;
            }

            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);


        }
        #endregion "Upload"

        public async Task<ActionResult> GetPDF(int id)
        {
            if (Session["SaleContractImagePathList"] == null)
            {
                var fileStream = new FileStream(HostingEnvironment.MapPath("~/" + (await _saleService.FindHeaderByID(id)).ContractUrl),
                                                 FileMode.Open,
                                                 FileAccess.Read
                                               );
                var fsResult = new FileStreamResult(fileStream, "application/pdf");
                return fsResult;
            }
            else
            {
                var pdfPath = ((List<string>)Session["SaleContractImagePathList"])[0];
                var fileStream = new FileStream(HostingEnvironment.MapPath("~/" + pdfPath),
                                                 FileMode.Open,
                                                 FileAccess.Read
                                               );
                var fsResult = new FileStreamResult(fileStream, "application/pdf");
                return fsResult;
            }
        }
        public ActionResult SalesToCollect(string salescollects)
        {

            var SaleToCollectDto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaleHeaderDto>>(salescollects);
            var SaleToCollectVM = Mapper.Map<List<SaleHeaderDto>, List<SaleContractViewModel>>(SaleToCollectDto);
            return View(SaleToCollectVM);
        }
        public async Task<ActionResult> UpdateNextSaleInstalled(DateTime newDate, int saleContractId, string amount, string nextPuls)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var nextSale = await _saleService.FindHeaderByID(saleContractId);
            var nextFee = int.Parse(amount);
            var nextInstallValue = int.Parse(nextPuls);
            if (nextSale.TotalAmount != (nextSale.PaidAmount + nextSale.ReservDeposit + nextSale.DueAmount) || nextSale.PaidAmount > nextSale.TotalAmount)
            {
                _conf.Message = "الميعاد غير سليم او  اقل من تاريخ الدفعه السابقه او المبلغ غير صحيح";
                return Json(new { Valid = false, message = _conf.Message }, JsonRequestBehavior.AllowGet);
            }
            await _saleService.UpdateNextIntstall(nextSale, nextFee, newDate, userId, nextInstallValue);
            return Json(new { Valid = true, message = "تم تحصيل الايجار" }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public async Task<TransDto> GetTransIdByCategory(int catId, int clientId, int AvailableId)
        {
            var trans = new TransDto();
            if (catId == Categories.Apartements)
            {
                var transId = (await _availableService.GetClosedSalesByClient(clientId)).Where(c => c.PK_AvailableUnits_Id == AvailableId).FirstOrDefault().FK_AvaliableUnits_Transaction_TransactionId;
                trans = await _transService.FindByID(transId);
            }
            //this's for Villas
            else if (catId == Categories.Villas)
            {
                var transId = (await _villasAvailableService.GetClosedSales(clientId)).Where(c => c.PK_VillasAvailables_Id == AvailableId).FirstOrDefault().FK_VillasAvailables_Transactions_Id;
                trans = await _transService.FindByID(transId);
            }
            //this's for Land
            else if (catId == Categories.Lands)
            {
                var transId = (await _availableLandService.GetClosedLandAvailable(clientId)).Where(c => c.PK_AvailableLands_Id == AvailableId).FirstOrDefault().FK_AvaliableLands_Transactions_TransactionId;
                trans = await _transService.FindByID(transId);
            }
            else if (catId == Categories.Shops)
            {
                var transId = (await _shopAvailableService.GetClosedShopAvailable(clientId)).Where(c => c.PK_ShopAvailable_Id == AvailableId).FirstOrDefault().FK_ShopAvailable_Transactions_Id;
                trans = await _transService.FindByID(transId);
            }
            return trans;
        }

        [NonAction]
        public async Task<bool> UpdateAvailableTransaction(int catId, int AvailableId)
        {
            var updatedAvailable = false;
            if (catId == Categories.Apartements)
            {
                updatedAvailable = await _availableService.UpdateAvailableTransaction(AvailableId, TranscatTypes.Sale);
            }
            //this's for Villas
            else if (catId == Categories.Villas)
            {
                updatedAvailable = await _villasAvailableService.UpdateAvailableTransaction(AvailableId, TranscatTypes.Sale);
            }
            else if (catId == Categories.Lands)
            {
                updatedAvailable = await _availableLandService.UpdateLandAvailableTransaction(AvailableId, TranscatTypes.Sale);
            }
            else if (catId == Categories.Shops)
            {
                updatedAvailable = await _shopAvailableService.UpdateShopAvailableTransaction(AvailableId, TranscatTypes.Sale);
            }
            return updatedAvailable;
        }

        [NonAction]
        public async Task<bool> UpdateDemandTransaction(int catId, int previewId)
        {
            var updatedDemand = false;
            if (catId == Categories.Apartements)
            {
                updatedDemand = await _demandService.UpdateDemandTransaction((await _previewService.FindById(previewId)).DemandUnit_Id, TranscatTypes.Sale);
            }
            //this's for Villas
            else if (catId == Categories.Villas)
            {
                updatedDemand = await _villasDemandService.UpdateDemandTransaction((await _previewService.FindById(previewId)).DemandUnit_Id, TranscatTypes.Sale);
            }
            //this's for Land
            else if (catId == Categories.Lands)
            {
                updatedDemand = await _landDemandService.UpdateLandDemandTransaction((await _previewService.FindById(previewId)).DemandUnit_Id, TranscatTypes.Sale);
            }
            //this's for Shop
            else if (catId == Categories.Shops)
            {
                updatedDemand = await _shopDemandService.UpdateDemandTransaction((await _previewService.FindById(previewId)).DemandUnit_Id, TranscatTypes.Sale);
            }
            return updatedDemand;
        }

        [NonAction]
        public async Task<bool> CloseAvailable(int availableId, int catId, int userId)
        {
            var closeAvailable = false;
            if (catId == Categories.Apartements)
            {
                closeAvailable = await _availableService.CloseAvailable(availableId, userId);
            }
            //this's for Villas
            else if (catId == Categories.Villas)
            {
                closeAvailable = await _villasAvailableService.CloseAvailable(availableId, userId);
            }
            //this's for Land
            else if (catId == Categories.Lands)
            {
                closeAvailable = await _availableLandService.CloseLandAvailable(availableId, userId);
            }
            //this's for Shop
            else if (catId == Categories.Shops)
            {
                return await _shopAvailableService.CloseShopAvailable(availableId, userId);
            }
            return closeAvailable;
        }
        // function for Release Available
        [NonAction]
        public async Task<bool> ReleaseAvailable(int availableId, int catId, int userId)
        {
            var ReleaseAvailable = false;
            if (catId == Categories.Apartements)
            {
                ReleaseAvailable = await _availableService.ReleaseAvailable(availableId, userId);
            }
            //this's for Villas
            else if (catId == Categories.Villas)
            {
                ReleaseAvailable = await _villasAvailableService.ReleaseAvailable(availableId, userId);
            }
            //this's for Land
            else if (catId == Categories.Lands)
            {
                ReleaseAvailable = await _availableLandService.ReleaseLandAvailable(availableId, userId);
            }
            //this's for Shop
            else if (catId == Categories.Shops)
            {
                return await _shopAvailableService.ReleaseShopAvailable(availableId, userId);
            }
            return ReleaseAvailable;
        }

        [NonAction]
        public async Task<bool> CloseDemand(int demandId, int catId, int userId)
        {
            var closeDemand = false;
            if (catId == Categories.Apartements)
            {
                closeDemand = await _demandService.CloseDemand(demandId, userId);
            }
            //this's for Villas
            else if (catId == Categories.Villas)
            {
                closeDemand = await _villasDemandService.CloseDemand(demandId, userId);
            }
            //this's for Land
            else if (catId == Categories.Lands)
            {
                closeDemand = await _landDemandService.CloseLandDemand(demandId, userId);
            }
            //this's for Shop
            else if (catId == Categories.Shops)
            {
                return await _shopDemandService.CloseDemand(demandId, userId);//// for hosseny you need to create CloseAvailable on your Shop service please 
            }
            return closeDemand;
        }
        // function for Release Available
        [NonAction]
        public async Task<bool> ReleaseDemand(int demandId, int catId, int userId)
        {
            var releaseDemand = false;
            if (catId == Categories.Apartements)
            {
                releaseDemand = await _demandService.ReleaseDemand(demandId, userId);
            }
            //this's for Villas
            else if (catId == Categories.Villas)
            {
                releaseDemand = await _villasDemandService.ReleaseDemand(demandId, userId);
            }
            //this's for Land
            else if (catId == Categories.Lands)
            {
                releaseDemand = await _landDemandService.ReleaseLandDemand(demandId, userId);
            }
            //this's for Shop
            else if (catId == Categories.Shops)
            {
                return await _shopDemandService.ReleaseDemand(demandId, userId);//// for hosseny you need to create ReleaseAvailable on your Shop service please 
            }
            return releaseDemand;
        }

        //[NonAction]
        //public int GetUsetId (int demandId, int catId)
        //{
        //    var userId = 0;
        //    if (catId == Categories.Apartements)
        //    {
        //        userId = _demandService.EditClientDemand(demandId).FK_DemandUnits_Users_CreatedBy;
        //    }
        //    //this's for Villas
        //    else if (catId == Categories.Villas)
        //    {
        //        userId = _villasDemandService.EditVillaDemand(demandId).FK_VillasDemands_Users_CreatedBy;
        //    }
        //    //this's for Land
        //    else if (catId == Categories.Lands)
        //    {
        //        userId = _landDemandService.EditLandDemand(demandId).FK_LandsDemands_Users_CreatedBy;
        //    }
        //    //this's for Shop
        //    else if (catId == Categories.Shops)
        //    {
        //        userId = _shopDemandService.EditShopDemand(demandId).FK_ShopDemands_Users_CreatedBy;
        //    }
        //    return userId;

        //}
    }


}
