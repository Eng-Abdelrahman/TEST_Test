
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Services;
using _3aqarak.MVC.Interfaces;
using _3aqarak.MVC.NotificationCacheClasses;
using _3aqarak.MVC.ViewModels;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.DI
{
    public class AutofacMVCContainer : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUSerService>();
            builder.RegisterType<ClientService>().As<IClientService>();
            builder.RegisterType<AccessService>().As<IAccessService>();
            builder.RegisterType<FinishService>().As<IFinishService>();
            builder.RegisterType<RegionService>().As<IRegionService>();
            builder.RegisterType<BasisService>().As<IBasisService>();
            builder.RegisterType<TransService>().As<ITransService>();
            builder.RegisterType<CatService>().As<ICatService>();
            builder.RegisterType<StaticService>().As<IStaticService>();
            builder.RegisterType<AvailableService>().As<IAvailableService>();
            builder.RegisterType<DemandService>().As<IDemandService>();
            builder.RegisterType<ViewService>().As<IViewService>();
            builder.RegisterType<PreviewService>().As<IPreviewService>();
            builder.RegisterType<BranchService>().As<IBranchService>();
            builder.RegisterType<DeptService>().As<IDeptService>();
            builder.RegisterType<SpecialService>().As<ISpecialService>();
            builder.RegisterType<CallsService>().As<ICallService>();
            builder.RegisterType<FellowupCallService>().As<IFellowCallService>();
            builder.RegisterType<ExpectedContractService>().As<IexpectedContractService>();
            builder.RegisterType<RentContractService>().As<IRentContractService>();
            builder.RegisterType<SaleContractService>().As<ISaleContractService>();
            builder.RegisterType<FinancialService>().As<IFinancialService>();
            builder.RegisterType<CommissionService>().As<ICommissionsService>();
            builder.RegisterType<AccountingService>().As<IAccountingService>();
            builder.RegisterType<ProfitService>().As<IProfitService>();
            builder.RegisterType<NotificationService>().As<INotificationService>();
            builder.RegisterType<NotificationCacheClass>().As<INotificationCacheClasses>();
            //Add new line here 
            builder.RegisterType<ContractArchiveService>().As<IContractArchiveService>();
            builder.RegisterType<ReportService>().As<IReportServices>();
            //Add also new one for EventLog *********************************************
            builder.RegisterType<EventLogService>().As<IEventLogService>();

            builder.RegisterType<LandsAvailableService>().As<IAvailableLandsSevice>();
            // inject VillasAvailablesService here **************************************
            builder.RegisterType<VillasAvailableService>().As<IVillasAvailablesService>();

            builder.RegisterType<LandsDemandsService>().As<ILandsDemandsService>();
            builder.RegisterType<VillaDemandService>().As<IVillasDemandService>();

            // Hello Welcome again in My ShopDemand part this's inject part *****************
            builder.RegisterType<ShopDemandService>().As<IShopDemandService>();
            builder.RegisterType<ShopAvailableAccessoriesService>().As<IShopAvailableAccessoriesService>();
            builder.RegisterType<ShopAvailableService>().As<IShopAvailableService>();
            builder.RegisterType<ReportsViewModel>().As<IReportsViewModel>();
            builder.RegisterType<PostService>().As<IpostsService>();

            //Add Message here
            builder.RegisterType<MessageService>().As<IMessageService>();

            //Add Import here
           // builder.RegisterType<ImportService>().As<IMessageService>();
        }

    }
}