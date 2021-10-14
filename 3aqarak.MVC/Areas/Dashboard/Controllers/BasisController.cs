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
    public class BasisController : Controller
    {
        // GET: Dashboard/Basis
        private readonly IBasisService _BasisService;

        public BasisController(IBasisService BasisService)
        {
            _BasisService = BasisService;
        }

        public async Task<ActionResult> Basis()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var basisVM = Mapper.Map<List<BasisDto>, List<BasisViewModel>>(await _BasisService.GetBasis());
            return View(basisVM);
        }

        public ActionResult AddBasis()
        {
            var basisVM = new BasisViewModel();
            return View(basisVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveBasis(BasisViewModel basisVM)
        {
            var valid = false;
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                valid = await _BasisService.SaveBasis(Mapper.Map<BasisViewModel, BasisDto>(basisVM), userId);

            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditBasis(string id)
        {
            var BasisId = int.Parse(id);
            var basisVM = Mapper.Map<BasisDto, BasisViewModel>(await _BasisService.FindByID(BasisId));
            return View(basisVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateBasis(BasisViewModel basisVM)
        {
            var valid = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    valid = await _BasisService.UpdateBasis(Mapper.Map<BasisViewModel, BasisDto>(basisVM), userId);

                }
            }
            catch (Exception)
            {
               
            }
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteBasis(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _BasisService.DeleteBasis(int.Parse(id),userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }
    }
}