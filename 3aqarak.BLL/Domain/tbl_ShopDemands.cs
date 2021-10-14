namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_ShopDemands
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_ShopDemands()
        {
            tbl_ShopDemandAccessories = new HashSet<tbl_ShopDemandAccessories>();
            tbl_ShopDemandViews = new HashSet<tbl_ShopDemandViews>();
        }

        [Key]
        public int PK_ShopDemands_Id { get; set; }

        public int FK_ShopDemands_Clients_ClientId { get; set; }

        public string Notes { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_ShopDemands_Users_CreatedBy { get; set; }

        public int FK_ShopDemands_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_ShopDemands_Categories_Id { get; set; }

        public int FK_ShopDemands_Transactions_Id { get; set; }

        public int FK_ShopDemands_Regions_FromId { get; set; }

        public int FK_ShopDemands_Regions_ToId { get; set; }

        public int FK_ShopDemands_PaymentMethod_Id { get; set; }

        [Column(TypeName = "money")]
        public decimal MinPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal MaxPrice { get; set; }

        public decimal MinSpace { get; set; }

        public decimal MaxSpace { get; set; }

        public int MinBathRooms { get; set; }

        public int MaxBathRooms { get; set; }

        public int ScaleNumber { get; set; }

        public bool IsDivider { get; set; }

        public bool IsClosed { get; set; }

        public int FK_ShopDemands_Branches_BranchId { get; set; }

        public int FK_ShopDemands_Users_SalesId { get; set; }

        public int DateOfBuildFrom { get; set; }

        public int DateOfBuildTo { get; set; }

        public int FK_ShopDemands_Usage_Id { get; set; }

        public bool Islicense { get; set; }

        public bool IsFurnisher { get; set; }

        public virtual tbl_Branches tbl_Branches { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_Clients tbl_Clients { get; set; }

        public virtual tbl_PaymentMethods tbl_PaymentMethods { get; set; }

        public virtual tbl_Regions tbl_Regions { get; set; }

        public virtual tbl_Regions tbl_Regions1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemandAccessories> tbl_ShopDemandAccessories { get; set; }

        public virtual tbl_Transactions tbl_Transactions { get; set; }

        public virtual tbl_UnitUsage tbl_UnitUsage { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemandViews> tbl_ShopDemandViews { get; set; }
    }
}
