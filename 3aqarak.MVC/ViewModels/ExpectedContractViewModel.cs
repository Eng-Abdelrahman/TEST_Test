using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class ExpectedContractViewModel
    {

        public int PK_ExpectContracts_Id { get; set; }

        public int FK_ExpectContract_Clients_Seller { get; set; }

        public int FK_ExpectContract_Clients_Buyer { get; set; }

        public int AvailableUnits_Id { get; set; }
        //add one here
        public int CategoryId { get; set; }

        public DateTime ExpectDateTime { get; set; }

        public string DateTimeString { get; set; }

        public string BuyerName { get; set; }

        public string SellerName { get; set; }

        public bool IsDone { get; set; }

        public string BuyerAddress { get; set; }

        public string BuyerNationalId { get; set; }

        public string SellerAddress { get; set; }

        public string SellerNationalId { get; set; }

        public bool IsPostponed { get; set; }

        public bool IsCancelled { get; set; }

        public decimal? Deposit { get; set; }

        public string Notes { get; set; }

        public DateTime? PostponeDateTime { get; set; }

        public int FK_ExpectedContract_Preview_Id { get; set; }
        public SelectList Cats { get; set; }

        public bool IsContractAgreement { get; set; }

        public string ContractType { set; get; }
    }
}