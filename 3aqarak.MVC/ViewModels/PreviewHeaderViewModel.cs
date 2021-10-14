using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class PreviewHeaderViewModel
    {

        public int PK_PreviewHeaders_Id { get; set; }

        [Required]
        [Display(Name ="اسم البائع")]
        public string BuyerName { get; set; }

        public string PhoneNumber { get; set; }

        public int FK_PreviewHeaders_Clients_BuyerId { get; set; }

        [Display(Name = "تاريخ المعاينة")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "رقم الطلب")]
        public int DemandUnit_Id { get; set; }

        [Required]
        [Display(Name = "اسم الموظــف")]
        public string EmpName { get; set; }

        [Display(Name = "رقم الموظــف")]
        public int? FK_PreviewHeaders_Users_CreatedBy { get; set; }

        [Display(Name = "تاريخ المعاينه")]
        public DateTime ReviewDate { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsRejected { get; set; }

        public bool? IsSuspended { get; set; }

        public bool? IsCancelled { get; set; }

        public bool? IsSucceded { get; set; }

        public bool? IsNoDecision { get; set; }

        public string PreviewStatus { get; set; }

        public int Category_Id { get; set; }
    }
}