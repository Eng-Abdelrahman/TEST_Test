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
    public class AdminController : Controller
    {
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly IClientService _clientService;
        private IConfirmation _conf;

        public AdminController(IUSerService userServive, IConfirmation conf, ISpecialService specialService,IClientService clientService)
        {
            _userService = userServive;
            _conf = conf;
            _specialService = specialService;
            _clientService = clientService;
        }

        public async Task<ActionResult> EmpAutoComplete(string text)
       {
            var users = Mapper.Map<List<UserDto>, List<UserViewModel>>(await _userService.EmpAutoComplete(text));
            return Json(users, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> ClientAutoComplete(string text)
        {
            var clients = Mapper.Map<List<ClientDto>, List<ClientViewModel>>(await _clientService.ClientAutoComplete(text));
            return Json(clients, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Users()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            var usersVM = Mapper.Map<List<UserDto>, List<UserViewModel>>(await _userService.GetUsers());
            return View(usersVM);
        }

        public async Task<ActionResult> GetSpecializations(int deptId)
        {
            var specials = await _specialService.GetSpecials(deptId);
            return Json(specials, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AddUser()
        {
            var userVM = new UserViewModel();
            userVM.PhotoUrl = "Assets/Img/Users/anime3.png";
            userVM.GenderList = await _userService.GetGenderList();
            userVM.BranchList = await _userService.GetBranchList();
            userVM.DeptList = await _userService.GetDeptList();
            userVM.SpecialList = new SelectList(await _specialService.GetSpecials(), "PK_Specialization_Id", "Name");

            return View(userVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveUser(UserViewModel userVM)
        {


            if (Session["Password"] == null && Session["Salt"] == null)
            {
                _conf.Valid = false;
                return Json(new { Confirm = _conf.Valid, password = false });
            }


            if (ModelState.IsValid)
            {
                if (Session["UsersImagePathList"] != null)
                {
                    userVM.PhotoUrl = Session["UsersImagePathList"].ToString();
                    Session["UsersImagePathList"] = null;
                }
                else
                {

                    userVM.PhotoUrl = "assets/img/Users/anime3.png";

                }

                if (Session["Password"] != null && Session["Salt"] != null)
                {
                    userVM.Password = Session["Password"].ToString();
                    userVM.Salt = Session["Salt"].ToString();
                    Session["Salt"] = null;
                    Session["Password"] = null;
                }
                var userId = ((UserDto)Session["User"]).PK_Users_Id;

                _conf.Valid = await _userService.SaveUser(Mapper.Map<UserViewModel, UserDto>(userVM), userId);
            }
            return Json(_conf, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditUser(string id)
        {
            var userId = int.Parse(id);
            var userVM = Mapper.Map<UserDto, UserViewModel>(await _userService.FindUserByID(userId));
            userVM.GenderList = await _userService.GetGenderListById(userVM.FK_Users_Genders_Id);
            userVM.BranchList = await _userService.GetBranchListById(userVM.FK_Users_Branches_Id);
            userVM.DeptList = await _userService.GetDeptListById(userVM.FK_Users_Departement_Id);
            if (userVM.Specialization_Id.HasValue)
            {
                var specials = await _specialService.GetSpecials(userVM.FK_Users_Departement_Id);
                userVM.SpecialList = new SelectList(specials, "PK_Specialization_Id", "Name", specials.Where(s => s.FK_Specialization_Dept_DeptId == userVM.FK_Users_Departement_Id));
            }

            return View(userVM);
        }

        public async Task<ActionResult> ViewProfile(string id)
        {
            var userId = int.Parse(id);
            var userVM = Mapper.Map<UserDto, ProfileViewModel>(await _userService.FindUserByID(userId));
            userVM.GenderList = await _userService.GetGenderListById(userVM.FK_Users_Genders_Id);
            return View(userVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUserProfile(ProfileViewModel userVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Session["UsersImagePathList"] != null)
                    {
                        userVM.PhotoUrl = Session["UsersImagePathList"].ToString();
                        Session["UsersImagePathList"] = null;
                    }

                    if (Session["Password"] != null && Session["Salt"] != null)
                    {
                        userVM.Password = Session["Password"].ToString();
                        userVM.Salt = Session["Salt"].ToString();
                        Session["Salt"] = null;
                        Session["Password"] = null;
                    }
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    _conf.Valid = await _userService.UpdateUserProfile(Mapper.Map<ProfileViewModel, UserDto>(userVM), userId);
                }
            }
            catch (Exception)
            {

            }
            return Json(_conf, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUser(UserViewModel userVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Session["UsersImagePathList"] != null)
                    {
                        userVM.PhotoUrl = Session["UsersImagePathList"].ToString();
                        Session["UsersImagePathList"] = null;
                    }

                    if (Session["Password"] != null && Session["Salt"] != null)
                    {
                        userVM.Password = Session["Password"].ToString();
                        userVM.Salt = Session["Salt"].ToString();
                        Session["Salt"] = null;
                        Session["Password"] = null;
                    }
                    var userId = ((UserDto)Session["User"]).PK_Users_Id;
                    _conf.Valid = await _userService.UpdateUser(Mapper.Map<UserViewModel, UserDto>(userVM), userId);
                }
            }
            catch (Exception)
            {

            }
            return Json(_conf, JsonRequestBehavior.AllowGet);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> CheckUserName(string userName, int id = 0)
        {
            var exists =await _userService.CheckUserName(userName, id);
            return Json(exists, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> CheckEmail(string email, int id = 0)
        {
            var exists =await _userService.CheckEmail(email, id);
            return Json(exists, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string password, string confirmation)
        {
            var conf = _userService.ChangePassword(password, confirmation);
            Session["Password"] = conf.Password;
            Session["Salt"] = conf.Salt;
            return Json(new { result = conf.Valid, message = conf.Message }, JsonRequestBehavior.AllowGet);
        }


        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            _conf.Valid = await _userService.DeleteUser(int.Parse(id), userId);

            if (_conf.Valid && userId == int.Parse(id))
            {
                return RedirectToAction("logout", "Account", new { area = "" });
            }
            return Json(new { result = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);


        }

        #region Upload

        [HttpPost]
        public ActionResult UploadImage()
        {
            try
            {
                if (Session["UsersImagePathList"] != null)
                {
                    var photo = Directory
                        .GetFiles(Server.MapPath("/Assets/Img/Users"), (Session["UsersImagePathList"]
                        .ToString()).Split('/')[3], SearchOption.AllDirectories)
                        .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                        Session["UsersImagePathList"] = null;
                    }
                }
                // Get all files from Request object
                HttpFileCollectionBase files = Request.Files;

                HttpPostedFileBase file = files[0];
                _conf = _userService.SavePhoto(file);
                Session["UsersImagePathList"] = _conf.Url;
            }

            catch 
            {

                _conf.Valid = false;
                _conf.Message = "حدث خطا ما عند حفظ الصورة!";
            }
            return Json(new { valid = _conf.Valid, message = _conf.Message }, JsonRequestBehavior.AllowGet);


        }


        #endregion "Upload"


    }
}