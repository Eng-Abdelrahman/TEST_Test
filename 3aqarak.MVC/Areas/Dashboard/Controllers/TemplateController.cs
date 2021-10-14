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
    public class TemplateController : Controller
    {
        private readonly IStaticService _statService;

        public TemplateController(IStaticService statService)
        {
            _statService = statService;
        }

        public async Task<ActionResult> Templates()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            ViewBag.Cats = await _statService.GetCatList();
            ViewBag.Trans =await _statService.GetTransList();
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

            var catId = !string.IsNullOrEmpty(Request.Form.GetValues("catId").FirstOrDefault()) ? int.Parse(Request.Form.GetValues("catId").FirstOrDefault()) : 0;
            var transId =!string.IsNullOrEmpty(Request.Form.GetValues("transId").FirstOrDefault())?int.Parse(Request.Form.GetValues("transId").FirstOrDefault()):0;
           
            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;

            DataTableViewModel tableData =await  GetTableData(data,catId,transId);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.Templates,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DataTableViewModel> GetTableData(DataTableViewModel tableData,int catId,int transId)
        {
            List<StaticViewModel> entityList = new List<StaticViewModel>();
            // Getting all entity data
            if (catId>0 && transId>0)
            {
                entityList = Mapper.Map<List<StaticDto>, List<StaticViewModel>>((await _statService.GetTemplates()).Where(e=>e.FK_StaticContract_Categories_CatId==catId&&e.FK_StaticContract_Transaction_Transid==transId).ToList());

            }
            else
	        {
                 entityList = Mapper.Map<List<StaticDto>, List<StaticViewModel>>(await _statService.GetTemplates()); 
            }

            //total number of rows count     
            tableData.RecordsTotal = entityList.Count();

            //Paging
            entityList = entityList.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.Templates = entityList;

            return tableData;
        }

        public async Task<ActionResult> AddTemplate()
        {
            var tempVM = new StaticViewModel();
            tempVM.Cats = await _statService.GetCatList();
            tempVM.Trans =await  _statService.GetTransList();
            return View(tempVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTemplate(StaticViewModel tempVM)
        {
            var valid = false;
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                valid = await _statService.SaveTemplate(Mapper.Map<StaticViewModel, StaticDto>(tempVM), userId);

            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditTemplate(string id)
        {
            var templateId = int.Parse(id);
            var tempVM = Mapper.Map<StaticDto, StaticViewModel>(await _statService.FindTemplateByID(templateId));
            tempVM.Cats =await  _statService.GetCatListById(templateId);
            tempVM.Trans = await _statService.GetTransListById(templateId);
            return View(tempVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateTemplate(StaticViewModel tempVM)
        {
            var valid = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    valid = await _statService.UpdateTemplate(Mapper.Map<StaticViewModel, StaticDto>(tempVM), userId);

                }
            }

            catch (Exception )
            {
               
            }
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteTemplate(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _statService.DeleteTemplate(int.Parse(id),userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }
    }
}