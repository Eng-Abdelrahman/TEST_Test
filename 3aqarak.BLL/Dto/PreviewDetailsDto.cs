using _3aqarak.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class PreviewDetailsDto
    {
        public int PK_PreviewDetails_Id { get; set; }

        public int AvailableUnits_Id { get; set; }

        public int Fk_PreviewDetails_Clients_SellerId { get; set; }

        public string SellerName { get; set; }

        public bool IsSucceded { get; set; }

        public bool IsPostponed { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsRejected { get; set; }

        public bool IsNoDecision { get; set; }

        public int Fk_PreviewDetails_PreviewHeaders_Id { get; set; }

        public DateTime? PostPoneDate { get; set; }

        public string  DetailStatus { get; set; }

        public string PreviewTime { get; set; }

        public DateTime PreviewHeaderDate { get; set; }

        public int Category_Id { get; set; }

    }
}
