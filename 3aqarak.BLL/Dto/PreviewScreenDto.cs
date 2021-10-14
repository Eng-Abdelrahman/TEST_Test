using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
     public class PreviewScreenDto
    {
        public PreviewHeaderDto Header { get; set; }

        public List<PreviewDetailsDto> Details { get; set; }
    }
}
