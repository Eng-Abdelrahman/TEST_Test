using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3aqarak.BLL.Dto;

namespace _3aqarak.BLL.Helpers
{
    public class Confirmation : IConfirmation
    {
        public bool Valid { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public List<string> UrlList { get; set; }

        public int Index { get; set; }

        public int LoginStatus { get; set; }

        public List<AvailableDto> availables { get; set; }

        public List<VillasAvailableDto> VillAvailables { get; set; }

        public List<DemandDto> Demands { get; set; }

        public List<VillasDemandDto> VillDemands { get; set; }

        public List<ClientDto> Clients { get; set; }

        public (List<AvailableDto>, AvailableDto, List<AvailableDto>) AvailablesAndExcluded { get; set; }

        public (List<VillasAvailableDto>, VillasAvailableDto, List<VillasAvailableDto>) VillAvailablesAndExcluded { get; set; }


        //welcome in my part this's Mostafa's Part 
        public List<ShopDemandDto> ShopDemands { get; set; }


        public int Id { get; set; }

        public (List<AvailableLandsDto>, AvailableLandsDto, List<AvailableLandsDto>) LandsAvailableAndExcluded { get; set; }
        public List<AvailableLandsDto> LandAvailables { get; set; }
        public List<LandsDemandsDto> landsDemands { get; set; }


        public (List<ShopAvailableDto>, ShopAvailableDto, List<ShopAvailableDto>) ShopAvailableAndExcluded { get; set; }
        public List<ShopAvailableDto> ShopAvailables { get; set; }
        (List<DemandDto>, DemandDto, List<DemandDto>, List<DemandDto>) IConfirmation.DemandsAndExcluded { get; set; }
        (List<VillasDemandDto>, VillasDemandDto, List<VillasDemandDto>, List<VillasDemandDto>) IConfirmation.VillDemandsAndExcluded { get; set; }
        (List<LandsDemandsDto>, LandsDemandsDto, List<LandsDemandsDto>, List<LandsDemandsDto>) IConfirmation.LandsDemandsAndExcluded { get; set; }
        (List<ShopDemandDto>, ShopDemandDto, List<ShopDemandDto>, List<ShopDemandDto>) IConfirmation.ShopDemandsAndExcluded { get; set; }
        public PostsDto Post { get ; set ; }

        public Confirmation()
        {
            UrlList = new List<string>();
            Demands = new List<DemandDto>();
            availables = new List<AvailableDto>();
            VillAvailables = new List<VillasAvailableDto>();
            VillDemands = new List<VillasDemandDto>();
            ShopDemands = new List<ShopDemandDto>();
            Clients = new List<ClientDto>();
            LandAvailables = new List<AvailableLandsDto>(); //land
            landsDemands = new List<LandsDemandsDto>(); //land
            ShopAvailables = new List<ShopAvailableDto>();
        }
    }

}



