using _3aqarak.MVC.Areas.Dashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Areas.Dashboard.Validations
{
    public class CompareMobile:ValidationAttribute
    {
        private string _property;
        public CompareMobile( string property)
        {
            _property = property;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var val = (ClientViewModel)validationContext.ObjectInstance;
            return value.ToString() == val.Mobile2 ? new ValidationResult("الموبايل الثاني لابد ان يكون مختلف عن الاول!") : ValidationResult.Success;
            
        }
    }
}