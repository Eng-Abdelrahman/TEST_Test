namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_PostbonedCalls
    {
        [Key]
        public int PK_PostbonedCalls { get; set; }

        public int FK_PostponedCalls_Clients_ClientId { get; set; }

        [Required]
        [StringLength(11)]
        public string PhoneNumber { get; set; }

        public DateTime DateTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_PostbonedCalls_Users_CreatedBy { get; set; }

        public int FK_PostbonedCalls_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        [Required]
        public string Descreption { get; set; }

        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        public string Nots { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientName { get; set; }

        public int? AvailableCode { get; set; }

        public int? DemandCode { get; set; }

        public int FK_MarketingCalls_Users_EmpolyeeId { get; set; }

        public bool IsDone { get; set; }

        public int CategoryId { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }
    }
}
