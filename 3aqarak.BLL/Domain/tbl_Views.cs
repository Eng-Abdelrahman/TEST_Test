namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Views
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Views()
        {
            tbl_AvailableLands = new HashSet<tbl_AvailableLands>();
            tbl_DemandViews = new HashSet<tbl_DemandViews>();
            tbl_LandDemandViews = new HashSet<tbl_LandDemandViews>();
            tbl_ShopAvailable = new HashSet<tbl_ShopAvailable>();
            tbl_ShopDemandViews = new HashSet<tbl_ShopDemandViews>();
            tbl_units = new HashSet<tbl_units>();
            tbl_VillasAvailables = new HashSet<tbl_VillasAvailables>();
        }

        [Key]
        public int PK_Views_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_Views_Users_CreatedBy { get; set; }

        public int FK_Views_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AvailableLands> tbl_AvailableLands { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandViews> tbl_DemandViews { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LandDemandViews> tbl_LandDemandViews { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailable> tbl_ShopAvailable { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemandViews> tbl_ShopDemandViews { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_units> tbl_units { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasAvailables> tbl_VillasAvailables { get; set; }
    }
}
