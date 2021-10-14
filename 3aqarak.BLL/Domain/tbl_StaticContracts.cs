namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_StaticContracts
    {
        [Key]
        public int PK_StatContract_Id { get; set; }

        public int FK_StaticContract_Categories_CatId { get; set; }

        public int FK_StaticContract_Transaction_Transid { get; set; }

        [Required]
        public string ContactContent { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_StatContract_Users_CreatedBy { get; set; }

        public int FK_StatContract_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_Transactions tbl_Transactions { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }
    }
}
