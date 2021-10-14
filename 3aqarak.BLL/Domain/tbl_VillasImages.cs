namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_VillasImages
    {
        [Key]
        public int PK_UnitImages_Id { get; set; }

        public int FK_VillasImages_VillasAvailables_VillaId { get; set; }

        public string ImageUrl { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_UnitImages_Users_CreatedBy { get; set; }

        public int FK_UnitImages_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsMainImage { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_VillasAvailables tbl_VillasAvailables { get; set; }
    }
}
