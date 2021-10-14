using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class BranchViewModel
    {
        public int PK_Branch_Id { get; set; }

        [Required]
        [Display(Name ="أسم الفرع")]
        public string Name { get; set; }

       
        [Display(Name = "عنوان الفرع")]
        public string Address { get; set; }

        [Display(Name = "تليفون الفرع")]
        public string PhoneNumber { get; set; }

        
        [Display(Name = "مدير الفرع")]
        public int FK_Branches_Users_MgrId { get; set; }

        public bool IsMainBranch { get; set; }
    }
}