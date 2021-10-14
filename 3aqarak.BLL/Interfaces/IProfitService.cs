using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IProfitService
    {
        Task<ProfitDto> GetProfitSummary(DateTime from,DateTime to);
        Task<bool> SaveProfitSummary(ProfitDto profitSummary);
    }
}
