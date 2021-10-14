namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_AvailableLands
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_AvailableLands()
        {
            tbl_LandImages = new HashSet<tbl_LandImages>();
        }

        [Key]
        public int PK_AvailableLands_Id { get; set; }

        public bool Type { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public decimal Space { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Description { get; set; }

        public string Notes { get; set; }

        public bool IsClosed { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsNegotiable { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsMarketSearch { get; set; }

        public int FK_AvailableLands_PaymentMethod_Id { get; set; }

        public int FK_AvaliableLands_Branches_BranchId { get; set; }

        public int FK_AvaliableLands_Transactions_TransactionId { get; set; }

        public int FK_AvaliableLands_Users_ModifiedBy { get; set; }

        public int FK_AvaliableLands_Users_CreatedBy { get; set; }

        public int FK_AvaliableLands_Clients_ClientId { get; set; }

        public int FK_AvaliableLands_Users_SalesId { get; set; }

        public int FK_AvailableLands_Views_ViewId { get; set; }

        public int FK_AvailabeLands_Regions_RegionId { get; set; }

        public int FK_AvailableLands_Categories_CategoryId { get; set; }

        public virtual tbl_Branches tbl_Branches { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_Clients tbl_Clients { get; set; }

        public virtual tbl_PaymentMethods tbl_PaymentMethods { get; set; }

        public virtual tbl_Regions tbl_Regions { get; set; }

        public virtual tbl_Transactions tbl_Transactions { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }

        public virtual tbl_Views tbl_Views { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LandImages> tbl_LandImages { get; set; }
    }
}
