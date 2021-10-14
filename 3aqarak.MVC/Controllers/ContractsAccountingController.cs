using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Filters;
using _3aqarak.MVC.Helpers;
using _3aqarak.MVC.Structs;
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
    [IsBranchManager]
    public class ContractsAccountingController : Controller
    {
        private readonly IAccountingService _accountingService;
        private readonly IRentContractService _rentService;
        private readonly ISaleContractService _saleService;

        public ContractsAccountingController(IAccountingService accountingService, IRentContractService rentService, ISaleContractService saleService)
        {
            _accountingService = accountingService;
            _rentService = rentService;
            _saleService = saleService;
        }

        public ActionResult ContractCommissions()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoadContracts()
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

            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            bool isCalc = Request.Form.GetValues("isCalc") != null ? Boolean.Parse(Request.Form.GetValues("isCalc")[0]) : true;
            int transType = Request.Form.GetValues("type") != null ? int.Parse(Request.Form.GetValues("type")[0]) : 0;
            if (!string.IsNullOrEmpty(Request.Form.GetValues("fromDate").FirstOrDefault()))
            {
                fromDate = DateTime.Parse(Request.Form.GetValues("fromDate").FirstOrDefault());
            }
            else
            {
                fromDate = DateTime.Now.Date;
            }

            if (!string.IsNullOrEmpty(Request.Form.GetValues("toDate").FirstOrDefault()))
            {
                toDate = DateTime.Parse(Request.Form.GetValues("toDate").FirstOrDefault());
            }
            else
            {
                toDate = DateTime.Now.Date;
            }
            DataTableViewModel tableData = await GetContractData(data, isCalc, fromDate, toDate, transType);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.Contracts,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetContractData(DataTableViewModel tableData, bool isCalc, DateTime fromDate, DateTime toDate, int type)
        {

            // Getting all entity data
            List<ContractCommissionsViewModel> entityList = Mapper.Map<List<ContractCommissionsDto>, List<ContractCommissionsViewModel>>(await _accountingService.GetContracts(fromDate, toDate, isCalc, type));

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                entityList = entityList.Where(e => e.StringDate.Contains(tableData.SearchValue) || e.Type.Contains(tableData.SearchValue) || e.Calculated.Contains(tableData.SearchValue)).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    entityList = entityList.OrderBy(e => e.Calculated).ToList();
                }
                else
                {
                    entityList = entityList.OrderByDescending(e => e.ContractId).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = entityList.Count();

            //Paging
            entityList = entityList.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.Contracts = entityList;

            return tableData;
        }

        public async Task<ActionResult> SetContractCommmissions(string id, string code)
        {
            var model = new ContractCommissionsViewModel();
            if (int.Parse(code) == TranscatTypes.Rental)
            {
                var rentHeader =await _rentService.FindHeaderByID(int.Parse(id));
                model = Mapper.Map<ContractCommissionsDto, ContractCommissionsViewModel>(await _accountingService.SetRentContractCommissions(rentHeader));
                model.TransCode = TranscatTypes.Rental;
            }
            else
            {
                var saleHeader =await _saleService.FindHeaderByID(int.Parse(id));
                model = Mapper.Map<ContractCommissionsDto, ContractCommissionsViewModel>(await _accountingService.SetSalesContractCommissions(saleHeader));
                model.TransCode = TranscatTypes.Sale;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveContractCommmissions(CommissionsPercts comms)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var empsCommSaved = await _accountingService.SaveEmpsCommissions(Mapper.Map<CommissionsPercts, CommPctgsDto>(comms), userId);
            var compComm = comms.Commission - (comms.SalesComm + comms.TeleSalesComm + comms.MgrComm);
            var compCommSaved = empsCommSaved == true ? await _accountingService.SaveCompCommission(compComm, comms.ContractId,userId) : false;
            if (empsCommSaved && compCommSaved)
            {

                var contractModified = await _accountingService.SentContarctAsCalculated(comms.ContractId, userId, comms.Code);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet); ;
        }


    }

}