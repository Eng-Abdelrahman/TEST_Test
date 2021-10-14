using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.BLL.Interfaces
{
    public interface ISpecialService
    {
        Task<List<SpecialDto>> GetSpecials(int deptId=0);
        Task<SpecialDto> FindByID(int id);
        Task<bool> UpdateSpecial(SpecialDto special, int userId);
        Task<bool> SaveSpecial(SpecialDto special, int userId);
        Task<bool> DeleteSpecial(int id, int userId);
        Task<SelectList> GetDepts();
        Task<SelectList> GetDeptsById(int deptId);
    }
}
