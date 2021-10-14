using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class PaymentDto
    {
        public int PK_PaymentMethods_Id { get; set; }
      
        public string PaymentMethod { get; set; }

        public bool IsMaster { get; set; }

    }
}
