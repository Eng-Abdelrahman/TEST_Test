namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_Revenues
    {
        [Key]
        public int PK_Revenues_Id { get; set; }

        public int FK_Revenues_AgreementHeader_Id { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public int FK_Revenues_Users_Employee { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public bool IsCollection { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_Revenues_Users_CreatedBy { get; set; }

        public int FK_Revenues_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }
    }
}
