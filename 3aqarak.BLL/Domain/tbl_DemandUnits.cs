namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_DemandUnits
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_DemandUnits()
        {
            tbl_Demand_Finishings = new HashSet<tbl_Demand_Finishings>();
            tbl_DemandAccessories = new HashSet<tbl_DemandAccessories>();
            tbl_DemandViews = new HashSet<tbl_DemandViews>();
        }

        [Key]
        public int PK_DemandUnits_Id { get; set; }

        public int FK_DemandUnits_Clients_ClientId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public string Notes { get; set; }

        public bool IsMatched { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_DemandUnits_Users_CreatedBy { get; set; }

        public int FK_DemandUnits_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_DemandUnits_Categories_Id { get; set; }

        public int FK_DemandUnits_Transactions_Id { get; set; }

        public int FK_DemandUnits_Regions_FromId { get; set; }

        public int FK_DemandUnits_Regions_ToId { get; set; }

        public int FK_DemandUnits_PaymentMethod_Id { get; set; }

        [Column(TypeName = "money")]
        public decimal MinPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal MaxPrice { get; set; }

        public decimal MinSpace { get; set; }

        public decimal MaxSpace { get; set; }

        public int MinBathRooms { get; set; }

        public int MaxBathRooms { get; set; }

        public int MinRooms { get; set; }

        public int MaxRooms { get; set; }

        public int MinFloor { get; set; }

        public int MaxFloor { get; set; }

        public bool IsFurnished { get; set; }

        public bool IsClosed { get; set; }

        public int FK_DemandUnits_Branches_BranchId { get; set; }

        public int FK_DemandUnits_Users_Sales { get; set; }

        public bool IsResidential { get; set; }

        public int NoOfElevatorsFrom { get; set; }

        public int NoOfElevatorsTo { get; set; }

        public int DateOfBuildFrom { get; set; }

        public int DateOfBuildTo { get; set; }

        public int FK_DemandUnits_Usage_Id { get; set; }

        public virtual tbl_Branches tbl_Branches { get; set; }

        public virtual tbl_Clients tbl_Clients { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Demand_Finishings> tbl_Demand_Finishings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandAccessories> tbl_DemandAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandViews> tbl_DemandViews { get; set; }

        public virtual tbl_PaymentMethods tbl_PaymentMethods { get; set; }

        public virtual tbl_Regions tbl_Regions { get; set; }

        public virtual tbl_Regions tbl_Regions1 { get; set; }

        public virtual tbl_Transactions tbl_Transactions { get; set; }

        public virtual tbl_UnitUsage tbl_UnitUsage { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }
    }
}
