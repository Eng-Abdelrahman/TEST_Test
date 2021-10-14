using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace _3aqarak.BLL.Interfaces
{
    public interface IAvailableLandsSevice
    {
        Task<IConfirmation> CheckDuplicateLandSales(AvailableLandsDto availableLandsDto);
        Task<List<AvailableLandsDto>> ClientBranchSalesByDateAndRegion(DateTime date, int regionId, int branchId);
        Task<bool> CloseLandAvailable(int id, int userId);
        Task<IConfirmation> DeleteLandAvailable(int id, int userId);
        Task<AvailableLandsDto> EditAvailableLand(int id);
        Task<(List<LandsDemandsDto>, LandsDemandsDto, List<LandsDemandsDto>, List<LandsDemandsDto>)> FilterLandAvailable(AvailableLandsDto availableLandsDto, int userId);
        Task<List<AvailableLandsDto>> GetAllAveilableByDateAndRegion(DateTime from, DateTime to, int regionIdFrom, int regionIdTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Available_Id);
        Task<(List<LandsDemandsDto>, List<LandsDemandsDto>)> GetAvailableLandMatches(AvailableLandsDto availableLandsDto, int userId);
        Task<List<LandsDemandsDto>> GetBuyerName(List<LandsDemandsDto> landsDemandsDtos);
        Task<List<AvailableLandsDto>> GetClosedLandAvailable(int id);
        Task<IConfirmation> GetInstantMatches(int saleId, int userId);
        Task<SelectList> GetPayments();
        Task<SelectList> GetPaymentsId(int id);
        Task<SelectList> GetRegionById(int id);
        Task<SelectList> GetRegions();
        Task<SelectList> GetTrans(string name);
        Task<SelectList> GetTransactions();
        Task<SelectList> GetTransById(int regId, string name);
        Task<SelectList> GetViews();
        Task<SelectList> GetViewsById(int id);
        Task<bool> ReleaseLandAvailable(int id, int userId);
        Task<IConfirmation> SaveLandsSale(AvailableLandsDto availableLandsDto, int userId, List<string> images, int branchId);
        IConfirmation SavePhotos(HttpFileCollectionBase files);
        Task<IConfirmation> UpdateAvailableLand(AvailableLandsDto sale, int userId, List<string> images, int branchId);
        Task<bool> UpdateLandAvailableTransaction(int availableId, int transCode);
        IConfirmation ValidatePhotos(HttpFileCollectionBase files);
    }
}
