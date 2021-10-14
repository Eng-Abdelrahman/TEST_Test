namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_RentAgreementHeaders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_RentAgreementHeaders()
        {
            tbl_RentAgreementDetails = new HashSet<tbl_RentAgreementDetails>();
            tbl_RentalArchives = new HashSet<tbl_RentalArchives>();
        }

        [Key]
        public int PK_RentAgreements_Id { get; set; }

        public int FK_RentAgreements_Users_EmpId { get; set; }

        public int FK_RentAgreements_Clients_Seller { get; set; }

        public int FK_RentAgreements_Clients_Buyer { get; set; }

        public string ContractUrl { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public bool HasEnded { get; set; }

        [Column(TypeName = "date")]
        public DateTime RentalStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime RentalEndDate { get; set; }

        public int FK_RentAgreementHeader_PaymentBasis_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateNxtRent { get; set; }

        [Column(TypeName = "money")]
        public decimal ValueOfRental { get; set; }

        [Required]
        [StringLength(50)]
        public string SellerNationality { get; set; }

        [Required]
        [StringLength(50)]
        public string BuyerNationality { get; set; }

        [Required]
        [StringLength(50)]
        public string SellerIdSource { get; set; }

        [Required]
        [StringLength(50)]
        public string BuyerIdSource { get; set; }

        [Required]
        public string SellerAddress { get; set; }

        [Required]
        public string BuyerAddress { get; set; }

        [Required]
        [StringLength(14)]
        public string SellerIdNumber { get; set; }

        [Required]
        [StringLength(14)]
        public string BuyerIdNumber { get; set; }

        public bool IsCalculated { get; set; }

        public int DemandUnits_Id { get; set; }

        public int AvailableUnits_Id { get; set; }

        public int AvailableCat { get; set; }

        public int DemandCat { get; set; }
        public bool IsFromAgreementContract { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_Categories tbl_Categories1 { get; set; }

        public virtual tbl_Clients tbl_Clients { get; set; }

        public virtual tbl_Clients tbl_Clients1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_RentAgreementDetails> tbl_RentAgreementDetails { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_RentalArchives> tbl_RentalArchives { get; set; }
    }
}
