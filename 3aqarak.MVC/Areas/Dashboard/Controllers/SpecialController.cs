using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Areas.Dashboard.ViewModels;
using _3aqarak.MVC.Filters;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Areas.Dashboard.Controllers
{
    [IsAdminFilter]
    public class SpecialController : Controller
    {
        // GET: Dashboard/Special
        private readonly ISpecialService _SpecialService;

        public SpecialController(ISpecialService SpecialService)
        {
            _SpecialService = SpecialService;
        }

        public async Task<ActionResult> Specials()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }         
            ViewBag.Depts =await _SpecialService.GetDepts();
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
            var deptId = !string.IsNullOrEmpty(Request.Form.GetValues("deptId").FirstOrDefault()) ? int.Parse(Request.Form.GetValues("deptId").FirstOrDefault()) : 0;
            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await  GetTableData(data,deptId);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.Specials,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetTableData(DataTableViewModel tableData,int deptId)
        {

            // Getting all entity data
            var entityList = Mapper.Map<List<SpecialDto>, List<SpecialViewModel>>(await _SpecialService.GetSpecials(deptId));

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                entityList = entityList.Where(e => e.Name.Contains(tableData.SearchValue) ).ToList();
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

            tableData.Specials = entityList;

            return tableData;
        }


        public async Task<ActionResult> AddSpecial()
        {
            var SpecialVM = new SpecialViewModel();
            SpecialVM.Depts =await  _SpecialService.GetDepts();
            return View(SpecialVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveSpecial(SpecialViewModel SpecialVM)
        {
            var valid = false;
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                valid = await _SpecialService.SaveSpecial(Mapper.Map<SpecialViewModel, SpecialDto>(SpecialVM), userId);

            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditSpecial(string id)
        {
            var SpecialId = int.Parse(id);
            var SpecialVM = Mapper.Map<SpecialDto, SpecialViewModel>(await _SpecialService.FindByID(SpecialId));
            SpecialVM.Depts = await _SpecialService.GetDeptsById(SpecialVM.FK_Specialization_Dept_DeptId);
            return View(SpecialVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateSpecial(SpecialViewModel SpecialVM)
        {
            var valid = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    valid = await _SpecialService.UpdateSpecial(Mapper.Map<SpecialViewModel, SpecialDto>(SpecialVM), userId);

                }
            }
            catch (Exception)
            {

            }
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteSpecial(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _SpecialService.DeleteSpecial(int.Parse(id), userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }
    }
}