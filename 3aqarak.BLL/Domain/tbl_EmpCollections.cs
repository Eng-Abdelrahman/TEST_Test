namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_EmpCollections
    {
        [Key]
        public int PK_EmpCollect_Id { get; set; }

        public int FK_EmpCollect_Contarct_Id { get; set; }

        public int FK_EmpCollect_EmpId { get; set; }

        [Column(TypeName = "money")]
        public decimal CommValue { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_EmpCollect_Users_CreatedBy { get; set; }

        public int FK_EmpCollect_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_EmpCollect_Collections_Id { get; set; }

        public virtual tbl_Collections tbl_Collections { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }
    }
}
