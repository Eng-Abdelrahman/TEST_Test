using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces.Repositories
{
    public interface IShopCustomRepository
    {
        Task<List<tbl_ShopDemands>> DemandsByPayment(tbl_ShopAvailable ShopAvailableDto);
        Task<List<tbl_ShopDemands>> DemnadsByPaymentUsage(tbl_ShopAvailable ShopAvailable);
        Task<List<tbl_ShopDemands>> DemandsByUsage(tbl_ShopAvailable ShopAvailableDto);
        Task<List<tbl_ShopDemands>> DemandsByMasters(tbl_ShopAvailable ShopAvailableDto);
        Task<List<tbl_ShopAvailable>> AvailablesForPayment(tbl_ShopDemands demand, int[] codes);
        Task<List<tbl_ShopAvailable>> AvailablesForPaymentAndUsage(tbl_ShopDemands demand, int[] codes);
        Task<List<tbl_ShopAvailable>> AvailablesForMasters(tbl_ShopDemands demand, int[] codes);
        Task<List<tbl_ShopAvailable>> AvailablesForUsage(tbl_ShopDemands demand, int[] codes);
        Task<List<DemandsWithPreviews>> FilterDemandsHasPreviews(List<ShopDemandDto> empDemands, List<ShopDemandDto> colleguesDemands, ShopAvailableDto available);



    }
}
