namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Accessories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Accessories()
        {
            tbl_DemandAccessories = new HashSet<tbl_DemandAccessories>();
            tbl_ShopAvailabeAccessories = new HashSet<tbl_ShopAvailabeAccessories>();
            tbl_ShopDemandAccessories = new HashSet<tbl_ShopDemandAccessories>();
            tbl_UnitAccessories = new HashSet<tbl_UnitAccessories>();
            tbl_VillaAccessories = new HashSet<tbl_VillaAccessories>();
            tbl_VillaDemandAccessories = new HashSet<tbl_VillaDemandAccessories>();
        }

        [Key]
        public int PK_Accessories_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_Accessories_Users_CreatedBy { get; set; }

        public int FK_Accessories_Users_ModidfiedBy { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandAccessories> tbl_DemandAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailabeAccessories> tbl_ShopAvailabeAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemandAccessories> tbl_ShopDemandAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_UnitAccessories> tbl_UnitAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillaAccessories> tbl_VillaAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillaDemandAccessories> tbl_VillaDemandAccessories { get; set; }
    }
}
