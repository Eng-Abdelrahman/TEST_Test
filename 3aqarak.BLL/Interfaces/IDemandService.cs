using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.BLL.Interfaces
{
    public interface IDemandService
    {
        Task<List<DemandDto>> ClientDemands(int id);
        Task<bool> CloseDemand(int id, int userId);
        Task<IConfirmation> CreateDemandForAvailable(AvailableDto available, int userId, int branchId, int clientId, string notes);
        Task<IConfirmation> DeleteClientDemand(int id, int userId);
        Task<List<DemandDto>> DemandsByDateAndRegion(DateTime fromDate, DateTime toDate, int cat, int regionIdFrom, int regionIdTo, int ElevatorFrom, int ElevatorTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Demand_Id);
        Task<DemandDto> EditClientDemand(int id);
        Task<(List<AvailableDto>, AvailableDto, List<AvailableDto>)> FilterAvailableMatches(DemandDto demand);
        Task<MultiSelectList> GetAccess();
        Task<MultiSelectList> GetAccessById(int[] ids);
        Task<List<tbl_AvailableUnits>> GetAvailableMatches(DemandDto demand);
        Task<List<AvailableDto>> GetAvailableMatchesOnTheFly(DemandDto demand);
        Task<SelectList> GetCats();
        Task<SelectList> GetCatsById(int id);
        Task<MultiSelectList> GetFinishings();
        Task<MultiSelectList> GetFinishingsById(int[] ids);
        Task<IConfirmation> GetInstantMatches(int demandId);
        Task<SelectList> GetPayments();
        Task<SelectList> GetPaymentsId(int id);
        Task<SelectList> GetRegionById(int id);
        Task<SelectList> GetRegions();
        Task<List<AvailableDto>> GetSellerName(List<AvailableDto> availableDtos);
        Task<SelectList> GetTrans(string name);
        Task<SelectList> GetTransById(int id, string name);
        Task<SelectList> GetUsages();
        Task<SelectList> GetUsagesId(int usageId);
        Task<MultiSelectList> GetViews();
        Task<MultiSelectList> GetViewsById(int[] ids);
        Task<bool> ReleaseDemand(int id, int userId);
        Task<IConfirmation> SaveClientDemand(DemandDto demand, int userId, int branchId);
        Task<IConfirmation> UpdateClientDemand(DemandDto demand, int userId, int branchId);
        Task<bool> UpdateDemandTransaction(int demandId, int transCode);
        Task<IConfirmation> SaveImportDemand(DemandDto demand, int userId, int branchId);

    }
}
