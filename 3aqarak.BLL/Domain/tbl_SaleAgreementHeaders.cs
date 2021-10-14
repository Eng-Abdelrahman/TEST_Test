namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_SaleAgreementHeaders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_SaleAgreementHeaders()
        {
            tbl_SaleAgreementDetails = new HashSet<tbl_SaleAgreementDetails>();
        }

        [Key]
        public int PK_SalesHeaders_Id { get; set; }

        public int FK_SalesHeaders_Users_EmpId { get; set; }

        public int FK_SalesHeaders_Clients_SellerId { get; set; }

        public int FK_SalesHeaders_Clients_BuyerId { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalAmount { get; set; }

        public int? FK_SalesHeaders_PaymentBasis_Id { get; set; }

        public bool IsDone { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfFirstInstall { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfNextInstall { get; set; }

        [Column(TypeName = "money")]
        public decimal? DefaultInstallValue { get; set; }

        [Column(TypeName = "money")]
        public decimal? NextInstallValue { get; set; }

        [Column(TypeName = "money")]
        public decimal? ReservDeposit { get; set; }
        public bool IsFromAgreementContract { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_SalesHeaders_Users_CreatedBy { get; set; }

        public int FK_SalesHeaders_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public string SellerAddress { get; set; }

        [Required]
        public string BuyerAddress { get; set; }

        [Required]
        [StringLength(14)]
        public string SellerId { get; set; }

        [Required]
        [StringLength(14)]
        public string BuyerId { get; set; }

        public bool IsInstallable { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfLastInstall { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int DemandUnits_Id { get; set; }

        public int AvailableUnits_Id { get; set; }

        public bool IsCalculated { get; set; }

        public string ContractUrl { get; set; }

        [Column(TypeName = "money")]
        public decimal PaidAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal DueAmount { get; set; }

        public int AvailableCat { get; set; }

        public int DemandCat { get; set; }


        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_Categories tbl_Categories1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SaleAgreementDetails> tbl_SaleAgreementDetails { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }
    }
}
