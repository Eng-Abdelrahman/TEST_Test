using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3aqarak.BLL.Dto;
using AutoMapper;
using _3aqarak.BLL.Models;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using _3aqarak.BLL.Helpers;
using _3aqarak.Security;

namespace _3aqarak.BLL.Services
{   

    public class UserService : IUSerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpServerUtilityBase _server;
        private readonly HttpSessionStateBase _session;
        private readonly HttpResponseBase _response;




        public UserService(IUnitOfWork uow, IConfirmation conf, HttpServerUtilityBase server, HttpSessionStateBase session, HttpResponseBase response)
        {
            _uow = uow;
            _conf = conf;
            _server = server;
            _session = session;
            _response = response;
        }

        public async Task<UserDto> FindUserByID(int id)
        {
            var user = (await _uow.UserRepo.FindAsync(u => u.PK_Users_Id == id)).FirstOrDefault();
            return (user != null) ? Mapper.Map<tbl_Users, UserDto>(user) : new UserDto();
        }
        /// <summary>
        /// New Function without Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserDto FindUserByIDWithoutAsync(int id)
        {
            var user =  _uow.UserCustRepoWithoutAsync.Find(u => u.PK_Users_Id == id).FirstOrDefault();
            return (user != null) ? Mapper.Map<tbl_Users, UserDto>(user) : new UserDto();
        }
        //new one from Mostafa
        public async Task<SelectList> GetAllUsers()
        {
            return new SelectList(Mapper.Map<List<tbl_Users>, List<UserDto>>((await _uow.UserRepo.FindAsync(v => v.IsActive == true)).ToList()), "PK_Users_Id", "FullName");
        }
        public async Task<List<UserDto>> UserAutoComplete(string text)
        {
            return Mapper.Map<List<tbl_Users>, List<UserDto>>((await _uow.UserRepo.FindAsync(c => (c.FirstName.ToLower().Contains(text.ToLower()) || c.UserName.Contains(text)) && c.IsActive)).Take(10).ToList());
        }
        //********************end here*********
        public async Task<List<UserDto>> GetUsers()
        {
            var users = (await _uow.UserRepo.FindAsync(u => u.IsActive)).ToList();
            if (users.Any() && users != null)
            {
                return Mapper.Map<List<tbl_Users>, List<UserDto>>(users);

            }
            return new List<UserDto>();
        }

        public async Task<bool> SaveUser(UserDto user, int userId)
        {
            if (user.PK_Users_Id == 0)
            {
                user.IsActive = true;
                var newUser = Mapper.Map<UserDto, tbl_Users>(user);
                newUser.CreatedBy = userId;
                newUser.ModifiedBy = userId;
                _uow.UserRepo.Add(newUser);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> DeleteUser(int id, int userId)
        {
            var DBuser = (await _uow.UserRepo.FindAsync(u => u.PK_Users_Id == id)).FirstOrDefault();
            if (DBuser != null)
            {
                DBuser.IsActive = false;
                _uow.UserRepo.Update(DBuser);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateUser(UserDto user, int userId)
        {
            var DBuser = (await _uow.UserRepo.FindAsync(u => u.PK_Users_Id == user.PK_Users_Id)).FirstOrDefault();
            if (DBuser != null)
            {
                DBuser.ModifiedBy = userId;
                DBuser.FirstName = user.FirstName;
                DBuser.LastName = user.LastName;
                DBuser.UserName = user.UserName;
                DBuser.IsAdmin = user.IsAdmin;
                DBuser.IsOwner = user.IsOwner;
                DBuser.PhoneNumber = user.PhoneNumber;
                if (user.Specialization_Id.HasValue)
                {
                    DBuser.Specialization_Id = user.Specialization_Id.Value;
                }
                else
                {
                    DBuser.Specialization_Id = null;
                }
                if (!string.IsNullOrEmpty(user.PhotoUrl))
                {
                    DBuser.PhotoUrl = user.PhotoUrl;
                }
                if (!string.IsNullOrEmpty(user.Password) || !string.IsNullOrEmpty(user.Salt))
                {
                    DBuser.Password = user.Password;
                    DBuser.Salt = user.Salt;
                }
                DBuser.FK_Users_Genders_Id = user.FK_Users_Genders_Id;
                DBuser.FK_Users_Branches_Id = user.FK_Users_Branches_Id;
                DBuser.FK_Users_Departement_Id = user.FK_Users_Departement_Id;
                DBuser.Email = user.Email;
                DBuser.DateOfBirth = user.DateOfBirth;
                _uow.UserRepo.Update(DBuser);

            }

            return await _uow.SaveAsync() > 0;
        }

        public async Task<SelectList> GetGenderList()
        {
            return new SelectList(Mapper.Map<List<tbl_Genders>, List<GenderDto>>((await _uow.GenderRepo.GetAllAsync()).ToList()), "PK_Genders_Id", "Name");
        }

        public async Task<SelectList> GetGenderListById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Genders>, List<GenderDto>>((await _uow.GenderRepo.GetAllAsync()).ToList()), "PK_Genders_Id", "Name", (await _uow.GenderRepo.FindAsync(u => u.PK_Genders_Id == id)));
        }

        public async Task<SelectList> GetDeptList()
        {
            return new SelectList(Mapper.Map<List<tbl_Departements>, List<DeptDto>>((await _uow.DeptRepo.FindAsync(d => d.IsDeleted == false)).ToList()), "PK_Departement_Id", "Name");
        }

        public async Task<SelectList> GetDeptListById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Departements>, List<DeptDto>>((await _uow.DeptRepo.FindAsync(d => d.IsDeleted == false)).ToList()), "PK_Departement_Id", "Name", (await _uow.DeptRepo.GetAllAsync()).Where(u => u.PK_Departement_Id == id));
        }

        public async Task<SelectList> GetBranchList()
        {
            return new SelectList(Mapper.Map<List<tbl_Branches>, List<BranchDto>>((await _uow.BranchRepo.FindAsync(b => !b.IsDeleted)).ToList()), "PK_Branch_Id", "Name");
        }

        public async Task<SelectList> GetBranchListById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Branches>, List<BranchDto>>((await _uow.BranchRepo.GetAllAsync()).Where(b => !b.IsDeleted).ToList()), "PK_Branch_Id", "Name", (await _uow.BranchRepo.GetAllAsync()).Where(u => u.PK_Branch_Id == id));
        }

        public IConfirmation SavePhoto(HttpPostedFileBase file)
        {


            var conf = ValidatePhoto(file);
            if (conf.Valid)
            {
                try
                {
                    var fname = Path.Combine("Assets/Img/Users/", Guid.NewGuid().ToString() + new FileInfo(file.FileName).Extension);
                    file.SaveAs(_server.MapPath("/" + fname));
                    _conf.Url = fname;
                    _conf.Message = "تم حفظ الصورة بنجاح!";
                    return _conf;
                }
                catch (Exception)
                {
                    _conf.Valid = false;
                    _conf.Message = "حدث خطا ما عند حفظ الصورة!";
                }
            }

            return _conf;
        }

        public IConfirmation ValidatePhoto(HttpPostedFileBase file)
        {
            if (file.ContentLength > (3 * (1024 * 1024)))
            {
                _conf.Message = "Not allowed to upload files over 3 MB";
                _conf.Valid = false;

            }
            var ext = file.ContentType.Split('/')[1].ToLower();
            if (!ext.Equals("jpeg", StringComparison.OrdinalIgnoreCase) &&
                !ext.Equals("png", StringComparison.OrdinalIgnoreCase) &&
                 !ext.Equals("jpg", StringComparison.OrdinalIgnoreCase))

            {
                _conf.Message = "only allowed file types:.png,.jpg,.jpeg";
                _conf.Valid = false;
            }
            else
            {
                _conf.Valid = true;
            }
            return _conf;
        }

        public IConfirmation ChangePassword(string password, string confirmation)
        {
            Regex Pattern = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$");
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    _conf.Valid = false;
                    _conf.Message = "لابد من ادخال كلمة السر!";
                }
                else if (password != confirmation)
                {
                    _conf.Valid = false;
                    _conf.Message = "كلمة السر وتاكيدها لابد ان يتطابقا!";
                }
                else if (!Pattern.IsMatch(password))
                {
                    _conf.Valid = false;
                    _conf.Message = "Password must contain at least 6 characters, including UPPER/lowercase and numbers.";
                }
                else
                {
                    _conf.Valid = true;
                    _conf.Message = "من فضلك قم بحفظ بيانات المستخدم لكي يتم تاكيد حفظ كلمة السر!";
                    var Salt = Security.Security.GetRandomKey(64);
                    var EncrypPass = Security.Security.Encrypt(password, Salt);
                    _conf.Password = EncrypPass;
                    _conf.Salt = Salt;
                }
            }
            catch (Exception)
            {
                _conf.Valid = false;
                _conf.Message = "حدث خطا ما !";
            }

            return _conf;
        }

        public async Task<bool> CheckEmail(string mail, int id)
        {
            if (!string.IsNullOrEmpty(mail) && id > 0)
            {
                var user = (await _uow.UserRepo.FindAsync(u => u.Email == mail)).FirstOrDefault();

                return (user != null && user.PK_Users_Id != id) ? true : false;
            }
            else if (!string.IsNullOrEmpty(mail) && id == 0)
            {
                var user = (await _uow.UserRepo.FindAsync(u => u.Email == mail)).FirstOrDefault();

                return (user != null) ? true : false;
            }

            return false;
        }

        public async Task<bool> CheckUserName(string userName, int id)
        {
            if (!string.IsNullOrEmpty(userName) && id > 0)
            {
                var user = (await _uow.UserRepo.FindAsync(u => u.UserName == userName && u.IsActive == true)).FirstOrDefault();

                return (user != null && user.PK_Users_Id != id) ? true : false;
            }
            else if (!string.IsNullOrEmpty(userName) && id == 0)
            {

                    var user = (await _uow.UserRepo.FindAsync(u => u.UserName == userName && u.IsActive == true)).FirstOrDefault();
                    return (user != null) ? true : false;

            }
            return false;
        }

        public async Task<IConfirmation> Login(string userName, string password, bool? rememberMe)
        {
            var users = (await _uow.UserRepo.FindAsync(u => u.IsActive && u.UserName == userName));
            //1. Check for email.
            if (users.Any())
            {
                //var testedUser = _uow.UserRepo.Reload(Mapper.Map<UserDto, tbl_Users>(users.FirstOrDefault()));
                var testedUser = await _uow.UserRepo.ReloadAsync(users.FirstOrDefault());
                //2. Check for lock.
                if (testedUser.IsLocked)
                {
                    if (testedUser.LockEnd < DateTime.Now)
                    {
                        testedUser.IsLocked = false;
                        _uow.UserRepo.Update(testedUser);
                        await _uow.SaveAsync();
                    }
                    else
                    {
                        _conf.Message = "Your Acccount Has Been Locked For Five Minutes , Try Logging in after that.";
                        _conf.LoginStatus = Status.Locked;
                        return _conf;
                    }
                }

                if (!testedUser.IsLocked)
                {
                    //3. Check for password.
                    string decryptedPassword = Security.Security.Decrypt(testedUser.Password, testedUser.Salt);
                    if (decryptedPassword == password)
                    {
                        // Reset error message.                  

                        //Reset invalid attempts.
                        if (testedUser.InvalidAttempts > 0)
                        {
                            testedUser.InvalidAttempts = 0;
                            _uow.UserRepo.Update(testedUser);
                            await _uow.SaveAsync();
                        }
                        //SavePortofolio user data in the current session.
                        _session.Add("User", Mapper.Map<tbl_Users, UserDto>(testedUser));

                        //Check if user want a remember me cookie.
                        if (rememberMe == true)
                        {
                            testedUser.RememberToken = Security.Security.GetRandomKey(64);
                            _uow.UserRepo.Update(testedUser);
                            await _uow.SaveAsync();

                            HttpCookie LoginCookie = new HttpCookie("Rck");
                            LoginCookie.Values.Add("Rtkn", testedUser.RememberToken);
                            LoginCookie.Expires = DateTime.Now.AddHours(4);
                            _response.Cookies.Set(LoginCookie);
                        }
                        _conf.Message = null;
                        _conf.LoginStatus = Status.Succeeded;
                        return _conf;
                    }
                    else
                    {
                        if (testedUser.InvalidAttempts < 3)
                        {
                            testedUser.InvalidAttempts += 1;
                            _uow.UserRepo.Update(testedUser);
                            await _uow.SaveAsync();
                            _conf.Message = "Incorrect Email or Password";
                            _conf.LoginStatus = Status.Failure;
                            return _conf;
                        }
                        else
                        {
                            testedUser.InvalidAttempts = 0;
                            testedUser.IsLocked = true;
                            testedUser.LockEnd = DateTime.Now.AddMinutes(5);
                            _uow.UserRepo.Update(testedUser);
                            await _uow.SaveAsync();
                            _conf.Message = "Your Acccount Has Been Locked For Five Minutes , Try Logging in after that.";
                            _conf.LoginStatus = Status.Locked;
                            return _conf;
                        }
                    }
                }
            }
            else //Check for email
            {
                _conf.Message = "Incorrect User or Password";
                _conf.LoginStatus = Status.Failure;

            }
            return _conf;
        }

        public async Task<UserDto> FindByRtkn(string rtkn)
        {
            return Mapper.Map<tbl_Users, UserDto>((await _uow.UserRepo.FindAsync(u => u.RememberToken == rtkn)).FirstOrDefault());
        }

        public UserDto FindByRtknWithoutAsync(string rtkn)
        {
            return Mapper.Map<tbl_Users, UserDto>( _uow.UserCustRepoWithoutAsync.Find(u => u.RememberToken == rtkn).FirstOrDefault());
        }
        public async Task<List<UserDto>> EmpAutoComplete(string text)
        {
            var user = (await _uow.UserRepo.FindAsync(u => (u.FirstName.ToLower().Contains(text.ToLower()) || u.LastName.ToLower().Contains(text.ToLower()) || u.PhoneNumber.ToLower().Contains(text.ToLower())) && u.IsActive)).Take(10).ToList();
            return Mapper.Map<List<tbl_Users>, List<UserDto>>(user);
        }

        public async Task<bool> UpdateUserProfile(UserDto user, int userId)
        {
            var DBuser = (await _uow.UserRepo.FindAsync(u => u.PK_Users_Id == user.PK_Users_Id)).FirstOrDefault();
            if (DBuser != null)
            {
                DBuser.ModifiedBy = userId;
                DBuser.FirstName = user.FirstName;
                DBuser.LastName = user.LastName;
                DBuser.UserName = user.UserName;
                DBuser.PhoneNumber = user.PhoneNumber;
                if (!string.IsNullOrEmpty(user.PhotoUrl))
                {
                    DBuser.PhotoUrl = user.PhotoUrl;
                }
                if (!string.IsNullOrEmpty(user.Password) || !string.IsNullOrEmpty(user.Salt))
                {
                    DBuser.Password = user.Password;
                    DBuser.Salt = user.Salt;
                }
                DBuser.FK_Users_Genders_Id = user.FK_Users_Genders_Id;
                DBuser.Email = user.Email;
                DBuser.DateOfBirth = user.DateOfBirth;
                _uow.UserRepo.Update(DBuser);

            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
