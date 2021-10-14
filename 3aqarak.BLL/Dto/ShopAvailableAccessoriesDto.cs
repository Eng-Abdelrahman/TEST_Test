using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class ShopAvailableAccessoriesDto
    {
        public int PK_ShopAccessories_ID { get; set; }

        public int FK_ShopAccessories_ShopAvailable_Id { get; set; }

        public int FK_ShopAccessories_Accessories_Id { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public int FK_ShopAccessories_Users_CreatedBy { get; set; }

        public int FK_ShopAccessories_Users_ModidfiedBy { get; set; }
        
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
