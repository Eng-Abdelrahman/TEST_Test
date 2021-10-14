namespace _3aqarak.BLL.Models
{
    using _3aqarak.BLL.Domain;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class tbl_Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Users()
        {
            tbl_Accessories = new HashSet<tbl_Accessories>();
            tbl_Accessories1 = new HashSet<tbl_Accessories>();
            tbl_AvailableLands = new HashSet<tbl_AvailableLands>();
            tbl_AvailableLands1 = new HashSet<tbl_AvailableLands>();
            tbl_AvailableLands2 = new HashSet<tbl_AvailableLands>();
            tbl_AvailableUnits = new HashSet<tbl_AvailableUnits>();
            tbl_Branches = new HashSet<tbl_Branches>();
            tbl_Branches1 = new HashSet<tbl_Branches>();
            tbl_Branches2 = new HashSet<tbl_Branches>();
            tbl_Categories = new HashSet<tbl_Categories>();
            tbl_Categories1 = new HashSet<tbl_Categories>();
            tbl_Clients = new HashSet<tbl_Clients>();
            tbl_Clients1 = new HashSet<tbl_Clients>();
            tbl_ClientsCalls = new HashSet<tbl_ClientsCalls>();
            tbl_ClientsCalls1 = new HashSet<tbl_ClientsCalls>();
            tbl_ClientsCalls2 = new HashSet<tbl_ClientsCalls>();
            tbl_Collections = new HashSet<tbl_Collections>();
            tbl_Collections1 = new HashSet<tbl_Collections>();
            tbl_Commissions = new HashSet<tbl_Commissions>();
            tbl_Commissions1 = new HashSet<tbl_Commissions>();
            tbl_CompCommissions = new HashSet<tbl_CompCommissions>();
            tbl_CompCommissions1 = new HashSet<tbl_CompCommissions>();
            tbl_ContractArchives = new HashSet<tbl_ContractArchives>();
            tbl_ContractArchives1 = new HashSet<tbl_ContractArchives>();
            tbl_Demand_Finishings = new HashSet<tbl_Demand_Finishings>();
            tbl_Demand_Finishings1 = new HashSet<tbl_Demand_Finishings>();
            tbl_DemandAccessories = new HashSet<tbl_DemandAccessories>();
            tbl_DemandAccessories1 = new HashSet<tbl_DemandAccessories>();
            tbl_DemandUnits = new HashSet<tbl_DemandUnits>();
            tbl_DemandUnits1 = new HashSet<tbl_DemandUnits>();
            tbl_DemandUnits2 = new HashSet<tbl_DemandUnits>();
            tbl_DemandViews = new HashSet<tbl_DemandViews>();
            tbl_DemandViews1 = new HashSet<tbl_DemandViews>();
            tbl_Departements = new HashSet<tbl_Departements>();
            tbl_Departements1 = new HashSet<tbl_Departements>();
            tbl_EmpCollections = new HashSet<tbl_EmpCollections>();
            tbl_EmpCollections1 = new HashSet<tbl_EmpCollections>();
            tbl_EmpCollections2 = new HashSet<tbl_EmpCollections>();
            tbl_EmpCommissions = new HashSet<tbl_EmpCommissions>();
            tbl_EmpCommissions1 = new HashSet<tbl_EmpCommissions>();
            tbl_EmpCommissions2 = new HashSet<tbl_EmpCommissions>();
            tbl_EventLogs = new HashSet<tbl_EventLogs>();
            tbl_ExpectedContracts = new HashSet<tbl_ExpectedContracts>();
            tbl_ExpectedContracts1 = new HashSet<tbl_ExpectedContracts>();
            tbl_Expenses = new HashSet<tbl_Expenses>();
            tbl_Expenses1 = new HashSet<tbl_Expenses>();
            tbl_FinancialItems = new HashSet<tbl_FinancialItems>();
            tbl_FinancialItems1 = new HashSet<tbl_FinancialItems>();
            tbl_Finishings = new HashSet<tbl_Finishings>();
            tbl_Finishings1 = new HashSet<tbl_Finishings>();
            tbl_LandDemandViews = new HashSet<tbl_LandDemandViews>();
            tbl_LandDemandViews1 = new HashSet<tbl_LandDemandViews>();
            tbl_LandImages = new HashSet<tbl_LandImages>();
            tbl_LandImages1 = new HashSet<tbl_LandImages>();
            tbl_LandsDemands = new HashSet<tbl_LandsDemands>();
            tbl_LandsDemands1 = new HashSet<tbl_LandsDemands>();
            tbl_LandsDemands2 = new HashSet<tbl_LandsDemands>();
            tbl_Messages = new HashSet<tbl_Messages>();
            tbl_Messages1 = new HashSet<tbl_Messages>();
            tbl_Offers = new HashSet<tbl_Offers>();
            tbl_Offers1 = new HashSet<tbl_Offers>();
            tbl_PaymentBasis = new HashSet<tbl_PaymentBasis>();
            tbl_PaymentBasis1 = new HashSet<tbl_PaymentBasis>();
            tbl_PaymentMethods = new HashSet<tbl_PaymentMethods>();
            tbl_PaymentMethods1 = new HashSet<tbl_PaymentMethods>();
            tbl_PostbonedCalls = new HashSet<tbl_PostbonedCalls>();
            tbl_PostbonedCalls1 = new HashSet<tbl_PostbonedCalls>();
            tbl_PostbonedCalls2 = new HashSet<tbl_PostbonedCalls>();
            tbl_PreviewDetails = new HashSet<tbl_PreviewDetails>();
            tbl_PreviewHeaders = new HashSet<tbl_PreviewHeaders>();
            tbl_Regions = new HashSet<tbl_Regions>();
            tbl_Regions1 = new HashSet<tbl_Regions>();
            tbl_RentAgreementDetails = new HashSet<tbl_RentAgreementDetails>();
            tbl_RentAgreementDetails1 = new HashSet<tbl_RentAgreementDetails>();
            tbl_RentAgreementHeaders = new HashSet<tbl_RentAgreementHeaders>();
            tbl_RentAgreementHeaders1 = new HashSet<tbl_RentAgreementHeaders>();
            tbl_RentalArchives = new HashSet<tbl_RentalArchives>();
            tbl_RentalArchives1 = new HashSet<tbl_RentalArchives>();
            tbl_Revenues = new HashSet<tbl_Revenues>();
            tbl_Revenues1 = new HashSet<tbl_Revenues>();
            tbl_Revenues2 = new HashSet<tbl_Revenues>();
            tbl_SaleAgreementDetails = new HashSet<tbl_SaleAgreementDetails>();
            tbl_SaleAgreementDetails1 = new HashSet<tbl_SaleAgreementDetails>();
            tbl_SaleAgreementHeaders = new HashSet<tbl_SaleAgreementHeaders>();
            tbl_SaleAgreementHeaders1 = new HashSet<tbl_SaleAgreementHeaders>();
            tbl_SaleAgreementHeaders2 = new HashSet<tbl_SaleAgreementHeaders>();
            tbl_SalesArchives = new HashSet<tbl_SalesArchives>();
            tbl_SalesArchives1 = new HashSet<tbl_SalesArchives>();
            tbl_ShopAvailabeAccessories = new HashSet<tbl_ShopAvailabeAccessories>();
            tbl_ShopAvailabeAccessories1 = new HashSet<tbl_ShopAvailabeAccessories>();
            tbl_ShopAvailable = new HashSet<tbl_ShopAvailable>();
            tbl_ShopAvailable1 = new HashSet<tbl_ShopAvailable>();
            tbl_ShopAvailable2 = new HashSet<tbl_ShopAvailable>();
            tbl_ShopAvailableImages = new HashSet<tbl_ShopAvailableImages>();
            tbl_ShopAvailableImages1 = new HashSet<tbl_ShopAvailableImages>();
            tbl_ShopDemandAccessories = new HashSet<tbl_ShopDemandAccessories>();
            tbl_ShopDemandAccessories1 = new HashSet<tbl_ShopDemandAccessories>();
            tbl_ShopDemands = new HashSet<tbl_ShopDemands>();
            tbl_ShopDemands1 = new HashSet<tbl_ShopDemands>();
            tbl_ShopDemands2 = new HashSet<tbl_ShopDemands>();
            tbl_ShopDemandViews = new HashSet<tbl_ShopDemandViews>();
            tbl_ShopDemandViews1 = new HashSet<tbl_ShopDemandViews>();
            tbl_Specializations = new HashSet<tbl_Specializations>();
            tbl_Specializations1 = new HashSet<tbl_Specializations>();
            tbl_StaticContracts = new HashSet<tbl_StaticContracts>();
            tbl_StaticContracts1 = new HashSet<tbl_StaticContracts>();
            tbl_Transactions = new HashSet<tbl_Transactions>();
            tbl_Transactions1 = new HashSet<tbl_Transactions>();
            tbl_UnitAccessories = new HashSet<tbl_UnitAccessories>();
            tbl_UnitAccessories1 = new HashSet<tbl_UnitAccessories>();
            tbl_UnitImages = new HashSet<tbl_UnitImages>();
            tbl_UnitImages1 = new HashSet<tbl_UnitImages>();
            tbl_Views = new HashSet<tbl_Views>();
            tbl_Views1 = new HashSet<tbl_Views>();
            tbl_VillaAccessories = new HashSet<tbl_VillaAccessories>();
            tbl_VillaAccessories1 = new HashSet<tbl_VillaAccessories>();
            tbl_VillaDemandAccessories = new HashSet<tbl_VillaDemandAccessories>();
            tbl_VillaDemandAccessories1 = new HashSet<tbl_VillaDemandAccessories>();
            tbl_VillademandFinishings = new HashSet<tbl_VillademandFinishings>();
            tbl_VillademandFinishings1 = new HashSet<tbl_VillademandFinishings>();
            tbl_VillasAvailables = new HashSet<tbl_VillasAvailables>();
            tbl_VillasAvailables1 = new HashSet<tbl_VillasAvailables>();
            tbl_VillasAvailables2 = new HashSet<tbl_VillasAvailables>();
            tbl_VillasDemands = new HashSet<tbl_VillasDemands>();
            tbl_VillasDemands1 = new HashSet<tbl_VillasDemands>();
            tbl_VillasDemands2 = new HashSet<tbl_VillasDemands>();
            tbl_VillasDemandViews = new HashSet<tbl_VillasDemandViews>();
            tbl_VillasDemandViews1 = new HashSet<tbl_VillasDemandViews>();
            tbl_VillasImages = new HashSet<tbl_VillasImages>();
            tbl_VillasImages1 = new HashSet<tbl_VillasImages>();
            tbl_FellowupCalls = new HashSet<tbl_FellowupCall>();
            tbl_FellowupCalls1 = new HashSet<tbl_FellowupCall>();
            tbl_FellowupCalls2 = new HashSet<tbl_FellowupCall>();


        }

        [Key]
        public int PK_Users_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Salt { get; set; }

        public int CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public int ModifiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedAt { get; set; }

        public bool IsActive { get; set; }

        public bool IsLocked { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LockEnd { get; set; }

        public bool IsAdmin { get; set; }

        public int InvalidAttempts { get; set; }

        [StringLength(64)]
        public string RememberToken { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(12)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int FK_Users_Genders_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateOfBirth { get; set; }

        public string PhotoUrl { get; set; }

        public int FK_Users_Departement_Id { get; set; }

        public int FK_Users_Branches_Id { get; set; }

        public bool IsOwner { get; set; }

        public int? Specialization_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Accessories> tbl_Accessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Accessories> tbl_Accessories1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AvailableLands> tbl_AvailableLands { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AvailableLands> tbl_AvailableLands1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AvailableLands> tbl_AvailableLands2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AvailableUnits> tbl_AvailableUnits { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Branches> tbl_Branches { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Branches> tbl_Branches1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Branches> tbl_Branches2 { get; set; }

        public virtual tbl_Branches tbl_Branches3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Categories> tbl_Categories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Categories> tbl_Categories1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Clients> tbl_Clients { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Clients> tbl_Clients1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ClientsCalls> tbl_ClientsCalls { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ClientsCalls> tbl_ClientsCalls1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ClientsCalls> tbl_ClientsCalls2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_FellowupCall> tbl_FellowupCalls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_FellowupCall> tbl_FellowupCalls1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_FellowupCall> tbl_FellowupCalls2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Collections> tbl_Collections { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Collections> tbl_Collections1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Commissions> tbl_Commissions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Commissions> tbl_Commissions1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_CompCommissions> tbl_CompCommissions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_CompCommissions> tbl_CompCommissions1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ContractArchives> tbl_ContractArchives { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ContractArchives> tbl_ContractArchives1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Demand_Finishings> tbl_Demand_Finishings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Demand_Finishings> tbl_Demand_Finishings1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandAccessories> tbl_DemandAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandAccessories> tbl_DemandAccessories1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandUnits> tbl_DemandUnits { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandUnits> tbl_DemandUnits1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandUnits> tbl_DemandUnits2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandViews> tbl_DemandViews { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DemandViews> tbl_DemandViews1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Departements> tbl_Departements { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Departements> tbl_Departements1 { get; set; }

        public virtual tbl_Departements tbl_Departements2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EmpCollections> tbl_EmpCollections { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EmpCollections> tbl_EmpCollections1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EmpCollections> tbl_EmpCollections2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EmpCommissions> tbl_EmpCommissions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EmpCommissions> tbl_EmpCommissions1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EmpCommissions> tbl_EmpCommissions2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EventLogs> tbl_EventLogs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ExpectedContracts> tbl_ExpectedContracts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ExpectedContracts> tbl_ExpectedContracts1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Expenses> tbl_Expenses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Expenses> tbl_Expenses1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_FinancialItems> tbl_FinancialItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_FinancialItems> tbl_FinancialItems1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Finishings> tbl_Finishings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Finishings> tbl_Finishings1 { get; set; }

        public virtual tbl_Genders tbl_Genders { get; set; }

        public virtual tbl_Genders tbl_Genders1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LandDemandViews> tbl_LandDemandViews { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LandDemandViews> tbl_LandDemandViews1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LandImages> tbl_LandImages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LandImages> tbl_LandImages1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LandsDemands> tbl_LandsDemands { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LandsDemands> tbl_LandsDemands1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LandsDemands> tbl_LandsDemands2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Messages> tbl_Messages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Messages> tbl_Messages1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Offers> tbl_Offers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Offers> tbl_Offers1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PaymentBasis> tbl_PaymentBasis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PaymentBasis> tbl_PaymentBasis1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PaymentMethods> tbl_PaymentMethods { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PaymentMethods> tbl_PaymentMethods1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PostbonedCalls> tbl_PostbonedCalls { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PostbonedCalls> tbl_PostbonedCalls1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PostbonedCalls> tbl_PostbonedCalls2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PreviewDetails> tbl_PreviewDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PreviewHeaders> tbl_PreviewHeaders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Regions> tbl_Regions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Regions> tbl_Regions1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_RentAgreementDetails> tbl_RentAgreementDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_RentAgreementDetails> tbl_RentAgreementDetails1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_RentAgreementHeaders> tbl_RentAgreementHeaders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_RentAgreementHeaders> tbl_RentAgreementHeaders1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_RentalArchives> tbl_RentalArchives { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_RentalArchives> tbl_RentalArchives1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Revenues> tbl_Revenues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Revenues> tbl_Revenues1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Revenues> tbl_Revenues2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SaleAgreementDetails> tbl_SaleAgreementDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SaleAgreementDetails> tbl_SaleAgreementDetails1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SaleAgreementHeaders> tbl_SaleAgreementHeaders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SaleAgreementHeaders> tbl_SaleAgreementHeaders1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SaleAgreementHeaders> tbl_SaleAgreementHeaders2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SalesArchives> tbl_SalesArchives { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SalesArchives> tbl_SalesArchives1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailabeAccessories> tbl_ShopAvailabeAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailabeAccessories> tbl_ShopAvailabeAccessories1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailable> tbl_ShopAvailable { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailable> tbl_ShopAvailable1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailable> tbl_ShopAvailable2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailableImages> tbl_ShopAvailableImages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopAvailableImages> tbl_ShopAvailableImages1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemandAccessories> tbl_ShopDemandAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemandAccessories> tbl_ShopDemandAccessories1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemands> tbl_ShopDemands { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemands> tbl_ShopDemands1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemands> tbl_ShopDemands2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemandViews> tbl_ShopDemandViews { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ShopDemandViews> tbl_ShopDemandViews1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Specializations> tbl_Specializations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Specializations> tbl_Specializations1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_StaticContracts> tbl_StaticContracts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_StaticContracts> tbl_StaticContracts1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Transactions> tbl_Transactions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Transactions> tbl_Transactions1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_UnitAccessories> tbl_UnitAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_UnitAccessories> tbl_UnitAccessories1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_UnitImages> tbl_UnitImages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_UnitImages> tbl_UnitImages1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Views> tbl_Views { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Views> tbl_Views1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillaAccessories> tbl_VillaAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillaAccessories> tbl_VillaAccessories1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillaDemandAccessories> tbl_VillaDemandAccessories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillaDemandAccessories> tbl_VillaDemandAccessories1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillademandFinishings> tbl_VillademandFinishings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillademandFinishings> tbl_VillademandFinishings1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasAvailables> tbl_VillasAvailables { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasAvailables> tbl_VillasAvailables1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasAvailables> tbl_VillasAvailables2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasDemands> tbl_VillasDemands { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasDemands> tbl_VillasDemands1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasDemands> tbl_VillasDemands2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasDemandViews> tbl_VillasDemandViews { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasDemandViews> tbl_VillasDemandViews1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasImages> tbl_VillasImages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_VillasImages> tbl_VillasImages1 { get; set; }
    }
}
