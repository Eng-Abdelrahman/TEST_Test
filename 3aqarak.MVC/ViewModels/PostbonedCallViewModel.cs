using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class PostbonedCallViewModel
    {
        public int PK_PostbonedCalls { get; set; }

        public DateTime? DateTime { get; set; }

        public string Descreption { get; set; }

        public string ClientName { get; set; }

        public string PhoneNumber { get; set; }

        public string Subject { get; set; }

        public string Notes { get; set; }

        public int? AvailableCode { get; set; }

        public int? DemandCode { get; set; }

        public int Clients_Id { get; set; }

        public int CategoryId { get; set; }
    }
}