namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_LandsDemands
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_LandsDemands()
        {
            tbl_LandDemandViews = new HashSet<tbl_LandDemandViews>();
        }

        [Key]
        public int PK_LandsDemands_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_LandsDemands_Users_CreatedBy { get; set; }

        public int FK_LandsDemands_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_LandsDemands_Clients_ClientId { get; set; }

        public string Notes { get; set; }

        [Column(TypeName = "money")]
        public decimal MinPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal MaxPrice { get; set; }

        public decimal MinSpace { get; set; }

        public decimal MaxSpace { get; set; }

        public int FK_LandsDemands_Branches_BranchId { get; set; }

        public int FK_LandsDemands_Users_SalesId { get; set; }

        public int FK_LandsDemands_Categories_Id { get; set; }

        public int FK_LandsDemands_Transactions_Id { get; set; }

        public int FK_LandsDemands_Regions_FromId { get; set; }

        public int FK_LandsDemands_Regions_ToId { get; set; }

        public int FK_LandsDemands_PaymentMethod_Id { get; set; }

        public bool IsMatched { get; set; }

        public bool IsClosed { get; set; }

        public bool Type { get; set; }

        public virtual tbl_Branches tbl_Branches { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_Clients tbl_Clients { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LandDemandViews> tbl_LandDemandViews { get; set; }

        public virtual tbl_PaymentMethods tbl_PaymentMethods { get; set; }

        public virtual tbl_Regions tbl_Regions { get; set; }

        public virtual tbl_Regions tbl_Regions1 { get; set; }

        public virtual tbl_Transactions tbl_Transactions { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }
    }
}
