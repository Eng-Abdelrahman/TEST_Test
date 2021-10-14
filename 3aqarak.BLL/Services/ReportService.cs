using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Services
{
    public class ReportService : IReportServices
    {
        private readonly IUnitOfWork _uow;
        private static List<int> _monthlyRentContracts = new List<int>();
        private static List<int> _monthlySaleContracts = new List<int>();      

        public ReportService(IUnitOfWork uow)
        {
            _uow = uow;          

        }

        string GetCatName(int cat)
        {
            string CatName = "";
            if (cat == Categories.Apartements)
            {
                CatName = Categories.ApartementName;
            }
            if (cat == Categories.Villas)
            {
                CatName = Categories.VillaName;
            }
            if (cat == Categories.Lands)
            {
                CatName = Categories.LandName;
            }
            if (cat == Categories.Shops)
            {
                CatName = Categories.ShopName;
            }
            return CatName;
        }
        public async Task<(IEnumerable<string>, IEnumerable<int>,IEnumerable<EmpGroupByCategory>)> rentalEmpMonthlyAcheivement(int month)
        {
            List<BLL.Models.tbl_RentAgreementHeaders> monthContracts =(await _uow.RentHeaderRepo.FindAsync(h => !h.IsDeleted && h.Date.Month == month)).ToList();
            List<EmployeeAndCat> EmpAndCat = new List<EmployeeAndCat>();
            foreach (BLL.Models.tbl_RentAgreementHeaders contract in monthContracts)
            {
                //Apartment Demand
                if (contract.DemandCat == Categories.Apartements)
                {
                    EmpAndCat.Add((from de in await _uow.DemandRepo.FindAsync(a => a.PK_DemandUnits_Id == contract.DemandUnits_Id)
                                   join emp in await _uow.UserRepo.GetAllAsync()
                                   on de.FK_DemandUnits_Users_CreatedBy equals emp.PK_Users_Id
                                  // where !de.IsDeleted && emp.IsActive
                                   select new EmployeeAndCat() { Employee = string.Concat(emp.FirstName, " ", emp.LastName), Cat = Categories.Apartements }).FirstOrDefault());
                }
                //Villas Demand
                else if (contract.DemandCat == Categories.Villas)
                {
                    EmpAndCat.Add((from de in await  _uow.VillasDemandsRepo.FindAsync(a => a.PK_VillasDemands_Id == contract.DemandUnits_Id)
                                   join emp in await _uow.UserRepo.GetAllAsync()
                                   on de.FK_VillasDemands_Users_CreatedBy equals emp.PK_Users_Id
                                   //where !de.IsDeleted && emp.IsActive
                                   select new EmployeeAndCat() { Employee = string.Concat(emp.FirstName, " ", emp.LastName), Cat = Categories.Villas }).FirstOrDefault());
                }
                //land Demand
                else if (contract.DemandCat == Categories.Lands)
                {
                    EmpAndCat.Add((from de in await _uow.LandsDemandsRepo.FindAsync(a => a.PK_LandsDemands_Id == contract.DemandUnits_Id)
                                   join emp in await _uow.UserRepo.GetAllAsync()
                                   on de.FK_LandsDemands_Users_CreatedBy equals emp.PK_Users_Id
                                   //where !de.IsDeleted && emp.IsActive
                                   select new EmployeeAndCat() { Employee = string.Concat(emp.FirstName, " ", emp.LastName), Cat = Categories.Lands }).FirstOrDefault());
                }
                //Shop Demand
                else if (contract.DemandCat == Categories.Shops)
                {
                    EmpAndCat.Add((from de in await _uow.ShopDemandsRepo.FindAsync(a => a.PK_ShopDemands_Id == contract.DemandUnits_Id)
                                   join emp in await _uow.UserRepo.GetAllAsync()
                                   on de.FK_ShopDemands_Users_CreatedBy equals emp.PK_Users_Id
                                   //where !de.IsDeleted && emp.IsActive
                                   select new EmployeeAndCat() { Employee = string.Concat(emp.FirstName, " ", emp.LastName), Cat = Categories.Shops }).FirstOrDefault());
                }

            }
            List<string> destinctEmployees = EmpAndCat.Select(n => n.Employee).Distinct().ToList();
            List<int> employeeContractCount = new List<int>();
            
            foreach (string emp in destinctEmployees)
            {
                employeeContractCount.Add(EmpAndCat.Count(e => e.Employee.Equals(emp, StringComparison.OrdinalIgnoreCase)));
            }
           
            var EmpGroupsByCat = EmpAndCat.GroupBy(x => new { x.Employee, x.Cat }).OrderBy(g => g.Key.Employee).ThenBy(g => g.Key.Cat).Select(g => new EmpGroupByCategory { EmpName = g.Key.Employee, Category = GetCatName(g.Key.Cat), Count = g.Count() });


            return (destinctEmployees, employeeContractCount,EmpGroupsByCat);
        }

        public async Task<(IEnumerable<string>, IEnumerable<int>, IEnumerable<EmpGroupByCategory>)> sellEmpMonthlyAcheivement(int month)
        {
            List<BLL.Models.tbl_SaleAgreementHeaders> monthContracts =(await _uow.SaleHeaderRepo.FindAsync(h => !h.IsDeleted && h.Date.Month == month)).ToList();
            List<EmployeeAndCat> EmpAndCat = new List<EmployeeAndCat>();
            foreach (BLL.Models.tbl_SaleAgreementHeaders contract in monthContracts)
            {
                //Apartment Demand
                if (contract.DemandCat == Categories.Apartements)
                {
                    var query = from de in await _uow.DemandRepo.FindAsync(d=>d.PK_DemandUnits_Id == contract.DemandUnits_Id)
                                 join emp in await _uow.UserRepo.GetAllAsync()
                                 on de.FK_DemandUnits_Users_CreatedBy equals emp.PK_Users_Id
                                 //where !de.IsDeleted && emp.IsActive 
                                 select new EmployeeAndCat() { Employee = string.Concat(emp.FirstName, " ", emp.LastName), Cat = Categories.Apartements };
                    EmpAndCat.Add(query.FirstOrDefault());
                }
                //Villas Demand
                else if (contract.DemandCat == Categories.Villas)
                {
                    EmpAndCat.Add((from de in await _uow.VillasDemandsRepo.FindAsync(a => a.PK_VillasDemands_Id == contract.DemandUnits_Id)
                                   join emp in await _uow.UserRepo.GetAllAsync()
                                   on de.FK_VillasDemands_Users_CreatedBy equals emp.PK_Users_Id
                                   //where !de.IsDeleted && emp.IsActive
                                   select new EmployeeAndCat() { Employee = string.Concat(emp.FirstName, " ", emp.LastName), Cat = Categories.Villas }).FirstOrDefault());
                }
                //land Demand
                else if (contract.DemandCat == Categories.Lands)
                {
                    EmpAndCat.Add((from de in await _uow.LandsDemandsRepo.FindAsync(a => a.PK_LandsDemands_Id == contract.DemandUnits_Id)
                                   join emp in await _uow.UserRepo.GetAllAsync()
                                   on de.FK_LandsDemands_Users_CreatedBy equals emp.PK_Users_Id
                                   //where !de.IsDeleted && emp.IsActive
                                   select new EmployeeAndCat() { Employee = string.Concat(emp.FirstName, " ", emp.LastName), Cat = Categories.Lands }).FirstOrDefault());
                }
                //Shop Demand
                else if (contract.DemandCat == Categories.Shops)
                {
                    EmpAndCat.Add((from de in await _uow.ShopDemandsRepo.FindAsync(a => a.PK_ShopDemands_Id == contract.DemandUnits_Id)
                                   join emp in await _uow.UserRepo.GetAllAsync()
                                   on de.FK_ShopDemands_Users_CreatedBy equals emp.PK_Users_Id
                                   //where !de.IsDeleted && emp.IsActive
                                   select new EmployeeAndCat() { Employee = string.Concat(emp.FirstName, " ", emp.LastName), Cat = Categories.Shops }).FirstOrDefault());
                }

            }
            List<string> destinctEmployees = EmpAndCat.Select(n => n.Employee).Distinct().ToList();
            List<int> employeeContractCount = new List<int>();

            foreach (string emp in destinctEmployees)
            {
                employeeContractCount.Add(EmpAndCat.Count(e => e.Employee.Equals(emp, StringComparison.OrdinalIgnoreCase)));
            }
            var EmpGroupsByCat = EmpAndCat.GroupBy(x => new { x.Employee, x.Cat }).OrderBy(g => g.Key.Employee).ThenBy(g => g.Key.Cat).Select(g => new EmpGroupByCategory { EmpName = g.Key.Employee, Category = GetCatName(g.Key.Cat), Count = g.Count() });


            return (destinctEmployees, employeeContractCount, EmpGroupsByCat);
        }

       

        public async Task<IEnumerable<int>> EmpSaleContracts(int userId) {
            int year = DateTime.Now.Year;
            var TotalAcheivemnt = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                var counter = 0;
                List<BLL.Models.tbl_SaleAgreementHeaders> monthContracts =(await _uow.SaleHeaderRepo.FindAsync(h => !h.IsDeleted && h.Date.Year == year && h.Date.Month == i)).ToList();
                foreach (BLL.Models.tbl_SaleAgreementHeaders contract in monthContracts)
                {

                    //Apartment Demand
                    if (contract.DemandCat == Categories.Apartements)
                    {
                        var demandEmpId =(await _uow.DemandRepo.FindAsync(a => a.PK_DemandUnits_Id == contract.DemandUnits_Id)).FirstOrDefault().FK_DemandUnits_Users_CreatedBy;
                        if (demandEmpId == userId)
                        {
                            counter++;
                            TotalAcheivemnt.Add(counter);
                        }
                    }
                    //Villas Demand
                    else if (contract.DemandCat == Categories.Villas)
                    {
                        var demandEmpId =(await _uow.VillasDemandsRepo.FindAsync(a => a.PK_VillasDemands_Id == contract.DemandUnits_Id)).FirstOrDefault().FK_VillasDemands_Users_CreatedBy;
                        if (demandEmpId == userId)
                        {
                            counter++;
                            TotalAcheivemnt.Add(counter);
                        }
                    }
                    //land Demand
                    else if (contract.DemandCat == Categories.Lands)
                    {
                        var demandEmpId = (await _uow.LandsDemandsRepo.FindAsync(a => a.PK_LandsDemands_Id == contract.DemandUnits_Id)).FirstOrDefault().FK_LandsDemands_Users_CreatedBy;
                        if (demandEmpId == userId)
                        {
                            counter++;
                            TotalAcheivemnt.Add(counter);
                        }
                    }
                    //Shop Demand
                    else if (contract.DemandCat == Categories.Shops)
                    {
                        var demandEmpId = (await _uow.ShopDemandsRepo.FindAsync(a => a.PK_ShopDemands_Id == contract.DemandUnits_Id)).FirstOrDefault().FK_ShopDemands_Users_CreatedBy;
                        if (demandEmpId == userId)
                        {
                            counter++;
                            TotalAcheivemnt.Add(counter);
                        }
                    }


                }
            }
            return TotalAcheivemnt;
        }

        public async Task<IEnumerable<int>> EmpRentContracts(int userId) {
            int year = DateTime.Now.Year;
            var TotalAcheivemnt = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                var counter = 0;
                List<BLL.Models.tbl_RentAgreementHeaders> monthContracts =(await _uow.RentHeaderRepo.FindAsync(h => !h.IsDeleted && h.Date.Year == year && h.Date.Month == i)).ToList();
                foreach (BLL.Models.tbl_RentAgreementHeaders contract in monthContracts)
                {

                    //Apartment Demand
                    if (contract.DemandCat == Categories.Apartements)
                    {
                        var demandEmpId =( await _uow.DemandRepo.FindAsync(a => a.PK_DemandUnits_Id == contract.DemandUnits_Id)).FirstOrDefault().FK_DemandUnits_Users_CreatedBy;
                        if (demandEmpId==userId)
                        {
                            counter++;
                            TotalAcheivemnt.Add(counter);
                        }
                    }
                    //Villas Demand
                    else if (contract.DemandCat == Categories.Villas)
                    {
                        var demandEmpId = (await _uow.VillasDemandsRepo.FindAsync(a => a.PK_VillasDemands_Id == contract.DemandUnits_Id)).FirstOrDefault().FK_VillasDemands_Users_CreatedBy;
                        if (demandEmpId == userId)
                        {
                            counter++;
                            TotalAcheivemnt.Add(counter);
                        }
                    }
                    //land Demand
                    else if (contract.DemandCat == Categories.Lands)
                    {
                        var demandEmpId =(await _uow.LandsDemandsRepo.FindAsync(a => a.PK_LandsDemands_Id == contract.DemandUnits_Id)).FirstOrDefault().FK_LandsDemands_Users_CreatedBy;
                        if (demandEmpId == userId)
                        {
                            counter++;
                            TotalAcheivemnt.Add(counter);
                        }
                    }
                    //Shop Demand
                    else if (contract.DemandCat == Categories.Shops)
                    {
                       var demandEmpId = (await _uow.ShopDemandsRepo.FindAsync(a => a.PK_ShopDemands_Id == contract.DemandUnits_Id)).FirstOrDefault().FK_ShopDemands_Users_CreatedBy;
                        if (demandEmpId == userId)
                        {
                            counter++;
                            TotalAcheivemnt.Add(counter);
                        }
                    }


                }
            }
            return TotalAcheivemnt;

        }
        public async Task<IEnumerable<int>> EmpContracts(int userId,List<int> rentals,List<int> sales)
        {
            int year = DateTime.Now.Year;
            var TotalAcheivemnt = new List<int>();           
            if (rentals==null||rentals.Count==0)
            {
                rentals =(await EmpRentContracts(userId)).ToList();
            }
            if (sales == null || sales.Count == 0)
            {
                sales = new List<int>();
            }
            for (int i = 0; i < sales.Count(); i++)
            {
                TotalAcheivemnt.Add(sales[i] + rentals[i]);
            }
            return TotalAcheivemnt;
        }

        public async Task<IEnumerable<int>> MonthlySaleContracts()
        {
            int year = DateTime.Now.Year;
            var monthlyAcheivement = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                monthlyAcheivement.Add((await _uow.SaleHeaderRepo.FindAsync(h => !h.IsDeleted && h.Date.Year == year && h.Date.Month == i)).Count());
            }
            _monthlySaleContracts = monthlyAcheivement;
            return monthlyAcheivement;

        }
        public async Task<IEnumerable<int>> MonthlyRentContracts()
        {
            int year = DateTime.Now.Year;
            var monthlyAcheivement = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                monthlyAcheivement.Add((await _uow.RentHeaderRepo.FindAsync(h => !h.IsDeleted && h.Date.Year == year && h.Date.Month == i)).Count());
            }
            _monthlyRentContracts = monthlyAcheivement;
            return monthlyAcheivement;
        }
        public async Task<IEnumerable<int>> TotalMonthlyContracts()
        {
            int year = DateTime.Now.Year;
            var TotalAcheivemnt = new List<int>();
            if (!_monthlyRentContracts.Any())
            {
                for (int i = 1; i < 13; i++)
                {
                    _monthlyRentContracts.Add((await _uow.RentHeaderRepo.FindAsync(h => !h.IsDeleted && h.Date.Year == year && h.Date.Month == i)).Count());
                }
            }
            if (!_monthlySaleContracts.Any())
            {
                for (int i = 1; i < 13; i++)
                {
                    _monthlySaleContracts.Add((await _uow.SaleHeaderRepo.FindAsync(h => !h.IsDeleted && h.Date.Year == year && h.Date.Month == i)).Count());
                }
            }
            for (int i = 0; i < _monthlySaleContracts.Count(); i++)
            {
                TotalAcheivemnt.Add(_monthlySaleContracts[i] + _monthlyRentContracts[i]);
            }
            return TotalAcheivemnt;
        }
    }
}
