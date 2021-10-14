using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
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
    public class ExpectedContractController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IUSerService _userService;
        private readonly IexpectedContractService _expectedService;
        private readonly IPreviewService _previewService;
        private readonly IAvailableService _availableService;
        private readonly IDemandService _demandService;
        private readonly IVillasAvailablesService _villasAvailableService;
        private readonly IAvailableLandsSevice _availableLandService;
        private readonly IShopAvailableService _shopAvailableService;


        public ExpectedContractController(IClientService clientService,IShopAvailableService shopAvailable,IAvailableLandsSevice availableLand,IVillasAvailablesService villasAvailable, IUSerService userService, IDemandService demandService, IexpectedContractService expectedService, IPreviewService previewService, IAvailableService availableService)
        {
            _shopAvailableService = shopAvailable;
            _availableLandService = availableLand;
            _villasAvailableService = villasAvailable;
            _clientService = clientService;
            _userService = userService;
            _expectedService = expectedService;
            _previewService = previewService;
            _availableService = availableService;
            _demandService = demandService;
        }

        
        public async Task<ActionResult> Expectedcontracts()
        {              
            var expeccontractVM = new ExpectedContractViewModel()
            {
                Cats = await _expectedService.GetCats(),
            };
            return View(expeccontractVM);
        }

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
            if (!string.IsNullOrEmpty(Request.Form.GetValues("fromDate").FirstOrDefault()))
            {
                 fromDate = DateTime.Parse(Request.Form.GetValues("fromDate").FirstOrDefault());
            }
            else
            {
                fromDate = DateTime.UtcNow.AddMinutes(120);
            }

            if (!string.IsNullOrEmpty(Request.Form.GetValues("toDate").FirstOrDefault()))
            {
                 toDate = DateTime.Parse(Request.Form.GetValues("toDate").FirstOrDefault());
            }
            else
            {
                toDate = DateTime.UtcNow.AddMinutes(120);
            }
            var catId = !string.IsNullOrEmpty(Request.Form["catId"]) ? int.Parse(Request.Form["catId"]) : 0;
                       
            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await GetTableData(data,fromDate,toDate,catId);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.Expectedcontracts,
                dateProblem = false,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetTableData(DataTableViewModel tableData,DateTime fromDate,DateTime toDate,int catId)
        {

            // Getting all entity data
            List<ExpectedContractViewModel> entityList = Mapper.Map<List<ExpectedContractDto>, List<ExpectedContractViewModel>>(await _expectedService.GetExpectedContracts(fromDate,toDate,catId));
            string GetContractType(bool type)
            {
                if (type)
                { return "عقد إتفاق"; }
                return "إيصال عربون";

            }
            foreach (var item in entityList)
            {
                var buyer = (await _clientService.FindClientByID(item.FK_ExpectContract_Clients_Buyer)).Name;
                var seller = (await _clientService.FindClientByID(item.FK_ExpectContract_Clients_Seller)).Name;
                item.BuyerName = buyer;
                item.SellerName = seller;
                item.ContractType = GetContractType(item.IsContractAgreement);
                if (item.PostponeDateTime!=null)
                {
                    item.DateTimeString = item.PostponeDateTime.Value.ToString("dd/MM/yyy hh:mm tt");
                }
                else
                {
                    item.DateTimeString = item.ExpectDateTime.ToString("dd/MM/yyy hh:mm tt");
                }
                }
            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                entityList = entityList.Where(e => e.SellerName.Contains(tableData.SearchValue) || e.BuyerName.Contains(tableData.SearchValue)|| e.DateTimeString.Contains(tableData.SearchValue)).ToList();
            }

                
            
            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    entityList = entityList.OrderBy(e => e.PK_ExpectContracts_Id).ToList();
                }
                else
                {
                    entityList = entityList.OrderByDescending(e => e.PK_ExpectContracts_Id).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = entityList.Count();

            //Paging
            entityList = entityList.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.Expectedcontracts = entityList;

            return tableData;
        }

        [HttpGet]
        public async Task<ActionResult> AddExpectedContract(string seller,string buyer,string available,string preview,string detail ,string availableCat,string option)
        {
            var expectedVm = new ExpectedContractViewModel();
            expectedVm.CategoryId = int.Parse(availableCat);
            expectedVm.AvailableUnits_Id = int.Parse(available);
            expectedVm.FK_ExpectContract_Clients_Buyer = int.Parse(buyer);
            expectedVm.FK_ExpectContract_Clients_Seller = int.Parse(seller);
            expectedVm.FK_ExpectedContract_Preview_Id = int.Parse(preview);
            expectedVm.BuyerName = (await _clientService.FindClientByID(expectedVm.FK_ExpectContract_Clients_Buyer)).Name;
            expectedVm.SellerName = (await _clientService.FindClientByID(expectedVm.FK_ExpectContract_Clients_Seller)).Name;
            Session["detailId"] = int.Parse(detail);
            if (bool.Parse(option))

                return View("AddAgreementContract",expectedVm);
           
            return View(expectedVm);
        }
        //*****************************************add rule here********************************************************************\\
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveExpectedContract(ExpectedContractViewModel expectVM)
        {
            var valid = false;
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            try
            {
                if (ModelState.IsValid)
                {
                    //var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    var expectDto = Mapper.Map<ExpectedContractViewModel, ExpectedContractDto>(expectVM);
                    valid = await _expectedService.SaveExpectedContract(expectDto, userId);
                    if (valid)
                    {
                        var detailId = Convert.ToInt32(Session["detailId"]);
                        var modified = await _previewService.SetPreviewDetSatus(detailId, PreviewStatus.IsSucceded);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            //here the change
            if (expectVM.Deposit.HasValue)
            {

                await CloseAvailable(expectVM.AvailableUnits_Id, expectVM.CategoryId, userId);

            }
            return Json(valid,JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditExpectedContract(string id)
        {
            var ExpectedContractId = int.Parse(id);
            var expectVM = Mapper.Map<ExpectedContractDto, ExpectedContractViewModel>(await _expectedService.FindByID(ExpectedContractId));
            expectVM.BuyerName = (await _clientService.FindClientByID(expectVM.FK_ExpectContract_Clients_Buyer)).Name;
            expectVM.SellerName = (await _clientService.FindClientByID(expectVM.FK_ExpectContract_Clients_Seller)).Name;
            var option = expectVM.IsContractAgreement;

            if (option)
                return View("EditAgreementContract", expectVM);

            return View(expectVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateExpectedContract(ExpectedContractViewModel expectVM)
        {
            var valid = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    valid = await _expectedService.UpdateExpectedContract(Mapper.Map<ExpectedContractViewModel, ExpectedContractDto>(expectVM), userId);

                }
            }

            catch (Exception)
            {
                throw;
            }
            return Json(valid, JsonRequestBehavior.AllowGet);
        }
        //*****************************************add rule here********************************************************************\\
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteExpectedContract(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            //here the change
            var expected =await _expectedService.FindByID(int.Parse(id));
            var AvailableId = expected.AvailableUnits_Id;
            var valid = await _expectedService.DeleteExpectedContract(int.Parse(id), userId);
            //here the change
            if (valid)
            {
                await ReleaseAvailable(AvailableId, expected.CategoryId, userId);
            }
            return Json(valid, JsonRequestBehavior.AllowGet);

        }
        //*****************************************add rule here********************************************************************\\
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelExpectedContract(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            //here the change
            var expected =await _expectedService.FindByID(int.Parse(id));
            var AvailableId = expected.AvailableUnits_Id;

            var valid = await _expectedService.CancelExpectedContract(int.Parse(id), userId);
            //here the change
            if (valid)
            {
                await ReleaseAvailable(AvailableId, expected.CategoryId, userId);
            }


            return Json(valid, JsonRequestBehavior.AllowGet);


        }

        // function for close 
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
                return await _availableLandService.CloseLandAvailable(availableId, userId);
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
                return await _availableLandService.ReleaseLandAvailable(availableId, userId);
            }
            //this's for Shop
            else if (catId == Categories.Shops)
            {
                return await _shopAvailableService.ReleaseShopAvailable(availableId, userId);
            }
            return ReleaseAvailable;
        }


    }
}