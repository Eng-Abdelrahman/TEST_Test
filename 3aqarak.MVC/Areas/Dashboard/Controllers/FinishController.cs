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
    public class FinishController : Controller
    {
        // GET: Dashboard/Finish
        private readonly IFinishService _finishService;
        private  IConfirmation _conf;
        public FinishController(IFinishService finishService, IConfirmation conf)
        {
            _finishService = finishService;
            _conf = conf;
        }

        public async Task<ActionResult> Finishings()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var finVM = Mapper.Map<List<FinishingDto>, List<FinishViewModel>>(await _finishService.GetFinishings());
            return View(finVM);
        }

        public ActionResult AddFinish()
        {
            var finVM = new FinishViewModel();
            return View(finVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveFinish(FinishViewModel finVM)
        {
            if (finVM.IsMaster)
            {
                _conf = await _finishService.CheckIsMaster(Mapper.Map<FinishViewModel, FinishingDto>(finVM));
                if (!_conf.Valid)
                {
                    return Json(_conf, JsonRequestBehavior.AllowGet);
                } 
            }
          
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
               _conf.Valid = await _finishService.SaveFinish(Mapper.Map<FinishViewModel, FinishingDto>(finVM), userId);
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

        public async Task<ActionResult> EditFinish(string id)
        {
            var finishId = int.Parse(id);
            var finVM = Mapper.Map<FinishingDto, FinishViewModel>(await _finishService.FindByID(finishId));
            return View(finVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateFinish(FinishViewModel finVM)
        {
            if (finVM.IsMaster)
            {
                _conf = await _finishService.CheckIsMaster(Mapper.Map<FinishViewModel, FinishingDto>(finVM));
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
                    _conf.Valid = await _finishService.UpdateFinish(Mapper.Map<FinishViewModel, FinishingDto>(finVM), userId);
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
            catch (Exception e)
            {
                var x = e.Message;
            }
            return Json(_conf, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deletefinish(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _finishService.DeleteFinish(int.Parse(id),userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }
    }
}