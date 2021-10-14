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
    public class TransactionController : Controller
    {
        private readonly ITransService _transService;
        private  IConfirmation _conf;

        public TransactionController(ITransService transService,IConfirmation conf)
        {
            _transService = transService;
            _conf = conf;
        }

        public async Task<ActionResult> Trans()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var transVM = Mapper.Map<List<TransDto>, List<TransactionViewModel>>(await _transService.GetTrans());
            return View(transVM);
        }

        public ActionResult AddTrans()
        {
            var transVM = new TransactionViewModel();
            return View(transVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTrans(TransactionViewModel transVM)
        {            
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                _conf = await _transService.SaveTrans(Mapper.Map<TransactionViewModel, TransDto>(transVM), userId);

            }

            return Json(_conf, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditTrans(string id)
        {
            var TransId = int.Parse(id);
            var transVM = Mapper.Map<TransDto, TransactionViewModel>(await _transService.FindByID(TransId));
            return View(transVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateTrans(TransactionViewModel transVM)
        {
          
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    _conf = await _transService.UpdateTrans(Mapper.Map<TransactionViewModel, TransDto>(transVM), userId);

                }
            }
            catch (Exception)
            {
              
            }
            return Json(_conf, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteTrans(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _transService.DeleteTrans(int.Parse(id),userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }
    }
}