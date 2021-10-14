namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_EmpCommissions
    {
        [Key]
        public int PK_EmpComm_Id { get; set; }

        public int FK_EmpComm_Contarct_Id { get; set; }

        public int FK_EmpComm_Emp_Id { get; set; }

        [Column(TypeName = "money")]
        public decimal CommValue { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_EmpComm_Users_CreatedBy { get; set; }

        public int FK_EmpComm_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }
    }
}
