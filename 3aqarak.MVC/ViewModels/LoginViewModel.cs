using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "لابد من  ادخال اسم المستخدم")]
        [Display(Name = "اسم المستخدم:")]       
        public string UserName { get; set; }

        [Required(ErrorMessage = "لابد من  ادخال كلمة السر")]
        [DataType(DataType.Password)]
        [Display(Name = ":كلمة الســر")]
        public string Password { get; set; }

        [Display(Name = "حفظ كلمة السر")]
        public bool RememberMe { get; set; }
    }
}