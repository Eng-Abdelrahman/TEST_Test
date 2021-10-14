namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_EventLogs
    {
        [Key]
        public int PK_Event_Id { get; set; }

        public int FK_Event_Users_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string TableName { get; set; }

        [Required]
        [StringLength(10)]
        public string EventType { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }
    }
}
