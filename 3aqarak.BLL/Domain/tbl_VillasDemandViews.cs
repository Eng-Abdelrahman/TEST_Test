namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_VillasDemandViews
    {
        [Key]
        public int PK_VillaDemandView_Id { get; set; }

        public int FK_VillaDemandView_View_ViewId { get; set; }

        public int FK_VillaDemandView_VillaDemand_DemandId { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_VillaDemandView_Users_CreatedBy { get; set; }

        public int FK_VillaDemandView_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_VillasDemands tbl_VillasDemands { get; set; }
    }
}
