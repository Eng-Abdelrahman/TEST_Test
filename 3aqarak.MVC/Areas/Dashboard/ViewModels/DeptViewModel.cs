using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class DeptViewModel
    {
        public int PK_Departement_Id { get; set; }

        [Required]
        [Display(Name = "اسم القسم")]
        public string Name { get; set; }

        [Display(Name = "اسم المدير")]
        public int FK_Depts_Users_MgrId { get; set; }

        [Required]
        [Display(Name = "كود القسم")]
        public int RegCode { get; set; }
    }
}