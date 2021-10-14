using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.BLL.Interfaces
{
    public interface ICommissionsService
    {
        Task<List<CommissionsDto>> GetCommissions();
        Task<CommissionsDto> FindByID(int id);
        Task<bool> UpdateCommission(CommissionsDto commission, int userId);
        Task<bool> SaveCommission(CommissionsDto commission, int userId);
        Task<bool> DeleteCommission(int id, int userId);
        Task<SelectList> GetCatList();
        Task<SelectList> GetCatListById(int id);
        Task<SelectList> GetTransList();
        Task<SelectList> GetTransListById(int id);
    }
}
