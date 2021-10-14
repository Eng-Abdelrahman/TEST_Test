namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_VillasDemands
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_VillasDemands()
        {
            tbl_VillaDemandAccessories = new HashSet<tbl_VillaDemandAccessories>();
            tbl_VillademandFinishings = new HashSet<tbl_VillademandFinishings>();
            tbl_VillasDemandViews = new HashSet<tbl_VillasDemandViews>();
        }

        [Key]
        public int PK_VillasDemands_Id { get; set; }

        public int FK_VillasDemands_Clients_ClientId { get; set; }

        public string Notes { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_VillasDemands_Users_CreatedBy { get; set; }

        public int FK_VillasDemands_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_VillasDemands_Categories_Id { get; set; }

        public int FK_VillasDemands_Transactions_Id { get; set; }

        public int FK_VillasDemands_Regions_FromId { get; set; }

        public int FK_VillasDemands_Regions_ToId { get; set; }

        public int FK_VillasDemands_PaymentMethod_Id { get; set; }

        [Column(TypeName = "money")]
        public decimal MinPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal MaxPrice { get; set; }

        public decimal MinSpace { get; set; }

        public decimal MaxSpace { get; set; }

        public decimal MaxAreaSpace { get; set; }

        public decimal MinAreaSpace { get; set; }

        public int MinBathRooms { get; set; }

        public int MaxBathRooms { get; set; }

        public int MinRooms { get; set; }

        public int MaxRooms { get; set; }

        public bool IsFurnished { get; set; }

        public bool IsClosed { get; set; }

        public int FK_VillasDemands_Branches_BranchId { get; set; }

        public int FK_VillasDemands_Users_SalesId { get; set; }

        public int MaxNoOfElevators { get; set; }

        public int MinNoOfElevators { get; set; }

        public int DateOfBuildFrom { get; set; }

        public int DateOfBuildTo { get; set; }

        public int FK_VillasDemands_Usage_Id { get; set; }

        public virtual tbl_Branches tbl_Branches { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_Clients tbl_Clients { get; set; }

        public virtual tbl_PaymentMethods tbl_PaymentMethods { get; set; }

        public virtual tbl_Regions tbl_Regions { get; set; }

        public virtual tbl_Regions tbl_Regions1 { get; set; }

        public virtual tbl_Transactions tbl_Transactions { get; set; }

        public virtual tbl_UnitUsage tbl_UnitUsage { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillaDemandAccessories> tbl_VillaDemandAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillademandFinishings> tbl_VillademandFinishings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasDemandViews> tbl_VillasDemandViews { get; set; }
    }
}
