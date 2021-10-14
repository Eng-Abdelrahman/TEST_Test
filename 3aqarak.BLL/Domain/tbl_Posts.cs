namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Posts
    {
        [Key]
        public int PK_PostId { get; set; }

        public int Unit_Id { get; set; }

        public int FK_Posts_Categories_Id { get; set; }

        public bool IsClosed { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(20)]
        public string Mobile2 { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string ContactHour { get; set; }

        public string Notes { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public virtual tbl_Categories tbl_Categories { get; set; }
    }
}
