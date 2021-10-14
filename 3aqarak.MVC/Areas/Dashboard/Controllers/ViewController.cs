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
    public class ViewController : Controller
    {
        // GET: Dashboard/View
        private readonly IViewService _viewService;

        public ViewController(IViewService viewService)
        {
            _viewService = viewService;
        }

        public async Task< ActionResult> Views()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var viewVM = Mapper.Map<List<ViewDto>, List<ViewsViewModel>>(await _viewService.GetViews());
            return View(viewVM);
        }

        public ActionResult AddView()
        {
            var viewVM = new ViewsViewModel();
            return View(viewVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveView(ViewsViewModel viewVM)
        {
            var valid = false;
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                valid = await _viewService.SaveView(Mapper.Map<ViewsViewModel, ViewDto>(viewVM), userId);

            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditView(string id)
        {
            var viewId = int.Parse(id);
            var viewVM = Mapper.Map<ViewDto, ViewsViewModel>(await _viewService.FindByID(viewId));
            return View(viewVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateView(ViewsViewModel viewVM)
        {
            var valid = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    valid = await _viewService.UpdateView(Mapper.Map<ViewsViewModel, ViewDto>(viewVM), userId);

                }
            }
            catch (Exception e)
            {
                var x = e.Message;
            }
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deleteview(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _viewService.DeleteView(int.Parse(id), userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }
    }
}