using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class CatViewModel
    {
        
        public int PK_Categories_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

    }
}