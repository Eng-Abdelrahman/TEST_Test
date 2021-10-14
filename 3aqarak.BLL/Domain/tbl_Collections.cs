namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Collections
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Collections()
        {
            tbl_EmpCollections = new HashSet<tbl_EmpCollections>();
        }

        [Key]
        public int PK_Collections_Id { get; set; }

        [Required]
        [StringLength(15)]
        public string CollectionName { get; set; }

        public decimal EmployeeCommission { get; set; }

        public decimal CompanyCommession { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_Collections_Users_CreatedBy { get; set; }

        public int FK_Collections_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EmpCollections> tbl_EmpCollections { get; set; }
    }
}
