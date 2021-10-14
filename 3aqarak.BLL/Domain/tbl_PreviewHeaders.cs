namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_PreviewHeaders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_PreviewHeaders()
        {
            tbl_ExpectedContracts = new HashSet<tbl_ExpectedContracts>();
            tbl_PreviewDetails = new HashSet<tbl_PreviewDetails>();
        }

        [Key]
        public int PK_PreviewHeaders_Id { get; set; }

        public int FK_PreviewHeaders_Clients_BuyerId { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_PreviewHeaders_Users_CreatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public int DemandUnit_Id { get; set; }

        public bool IsSucceded { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsSuspended { get; set; }

        public bool IsRejected { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ReviewDate { get; set; }

        public bool IsNoDecision { get; set; }

        public int Category_Id { get; set; }

        public virtual tbl_Clients tbl_Clients { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ExpectedContracts> tbl_ExpectedContracts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PreviewDetails> tbl_PreviewDetails { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }
    }
}
