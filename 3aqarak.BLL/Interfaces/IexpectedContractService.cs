using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.BLL.Interfaces
{
    public interface IexpectedContractService
    {
        Task<List<ExpectedContractDto>> GetExpectedContracts(DateTime from, DateTime to,int catId);
        Task<ExpectedContractDto> FindByID(int id);
        Task<bool> UpdateExpectedContract(ExpectedContractDto ExpectedContract, int userId);
        Task<bool> SaveExpectedContract(ExpectedContractDto ExpectedContract, int userId);
        Task<bool> DeleteExpectedContract(int id, int userId);
        Task<bool> CancelExpectedContract(int id, int userId);
        Task<bool> CloseExpected(int id, int userId);
        Task<SelectList> GetCats();

    }
}
