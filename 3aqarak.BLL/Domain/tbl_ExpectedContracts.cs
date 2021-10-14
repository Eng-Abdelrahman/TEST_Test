namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_ExpectedContracts
    {
        [Key]
        public int PK_ExpectContracts_Id { get; set; }

        public int FK_ExpectContract_Clients_Seller { get; set; }

        public int FK_ExpectContract_Clients_Buyer { get; set; }

        public int AvailableUnits_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime ExpectDateTime { get; set; }

        public bool IsDone { get; set; }

        public bool IsPostponed { get; set; }

        public bool IsCancelled { get; set; }

        [Column(TypeName = "money")]
        public decimal Deposit { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public string BuyerAddress { get; set; }

        public string BuyerNationalId { get; set; }

        public string SellerAddress { get; set; }

        public string SellerNationalId { get; set; }

        public int FK_ExpectContracts_Users_CreatedBy { get; set; }

        public int FK_ExpectContracts_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsContractAgreement { get; set; }

        public string Notes { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PostponeDateTime { get; set; }

        public int FK_ExpectedContract_Preview_Id { get; set; }

        public int AvailableCat { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_PreviewHeaders tbl_PreviewHeaders { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }
    }
}
