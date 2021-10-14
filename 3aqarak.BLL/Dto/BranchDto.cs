using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class BranchDto
    {
        public int PK_Branch_Id { get; set; }
       
        public string Name { get; set; }

        public string Address { get; set; }
     
        public string PhoneNumber { get; set; }

        public int FK_Branches_Users_MgrId { get; set; }

        public bool IsMainBranch { get; set; }

    }
}
