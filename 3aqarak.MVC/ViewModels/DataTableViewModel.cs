using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class DataTableViewModel
    {
        public string Draw { get; set; }

        public string Start { get; set; }

        public string Length { get; set; }

        public string SortColumn { get; set; }

        public string SortColumnDir { get; set; }

        public string SearchValue { get; set; }

        public int PageSize { get; set; }

        public int Skip { get; set; }

        public int RecordsTotal { get; set; }    
        
        public List<ClientsViewModel> Clients { get; set; }

        public List<AvailableViewModel> ClientSales { get; set; }

        public List<DemandViewModel> ClientDemands { get; set; }

        public List<ExpectedContractViewModel> Expectedcontracts { get; set; }

        public List<AgreementContractViewModel> AgreementContracts { get; set; }

        public List<RentContractViewModel> RentContracts { get; set; }

        public List<SaleContractViewModel> SaleContracts { get; set; }

        public List<FinancialItemsViewModel> FinancialItems { get; set; }

        public List<ContractCommissionsViewModel> Contracts { get; set; }

        public List<AdvertisingVM> advertisingVMs { get; set; }

        public List<AvailableViewModel> AvailableUnit { get; set; }

        public List<DemandGetDataViewModel> DemandGetDataVM { set; get; }

        public List<VillsAvailableViewModel> VillasAvalable { set; get; }

        public List<AvailableLandsViewModel> AvailableLands { get; set; }

        public List<AvailableLandsViewModel> LandsAvalable { set; get; }

        public List<VillaDemandViewModel> VillasClientDemand { set; get; }

        public List<ShopDemandViewModel> ShopDemand { set; get; }

        public List<VillaClientDemandViewModel> VillasDemand { set; get; }

        public List<LandsDemandsViewModel> LandsDemand { set; get; }

        public List<ShopAvailableViewModel> ShopAvailable { set; get; }

        public List<PostsViewModel> Posts { set; get; }

        public List<MessageViewModel> Message { set; get; }

    }
}