namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_Expenses
    {
        [Key]
        public int PK_Expenses_Id { get; set; }

        [Column(TypeName = "money")]
        public decimal Salaries { get; set; }

        [Column(TypeName = "money")]
        public decimal Commissions { get; set; }

        [Column(TypeName = "money")]
        public decimal Collections { get; set; }

        [Column(TypeName = "money")]
        public decimal Bonuses { get; set; }

        [Column(TypeName = "money")]
        public decimal Rentals { get; set; }

        [Column(TypeName = "money")]
        public decimal Others { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_Expenses_Users_CreatedBy { get; set; }

        public int FK_Expenses_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }
    }
}
