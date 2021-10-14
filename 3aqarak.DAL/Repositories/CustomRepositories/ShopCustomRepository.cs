using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces.Repositories;
using _3aqarak.BLL.Models;
using _3aqarak.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.DAL.Repositories.CustomRepositories
{
    public class ShopCustomRepository : IShopCustomRepository
    {
        private readonly RealEstateDB _context;

        public ShopCustomRepository(RealEstateDB context)
        {
            _context = context;
        }

        public async Task<List<tbl_ShopDemands>> DemandsByMasters(tbl_ShopAvailable ShopAvailable)
        {
            var loadedAvailable = await _context.tbl_ShopAvailable
                .Where(a => a.PK_ShopAvailable_Id == ShopAvailable.PK_ShopAvailable_Id)
                .Include(a => a.tbl_Regions).Include(a => a.tbl_PaymentMethods)
                .FirstOrDefaultAsync();

            return await _context.tbl_ShopDemands
                .Include(d => d.tbl_Regions).Include(d => d.tbl_Regions1)
                .Include(d => d.tbl_PaymentMethods)
                .Where(sd => !sd.IsClosed && !sd.IsDeleted
                                                    && sd.MaxPrice <= loadedAvailable.Price && sd.MinPrice >= loadedAvailable.Price
                                                    && sd.MaxSpace <= loadedAvailable.Space && sd.MinSpace >= loadedAvailable.Space
                                                    && sd.MaxBathRooms <= loadedAvailable.BathRooms && sd.MinBathRooms >= loadedAvailable.BathRooms
                                                    && sd.FK_ShopDemands_Categories_Id == loadedAvailable.FK_ShopAvailable_Categories_Id
                                                    && sd.FK_ShopDemands_Transactions_Id == loadedAvailable.FK_ShopAvailable_Transactions_Id &&
                                                    (
                                                        (
                                                            (sd.tbl_Regions.RegCode) <= (loadedAvailable.tbl_Regions.RegCode) &&
                                                            (sd.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_Regions.RegCode)
                                                        )
                                                        ||
                                                        (
                                                            (sd.tbl_Regions.RegCode) >= (loadedAvailable.tbl_Regions.RegCode) &&
                                                            (sd.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_Regions.RegCode)
                                                        )
                                                    )
                                                    && sd.IsFurnisher == loadedAvailable.IsFurnished
                                                    && sd.IsDivider == loadedAvailable.IsDivider
                                                    ).ToListAsync();
        }

        public async Task<List<tbl_ShopDemands>> DemandsByPayment(tbl_ShopAvailable ShopAvailable)
        {

            var loadedAvailable = await _context.tbl_ShopAvailable
                .Where(a => a.PK_ShopAvailable_Id == ShopAvailable.PK_ShopAvailable_Id)
                .Include(a => a.tbl_Regions).Include(a => a.tbl_PaymentMethods)
                .FirstOrDefaultAsync();

            return await _context.tbl_ShopDemands
                .Include(d => d.tbl_Regions).Include(d => d.tbl_Regions1)
                .Include(d => d.tbl_PaymentMethods)
                .Where(sd => !sd.IsClosed && !sd.IsDeleted
                                                    && sd.MaxPrice <= loadedAvailable.Price && sd.MinPrice >= loadedAvailable.Price
                                                    && sd.MaxSpace <= loadedAvailable.Space && sd.MinSpace >= loadedAvailable.Space
                                                    && sd.MaxBathRooms <= loadedAvailable.BathRooms && sd.MinBathRooms >= loadedAvailable.BathRooms
                                                    && sd.FK_ShopDemands_Categories_Id == loadedAvailable.FK_ShopAvailable_Categories_Id
                                                    && sd.FK_ShopDemands_Transactions_Id == loadedAvailable.FK_ShopAvailable_Transactions_Id &&
                                                    sd.FK_ShopDemands_PaymentMethod_Id == loadedAvailable.FK_ShopAvailable_PaymentMethod_Id &&
                                                    (
                                                        (
                                                            (sd.tbl_Regions.RegCode) <= (loadedAvailable.tbl_Regions.RegCode) &&
                                                            (sd.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_Regions.RegCode)
                                                        )
                                                        ||
                                                        (
                                                            (sd.tbl_Regions.RegCode) >= (loadedAvailable.tbl_Regions.RegCode) &&
                                                            (sd.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_Regions.RegCode)
                                                        )
                                                    )
                                                    && sd.IsFurnisher == loadedAvailable.IsFurnished
                                                    && sd.IsDivider == loadedAvailable.IsDivider
                                                    ).ToListAsync();
        }

        public async Task<List<tbl_ShopDemands>> DemandsByUsage(tbl_ShopAvailable ShopAvailable)
        {
            var loadedAvailable = await _context.tbl_ShopAvailable
              .Where(a => a.PK_ShopAvailable_Id == ShopAvailable.PK_ShopAvailable_Id)
              .Include(a => a.tbl_Regions).Include(a => a.tbl_PaymentMethods)
              .FirstOrDefaultAsync();
            return await _context.tbl_ShopDemands
                           .Include(d => d.tbl_Regions).Include(d => d.tbl_Regions1)
                           .Include(d => d.tbl_PaymentMethods)
                           .Where(sd => !sd.IsClosed && !sd.IsDeleted
                                                               && sd.MaxPrice <= loadedAvailable.Price && sd.MinPrice >= loadedAvailable.Price
                                                               && sd.MaxSpace <= loadedAvailable.Space && sd.MinSpace >= loadedAvailable.Space
                                                               && sd.MaxBathRooms <= loadedAvailable.BathRooms && sd.MinBathRooms >= loadedAvailable.BathRooms
                                                               && sd.FK_ShopDemands_Categories_Id == loadedAvailable.FK_ShopAvailable_Categories_Id
                                                               && sd.FK_ShopDemands_Transactions_Id == loadedAvailable.FK_ShopAvailable_Transactions_Id &&
                                                               (sd.FK_ShopDemands_Usage_Id == loadedAvailable.FK_ShopAvailable_Usage_Id || sd.FK_ShopDemands_Usage_Id == UnitUsages.Multiple)
                                                               &&
                                                               (
                                                                   (
                                                                       (sd.tbl_Regions.RegCode) <= (loadedAvailable.tbl_Regions.RegCode) &&
                                                                       (sd.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_Regions.RegCode)
                                                                   )
                                                                   ||
                                                                   (
                                                                       (sd.tbl_Regions.RegCode) >= (loadedAvailable.tbl_Regions.RegCode) &&
                                                                       (sd.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_Regions.RegCode)
                                                                   )
                                                               )
                                                               && sd.IsFurnisher == loadedAvailable.IsFurnished
                                                               && sd.IsDivider == loadedAvailable.IsDivider
                                                               ).ToListAsync();
        }

        public async Task<List<tbl_ShopDemands>> DemnadsByPaymentUsage(tbl_ShopAvailable ShopAvailable)
        {
            var loadedAvailable = await _context.tbl_ShopAvailable
                .Where(a => a.PK_ShopAvailable_Id == ShopAvailable.PK_ShopAvailable_Id)
                .Include(a => a.tbl_Regions).Include(a => a.tbl_PaymentMethods)
                .FirstOrDefaultAsync();
            
            return await _context.tbl_ShopDemands
                                  .Include(d => d.tbl_Regions).Include(d => d.tbl_Regions1)
                                  .Include(d => d.tbl_PaymentMethods)
                                  .Where(sd => !sd.IsClosed && !sd.IsDeleted
                                                                      && sd.MaxPrice <= loadedAvailable.Price && sd.MinPrice >= loadedAvailable.Price
                                                                      && sd.MaxSpace <= loadedAvailable.Space && sd.MinSpace >= loadedAvailable.Space
                                                                      && sd.MaxBathRooms <= loadedAvailable.BathRooms && sd.MinBathRooms >= loadedAvailable.BathRooms
                                                                      && sd.FK_ShopDemands_Categories_Id == loadedAvailable.FK_ShopAvailable_Categories_Id
                                                                      && sd.FK_ShopDemands_Transactions_Id == loadedAvailable.FK_ShopAvailable_Transactions_Id &&
                                                                      (sd.FK_ShopDemands_Usage_Id == loadedAvailable.FK_ShopAvailable_Usage_Id || sd.FK_ShopDemands_Usage_Id == UnitUsages.Multiple)
                                                                      &&
                                                                      sd.FK_ShopDemands_PaymentMethod_Id == loadedAvailable.FK_ShopAvailable_PaymentMethod_Id &&
                                                                      (
                                                                          (
                                                                              (sd.tbl_Regions.RegCode) <= (loadedAvailable.tbl_Regions.RegCode) &&
                                                                              (sd.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_Regions.RegCode)
                                                                          )
                                                                          ||
                                                                          (
                                                                              (sd.tbl_Regions.RegCode) >= (loadedAvailable.tbl_Regions.RegCode) &&
                                                                              (sd.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_Regions.RegCode)
                                                                          )
                                                                      )
                                                                      && sd.IsFurnisher == loadedAvailable.IsFurnished
                                                                      && sd.IsDivider == loadedAvailable.IsDivider
                                                                      ).ToListAsync();
        }
        public async Task<List<tbl_ShopAvailable>> AvailablesForPayment(tbl_ShopDemands demand, int[] codes)
        {
            var lastCode = codes.LastOrDefault();
            List<tbl_ShopAvailable> availables;
            availables = await _context.tbl_ShopAvailable.Include(v => v.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                a.FK_ShopAvailable_Transactions_Id == demand.FK_ShopDemands_Transactions_Id &&
                                a.Price <= demand.MaxPrice &&
                                !a.IsDeleted &&
                                (a.BathRooms >= demand.MinBathRooms && a.BathRooms <= demand.MaxBathRooms) &&
                                a.FK_ShopAvailable_Categories_Id == demand.FK_ShopDemands_Categories_Id &&
                                (a.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_Regions.RegCode <= lastCode) &&
                                a.IsFurnished == demand.IsFurnisher &&
                                a.IsDivider == demand.IsDivider &&
                                a.Islicense == demand.Islicense &&
                                (a.Space >= demand.MinSpace && a.Space <= demand.MaxSpace) &&
                                 (a.FK_ShopAvailable_PaymentMethod_Id == demand.FK_ShopDemands_PaymentMethod_Id ||
                                a.tbl_PaymentMethods.IsMaster
                              )).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_ShopAvailable>> AvailablesForPaymentAndUsage(tbl_ShopDemands demand, int[] codes)
        {
            var lastCode = codes.LastOrDefault();
            List<tbl_ShopAvailable> availables;
            availables = await _context.tbl_ShopAvailable.Include(v => v.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                a.FK_ShopAvailable_Transactions_Id == demand.FK_ShopDemands_Transactions_Id &&
                                a.Price <= demand.MaxPrice &&
                                 (a.FK_ShopAvailable_Usage_Id == demand.FK_ShopDemands_Usage_Id || a.FK_ShopAvailable_Usage_Id == UnitUsages.Multiple) &&
                                !a.IsDeleted &&
                                (a.BathRooms >= demand.MinBathRooms && a.BathRooms <= demand.MaxBathRooms) &&
                                a.FK_ShopAvailable_Categories_Id == demand.FK_ShopDemands_Categories_Id &&
                                (a.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_Regions.RegCode <= lastCode) &&
                                a.IsFurnished == demand.IsFurnisher &&
                                a.IsDivider == demand.IsDivider &&
                                a.Islicense == demand.Islicense &&
                                (a.Space >= demand.MinSpace && a.Space <= demand.MaxSpace) &&
                                 (a.FK_ShopAvailable_PaymentMethod_Id == demand.FK_ShopDemands_PaymentMethod_Id ||
                                a.tbl_PaymentMethods.IsMaster
                              )).ToListAsync();
            return availables;

        }

        public async Task<List<tbl_ShopAvailable>> AvailablesForMasters(tbl_ShopDemands demand, int[] codes)
        {
            var lastCode = codes.LastOrDefault();
            List<tbl_ShopAvailable> availables;
            availables = await _context.tbl_ShopAvailable.Include(v => v.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                a.FK_ShopAvailable_Transactions_Id == demand.FK_ShopDemands_Transactions_Id &&
                                a.Price <= demand.MaxPrice &&
                                !a.IsDeleted &&
                                (a.BathRooms >= demand.MinBathRooms && a.BathRooms <= demand.MaxBathRooms) &&
                                a.FK_ShopAvailable_Categories_Id == demand.FK_ShopDemands_Categories_Id &&
                                (a.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_Regions.RegCode <= lastCode) &&
                                a.IsFurnished == demand.IsFurnisher &&
                                a.IsDivider == demand.IsDivider &&
                                a.Islicense == demand.Islicense &&
                                (a.Space >= demand.MinSpace && a.Space <= demand.MaxSpace)
                              ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_ShopAvailable>> AvailablesForUsage(tbl_ShopDemands demand, int[] codes)
        {
            var lastCode = codes.LastOrDefault();
            List<tbl_ShopAvailable> availables;
            availables = await _context.tbl_ShopAvailable.Include(v => v.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                a.FK_ShopAvailable_Transactions_Id == demand.FK_ShopDemands_Transactions_Id &&
                                a.Price <= demand.MaxPrice &&
                                 (a.FK_ShopAvailable_Usage_Id == demand.FK_ShopDemands_Usage_Id || a.FK_ShopAvailable_Usage_Id == UnitUsages.Multiple) &&
                                !a.IsDeleted &&
                                (a.BathRooms >= demand.MinBathRooms && a.BathRooms <= demand.MaxBathRooms) &&
                                a.FK_ShopAvailable_Categories_Id == demand.FK_ShopDemands_Categories_Id &&
                                (a.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_Regions.RegCode <= lastCode) &&
                                a.IsFurnished == demand.IsFurnisher &&
                                a.IsDivider == demand.IsDivider &&
                                a.Islicense == demand.Islicense &&
                                (a.Space >= demand.MinSpace && a.Space <= demand.MaxSpace)
                                ).ToListAsync();
            return availables;
        }

        public async Task<List<DemandsWithPreviews>> FilterDemandsHasPreviews(List<ShopDemandDto> empDemands, List<ShopDemandDto> colleguesDemands, ShopAvailableDto shopAvailableDto)
        {
            var headers= await (from header in _context.tbl_PreviewHeaders
             join detail in _context.tbl_PreviewDetails
             on header.PK_PreviewHeaders_Id equals detail.Fk_PreviewDetails_PreviewHeaders_Id
             where (detail.AvailableUnits_Id == shopAvailableDto.PK_ShopAvailable_Id && detail.Fk_PreviewDetails_Clients_SellerId == shopAvailableDto.FK_ShopAvailable_Clients_ClientId)
                   && (header.ReviewDate >= TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Today, "Egypt Standard Time"))
                   && !header.IsDeleted && !detail.IsDeleted && detail.IsNoDecision
                   //&& (empDemands.Exists(d => d.PK_ShopDemands_Id == header.DemandUnit_Id && d.FK_ShopDemands_Clients_ClientId == header.FK_PreviewHeaders_Clients_BuyerId)||
                   //colleguesDemands.Exists(d => d.PK_ShopDemands_Id == header.DemandUnit_Id && d.FK_ShopDemands_Clients_ClientId == header.FK_PreviewHeaders_Clients_BuyerId)
                   //)
             select new DemandsWithPreviews
             {
                 HeaderId = header.PK_PreviewHeaders_Id,
                 PreviewDate = header.ReviewDate,
                 DemandId = header.DemandUnit_Id,
                 BuyerId = header.FK_PreviewHeaders_Clients_BuyerId,
                 AvailableId = detail.AvailableUnits_Id,
                 Seller = detail.Fk_PreviewDetails_Clients_SellerId,
                 DetailId = detail.PK_PreviewDetails_Id
             }).ToListAsync();

            var filterdHeaders = headers.Where(h => empDemands.Any(d => d.PK_ShopDemands_Id == h.DemandId && d.FK_ShopDemands_Clients_ClientId == h.BuyerId) ||
                                                    colleguesDemands.Any(d => d.PK_ShopDemands_Id == h.DemandId && d.FK_ShopDemands_Clients_ClientId == h.BuyerId));
            return filterdHeaders.ToList();
        }
    }
}
