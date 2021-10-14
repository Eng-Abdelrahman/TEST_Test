using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Filters;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Controllers
{
    [IsBranchManager]
    public class ProfitController : Controller
    {
        private readonly IProfitService _profitService;
        
        public ProfitController(IProfitService profitService)
        {
            _profitService = profitService;
        }
        // GET: Profit
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            DateTime from = DateTime.Today.Date;
            DateTime to = DateTime.Today.Date;
            var profSummary = await _profitService.GetProfitSummary(from,to);
            var prfitVM = Mapper.Map<ProfitDto, ProfitViewModel>(profSummary);
            var saved = await _profitService.SaveProfitSummary(profSummary);
            return View(prfitVM);
        }
        [HttpPost]
        public async Task<ActionResult> Index(DateTime? from,DateTime? to)
        {

            from = (from==null)? DateTime.Today.Date:from.Value;
            to = (to==null)?DateTime.Today.Date: to.Value;
            if (from>to)
            {
                ModelState.AddModelError(string.Empty, "تاريخ البدء لابد ان يكون اقل من تاريخ الانتهاء!");
                return View(new ProfitViewModel());

            }
            var profSummary = await _profitService.GetProfitSummary(from.Value, to.Value);
            var prfitVM = Mapper.Map<ProfitDto, ProfitViewModel>(profSummary);
            return View(prfitVM);
        }
    }
}