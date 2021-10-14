using _3aqarak.BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Domain
{
    public partial class tbl_FellowupCall
    {
        [Key]
        public int PK_FellowupCalls_Id { get; set; }

        public int FK_FellowupCalls_Users_EmpolyeeId { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        public string Descreption { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_FellowupCalls_Users_CreatedBy { get; set; }

        public int FK_FellowupCalls_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsDone { get; set; }


        [Required]
        [StringLength(50)]
        public string ClientName { get; set; }

        [Required]
        public string PhoneNumber1 { get; set; }

        [Required]
        public string PhoneNumber2 { get; set; }

        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        public string Notes { get; set; }

        public int FK_ClientCalls_Clients_Id { get; set; }

        public virtual tbl_Clients tbl_Clients { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }
    }
}
