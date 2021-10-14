using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Filters;
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
    public class FinancialItemsController : Controller
    {
        private readonly IFinancialService _financeService;    
        

        public FinancialItemsController(IFinancialService financeService)
        {
            _financeService = financeService;
        }

        public ActionResult Index()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            return View();
        }     

        public ActionResult FinancialItems()
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
            bool isExpenses = Request.Form.GetValues("isExpenses") != null ? Boolean.Parse(Request.Form.GetValues("isExpenses")[0]): true;
            DataTableViewModel tableData =await GetTableData(data,isExpenses);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.FinancialItems,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetTableData(DataTableViewModel tableData,bool isExpenses)
        {

            // Getting all entity data
            List<FinancialItemsViewModel> entityList = Mapper.Map<List<FinancialItemsDto>, List<FinancialItemsViewModel>>(await _financeService.GetFinancialItems(isExpenses));
            foreach (var item in entityList)
            {
                item.DateString = item.Date.ToShortDateString();
                item.Type = item.IsExpenses ? "مصــروفات" : "ايـــرادات";
            }
            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                entityList = entityList.Where(e => e.Description.Contains(tableData.SearchValue) || e.Date.ToShortDateString().Contains(tableData.SearchValue)).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    entityList = entityList.OrderBy(e => e.Date).ToList();
                }
                else
                {
                    entityList = entityList.OrderByDescending(e => e.Date).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = entityList.Count();

            //Paging
            entityList = entityList.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.FinancialItems = entityList;

            return tableData;
        }

        public ActionResult AddFinancialItem()
        {
            var financeVM = new FinancialItemsViewModel();
            return View(financeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveFinancialItem(FinancialItemsViewModel financeVM)
        {
            var valid = false;
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            if (ModelState.IsValid)
            {
                valid = await _financeService.SaveFinancialItem(Mapper.Map<FinancialItemsViewModel, FinancialItemsDto>(financeVM), userId);

            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditFinancialItem(string id)
        {
            var FinancialItemId = int.Parse(id);
            var financeVM = Mapper.Map<FinancialItemsDto, FinancialItemsViewModel>(await _financeService.FindByID(FinancialItemId));
            return View(financeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateFinancialItem(FinancialItemsViewModel financeVM)
        {
            var valid = false;
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            try
            {
                if (ModelState.IsValid)
                {

                    valid = await _financeService.UpdateFinancialItem(Mapper.Map<FinancialItemsViewModel, FinancialItemsDto>(financeVM), userId);

                }
            }
            catch (Exception e)
            {
                var x = e.Message;
            }
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteFinancialItem(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _financeService.DeleteFinancialItem(int.Parse(id), userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }
    }
}