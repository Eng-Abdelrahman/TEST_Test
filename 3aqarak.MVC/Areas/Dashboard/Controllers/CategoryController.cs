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
    public class CategoryController : Controller
    {
        private readonly ICatService _catService;

        public CategoryController(ICatService catService)
        {
            _catService = catService;
        }

        public async Task<ActionResult> Cats()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var catVM = Mapper.Map<List<CatDto>, List<CatViewModel>>(await _catService.GetCats());
            return View(catVM);
        }

        public ActionResult AddCat()
        {
            var catVM = new CatViewModel();
            return View(catVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveCat(CatViewModel catVM)
        {
            var valid = false;
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                valid = await _catService.SaveCat(Mapper.Map<CatViewModel, CatDto>(catVM), userId);

            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditCat(string id)
        {
            var CatId = int.Parse(id);
            var catVM = Mapper.Map<CatDto,CatViewModel>(await _catService.FindByID(CatId));
            return View(catVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateCat(CatViewModel catVM)
        {
            var valid = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    valid = await _catService.UpdateCat(Mapper.Map<CatViewModel, CatDto>(catVM), userId);

                }
            }

            catch(Exception e)
            {
                var x = e.Message;
            }
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCat(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _catService.DeleteCat(int.Parse(id),userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }
    }
}