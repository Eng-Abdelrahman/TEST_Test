namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Commissions
    {
        [Key]
        public int PK_Commissions_Id { get; set; }

        [Column(TypeName = "money")]
        public decimal SalesComission { get; set; }

        [Column(TypeName = "money")]
        public decimal TelesalesComission { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_Commissions_users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_Commissions_Transactions_Id { get; set; }

        public int FK_Commissions_Categories_Id { get; set; }

        public int FK_Commissions_users_CreatedBy { get; set; }

        [Column(TypeName = "money")]
        public decimal MgrCommission { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_Transactions tbl_Transactions { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }
    }
}
