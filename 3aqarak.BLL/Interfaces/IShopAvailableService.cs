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
    public interface IShopAvailableService
    {
        Task<IConfirmation> CheckDuplicateClientShopAvailable(ShopAvailableDto ShopAvailableDto);
        Task<List<ShopAvailableDto>> ClientBranchSalesByDateAndRegion(DateTime date, int regionId, int branchId);
        Task<bool> CloseShopAvailable(int id, int userId);
        Task<IConfirmation> DeleteShopAvailable(int id, int userId);
        Task<ShopAvailableDto> EditAvailableShop(int id);
        Task<(List<ShopDemandDto>, ShopDemandDto, List<ShopDemandDto>, List<ShopDemandDto>)> FilterShopAvailables(ShopAvailableDto shopAvailableDto, int userId);
        Task<MultiSelectList> GetAccess();
        Task<MultiSelectList> GetAccessById(int[] ids);
        Task<List<ShopAvailableDto>> GetAllAveilableByDateAndRegion(DateTime from, DateTime to, int regionIdFrom, int regionIdTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo, int Available_Id);
        Task<List<ShopAvailableDto>> GetClosedShopAvailable(int id);
        Task<IConfirmation> GetInstantMatches(int saleId, int userId);
        Task<SelectList> GetPayments();
        Task<SelectList> GetPaymentsId(int id);
        Task<SelectList> GetRegionById(int id);
        Task<SelectList> GetRegions();
        Task<(List<ShopDemandDto>, List<ShopDemandDto>)> GetShopAvailableMatches(ShopAvailableDto ShopAvailableDto, int userId);
        Task<List<ShopDemandDto>> GetShopDemandsBuyerName(List<ShopDemandDto> shopDemandDtos);
        Task<SelectList> GetTrans(string name);
        Task<SelectList> GetTransactions();
        Task<SelectList> GetTransById(int id, string name);
        Task<SelectList> GetUsages();
        Task<SelectList> GetUsagesId(int usageId);
        Task<MultiSelectList> GetViews();
        Task<MultiSelectList> GetViewsById(int[] ids);
        Task<bool> ReleaseShopAvailable(int id, int userId);
        IConfirmation SavePhotos(HttpFileCollectionBase files);
        Task<IConfirmation> SaveShopAvailable(ShopAvailableDto shopAvailableDto, int userId, List<string> images, int branchId);
        Task<IConfirmation> UpdateAvailableLand(ShopAvailableDto sale, int userId, List<string> images, int branchId);
        Task<bool> UpdateShopAvailableTransaction(int availableId, int transId);
        IConfirmation ValidatePhotos(HttpFileCollectionBase files);

    }
}
