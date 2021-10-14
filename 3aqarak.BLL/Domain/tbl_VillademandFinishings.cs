namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_VillademandFinishings
    {
        [Key]
        public int PK_VillaDemandFinishing_Id { get; set; }

        public int FK_VillaDemandFinishing_VillaDemand_Id { get; set; }

        public int FK_VillaDemandFinishing_Finish_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_VillaDemandFinishing_Users_CreatedBy { get; set; }

        public int FK_VillaDemandFinishing_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_Finishings tbl_Finishings { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }

        public virtual tbl_VillasDemands tbl_VillasDemands { get; set; }
    }
}
