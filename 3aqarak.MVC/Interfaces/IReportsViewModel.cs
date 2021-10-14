using _3aqarak.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.MVC.Interfaces
{
    public interface IReportsViewModel
    {
        (IEnumerable<string>, IEnumerable<int>, IEnumerable<EmpGroupByCategory>) reportsDataRental { get; set; }
        (IEnumerable<string>, IEnumerable<int>, IEnumerable<EmpGroupByCategory>) reportsDataSales { get; set; }
        IEnumerable<int> reportsTotalDataSales { get; set; }
        IEnumerable<int> EmpContracts { get; set; }
        IEnumerable<int> EmpRentContracts { get; set; }
        IEnumerable<int> EmpSaleContracts { get; set; }
    }
}
