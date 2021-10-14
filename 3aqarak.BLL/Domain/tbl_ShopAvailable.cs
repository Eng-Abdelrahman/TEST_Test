namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_ShopAvailable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_ShopAvailable()
        {
            tbl_ShopAvailabeAccessories = new HashSet<tbl_ShopAvailabeAccessories>();
            tbl_ShopAvailableImages = new HashSet<tbl_ShopAvailableImages>();
        }

        [Key]
        public int PK_ShopAvailable_Id { get; set; }

        public int FK_ShopAvailable_Clients_ClientId { get; set; }

        public string Notes { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_ShopAvailable_Users_CreatedBy { get; set; }

        public int FK_ShopAvailable_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_ShopAvailable_Categories_Id { get; set; }

        public int FK_ShopAvailable_Transactions_Id { get; set; }

        public int FK_ShopAvailable_Regions_Id { get; set; }

        public int FK_ShopAvailable_PaymentMethod_Id { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public decimal Space { get; set; }

        public int BathRooms { get; set; }

        public bool IsFurnished { get; set; }

        public bool IsClosed { get; set; }

        public int FK_ShopAvailable_Branches_BranchId { get; set; }

        public int FK_ShopAvailable_Users_SalesId { get; set; }

        public bool IsMarketResearch { get; set; }

        [Required]
        public string Address { get; set; }

        public bool IsNegotiable { get; set; }

        public int DateOfBuild { get; set; }

        public int ScaleNumber { get; set; }

        public bool IsDivider { get; set; }

        public int FK_ShopAvailable_Usage_Id { get; set; }

        public bool Islicense { get; set; }

        public int FK_ShopAvialable_Views_Id { get; set; }

        public virtual tbl_Branches tbl_Branches { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_Clients tbl_Clients { get; set; }

        public virtual tbl_PaymentMethods tbl_PaymentMethods { get; set; }

        public virtual tbl_Regions tbl_Regions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailabeAccessories> tbl_ShopAvailabeAccessories { get; set; }

        public virtual tbl_Transactions tbl_Transactions { get; set; }

        public virtual tbl_UnitUsage tbl_UnitUsage { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }

        public virtual tbl_Views tbl_Views { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailableImages> tbl_ShopAvailableImages { get; set; }
    }
}
