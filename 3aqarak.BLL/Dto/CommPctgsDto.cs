using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class CommPctgsDto
    {
        public int ContractId { get; set; }
        public int SalesId { get; set; }
        public int TeleSalesId { get; set; }
        public int MgrId { get; set; }
        public decimal Commission { get; set; }
        public decimal SalesComm { get; set; }
        public decimal TeleSalesComm { get; set; }
        public decimal MgrComm { get; set; }
    }
}
