namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_Specializations
    {
        [Key]
        public int PK_Specialization_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_Specialization_Users_CreatedBy { get; set; }

        public int FK_Specialization_Users_ModidfiedBy { get; set; }

        public int FK_Specialization_Dept_DeptId { get; set; }

        public virtual tbl_Departements tbl_Departements { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }
    }
}
