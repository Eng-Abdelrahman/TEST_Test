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
    public class RegionController : Controller
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        public async Task<ActionResult> Regions()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var regVM = Mapper.Map<List<RegionDto>, List<RegionViewModel>>(await _regionService.GetRegions());
            return View(regVM);
        }

        public ActionResult AddRegion()
        {
            var regVM = new RegionViewModel();
            return View(regVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveRegion(RegionViewModel regVM)
        {
            var valid = false;
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                valid = await _regionService.SaveRegion(Mapper.Map<RegionViewModel, RegionDto>(regVM), userId);

            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditRegion(string id)
        {
            var RegionId = int.Parse(id);
            var regVM = Mapper.Map<RegionDto, RegionViewModel>(await _regionService.FindByID(RegionId));
            return View(regVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateRegion(RegionViewModel regVM)
        {
            var valid = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    valid = await _regionService.UpdateRegion(Mapper.Map<RegionViewModel, RegionDto>(regVM), userId);

                }
            }
            catch (Exception )
            {
               
            }
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRegion(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _regionService.DeleteRegion(int.Parse(id),userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckRegCode(int code,int id=0)
        {
            var exist = _regionService.CheckRegCode(code,id);
            return Json(exist, JsonRequestBehavior.AllowGet);
        }
    }
}