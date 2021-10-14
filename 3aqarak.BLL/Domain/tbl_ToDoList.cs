namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
  

    public partial class tbl_ToDoList
    {
        [Key]
        public int PK_ToDoList { get; set; }

        public int FK_ToDoList_Users_Employee { get; set; }

        [Required]
        [StringLength(100)]
        public string Task { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime Deadline { get; set; }

        [Column(TypeName = "date")]
        public DateTime TaskEndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Result { get; set; }

        [Required]
        public string Notes { get; set; }
    }
}
