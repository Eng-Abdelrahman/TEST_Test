namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_SaleAgreementDetails
    {
        [Key]
        public int PK_SaleDetails_Id { get; set; }

        public int FK_SaleDetails_SaleHeaders_Id { get; set; }

        [Required]
        public string DetailContent { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_SaleDetails_Users_CreatedBy { get; set; }

        public int FK_SaleDetails_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_SaleAgreementHeaders tbl_SaleAgreementHeaders { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }
    }
}
