namespace _3aqarak.DAL.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using _3aqarak.BLL.Models;
    using _3aqarak.BLL.Domain;

    public partial class RealEstateDB : DbContext
    {
        public RealEstateDB()
        {
        }
        public RealEstateDB(string conn)
            : base(conn)
        {
        }

        public virtual DbSet<AspNet_SqlCacheTablesForChangeNotification> AspNet_SqlCacheTablesForChangeNotification { get; set; }
        public virtual DbSet<tbl_Accessories> tbl_Accessories { get; set; }
        public virtual DbSet<tbl_AvailableLands> tbl_AvailableLands { get; set; }
        public virtual DbSet<tbl_AvailableUnits> tbl_AvailableUnits { get; set; }
        public virtual DbSet<tbl_Branches> tbl_Branches { get; set; }
        public virtual DbSet<tbl_Categories> tbl_Categories { get; set; }
        public virtual DbSet<tbl_Clients> tbl_Clients { get; set; }
        public virtual DbSet<tbl_ClientsCalls> tbl_ClientsCalls { get; set; }
        public virtual DbSet<tbl_FellowupCall> tbl_FellowupCalls { get; set; }
        public virtual DbSet<tbl_Collections> tbl_Collections { get; set; }
        public virtual DbSet<tbl_Commissions> tbl_Commissions { get; set; }
        public virtual DbSet<tbl_CompCommissions> tbl_CompCommissions { get; set; }
        public virtual DbSet<tbl_ContractArchives> tbl_ContractArchives { get; set; }
        public virtual DbSet<tbl_Demand_Finishings> tbl_Demand_Finishings { get; set; }
        public virtual DbSet<tbl_DemandAccessories> tbl_DemandAccessories { get; set; }
        public virtual DbSet<tbl_DemandUnits> tbl_DemandUnits { get; set; }
        public virtual DbSet<tbl_DemandViews> tbl_DemandViews { get; set; }
        public virtual DbSet<tbl_Departements> tbl_Departements { get; set; }
        public virtual DbSet<tbl_EmpCollections> tbl_EmpCollections { get; set; }
        public virtual DbSet<tbl_EmpCommissions> tbl_EmpCommissions { get; set; }
        public virtual DbSet<tbl_EventLogs> tbl_EventLogs { get; set; }
        public virtual DbSet<tbl_ExpectedContracts> tbl_ExpectedContracts { get; set; }
        public virtual DbSet<tbl_Expenses> tbl_Expenses { get; set; }
        public virtual DbSet<tbl_FinancialItems> tbl_FinancialItems { get; set; }
        public virtual DbSet<tbl_FinancialSummaries> tbl_FinancialSummaries { get; set; }
        public virtual DbSet<tbl_Finishings> tbl_Finishings { get; set; }
        public virtual DbSet<tbl_Genders> tbl_Genders { get; set; }
        public virtual DbSet<tbl_LandDemandViews> tbl_LandDemandViews { get; set; }
        public virtual DbSet<tbl_LandImages> tbl_LandImages { get; set; }
        public virtual DbSet<tbl_LandsDemands> tbl_LandsDemands { get; set; }
        public virtual DbSet<tbl_Messages> tbl_Messages { get; set; }
        public virtual DbSet<tbl_Offers> tbl_Offers { get; set; }
        public virtual DbSet<tbl_PaymentBasis> tbl_PaymentBasis { get; set; }
        public virtual DbSet<tbl_PaymentMethods> tbl_PaymentMethods { get; set; }
        public virtual DbSet<tbl_PostbonedCalls> tbl_PostbonedCalls { get; set; }
        public virtual DbSet<tbl_Posts> tbl_Posts { get; set; }
        public virtual DbSet<tbl_PreviewDetails> tbl_PreviewDetails { get; set; }
        public virtual DbSet<tbl_PreviewHeaders> tbl_PreviewHeaders { get; set; }
        public virtual DbSet<tbl_Regions> tbl_Regions { get; set; }
        public virtual DbSet<tbl_RentAgreementDetails> tbl_RentAgreementDetails { get; set; }
        public virtual DbSet<tbl_RentAgreementHeaders> tbl_RentAgreementHeaders { get; set; }
        public virtual DbSet<tbl_RentalArchives> tbl_RentalArchives { get; set; }
        public virtual DbSet<tbl_Revenues> tbl_Revenues { get; set; }
        public virtual DbSet<tbl_SaleAgreementDetails> tbl_SaleAgreementDetails { get; set; }
        public virtual DbSet<tbl_SaleAgreementHeaders> tbl_SaleAgreementHeaders { get; set; }
        public virtual DbSet<tbl_SalesArchives> tbl_SalesArchives { get; set; }
        public virtual DbSet<tbl_ShopAvailabeAccessories> tbl_ShopAvailabeAccessories { get; set; }
        public virtual DbSet<tbl_ShopAvailable> tbl_ShopAvailable { get; set; }
        public virtual DbSet<tbl_ShopAvailableImages> tbl_ShopAvailableImages { get; set; }
        public virtual DbSet<tbl_ShopDemandAccessories> tbl_ShopDemandAccessories { get; set; }
        public virtual DbSet<tbl_ShopDemands> tbl_ShopDemands { get; set; }
        public virtual DbSet<tbl_ShopDemandViews> tbl_ShopDemandViews { get; set; }
        public virtual DbSet<tbl_Specializations> tbl_Specializations { get; set; }
        public virtual DbSet<tbl_StaticContracts> tbl_StaticContracts { get; set; }
        public virtual DbSet<tbl_ToDoList> tbl_ToDoList { get; set; }
        public virtual DbSet<tbl_Transactions> tbl_Transactions { get; set; }
        public virtual DbSet<tbl_UnitAccessories> tbl_UnitAccessories { get; set; }
        public virtual DbSet<tbl_UnitImages> tbl_UnitImages { get; set; }
        public virtual DbSet<tbl_units> tbl_units { get; set; }
        public virtual DbSet<tbl_UnitUsage> tbl_UnitUsage { get; set; }
        public virtual DbSet<tbl_Users> tbl_Users { get; set; }
        public virtual DbSet<tbl_Views> tbl_Views { get; set; }
        public virtual DbSet<tbl_VillaAccessories> tbl_VillaAccessories { get; set; }
        public virtual DbSet<tbl_VillaDemandAccessories> tbl_VillaDemandAccessories { get; set; }
        public virtual DbSet<tbl_VillademandFinishings> tbl_VillademandFinishings { get; set; }
        public virtual DbSet<tbl_VillasAvailables> tbl_VillasAvailables { get; set; }
        public virtual DbSet<tbl_VillasDemands> tbl_VillasDemands { get; set; }
        public virtual DbSet<tbl_VillasDemandViews> tbl_VillasDemandViews { get; set; }
        public virtual DbSet<tbl_VillasImages> tbl_VillasImages { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_Accessories>()
                .HasMany(e => e.tbl_DemandAccessories)
                .WithRequired(e => e.tbl_Accessories)
                .HasForeignKey(e => e.FK_DemandAccessories_Accessories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Accessories>()
                .HasMany(e => e.tbl_ShopAvailabeAccessories)
                .WithRequired(e => e.tbl_Accessories)
                .HasForeignKey(e => e.FK_ShopAccessories_Accessories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Accessories>()
                .HasMany(e => e.tbl_ShopDemandAccessories)
                .WithRequired(e => e.tbl_Accessories)
                .HasForeignKey(e => e.FK_ShopDemandAccessories_Accessories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Accessories>()
                .HasMany(e => e.tbl_UnitAccessories)
                .WithRequired(e => e.tbl_Accessories)
                .HasForeignKey(e => e.FK_UnitAccessories_Accessories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Accessories>()
                .HasMany(e => e.tbl_VillaAccessories)
                .WithRequired(e => e.tbl_Accessories)
                .HasForeignKey(e => e.FK_VillasAccessories_Accessories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Accessories>()
                .HasMany(e => e.tbl_VillaDemandAccessories)
                .WithRequired(e => e.tbl_Accessories)
                .HasForeignKey(e => e.FK_VillaDemandAccessories_Accessories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_AvailableLands>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_AvailableLands>()
                .Property(e => e.Space)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_AvailableLands>()
                .HasMany(e => e.tbl_LandImages)
                .WithRequired(e => e.tbl_AvailableLands)
                .HasForeignKey(e => e.FK_LandImages_AvailableLands_AvailableLandId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_AvailableUnits>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_AvailableUnits>()
                .HasMany(e => e.tbl_Offers)
                .WithRequired(e => e.tbl_AvailableUnits)
                .HasForeignKey(e => e.FK_Offers_AvailableUnits_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Branches>()
                .HasMany(e => e.tbl_AvailableLands)
                .WithRequired(e => e.tbl_Branches)
                .HasForeignKey(e => e.FK_AvaliableLands_Branches_BranchId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Branches>()
                .HasMany(e => e.tbl_AvailableUnits)
                .WithRequired(e => e.tbl_Branches)
                .HasForeignKey(e => e.FK_AvaliableUnits_Branches_BranchId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Branches>()
                .HasMany(e => e.tbl_DemandUnits)
                .WithRequired(e => e.tbl_Branches)
                .HasForeignKey(e => e.FK_DemandUnits_Branches_BranchId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Branches>()
                .HasMany(e => e.tbl_LandsDemands)
                .WithRequired(e => e.tbl_Branches)
                .HasForeignKey(e => e.FK_LandsDemands_Branches_BranchId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Branches>()
                .HasMany(e => e.tbl_ShopAvailable)
                .WithRequired(e => e.tbl_Branches)
                .HasForeignKey(e => e.FK_ShopAvailable_Branches_BranchId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Branches>()
                .HasMany(e => e.tbl_ShopDemands)
                .WithRequired(e => e.tbl_Branches)
                .HasForeignKey(e => e.FK_ShopDemands_Branches_BranchId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Branches>()
                .HasMany(e => e.tbl_Users3)
                .WithRequired(e => e.tbl_Branches3)
                .HasForeignKey(e => e.FK_Users_Branches_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Branches>()
                .HasMany(e => e.tbl_VillasAvailables)
                .WithRequired(e => e.tbl_Branches)
                .HasForeignKey(e => e.FK_VillasAvailables_Branches_BranchId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Branches>()
                .HasMany(e => e.tbl_VillasDemands)
                .WithRequired(e => e.tbl_Branches)
                .HasForeignKey(e => e.FK_VillasDemands_Branches_BranchId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_AvailableLands)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.FK_AvailableLands_Categories_CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_ClientsCalls)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_Commissions)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.FK_Commissions_Categories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_ExpectedContracts)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.AvailableCat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_LandsDemands)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.FK_LandsDemands_Categories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_PostbonedCalls)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_Posts)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.FK_Posts_Categories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_RentAgreementHeaders)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.AvailableCat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_RentAgreementHeaders1)
                .WithRequired(e => e.tbl_Categories1)
                .HasForeignKey(e => e.DemandCat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_SaleAgreementHeaders)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.AvailableCat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_SaleAgreementHeaders1)
                .WithRequired(e => e.tbl_Categories1)
                .HasForeignKey(e => e.DemandCat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_ShopAvailable)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.FK_ShopAvailable_Categories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_ShopDemands)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.FK_ShopDemands_Categories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_StaticContracts)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.FK_StaticContract_Categories_CatId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_units)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.FK_Units_Categories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_VillasAvailables)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.FK_VillasAvailables_Categories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Categories>()
                .HasMany(e => e.tbl_VillasDemands)
                .WithRequired(e => e.tbl_Categories)
                .HasForeignKey(e => e.FK_VillasDemands_Categories_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_AvailableLands)
                .WithRequired(e => e.tbl_Clients)
                .HasForeignKey(e => e.FK_AvaliableLands_Clients_ClientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_ClientsCalls)
                .WithRequired(e => e.tbl_Clients)
                .HasForeignKey(e => e.FK_ClientCalls_Clients_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_FellowupCalls)
                .WithRequired(e => e.tbl_Clients)
                .HasForeignKey(e => e.FK_ClientCalls_Clients_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_DemandUnits)
                .WithRequired(e => e.tbl_Clients)
                .HasForeignKey(e => e.FK_DemandUnits_Clients_ClientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_LandsDemands)
                .WithRequired(e => e.tbl_Clients)
                .HasForeignKey(e => e.FK_LandsDemands_Clients_ClientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_PreviewDetails)
                .WithRequired(e => e.tbl_Clients)
                .HasForeignKey(e => e.Fk_PreviewDetails_Clients_SellerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_PreviewHeaders)
                .WithRequired(e => e.tbl_Clients)
                .HasForeignKey(e => e.FK_PreviewHeaders_Clients_BuyerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_RentAgreementHeaders)
                .WithRequired(e => e.tbl_Clients)
                .HasForeignKey(e => e.FK_RentAgreements_Clients_Buyer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_RentAgreementHeaders1)
                .WithRequired(e => e.tbl_Clients1)
                .HasForeignKey(e => e.FK_RentAgreements_Clients_Seller)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_ShopAvailable)
                .WithRequired(e => e.tbl_Clients)
                .HasForeignKey(e => e.FK_ShopAvailable_Clients_ClientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_ShopDemands)
                .WithRequired(e => e.tbl_Clients)
                .HasForeignKey(e => e.FK_ShopDemands_Clients_ClientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_VillasAvailables)
                .WithRequired(e => e.tbl_Clients)
                .HasForeignKey(e => e.FK_VillasAvailables_Clients_ClientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Clients>()
                .HasMany(e => e.tbl_VillasDemands)
                .WithRequired(e => e.tbl_Clients)
                .HasForeignKey(e => e.FK_VillasDemands_Clients_ClientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Collections>()
                .Property(e => e.EmployeeCommission)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_Collections>()
                .Property(e => e.CompanyCommession)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_Collections>()
                .HasMany(e => e.tbl_EmpCollections)
                .WithRequired(e => e.tbl_Collections)
                .HasForeignKey(e => e.FK_EmpCollect_Collections_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Commissions>()
                .Property(e => e.SalesComission)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_Commissions>()
                .Property(e => e.TelesalesComission)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_Commissions>()
                .Property(e => e.MgrCommission)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_CompCommissions>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_DemandUnits>()
                .Property(e => e.MinPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_DemandUnits>()
                .Property(e => e.MaxPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_DemandUnits>()
                .Property(e => e.MinSpace)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_DemandUnits>()
                .Property(e => e.MaxSpace)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_DemandUnits>()
                .HasMany(e => e.tbl_Demand_Finishings)
                .WithRequired(e => e.tbl_DemandUnits)
                .HasForeignKey(e => e.FK_DemandFinishing_Demand_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_DemandUnits>()
                .HasMany(e => e.tbl_DemandAccessories)
                .WithRequired(e => e.tbl_DemandUnits)
                .HasForeignKey(e => e.FK_DemandAccessories_Demand_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_DemandUnits>()
                .HasMany(e => e.tbl_DemandViews)
                .WithRequired(e => e.tbl_DemandUnits)
                .HasForeignKey(e => e.FK_DemandView_Demand_DemandId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Departements>()
                .HasMany(e => e.tbl_Specializations)
                .WithRequired(e => e.tbl_Departements)
                .HasForeignKey(e => e.FK_Specialization_Dept_DeptId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Departements>()
                .HasMany(e => e.tbl_Users2)
                .WithRequired(e => e.tbl_Departements2)
                .HasForeignKey(e => e.FK_Users_Departement_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_EmpCollections>()
                .Property(e => e.CommValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_EmpCommissions>()
                .Property(e => e.CommValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_ExpectedContracts>()
                .Property(e => e.Deposit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_Expenses>()
                .Property(e => e.Salaries)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_Expenses>()
                .Property(e => e.Commissions)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_Expenses>()
                .Property(e => e.Collections)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_Expenses>()
                .Property(e => e.Bonuses)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_Expenses>()
                .Property(e => e.Rentals)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_Expenses>()
                .Property(e => e.Others)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_FinancialItems>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_FinancialSummaries>()
                .Property(e => e.ExpensesSummary)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_FinancialSummaries>()
                .Property(e => e.IncomeSummary)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_FinancialSummaries>()
                .Property(e => e.ProfitSummary)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_Finishings>()
                .HasMany(e => e.tbl_Demand_Finishings)
                .WithRequired(e => e.tbl_Finishings)
                .HasForeignKey(e => e.FK_DemandFinishing_Finish_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Finishings>()
                .HasMany(e => e.tbl_VillademandFinishings)
                .WithRequired(e => e.tbl_Finishings)
                .HasForeignKey(e => e.FK_VillaDemandFinishing_Finish_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Finishings>()
                .HasMany(e => e.tbl_VillasAvailables)
                .WithRequired(e => e.tbl_Finishings)
                .HasForeignKey(e => e.FK_VillasAvailables_Finishings_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Genders>()
                .HasMany(e => e.tbl_Users)
                .WithRequired(e => e.tbl_Genders)
                .HasForeignKey(e => e.FK_Users_Genders_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Genders>()
                .HasMany(e => e.tbl_Users1)
                .WithRequired(e => e.tbl_Genders1)
                .HasForeignKey(e => e.FK_Users_Genders_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_LandImages>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_LandsDemands>()
                .Property(e => e.MinPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_LandsDemands>()
                .Property(e => e.MaxPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_LandsDemands>()
                .Property(e => e.MinSpace)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_LandsDemands>()
                .Property(e => e.MaxSpace)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_LandsDemands>()
                .HasMany(e => e.tbl_LandDemandViews)
                .WithRequired(e => e.tbl_LandsDemands)
                .HasForeignKey(e => e.FK_LandDemandView_LandDemand_DemandId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_PaymentMethods>()
                .HasMany(e => e.tbl_AvailableLands)
                .WithRequired(e => e.tbl_PaymentMethods)
                .HasForeignKey(e => e.FK_AvailableLands_PaymentMethod_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_PaymentMethods>()
                .HasMany(e => e.tbl_AvailableUnits)
                .WithRequired(e => e.tbl_PaymentMethods)
                .HasForeignKey(e => e.FK_AvailableUnits_PaymentMethod_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_PaymentMethods>()
                .HasMany(e => e.tbl_DemandUnits)
                .WithRequired(e => e.tbl_PaymentMethods)
                .HasForeignKey(e => e.FK_DemandUnits_PaymentMethod_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_PaymentMethods>()
                .HasMany(e => e.tbl_LandsDemands)
                .WithRequired(e => e.tbl_PaymentMethods)
                .HasForeignKey(e => e.FK_LandsDemands_PaymentMethod_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_PaymentMethods>()
                .HasMany(e => e.tbl_ShopAvailable)
                .WithRequired(e => e.tbl_PaymentMethods)
                .HasForeignKey(e => e.FK_ShopAvailable_PaymentMethod_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_PaymentMethods>()
                .HasMany(e => e.tbl_ShopDemands)
                .WithRequired(e => e.tbl_PaymentMethods)
                .HasForeignKey(e => e.FK_ShopDemands_PaymentMethod_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_PaymentMethods>()
                .HasMany(e => e.tbl_VillasAvailables)
                .WithRequired(e => e.tbl_PaymentMethods)
                .HasForeignKey(e => e.FK_VillasAvailables_PaymentMethod_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_PaymentMethods>()
                .HasMany(e => e.tbl_VillasDemands)
                .WithRequired(e => e.tbl_PaymentMethods)
                .HasForeignKey(e => e.FK_VillasDemands_PaymentMethod_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_PreviewHeaders>()
                .HasMany(e => e.tbl_ExpectedContracts)
                .WithRequired(e => e.tbl_PreviewHeaders)
                .HasForeignKey(e => e.FK_ExpectedContract_Preview_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_PreviewHeaders>()
                .HasMany(e => e.tbl_PreviewDetails)
                .WithRequired(e => e.tbl_PreviewHeaders)
                .HasForeignKey(e => e.Fk_PreviewDetails_PreviewHeaders_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Regions>()
                .HasMany(e => e.tbl_AvailableLands)
                .WithRequired(e => e.tbl_Regions)
                .HasForeignKey(e => e.FK_AvailabeLands_Regions_RegionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Regions>()
                .HasMany(e => e.tbl_DemandUnits)
                .WithRequired(e => e.tbl_Regions)
                .HasForeignKey(e => e.FK_DemandUnits_Regions_FromId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Regions>()
                .HasMany(e => e.tbl_DemandUnits1)
                .WithRequired(e => e.tbl_Regions1)
                .HasForeignKey(e => e.FK_DemandUnits_Regions_ToId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Regions>()
                .HasMany(e => e.tbl_LandsDemands)
                .WithRequired(e => e.tbl_Regions)
                .HasForeignKey(e => e.FK_LandsDemands_Regions_FromId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Regions>()
                .HasMany(e => e.tbl_LandsDemands1)
                .WithRequired(e => e.tbl_Regions1)
                .HasForeignKey(e => e.FK_LandsDemands_Regions_ToId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Regions>()
                .HasMany(e => e.tbl_ShopAvailable)
                .WithRequired(e => e.tbl_Regions)
                .HasForeignKey(e => e.FK_ShopAvailable_Regions_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Regions>()
                .HasMany(e => e.tbl_ShopDemands)
                .WithRequired(e => e.tbl_Regions)
                .HasForeignKey(e => e.FK_ShopDemands_Regions_FromId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Regions>()
                .HasMany(e => e.tbl_ShopDemands1)
                .WithRequired(e => e.tbl_Regions1)
                .HasForeignKey(e => e.FK_ShopDemands_Regions_ToId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Regions>()
                .HasMany(e => e.tbl_units)
                .WithRequired(e => e.tbl_Regions)
                .HasForeignKey(e => e.FK_Units_Regions_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Regions>()
                .HasMany(e => e.tbl_VillasAvailables)
                .WithRequired(e => e.tbl_Regions)
                .HasForeignKey(e => e.FK_VillasAvailables_Regions_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Regions>()
                .HasMany(e => e.tbl_VillasDemands)
                .WithRequired(e => e.tbl_Regions)
                .HasForeignKey(e => e.FK_VillasDemands_Regions_FromId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Regions>()
                .HasMany(e => e.tbl_VillasDemands1)
                .WithRequired(e => e.tbl_Regions1)
                .HasForeignKey(e => e.FK_VillasDemands_Regions_ToId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_RentAgreementHeaders>()
                .Property(e => e.ValueOfRental)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_RentAgreementHeaders>()
                .HasMany(e => e.tbl_RentAgreementDetails)
                .WithRequired(e => e.tbl_RentAgreementHeaders)
                .HasForeignKey(e => e.FK_RentHeaders_RentDetails_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_RentAgreementHeaders>()
                .HasMany(e => e.tbl_RentalArchives)
                .WithRequired(e => e.tbl_RentAgreementHeaders)
                .HasForeignKey(e => e.FK_RentalArchives_RentHeaders_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Revenues>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_SaleAgreementHeaders>()
                .Property(e => e.TotalAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_SaleAgreementHeaders>()
                .Property(e => e.DefaultInstallValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_SaleAgreementHeaders>()
                .Property(e => e.NextInstallValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_SaleAgreementHeaders>()
                .Property(e => e.ReservDeposit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_SaleAgreementHeaders>()
                .Property(e => e.PaidAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_SaleAgreementHeaders>()
                .Property(e => e.DueAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_SaleAgreementHeaders>()
                .HasMany(e => e.tbl_SaleAgreementDetails)
                .WithRequired(e => e.tbl_SaleAgreementHeaders)
                .HasForeignKey(e => e.FK_SaleDetails_SaleHeaders_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_ShopAvailable>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_ShopAvailable>()
                .Property(e => e.Space)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_ShopAvailable>()
                .HasMany(e => e.tbl_ShopAvailabeAccessories)
                .WithRequired(e => e.tbl_ShopAvailable)
                .HasForeignKey(e => e.FK_ShopAccessories_ShopAvailable_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_ShopAvailable>()
                .HasMany(e => e.tbl_ShopAvailableImages)
                .WithRequired(e => e.tbl_ShopAvailable)
                .HasForeignKey(e => e.FK_ShopAvailableImages_ShopAvailable_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_ShopDemands>()
                .Property(e => e.MinPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_ShopDemands>()
                .Property(e => e.MaxPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_ShopDemands>()
                .Property(e => e.MinSpace)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_ShopDemands>()
                .Property(e => e.MaxSpace)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_ShopDemands>()
                .HasMany(e => e.tbl_ShopDemandAccessories)
                .WithRequired(e => e.tbl_ShopDemands)
                .HasForeignKey(e => e.FK_ShopDemandAccessories_ShopDemand_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_ShopDemands>()
                .HasMany(e => e.tbl_ShopDemandViews)
                .WithRequired(e => e.tbl_ShopDemands)
                .HasForeignKey(e => e.FK_ShopDemandView_ShopDemand_DemandId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Transactions>()
                .HasMany(e => e.tbl_AvailableLands)
                .WithRequired(e => e.tbl_Transactions)
                .HasForeignKey(e => e.FK_AvaliableLands_Transactions_TransactionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Transactions>()
                .HasMany(e => e.tbl_Commissions)
                .WithRequired(e => e.tbl_Transactions)
                .HasForeignKey(e => e.FK_Commissions_Transactions_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Transactions>()
                .HasMany(e => e.tbl_DemandUnits)
                .WithRequired(e => e.tbl_Transactions)
                .HasForeignKey(e => e.FK_DemandUnits_Transactions_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Transactions>()
                .HasMany(e => e.tbl_LandsDemands)
                .WithRequired(e => e.tbl_Transactions)
                .HasForeignKey(e => e.FK_LandsDemands_Transactions_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Transactions>()
                .HasMany(e => e.tbl_ShopAvailable)
                .WithRequired(e => e.tbl_Transactions)
                .HasForeignKey(e => e.FK_ShopAvailable_Transactions_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Transactions>()
                .HasMany(e => e.tbl_ShopDemands)
                .WithRequired(e => e.tbl_Transactions)
                .HasForeignKey(e => e.FK_ShopDemands_Transactions_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Transactions>()
                .HasMany(e => e.tbl_StaticContracts)
                .WithRequired(e => e.tbl_Transactions)
                .HasForeignKey(e => e.FK_StaticContract_Transaction_Transid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Transactions>()
                .HasMany(e => e.tbl_VillasAvailables)
                .WithRequired(e => e.tbl_Transactions)
                .HasForeignKey(e => e.FK_VillasAvailables_Transactions_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Transactions>()
                .HasMany(e => e.tbl_VillasDemands)
                .WithRequired(e => e.tbl_Transactions)
                .HasForeignKey(e => e.FK_VillasDemands_Transactions_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_units>()
                .HasMany(e => e.tbl_AvailableUnits)
                .WithRequired(e => e.tbl_units)
                .HasForeignKey(e => e.FK_AvaliableUnits_Units_UnitId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_units>()
                .HasMany(e => e.tbl_UnitAccessories)
                .WithRequired(e => e.tbl_units)
                .HasForeignKey(e => e.FK_UnitAccessories_Units_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_units>()
                .HasMany(e => e.tbl_UnitImages)
                .WithRequired(e => e.tbl_units)
                .HasForeignKey(e => e.FK_UnitImages_Units_UnitId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_UnitUsage>()
                .HasMany(e => e.tbl_AvailableUnits)
                .WithRequired(e => e.tbl_UnitUsage)
                .HasForeignKey(e => e.FK_AvailableUnits_Usage_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_UnitUsage>()
                .HasMany(e => e.tbl_DemandUnits)
                .WithRequired(e => e.tbl_UnitUsage)
                .HasForeignKey(e => e.FK_DemandUnits_Usage_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_UnitUsage>()
                .HasMany(e => e.tbl_ShopAvailable)
                .WithRequired(e => e.tbl_UnitUsage)
                .HasForeignKey(e => e.FK_ShopAvailable_Usage_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_UnitUsage>()
                .HasMany(e => e.tbl_ShopDemands)
                .WithRequired(e => e.tbl_UnitUsage)
                .HasForeignKey(e => e.FK_ShopDemands_Usage_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_UnitUsage>()
                .HasMany(e => e.tbl_VillasAvailables)
                .WithRequired(e => e.tbl_UnitUsage)
                .HasForeignKey(e => e.FK_VillasAvailables_Usage_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_UnitUsage>()
                .HasMany(e => e.tbl_VillasDemands)
                .WithRequired(e => e.tbl_UnitUsage)
                .HasForeignKey(e => e.FK_VillasDemands_Usage_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Accessories)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Accessories_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Accessories1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Accessories_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_AvailableLands)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_AvaliableLands_Users_ModifiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_AvailableLands1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_AvaliableLands_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_AvailableLands2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_AvaliableLands_Users_SalesId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_AvailableUnits)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_AvaliableUnits_Users_SalesId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Branches)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Branches_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Branches1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Branches_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Branches2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_Branches_Users_MgrId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Categories)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Categories_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Categories1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Categories_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Clients)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Clients_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Clients1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Clients_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ClientsCalls)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_MarketingCalls_Users_EmpolyeeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ClientsCalls1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_MarketingCalls_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ClientsCalls2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_MarketingCalls_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
              .HasMany(e => e.tbl_FellowupCalls)
              .WithRequired(e => e.tbl_Users)
              .HasForeignKey(e => e.FK_FellowupCalls_Users_EmpolyeeId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
             .HasMany(e => e.tbl_FellowupCalls1)
             .WithRequired(e => e.tbl_Users1)
             .HasForeignKey(e => e.FK_FellowupCalls_Users_CreatedBy)
             .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
           .HasMany(e => e.tbl_FellowupCalls2)
           .WithRequired(e => e.tbl_Users2)
           .HasForeignKey(e => e.FK_FellowupCalls_Users_ModidfiedBy)
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Collections)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Collections_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Collections1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Collections_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Commissions)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Commissions_users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Commissions1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Commissions_users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_CompCommissions)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_CompComm_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_CompCommissions1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_CompComm_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ContractArchives)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_ContractArchives_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ContractArchives1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_ContractArchives_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Demand_Finishings)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_DemandFinishing_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Demand_Finishings1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_DemandFinishing_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_DemandAccessories)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_DemandAccessories_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_DemandAccessories1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_DemandAccessories_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_DemandUnits)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_DemandUnits_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_DemandUnits1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_DemandUnits_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_DemandUnits2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_DemandUnits_Users_Sales)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_DemandViews)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_DemandView_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_DemandViews1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_DemandView_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Departements)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Depts_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Departements1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Depts_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_EmpCollections)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_EmpCollect_EmpId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_EmpCollections1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_EmpCollect_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_EmpCollections2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_EmpCollect_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_EmpCommissions)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_EmpComm_Emp_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_EmpCommissions1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_EmpComm_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_EmpCommissions2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_EmpComm_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_EventLogs)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Event_Users_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ExpectedContracts)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_ExpectContracts_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ExpectedContracts1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_ExpectContracts_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Expenses)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Expenses_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Expenses1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Expenses_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_FinancialItems)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_FinancialItems_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_FinancialItems1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_FinancialItems_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Finishings)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Finishings_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Finishings1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Finishings_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_LandDemandViews)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_LandDemandView_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_LandDemandViews1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_LandDemandView_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_LandImages)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_LandImages_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_LandImages1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_LandImages_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_LandsDemands)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_LandsDemands_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_LandsDemands1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_LandsDemands_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_LandsDemands2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_LandsDemands_Users_SalesId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Messages)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Messages_Users_SenderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Messages1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Messages_Users_RecieverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Offers)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Offers_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Offers1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Offers_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_PaymentBasis)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_PaymentBasis_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_PaymentBasis1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_PaymentBasis_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_PaymentMethods)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_PaymentMethods_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_PaymentMethods1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_PaymentMethods_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_PostbonedCalls)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_PostbonedCalls_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_PostbonedCalls1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_PostbonedCalls_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_PostbonedCalls2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_MarketingCalls_Users_EmpolyeeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_PreviewDetails)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_PreviewDetails_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_PreviewHeaders)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_PreviewHeaders_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Regions)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Regions_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Regions1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Regions_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_RentAgreementDetails)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_RentDetails_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_RentAgreementDetails1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_RentDetails_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_RentAgreementHeaders)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_RentAgreements_Users_EmpId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_RentAgreementHeaders1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_RentalArchives)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_RentalArchives_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_RentalArchives1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_RentalArchives_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Revenues)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Revenues_Users_Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Revenues1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Revenues_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Revenues2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_Revenues_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_SaleAgreementDetails)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_SaleDetails_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_SaleAgreementDetails1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_SaleDetails_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_SaleAgreementHeaders)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_SalesHeaders_Users_EmpId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_SaleAgreementHeaders1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_SalesHeaders_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_SaleAgreementHeaders2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_SalesHeaders_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_SalesArchives)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_SalesArchives_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_SalesArchives1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_SalesArchives_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopAvailabeAccessories)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_ShopAccessories_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopAvailabeAccessories1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_ShopAccessories_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopAvailable)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_ShopAvailable_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopAvailable1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_ShopAvailable_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopAvailable2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_ShopAvailable_Users_SalesId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopAvailableImages)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_ShopAvailableImages_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopAvailableImages1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_ShopAvailableImages_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopDemandAccessories)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_ShopDemandAccessories_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopDemandAccessories1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_ShopDemandAccessories_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopDemands)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_ShopDemands_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopDemands1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_ShopDemands_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopDemands2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_ShopDemands_Users_SalesId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopDemandViews)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_ShopDemandView_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_ShopDemandViews1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_ShopDemandView_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Specializations)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Specialization_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Specializations1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Specialization_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_StaticContracts)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_StatContract_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_StaticContracts1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_StatContract_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Transactions)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Transactions_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Transactions1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Transactions_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_UnitAccessories)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_UnitAccessories_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_UnitAccessories1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_UnitAccessories_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_UnitImages)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_UnitImages_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_UnitImages1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_UnitImages_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Views)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_Views_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_Views1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_Views_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillaAccessories)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_VillasAccessories_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillaAccessories1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_VillasAccessories_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillaDemandAccessories)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_VillaDemandAccessories_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillaDemandAccessories1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_VillaDemandAccessories_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillademandFinishings)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_VillaDemandFinishing_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillademandFinishings1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_VillaDemandFinishing_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillasAvailables)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_VillasAvailables_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillasAvailables1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_VillasAvailables_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillasAvailables2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_VillasAvailables_Users_SalesId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillasDemands)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_VillasDemands_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillasDemands1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_VillasDemands_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillasDemands2)
                .WithRequired(e => e.tbl_Users2)
                .HasForeignKey(e => e.FK_VillasDemands_Users_SalesId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillasDemandViews)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_VillaDemandView_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillasDemandViews1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_VillaDemandView_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillasImages)
                .WithRequired(e => e.tbl_Users)
                .HasForeignKey(e => e.FK_UnitImages_Users_CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Users>()
                .HasMany(e => e.tbl_VillasImages1)
                .WithRequired(e => e.tbl_Users1)
                .HasForeignKey(e => e.FK_UnitImages_Users_ModidfiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Views>()
                .HasMany(e => e.tbl_AvailableLands)
                .WithRequired(e => e.tbl_Views)
                .HasForeignKey(e => e.FK_AvailableLands_Views_ViewId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Views>()
                .HasMany(e => e.tbl_DemandViews)
                .WithRequired(e => e.tbl_Views)
                .HasForeignKey(e => e.FK_DemandView_View_ViewId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Views>()
                .HasMany(e => e.tbl_LandDemandViews)
                .WithRequired(e => e.tbl_Views)
                .HasForeignKey(e => e.FK_LandDemandView_View_ViewId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Views>()
                .HasMany(e => e.tbl_ShopAvailable)
                .WithRequired(e => e.tbl_Views)
                .HasForeignKey(e => e.FK_ShopAvialable_Views_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Views>()
                .HasMany(e => e.tbl_ShopDemandViews)
                .WithRequired(e => e.tbl_Views)
                .HasForeignKey(e => e.FK_ShopDemandView_View_ViewId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Views>()
                .HasMany(e => e.tbl_units)
                .WithRequired(e => e.tbl_Views)
                .HasForeignKey(e => e.FK_Units_Views_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Views>()
                .HasMany(e => e.tbl_VillasAvailables)
                .WithRequired(e => e.tbl_Views)
                .HasForeignKey(e => e.FK_VillasAvailables_View_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_VillasAvailables>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_VillasAvailables>()
                .Property(e => e.Space)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_VillasAvailables>()
                .Property(e => e.AreaSpace)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_VillasAvailables>()
                .HasMany(e => e.tbl_VillaAccessories)
                .WithRequired(e => e.tbl_VillasAvailables)
                .HasForeignKey(e => e.FK_VillaAccessories_Villas_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_VillasAvailables>()
                .HasMany(e => e.tbl_VillasImages)
                .WithRequired(e => e.tbl_VillasAvailables)
                .HasForeignKey(e => e.FK_VillasImages_VillasAvailables_VillaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_VillasDemands>()
                .Property(e => e.MinPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_VillasDemands>()
                .Property(e => e.MaxPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_VillasDemands>()
                .Property(e => e.MinSpace)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_VillasDemands>()
                .Property(e => e.MaxSpace)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_VillasDemands>()
                .Property(e => e.MaxAreaSpace)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_VillasDemands>()
                .Property(e => e.MinAreaSpace)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tbl_VillasDemands>()
                .HasMany(e => e.tbl_VillaDemandAccessories)
                .WithRequired(e => e.tbl_VillasDemands)
                .HasForeignKey(e => e.FK_VillaDemandAccessories_VillaDemand_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_VillasDemands>()
                .HasMany(e => e.tbl_VillademandFinishings)
                .WithRequired(e => e.tbl_VillasDemands)
                .HasForeignKey(e => e.FK_VillaDemandFinishing_VillaDemand_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_VillasDemands>()
                .HasMany(e => e.tbl_VillasDemandViews)
                .WithRequired(e => e.tbl_VillasDemands)
                .HasForeignKey(e => e.FK_VillaDemandView_VillaDemand_DemandId)
                .WillCascadeOnDelete(false);


        }
    }
}
