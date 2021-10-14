using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IFinishService
    {
        Task<List<FinishingDto>> GetFinishings();
        Task<FinishingDto> FindByID(int id);
        Task<bool> UpdateFinish(FinishingDto Finish, int userId);
        Task<bool> SaveFinish(FinishingDto Finish, int userId);
        Task<bool> DeleteFinish(int id,int userId);
        Task<IConfirmation> CheckIsMaster(FinishingDto Finish);
    }
}
