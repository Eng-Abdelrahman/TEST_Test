using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using _3aqarak.BLL.Models;
using _3aqarak.DAL.Repositories.CustomRepositories;
using _3aqarak.DAL.Repositories;
using _3aqarak.DAL.Models;
using _3aqarak.BLL.Interfaces.Repositories;
using _3aqarak.DAL.Services;
using _3aqarak.BLL.Domain;

namespace _3aqarak.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        //Custom repositories

        public IScalarValsRepository<tbl_units> UnitCustRepo { get; }
        public IScalarValsRepository<tbl_AvailableUnits> AvailableCustRepo { get; }
        public IScalarValsRepository<tbl_DemandUnits> DemandCustRepo { get; }
        public IScalarValsRepository<tbl_VillasAvailables> VillasAvailablesCustRepo { get; }
        public IScalarValsRepository<tbl_VillasDemands> VillasDemandsCustRepo { get; }
        public IScalarValsRepository<tbl_ShopAvailable> ShopAvailableCustRepo { get; }
        public IScalarValsRepository<tbl_ShopDemands> ShopDemandsCustRepo { get; }
        public IScalarValsRepository<tbl_AvailableLands> AvailableLandsCustRepo { get; }
        public IScalarValsRepository<tbl_LandsDemands> LandsDemandsCustRepo { get; }
        public IAppartmentCustomRepository AppartmentCustomrepository { get; }
        public IVillaCustomRepository VillaCustomrepository { get; }
        public IShopCustomRepository ShopCustomRepository { get; }
        public ILandCustomRepository LandCustomRepository { get; }
        public ICustomPreviewRepository PreviewCustomRepository { get; }



        /// <summary>
        /// custom REepo without Async
        /// </summary>
        public IFilterService<tbl_Users> UserCustRepoWithoutAsync { get; }
        public IFilterService<tbl_Branches> BranchesCustRepoWithoutAsync { get; }
        public IFilterService<tbl_RentAgreementHeaders> RentAgreementHeaderWithoutAsync { get; }
        public IFilterService<tbl_ExpectedContracts> ExpectedContractWithoutAsync { get; }
        public IFilterService<tbl_PostbonedCalls> PostboneWithoutAsync { get; }
        public IFilterService<tbl_FellowupCall> FellowupCallWithoutAsync { get; }


        //Generic repositories
        public IRepository<tbl_Messages> MessagesRepo { get; }
        public IRepository<tbl_Users> UserRepo { get; }

        public IRepository<tbl_Genders> GenderRepo { get; }

        public IRepository<tbl_Clients> ClientRepo { get; }

        public IRepository<tbl_Regions> RegionRepo { get; }

        public IRepository<tbl_Accessories> AcssRepo { get; }

        public IRepository<tbl_Finishings> FinishRepo { get; }

        public IRepository<tbl_PaymentBasis> PayBasisRepo { get; }

        public IRepository<tbl_Transactions> TransRepo { get; }

        public IRepository<tbl_Categories> CatRepo { get; }

        public IRepository<tbl_StaticContracts> STContRepo { get; }

        public IRepository<tbl_AvailableUnits> AvailableRepo { get; }

        public IRepository<tbl_DemandUnits> DemandRepo { get; }

        public IRepository<tbl_units> UnitRepo { get; }

        public IRepository<tbl_UnitImages> UnitImagesRepo { get; }

        public IRepository<tbl_UnitAccessories> UnitAccessRepo { get; }

        public IRepository<tbl_Views> ViewsRepo { get; }

        public IRepository<tbl_DemandViews> DemandViewRepo { get; }

        public IRepository<tbl_PaymentMethods> PaymentRepo { get; }

        public IRepository<tbl_DemandAccessories> DemandAccessRepo { get; }

        public IRepository<tbl_PreviewHeaders> PreviewRepo { get; }

        public IRepository<tbl_PreviewDetails> PreviewDetailRepo { get; }

        public IRepository<tbl_Branches> BranchRepo { get; }

        public IRepository<tbl_Departements> DeptRepo { get; }

        public IRepository<tbl_Demand_Finishings> DemandFinishRepo { get; }

        public IRepository<tbl_Specializations> SpecialRepo { get; }

        public IRepository<tbl_ClientsCalls> ClienCallRepo { get; }

        public IRepository<tbl_FellowupCall> FellowupCallsRepo { get; }


        public IRepository<tbl_PostbonedCalls> PostponedRepo { get; }

        public IRepository<tbl_ExpectedContracts> ExpectedRepo { get; }

        public IRepository<tbl_RentAgreementDetails> RentDetailsRepo { get; }

        public IRepository<tbl_RentAgreementHeaders> RentHeaderRepo { get; }

        public IRepository<tbl_SaleAgreementDetails> SaleDetailsRepo { get; }

        public IRepository<tbl_SaleAgreementHeaders> SaleHeaderRepo { get; }

        public IRepository<tbl_FinancialItems> FinancialRepo { get; }

        public IRepository<tbl_Commissions> CommissionsRepo { get; }

        public IRepository<tbl_EmpCommissions> EmpCommRepo { get; }

        public IRepository<tbl_CompCommissions> CompCommRepo { get; }

        public IRepository<tbl_FinancialSummaries> FinancialSummaryRepo { get; }
        /// <summary>
        /// add new ContractArchiveRepo here
        /// </summary>
        public IRepository<tbl_ContractArchives> ContractArchiveRepo { get; }

        public IRepository<tbl_EventLogs> EventLogsRepo { get; }

        public IRepository<tbl_UnitUsage> UnitUsageRepo { get; }

        public IRepository<tbl_AvailableLands> AvailableLandsRepo { get; }

        public IRepository<tbl_VillasAvailables> VillasAvailablesRepo { get; }

        public IRepository<tbl_VillaAccessories> VillaAccessoriesRepo { get; }

        public IRepository<tbl_VillasImages> VillasImagesRepo { get; }

        public IRepository<tbl_VillasDemands> VillasDemandsRepo { get; }

        public IRepository<tbl_VillaDemandAccessories> VillaDemandAccessoriesRepo { get; }

        public IRepository<tbl_VillademandFinishings> VillademandFinishingsRepo { get; }

        public IRepository<tbl_VillasDemandViews> VillasDemandViewsRepo { get; }

        public IRepository<tbl_LandsDemands> LandsDemandsRepo { get; }
        public IRepository<tbl_LandImages> LandImagesRepo { get; }
        public IRepository<tbl_LandDemandViews> LandsDemandViewsRepo { get; }
        public IRepository<tbl_ShopDemands> ShopDemandsRepo { get; }
        public IRepository<tbl_ShopDemandAccessories> ShopDemandAccessoriesRepo { get; }
        public IRepository<tbl_ShopDemandViews> ShopDemandViewsRepo { get; }

        /// <summary>
        /// add shop available her
        /// </summary>
        public IRepository<tbl_ShopAvailable> ShopAvailableRepo { get; }
        public IRepository<tbl_ShopAvailableImages> ShopAvailableImagesRepo { get; }

        public IRepository<tbl_ShopAvailabeAccessories> ShopAvailableAccessoriesRepo { get; }

        public IRepository<tbl_Posts> PostsRepo  { get; }


        private readonly RealEstateDB _dbContext;

        private bool _disposed = false;


        public UnitOfWork(string connectionString)
        {
            _dbContext = new RealEstateDB(connectionString);
            UnitCustRepo = new ScalarValsRepository<tbl_units>(_dbContext);
            AvailableCustRepo = new ScalarValsRepository<tbl_AvailableUnits>(_dbContext);
            DemandCustRepo = new ScalarValsRepository<tbl_DemandUnits>(_dbContext);
            VillasAvailablesCustRepo = new ScalarValsRepository<tbl_VillasAvailables>(_dbContext);
            VillasDemandsCustRepo = new ScalarValsRepository<tbl_VillasDemands>(_dbContext);
            ShopAvailableCustRepo = new ScalarValsRepository<tbl_ShopAvailable>(_dbContext);
            ShopDemandsCustRepo = new ScalarValsRepository<tbl_ShopDemands>(_dbContext);
            AvailableLandsCustRepo = new ScalarValsRepository<tbl_AvailableLands>(_dbContext);
            LandsDemandsCustRepo = new ScalarValsRepository<tbl_LandsDemands>(_dbContext);
            AppartmentCustomrepository = new AppartmentCustomRepository(_dbContext);
            VillaCustomrepository = new VillaCustomRepository(_dbContext);
            ShopCustomRepository = new ShopCustomRepository(_dbContext);
            LandCustomRepository = new LandCustomRepository(_dbContext);
            PreviewCustomRepository = new CustomPreviewRepository(_dbContext);

            UserCustRepoWithoutAsync = new FilterService<tbl_Users>(_dbContext);
            BranchesCustRepoWithoutAsync = new FilterService<tbl_Branches>(_dbContext);
            RentAgreementHeaderWithoutAsync = new FilterService<tbl_RentAgreementHeaders>(_dbContext);
            ExpectedContractWithoutAsync = new FilterService<tbl_ExpectedContracts>(_dbContext);
            PostboneWithoutAsync = new FilterService<tbl_PostbonedCalls>(_dbContext);

            MessagesRepo = new GenericRepository<tbl_Messages>(_dbContext);
            PostsRepo = new GenericRepository<tbl_Posts>(_dbContext);
            UserRepo = new GenericRepository<tbl_Users>(_dbContext);
            ClientRepo = new GenericRepository<tbl_Clients>(_dbContext);
            RegionRepo = new GenericRepository<tbl_Regions>(_dbContext);
            GenderRepo = new GenericRepository<tbl_Genders>(_dbContext);
            AcssRepo = new GenericRepository<tbl_Accessories>(_dbContext);
            FinishRepo = new GenericRepository<tbl_Finishings>(_dbContext);
            PayBasisRepo = new GenericRepository<tbl_PaymentBasis>(_dbContext);
            TransRepo = new GenericRepository<tbl_Transactions>(_dbContext);
            CatRepo = new GenericRepository<tbl_Categories>(_dbContext);
            STContRepo = new GenericRepository<tbl_StaticContracts>(_dbContext);
            AvailableRepo = new GenericRepository<tbl_AvailableUnits>(_dbContext);
            DemandRepo = new GenericRepository<tbl_DemandUnits>(_dbContext);
            UnitRepo = new GenericRepository<tbl_units>(_dbContext);
            UnitImagesRepo = new GenericRepository<tbl_UnitImages>(_dbContext);
            UnitAccessRepo = new GenericRepository<tbl_UnitAccessories>(_dbContext);
            ViewsRepo = new GenericRepository<tbl_Views>(_dbContext);
            DemandViewRepo = new GenericRepository<tbl_DemandViews>(_dbContext);
            PaymentRepo = new GenericRepository<tbl_PaymentMethods>(_dbContext);
            DemandAccessRepo = new GenericRepository<tbl_DemandAccessories>(_dbContext);
            PreviewRepo = new GenericRepository<tbl_PreviewHeaders>(_dbContext);
            PreviewDetailRepo = new GenericRepository<tbl_PreviewDetails>(_dbContext);
            BranchRepo = new GenericRepository<tbl_Branches>(_dbContext);
            DeptRepo = new GenericRepository<tbl_Departements>(_dbContext);
            DemandFinishRepo = new GenericRepository<tbl_Demand_Finishings>(_dbContext);
            SpecialRepo = new GenericRepository<tbl_Specializations>(_dbContext);
            ClienCallRepo = new GenericRepository<tbl_ClientsCalls>(_dbContext);
            PostponedRepo = new GenericRepository<tbl_PostbonedCalls>(_dbContext);
            ExpectedRepo = new GenericRepository<tbl_ExpectedContracts>(_dbContext);
            RentDetailsRepo = new GenericRepository<tbl_RentAgreementDetails>(_dbContext);
            RentHeaderRepo = new GenericRepository<tbl_RentAgreementHeaders>(_dbContext);
            SaleDetailsRepo = new GenericRepository<tbl_SaleAgreementDetails>(_dbContext);
            SaleHeaderRepo = new GenericRepository<tbl_SaleAgreementHeaders>(_dbContext);
            FinancialRepo = new GenericRepository<tbl_FinancialItems>(_dbContext);
            CommissionsRepo = new GenericRepository<tbl_Commissions>(_dbContext);
            EmpCommRepo = new GenericRepository<tbl_EmpCommissions>(_dbContext);
            CompCommRepo = new GenericRepository<tbl_CompCommissions>(_dbContext);
            FinancialSummaryRepo = new GenericRepository<tbl_FinancialSummaries>(_dbContext);
            //add new change here
            ContractArchiveRepo = new GenericRepository<tbl_ContractArchives>(_dbContext);
            EventLogsRepo = new GenericRepository<tbl_EventLogs>(_dbContext);
            UnitUsageRepo = new GenericRepository<tbl_UnitUsage>(_dbContext);
            AvailableLandsRepo = new GenericRepository<tbl_AvailableLands>(_dbContext);
            VillasAvailablesRepo = new GenericRepository<tbl_VillasAvailables>(_dbContext);
            VillaAccessoriesRepo = new GenericRepository<tbl_VillaAccessories>(_dbContext);
            VillasImagesRepo = new GenericRepository<tbl_VillasImages>(_dbContext);
            VillasDemandsRepo = new GenericRepository<tbl_VillasDemands>(_dbContext);
            VillaDemandAccessoriesRepo = new GenericRepository<tbl_VillaDemandAccessories>(_dbContext);
            VillademandFinishingsRepo = new GenericRepository<tbl_VillademandFinishings>(_dbContext);
            VillasDemandViewsRepo = new GenericRepository<tbl_VillasDemandViews>(_dbContext);
            LandsDemandsRepo = new GenericRepository<tbl_LandsDemands>(_dbContext);
            LandImagesRepo = new GenericRepository<tbl_LandImages>(_dbContext);
            LandsDemandViewsRepo=new GenericRepository<tbl_LandDemandViews>(_dbContext);
            //add Shop here Mostafa  
            ShopDemandsRepo = new GenericRepository<tbl_ShopDemands>(_dbContext);
            ShopDemandAccessoriesRepo = new GenericRepository<tbl_ShopDemandAccessories>(_dbContext);
            ShopDemandViewsRepo = new GenericRepository<tbl_ShopDemandViews>(_dbContext);
            /// add shop available and its access
            ShopAvailableRepo = new GenericRepository<tbl_ShopAvailable>(_dbContext);
            ShopAvailableImagesRepo = new GenericRepository<tbl_ShopAvailableImages>(_dbContext);
            ShopAvailableAccessoriesRepo = new GenericRepository<tbl_ShopAvailabeAccessories>(_dbContext);
            FellowupCallsRepo = new GenericRepository<tbl_FellowupCall>(_dbContext);
            FellowupCallWithoutAsync= new FilterService<tbl_FellowupCall>(_dbContext);


        }

        public async Task<int> SaveAsync()
        {

                return await this._dbContext.SaveChangesAsync();
        }

        private void _Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._dbContext.Dispose();
                }
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            this._Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
