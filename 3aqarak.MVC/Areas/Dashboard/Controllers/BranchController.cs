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
    public class BranchController : Controller
    {
        private readonly IBranchService _BranchService;
        private readonly IUSerService _userService;
        private IConfirmation _conf;


        public BranchController(IBranchService BranchService, IUSerService userService, IConfirmation conf)
        {
            _BranchService = BranchService;
            _userService = userService;
            _conf = conf;
        }

        public async Task<ActionResult> Branches()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var BranchVM = Mapper.Map<List<BranchDto>, List<BranchViewModel>>(await _BranchService.GetBranches());
            return View(BranchVM);
        }

        public ActionResult AddBranch()
        {
            var BranchVM = new BranchViewModel();
            return View(BranchVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveBranch(BranchViewModel branchVM)
        {
            if (branchVM.IsMainBranch)
            {
                _conf = await _BranchService.CheckIsMainBranch(Mapper.Map<BranchViewModel, BranchDto>(branchVM));
                if (!_conf.Valid)
                {
                    return Json(_conf, JsonRequestBehavior.AllowGet);
                }
            }
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                _conf.Valid = await _BranchService.SaveBranch(Mapper.Map<BranchViewModel, BranchDto>(branchVM), userId);
                if (_conf.Valid)
                {
                    _conf.Message = "تم الحفظ بنجاح!";
                }
                else
                {
                    _conf.Message = "لم يتم الحفظ بنجاح!";
                }
            }

            return Json(_conf, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditBranch(string id)
        {
            var BranchId = int.Parse(id);
            var BranchVM = Mapper.Map<BranchDto, BranchViewModel>(await _BranchService.FindByID(BranchId));
            var mgr = await(_userService.FindUserByID(BranchVM.FK_Branches_Users_MgrId));
            ViewBag.MgrName = mgr.FirstName+" "+mgr.LastName;
            return View(BranchVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateBranch(BranchViewModel branchVM)
        {
            if (branchVM.IsMainBranch)
            {
                _conf = await _BranchService.CheckIsMainBranch(Mapper.Map<BranchViewModel, BranchDto>(branchVM));
                if (!_conf.Valid)
                {
                    return Json(_conf, JsonRequestBehavior.AllowGet);
                }
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    _conf.Valid = await _BranchService.UpdateBranch(Mapper.Map<BranchViewModel, BranchDto>(branchVM), userId);
                    if (_conf.Valid)
                    {
                        _conf.Message = "تم الحفظ بنجاح!";
                    }
                    else
                    {
                        _conf.Message = "لم يتم الحفظ بنجاح!";
                    }

                }
            }

            catch (Exception)
            {
                throw;
            }
            return Json(_conf, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteBranch(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _BranchService.DeleteBranch(int.Parse(id), userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }


    }
}