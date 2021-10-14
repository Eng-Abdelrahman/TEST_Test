using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IAccessService
    {
        Task<List<AccessDto>> GetAccessoriesAsync();
        Task<AccessDto> FindByID(int id);
        Task<bool> UpdateAccess(AccessDto access, int userId);
        Task<bool> SaveAccess(AccessDto access, int userId);
        Task<bool> DeleteAccess(int id,int userId);
       
    }
}
