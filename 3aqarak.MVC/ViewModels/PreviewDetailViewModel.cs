using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class PreviewDetailViewModel
    {
        public int PK_PreviewDetails_Id { get; set; }

        [Display(Name ="رقم العــرض")]
        public int AvailableUnits_Id { get; set; }

        [Display(Name = "رقم البائع")]
        public int Fk_PreviewDetails_Clients_SellerId { get; set; }

        [Display(Name = "اسـم البائع")]
        public string SellerName { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsPostponed { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsRejected { get; set; }

        public bool IsNoDecision { get; set; }

        [Display(Name = "رقم المعاينة")]
        public int Fk_PreviewDetails_PreviewHeaders_Id { get; set; }

        [Display(Name = "تاريخ التاجيل")]
        public DateTime? PostPoneDate { get; set; }

        public string DetailStatus { get; set; }

        [Display(Name = "وقت المعاينه")]
        public string PreviewTime { get; set; }

        public int Category_Id { get; set; }

    }
}