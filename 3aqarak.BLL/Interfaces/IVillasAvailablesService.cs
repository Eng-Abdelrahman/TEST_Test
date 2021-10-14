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
   public interface IVillasAvailablesService
    {
        Task<List<VillasAvailableDto>> ClientSales(int id);
        Task<List<VillasAvailableDto>> ClientBranchSalesByDateAndRegion(DateTime date, int regionId, int branchId);
        Task<SelectList> GetRegions();
        Task<SelectList> GetRegionById(int regId);
        Task<SelectList> GetFinishings();
        Task<SelectList> GetFinishingsById(int regId);
        Task<SelectList> GetCats();
        Task<SelectList> GetCatsById(int regId);
        Task<SelectList> GetTrans(string name);
        Task<SelectList> GetTransById(int regId, string name);
        Task<MultiSelectList> GetAccess();
        Task<MultiSelectList> GetAccessById(int[] ids);
        Task<SelectList> GetPayments();
        Task<SelectList> GetPaymentsId(int regId);
        Task<SelectList> GetViews();
        Task<SelectList> GetViewsById(int id);
        IConfirmation SavePhotos(HttpFileCollectionBase files);
        IConfirmation ValidatePhotos(HttpFileCollectionBase files);
        Task<IConfirmation> SaveClientSale(VillasAvailableDto sale, int userId, List<string> images, int branchId);
        Task<IConfirmation> SaveImportedVillasAvailable(ImportVillasAvailableDto sale, int userId, int branchId);
        Task<IConfirmation> UpdateClientSale(VillasAvailableDto sale, int userId, List<string> images, int branchId);
        Task<VillasAvailableDto> EditClientSale(int id);
        Task<IConfirmation> DeleteClientSale(int id, int userId);
        Task<(List<VillasDemandDto>, List<VillasDemandDto>)> GetAvailableMatches(VillasAvailableDto available, int userId);
        Task<IConfirmation> GetInstantMatches(int saleId, int userId);
        Task<bool> CloseAvailable(int id, int userId);
        Task<bool> UpdateAvailableTransaction(int availableId, int transId);
        Task<bool> ReleaseAvailable(int id, int userId);
        Task<IConfirmation> CheckDuplicateClientSales(VillasAvailableDto ClientSales);
        Task<List<VillasAvailableDto>> GetClosedSales(int id);
        Task<SelectList> GetUsages();
        //we need to modify that>>>>>>>>>>>>>>>>>>>>>>>>
        Task<List<VillasAvailableDto>> GetAllAveilableByDateAndRegion(DateTime from, DateTime to, int regionIdFrom, int regionIdTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo, 
            int Payments, int BasisOfInstallmentDropdown, int OverFrom, int OverTo, int RemainingFrom, int RemainingTo,
            int YearOfInstallmentFrom, int YearOfInstallmentTo, string VillaNumber, string GroupNumber, int Available_Id);
        Task<(List<VillasDemandDto>, VillasDemandDto, List<VillasDemandDto>, List<VillasDemandDto>)> FilterAvailables(VillasAvailableDto availableDto, int userId);
        Task<SelectList> GetPaymentsAsync();
        Task<SelectList> GetUsagesId(int usageId);
    }
}
