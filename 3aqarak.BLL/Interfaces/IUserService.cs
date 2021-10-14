using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.BLL.Interfaces
{
    public interface IUSerService
    {
        IConfirmation ChangePassword(string password, string confirmation);
        Task<bool> CheckEmail(string mail, int id);
        Task<bool> CheckUserName(string userName, int id);
        Task<bool> DeleteUser(int id, int userId);
        Task<List<UserDto>> EmpAutoComplete(string text);
        Task<UserDto> FindByRtkn(string rtkn);
        Task<UserDto> FindUserByID(int id);
        Task<SelectList> GetBranchList();
        Task<SelectList> GetBranchListById(int id);
        Task<SelectList> GetDeptList();
        Task<SelectList> GetDeptListById(int id);
        Task<SelectList> GetGenderList();
        Task<SelectList> GetGenderListById(int id);
        Task<List<UserDto>> GetUsers();
        Task<IConfirmation> Login(string userName, string password, bool? rememberMe);
        IConfirmation SavePhoto(HttpPostedFileBase file);
        Task<bool> SaveUser(UserDto user, int userId);
        Task<bool> UpdateUser(UserDto user, int userId);
        Task<bool> UpdateUserProfile(UserDto user, int userId);
        IConfirmation ValidatePhoto(HttpPostedFileBase file);

        UserDto FindUserByIDWithoutAsync(int id);
        UserDto FindByRtknWithoutAsync(string rtkn);
        Task<SelectList> GetAllUsers();
        Task<List<UserDto>> UserAutoComplete(string text);

    }
}
