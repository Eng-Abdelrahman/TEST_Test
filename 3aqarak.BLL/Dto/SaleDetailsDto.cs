using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class SaleDetailsDto
    {        
        public int PK_SaleDetails_Id { get; set; }

        public int FK_SaleDetails_SaleHeaders_Id { get; set; }
       
        public string DetailContent { get; set; }     
      
    }
}
