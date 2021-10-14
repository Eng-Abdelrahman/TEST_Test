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
    public interface IAvailableService
    {

        Task<IConfirmation> CheckDuplicateClientSales(AvailableDto ClientSales);
        Task<List<AvailableDto>> ClientBranchSalesByDateAndRegion(DateTime date, int regionId, int branchId);
        Task<List<AvailableDto>> ClientSales(int id);
        Task<bool> CloseAvailable(int id, int userId);
        Task<IConfirmation> DeleteClientSale(int id, int userId);
        Task<AvailableDto> EditClientSale(int id);
        Task<(List<DemandDto>, DemandDto, List<DemandDto>, List<DemandDto>)> FilterAvailables(AvailableDto available, int userId);
        Task<MultiSelectList> GetAccessAsync();
        Task<MultiSelectList> GetAccessByIdAsync(int[] ids);
        Task<List<AvailableDto>> GetAllAveilableByDateAndRegion(DateTime from, DateTime to, int cat, int regionIdFrom, int regionIdTo, int ElevatorFrom, int ElevatorTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,
            int Payments, int BasisOfInstallmentDropdown, int OverFrom, int OverTo, int RemainingFrom, int RemainingTo,
            int YearOfInstallmentFrom, int YearOfInstallmentTo, string FlatNumber, string ApartmentNumber, string GroupNumber,int Available_Id);
        Task<(List<DemandDto>, List<DemandDto>)> GetAvailableMatches(AvailableDto available, int userId);
        Task<SelectList> GetCatsAsync();
        Task<SelectList> GetCatsById(int id);
        Task<List<AvailableDto>> GetClosedSalesByClient(int id);
        Task<List<DemandDto>> GetDemandsBuyerName(List<DemandDto> demandDtos);
        Task<SelectList> GetFinishingsAsync();
        Task<SelectList> GetFinishingsByIdAsync(int id);
        Task<IConfirmation> GetInstantMatches(int saleId, int userId);
        Task<SelectList> GetPaymentsAsync();
        Task<SelectList> GetPaymentsIdAsync(int id);
        Task<SelectList> GetRegionById(int id);
        Task<SelectList> GetRegionsAsync();
        Task<SelectList> GetTransAsync(string name);
        Task<SelectList> GetTransById(int id, string name);
        Task<SelectList> GetViewsAsync();
        Task<SelectList> GetViewsByIdAsync(int id);
        Task<bool> ReleaseAvailable(int id, int userId);
        Task<IConfirmation> SaveClientSale(AvailableDto sale, int userId, List<string> images, int branchId);
        Task<IConfirmation> SaveImportedClientSale(AvailableDto sale, int userId, int branchId);
        IConfirmation SavePhotos(HttpFileCollectionBase files);
        Task<bool> UpdateAvailableTransaction(int availableId, int transCode);
        Task<IConfirmation> UpdateClientSale(AvailableDto sale, int userId, List<string> images, int branchId);
        IConfirmation ValidatePhotos(HttpFileCollectionBase files);
    }
}
