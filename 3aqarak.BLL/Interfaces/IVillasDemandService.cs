using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.BLL.Interfaces
{
    public interface IVillasDemandService
    {
        Task<SelectList> GetRegions();
        Task<SelectList> GetRegionById(int regId);
        Task<MultiSelectList> GetFinishings();
        Task<MultiSelectList> GetFinishingsById(int[] ids);        
        Task<SelectList> GetTrans(string name);
        Task<SelectList> GetTransById(int regId, string name);
        Task<SelectList> GetPayments();
        Task<SelectList> GetPaymentsId(int regId);
        Task<SelectList> GetUsages();
        Task<SelectList> GetUsagesId(int usageId);
        Task<MultiSelectList> GetAccess();
        Task<MultiSelectList> GetAccessById(int[] ids);
        Task<MultiSelectList> GetViews();
        Task<MultiSelectList> GetViewsById(int[] ids);
        Task<IConfirmation> SaveVillaDemand(VillasDemandDto demand, int userId, int branchId);
        Task<IConfirmation> UpdateVillaDemand(VillasDemandDto demand, int userId, int branchId);
        Task<VillasDemandDto> EditVillaDemand(int id);
        Task<IConfirmation> DeleteVillaDemand(int id, int userId);
        Task<(List<VillasAvailableDto>, VillasAvailableDto, List<VillasAvailableDto>)> FilterAvailableMatches(VillasDemandDto demand);
        Task<List<tbl_VillasAvailables>> GetAvailableMatches(VillasDemandDto demand);
        Task<List<VillasAvailableDto>> GetAvailableMatchesOnTheFly(VillasDemandDto demand);
        Task<IConfirmation> GetInstantMatches(int demandId);
        Task<bool> CloseDemand(int id, int userId);
        Task<bool> ReleaseDemand(int id, int userId);
        Task<bool> UpdateDemandTransaction(int demandId, int transId);
        Task<List<VillasDemandDto>> DemandsByDateAndRegion(DateTime fromDate, DateTime toDate, int regionIdFrom, int regionIdTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Demand_Id);
        Task<List<VillasDemandDto>> villaDemands(int id);
        Task<IConfirmation> CreateDemandForAvailable(VillasAvailableDto available, int userId, int branchId, int clientId, string notes);

        Task<IConfirmation> SaveImportVillaDemand(VillasDemandDto demand, int userId, int branchId);

    }

}
