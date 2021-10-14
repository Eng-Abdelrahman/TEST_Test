using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class PostponedCallDto
    {
       
        public int PK_PostbonedCalls { get; set; }

        public int FK_PostponedCalls_Clients_ClientId { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateTime { get; set; }          
       
        public string Descreption { get; set; }
      
        public string Subject { get; set; }

        public string Nots { get; set; }
        
        public string ClientName { get; set; }        

        public int? AvailableCode { get; set; }

        public int? DemandCode { get; set; }

        public int CategoryId { get; set; }

    }
}
