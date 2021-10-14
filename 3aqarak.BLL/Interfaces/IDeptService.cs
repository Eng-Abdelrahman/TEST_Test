using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IDeptService
    {
        Task<List<DeptDto>> GetDepts();
        Task<DeptDto> FindByID(int id);
        Task<IConfirmation> UpdateDept(DeptDto Dept, int userId);
        Task<IConfirmation> SaveDept(DeptDto Dept, int userId);
        Task<bool> DeleteDept(int id, int userId);
    }
}
