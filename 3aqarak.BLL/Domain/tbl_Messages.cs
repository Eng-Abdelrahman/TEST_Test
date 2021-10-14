namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Messages
    {
        [Key]
        public int PK_Messages_Id { get; set; }

        public int FK_Messages_Users_SenderId { get; set; }

        public int FK_Messages_Users_RecieverId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateTimeStart { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateTimeEnd { get; set; }

        [Required]
        public string MessageContent { get; set; }

        public bool IsRead { get; set; }

        public bool IsDone { get; set; }

        public bool IsCritical { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }

        public virtual tbl_Users tbl_Users1 { get; set; }
    }
}
