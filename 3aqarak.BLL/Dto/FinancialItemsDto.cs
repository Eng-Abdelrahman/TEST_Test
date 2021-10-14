using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class FinancialItemsDto
    {
        public int PK_Item_Id { get; set; }

        public bool IsExpenses { get; set; }
       
        public DateTime Date { get; set; }
      
        public decimal Amount { get; set; }
        
        public string Description { get; set; }
    }
}
