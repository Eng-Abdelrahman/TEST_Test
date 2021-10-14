using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.BLL.Interfaces
{
    public interface IFinancialService
    {
        Task<List<FinancialItemsDto>> GetFinancialItems(bool IsExpenses);
        Task<FinancialItemsDto> FindByID(int id);
        Task<bool> UpdateFinancialItem(FinancialItemsDto financialItem, int userId);
        Task<bool> SaveFinancialItem(FinancialItemsDto financialItem, int userId);
        Task<bool> DeleteFinancialItem(int id, int userId);        
    }
}
