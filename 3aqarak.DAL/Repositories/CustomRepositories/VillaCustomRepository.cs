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
    public class VillaCustomRepository : IVillaCustomRepository
    {
        private readonly RealEstateDB _context;

        public VillaCustomRepository(RealEstateDB context)
        {
            _context = context;
        }
        public async Task<List<tbl_VillasDemands>> DemandsForpayment(tbl_VillasAvailables available)
        {
            var loadedAvailable = await _context.tbl_VillasAvailables
               .Where(a => a.PK_VillasAvailables_Id == available.PK_VillasAvailables_Id)
               .Include(a => a.tbl_Regions).Include(a => a.tbl_PaymentMethods)
               .FirstOrDefaultAsync();

            return await _context.tbl_VillasDemands
                .Include(v => v.tbl_Regions)
                .Include(v => v.tbl_Regions1).Include(v => v.tbl_PaymentMethods)
                .Where(d => d.IsClosed == false && !d.IsDeleted &&
                                                    d.MinNoOfElevators <= loadedAvailable.NoOfElevators && d.MaxNoOfElevators >= loadedAvailable.NoOfElevators &&
                                                    d.DateOfBuildFrom <= loadedAvailable.DateOfBuild && d.DateOfBuildTo >= loadedAvailable.DateOfBuild &&
                                                    d.FK_VillasDemands_Transactions_Id == loadedAvailable.FK_VillasAvailables_Transactions_Id &&
                                                    d.MaxPrice >= loadedAvailable.Price &&
                                                    (d.MinBathRooms <= loadedAvailable.BathRooms && d.MaxBathRooms >= loadedAvailable.BathRooms) &&
                                                    d.FK_VillasDemands_Categories_Id == loadedAvailable.FK_VillasAvailables_Categories_Id
                                                   &&
                                                    (
                                                    (
                                                    (d.tbl_Regions.RegCode) <= (loadedAvailable.tbl_Regions.RegCode) &&
                                                    (d.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_Regions.RegCode)
                                                    ) ||
                                                    (
                                                    (d.tbl_Regions.RegCode) >= (loadedAvailable.tbl_Regions.RegCode) &&
                                                    (d.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_Regions.RegCode)
                                                    )
                                                    )


                                                    &&
                                                    d.IsFurnished == loadedAvailable.IsFurnished &&
                                                    (d.MinRooms <= loadedAvailable.Rooms && d.MaxRooms >= loadedAvailable.Rooms) &&
                                                    (d.MinSpace <= loadedAvailable.Space && d.MaxSpace >= loadedAvailable.Space) &&
                                                    (
                                                     (
                                                     //if available is cash
                                                     ((loadedAvailable.FK_VillasAvailables_PaymentMethod_Id == 3 && d.FK_VillasDemands_PaymentMethod_Id == 3 || d.tbl_PaymentMethods.IsMaster) && d.MaxPrice >= loadedAvailable.Price)
                                                       ||
                                                     //if available is installable
                                                     ((loadedAvailable.FK_VillasAvailables_PaymentMethod_Id == 4 && d.FK_VillasDemands_PaymentMethod_Id == 4 || d.tbl_PaymentMethods.IsMaster) && d.MaxPrice >= (loadedAvailable.Price + loadedAvailable.Over))
                                                     )
                                                    &&

                                                    (d.tbl_VillademandFinishings
                                                    .Where(df => !df.IsDeleted && _context.tbl_Finishings
                                                    .Where(f => f.PK_Finishings_Id == df.FK_VillaDemandFinishing_Finish_Id && f.IsMaster).Any()).Any())
                                                    || d.tbl_VillademandFinishings.Where(df => !df.IsDeleted && df.FK_VillaDemandFinishing_Finish_Id == loadedAvailable.FK_VillasAvailables_Finishings_Id).Any()

                                                   )).ToListAsync();
        }

        public async Task<List<tbl_VillasDemands>> DemandsForpaymentAndUsage(tbl_VillasAvailables available)
        {
            var loadedAvailable = await _context.tbl_VillasAvailables
               .Where(a => a.PK_VillasAvailables_Id == available.PK_VillasAvailables_Id)
               .Include(a => a.tbl_Regions).Include(a => a.tbl_PaymentMethods)
               .FirstOrDefaultAsync();
            return await _context.tbl_VillasDemands
                            .Include(v => v.tbl_Regions)
                            .Include(v => v.tbl_Regions1).Include(v => v.tbl_PaymentMethods)
                            .Where(d => d.IsClosed == false && !d.IsDeleted &&
                                                    d.MinNoOfElevators <= loadedAvailable.NoOfElevators && d.MaxNoOfElevators >= loadedAvailable.NoOfElevators &&
                                                    d.DateOfBuildFrom <= loadedAvailable.DateOfBuild && d.DateOfBuildTo >= loadedAvailable.DateOfBuild &&
                                                    d.FK_VillasDemands_Transactions_Id == loadedAvailable.FK_VillasAvailables_Transactions_Id &&
                                                    (d.FK_VillasDemands_Usage_Id == loadedAvailable.FK_VillasAvailables_Usage_Id || d.FK_VillasDemands_Usage_Id == UnitUsages.Multiple) &&
                                                    
                                                    (d.MinBathRooms <= loadedAvailable.BathRooms && d.MaxBathRooms >= loadedAvailable.BathRooms) &&
                                                    d.FK_VillasDemands_Categories_Id == loadedAvailable.FK_VillasAvailables_Categories_Id
                                                   &&
                                                    (
                                                    (
                                                    (d.tbl_Regions.RegCode) <= (loadedAvailable.tbl_Regions.RegCode) &&
                                                    (d.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_Regions.RegCode)
                                                    ) ||
                                                    (
                                                    (d.tbl_Regions.RegCode) >= (loadedAvailable.tbl_Regions.RegCode) &&
                                                    (d.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_Regions.RegCode)
                                                    )
                                                    )

                                                    
                                                    &&
                                                    d.IsFurnished == loadedAvailable.IsFurnished &&
                                                    (d.MinRooms <= loadedAvailable.Rooms && d.MaxRooms >= loadedAvailable.Rooms) &&
                                                    (d.MinSpace <= loadedAvailable.Space && d.MaxSpace >= loadedAvailable.Space) &&
                                                    ((
                                     //if available is cash
                                     ((loadedAvailable.FK_VillasAvailables_PaymentMethod_Id == 3 && d.FK_VillasDemands_PaymentMethod_Id == 3 || d.tbl_PaymentMethods.IsMaster) && d.MaxPrice >= loadedAvailable.Price)
                                     ||
                                     //if available is installable
                                     ((loadedAvailable.FK_VillasAvailables_PaymentMethod_Id == 4 && d.FK_VillasDemands_PaymentMethod_Id == 4 || d.tbl_PaymentMethods.IsMaster) && d.MaxPrice >= (loadedAvailable.Price + loadedAvailable.Over))
                                    )
                                                    &&
                                                    (d.tbl_VillademandFinishings
                                                    .Where(df => !df.IsDeleted && _context.tbl_Finishings
                                                    .Where(f => f.PK_Finishings_Id == df.FK_VillaDemandFinishing_Finish_Id && f.IsMaster).Any()).Any())
                                                    || d.tbl_VillademandFinishings.Where(df => !df.IsDeleted && df.FK_VillaDemandFinishing_Finish_Id == loadedAvailable.FK_VillasAvailables_Finishings_Id).Any()

                                                   )).ToListAsync();
        }

        //public async Task<List<tbl_VillasDemands>> DemandsForAnypaymentAnyUsage(tbl_VillasAvailables available)
        //{
        //    var loadedAvailable = await _context.tbl_VillasAvailables
        //        .Where(a => a.PK_VillasAvailables_Id == available.PK_VillasAvailables_Id)
        //        .Include(a => a.tbl_Regions).Include(a => a.tbl_PaymentMethods)
        //        .FirstOrDefaultAsync();
        //    return await _context.tbl_VillasDemands
        //                    .Include(v => v.tbl_Regions)
        //                    .Include(v => v.tbl_Regions1).Include(v => v.tbl_PaymentMethods)
        //                    .Where(d => d.IsClosed == false && !d.IsDeleted &&
        //                                            d.MinNoOfElevators <= loadedAvailable.NoOfElevators && d.MaxNoOfElevators >= loadedAvailable.NoOfElevators &&
        //                                            d.DateOfBuildFrom <= loadedAvailable.DateOfBuild && d.DateOfBuildTo >= loadedAvailable.DateOfBuild &&
        //                                            d.FK_VillasDemands_Transactions_Id == loadedAvailable.FK_VillasAvailables_Transactions_Id  &&
        //                                            d.MaxPrice >= loadedAvailable.Price &&
        //                                            (d.MinBathRooms <= loadedAvailable.BathRooms && d.MaxBathRooms >= loadedAvailable.BathRooms) &&
        //                                            d.FK_VillasDemands_Categories_Id == loadedAvailable.FK_VillasAvailables_Categories_Id
        //                                           &&
        //                                            (
        //                                            (
        //                                            (d.tbl_Regions.RegCode) <= (loadedAvailable.tbl_Regions.RegCode) &&
        //                                            (d.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_Regions.RegCode)
        //                                            ) ||
        //                                            (
        //                                            (d.tbl_Regions.RegCode) >= (loadedAvailable.tbl_Regions.RegCode) &&
        //                                            (d.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_Regions.RegCode)
        //                                            )
        //                                            )

                                                   
        //                                            &&
        //                                            d.IsFurnished == loadedAvailable.IsFurnished &&
        //                                            (d.MinRooms <= loadedAvailable.Rooms && d.MaxRooms >= loadedAvailable.Rooms) &&
        //                                            (d.MinSpace <= loadedAvailable.Space && d.MaxSpace >= loadedAvailable.Space) &&
        //                                            ( 
        //                                            (d.tbl_VillademandFinishings
        //                                            .Where(df => !df.IsDeleted && _context.tbl_Finishings
        //                                            .Where(f => f.PK_Finishings_Id == df.FK_VillaDemandFinishing_Finish_Id && f.IsMaster).Any()).Any())
        //                                            || d.tbl_VillademandFinishings.Where(df => !df.IsDeleted && df.FK_VillaDemandFinishing_Finish_Id == loadedAvailable.FK_VillasAvailables_Finishings_Id).Any()

        //                                           )).ToListAsync();
        //}

        //public async Task<List<tbl_VillasDemands>> DemandsForAnypaymentAndUsage(tbl_VillasAvailables available)
        //{
        //    var loadedAvailable = await _context.tbl_VillasAvailables
        //         .Where(a => a.PK_VillasAvailables_Id == available.PK_VillasAvailables_Id)
        //         .Include(a => a.tbl_Regions).Include(a => a.tbl_PaymentMethods)
        //         .FirstOrDefaultAsync();
        //    return await _context.tbl_VillasDemands
        //                    .Include(v => v.tbl_Regions)
        //                    .Include(v => v.tbl_Regions1).Include(v => v.tbl_PaymentMethods)
        //                    .Where(d => d.IsClosed == false && !d.IsDeleted &&
        //                                            d.MinNoOfElevators <= loadedAvailable.NoOfElevators && d.MaxNoOfElevators >= loadedAvailable.NoOfElevators &&
        //                                            d.DateOfBuildFrom <= loadedAvailable.DateOfBuild && d.DateOfBuildTo >= loadedAvailable.DateOfBuild &&
        //                                            d.FK_VillasDemands_Transactions_Id == loadedAvailable.FK_VillasAvailables_Transactions_Id &&
        //                                            d.MaxPrice >= loadedAvailable.Price &&
        //                                            (d.FK_VillasDemands_Usage_Id == loadedAvailable.FK_VillasAvailables_Usage_Id || d.FK_VillasDemands_Usage_Id == UnitUsages.Multiple) &&
        //                                            (d.MinBathRooms <= loadedAvailable.BathRooms && d.MaxBathRooms >= loadedAvailable.BathRooms) &&
        //                                            d.FK_VillasDemands_Categories_Id == loadedAvailable.FK_VillasAvailables_Categories_Id
        //                                           &&
        //                                            (
        //                                            (
        //                                            (d.tbl_Regions.RegCode) <= (loadedAvailable.tbl_Regions.RegCode) &&
        //                                            (d.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_Regions.RegCode)
        //                                            ) ||
        //                                            (
        //                                            (d.tbl_Regions.RegCode) >= (loadedAvailable.tbl_Regions.RegCode) &&
        //                                            (d.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_Regions.RegCode)
        //                                            )
        //                                            )

                                                    
        //                                            &&
        //                                            d.IsFurnished == loadedAvailable.IsFurnished &&
        //                                            (d.MinRooms <= loadedAvailable.Rooms && d.MaxRooms >= loadedAvailable.Rooms) &&
        //                                            (d.MinSpace <= loadedAvailable.Space && d.MaxSpace >= loadedAvailable.Space) &&
        //                                            (
        //                                            (d.tbl_VillademandFinishings
        //                                            .Where(df => !df.IsDeleted && _context.tbl_Finishings
        //                                            .Where(f => f.PK_Finishings_Id == df.FK_VillaDemandFinishing_Finish_Id && f.IsMaster).Any()).Any())
        //                                            || d.tbl_VillademandFinishings.Where(df => !df.IsDeleted && df.FK_VillaDemandFinishing_Finish_Id == loadedAvailable.FK_VillasAvailables_Finishings_Id).Any()

        //                                           )).ToListAsync();
        //}

        public async Task<List<tbl_VillasAvailables>> AvailableForPaymentAndFinish(tbl_VillasDemands demand, int[] codes, int[] finishIds)
        {
            var lastCode = codes.LastOrDefault();
            List<tbl_VillasAvailables> availables;
            availables = await _context.tbl_VillasAvailables.Include(v => v.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                (a.NoOfElevators >= demand.MinNoOfElevators && a.NoOfElevators <= demand.MaxNoOfElevators) &&
                                (a.AreaSpace >= demand.MinAreaSpace && a.AreaSpace <= demand.MaxAreaSpace) &&
                                a.FK_VillasAvailables_Transactions_Id == demand.FK_VillasDemands_Transactions_Id &&
                                //a.Price <= demand.MaxPrice &&
                                !a.IsDeleted &&
                                (a.BathRooms >= demand.MinBathRooms && a.BathRooms <= demand.MaxBathRooms) &&
                                a.FK_VillasAvailables_Categories_Id == demand.FK_VillasDemands_Categories_Id &&                              
                                (a.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_Regions.RegCode <= lastCode) &&
                                
                                a.IsFurnished == demand.IsFurnished &&
                                (a.Rooms >= demand.MinRooms && a.Rooms <= demand.MaxRooms) &&
                                (a.Space >= demand.MinSpace && a.Space <= demand.MaxSpace) &&
                                 (
                                 ((a.FK_VillasAvailables_PaymentMethod_Id ==3&& demand.FK_VillasDemands_PaymentMethod_Id==3) || a.Price <= demand.MaxPrice)
                                 ||
                                 ((a.FK_VillasAvailables_PaymentMethod_Id == 4 && demand.FK_VillasDemands_PaymentMethod_Id == 4) || (a.Price + a.Over) <= demand.MaxPrice)
                                 ||
                                 ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                 ) 
                                 &&
                                  finishIds.Contains(a.FK_VillasAvailables_Finishings_Id)                               
                                ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_VillasAvailables>> AvailableForPaymentFinishUsage(tbl_VillasDemands demand, int[] codes, int[] finishIds)
        {
            var lastCode = codes.LastOrDefault();
            List<tbl_VillasAvailables> availables;
            availables = await _context.tbl_VillasAvailables.Include(v => v.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                   (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                   (a.NoOfElevators >= demand.MinNoOfElevators && a.NoOfElevators <= demand.MaxNoOfElevators) &&
                                   (a.AreaSpace >= demand.MinAreaSpace && a.AreaSpace <= demand.MaxAreaSpace) &&
                                   (a.FK_VillasAvailables_Usage_Id == demand.FK_VillasDemands_Usage_Id || a.FK_VillasAvailables_Usage_Id == UnitUsages.Multiple) &&
                                   a.FK_VillasAvailables_Transactions_Id == demand.FK_VillasDemands_Transactions_Id &&
                                   //a.Price <= demand.MaxPrice &&
                                   !a.IsDeleted &&
                                   (a.BathRooms >= demand.MinBathRooms && a.BathRooms <= demand.MaxBathRooms) &&
                                   a.FK_VillasAvailables_Categories_Id == demand.FK_VillasDemands_Categories_Id &&
                                   (a.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_Regions.RegCode <= lastCode) &&
                                   
                                   a.IsFurnished == demand.IsFurnished &&
                                   (a.Rooms >= demand.MinRooms && a.Rooms <= demand.MaxRooms) &&
                                   (a.Space >= demand.MinSpace && a.Space <= demand.MaxSpace) &&
                                    (
                                 //if demand is cash
                                 ((a.FK_VillasAvailables_PaymentMethod_Id == 3 && demand.FK_VillasDemands_PaymentMethod_Id == 3) || a.Price <= demand.MaxPrice)
                                 ||
                                 //if demand is installable
                                 ((a.FK_VillasAvailables_PaymentMethod_Id == 4 && demand.FK_VillasDemands_PaymentMethod_Id == 4) || (a.Price + a.Over) <= demand.MaxPrice)
                                 ||
                                 //if demand is ALL(installable AND cash)
                                 ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                 )
                                 &&
                                     finishIds.Contains(a.FK_VillasAvailables_Finishings_Id)
                                   ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_VillasAvailables>> AvailableMatches(tbl_VillasDemands demand, int[] codes)
        {
            var lastCode = codes.LastOrDefault();
            List<tbl_VillasAvailables> availables;
            availables = await _context.tbl_VillasAvailables.Include(v => v.tbl_Regions).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                   (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                   (a.NoOfElevators >= demand.MinNoOfElevators && a.NoOfElevators <= demand.MaxNoOfElevators) &&
                                   (a.AreaSpace >= demand.MinAreaSpace && a.AreaSpace <= demand.MaxAreaSpace) &&
                                   a.FK_VillasAvailables_Transactions_Id == demand.FK_VillasDemands_Transactions_Id &&
                                   //a.Price <= demand.MaxPrice &&
                                   ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                   &&
                                   !a.IsDeleted &&
                                   (a.BathRooms >= demand.MinBathRooms && a.BathRooms <= demand.MaxBathRooms) &&
                                   a.FK_VillasAvailables_Categories_Id == demand.FK_VillasDemands_Categories_Id &&
                                   (a.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_Regions.RegCode <= lastCode) &&
                                   
                                   a.IsFurnished == demand.IsFurnished &&
                                   (a.Rooms >= demand.MinRooms && a.Rooms <= demand.MaxRooms) &&
                                   (a.Space >= demand.MinSpace && a.Space <= demand.MaxSpace)                                    
                                   ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_VillasAvailables>> AvailableMatchesForUsage(tbl_VillasDemands demand, int[] codes)
        {
            var lastCode = codes.LastOrDefault();
            List<tbl_VillasAvailables> availables;
            availables = await _context.tbl_VillasAvailables.Include(v => v.tbl_Regions).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                   (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                   (a.NoOfElevators >= demand.MinNoOfElevators && a.NoOfElevators <= demand.MaxNoOfElevators) &&
                                   (a.AreaSpace >= demand.MinAreaSpace && a.AreaSpace <= demand.MaxAreaSpace) &&
                                   (a.FK_VillasAvailables_Usage_Id == demand.FK_VillasDemands_Usage_Id || a.FK_VillasAvailables_Usage_Id == UnitUsages.Multiple) &&
                                   a.FK_VillasAvailables_Transactions_Id == demand.FK_VillasDemands_Transactions_Id &&
                                   //a.Price <= demand.MaxPrice &&
                                   ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                   &&
                                   !a.IsDeleted &&
                                   (a.BathRooms >= demand.MinBathRooms && a.BathRooms <= demand.MaxBathRooms) &&
                                   a.FK_VillasAvailables_Categories_Id == demand.FK_VillasDemands_Categories_Id &&
                                   (a.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_Regions.RegCode <= lastCode) &&
                                  
                                   a.IsFurnished == demand.IsFurnished &&
                                   (a.Rooms >= demand.MinRooms && a.Rooms <= demand.MaxRooms) &&
                                   (a.Space >= demand.MinSpace && a.Space <= demand.MaxSpace)
                                   ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_VillasAvailables>> AvailableMatchesForFinish(tbl_VillasDemands demand, int[] codes, int[] finishIds)
        {
            var lastCode = codes.LastOrDefault();
            List<tbl_VillasAvailables> availables;
            availables = await _context.tbl_VillasAvailables.Include(v => v.tbl_Regions).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                   (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                   (a.NoOfElevators >= demand.MinNoOfElevators && a.NoOfElevators <= demand.MaxNoOfElevators) &&
                                   (a.AreaSpace >= demand.MinAreaSpace && a.AreaSpace <= demand.MaxAreaSpace) &&
                                   a.FK_VillasAvailables_Transactions_Id == demand.FK_VillasDemands_Transactions_Id &&
                                   //a.Price <= demand.MaxPrice &&
                                   ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                   &&
                                   !a.IsDeleted &&
                                   (a.BathRooms >= demand.MinBathRooms && a.BathRooms <= demand.MaxBathRooms) &&
                                   a.FK_VillasAvailables_Categories_Id == demand.FK_VillasDemands_Categories_Id &&
                                   (a.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_Regions.RegCode <= lastCode) &&
                                   
                                   a.IsFurnished == demand.IsFurnished &&
                                   (a.Rooms >= demand.MinRooms && a.Rooms <= demand.MaxRooms) &&
                                   (a.Space >= demand.MinSpace && a.Space <= demand.MaxSpace) &&
                                    finishIds.Contains(a.FK_VillasAvailables_Finishings_Id)
                                   ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_VillasAvailables>> AvailableMatchesForFinishAndUsage(tbl_VillasDemands demand, int[] codes, int[] finishIds)
        {
            var lastCode = codes.LastOrDefault();
            List<tbl_VillasAvailables> availables;
            availables = await _context.tbl_VillasAvailables.Include(v => v.tbl_Regions).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                   (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                   (a.NoOfElevators >= demand.MinNoOfElevators && a.NoOfElevators <= demand.MaxNoOfElevators) &&
                                   (a.AreaSpace >= demand.MinAreaSpace && a.AreaSpace <= demand.MaxAreaSpace) &&
                                   (a.FK_VillasAvailables_Usage_Id == demand.FK_VillasDemands_Usage_Id || a.FK_VillasAvailables_Usage_Id == UnitUsages.Multiple) &&
                                   a.FK_VillasAvailables_Transactions_Id == demand.FK_VillasDemands_Transactions_Id &&
                                   //a.Price <= demand.MaxPrice &&
                                   ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                   &&
                                   !a.IsDeleted &&
                                   (a.BathRooms >= demand.MinBathRooms && a.BathRooms <= demand.MaxBathRooms) &&
                                   a.FK_VillasAvailables_Categories_Id == demand.FK_VillasDemands_Categories_Id &&
                                   (a.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_Regions.RegCode <= lastCode) &&
                                   
                                   a.IsFurnished == demand.IsFurnished &&
                                   (a.Rooms >= demand.MinRooms && a.Rooms <= demand.MaxRooms) &&
                                   (a.Space >= demand.MinSpace && a.Space <= demand.MaxSpace) &&
                                    finishIds.Contains(a.FK_VillasAvailables_Finishings_Id)
                                   ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_VillasAvailables>> AvailableMatchesForPayment(tbl_VillasDemands demand, int[] codes)
        {
            var lastCode = codes.LastOrDefault();
            List<tbl_VillasAvailables> availables;
            availables = await _context.tbl_VillasAvailables.Include(v => v.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                   (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                   (a.NoOfElevators >= demand.MinNoOfElevators && a.NoOfElevators <= demand.MaxNoOfElevators) &&
                                   (a.AreaSpace >= demand.MinAreaSpace && a.AreaSpace <= demand.MaxAreaSpace) &&
                                   a.FK_VillasAvailables_Transactions_Id == demand.FK_VillasDemands_Transactions_Id &&
                                   //a.Price <= demand.MaxPrice &&
                                   !a.IsDeleted &&
                                   (a.BathRooms >= demand.MinBathRooms && a.BathRooms <= demand.MaxBathRooms) &&
                                   a.FK_VillasAvailables_Categories_Id == demand.FK_VillasDemands_Categories_Id &&
                                   (a.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_Regions.RegCode <= lastCode) &&
                                   a.IsFurnished == demand.IsFurnished &&
                                   (a.Rooms >= demand.MinRooms && a.Rooms <= demand.MaxRooms) &&
                                   (a.Space >= demand.MinSpace && a.Space <= demand.MaxSpace) &&
                                     (
                                 //if demand is cash
                                 ((a.FK_VillasAvailables_PaymentMethod_Id == 3 && demand.FK_VillasDemands_PaymentMethod_Id == 3) || a.Price <= demand.MaxPrice)
                                 ||
                                 //if demand is installable
                                 ((a.FK_VillasAvailables_PaymentMethod_Id == 4 && demand.FK_VillasDemands_PaymentMethod_Id == 4) || (a.Price + a.Over) <= demand.MaxPrice)
                                 ||
                                 //if demand is ALL(installable AND cash)
                                 ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                 )

                                   ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_VillasAvailables>> AvailableMatchesForPaymentAndUsage(tbl_VillasDemands demand, int[] codes)
        {
            var lastCode = codes.LastOrDefault();
            List<tbl_VillasAvailables> availables;
            availables = await _context.tbl_VillasAvailables.Include(v => v.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                   (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                   (a.NoOfElevators >= demand.MinNoOfElevators && a.NoOfElevators <= demand.MaxNoOfElevators) &&
                                   (a.AreaSpace >= demand.MinAreaSpace && a.AreaSpace <= demand.MaxAreaSpace) &&
                                   a.FK_VillasAvailables_Transactions_Id == demand.FK_VillasDemands_Transactions_Id &&
                                   (a.FK_VillasAvailables_Usage_Id == demand.FK_VillasDemands_Usage_Id || a.FK_VillasAvailables_Usage_Id == UnitUsages.Multiple) &&
                                   //a.Price <= demand.MaxPrice &&
                                   !a.IsDeleted &&
                                   (a.BathRooms >= demand.MinBathRooms && a.BathRooms <= demand.MaxBathRooms) &&
                                   a.FK_VillasAvailables_Categories_Id == demand.FK_VillasDemands_Categories_Id &&
                                   (a.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_Regions.RegCode <= lastCode) &&
                                   
                                   a.IsFurnished == demand.IsFurnished &&
                                   (a.Rooms >= demand.MinRooms && a.Rooms <= demand.MaxRooms) &&
                                   (a.Space >= demand.MinSpace && a.Space <= demand.MaxSpace) &&
                                     (
                                 //if demand is cash
                                 ((a.FK_VillasAvailables_PaymentMethod_Id == 3 && demand.FK_VillasDemands_PaymentMethod_Id == 3) || a.Price <= demand.MaxPrice)
                                 ||
                                 //if demand is installable
                                 ((a.FK_VillasAvailables_PaymentMethod_Id == 4 && demand.FK_VillasDemands_PaymentMethod_Id == 4) || (a.Price + a.Over) <= demand.MaxPrice)
                                 ||
                                 //if demand is ALL(installable AND cash)
                                 ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                 )
                                 
                                   ).ToListAsync();
            return availables;
        }



         public async Task<List<DemandsWithPreviews>> FilterDemandsHasPreviews(List<VillasDemandDto> empDemands, List<VillasDemandDto> colleguesDemands, VillasAvailableDto villasAvailableDto)
        {
            var headers= await (from header in _context.tbl_PreviewHeaders
             join detail in _context.tbl_PreviewDetails
             on header.PK_PreviewHeaders_Id equals detail.Fk_PreviewDetails_PreviewHeaders_Id
             where (detail.AvailableUnits_Id == villasAvailableDto.PK_VillasAvailables_Id && detail.Fk_PreviewDetails_Clients_SellerId == villasAvailableDto.FK_VillasAvailables_Clients_ClientId)
                                              && (header.ReviewDate >= DateTime.Today)
                                              && !header.IsDeleted && !detail.IsDeleted && detail.IsNoDecision
                                //&& (empDemands.Exists(d => d.PK_VillasAvailables_Id == header.DemandUnit_Id && d.FK_ShopDemands_Clients_ClientId == header.FK_VillasAvailables_Clients_ClientId)||
                                //colleguesDemands.Exists(d => d.PK_VillasAvailables_Id == header.DemandUnit_Id && d.FK_ShopDemands_Clients_ClientId == header.FK_VillasAvailables_Clients_ClientId)
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

            var filterdHeaders = headers.Where(h => empDemands.Any(d => d.PK_VillasDemands_Id == h.DemandId && d.FK_VillasDemands_Clients_ClientId == h.BuyerId) ||
            colleguesDemands.Any(d => d.PK_VillasDemands_Id == h.DemandId && d.FK_VillasDemands_Clients_ClientId == h.BuyerId));
            return filterdHeaders.ToList();
        }


    }
}

