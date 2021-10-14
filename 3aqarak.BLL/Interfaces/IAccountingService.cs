using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IAccountingService
    {
        Task<ContractCommissionsDto> SetRentContractCommissions(RentHeaderDto contract);
        Task<ContractCommissionsDto> SetSalesContractCommissions(SaleHeaderDto contract);
        Task<List<ContractCommissionsDto>> GetContracts(DateTime from, DateTime to, bool iscalc, int type);
        Task<bool> SaveCompCommission(decimal amount, int contractId,int userId);
        Task<bool> SaveEmpsCommissions(CommPctgsDto comms,int userId);
        Task<bool> SentContarctAsCalculated(int contractId, int userId, string type);
    }
}
