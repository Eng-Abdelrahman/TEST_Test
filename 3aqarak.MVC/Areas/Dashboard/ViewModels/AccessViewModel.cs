using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class AccessViewModel
    {
        public int PK_Accessories_Id { get; set; }

        [Required]
        public string Name { get; set; }
     
    }
}