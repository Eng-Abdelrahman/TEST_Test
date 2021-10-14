using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IPreviewFilter
    {
        DateTime? fromDate { get; set; }
        DateTime? tomDate { get; set; }
        int? EmpId { get; set; }
        int? BuyerId { get; set; }
        int? sellerId { get; set; }

    }
}
