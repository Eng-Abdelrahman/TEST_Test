using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class FellowCallDto
    {
        public int PK_FellowupCalls_Id { get; set; }
        public DateTime DateTime { get; set; }

        public string Descreption { get; set; }

        public string ClientName { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string Subject { get; set; }

        public string Notes { get; set; }

        public int ClientId { get; set; }
    }
}
