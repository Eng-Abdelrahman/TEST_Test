using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class UserViewModel
    {
        public int PK_Users_Id { get; set; }      
        
        public bool IsActive { get; set; }
    
        public bool IsAdmin { get; set; }
       
        public string Email { get; set; }
        [Required(ErrorMessage ="الرجاء إدخال رقم الموبايل")]
        [RegularExpression("(01)[0-9]{9}", ErrorMessage = "رقم الموبايل غير صحيح!")]
        public string PhoneNumber { get; set; }

        public string UserName { get; set; }
       
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }        
        public DateTime? DateOfBirth { get; set; }

        public string PhotoUrl { get; set; }
        [Required(ErrorMessage ="الرجاء إدخال النوع")]
        public int FK_Users_Genders_Id { get; set; }

        public SelectList GenderList { get; set; }

        public SelectList BranchList { get; set; }

        public SelectList DeptList { get; set; }

        public SelectList SpecialList { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }
        [Required(ErrorMessage ="الراجاء إختيار القسم")]
        public int FK_Users_Departement_Id { get; set; }
        [Required(ErrorMessage = "الراجاء إختيار الفرع")]
        public int FK_Users_Branches_Id { get; set; }

        public bool IsOwner { get; set; }

        public int? Specialization_Id { get; set; }



    }
}