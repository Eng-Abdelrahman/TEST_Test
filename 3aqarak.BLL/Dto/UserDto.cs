using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
     public class UserDto
    {
        public int PK_Users_Id { get; set; }     
        public bool IsActive { get; set; }       
        public bool IsAdmin { get; set; }      
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int FK_Users_Genders_Id { get; set; }
        public System.DateTime? DateOfBirth { get; set; }
        public string PhotoUrl { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string RememberToken { get; set; }
        public int FK_Users_Departement_Id { get; set; }
        public int FK_Users_Branches_Id { get; set; }
        public bool IsOwner { get; set; }
        public int? Specialization_Id { get; set; }

        //Mostafa Add here Full name for Message 
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        //public static implicit operator UserDto(Task<UserDto> v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
