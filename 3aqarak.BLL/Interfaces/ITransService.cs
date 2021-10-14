using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface ITransService
    {
        Task<bool> DeleteTrans(int id, int userId);
        Task<TransDto> FindByID(int id);
        Task<List<TransDto>> GetTrans();
        Task<IConfirmation> SaveTrans(TransDto Trans, int userId);
        Task<IConfirmation> UpdateTrans(TransDto Trans, int userId);
    }
}
