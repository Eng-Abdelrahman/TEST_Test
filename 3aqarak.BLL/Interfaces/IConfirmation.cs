using _3aqarak.BLL.Dto;
using System.Collections.Generic;

namespace _3aqarak.BLL.Interfaces
{
    public interface IConfirmation
    {
        int Id { get; set; }
        int Index { get; set; }
        bool Valid { get; set; }
        string Url { get; set; }       
        string Salt { get; set; }
        string Message { get; set; }
        string Password { get; set; }
        int LoginStatus { get; set; }
        PostsDto Post { get; set; }
        List<string> UrlList { get; set; }
        List<ClientDto> Clients { get; set; }

        List<DemandDto> Demands { get; set; }
        List<VillasDemandDto> VillDemands { get; set; }
        List<LandsDemandsDto> landsDemands { get; set; }
        List<ShopDemandDto> ShopDemands { get; set; }

        List<AvailableDto> availables { get; set; }
        List<VillasAvailableDto> VillAvailables { get; set; }
        List<AvailableLandsDto> LandAvailables { get; set; }
        List<ShopAvailableDto> ShopAvailables { get; set; }

        (List<VillasAvailableDto>, VillasAvailableDto, List<VillasAvailableDto>) VillAvailablesAndExcluded { get; set; }
        (List<AvailableDto>, AvailableDto, List<AvailableDto>) AvailablesAndExcluded { get; set; }
        (List<AvailableLandsDto>, AvailableLandsDto, List<AvailableLandsDto>) LandsAvailableAndExcluded { get; set; }
        (List<ShopAvailableDto>, ShopAvailableDto, List<ShopAvailableDto>) ShopAvailableAndExcluded { get; set; }    

        (List<DemandDto>, DemandDto, List<DemandDto>, List<DemandDto>) DemandsAndExcluded { get; set; }
        (List<VillasDemandDto>, VillasDemandDto, List<VillasDemandDto>, List<VillasDemandDto>) VillDemandsAndExcluded { get; set; }
        (List<LandsDemandsDto>, LandsDemandsDto, List<LandsDemandsDto>, List<LandsDemandsDto>) LandsDemandsAndExcluded { get; set; } 
        (List<ShopDemandDto>, ShopDemandDto, List<ShopDemandDto>,List<ShopDemandDto>) ShopDemandsAndExcluded { get; set; }
    }
}
