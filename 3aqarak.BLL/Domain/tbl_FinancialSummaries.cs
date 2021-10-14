namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_FinancialSummaries
    {
        [Key]
        public int PK_FinancialSummaries_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "money")]
        public decimal ExpensesSummary { get; set; }

        [Column(TypeName = "money")]
        public decimal IncomeSummary { get; set; }

        [Column(TypeName = "money")]
        public decimal ProfitSummary { get; set; }
    }
}
