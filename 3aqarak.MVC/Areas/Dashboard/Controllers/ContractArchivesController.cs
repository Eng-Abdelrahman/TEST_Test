using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Areas.Dashboard.ViewModels;
using _3aqarak.MVC.Filters;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace _3aqarak.MVC.Areas.Dashboard.Controllers
{
    [IsAdminFilter]
    public class ContractArchivesController : Controller
    {
        private readonly IContractArchiveService _ContractArchiveService;
        private IConfirmation _conf;

        public ContractArchivesController(IContractArchiveService ContractArchiveService, IConfirmation conf)
        {
            _ContractArchiveService = ContractArchiveService;
            _conf = conf;
        }
        // GET: ContractArchives
        public ActionResult Index()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            return View();
        }

        public async Task<ActionResult> LoadData()
        {
            DataTableViewModel data = new DataTableViewModel
            {
                Draw = Request.Form.GetValues("draw").FirstOrDefault(),
                Start = Request.Form.GetValues("start").FirstOrDefault(),
                Length = Request.Form.GetValues("length").FirstOrDefault(),
                SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault(),
                SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault(),
                SearchValue = Request.Form.GetValues("search[value]").FirstOrDefault(),
            };

            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            DataTableViewModel tableData = await GetTableData(data);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.Contract,
            }, JsonRequestBehavior.AllowGet);
        }
        private async Task<DataTableViewModel> GetTableData(DataTableViewModel tableData)
        {

            // Getting all entity data
            List<ContractArchiveViewModel> entityList = Mapper.Map<List<ContractArchiveDto>, List<ContractArchiveViewModel>>(await _ContractArchiveService.GetContractArchives());

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                entityList = entityList.Where(e => e.ContractID.Contains(tableData.SearchValue)).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    entityList = entityList.OrderBy(e => e.ContractID).ToList();
                }
                else
                {
                    entityList = entityList.OrderByDescending(e => e.ContractID).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = entityList.Count();

            //Paging
            entityList = entityList.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.Contract = entityList;

            return tableData;
        }
        // ContractArchives/AddContract
        public ActionResult AddContract()
        {
            var ContractVM = new ContractArchiveViewModel();
            // ViewBag.RedirectTo = HttpContext.Items["RedirectUrl"]?.ToString();
            return View(ContractVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveContract(ContractArchiveViewModel contractVM)
        {
            if (Session["contractArchiveImagePathList"] == null)
            {
                _conf.Valid = false;
                _conf.Message = "PDF  لابد من تحميل  ملف!";
                return Json(_conf, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                if (Session["contractArchiveImagePathList"] != null)
                {
                    contractVM.ImageURL = ((List<string>)Session["contractArchiveImagePathList"])[0];

                    Session["contractArchiveImagePathList"] = null;
                }

                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                _conf.Valid = await _ContractArchiveService.SaveContractArchive(Mapper.Map<ContractArchiveViewModel, ContractArchiveDto>(contractVM), userId);
            }
            return Json(_conf, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> CheckContractId(string contract, int id = 0)
        {
            var exists =await _ContractArchiveService.CheckContractId(contract, id);
            return Json(exists, JsonRequestBehavior.AllowGet);
            //return Json(new { exists, message = "رقم هذا العقد موجود مسبقاً" }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> EditContract(string id)
        {
            var ContractId = int.Parse(id);
            var ContractVM = Mapper.Map<ContractArchiveDto, ContractArchiveViewModel>(await _ContractArchiveService.FindContractArchiveByID(ContractId));
            ViewBag.RedirectTo = HttpContext.Items["RedirectUrl"]?.ToString();
            return View(ContractVM);
        }
        // ContractArchives/UpdateContract
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateContract(ContractArchiveViewModel contractVM)
        {
            if (ModelState.IsValid)
            {
                if (Session["contractArchiveImagePathList"] != null)
                {
                    contractVM.ImageURL = ((List<string>)Session["contractArchiveImagePathList"])[0];
                    Session["contractArchiveImagePathList"] = null;
                }

                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                //var ContractId = ((ContractArchiveDto)Session["Contract"]).PK_ContractArchives_Id;
                _conf.Valid = await _ContractArchiveService.UpdateContractArchive(Mapper.Map<ContractArchiveViewModel, ContractArchiveDto>(contractVM), userId);

            }

            return Json(_conf, JsonRequestBehavior.AllowGet);
        }
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteContract(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            // var ContractId = ((ContractArchiveDto)Session["Contract"]).PK_ContractArchives_Id;
            _conf.Valid = await _ContractArchiveService.DeleteContractArchive(int.Parse(id), userId);
            return Json(new { result = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);

        }

        #region Upload

        [HttpPost]
        public ActionResult UploadImages()
        {
            try
            {
                if (Session["contractArchiveImagePathList"] != null && ((List<string>)Session["contractArchiveImagePathList"]).Count > 0)
                {
                    var paths = (List<string>)Session["contractArchiveImagePathList"];
                    foreach (var path in paths)
                    {
                        var photo = Directory
                                 .GetFiles(Server.MapPath("/Assets/ContractImage"), path.Split('/')[2], SearchOption.AllDirectories)
                                 .FirstOrDefault();
                        if (photo != null)
                        {
                            System.IO.File.Delete(photo);
                        }
                    }
                    Session["contractArchiveImagePathList"] = null;
                }
                // Get all files from Request object
                HttpFileCollectionBase files = Request.Files;


                //HttpPostedFileBase file = files[i];
                _conf = _ContractArchiveService.SavePhotos(files);
                Session["contractArchiveImagePathList"] = _conf.UrlList;

            }
            catch (Exception)
            {
                _conf.Valid = false;
                _conf.Message = "حدث خطا ما عند حفظ الصور!";
                Session["contractArchiveImagePathList"] = _conf.UrlList;
            }
            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);
        }
        #endregion "Upload"

        public async Task<ActionResult> GetPDF(int id)
        {
            if (Session["contractArchiveImagePathList"] == null)
            {
                var fileStream = new FileStream(HostingEnvironment.MapPath("~/" + (await _ContractArchiveService.FindContractArchiveByID(id)).ImageURL),
                                                 FileMode.Open,
                                                 FileAccess.Read
                                               );
                var fsResult = new FileStreamResult(fileStream, "application/pdf");
                return fsResult;
            }
            else
            {
                var pdfPath = ((List<string>)Session["contractArchiveImagePathList"])[0];
                var fileStream = new FileStream(HostingEnvironment.MapPath("~/" + pdfPath),
                                                 FileMode.Open,
                                                 FileAccess.Read
                                               );
                var fsResult = new FileStreamResult(fileStream, "application/pdf");
                return fsResult;
            }
        }
    }
}