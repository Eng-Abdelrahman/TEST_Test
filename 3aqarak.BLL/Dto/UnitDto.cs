using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
     public class UnitDto
    {
        public int PK_Units_Id { get; set; }

        public int FK_Units_Client_Id { get; set; }

        public int Space { get; set; }

        public int Rooms { get; set; }

        public int Bathrooms { get; set; }
      
        public int Floor { get; set; }

        public int FK_Units_Regions_Id { get; set; }
     
        //public string Address { get; set; }
        public string FlatNumber { get; set; }

        public string ApartmentNumber { get; set; }

        public string GroupNumber { get; set; }
        public int FK_Units_Finishing_Id { get; set; }

        public int FK_Units_Views_Id { get; set; }

        public bool IsFurniture { get; set; }

        public bool IsMarketResearch { get; set; }
                
        public string Descreption { get; set; }

        public int FK_Units_Categories_Id { get; set; }
    }
}
