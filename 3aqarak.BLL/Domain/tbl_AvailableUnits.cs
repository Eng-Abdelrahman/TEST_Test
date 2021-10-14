namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_AvailableUnits
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_AvailableUnits()
        {
            tbl_Offers = new HashSet<tbl_Offers>();
        }

        [Key]
        public int PK_AvailableUnits_Id { get; set; }

        public int FK_AvaliableUnits_Units_UnitId { get; set; }

        public int FK_AvaliableUnits_Clients_ClientId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int FK_AvaliableUnits_Transaction_TransactionId { get; set; }

        public int FK_AvaliableUnits_Users_CreatedBy { get; set; }

        public string Notes { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        //NEW FEILD 
        [Column(TypeName = "money")]
        public decimal Remaining { get; set; }
        [Column(TypeName = "money")]
        public decimal Over { get; set; }
        public decimal YearOfInstallment { get; set; }
        [Range(1,4)]
        public byte? BasisOfInstallment { get; set; }

        public int FK_AvaliableUnits_Users_ModifiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_AvailableUnits_PaymentMethod_Id { get; set; }

        public bool IsNegotiable { get; set; }

        public bool IsClosed { get; set; }

        public int FK_AvaliableUnits_Branches_BranchId { get; set; }

        public int FK_AvaliableUnits_Users_SalesId { get; set; }

        public int NoOfElevators { get; set; }

        public int DateOfBuild { get; set; }

        public int FK_AvailableUnits_Usage_Id { get; set; }

        public decimal AdvancePayment { set; get; }

        public virtual tbl_Branches tbl_Branches { get; set; }

        public virtual tbl_PaymentMethods tbl_PaymentMethods { get; set; }

        public tbl_units tbl_units { get; set; }

        public virtual tbl_UnitUsage tbl_UnitUsage { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Offers> tbl_Offers { get; set; }
    }
}
