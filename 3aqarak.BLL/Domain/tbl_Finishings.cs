namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Finishings
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Finishings()
        {
            tbl_Demand_Finishings = new HashSet<tbl_Demand_Finishings>();
            tbl_VillademandFinishings = new HashSet<tbl_VillademandFinishings>();
            tbl_VillasAvailables = new HashSet<tbl_VillasAvailables>();
        }

        [Key]
        public int PK_Finishings_Id { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_Finishings_Users_CreatedBy { get; set; }

        public int FK_Finishings_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsMaster { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Demand_Finishings> tbl_Demand_Finishings { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillademandFinishings> tbl_VillademandFinishings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasAvailables> tbl_VillasAvailables { get; set; }
    }
}
