namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_UnitUsage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_UnitUsage()
        {
            tbl_AvailableUnits = new HashSet<tbl_AvailableUnits>();
            tbl_DemandUnits = new HashSet<tbl_DemandUnits>();
            tbl_ShopAvailable = new HashSet<tbl_ShopAvailable>();
            tbl_ShopDemands = new HashSet<tbl_ShopDemands>();
            tbl_VillasAvailables = new HashSet<tbl_VillasAvailables>();
            tbl_VillasDemands = new HashSet<tbl_VillasDemands>();
        }

        [Key]
        public int PK_UnitUsage_Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AvailableUnits> tbl_AvailableUnits { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandUnits> tbl_DemandUnits { get; set; }

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
