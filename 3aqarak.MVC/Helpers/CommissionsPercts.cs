using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Helpers
{
    public class CommissionsPercts
    {
        public int SalesId { get; set; }
        public int ContractId { get; set; }
        public int TeleSalesId { get; set; }
        public int MgrId { get; set; }
        public decimal Commission { get; set; }
        public decimal SalesComm { get; set; }
        public decimal TeleSalesComm { get; set; }
        public decimal MgrComm { get; set; }
        public string Code { get; set; }
    }
}