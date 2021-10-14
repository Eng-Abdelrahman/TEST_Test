using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IBasisService
    {
        Task<List<BasisDto>> GetBasis();
        Task<BasisDto> FindByID(int id);
        Task<bool> UpdateBasis(BasisDto Basis, int userId);
        Task<bool> SaveBasis(BasisDto Basis, int userId);
        Task<bool> DeleteBasis(int id, int userId);
    }
}
