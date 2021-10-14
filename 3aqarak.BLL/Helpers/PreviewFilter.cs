using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Helpers
{
    public class PreviewFilter 
    {
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int? EmpId { get; set; }
        public int? BuyerId { get; set; }
        public int? sellerId { get; set; }
    }
}
