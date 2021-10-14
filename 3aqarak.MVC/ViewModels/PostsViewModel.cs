using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _3aqarak.MVC.ViewModels
{
    public class PostsViewModel
    {
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

        public string CreatedAt { get; set; }


    }
}