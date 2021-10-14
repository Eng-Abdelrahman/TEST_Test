namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_LandDemandViews
    {
        [Key]
        public int PK_LandDemandView_Id { get; set; }

        public int FK_LandDemandView_View_ViewId { get; set; }

        public int FK_LandDemandView_LandDemand_DemandId { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_LandDemandView_Users_CreatedBy { get; set; }

        public int FK_LandDemandView_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_LandsDemands tbl_LandsDemands { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Views tbl_Views { get; set; }
    }
}
