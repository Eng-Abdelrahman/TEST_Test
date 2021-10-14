using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class ClientCallDto
    {
        public int PK_MarketingCalls_Id { get; set; }

        public int FK_MarketingCalls_Users_EmpolyeeId { get; set; }

        public int FK_ClientCalls_Clients_Id { get; set; }

        public DateTime DateTime { get; set; }
       
        public string Descreption { get; set; }      
      
        public string ClientName { get; set; }
        
        public string PhoneNumber { get; set; }      
     
        public string Subject { get; set; }

        public string Notes { get; set; }

        public int? AvailableCode { get; set; }

        public int? DemandCode { get; set; }

        public int CategoryId { get; set; }

    }
}
