namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_PaymentMethods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_PaymentMethods()
        {
            tbl_AvailableLands = new HashSet<tbl_AvailableLands>();
            tbl_AvailableUnits = new HashSet<tbl_AvailableUnits>();
            tbl_DemandUnits = new HashSet<tbl_DemandUnits>();
            tbl_LandsDemands = new HashSet<tbl_LandsDemands>();
            tbl_ShopAvailable = new HashSet<tbl_ShopAvailable>();
            tbl_ShopDemands = new HashSet<tbl_ShopDemands>();
            tbl_VillasAvailables = new HashSet<tbl_VillasAvailables>();
            tbl_VillasDemands = new HashSet<tbl_VillasDemands>();
        }

        [Key]
        public int PK_PaymentMethods_Id { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_PaymentMethods_Users_CreatedBy { get; set; }

        public int FK_PaymentMethods_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsMaster { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AvailableLands> tbl_AvailableLands { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AvailableUnits> tbl_AvailableUnits { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandUnits> tbl_DemandUnits { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LandsDemands> tbl_LandsDemands { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailable> tbl_ShopAvailable { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemands> tbl_ShopDemands { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasAvailables> tbl_VillasAvailables { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasDemands> tbl_VillasDemands { get; set; }
    }
}
