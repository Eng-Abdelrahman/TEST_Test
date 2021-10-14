using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class RentDetailsDto
    {
        public int PK_RentDetails_Id { get; set; }

        public int FK_RentHeaders_RentDetails_Id { get; set; }
       
        public string DetailContent { get; set; }

    }
}
