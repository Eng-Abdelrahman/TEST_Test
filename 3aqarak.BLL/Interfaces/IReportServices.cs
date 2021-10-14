using _3aqarak.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IReportServices
    {
        Task<(IEnumerable<string>, IEnumerable<int>, IEnumerable<EmpGroupByCategory>)> rentalEmpMonthlyAcheivement(int month);
        Task<(IEnumerable<string>, IEnumerable<int>, IEnumerable<EmpGroupByCategory>)> sellEmpMonthlyAcheivement(int month);
        Task<IEnumerable<int>> MonthlySaleContracts();
        Task<IEnumerable<int>> MonthlyRentContracts();
        Task<IEnumerable<int>> TotalMonthlyContracts();
        Task<IEnumerable<int>> EmpContracts(int userId, List<int> rentals, List<int> sales);
        Task<IEnumerable<int>> EmpSaleContracts(int userId);
        Task<IEnumerable<int>> EmpRentContracts(int userId);
    }
}
