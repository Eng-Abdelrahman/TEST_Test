using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class SpecialViewModel
    {

        public int PK_Specialization_Id { get; set; }

        [Required]
        [Display(Name ="الاسـم")]
        public string Name { get; set; }

        [Display(Name = "القسـم")]
        public int FK_Specialization_Dept_DeptId { get; set; }

        public SelectList Depts { get; set; }
    }
}