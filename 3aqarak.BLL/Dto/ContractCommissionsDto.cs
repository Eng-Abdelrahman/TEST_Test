using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class ContractCommissionsDto
    {
        public int ContractId { get; set; }

        public int AvailableCatId { get; set; }

        public int AvailableId { get; set; }

        public string StringDate { get; set; }

        public string Type { get; set; }

        public int TransCode { get; set; }

        public string Calculated { get; set; }

        public string TotalAmount { get; set; }

        public string RentAmount { get; set; }

        public int TeleSalesId { get; set; }

        public int SalesId { get; set; }

        public int MgrId { get; set; }

        public string TeleSalesName { get; set; }

        public string SalesName { get; set; }

        public string MgrName { get; set; }

        public string SalesCommissionPercent { get; set; }

        public string TelesalesCommissionPercent { get; set; }

        public string MgrCommissionPercent { get; set; }

        public decimal SalesCommissionAmount { get; set; }

        public decimal TelesalesCommissionAmount { get; set; }

        public decimal MgrCommissionAmount { get; set; }

    }
}
