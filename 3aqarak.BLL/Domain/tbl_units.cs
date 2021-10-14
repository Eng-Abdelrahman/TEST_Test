namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_units
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_units()
        {
            tbl_AvailableUnits = new HashSet<tbl_AvailableUnits>();
            tbl_UnitAccessories = new HashSet<tbl_UnitAccessories>();
            tbl_UnitImages = new HashSet<tbl_UnitImages>();
        }

        [Key]
        public int PK_Units_Id { get; set; }

        public int FK_Units_Client_Id { get; set; }

        public int Space { get; set; }

        public int Rooms { get; set; }

        public int Bathrooms { get; set; }

        public int Floor { get; set; }

        public int FK_Units_Regions_Id { get; set; }

        //[Required]
        //public string Address { get; set; }

            
        public string FlatNumber { get; set; }

        public string ApartmentNumber { get; set; }

        public string GroupNumber { get; set; }

        public int FK_Units_Finishing_Id { get; set; }

        public bool IsFurniture { get; set; }

        public bool IsMarketResearch { get; set; }

        [Required(ErrorMessage = "الرجاء Y")]
        public string Descreption { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_Units_Users_CreatedBy { get; set; }

        public int FK_Units_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_Units_Views_Id { get; set; }

        public int FK_Units_Categories_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AvailableUnits> tbl_AvailableUnits { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }

        public virtual tbl_Regions tbl_Regions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_UnitAccessories> tbl_UnitAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_UnitImages> tbl_UnitImages { get; set; }

        public virtual tbl_Views tbl_Views { get; set; }
    }
}
