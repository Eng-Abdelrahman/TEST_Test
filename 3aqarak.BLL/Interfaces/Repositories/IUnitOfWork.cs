
using _3aqarak.BLL.Models;
using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3aqarak.BLL.Interfaces.Repositories;
using _3aqarak.BLL.Domain;

namespace _3aqarak.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        //**** add without Async 
        IFilterService<tbl_Users> UserCustRepoWithoutAsync { get; }
        IFilterService<tbl_Branches> BranchesCustRepoWithoutAsync { get; }
        IFilterService<tbl_RentAgreementHeaders> RentAgreementHeaderWithoutAsync { get; }
        IFilterService<tbl_ExpectedContracts> ExpectedContractWithoutAsync { get; }
        IFilterService<tbl_PostbonedCalls> PostboneWithoutAsync { get; }
        IFilterService<tbl_FellowupCall> FellowupCallWithoutAsync { get; }



        //*** this part for get Max Value for some parameter 
        IScalarValsRepository<tbl_units> UnitCustRepo { get; }
        IScalarValsRepository<tbl_AvailableUnits> AvailableCustRepo { get; }
        IScalarValsRepository<tbl_DemandUnits> DemandCustRepo { get; }
        IScalarValsRepository<tbl_VillasAvailables> VillasAvailablesCustRepo { get; }
        IScalarValsRepository<tbl_VillasDemands> VillasDemandsCustRepo { get; }
        IScalarValsRepository<tbl_ShopAvailable> ShopAvailableCustRepo { get; }
        IScalarValsRepository<tbl_ShopDemands> ShopDemandsCustRepo { get; }
        IScalarValsRepository<tbl_AvailableLands> AvailableLandsCustRepo { get; }
        IScalarValsRepository<tbl_LandsDemands> LandsDemandsCustRepo { get; }
        //***********************************
        //Custom reositories
        IAppartmentCustomRepository AppartmentCustomrepository { get; }
        IVillaCustomRepository VillaCustomrepository { get; }
        IShopCustomRepository ShopCustomRepository { get; }
        ILandCustomRepository LandCustomRepository { get; }
        ICustomPreviewRepository PreviewCustomRepository { get; }



        //Generic repositories
        IRepository<tbl_Messages> MessagesRepo { get; }
        IRepository<tbl_Users> UserRepo { get; }
        IRepository<tbl_Genders> GenderRepo { get; }
        IRepository<tbl_Clients> ClientRepo { get; }
        IRepository<tbl_Regions> RegionRepo { get; }
        IRepository<tbl_Accessories> AcssRepo { get; }
        IRepository<tbl_Finishings> FinishRepo { get; }
        IRepository<tbl_PaymentBasis> PayBasisRepo { get; }
        IRepository<tbl_Transactions> TransRepo { get; }
        IRepository<tbl_Categories> CatRepo { get; }
        IRepository<tbl_StaticContracts> STContRepo { get; }
        IRepository<tbl_AvailableUnits> AvailableRepo { get; }
        IRepository<tbl_DemandUnits> DemandRepo { get; }
        IRepository<tbl_units> UnitRepo { get; }
        IRepository<tbl_UnitImages> UnitImagesRepo { get; }
        IRepository<tbl_UnitAccessories> UnitAccessRepo { get; }
        IRepository<tbl_Views> ViewsRepo { get; }
        IRepository<tbl_DemandViews> DemandViewRepo { get; }
        IRepository<tbl_DemandAccessories> DemandAccessRepo { get; }
        IRepository<tbl_PaymentMethods> PaymentRepo { get; }
        IRepository<tbl_PreviewHeaders> PreviewRepo { get; }
        IRepository<tbl_PreviewDetails> PreviewDetailRepo { get; }
        IRepository<tbl_Branches> BranchRepo { get; }
        IRepository<tbl_Departements> DeptRepo { get; }
        IRepository<tbl_Demand_Finishings> DemandFinishRepo { get; }
        IRepository<tbl_Specializations> SpecialRepo { get; }
        IRepository<tbl_ClientsCalls> ClienCallRepo { get; }
        IRepository<tbl_FellowupCall> FellowupCallsRepo { get; }

        IRepository<tbl_PostbonedCalls> PostponedRepo { get; }
        IRepository<tbl_ExpectedContracts> ExpectedRepo { get; }
        IRepository<tbl_RentAgreementDetails> RentDetailsRepo { get; }
        IRepository<tbl_RentAgreementHeaders> RentHeaderRepo { get; }
        IRepository<tbl_SaleAgreementDetails> SaleDetailsRepo { get; }
        IRepository<tbl_SaleAgreementHeaders> SaleHeaderRepo { get; }
        IRepository<tbl_FinancialItems> FinancialRepo { get; }
        IRepository<tbl_Commissions> CommissionsRepo { get; }
        IRepository<tbl_EmpCommissions> EmpCommRepo { get; }
        IRepository<tbl_CompCommissions> CompCommRepo { get; }
        IRepository<tbl_FinancialSummaries> FinancialSummaryRepo { get; }
        IRepository<tbl_Posts> PostsRepo { get; }

        IRepository<tbl_UnitUsage> UnitUsageRepo { get; }
        /// <summary>
        /// Add new Repo from ContractArchives
        /// </summary>
        IRepository<tbl_ContractArchives> ContractArchiveRepo { get; }
        IRepository<tbl_EventLogs> EventLogsRepo { get; }
        /// <summary>
        /// create villas available and demand (mostafa )
        /// </summary>
        IRepository<tbl_VillasAvailables> VillasAvailablesRepo { get; }
        IRepository<tbl_VillaAccessories> VillaAccessoriesRepo { get; }
        IRepository<tbl_VillasImages> VillasImagesRepo { get; }


        IRepository<tbl_VillasDemands> VillasDemandsRepo { get; }
        IRepository<tbl_VillaDemandAccessories> VillaDemandAccessoriesRepo { get; }
        IRepository<tbl_VillademandFinishings> VillademandFinishingsRepo { get; }
        IRepository<tbl_VillasDemandViews> VillasDemandViewsRepo { get; }


        IRepository<tbl_AvailableLands> AvailableLandsRepo { get; }
        IRepository<tbl_LandsDemands> LandsDemandsRepo { get; }
        IRepository<tbl_LandImages> LandImagesRepo { get; }
        IRepository<tbl_LandDemandViews> LandsDemandViewsRepo { get; }
        /// <summary>
        /// add Shop Demand Here Mostafa 
        /// </summary>
        IRepository<tbl_ShopDemands> ShopDemandsRepo { get; }
        IRepository<tbl_ShopDemandAccessories> ShopDemandAccessoriesRepo { get; }
        IRepository<tbl_ShopDemandViews> ShopDemandViewsRepo { get; }

        /// <summary>
        /// add shop available repos
        /// </summary>
        IRepository<tbl_ShopAvailable> ShopAvailableRepo { get; }
        IRepository<tbl_ShopAvailableImages> ShopAvailableImagesRepo { get; }
        IRepository<tbl_ShopAvailabeAccessories> ShopAvailableAccessoriesRepo { get; }     


        Task<int> SaveAsync();
    }
}
