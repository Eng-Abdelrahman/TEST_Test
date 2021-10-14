using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.BLL.Dto
{
    public class StaticDto
    {
        public int PK_StatContract_Id { get; set; }

        public int FK_StaticContract_Categories_CatId { get; set; }

        public int FK_StaticContract_Transaction_Transid { get; set; }
        
        public string ContactContent { get; set; }    

      
    }
}
