namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_RentalArchives
    {
        [Key]
        public int PK_RentalArchives_Id { get; set; }

        [Required]
        public string Notes { get; set; }

        public int FK_RentalArchives_RentHeaders_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_RentalArchives_Users_CreatedBy { get; set; }

        public int FK_RentalArchives_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_RentAgreementHeaders tbl_RentAgreementHeaders { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }
    }
}
