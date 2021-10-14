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
    public class CommissionController : Controller
    {
        private readonly ICommissionsService _commService;
        private readonly ITransService _transService;
        private readonly ICatService _catService;

        public CommissionController(ICommissionsService commService, ITransService transService, ICatService catService)
        {
            _commService = commService;
            _transService = transService;
            _catService = catService;
        }

        public async Task<ActionResult> Commissions()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var commVM = Mapper.Map<List<CommissionsDto>, List<CommissionsViewModel>>(await _commService.GetCommissions());
            foreach (var item in commVM)
            {
                item.Name = (await _catService.FindByID(item.FK_Commissions_Categories_Id)).CategoryName + " " + (await _transService.FindByID(item.FK_Commissions_Transactions_Id)).TransType;
            }
            return View(commVM);
        }

        public async Task<ActionResult> AddCommission()
        {
            var commVM = new CommissionsViewModel();
            commVM.Categories =await  _commService.GetCatList();
            commVM.Transactions =await  _commService.GetTransList();
            return View(commVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveCommission(CommissionsViewModel commVM)
        {
            var valid = false;
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                valid = await _commService.SaveCommission(Mapper.Map<CommissionsViewModel, CommissionsDto>(commVM), userId);
            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditCommission(string id)
        {
            var CommissionId = int.Parse(id);
            var commVM = Mapper.Map<CommissionsDto, CommissionsViewModel>(await _commService.FindByID(CommissionId));
            commVM.Categories = await _commService.GetCatListById(commVM.FK_Commissions_Categories_Id);
            commVM.Transactions = await _commService.GetTransListById(commVM.FK_Commissions_Transactions_Id);
            return View(commVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateCommission(CommissionsViewModel commVM)
        {
            var valid = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    valid = await _commService.UpdateCommission(Mapper.Map<CommissionsViewModel, CommissionsDto>(commVM), userId);

                }
            }

            catch (Exception)
            {
                throw;
            }
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCommission(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var valid = await _commService.DeleteCommission(int.Parse(id), userId);

            return Json(valid, JsonRequestBehavior.AllowGet);


        }
    }
}