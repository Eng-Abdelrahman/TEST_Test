using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
     public class ShopAvailableImagesDto
    {
        public int PK_ShopAvailableImages_Id { get; set; }

        public int FK_ShopAvailableImages_ShopAvailable_Id { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public int FK_ShopAvailableImages_Users_CreatedBy { get; set; }

        public int FK_ShopAvailableImages_Users_ModidfiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsMainImage { get; set; }
    }
}
