using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class DeptDto
    {          
        public int PK_Departement_Id { get; set; }
    
        public string Name { get; set; }

        public int FK_Depts_Users_MgrId { get; set; }

        public int RegCode { get; set; }

    }
}
