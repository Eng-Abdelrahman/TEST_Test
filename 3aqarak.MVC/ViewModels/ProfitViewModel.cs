
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class ProfitViewModel
    {
        public decimal ExpensesSummary { get; set; }
        public decimal IncomeSummary { get; set; }
        public decimal ProfitSummary { get; set; }
        public string Description { get; set; }
        public string StringDate { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public IEnumerable<ProfitViewModel> EmpCommissionsDetails { get; set; }
        public IEnumerable<ProfitViewModel> CompCommissionsDetails { get; set; }
        public IEnumerable<ProfitViewModel> ExpensesList { get; set; }
        public IEnumerable<ProfitViewModel> IncomeList { get; set; }
    }
}