
using _3aqarak.BLL.Domain;
using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Models;
using _3aqarak.MVC.Areas.Dashboard.ViewModels;
using _3aqarak.MVC.Helpers;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using System;
using System.Web.UI;

namespace _3aqarak.MVC.App_Start
{
    public class MVCAutoMapperConfig : Profile
    {
        public MVCAutoMapperConfig()
        {
            CreateMap<UserDto, UserViewModel>().ForMember(dest => dest.FullName,
               opts => opts.MapFrom(
                   src => string.Format("{0} {1}",
                       src.FirstName,
                       src.LastName)));
            CreateMap<UserViewModel, UserDto>();
            CreateMap<UserDto, tbl_Users>().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<tbl_Users, UserDto>().ForSourceMember(x => x.Password, opt => opt.DoNotValidate()).ForSourceMember(x => x.Salt, opt => opt.DoNotValidate());

            CreateMap<UserDto, ProfileViewModel>().ForMember(dest => dest.FullName,
             opts => opts.MapFrom(
                 src => string.Format("{0} {1}",
                     src.FirstName,
                     src.LastName)));
            CreateMap<ProfileViewModel, UserDto>();

            CreateMap<tbl_Genders, GenderDto>();
            CreateMap<GenderDto, tbl_Genders>();
            CreateMap<GenderDto, GenderViewModel>();
            CreateMap<GenderViewModel, GenderDto>();

            CreateMap<tbl_Clients, ClientDto>();
            CreateMap<tbl_Clients, ClientDto>().ReverseMap().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<ClientDto, ClientViewModel>();
            CreateMap<ClientDto, ClientViewModel>().ReverseMap();



            CreateMap<ClientDto, ClientsViewModel>();
            CreateMap<ClientDto, ClientsViewModel>().ReverseMap();

            //new here***********************************************************************************
            CreateMap<tbl_ContractArchives, ContractArchiveDto>();
            CreateMap<tbl_ContractArchives, ContractArchiveDto>().ReverseMap().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<ContractArchiveDto, ContractArchiveViewModel>();
            CreateMap<ContractArchiveDto, ContractArchiveViewModel>().ReverseMap();

            //New one for EvenyLog ***************************************************************************
            CreateMap<tbl_EventLogs, EventLogDto>();
            CreateMap<tbl_EventLogs, EventLogDto>().ReverseMap();
            CreateMap<EventLogDto, EventLogViewModel>();
            CreateMap<EventLogDto, EventLogViewModel>().ReverseMap();

            //Add VillsAvailable Here **************>>>>Start<<<<<<********************************************************
            CreateMap<tbl_VillasAvailables, VillasAvailableDto>();
            CreateMap<tbl_VillasAvailables, VillasAvailableDto>().ReverseMap();
            CreateMap<VillasAvailableDto, VillsAvailableViewModel>();
            CreateMap<VillasAvailableDto, VillsAvailableViewModel>().ReverseMap();

            CreateMap<MixedVillasAvailableDto, MixedVillasAvailableViewModel>();
            CreateMap<MixedVillasAvailableDto, MixedVillasAvailableViewModel>().ReverseMap();

            CreateMap<MixedVillasAvailableDto, ClientDto>();
            CreateMap<MixedVillasAvailableDto, ClientDto>().ReverseMap();

            CreateMap<MixedVillasAvailableDto, VillasAvailableDto>().ForMember(dest => dest.Notes, opts => opts.MapFrom(src => src.VillNotes));
            CreateMap<MixedVillasAvailableDto, VillasAvailableDto>().ReverseMap().ForMember(dest => dest.VillNotes, opts => opts.MapFrom(src => src.Notes));


            //**********************************>>>>>>>END<<<<<<<<********************************************************

            //*** Hello welcome to my part this's ShopDemand Part****************************************************
            CreateMap<tbl_ShopDemands, ShopDemandDto>();
            CreateMap<tbl_ShopDemands, ShopDemandDto>().ReverseMap();
            CreateMap<ShopDemandDto, ShopDemandViewModel>();
            CreateMap<ShopDemandDto, ShopDemandViewModel>().ReverseMap();

            CreateMap<ShopDemandDto, ClientDto>().ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.ClientAddress)).ForMember(dest => dest.Notes, opts => opts.MapFrom(src => src.ClientNotes));
            CreateMap<ShopDemandDto, ClientDto>().ReverseMap().ForMember(dest => dest.ClientAddress, opts => opts.MapFrom(src => src.Address)).ForMember(dest => dest.ClientNotes, opts => opts.MapFrom(src => src.Notes));

            CreateMap<tbl_ShopDemands, ShopDemandDto>();
            CreateMap<tbl_ShopDemands, ShopDemandDto>().ReverseMap();

            CreateMap<ShopDemandViewModel, ShopDemandDto>();
            CreateMap<ShopDemandViewModel, ShopDemandDto>().ReverseMap();

            CreateMap<ShopDemandDto, ShopDemandForUpdateViewModel>();
            CreateMap<ShopDemandDto, ShopDemandForUpdateViewModel>().ReverseMap();


            CreateMap<tbl_units, UnitDto>();
            CreateMap<tbl_units, UnitDto>().ReverseMap().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<UnitDto, UnitViewModel>();
            CreateMap<UnitDto, UnitViewModel>().ReverseMap();

            CreateMap<tbl_Accessories, AccessDto>();
            CreateMap<tbl_Accessories, AccessDto>().ReverseMap().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<AccessDto, AccessViewModel>();
            CreateMap<AccessDto, AccessViewModel>().ReverseMap();

            CreateMap<tbl_Finishings, FinishingDto>();
            CreateMap<tbl_Finishings, FinishingDto>().ReverseMap().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<FinishingDto, FinishViewModel>();
            CreateMap<FinishingDto, FinishViewModel>().ReverseMap();

            CreateMap<tbl_Regions, RegionDto>();
            CreateMap<tbl_Regions, RegionDto>().ReverseMap().ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<RegionDto, RegionViewModel>();
            CreateMap<RegionDto, RegionViewModel>().ReverseMap();

            CreateMap<tbl_PaymentBasis, BasisDto>();
            CreateMap<tbl_PaymentBasis, BasisDto>().ReverseMap().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<BasisDto, BasisViewModel>();
            CreateMap<BasisDto, BasisViewModel>().ReverseMap();

            CreateMap<tbl_Categories, CatDto>();
            CreateMap<tbl_Categories, CatDto>().ReverseMap().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<CatDto, CatViewModel>();
            CreateMap<CatDto, CatViewModel>().ReverseMap();

            CreateMap<tbl_Transactions, TransDto>();
            CreateMap<tbl_Transactions, TransDto>().ReverseMap().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<TransDto, TransactionViewModel>();
            CreateMap<TransDto, TransactionViewModel>().ReverseMap();

            CreateMap<tbl_StaticContracts, StaticDto>();
            CreateMap<tbl_StaticContracts, StaticDto>().ReverseMap().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<StaticDto, StaticViewModel>();
            CreateMap<StaticDto, StaticViewModel>().ReverseMap();

            CreateMap<tbl_AvailableUnits, AvailableDto>().ForMember(dest => dest.DateString, opts => opts.MapFrom(src => src.CreatedAt.ToShortDateString()));
            CreateMap<tbl_AvailableUnits, AvailableDto>().ReverseMap().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<AvailableDto, AvailableViewModel>();
            CreateMap<AvailableDto, AvailableViewModel>().ReverseMap();


            CreateMap<tbl_DemandUnits, DemandDto>().ForMember(dest => dest.NoElevatorsFrom, opts => opts.MapFrom(src => src.NoOfElevatorsFrom)).ForMember(dest => dest.NoElevatorsTo, opts => opts.MapFrom(src => src.NoOfElevatorsTo));
            CreateMap<tbl_DemandUnits, DemandDto>().ReverseMap().ForMember(dest => dest.NoOfElevatorsFrom, opts => opts.MapFrom(src => src.NoElevatorsFrom)).ForMember(dest => dest.NoOfElevatorsTo, opts => opts.MapFrom(src => src.NoElevatorsTo));
            CreateMap<DemandDto, DemandViewModel>();
            CreateMap<DemandDto, DemandViewModel>().ReverseMap();

            CreateMap<tbl_Views, ViewDto>();
            CreateMap<tbl_Views, ViewDto>().ReverseMap().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<ViewDto, ViewsViewModel>();
            CreateMap<ViewDto, ViewsViewModel>().ReverseMap();

            CreateMap<tbl_PaymentMethods, PaymentDto>();
            CreateMap<tbl_PaymentMethods, PaymentDto>().ReverseMap().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));


            CreateMap<PreviewHeaderDto, tbl_PreviewHeaders>().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<PreviewHeaderDto, tbl_PreviewHeaders>().ReverseMap();
            CreateMap<PreviewHeaderDto, PreviewHeaderViewModel>();
            CreateMap<PreviewHeaderDto, PreviewHeaderViewModel>().ReverseMap();


            CreateMap<PreviewDetailsDto, tbl_PreviewDetails>().ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<PreviewDetailsDto, tbl_PreviewDetails>().ReverseMap();
            CreateMap<PreviewDetailsDto, PreviewDetailViewModel>();
            CreateMap<PreviewDetailsDto, PreviewDetailViewModel>().ReverseMap();

            CreateMap<PreviewScreenDto, PreviewScreenViewModel>();
            CreateMap<PreviewScreenDto, PreviewScreenViewModel>().ReverseMap();


            CreateMap<BranchDto, tbl_Branches>().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<BranchDto, tbl_Branches>().ReverseMap();
            CreateMap<BranchDto, BranchViewModel>();
            CreateMap<BranchDto, BranchViewModel>().ReverseMap();


            CreateMap<DeptDto, tbl_Departements>().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<DeptDto, tbl_Departements>().ReverseMap();
            CreateMap<DeptDto, DeptViewModel>();
            CreateMap<DeptDto, DeptViewModel>().ReverseMap();

            CreateMap<SpecialDto, tbl_Specializations>().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<SpecialDto, tbl_Specializations>().ReverseMap();
            CreateMap<SpecialDto, SpecialViewModel>();
            CreateMap<SpecialDto, SpecialViewModel>().ReverseMap();

            CreateMap<ClientCallDto, tbl_ClientsCalls>().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<ClientCallDto, tbl_ClientsCalls>().ReverseMap();
            CreateMap<ClientCallDto, ClientCallViewModel>();
            CreateMap<ClientCallDto, ClientCallViewModel>().ReverseMap();

            CreateMap<PostponedCallDto, tbl_PostbonedCalls>().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<PostponedCallDto, tbl_PostbonedCalls>().ReverseMap();
            CreateMap<PostponedCallDto, ClientCallViewModel>().ForMember(dest => dest.Clients_Id, opts => opts.MapFrom(src => src.FK_PostponedCalls_Clients_ClientId));
            CreateMap<PostponedCallDto, ClientCallViewModel>().ReverseMap();

            CreateMap<ExpectedContractDto, tbl_ExpectedContracts>().ForMember(dest => dest.AvailableCat, opts => opts.MapFrom(src => src.CategoryId));
            CreateMap<ExpectedContractDto, tbl_ExpectedContracts>().ReverseMap().ForMember(dest => dest.CategoryId, opts => opts.MapFrom(src => src.AvailableCat));
            CreateMap<ExpectedContractDto, ExpectedContractViewModel>();
            CreateMap<ExpectedContractDto, ExpectedContractViewModel>().ReverseMap();


            CreateMap<RentHeaderDto, tbl_RentAgreementHeaders>().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<RentHeaderDto, tbl_RentAgreementHeaders>().ReverseMap();
            CreateMap<RentHeaderDto, RentContractViewModel>();
            CreateMap<RentHeaderDto, RentContractViewModel>().ReverseMap();

            CreateMap<RentDetailsDto, tbl_RentAgreementDetails>().ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<RentDetailsDto, tbl_RentAgreementDetails>().ReverseMap();
            CreateMap<RentDetailsDto, RentContractViewModel>();
            CreateMap<RentDetailsDto, RentContractViewModel>().ReverseMap();

            CreateMap<SaleHeaderDto, tbl_SaleAgreementHeaders>().ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<SaleHeaderDto, tbl_SaleAgreementHeaders>().ReverseMap();
            CreateMap<SaleHeaderDto, SaleContractViewModel>();
            CreateMap<SaleHeaderDto, SaleContractViewModel>().ReverseMap();

            CreateMap<SaleDetailsDto, tbl_SaleAgreementDetails>().ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<SaleDetailsDto, tbl_SaleAgreementDetails>().ReverseMap();
            CreateMap<SaleDetailsDto, SaleContractViewModel>();
            CreateMap<SaleDetailsDto, SaleContractViewModel>().ReverseMap();

            CreateMap<FinancialItemsDto, tbl_FinancialItems>().ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<FinancialItemsDto, tbl_FinancialItems>().ReverseMap();
            CreateMap<FinancialItemsDto, FinancialItemsViewModel>();
            CreateMap<FinancialItemsDto, FinancialItemsViewModel>().ReverseMap();

            CreateMap<CommissionsDto, tbl_Commissions>().ForMember(dest => dest.ModifiedAt, opts => opts.MapFrom(src => DateTime.Now)).ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => DateTime.Now));
            CreateMap<CommissionsDto, tbl_Commissions>().ReverseMap();
            CreateMap<CommissionsDto, CommissionsViewModel>();
            CreateMap<CommissionsDto, CommissionsViewModel>().ReverseMap();

            CreateMap<ContractCommissionsDto, ContractCommissionsViewModel>();
            CreateMap<ContractCommissionsDto, ContractCommissionsViewModel>().ReverseMap();

            CreateMap<CommPctgsDto, CommissionsPercts>();
            CreateMap<CommPctgsDto, CommissionsPercts>().ReverseMap();

            CreateMap<ProfitDto, tbl_FinancialSummaries>();
            CreateMap<ProfitDto, tbl_FinancialSummaries>().ReverseMap();
            CreateMap<ProfitDto, ProfitViewModel>();
            CreateMap<ProfitDto, ProfitViewModel>().ReverseMap();

            CreateMap<NotificationDto, NotificationViewModel>();
            CreateMap<NotificationDto, NotificationViewModel>().ReverseMap();

            CreateMap<PostponedCallDto, PostbonedCallViewModel>().ForMember(dest => dest.Clients_Id, opts => opts.MapFrom(src => src.FK_PostponedCalls_Clients_ClientId)).ForMember(dest => dest.Notes, opts => opts.MapFrom(src => src.Nots));
            CreateMap<PostbonedCallViewModel, ClientCallDto>();

            CreateMap<MixDemandClientDto, MixDemandClientViewModel>();
            CreateMap<MixDemandClientDto, MixDemandClientViewModel>().ReverseMap();

            CreateMap<MixDemandClientDto, ClientDto>();
            CreateMap<MixDemandClientDto, ClientDto>().ReverseMap();

            CreateMap<MixDemandClientDto, DemandDto>().ForMember(dest => dest.Notes, opts => opts.MapFrom(src => src.DemandNotes));
            CreateMap<MixDemandClientDto, DemandDto>().ReverseMap().ForMember(dest => dest.DemandNotes, opts => opts.MapFrom(src => src.Notes));

            // Demand Import mapping*****************************************************************
            CreateMap<ImportDemandViewModel, MixDemandClientViewModel>();
            CreateMap<ImportDemandViewModel, MixDemandClientViewModel>().ReverseMap();
            CreateMap<ImportDemandViewModel, DemandDto>();
            CreateMap<ImportDemandViewModel, DemandDto>().ReverseMap();
            CreateMap<ImportDemandViewModel, ClientDto>();
            CreateMap<ImportDemandViewModel, ClientDto>().ReverseMap();

            // Demand Import Villa mapping*****************************************************************
            CreateMap<ImportVillaDemandViewModel, VillaClientDemandViewModel>();
            CreateMap<ImportVillaDemandViewModel, VillaClientDemandViewModel>().ReverseMap();
            CreateMap<ImportVillaDemandViewModel, VillasDemandDto>();
            CreateMap<ImportVillaDemandViewModel, VillasDemandDto>().ReverseMap();
            CreateMap<ImportVillaDemandViewModel, ClientDto>();
            CreateMap<ImportVillaDemandViewModel, ClientDto>().ReverseMap();

            //Client Import Mapping 
            CreateMap<ImportCientViewModel, ClientDto>();
            CreateMap<ImportCientViewModel, ClientDto>().ReverseMap();


            // ******************************************************************************>>>>>>>>>>>>>>>>>>> Start Import Available mapping <<<<<<<<<<<<<<<*************************

            //  Import Available mapping                         
            CreateMap<ImportAvailableViewModel, AvailableDto>().ReverseMap();                                       
            CreateMap<ImportAvailableViewModel, ClientDto>().ReverseMap();
            CreateMap<tbl_units , AvailableDto> ()
 
                .ForMember(dest => dest.FK_AvaliableUnits_Regions_Id, opts => opts.MapFrom(src => src.FK_Units_Regions_Id))
                .ForMember(dest => dest.FK_AvailableUnits_Categories_Id , opts => opts.MapFrom(src => src.FK_Units_Categories_Id)).ReverseMap();
          
            //  Import Available Villa mapping                                                                      
            CreateMap<ImportVillaAvailableViewModel, ImportVillasAvailableDto>().ReverseMap();
            
            CreateMap<tbl_VillasAvailables, ImportVillasAvailableDto>()
                 .ForMember(dest => dest.FK_Units_Finishing_Id, opts => opts.MapFrom(src => src.FK_VillasAvailables_Finishings_Id))
                 .ForMember(dest => dest.FK_AvailableUnits_PaymentMethod_Id, opts => opts.MapFrom(src => src.FK_VillasAvailables_PaymentMethod_Id))
                 .ForMember(dest => dest.FK_AvailableUnits_Transactions_Id, opts => opts.MapFrom(src => src.FK_VillasAvailables_Transactions_Id))
                 .ForMember(dest => dest.FK_AvailableUnits_Usage_Id, opts => opts.MapFrom(src => src.FK_VillasAvailables_Usage_Id))
                 .ForMember(dest => dest.FK_Units_Views_Id, opts => opts.MapFrom(src => src.FK_VillasAvailables_View_Id))
                 .ForMember(dest => dest.FK_AvaliableUnits_Regions_Id, opts => opts.MapFrom(src => src.FK_VillasAvailables_Regions_Id))
                 .ReverseMap();

            CreateMap<ImportVillaAvailableViewModel, ClientDto>().ReverseMap();

            //// ******************************************************************************>>>>>>>>>>>>>>>>>>> End Import Available mapping <<<<<<<<<<<<<<<****************************

            CreateMap<MixedAvailableClientDto, MixedAvailableClientViewModel>();
            CreateMap<MixedAvailableClientDto, MixedAvailableClientViewModel>().ReverseMap();

            CreateMap<MixedAvailableClientDto, ClientDto>();
            CreateMap<MixedAvailableClientDto, ClientDto>().ReverseMap();

            CreateMap<MixedAvailableClientDto, AvailableDto>().ForMember(dest => dest.Notes, opts => opts.MapFrom(src => src.AvailableNotes));
            CreateMap<MixedAvailableClientDto, AvailableDto>().ReverseMap().ForMember(dest => dest.AvailableNotes, opts => opts.MapFrom(src => src.Notes));


            CreateMap<tbl_UnitUsage, UnitUsageDto>();
            CreateMap<tbl_UnitUsage, UnitUsageDto>().ReverseMap();

            /////////////////////////////////////////////////////
            CreateMap<tbl_AvailableLands, AvailableLandsDto>();
            CreateMap<tbl_AvailableLands, AvailableLandsDto>().ReverseMap();

            CreateMap<AvailableLandsDto, AvailableLandsViewModel>();
            CreateMap<AvailableLandsDto, AvailableLandsViewModel>().ReverseMap();

            CreateMap<AvailableLandsDto, LandAvailableForUpdateViewModel>();
            CreateMap<AvailableLandsDto, LandAvailableForUpdateViewModel>().ReverseMap();


            CreateMap<tbl_VillasDemands, VillasDemandDto>();
            CreateMap<tbl_VillasDemands, VillasDemandDto>().ReverseMap();
            CreateMap<VillasDemandDto, VillaClientDemandViewModel>();
            CreateMap<VillasDemandDto, VillaClientDemandViewModel>().ReverseMap();
            CreateMap<VillasDemandDto, VillaDemandViewModel>();
            CreateMap<VillasDemandDto, VillaDemandViewModel>().ReverseMap();

            CreateMap<VillasDemandDto, ClientDto>().ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.ClientAddress)).ForMember(dest => dest.Notes, opts => opts.MapFrom(src => src.ClientNotes));
            CreateMap<VillasDemandDto, ClientDto>().ReverseMap().ForMember(dest => dest.ClientAddress, opts => opts.MapFrom(src => src.Address)).ForMember(dest => dest.ClientNotes, opts => opts.MapFrom(src => src.Notes));



            CreateMap<tbl_LandsDemands, LandsDemandsDto>();
            CreateMap<tbl_LandsDemands, LandsDemandsDto>().ReverseMap();

            CreateMap<LandsDemandsDto, LandsDemandsViewModel>();
            CreateMap<LandsDemandsDto, LandsDemandsViewModel>().ReverseMap();

            CreateMap<AvailableLandsDto, ClientDto>();
            CreateMap<AvailableLandsDto, ClientDto>().ReverseMap();

            CreateMap<LandsDemandsDto, ClientDto>();
            CreateMap<LandsDemandsDto, ClientDto>().ReverseMap();

            CreateMap<LandsDemandsViewModel, ClientDto>();
            CreateMap<LandsDemandsViewModel, ClientDto>().ReverseMap();
            //add LandDemandForUpdateViewModel
            CreateMap<LandsDemandsDto, LandDemandForUpdateViewModel>();
            CreateMap<LandsDemandsDto, LandDemandForUpdateViewModel>().ReverseMap();


            CreateMap<tbl_ShopAvailable, ShopAvailableDto>();
            CreateMap<tbl_ShopAvailable, ShopAvailableDto>().ReverseMap();

            CreateMap<ShopAvailableViewModel, ShopAvailableDto>();
            CreateMap<ShopAvailableViewModel, ShopAvailableDto>().ReverseMap();

            CreateMap<ShopAvailableViewModel, ClientDto>();
            CreateMap<ShopAvailableViewModel, ClientDto>().ReverseMap();



            CreateMap<ShopAvailableDto, ClientDto>();
            CreateMap<ShopAvailableDto, ClientDto>().ReverseMap();

            //mapping for Apartment 
            CreateMap<tbl_DemandUnits, DemandCentralDto>().ForMember(dest => dest.DemandId, opts => opts.MapFrom(src => src.PK_DemandUnits_Id)).
                ForMember(dest => dest.BranchId, opts => opts.MapFrom(src => src.FK_DemandUnits_Branches_BranchId)).
                ForMember(dest => dest.CreatedBy, opts => opts.MapFrom(src => src.FK_DemandUnits_Users_CreatedBy)).
                ForMember(dest => dest.SalesId, opts => opts.MapFrom(src => src.FK_DemandUnits_Users_Sales)).
                ForMember(dest => dest.CategoryId, opts => opts.MapFrom(src => src.FK_DemandUnits_Categories_Id)).
                ForMember(dest => dest.TransId, opts => opts.MapFrom(src => src.FK_DemandUnits_Transactions_Id));

            //mapping for Villas Demnads
            CreateMap<tbl_VillasDemands, DemandCentralDto>().ForMember(dest => dest.DemandId, opts => opts.MapFrom(src => src.PK_VillasDemands_Id)).
                ForMember(dest => dest.BranchId, opts => opts.MapFrom(src => src.FK_VillasDemands_Branches_BranchId)).
                ForMember(dest => dest.CreatedBy, opts => opts.MapFrom(src => src.FK_VillasDemands_Users_CreatedBy)).
                ForMember(dest => dest.SalesId, opts => opts.MapFrom(src => src.FK_VillasDemands_Users_SalesId)).
                ForMember(dest => dest.CategoryId, opts => opts.MapFrom(src => src.FK_VillasDemands_Categories_Id)).
                ForMember(dest => dest.TransId, opts => opts.MapFrom(src => src.FK_VillasDemands_Transactions_Id));


            //mapping for Lands Demnads
            CreateMap<tbl_LandsDemands, DemandCentralDto>().ForMember(dest => dest.DemandId, opts => opts.MapFrom(src => src.PK_LandsDemands_Id)).
                ForMember(dest => dest.BranchId, opts => opts.MapFrom(src => src.FK_LandsDemands_Branches_BranchId)).
                ForMember(dest => dest.CreatedBy, opts => opts.MapFrom(src => src.FK_LandsDemands_Users_CreatedBy)).
                ForMember(dest => dest.SalesId, opts => opts.MapFrom(src => src.FK_LandsDemands_Users_SalesId)).
                ForMember(dest => dest.CategoryId, opts => opts.MapFrom(src => src.FK_LandsDemands_Categories_Id)).
                ForMember(dest => dest.TransId, opts => opts.MapFrom(src => src.FK_LandsDemands_Transactions_Id));


            //mapping for Shop Demnads
            CreateMap<tbl_ShopDemands, DemandCentralDto>().ForMember(dest => dest.DemandId, opts => opts.MapFrom(src => src.PK_ShopDemands_Id)).
                ForMember(dest => dest.BranchId, opts => opts.MapFrom(src => src.FK_ShopDemands_Branches_BranchId)).
                ForMember(dest => dest.CreatedBy, opts => opts.MapFrom(src => src.FK_ShopDemands_Users_CreatedBy)).
                ForMember(dest => dest.SalesId, opts => opts.MapFrom(src => src.FK_ShopDemands_Users_SalesId)).
                ForMember(dest => dest.CategoryId, opts => opts.MapFrom(src => src.FK_ShopDemands_Categories_Id)).
                ForMember(dest => dest.TransId, opts => opts.MapFrom(src => src.FK_ShopDemands_Transactions_Id));

            CreateMap<PostsDto, tbl_Posts>();
            CreateMap<PostsDto, tbl_Posts>().ReverseMap();
            CreateMap<PostsDto, PostsViewModel>();
            CreateMap<PostsDto, PostsViewModel>().ReverseMap();
            CreateMap<PostsViewModel, ClientDto>();
            CreateMap<PostsViewModel, ClientDto>().ReverseMap();

            //Mapper for Message Table
            CreateMap<MessagesDto, tbl_Messages>();
            CreateMap<MessagesDto, tbl_Messages>().ReverseMap();
            CreateMap<MessagesDto, MessageViewModel>();
            CreateMap<MessagesDto, MessageViewModel>().ReverseMap();

            //Mapper for fellowup calls
            CreateMap<FellowCallDto, tbl_FellowupCall>().ReverseMap();
            CreateMap<FellowupCallViewModel, FellowCallDto>().ReverseMap();
        }
    }
}