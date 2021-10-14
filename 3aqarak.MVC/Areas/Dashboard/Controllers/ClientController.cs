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
using System.Web.Mvc;

namespace _3aqarak.MVC.Areas.Dashboard.Controllers
{
    [IsAdminFilter]
   
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private IConfirmation _conf;

        public ClientController(IClientService clientService, IConfirmation conf)
        {
            _clientService = clientService;
            _conf = conf;
        }
        // GET: Client
        public ActionResult Clients()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }         
            return View();
        }

        [HttpPost]
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
                data = tableData.Clients,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task< DataTableViewModel> GetTableData(DataTableViewModel tableData)
        {

            // Getting all entity data
            List<ClientViewModel> entityList = Mapper.Map<List<ClientDto>, List<ClientViewModel>>(await _clientService.GetClients());

            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                entityList = entityList.Where(e => e.Name.Contains(tableData.SearchValue) || e.Mobile.Contains(tableData.SearchValue)|| (!string.IsNullOrEmpty(e.Mobile2) && e.Mobile2.Contains(tableData.SearchValue))).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    entityList = entityList.OrderBy(e => e.Name).ToList();
                }
                else
                {
                    entityList = entityList.OrderByDescending(e => e.Name).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = entityList.Count();

            //Paging
            entityList = entityList.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.Clients = entityList;

            return tableData;
        }

        [ClientPageSourceFilter]
        public ActionResult AddClient()
        {
            var clientVM = new ClientViewModel();       
           
            ViewBag.RedirectTo = HttpContext.Items["RedirectUrl"]?.ToString();
            return View(clientVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveClient(ClientViewModel clientVM)
        {         

            if (ModelState.IsValid)
            {
                if (Session["clientImagePathList"] != null)
                {
                    var paths = (List<string>)Session["clientImagePathList"];
                    clientVM.IdUrlFront = paths[0];
                    clientVM.IdUrlBack = paths[1];
                    Session["clientImagePathList"] = null;
                }
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                _conf = await _clientService.SaveClient(Mapper.Map<ClientViewModel, ClientDto>(clientVM), userId);
            }

            return Json(_conf, JsonRequestBehavior.AllowGet);
        }
        [ClientPageSourceFilter]
        public async Task<ActionResult> EditClient(string id)
        {
            var clientId = int.Parse(id);
            var clientVM = Mapper.Map<ClientDto, ClientViewModel>(await _clientService.FindClientByID(clientId));           
            ViewBag.RedirectTo = HttpContext.Items["RedirectUrl"]?.ToString();
            return View(clientVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateClient(ClientViewModel clientVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Session["clientImagePathList"] != null)
                    {
                        var paths = (List<string>)Session["clientImagePathList"];
                        clientVM.IdUrlFront = paths[0];
                        clientVM.IdUrlBack = paths[1];
                        Session["clientImagePathList"] = null;
                    }
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    _conf.Valid = await _clientService.UpdateClient(Mapper.Map<ClientViewModel, ClientDto>(clientVM), userId);
                }             
            }
            catch (Exception)
            {

            }
            //System.Threading.Thread.Sleep(7000);
            return Json(_conf, JsonRequestBehavior.AllowGet);
        }
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteClient(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            _conf.Valid = await _clientService.DeleteClient(int.Parse(id),userId);         
            return Json(new { result = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);

        }

        #region Upload

        [HttpPost]
        public ActionResult UploadImages()
        {
            try
            {
                if (Session["clientImagePathList"] != null && ((List<string>)Session["clientImagePathList"]).Count > 0)
                {
                    var paths = (List<string>)Session["clientImagePathList"];
                    foreach (var path in paths)
                    {
                        var photo = Directory
                                 .GetFiles(Server.MapPath("/Assets/Img/Clients"), path.Split('/')[3], SearchOption.AllDirectories)
                                 .FirstOrDefault();
                        if (photo != null)
                        {
                            System.IO.File.Delete(photo);
                        }
                    }
                    Session["clientImagePathList"] = null;
                }
                // Get all files from Request object
                HttpFileCollectionBase files = Request.Files;

                //HttpPostedFileBase file = files[i];
                _conf = _clientService.SavePhotos(files);                
                Session["clientImagePathList"] = _conf.UrlList;            

            }
            catch (Exception)
            {
                _conf.Valid = false;
                _conf.Message = "حدث خطا ما عند حفظ الصور!";
                Session["clientImagePathList"] = _conf.UrlList;
            }
            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);


        }


        #endregion "Upload"


    }
}