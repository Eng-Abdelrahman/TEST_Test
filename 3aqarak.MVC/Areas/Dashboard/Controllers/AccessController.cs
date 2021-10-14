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
    public class AccessController : Controller
    {
        private readonly IAccessService _accessService;

        public  AccessController(IAccessService accessService)
        {
            _accessService = accessService;
        }

        public async Task<ActionResult> Accessories()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var acessVM = Mapper.Map<List<AccessDto>, List<AccessViewModel>>(await _accessService.GetAccessoriesAsync());
            return View(acessVM);
        }

        public ActionResult AddAccess()
        {
            var acessVM = new AccessViewModel();        
            return View(acessVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAccess(AccessViewModel acessVM)
        {
            var valid = false;
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            if (ModelState.IsValid)
            {   
               valid = await _accessService.SaveAccess(Mapper.Map<AccessViewModel, AccessDto>(acessVM),userId);

            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditAccess(string id)
        {
            var AccessId = int.Parse(id);
            var acessVM = Mapper.Map<AccessDto, AccessViewModel>(await _accessService.FindByID(AccessId));          
            return View(acessVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateAccess(AccessViewModel acessVM)
        {
            var valid = false;
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            try
            {
                if (ModelState.IsValid)
                {                 

                   valid = await _accessService.UpdateAccess(Mapper.Map<AccessViewModel, AccessDto>(acessVM),userId);

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
        public async Task<ActionResult> DeleteAccess(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _accessService.DeleteAccess(int.Parse(id),userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }
    }
}