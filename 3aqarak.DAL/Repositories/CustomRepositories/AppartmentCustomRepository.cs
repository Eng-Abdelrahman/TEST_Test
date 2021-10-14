using _3aqarak.BLL.Interfaces;
using _3aqarak.DAL.Models;
using _3aqarak.BLL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;

namespace _3aqarak.DAL.Repositories.CustomRepositories
{
    public class AppartmentCustomRepository : IAppartmentCustomRepository
    {

        private readonly RealEstateDB _dbContext;

        public AppartmentCustomRepository(RealEstateDB context)
        {
            this._dbContext = context;
        }
        public async Task<tbl_AvailableUnits> LoadRelatedUnitAsync(int id)
        {
            return await _dbContext.tbl_AvailableUnits.Where(a => a.PK_AvailableUnits_Id == id).Include(a => a.tbl_units).FirstOrDefaultAsync();
        }
            public async Task<List<tbl_DemandUnits>> DemandsForMultiUsageAndPayment(tbl_AvailableUnits available)
        {
            List<tbl_DemandUnits> demands;

            var loadedAvailable = await _dbContext.tbl_AvailableUnits
                .Where(a => a.PK_AvailableUnits_Id == available.PK_AvailableUnits_Id)
                .Include(a => a.tbl_units.tbl_Regions).Include(a => a.tbl_PaymentMethods)
                .FirstOrDefaultAsync();

            demands = await _dbContext.tbl_DemandUnits
                .Include(d => d.tbl_Regions).Include(d => d.tbl_Regions1)
                .Include(d => d.tbl_PaymentMethods)
                .Include(d => d.tbl_Demand_Finishings)
                .Where(d => d.IsClosed == false && !d.IsDeleted &&
                                  (d.NoOfElevatorsFrom <= loadedAvailable.NoOfElevators && d.NoOfElevatorsTo >= loadedAvailable.NoOfElevators)
                                  &&
                                  (d.DateOfBuildFrom <= loadedAvailable.DateOfBuild && d.DateOfBuildTo >= loadedAvailable.DateOfBuild)
                                  &&
                                  d.FK_DemandUnits_Transactions_Id == loadedAvailable.FK_AvaliableUnits_Transaction_TransactionId                                
                                  &&
                                  (d.MinBathRooms <= loadedAvailable.tbl_units.Bathrooms && d.MaxBathRooms >= loadedAvailable.tbl_units.Bathrooms)
                                  &&
                                  d.FK_DemandUnits_Categories_Id == loadedAvailable.tbl_units.FK_Units_Categories_Id
                                  &&
                                  (d.MinFloor <= loadedAvailable.tbl_units.Floor && d.MaxFloor >= loadedAvailable.tbl_units.Floor)
                                  &&
                                  d.IsFurnished == loadedAvailable.tbl_units.IsFurniture
                                  &&
                                  (d.MinRooms <= loadedAvailable.tbl_units.Rooms && d.MaxRooms >= loadedAvailable.tbl_units.Rooms)
                                  &&
                                  (d.MinSpace <= loadedAvailable.tbl_units.Space && d.MaxSpace >= loadedAvailable.tbl_units.Space)
                                  &&
                                  (
                                  (
                                  (d.tbl_Regions.RegCode <= loadedAvailable.tbl_units.tbl_Regions.RegCode)
                                  &&
                                  (d.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_units.tbl_Regions.RegCode)
                                  ) ||
                                  (
                                  (d.tbl_Regions.RegCode)) >= (loadedAvailable.tbl_units.tbl_Regions.RegCode)
                                  &&
                                  (d.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_units.tbl_Regions.RegCode)
                                  )
                                  &&
                                  (
                                  //if available is cash
                                  ((loadedAvailable.FK_AvailableUnits_PaymentMethod_Id ==3 && d.FK_DemandUnits_PaymentMethod_Id == 3 || d.tbl_PaymentMethods.IsMaster) && d.MaxPrice >= loadedAvailable.Price)
                                  ||
                                  //if available is installable
                                  ((loadedAvailable.FK_AvailableUnits_PaymentMethod_Id ==4  && d.FK_DemandUnits_PaymentMethod_Id == 4|| d.tbl_PaymentMethods.IsMaster) && d.MaxPrice >= (loadedAvailable.Price + loadedAvailable.Over))
                                  )
                                  &&
                                   (d.tbl_Demand_Finishings
                                   .Where(df => !df.IsDeleted && _dbContext.tbl_Finishings
                                   .Where(f => f.PK_Finishings_Id == df.FK_DemandFinishing_Finish_Id && f.IsMaster).Any()).Any()
                                   )
                                  || d.tbl_Demand_Finishings.Where(df => !df.IsDeleted && df.FK_DemandFinishing_Finish_Id == loadedAvailable.tbl_units.FK_Units_Finishing_Id).Any()
                   ).ToListAsync();

            return demands;
        }

        public async Task<List<tbl_DemandUnits>> DemandsForUsageAndPayment(tbl_AvailableUnits available)
        {
            List<tbl_DemandUnits> demands;
            var loadedAvailable = await _dbContext.tbl_AvailableUnits
                .Where(a => a.PK_AvailableUnits_Id == available.PK_AvailableUnits_Id)
                .Include(a => a.tbl_units.tbl_Regions).Include(a => a.tbl_PaymentMethods)
                .FirstOrDefaultAsync();
            demands = await _dbContext.tbl_DemandUnits
                  .Include(d => d.tbl_Regions).Include(d => d.tbl_Regions1)
                  .Include(d => d.tbl_PaymentMethods)
                  .Include(d => d.tbl_Demand_Finishings)
                  .Where(d => d.IsClosed == false && !d.IsDeleted &&
                                    (d.NoOfElevatorsFrom <= loadedAvailable.NoOfElevators && d.NoOfElevatorsTo >= loadedAvailable.NoOfElevators)
                                    &&
                                    (d.DateOfBuildFrom <= loadedAvailable.DateOfBuild && d.DateOfBuildTo >= loadedAvailable.DateOfBuild)
                                    &&
                                    d.FK_DemandUnits_Transactions_Id == loadedAvailable.FK_AvaliableUnits_Transaction_TransactionId
                                    &&
                                    (d.FK_DemandUnits_Usage_Id == loadedAvailable.FK_AvailableUnits_Usage_Id || d.FK_DemandUnits_Usage_Id == UnitUsages.Multiple)
                                   
                                    &&
                                    (d.MinBathRooms <= loadedAvailable.tbl_units.Bathrooms && d.MaxBathRooms >= loadedAvailable.tbl_units.Bathrooms)
                                    &&
                                    d.FK_DemandUnits_Categories_Id == loadedAvailable.tbl_units.FK_Units_Categories_Id
                                    &&
                                    (d.MinFloor <= loadedAvailable.tbl_units.Floor && d.MaxFloor >= loadedAvailable.tbl_units.Floor)
                                    &&
                                    d.IsFurnished == loadedAvailable.tbl_units.IsFurniture
                                    &&
                                    (d.MinRooms <= loadedAvailable.tbl_units.Rooms && d.MaxRooms >= loadedAvailable.tbl_units.Rooms)
                                    &&
                                    (d.MinSpace <= loadedAvailable.tbl_units.Space && d.MaxSpace >= loadedAvailable.tbl_units.Space)
                                    &&
                                    (
                                    (
                                    (d.tbl_Regions.RegCode <= loadedAvailable.tbl_units.tbl_Regions.RegCode)
                                    &&
                                    (d.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_units.tbl_Regions.RegCode)
                                    ) ||
                                    (
                                    (d.tbl_Regions.RegCode)) >= (loadedAvailable.tbl_units.tbl_Regions.RegCode)
                                    &&
                                    (d.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_units.tbl_Regions.RegCode)
                                    )
                                    &&
                                    (
                                     //if available is cash
                                     ((loadedAvailable.FK_AvailableUnits_PaymentMethod_Id == 3 && d.FK_DemandUnits_PaymentMethod_Id == 3 || d.tbl_PaymentMethods.IsMaster) && d.MaxPrice >= loadedAvailable.Price)
                                     ||
                                     //if available is installable
                                     ((loadedAvailable.FK_AvailableUnits_PaymentMethod_Id == 4 && d.FK_DemandUnits_PaymentMethod_Id == 4 || d.tbl_PaymentMethods.IsMaster) && d.MaxPrice >= (loadedAvailable.Price + loadedAvailable.Over))
                                    ) &&
                                     (d.tbl_Demand_Finishings
                                     .Where(df => !df.IsDeleted && _dbContext.tbl_Finishings
                                     .Where(f => f.PK_Finishings_Id == df.FK_DemandFinishing_Finish_Id && f.IsMaster).Any()).Any()
                                     )
                                    || d.tbl_Demand_Finishings.Where(df => !df.IsDeleted && df.FK_DemandFinishing_Finish_Id == loadedAvailable.tbl_units.FK_Units_Finishing_Id).Any()
                     ).ToListAsync();

            return demands;
        }

        //public async Task<List<tbl_DemandUnits>> DemandsForMultiUsageAnyPayment(tbl_AvailableUnits available)
        //{
        //    List<tbl_DemandUnits> demands;

        //    var loadedAvailable = await _dbContext.tbl_AvailableUnits
        //        .Where(a => a.PK_AvailableUnits_Id == available.PK_AvailableUnits_Id)
        //        .Include(a => a.tbl_units.tbl_Regions).Include(a => a.tbl_PaymentMethods)
        //        .FirstOrDefaultAsync();

        //    demands = await _dbContext.tbl_DemandUnits
        //        .Include(d => d.tbl_Regions).Include(d => d.tbl_Regions1)
        //        .Include(d => d.tbl_PaymentMethods)
        //        .Include(d => d.tbl_Demand_Finishings)
        //        .Where(d => d.IsClosed == false && !d.IsDeleted &&
        //                          (d.NoOfElevatorsFrom <= loadedAvailable.NoOfElevators && d.NoOfElevatorsTo >= loadedAvailable.NoOfElevators)
        //                          &&
        //                          (d.DateOfBuildFrom <= loadedAvailable.DateOfBuild && d.DateOfBuildTo >= loadedAvailable.DateOfBuild)
        //                          &&
        //                          d.FK_DemandUnits_Transactions_Id == loadedAvailable.FK_AvaliableUnits_Transaction_TransactionId
        //                          &&
        //                          d.MaxPrice >= loadedAvailable.Price
        //                          &&
        //                          (d.MinBathRooms <= loadedAvailable.tbl_units.Bathrooms && d.MaxBathRooms >= loadedAvailable.tbl_units.Bathrooms)
        //                          &&
        //                          d.FK_DemandUnits_Categories_Id == loadedAvailable.tbl_units.FK_Units_Categories_Id
        //                          &&
        //                          (d.MinFloor <= loadedAvailable.tbl_units.Floor && d.MaxFloor >= loadedAvailable.tbl_units.Floor)
        //                          &&
        //                          d.IsFurnished == loadedAvailable.tbl_units.IsFurniture
        //                          &&
        //                          (d.MinRooms <= loadedAvailable.tbl_units.Rooms && d.MaxRooms >= loadedAvailable.tbl_units.Rooms)
        //                          &&
        //                          (d.MinSpace <= loadedAvailable.tbl_units.Space && d.MaxSpace >= loadedAvailable.tbl_units.Space)
        //                          &&
        //                          (
        //                          (
        //                          (d.tbl_Regions.RegCode <= loadedAvailable.tbl_units.tbl_Regions.RegCode)
        //                          &&
        //                          (d.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_units.tbl_Regions.RegCode)
        //                          ) ||
        //                          (
        //                          (d.tbl_Regions.RegCode)) >= (loadedAvailable.tbl_units.tbl_Regions.RegCode)
        //                          &&
        //                          (d.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_units.tbl_Regions.RegCode)
        //                          )
        //                          &&
        //                           (d.tbl_Demand_Finishings
        //                           .Where(df => !df.IsDeleted && _dbContext.tbl_Finishings
        //                           .Where(f => f.PK_Finishings_Id == df.FK_DemandFinishing_Finish_Id && f.IsMaster).Any()).Any()
        //                           )
        //                          || d.tbl_Demand_Finishings.Where(df => !df.IsDeleted && df.FK_DemandFinishing_Finish_Id == loadedAvailable.tbl_units.FK_Units_Finishing_Id).Any()
        //           ).ToListAsync();

        //    return demands;
        //}


        //public async Task<List<tbl_DemandUnits>> DemandsForUsageAnyPayment(tbl_AvailableUnits available)
        //{
        //    List<tbl_DemandUnits> demands;

        //    var loadedAvailable = await _dbContext.tbl_AvailableUnits
        //        .Where(a => a.PK_AvailableUnits_Id == available.PK_AvailableUnits_Id)
        //        .Include(a => a.tbl_units.tbl_Regions).Include(a => a.tbl_PaymentMethods)
        //        .FirstOrDefaultAsync();

        //    demands = await _dbContext.tbl_DemandUnits
        //        .Include(d => d.tbl_Regions).Include(d => d.tbl_Regions1)
        //        .Include(d => d.tbl_PaymentMethods)
        //        .Include(d => d.tbl_Demand_Finishings)
        //        .Where(d => d.IsClosed == false && !d.IsDeleted &&
        //                          (d.NoOfElevatorsFrom <= loadedAvailable.NoOfElevators && d.NoOfElevatorsTo >= loadedAvailable.NoOfElevators)
        //                          &&
        //                          (d.DateOfBuildFrom <= loadedAvailable.DateOfBuild && d.DateOfBuildTo >= loadedAvailable.DateOfBuild)
        //                          &&
        //                          d.FK_DemandUnits_Transactions_Id == loadedAvailable.FK_AvaliableUnits_Transaction_TransactionId
        //                          &&
        //                          (d.FK_DemandUnits_Usage_Id == loadedAvailable.FK_AvailableUnits_Usage_Id || d.FK_DemandUnits_Usage_Id == UnitUsages.Multiple)
        //                          &&
        //                          d.MaxPrice >= loadedAvailable.Price
        //                          &&
        //                          (d.MinBathRooms <= loadedAvailable.tbl_units.Bathrooms && d.MaxBathRooms >= loadedAvailable.tbl_units.Bathrooms)
        //                          &&
        //                          d.FK_DemandUnits_Categories_Id == loadedAvailable.tbl_units.FK_Units_Categories_Id
        //                          &&
        //                          (d.MinFloor <= loadedAvailable.tbl_units.Floor && d.MaxFloor >= loadedAvailable.tbl_units.Floor)
        //                          &&
        //                          d.IsFurnished == loadedAvailable.tbl_units.IsFurniture
        //                          &&
        //                          (d.MinRooms <= loadedAvailable.tbl_units.Rooms && d.MaxRooms >= loadedAvailable.tbl_units.Rooms)
        //                          &&
        //                          (d.MinSpace <= loadedAvailable.tbl_units.Space && d.MaxSpace >= loadedAvailable.tbl_units.Space)
        //                          &&
        //                          (
        //                          (
        //                          (d.tbl_Regions.RegCode <= loadedAvailable.tbl_units.tbl_Regions.RegCode)
        //                          &&
        //                          (d.tbl_Regions1.RegCode) >= (loadedAvailable.tbl_units.tbl_Regions.RegCode)
        //                          ) ||
        //                          (
        //                          (d.tbl_Regions.RegCode)) >= (loadedAvailable.tbl_units.tbl_Regions.RegCode)
        //                          &&
        //                          (d.tbl_Regions1.RegCode) <= (loadedAvailable.tbl_units.tbl_Regions.RegCode)
        //                          )
        //                          &&
        //                           (d.tbl_Demand_Finishings
        //                           .Where(df => !df.IsDeleted && _dbContext.tbl_Finishings
        //                           .Where(f => f.PK_Finishings_Id == df.FK_DemandFinishing_Finish_Id && f.IsMaster).Any()).Any()
        //                           )
        //                          || d.tbl_Demand_Finishings.Where(df => !df.IsDeleted && df.FK_DemandFinishing_Finish_Id == loadedAvailable.tbl_units.FK_Units_Finishing_Id).Any()
        //           ).ToListAsync();
        //    return demands;
        //}

        public async Task<List<tbl_AvailableUnits>> AvailablesForPaymentFinish(tbl_DemandUnits demand, int[] codes, int[] finishIds)
        {
            var lastCode = codes.LastOrDefault();
            demand.tbl_PaymentMethods = await _dbContext.tbl_PaymentMethods.Where(x => x.PK_PaymentMethods_Id == demand.FK_DemandUnits_PaymentMethod_Id).FirstOrDefaultAsync();
            List<tbl_AvailableUnits> availables;
            availables = await _dbContext.tbl_AvailableUnits.Include(a => a.tbl_units.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                  (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                  (a.NoOfElevators >= demand.NoOfElevatorsFrom && a.NoOfElevators <= demand.NoOfElevatorsTo) &&
                                  a.FK_AvaliableUnits_Transaction_TransactionId == demand.FK_DemandUnits_Transactions_Id &&
                                  //a.Price <= demand.MaxPrice &&
                                  !a.tbl_units.IsDeleted &&
                                  (a.tbl_units.Bathrooms >= demand.MinBathRooms && a.tbl_units.Bathrooms <= demand.MaxBathRooms) &&
                                  a.tbl_units.FK_Units_Categories_Id == demand.FK_DemandUnits_Categories_Id &&
                                  (a.tbl_units.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_units.tbl_Regions.RegCode <= lastCode) &&
                                  (a.tbl_units.Floor >= demand.MinFloor && a.tbl_units.Floor <= demand.MaxFloor) &&
                                  a.tbl_units.IsFurniture == demand.IsFurnished &&
                                  (a.tbl_units.Rooms >= demand.MinRooms && a.tbl_units.Rooms <= demand.MaxRooms) &&
                                  (a.tbl_units.Space >= demand.MinSpace && a.tbl_units.Space <= demand.MaxSpace) &&
                                  
                                  (
                                  ////if demand is cash
                                  (((a.FK_AvailableUnits_PaymentMethod_Id ==3&& demand.FK_DemandUnits_PaymentMethod_Id==3 )) && a.Price <= demand.MaxPrice)
                                  ||
                                  //if demand is installable
                                  (((a.FK_AvailableUnits_PaymentMethod_Id == 4 && demand.FK_DemandUnits_PaymentMethod_Id == 4) ) && (a.Price +a.Over)<= demand.MaxPrice)
                                  ||
                                   //if demand is ALL(installable AND cash)
                                   (( demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice )|| (a.Price  <= demand.MaxPrice))
                                  )
                                   &&
                                   finishIds.Contains(a.tbl_units.FK_Units_Finishing_Id)
                               ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_AvailableUnits>> AvailablesForPaymentFinishUsage(tbl_DemandUnits demand, int[] codes, int[] finishIds)
        {
            //add full condition
            try
            {
                List<tbl_AvailableUnits> availables;
                var lastCode = codes.LastOrDefault();
                demand.tbl_PaymentMethods = await _dbContext.tbl_PaymentMethods.Where(p => p.PK_PaymentMethods_Id == demand.FK_DemandUnits_PaymentMethod_Id).FirstOrDefaultAsync();
                
                 availables = await _dbContext.tbl_AvailableUnits.Include(a => a.tbl_units.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                  (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                  (a.NoOfElevators >= demand.NoOfElevatorsFrom && a.NoOfElevators <= demand.NoOfElevatorsTo) &&
                                  (a.FK_AvailableUnits_Usage_Id == demand.FK_DemandUnits_Usage_Id || a.FK_AvailableUnits_Usage_Id == UnitUsages.Multiple) &&
                                  a.FK_AvaliableUnits_Transaction_TransactionId == demand.FK_DemandUnits_Transactions_Id &&
                                  //a.Price <= demand.MaxPrice &&
                                  !a.tbl_units.IsDeleted &&
                                  (a.tbl_units.Bathrooms >= demand.MinBathRooms && a.tbl_units.Bathrooms <= demand.MaxBathRooms) &&
                                  a.tbl_units.FK_Units_Categories_Id == demand.FK_DemandUnits_Categories_Id
                                  &&
                                  (a.tbl_units.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_units.tbl_Regions.RegCode <= lastCode) &&
                                  (a.tbl_units.Floor >= demand.MinFloor && a.tbl_units.Floor <= demand.MaxFloor) &&
                                  a.tbl_units.IsFurniture == demand.IsFurnished &&
                                  (a.tbl_units.Rooms >= demand.MinRooms && a.tbl_units.Rooms <= demand.MaxRooms) &&
                                  (a.tbl_units.Space >= demand.MinSpace && a.tbl_units.Space <= demand.MaxSpace) &&
                                   (
                                      ////if demand is cash
                                      (((a.FK_AvailableUnits_PaymentMethod_Id == 3 && demand.FK_DemandUnits_PaymentMethod_Id == 3)) && a.Price <= demand.MaxPrice)
                                      ||
                                      //if demand is installable
                                      (((a.FK_AvailableUnits_PaymentMethod_Id == 4 && demand.FK_DemandUnits_PaymentMethod_Id == 4)) && (a.Price + a.Over) <= demand.MaxPrice)
                                      ||
                                       //if demand is ALL(installable AND cash)
                                       ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                      )
                                       &&
                                  finishIds.Contains(a.tbl_units.FK_Units_Finishing_Id)
                                  ).ToListAsync();
                return availables;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<tbl_AvailableUnits>> AvailableMatchesForMaster(tbl_DemandUnits demand, int[] codes)
        {
            //add thirs condition
            var lastCode = codes.LastOrDefault();
            List<tbl_AvailableUnits> availables;
            demand.tbl_PaymentMethods = await _dbContext.tbl_PaymentMethods.Where(p => p.PK_PaymentMethods_Id == demand.FK_DemandUnits_PaymentMethod_Id).FirstOrDefaultAsync();

            availables = await _dbContext.tbl_AvailableUnits.Include(a => a.tbl_units.tbl_Regions).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                        (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                        (a.NoOfElevators >= demand.NoOfElevatorsFrom && a.NoOfElevators <= demand.NoOfElevatorsTo) &&
                                        a.FK_AvaliableUnits_Transaction_TransactionId == demand.FK_DemandUnits_Transactions_Id &&
                                        //a.Price <= demand.MaxPrice &&
                                        ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                        &&
                                        !a.tbl_units.IsDeleted &&
                                        (a.tbl_units.Bathrooms >= demand.MinBathRooms && a.tbl_units.Bathrooms <= demand.MaxBathRooms) &&
                                        a.tbl_units.FK_Units_Categories_Id == demand.FK_DemandUnits_Categories_Id
                                        &&
                                        (a.tbl_units.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_units.tbl_Regions.RegCode <= lastCode) &&
                                        (a.tbl_units.Floor >= demand.MinFloor && a.tbl_units.Floor <= demand.MaxFloor) &&
                                        a.tbl_units.IsFurniture == demand.IsFurnished &&
                                        (a.tbl_units.Rooms >= demand.MinRooms && a.tbl_units.Rooms <= demand.MaxRooms) &&
                                        (a.tbl_units.Space >= demand.MinSpace && a.tbl_units.Space <= demand.MaxSpace)
                                       ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_AvailableUnits>> AvailableMatchesForUsage(tbl_DemandUnits demand, int[] codes)
        {
            /// third condition
            var lastCode = codes.LastOrDefault();
            List<tbl_AvailableUnits> availables;
            demand.tbl_PaymentMethods = await _dbContext.tbl_PaymentMethods.Where(p => p.PK_PaymentMethods_Id == demand.FK_DemandUnits_PaymentMethod_Id).FirstOrDefaultAsync();

            availables = await _dbContext.tbl_AvailableUnits.Include(a => a.tbl_units.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                          (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                          (a.NoOfElevators >= demand.NoOfElevatorsFrom && a.NoOfElevators <= demand.NoOfElevatorsTo) &&
                                          (a.FK_AvailableUnits_Usage_Id == demand.FK_DemandUnits_Usage_Id || a.FK_AvailableUnits_Usage_Id == UnitUsages.Multiple) &&
                                          a.FK_AvaliableUnits_Transaction_TransactionId == demand.FK_DemandUnits_Transactions_Id &&
                                          //a.Price <= demand.MaxPrice &&
                                          ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                          &&
                                          !a.tbl_units.IsDeleted &&
                                          (a.tbl_units.Bathrooms >= demand.MinBathRooms && a.tbl_units.Bathrooms <= demand.MaxBathRooms) &&
                                          a.tbl_units.FK_Units_Categories_Id == demand.FK_DemandUnits_Categories_Id
                                          &&
                                          (a.tbl_units.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_units.tbl_Regions.RegCode <= lastCode) &&
                                          (a.tbl_units.Floor >= demand.MinFloor && a.tbl_units.Floor <= demand.MaxFloor) &&
                                          a.tbl_units.IsFurniture == demand.IsFurnished &&
                                          (a.tbl_units.Rooms >= demand.MinRooms && a.tbl_units.Rooms <= demand.MaxRooms) &&
                                          (a.tbl_units.Space >= demand.MinSpace && a.tbl_units.Space <= demand.MaxSpace)
                                          ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_AvailableUnits>> AvailableMatchesForFinish(tbl_DemandUnits demand, int[] codes, int[] finishIds)
        {
            //third condition
            var lastCode = codes.LastOrDefault();
            List<tbl_AvailableUnits> availables;
            demand.tbl_PaymentMethods = await _dbContext.tbl_PaymentMethods.Where(p => p.PK_PaymentMethods_Id == demand.FK_DemandUnits_PaymentMethod_Id).FirstOrDefaultAsync();

            availables = await _dbContext.tbl_AvailableUnits.Include(a => a.tbl_units.tbl_Regions).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                         (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                         (a.NoOfElevators >= demand.NoOfElevatorsFrom && a.NoOfElevators <= demand.NoOfElevatorsTo) &&
                                         a.FK_AvaliableUnits_Transaction_TransactionId == demand.FK_DemandUnits_Transactions_Id &&
                                         //a.Price <= demand.MaxPrice &&
                                         ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                         &&
                                         !a.tbl_units.IsDeleted &&
                                         (a.tbl_units.Bathrooms >= demand.MinBathRooms && a.tbl_units.Bathrooms <= demand.MaxBathRooms) &&
                                         a.tbl_units.FK_Units_Categories_Id == demand.FK_DemandUnits_Categories_Id
                                         &&
                                         (a.tbl_units.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_units.tbl_Regions.RegCode <= lastCode) &&
                                         (a.tbl_units.Floor >= demand.MinFloor && a.tbl_units.Floor <= demand.MaxFloor) &&
                                         a.tbl_units.IsFurniture == demand.IsFurnished &&
                                         (a.tbl_units.Rooms >= demand.MinRooms && a.tbl_units.Rooms <= demand.MaxRooms) &&
                                         (a.tbl_units.Space >= demand.MinSpace && a.tbl_units.Space <= demand.MaxSpace) &&
                                         finishIds.Contains(a.tbl_units.FK_Units_Finishing_Id)
                                         ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_AvailableUnits>> AvailableMatchesForFinishAndUsage(tbl_DemandUnits demand, int[] codes, int[] finishIds)
        {
            //third condition
            var lastCode = codes.LastOrDefault();
            List<tbl_AvailableUnits> availables;
            demand.tbl_PaymentMethods = await _dbContext.tbl_PaymentMethods.Where(p => p.PK_PaymentMethods_Id == demand.FK_DemandUnits_PaymentMethod_Id).FirstOrDefaultAsync();

            availables = await _dbContext.tbl_AvailableUnits.Include(a => a.tbl_units.tbl_Regions).Where(a => a.IsClosed == false && !a.IsDeleted &&
                           (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                           (a.NoOfElevators >= demand.NoOfElevatorsFrom && a.NoOfElevators <= demand.NoOfElevatorsTo) &&
                           (a.FK_AvailableUnits_Usage_Id == demand.FK_DemandUnits_Usage_Id || a.FK_AvailableUnits_Usage_Id == UnitUsages.Multiple) &&
                           a.FK_AvaliableUnits_Transaction_TransactionId == demand.FK_DemandUnits_Transactions_Id &&
                           //a.Price <= demand.MaxPrice &&
                           ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                           &&
                           !a.tbl_units.IsDeleted &&
                           (a.tbl_units.Bathrooms >= demand.MinBathRooms && a.tbl_units.Bathrooms <= demand.MaxBathRooms) &&
                           a.tbl_units.FK_Units_Categories_Id == demand.FK_DemandUnits_Categories_Id
                           &&
                           (a.tbl_units.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_units.tbl_Regions.RegCode <= lastCode) &&
                           (a.tbl_units.Floor >= demand.MinFloor && a.tbl_units.Floor <= demand.MaxFloor) &&
                           a.tbl_units.IsFurniture == demand.IsFurnished &&
                           (a.tbl_units.Rooms >= demand.MinRooms && a.tbl_units.Rooms <= demand.MaxRooms) &&
                           (a.tbl_units.Space >= demand.MinSpace && a.tbl_units.Space <= demand.MaxSpace) &&
                           finishIds.Contains(a.tbl_units.FK_Units_Finishing_Id)
                           ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_AvailableUnits>> AvailableMatchesForPayment(tbl_DemandUnits demand, int[] codes)
        {
            //full condition
            var lastCode = codes.LastOrDefault();
            List<tbl_AvailableUnits> availables;
            demand.tbl_PaymentMethods = await _dbContext.tbl_PaymentMethods.Where(p => p.PK_PaymentMethods_Id == demand.FK_DemandUnits_PaymentMethod_Id).FirstOrDefaultAsync();

            availables = await _dbContext.tbl_AvailableUnits.Include(a => a.tbl_units.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                      (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                      (a.NoOfElevators >= demand.NoOfElevatorsFrom && a.NoOfElevators <= demand.NoOfElevatorsTo) &&
                                      a.FK_AvaliableUnits_Transaction_TransactionId == demand.FK_DemandUnits_Transactions_Id &&
                                      //a.Price <= demand.MaxPrice &&
                                      !a.tbl_units.IsDeleted &&
                                      (a.tbl_units.Bathrooms >= demand.MinBathRooms && a.tbl_units.Bathrooms <= demand.MaxBathRooms) &&
                                      a.tbl_units.FK_Units_Categories_Id == demand.FK_DemandUnits_Categories_Id
                                      &&
                                      (a.tbl_units.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_units.tbl_Regions.RegCode <= lastCode)
                                      &&
                                      ////viewsIds.Contains(a.tbl_units.FK_Units_Views_Id) &&
                                      (a.tbl_units.Floor >= demand.MinFloor && a.tbl_units.Floor <= demand.MaxFloor) &&
                                      a.tbl_units.IsFurniture == demand.IsFurnished &&
                                      (a.tbl_units.Rooms >= demand.MinRooms && a.tbl_units.Rooms <= demand.MaxRooms) &&
                                     (a.tbl_units.Space >= demand.MinSpace && a.tbl_units.Space <= demand.MaxSpace) &&
                                        (
                                  ////if demand is cash
                                  (((a.FK_AvailableUnits_PaymentMethod_Id == 3 && demand.FK_DemandUnits_PaymentMethod_Id == 3)) && a.Price <= demand.MaxPrice)
                                  ||
                                  //if demand is installable
                                  (((a.FK_AvailableUnits_PaymentMethod_Id == 4 && demand.FK_DemandUnits_PaymentMethod_Id == 4)) && (a.Price + a.Over) <= demand.MaxPrice)
                                  ||
                                   //if demand is ALL(installable AND cash)
                                   ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                  )
                                   
                                    ).ToListAsync();
            return availables;
        }

        public async Task<List<tbl_AvailableUnits>> AvailableMatchesForPaymentAndUsage(tbl_DemandUnits demand, int[] codes)
        {
            //full condition
            var lastCode = codes.LastOrDefault();
            List<tbl_AvailableUnits> availables;
            demand.tbl_PaymentMethods = await _dbContext.tbl_PaymentMethods.Where(p => p.PK_PaymentMethods_Id == demand.FK_DemandUnits_PaymentMethod_Id).FirstOrDefaultAsync();

            availables = await _dbContext.tbl_AvailableUnits.Include(a => a.tbl_units.tbl_Regions).Include(a => a.tbl_PaymentMethods).Where(a => a.IsClosed == false && !a.IsDeleted &&
                                   (a.DateOfBuild >= demand.DateOfBuildFrom && a.DateOfBuild <= demand.DateOfBuildTo) &&
                                   (a.NoOfElevators >= demand.NoOfElevatorsFrom && a.NoOfElevators <= demand.NoOfElevatorsTo) &&
                                   (a.FK_AvailableUnits_Usage_Id == demand.FK_DemandUnits_Usage_Id || a.FK_AvailableUnits_Usage_Id == UnitUsages.Multiple) &&
                                   a.FK_AvaliableUnits_Transaction_TransactionId == demand.FK_DemandUnits_Transactions_Id &&
                                   //a.Price <= demand.MaxPrice &&
                                   !a.tbl_units.IsDeleted &&
                                   (a.tbl_units.Bathrooms >= demand.MinBathRooms && a.tbl_units.Bathrooms <= demand.MaxBathRooms) &&
                                   a.tbl_units.FK_Units_Categories_Id == demand.FK_DemandUnits_Categories_Id &&                                
                                   (a.tbl_units.tbl_Regions.RegCode >= codes.FirstOrDefault() && a.tbl_units.tbl_Regions.RegCode <= lastCode) &&
                                   (a.tbl_units.Floor >= demand.MinFloor && a.tbl_units.Floor <= demand.MaxFloor) &&
                                   a.tbl_units.IsFurniture == demand.IsFurnished &&
                                   (a.tbl_units.Rooms >= demand.MinRooms && a.tbl_units.Rooms <= demand.MaxRooms) &&
                                   (a.tbl_units.Space >= demand.MinSpace && a.tbl_units.Space <= demand.MaxSpace) &&
                                     (
                                  ////if demand is cash
                                  (((a.FK_AvailableUnits_PaymentMethod_Id == 3 && demand.FK_DemandUnits_PaymentMethod_Id == 3)) && a.Price <= demand.MaxPrice)
                                  ||
                                  //if demand is installable
                                  (((a.FK_AvailableUnits_PaymentMethod_Id == 4 && demand.FK_DemandUnits_PaymentMethod_Id == 4)) && (a.Price + a.Over) <= demand.MaxPrice)
                                  ||
                                   //if demand is ALL(installable AND cash)
                                   ((demand.tbl_PaymentMethods.IsMaster) && ((a.Price + a.Over) <= demand.MaxPrice) || (a.Price <= demand.MaxPrice))
                                  )
                                   
                                   ).ToListAsync();
            return availables;
        }

        public async Task<List<DemandsWithPreviews>> FilterDemandsHasPreviews(List<tbl_DemandUnits> empDemands, List<tbl_DemandUnits> colleguesDemands,AvailableDto available)
        {

            //remove the rejected 
            //     && (empDemands.Any(d => d.PK_DemandUnits_Id == header.DemandUnit_Id && d.FK_DemandUnits_Clients_ClientId == header.FK_PreviewHeaders_Clients_BuyerId)
            //     ||
            //     colleguesDemands.Any(d => d.PK_DemandUnits_Id == header.DemandUnit_Id && d.FK_DemandUnits_Clients_ClientId == header.FK_PreviewHeaders_Clients_BuyerId))
            //         select new DemandsWithPreviews
            //    {
            //    HeaderId = header.PK_PreviewHeaders_Id,
            //    PreviewDate = header.ReviewDate,
            //    DemandId = header.DemandUnit_Id,
            //    BuyerId = header.FK_PreviewHeaders_Clients_BuyerId,
            //    AvailableId = detail.AvailableUnits_Id,
            //    Seller = detail.Fk_PreviewDetails_Clients_SellerId,
            //    DetailId = detail.PK_PreviewDetails_Id
            //}).ToListAsync();

            var headers = await (from header in _dbContext.tbl_PreviewHeaders
                                 join detail
                                 in _dbContext.tbl_PreviewDetails
                                 on header.PK_PreviewHeaders_Id equals detail.Fk_PreviewDetails_PreviewHeaders_Id
                                 where (detail.AvailableUnits_Id == available.PK_AvailableUnits_Id && detail.Fk_PreviewDetails_Clients_SellerId == available.FK_AvaliableUnits_Clients_ClientId)
                                      && (header.ReviewDate >= DateTime.Today)
                                      && !header.IsDeleted && !detail.IsDeleted && detail.IsNoDecision
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
                var filterdHeaders = headers.Where(h => empDemands.Any(d => d.PK_DemandUnits_Id == h.DemandId && d.FK_DemandUnits_Clients_ClientId == h.BuyerId) ||
                                                        colleguesDemands.Any(d => d.PK_DemandUnits_Id == h.DemandId && d.FK_DemandUnits_Clients_ClientId == h.BuyerId));

            return filterdHeaders.ToList();
        }
    }


}
