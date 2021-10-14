using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
     public class PreviewHeaderDto
    {
        public int PK_PreviewHeaders_Id { get; set; }

        public int FK_PreviewHeaders_Clients_BuyerId { get; set; }

        public string BuyerName { get; set; }

        public string PhoneNumber { get; set; }

        public string EmpName { get; set; }

        public int FK_PreviewHeaders_Users_CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }   

        public int DemandUnit_Id { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsRejected { get; set; }

        public bool IsSuspended { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsSucceded { get; set; }

        public bool IsNoDecision { get; set; }

        public DateTime ReviewDate { get; set; }

        public string PreviewStatus { get; set; }

        public int Category_Id { get; set; }
    }
}
