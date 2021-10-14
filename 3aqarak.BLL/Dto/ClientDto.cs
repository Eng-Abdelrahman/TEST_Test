using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
     public class ClientDto
    {
        
        public int PK_Client_Id { get; set; }
   
        public string Name { get; set; }
      
        public string Phone { get; set; }
    
        public string Mobile { get; set; }
     
        public string IdNumber { get; set; }
       
        public string Address { get; set; }

        public string IdUrlFront { get; set; }

        public string IdUrlBack { get; set; }

        public string Notes { get; set; }
      
        public string Job { get; set; }        
    
        public bool IsDeleted { get; set; }
     
        public string Mobile2 { get; set; }
     
        public string BestContactHour { get; set; }


    }
}
