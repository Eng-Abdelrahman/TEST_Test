namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_VillasAvailables
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_VillasAvailables()
        {
            tbl_VillaAccessories = new HashSet<tbl_VillaAccessories>();
            tbl_VillasImages = new HashSet<tbl_VillasImages>();
        }

        [Key]
        public int PK_VillasAvailables_Id { get; set; }

        public int FK_VillasAvailables_Clients_ClientId { get; set; }

        public string Notes { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_VillasAvailables_Users_CreatedBy { get; set; }

        public int FK_VillasAvailables_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_VillasAvailables_Categories_Id { get; set; }

        public int FK_VillasAvailables_Transactions_Id { get; set; }

        public int FK_VillasAvailables_Regions_Id { get; set; }

        public int FK_VillasAvailables_PaymentMethod_Id { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        //NEW FEILD 
        public decimal AdvancePayment { set; get; }
        [Column(TypeName = "money")] 
        public decimal Remaining { get; set; }
        [Column(TypeName = "money")]
        public decimal Over { get; set; }
        public decimal YearOfInstallment { get; set; }
        [Range(1, 4)]
        public byte? BasisOfInstallment { get; set; }

        public decimal Space { get; set; }

        public decimal AreaSpace { get; set; }

        public int BathRooms { get; set; }

        public int Rooms { get; set; }

        public bool IsFurnished { get; set; }

        public bool IsClosed { get; set; }

        public int FK_VillasAvailables_Branches_BranchId { get; set; }

        public int FK_VillasAvailables_Users_SalesId { get; set; }

        public int NoOfElevators { get; set; }

        public int DateOfBuild { get; set; }

        public int FK_VillasAvailables_Usage_Id { get; set; }

        public int FK_VillasAvailables_View_Id { get; set; }

        public int FK_VillasAvailables_Finishings_Id { get; set; }

        public bool IsNegotiable { get; set; }
  
        public string VillaNumber { get; set; }
      
        public string GroupNumber { get; set; }
        
        public bool IsMarketResearch { get; set; }

        public virtual tbl_Branches tbl_Branches { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_Clients tbl_Clients { get; set; }

        public virtual tbl_Finishings tbl_Finishings { get; set; }

        public virtual tbl_PaymentMethods tbl_PaymentMethods { get; set; }

        public virtual tbl_Regions tbl_Regions { get; set; }

        public virtual tbl_Transactions tbl_Transactions { get; set; }

        public virtual tbl_UnitUsage tbl_UnitUsage { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Users tbl_Users2 { get; set; }

        public virtual tbl_Views tbl_Views { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillaAccessories> tbl_VillaAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasImages> tbl_VillasImages { get; set; }
    }
}
