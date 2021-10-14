namespace _3aqarak.BLL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_PreviewDetails
    {
        [Key]
        public int PK_PreviewDetails_Id { get; set; }

        public int AvailableUnits_Id { get; set; }

        public int Fk_PreviewDetails_Clients_SellerId { get; set; }

        public bool IsSucceded { get; set; }

        public bool IsPostponed { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_PreviewDetails_Users_ModidfiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsRejected { get; set; }

        public int Fk_PreviewDetails_PreviewHeaders_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PostPoneDate { get; set; }

        [Required]
        [StringLength(50)]
        public string PreviewTime { get; set; }

        public bool IsNoDecision { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime PreviewHeaderDate { get; set; }

        public int Category_Id { get; set; }

        public virtual tbl_Clients tbl_Clients { get; set; }

        public virtual tbl_PreviewHeaders tbl_PreviewHeaders { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }
    }
}
