using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IBranchService
    {
        Task<List<BranchDto>> GetBranches();
        Task<BranchDto> FindByID(int id);       
        Task<bool> UpdateBranch(BranchDto Branch, int userId);
        Task<bool> SaveBranch(BranchDto Branch, int userId);
        Task<bool> DeleteBranch(int id, int userId);
        Task<IConfirmation> CheckIsMainBranch(BranchDto branch);

        BranchDto FindByIDWithoutAsync(int id);
    }
}
