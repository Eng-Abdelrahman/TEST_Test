using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Filters;
using _3aqarak.MVC.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace _3aqarak.MVC.Controllers
{

    public class HomeController : Controller
    {
        private readonly IUSerService _userService;
        private readonly IConfirmation _conf;
        private IReportServices _reportService;
        private IReportsViewModel _reportsVM;


        public HomeController(IUSerService userServive, IConfirmation conf, IReportServices reportService, IReportsViewModel reportsVM)
        {
            _userService = userServive;
            _conf = conf;
            _reportService = reportService;
            _reportsVM = reportsVM;
        }

        [AdminChartsFilter]
        public async Task<ActionResult> Index()
        {
            int month = System.DateTime.Now.Month;           
            _reportsVM.reportsDataRental = await _reportService.rentalEmpMonthlyAcheivement(month);
            _reportsVM.reportsTotalDataSales = await _reportService.TotalMonthlyContracts();
            _reportsVM.reportsDataSales = await _reportService.sellEmpMonthlyAcheivement(month);

            return View(_reportsVM);
        }

        public async Task<ActionResult> EmpIndex()
        {
            var userId= ((UserDto)Session["User"]).PK_Users_Id;
            var empRentContracts = (List<int>)Session["EmpRentContracts"];
            var empSaleContracts = (List<int>)Session["EmpSaleContracts"];
            _reportsVM.EmpContracts = await _reportService.EmpContracts(userId,empRentContracts,empSaleContracts);
            return View(_reportsVM);
        }
        public ActionResult CustomErrorView()
        {
            ViewBag.Message = Session["message"] != null ? Session["message"].ToString() : "";
            Session["message"] = null;
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {


            return View();
        }
        [HttpPost]
        public void DeleteImagesBeforUnloadPage(string sessionName, string path)
        {

            if (Session[sessionName] != null)
            {
                List<string> paths = (List<string>)Session[sessionName];
                foreach (string imgpath in paths)
                {
                    string photo = Directory
                             .GetFiles(Server.MapPath(path), imgpath.Split('/')[3], SearchOption.AllDirectories)
                             .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                    }
                }

            }
        }
    }
}