namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_ContractArchives
    {
        [Key]
        public int PK_ContractArchives_Id { get; set; }

        [Required]
        public string ImageURL { get; set; }

        [Required]
        [StringLength(30)]
        public string ContractID { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public int FK_ContractArchives_Users_CreatedBy { get; set; }

        public int FK_ContractArchives_Users_ModidfiedBy { get; set; }

        public bool IsDeleted { get; set; }

        

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }
    }
}
