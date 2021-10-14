namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_ShopDemandAccessories
    {
        [Key]
        public int PK_ShopDemandAccessories_ID { get; set; }

        public int FK_ShopDemandAccessories_ShopDemand_Id { get; set; }

        public int FK_ShopDemandAccessories_Accessories_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_ShopDemandAccessories_Users_CreatedBy { get; set; }

        public int FK_ShopDemandAccessories_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_Accessories tbl_Accessories { get; set; }

        public virtual tbl_ShopDemands tbl_ShopDemands { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }
    }
}
