
using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.BLL.Interfaces
{
   public interface IShopDemandService
    {
        Task<bool> CloseDemand(int id, int userId);
        Task<IConfirmation> CreateDemandForAvailable(ShopAvailableDto available, int userId, int branchId, int clientId, string notes);
        Task<IConfirmation> DeleteShopDemand(int id, int userId);
        Task<List<ShopDemandDto>> DemandsByDateAndRegion(DateTime fromDate, DateTime toDate, int regionIdFrom, int regionIdTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo, int Demand_Id);
        Task<ShopDemandDto> EditShopDemand(int id);
        Task<(List<ShopAvailableDto>, ShopAvailableDto, List<ShopAvailableDto>)> FilterAvailableMatches(ShopDemandDto demand);
        Task<MultiSelectList> GetAccess();
        Task<MultiSelectList> GetAccessById(int[] ids);
        Task<List<tbl_ShopAvailable>> GetAvailableMatches(ShopDemandDto demand);
        Task<List<ShopAvailableDto>> GetAvailableMatchesOnTheFly(ShopDemandDto demand);
        Task<IConfirmation> GetInstantMatches(int demandId);
        Task<SelectList> GetPayments();
        Task<SelectList> GetPaymentsId(int id);
        Task<SelectList> GetRegionById(int id);
        Task<SelectList> GetRegions();
        Task<string> GetShortDescAndDate(ShopDemandDto shopDemandDto);
        Task<SelectList> GetTrans(string name);
        Task<SelectList> GetTransById(int id, string name);
        Task<SelectList> GetUsages();
        Task<SelectList> GetUsagesId(int usageId);
        Task<MultiSelectList> GetViews();
        Task<MultiSelectList> GetViewsById(int[] ids);
        Task<bool> ReleaseDemand(int id, int userId);
        Task<IConfirmation> SaveShopDemand(ShopDemandDto demand, int userId, int branchId);
        List<ShopDemandDto> ShopClientDemands(int id);
        Task<List<ShopDemandDto>> ShopDemands(int id);
        Task<bool> UpdateDemandTransaction(int demandId, int transCode);
        Task<IConfirmation> UpdateShopDemand(ShopDemandDto demand, int userId, int branchId);
    }
}