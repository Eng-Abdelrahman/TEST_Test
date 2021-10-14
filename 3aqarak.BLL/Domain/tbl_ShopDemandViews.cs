namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_ShopDemandViews
    {
        [Key]
        public int PK_ShopDemandView_Id { get; set; }

        public int FK_ShopDemandView_View_ViewId { get; set; }

        public int FK_ShopDemandView_ShopDemand_DemandId { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_ShopDemandView_Users_CreatedBy { get; set; }

        public int FK_ShopDemandView_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_ShopDemands tbl_ShopDemands { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_Views tbl_Views { get; set; }
    }
}
