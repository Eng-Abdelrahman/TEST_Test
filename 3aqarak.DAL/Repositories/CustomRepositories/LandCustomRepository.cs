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
    public class LandCustomRepository: ILandCustomRepository
    {
        private readonly RealEstateDB _context;

        public LandCustomRepository(RealEstateDB context)
        {
            _context = context;
        }

        public async Task<List<tbl_LandsDemands>> AvailablesForMasters(tbl_AvailableLands available)
        {
            var loadedAvailable = await _context.tbl_AvailableLands
                .Where(a => a.PK_AvailableLands_Id == available.PK_AvailableLands_Id)
                .Include(a => a.tbl_Regions).Include(a => a.tbl_PaymentMethods)
                .FirstOrDefaultAsync();

            return await _context.tbl_LandsDemands
                            .Include(v => v.tbl_Regions)
                            .Include(v => v.tbl_Regions1).Include(v => v.tbl_PaymentMethods)
                            .Where(
                            ld => ld.IsClosed == false && !ld.IsDeleted
                            && ld.MaxPrice <= loadedAvailable.Price && ld.MinPrice >= loadedAvailable.Price
                            && ld.MaxSpace <= loadedAvailable.Space && ld.MinSpace >= loadedAvailable.Space
                            && ld.FK_LandsDemands_Transactions_Id == loadedAvailable.FK_AvaliableLands_Transactions_TransactionId
                            && ld.Type == loadedAvailable.Type
                            &&
                            (
                               (ld.tbl_Regions.RegCode) <= (loadedAvailable.tbl_Regions.RegCode) &&
                               (ld.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_Regions.RegCode)
                               ||
                               (ld.tbl_Regions.RegCode) >= (loadedAvailable.tbl_Regions.RegCode) &&
                               (ld.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_Regions.RegCode)
                            )
                           ).ToListAsync();
        }

        public async Task<List<tbl_LandsDemands>> AvailablesForPayment(tbl_AvailableLands available)
        {
            var loadedAvailable = await _context.tbl_AvailableLands
               .Where(a => a.PK_AvailableLands_Id == available.PK_AvailableLands_Id)
               .Include(a => a.tbl_Regions).Include(a => a.tbl_PaymentMethods)
               .FirstOrDefaultAsync();

            return await _context.tbl_LandsDemands
                            .Include(v => v.tbl_Regions)
                            .Include(v => v.tbl_Regions1).Include(v => v.tbl_PaymentMethods)
                            .Where(
                            ld => ld.IsClosed == false && !ld.IsDeleted
                            && ld.MaxPrice <= loadedAvailable.Price && ld.MinPrice >= loadedAvailable.Price
                            && ld.MaxSpace <= loadedAvailable.Space && ld.MinSpace >= loadedAvailable.Space
                            && ld.FK_LandsDemands_Transactions_Id == loadedAvailable.FK_AvaliableLands_Transactions_TransactionId
                            && ld.Type == loadedAvailable.Type &&
                            ld.FK_LandsDemands_PaymentMethod_Id == loadedAvailable.FK_AvailableLands_PaymentMethod_Id
                            &&
                            (
                               (ld.tbl_Regions.RegCode) <= (loadedAvailable.tbl_Regions.RegCode) &&
                               (ld.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_Regions.RegCode)
                               ||
                               (ld.tbl_Regions.RegCode) >= (loadedAvailable.tbl_Regions.RegCode) &&
                               (ld.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_Regions.RegCode)
                            )
                           ).ToListAsync();
        }

        public async Task<List<DemandsWithPreviews>> FilterDemandsHasPreviews(List<LandsDemandsDto> empDemands, List<LandsDemandsDto> colleguesDemands, AvailableLandsDto available)
        {
            var headers= await (from header in _context.tbl_PreviewHeaders
                          join detail
                          in _context.tbl_PreviewDetails
                          on header.PK_PreviewHeaders_Id equals detail.Fk_PreviewDetails_PreviewHeaders_Id
                          where (detail.AvailableUnits_Id == available.PK_AvailableLands_Id && detail.Fk_PreviewDetails_Clients_SellerId == available.FK_AvaliableLands_Clients_ClientId)
                               && (header.ReviewDate >= DateTime.Today)
                               && !header.IsDeleted && !detail.IsDeleted && detail.IsNoDecision //remove the rejected 
                               //&& (empDemands.Exists(d => d.PK_LandsDemands_Id == header.DemandUnit_Id && d.FK_LandsDemands_Clients_ClientId== header.FK_PreviewHeaders_Clients_BuyerId) ||
                               //colleguesDemands.Exists(d => d.PK_LandsDemands_Id == header.DemandUnit_Id && d.FK_LandsDemands_Clients_ClientId== header.FK_PreviewHeaders_Clients_BuyerId))
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
            var filterdHeaders = headers.Where(h => empDemands.Any(d => d.PK_LandsDemands_Id == h.DemandId && d.FK_LandsDemands_Clients_ClientId == h.BuyerId) ||
                                                        colleguesDemands.Any(d => d.PK_LandsDemands_Id == h.DemandId && d.FK_LandsDemands_Clients_ClientId == h.BuyerId));

            return filterdHeaders.ToList();
        }


    }
}
 
