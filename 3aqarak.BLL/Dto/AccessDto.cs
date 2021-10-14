using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
     public class AccessDto
    {
       
        public int PK_Accessories_Id { get; set; }

        public string Name { get; set; }
       
        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_Accessories_Users_CreatedBy { get; set; }

        public int FK_Accessories_Users_ModidfiedBy { get; set; }

    }
}
