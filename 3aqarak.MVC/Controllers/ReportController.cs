using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using System.Collections.Generic;
using System.Web.Mvc;

namespace _3aqarak.MVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly IUSerService _userService;
        private readonly IConfirmation _conf;
        private IReportServices _reportService;
        private readonly SelectList monthsDrpDown;


        public ReportController(IUSerService userServive, IConfirmation conf, IReportServices reportService)
        {
            _userService = userServive;
            _conf = conf;
            _reportService = reportService;
            var months = new[]
            {
                new { Text = "يناير", Value = "1" },
                new { Text = "فبراير", Value = "2" },
                new { Text = "مارس", Value = "3" },
                new { Text = "ابريل", Value = "4" },
                new { Text = "مايو", Value = "5" },
                new { Text = "يونيو", Value = "6" },
                new { Text = "يوليو", Value = "7" },
                new { Text = "اغسطس", Value = "8" },
                new { Text = "سبتمبر", Value = "9" },
                new { Text = "اكتوبر", Value = "10" } ,
                new { Text = "نوفمبر", Value = "11" },
                new { Text = "ديسمبر", Value = "12" }
            };

            monthsDrpDown = new SelectList(months, "Value", "Text", months[0]);
        }
        // GET: Report
        public ActionResult ReportsIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult RentalEmpMonthAcheivementPage()
        {
            ViewBag.Months = monthsDrpDown;
            return View();
        }
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> GetRentalEmpMonthAcheivement(int month)
        {
            (IEnumerable<string>, IEnumerable<int>, IEnumerable<EmpGroupByCategory>) reportsData = await _reportService.rentalEmpMonthlyAcheivement(month);

            return Json(new { emps = reportsData.Item1, pctgs = reportsData.Item2, empCatGroups = reportsData.Item3 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SellEmpMonthAcheivementPage()
        {
            ViewBag.Months = monthsDrpDown;
            return View();
        }
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> GetSellEmpMonthAcheivement(int month)
        {
            (IEnumerable<string>, IEnumerable<int>, IEnumerable<EmpGroupByCategory>) reportsData = await _reportService.sellEmpMonthlyAcheivement(month);

            return Json(new { emps = reportsData.Item1, pctgs = reportsData.Item2, empCatGroups = reportsData.Item3 }, JsonRequestBehavior.AllowGet);
        }

        public async System.Threading.Tasks.Task<ActionResult> GetMonthlySaleContracts()
        {
            IEnumerable<int> monthlyAcheivement = await _reportService.MonthlySaleContracts();

            return Json(new { monthlyAcheivement }, JsonRequestBehavior.AllowGet);
        }

        public async System.Threading.Tasks.Task<ActionResult> GetMonthlyRentContracts()
        {
            IEnumerable<int> monthlyAcheivement = await _reportService.MonthlyRentContracts();
            return Json(new { monthlyAcheivement }, JsonRequestBehavior.AllowGet);
        }

        public async System.Threading.Tasks.Task<ActionResult> GetMonthlyTotalContracts()
        {
            IEnumerable<int> monthlyAcheivement = await _reportService.TotalMonthlyContracts();
            return Json(new { monthlyAcheivement }, JsonRequestBehavior.AllowGet);
        }

        public async System.Threading.Tasks.Task<ActionResult> GetEmpContracts()
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            List<int> empRentContracts = (List<int>)Session["EmpRentContracts"];
            List<int> empSaleContracts = (List<int>)Session["EmpSaleContracts"];
            IEnumerable<int> monthlyAcheivement = await _reportService.EmpContracts(userId, empRentContracts, empSaleContracts);
            return Json(new { monthlyAcheivement }, JsonRequestBehavior.AllowGet);

        }

        public async System.Threading.Tasks.Task<ActionResult> GetEmpRentContracts()
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;            
            IEnumerable<int> monthlyAcheivement = await _reportService.EmpRentContracts(userId);
            Session["EmpRentContracts"] = monthlyAcheivement;
            return Json(new { monthlyAcheivement }, JsonRequestBehavior.AllowGet);
        }

        public async System.Threading.Tasks.Task<ActionResult> GetEmpSaleContracts()
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            IEnumerable<int> monthlyAcheivement = await _reportService.EmpRentContracts(userId);
            Session["EmpSaleContracts"] = monthlyAcheivement;
            return Json(new { monthlyAcheivement }, JsonRequestBehavior.AllowGet);
        }

    }
}