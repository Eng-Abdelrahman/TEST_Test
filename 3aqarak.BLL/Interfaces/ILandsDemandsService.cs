using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Models;
namespace _3aqarak.BLL.Interfaces
{
    public interface ILandsDemandsService
    {
        Task<bool> CloseLandDemand(int id, int userId);
        Task<IConfirmation> CreateDemandForAvailable(AvailableLandsDto available, int userId, int branchId, int clientId, string notes);
        Task<IConfirmation> DeleteLandDemand(int id, int userId);
        Task<List<LandsDemandsDto>> DemandsByDateAndRegion(DateTime fromDate, DateTime toDate, int regionIdFrom, int regionIdTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Demand);
        Task<LandsDemandsDto> EditLandDemand(int id);
        Task<(List<AvailableLandsDto>, AvailableLandsDto, List<AvailableLandsDto>)> FilterLandAvailable(LandsDemandsDto landsDemandsDto);
        Task<IConfirmation> GetInstantMatches(int demandId);
        Task<List<tbl_AvailableLands>> GetLandAvailableMatch(LandsDemandsDto landsDemandsDto);
        Task<List<AvailableLandsDto>> GetLandAvailableMatchOnFly(LandsDemandsDto landsDemandsDto);
        Task<SelectList> GetPayments();
        Task<SelectList> GetPaymentsId(int id);
        Task<SelectList> GetRegionById(int id);
        Task<SelectList> GetRegions();
        Task<List<AvailableLandsDto>> GetSellerName(List<AvailableLandsDto> availableLandsDtos);
        Task<SelectList> GetTrans(string name);
        Task<SelectList> GetTransactions();
        Task<SelectList> GetTransById(int regId, string name);
        Task<SelectList> GetViews();
        Task<MultiSelectList> GetViewsById(int[] ids);
        Task<List<LandsDemandsDto>> LandDemands(int id);
        Task<bool> ReleaseLandDemand(int id, int userId);
        Task<IConfirmation> SaveLandsDemand(LandsDemandsDto landsDemandsDto, int userId, int branchId);
        Task<IConfirmation> UpdateClientDemand(LandsDemandsDto landsDemandsDto, int userId, int branchId);
        Task<bool> UpdateLandDemandTransaction(int demandId, int transCode);

    }
}
