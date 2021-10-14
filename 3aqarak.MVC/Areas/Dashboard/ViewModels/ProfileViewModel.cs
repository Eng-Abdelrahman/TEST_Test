using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class ProfileViewModel
    {
        public int PK_Users_Id { get; set; }       

        public string Email { get; set; }

        [RegularExpression("(01)[0-9]{9}", ErrorMessage = "رقم الموبايل غير صحيح!")]
        public string PhoneNumber { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public System.DateTime DateOfBirth { get; set; }

        public string PhotoUrl { get; set; }

        public int FK_Users_Genders_Id { get; set; }

        public SelectList GenderList { get; set; }     

        public string Password { get; set; }

        public string Salt { get; set; }
       

    }
}