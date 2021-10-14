using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class FinishViewModel
    {
        public int PK_Finishings_Id { get; set; }

        [Required]
        public string Type { get; set; }

        public bool IsMaster { get; set; }
    }
}