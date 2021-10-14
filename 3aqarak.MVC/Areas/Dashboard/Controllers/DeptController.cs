using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Filters;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using _3aqarak.MVC.Areas.Dashboard.ViewModels;

namespace _3aqarak.MVC.Areas.Dashboard.Controllers
{
    [IsAdminFilter]
    public class DeptController : Controller
    {
        // GET: Dashboard/Dept
        private readonly IDeptService _DeptService;
        private readonly IUSerService _userService;
        private IConfirmation _conf;

        public DeptController(IDeptService DeptService, IUSerService userService, IConfirmation conf)
        {
            _DeptService = DeptService;
            _userService = userService;
            _conf = conf;
        }

        public async Task<ActionResult> Depts()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var DeptVM = Mapper.Map<List<DeptDto>, List<DeptViewModel>>(await _DeptService.GetDepts());
            return View(DeptVM);
        }

        public ActionResult AddDept()
        {
            var DeptVM = new DeptViewModel();
            return View(DeptVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveDept(DeptViewModel deptVM)
        {
           // var valid = false;
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                _conf = await _DeptService.SaveDept(Mapper.Map<DeptViewModel, DeptDto>(deptVM), userId);

            }

            return Json(_conf, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditDept(string id)
        {
            var DeptId = int.Parse(id);
            var DeptVM = Mapper.Map<DeptDto, DeptViewModel>(await _DeptService.FindByID(DeptId));
            var mgr = (await _userService.FindUserByID(DeptVM.FK_Depts_Users_MgrId));
            ViewBag.MgrName = mgr.FirstName + " " + mgr.LastName;
            return View(DeptVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateDept(DeptViewModel deptVM)
        {
            //var valid = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    _conf = await _DeptService.UpdateDept(Mapper.Map<DeptViewModel, DeptDto>(deptVM), userId);

                }
            }

            catch (Exception )
            {
                throw;
            }
            return Json(_conf, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteDept(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _DeptService.DeleteDept(int.Parse(id), userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }
        

    }
}