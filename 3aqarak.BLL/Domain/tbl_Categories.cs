namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Categories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Categories()
        {
            tbl_AvailableLands = new HashSet<tbl_AvailableLands>();
            tbl_ClientsCalls = new HashSet<tbl_ClientsCalls>();
            tbl_Commissions = new HashSet<tbl_Commissions>();
            tbl_ExpectedContracts = new HashSet<tbl_ExpectedContracts>();
            tbl_LandsDemands = new HashSet<tbl_LandsDemands>();
            tbl_PostbonedCalls = new HashSet<tbl_PostbonedCalls>();
            tbl_Posts = new HashSet<tbl_Posts>();
            tbl_RentAgreementHeaders = new HashSet<tbl_RentAgreementHeaders>();
            tbl_RentAgreementHeaders1 = new HashSet<tbl_RentAgreementHeaders>();
            tbl_SaleAgreementHeaders = new HashSet<tbl_SaleAgreementHeaders>();
            tbl_SaleAgreementHeaders1 = new HashSet<tbl_SaleAgreementHeaders>();
            tbl_ShopAvailable = new HashSet<tbl_ShopAvailable>();
            tbl_ShopDemands = new HashSet<tbl_ShopDemands>();
            tbl_StaticContracts = new HashSet<tbl_StaticContracts>();
            tbl_units = new HashSet<tbl_units>();
            tbl_VillasAvailables = new HashSet<tbl_VillasAvailables>();
            tbl_VillasDemands = new HashSet<tbl_VillasDemands>();
            
        }

        [Key]
        public int PK_Categories_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_Categories_Users_CreatedBy { get; set; }

        public int FK_Categories_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AvailableLands> tbl_AvailableLands { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }      

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ClientsCalls> tbl_ClientsCalls { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Commissions> tbl_Commissions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ExpectedContracts> tbl_ExpectedContracts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LandsDemands> tbl_LandsDemands { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PostbonedCalls> tbl_PostbonedCalls { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Posts> tbl_Posts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_RentAgreementHeaders> tbl_RentAgreementHeaders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_RentAgreementHeaders> tbl_RentAgreementHeaders1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SaleAgreementHeaders> tbl_SaleAgreementHeaders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SaleAgreementHeaders> tbl_SaleAgreementHeaders1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailable> tbl_ShopAvailable { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemands> tbl_ShopDemands { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_StaticContracts> tbl_StaticContracts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_units> tbl_units { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasAvailables> tbl_VillasAvailables { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasDemands> tbl_VillasDemands { get; set; }
    }
}
