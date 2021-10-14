using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class CommissionsDto
    {
        public int PK_Commissions_Id { get; set; }
       
        public decimal TelesalesComission { get; set; }
       
        public decimal SalesComission { get; set; }

        public decimal MgrCommission { get; set; }

        public int FK_Commissions_Transactions_Id { get; set; }

        public int FK_Commissions_Categories_Id { get; set; }

    }
}
