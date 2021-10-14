namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Demand_Finishings
    {
        [Key]
        public int PK_DemandFinishing_Id { get; set; }

        public int FK_DemandFinishing_Demand_Id { get; set; }

        public int FK_DemandFinishing_Finish_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int FK_DemandFinishing_Users_CreatedBy { get; set; }

        public int FK_DemandFinishing_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual tbl_DemandUnits tbl_DemandUnits { get; set; }

        public virtual tbl_Finishings tbl_Finishings { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }
    }
}
