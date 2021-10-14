using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class ProfitDto
    {
        public decimal ExpensesSummary { get; set; }
        public decimal IncomeSummary { get; set; }
        public decimal ProfitSummary { get; set; }
        public string Description { get; set; }
        public string StringDate { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public IEnumerable<ProfitDto> EmpCommissionsDetails { get; set; }
        public IEnumerable<ProfitDto> CompCommissionsDetails { get; set; }
        public IEnumerable<ProfitDto> ExpensesList { get; set; }
        public IEnumerable<ProfitDto> IncomeList { get; set; }
    }
}
