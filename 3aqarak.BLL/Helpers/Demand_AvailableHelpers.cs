using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Helpers
{
    public class EmployeeAndCat
    {
        public string Employee { get; set; }
        public int Cat { get; set; }
    }

    
    public struct EmpGroupByCategory
    {
        public string EmpName;
        public string Category;
        public int Count;
    }
    
}
