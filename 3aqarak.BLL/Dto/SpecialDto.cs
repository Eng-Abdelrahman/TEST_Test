using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class SpecialDto
    {
        public int PK_Specialization_Id { get; set; }
        
        public string Name { get; set; }   

        public int FK_Specialization_Dept_DeptId { get; set; }

    }
}
