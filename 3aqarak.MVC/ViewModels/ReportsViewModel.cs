using _3aqarak.BLL.Helpers;
using _3aqarak.MVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class ReportsViewModel : IReportsViewModel
    {
        public (IEnumerable<string>, IEnumerable<int>, IEnumerable<EmpGroupByCategory>) reportsDataRental { get; set; }
        public (IEnumerable<string>, IEnumerable<int>, IEnumerable<EmpGroupByCategory>) reportsDataSales { get; set; }
        public IEnumerable<int> reportsTotalDataSales { get; set; }
        public IEnumerable<int> EmpContracts { get; set; }
        public IEnumerable<int> EmpRentContracts { get; set; }
        public IEnumerable<int> EmpSaleContracts { get; set; }
    }
}