using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class BasisViewModel
    {
        public int PK_PaymentBasis_Id { get; set; }

        [Required]
        [Range(1,366,ErrorMessage =" مسموح بعدد أيام من 1 الى366 فقط!")]
        public int NoOfDays { get; set; }

        [Required]
        [MaxLength(50,ErrorMessage ="لايسمح باكثر من 50 حرف!")]
        public string Name { get; set; }

    }
}