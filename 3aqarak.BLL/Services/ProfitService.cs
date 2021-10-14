using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Services
{
    public class ProfitService : IProfitService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;

        public ProfitService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;

        }
        public  async Task<ProfitDto> GetProfitSummary(DateTime from, DateTime to)
        {           

            var profitSummary = new ProfitDto();
            var empCommissions = await _uow.EmpCommRepo.FindAsync(ec => (DbFunctions.TruncateTime(ec.Date) >= from&& DbFunctions.TruncateTime(ec.Date)<=to) && !ec.IsDeleted);
            var empCommSum = empCommissions != null ? empCommissions.Sum(ec => ec.CommValue) : 0;
            var financials = await _uow.FinancialRepo.FindAsync(f => (DbFunctions.TruncateTime(f.Date) >= from && DbFunctions.TruncateTime(f.Date) <= to) && !f.IsDeleted);
            var expensesSum = financials != null ? financials.Where(f => f.IsExpenses).Sum(e => e.Amount) : 0;
            profitSummary.ExpensesSummary = empCommSum + expensesSum;
            var compCommissions = await _uow.CompCommRepo.FindAsync(cc => (DbFunctions.TruncateTime(cc.Date) >= from && DbFunctions.TruncateTime(cc.Date) <= to) && !cc.IsDeleted);
            var compComms = compCommissions != null ? compCommissions.Sum(cc => cc.Amount) : 0;
            var incomSum = financials != null ? financials.Where(f => !f.IsExpenses).Sum(e => e.Amount) : 0;
            profitSummary.IncomeSummary = compComms + incomSum;
            profitSummary.ProfitSummary = profitSummary.IncomeSummary - profitSummary.ExpensesSummary;
            profitSummary.EmpCommissionsDetails = empCommissions != null ? empCommissions.Select(ec => new ProfitDto()
            {
                Amount = ec.CommValue,
                Description = "عمولة موظــف :"+ ec.tbl_Users.FirstName + " " + ec.tbl_Users.LastName,
                Name =ec.tbl_Users1.FirstName+" "+ec.tbl_Users1.LastName ,
                StringDate = ec.Date.Date.ToShortDateString(),
            }) : new List<ProfitDto>();

            profitSummary.CompCommissionsDetails = compCommissions != null ? compCommissions.Select(cc => new ProfitDto()
            {
                Amount = cc.Amount,
                Description = "عمولة مكتب",
                Name = cc.tbl_Users1.FirstName+" "+cc.tbl_Users1.LastName,
                StringDate = cc.Date.Date.ToShortDateString(),
            }) : new List<ProfitDto>();
            profitSummary.ExpensesList = financials != null ? financials.Where(f => f.IsExpenses).Select(f => new ProfitDto()
            {
                Amount = f.Amount,
                Description = f.Description,
                Name = String.Concat(f.tbl_Users1.FirstName," ",f.tbl_Users1.LastName ),
                StringDate = f.Date.Date.ToLongDateString(),
            }) : new List<ProfitDto>();
            profitSummary.IncomeList = financials != null ? financials.Where(f => !f.IsExpenses).Select(f => new ProfitDto()
            {
                Amount = f.Amount,
                Description =f.Description ,
                Name = String.Concat(f.tbl_Users1.FirstName, " ", f.tbl_Users1.LastName),
                StringDate = f.Date.Date.ToLongDateString(),
            }) : new List<ProfitDto>();           
            return profitSummary;


        }

     
        public async Task<bool> SaveProfitSummary(ProfitDto profitSummary)
        {
            var DBProfit = new tbl_FinancialSummaries()
            {
                Date = DateTime.UtcNow.AddMinutes(120),
                ExpensesSummary = profitSummary.ExpensesSummary,
                IncomeSummary = profitSummary.IncomeSummary,
                ProfitSummary = profitSummary.IncomeSummary,
            };
            _uow.FinancialSummaryRepo.Add(DBProfit);
           return  await _uow.SaveAsync()>0;

        }
    }
}
