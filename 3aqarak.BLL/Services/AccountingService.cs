using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Structs;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Services
{
    public class AccountingService : IAccountingService
    {
        private readonly IUnitOfWork _uow;

        public AccountingService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<ContractCommissionsDto>> GetContracts(DateTime from, DateTime to, bool isCalc, int type)
        {
            List<ContractCommissionsDto> contractData = new List<ContractCommissionsDto>();
            if (type == TranscatTypes.Rental)
            {
                if (isCalc)
                {
                    contractData =(await _uow.RentHeaderRepo.FindAsync(h => !h.IsDeleted && h.IsCalculated && !h.HasEnded && (h.Date >= from && h.Date <= to))).Select(h => new ContractCommissionsDto()
                    {
                        Calculated = "تم حسابه من قبل",
                        ContractId = h.PK_RentAgreements_Id,
                        StringDate = h.Date.ToShortDateString(),
                        Type = TranscatTypes.RentName,
                        AvailableCatId = h.AvailableCat,
                        AvailableId = h.AvailableUnits_Id,
                    }).ToList();

                }
                else
                {
                    contractData =(await _uow.RentHeaderRepo.FindAsync(h => !h.IsDeleted && !h.IsCalculated && !h.HasEnded && (h.Date >= from && h.Date <= to))).Select(h => new ContractCommissionsDto()
                    {
                        Calculated = "لم يتم حسابه من قبل! ",
                        ContractId = h.PK_RentAgreements_Id,
                        StringDate = h.Date.ToShortDateString(),
                        Type = TranscatTypes.RentName,
                        AvailableCatId = h.AvailableCat,
                        AvailableId = h.AvailableUnits_Id,


                    }).ToList();
                }

            }
            else if (type == TranscatTypes.Sale)
            {
                if (isCalc)
                {
                    contractData = (await _uow.SaleHeaderRepo.FindAsync(h => !h.IsDeleted && h.IsCalculated && h.IsDone && (h.Date >= from && h.Date <= to))).Select(h => new ContractCommissionsDto()
                    {
                        Calculated = "تم حسابه من قبل",
                        ContractId = h.PK_SalesHeaders_Id,
                        StringDate = h.Date.ToShortDateString(),
                        Type = TranscatTypes.SaleName,
                        AvailableCatId = h.AvailableCat,
                        AvailableId = h.AvailableUnits_Id,

                    }).ToList();
                }
                else
                {
                    contractData =(await _uow.SaleHeaderRepo.FindAsync(h => !h.IsDeleted && !h.IsCalculated && h.IsDone && (h.Date >= from && h.Date <= to))).Select(h => new ContractCommissionsDto()
                    {
                        Calculated = "لم يتم حسابه من قبل! ",
                        ContractId = h.PK_SalesHeaders_Id,
                        StringDate = h.Date.ToShortDateString(),
                        Type = TranscatTypes.SaleName,
                        AvailableCatId = h.AvailableCat,
                        AvailableId = h.AvailableUnits_Id,

                    }).ToList();
                }
            }

            for (int i = 0; i < contractData.Count(); i++)
            {
                if (contractData[i].AvailableCatId == Categories.Apartements)
                {
                    var ContractData = contractData[i];
                    var transId =(await _uow.AvailableRepo.FindAsync(a => a.PK_AvailableUnits_Id == ContractData.AvailableId && !a.IsDeleted)).FirstOrDefault().FK_AvaliableUnits_Transaction_TransactionId;
                     contractData[i].TransCode =(await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == transId)).FirstOrDefault().TransCode;
                }
                else if (contractData[i].AvailableCatId == Categories.Villas)
                {
                    var ContractData = contractData[i];
                    var transId =(await _uow.VillasAvailablesRepo.FindAsync(a => a.PK_VillasAvailables_Id == ContractData.AvailableId && !a.IsDeleted)).FirstOrDefault().FK_VillasAvailables_Transactions_Id;
                    contractData[i].TransCode =(await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == transId)).FirstOrDefault().TransCode;

                }
                else if (contractData[i].AvailableCatId == Categories.Lands)
                {
                    var ContractData = contractData[i];
                    var transId = (await _uow.AvailableLandsRepo.FindAsync(a => a.PK_AvailableLands_Id == ContractData.AvailableId && !a.IsDeleted)).FirstOrDefault().FK_AvaliableLands_Transactions_TransactionId;
                    contractData[i].TransCode =(await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id ==transId)).FirstOrDefault().TransCode;

                }
                //TODO: holding until finishing new shop available repo
                else if (contractData[i].AvailableCatId == Categories.Shops)
                {
                    var ContractData = contractData[i];
                    var transId =(await _uow.ShopAvailableRepo.FindAsync(a => a.PK_ShopAvailable_Id == ContractData.AvailableId && !a.IsDeleted)).FirstOrDefault().FK_ShopAvailable_Transactions_Id;
                    contractData[i].TransCode =(await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == transId)).FirstOrDefault().TransCode;

                }
            }

            return contractData;
        }
        public async Task<bool> SentContarctAsCalculated(int contractId, int userId, string type)
        {
            if (int.Parse(type) == TranscatTypes.Rental)
            {
                IEnumerable<tbl_RentAgreementHeaders> DBContract =await _uow.RentHeaderRepo.FindAsync(c => c.PK_RentAgreements_Id == contractId);
                if (DBContract != null)
                {
                    foreach (tbl_RentAgreementHeaders item in DBContract)
                    {
                        item.IsCalculated = true;

                    }
                }
            }
            if (int.Parse(type) == TranscatTypes.Sale)
            {
                IEnumerable<tbl_SaleAgreementHeaders> DBContract =await _uow.SaleHeaderRepo.FindAsync(c => c.PK_SalesHeaders_Id == contractId);
                if (DBContract != null)
                {
                    foreach (tbl_SaleAgreementHeaders item in DBContract)
                    {
                        item.IsCalculated = true;

                    }
                }
            }
            return await _uow.SaveAsync() > 0;
        }
        public async Task<bool> SaveCompCommission(decimal amount, int contractId, int userId)
        {
            IEnumerable<tbl_CompCommissions> DBComm =await _uow.CompCommRepo.FindAsync(cc => cc.FK_CommpComm_Contarct_Id == contractId);
            if (DBComm != null && DBComm.Any())
            {
                foreach (tbl_CompCommissions item in DBComm)
                {
                    _uow.CompCommRepo.Remove(item);
                }
                bool deleted = await _uow.SaveAsync() == DBComm.Count();
                if (!deleted)
                {
                    return deleted;
                }
            }
            tbl_CompCommissions compCommObj = new tbl_CompCommissions()
            {
                Date = DateTime.UtcNow.AddMinutes(120),
                ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                FK_CommpComm_Contarct_Id = contractId,
                Amount = amount,
                FK_CompComm_Users_CreatedBy = userId,
                FK_CompComm_Users_ModidfiedBy = userId,
            };
            _uow.CompCommRepo.Add(compCommObj);

            bool savedAll = await _uow.SaveAsync() > 0;
            return savedAll;
        }
        public async Task<bool> SaveEmpsCommissions(CommPctgsDto comms, int userId)
        {
            IEnumerable<tbl_EmpCommissions> DBComms =await _uow.EmpCommRepo.FindAsync(ec => ec.FK_EmpComm_Contarct_Id == comms.ContractId);
            if (DBComms != null && DBComms.Any())
            {
                foreach (tbl_EmpCommissions item in DBComms)
                {
                    _uow.EmpCommRepo.Remove(item);
                }
                bool deleted = await _uow.SaveAsync() == DBComms.Count();
                if (!deleted)
                {
                    return deleted;
                }
            }
            tbl_EmpCommissions salesCommObj = new tbl_EmpCommissions()
            {
                CommValue = comms.SalesComm,
                FK_EmpComm_Contarct_Id = comms.ContractId,
                CreatedAt = DateTime.UtcNow.AddMinutes(120),
                Date = DateTime.UtcNow.AddMinutes(120),
                FK_EmpComm_Emp_Id = comms.SalesId,
                FK_EmpComm_Users_CreatedBy = userId,
                FK_EmpComm_Users_ModidfiedBy = userId,
                ModifiedAt = DateTime.UtcNow.AddMinutes(120),
            };
            _uow.EmpCommRepo.Add(salesCommObj);

            tbl_EmpCommissions teleSalesCommObj = new tbl_EmpCommissions()
            {
                CommValue = comms.TeleSalesComm,
                FK_EmpComm_Contarct_Id = comms.ContractId,
                CreatedAt = DateTime.UtcNow.AddMinutes(120),
                Date = DateTime.UtcNow.AddMinutes(120),
                FK_EmpComm_Emp_Id = comms.TeleSalesId,
                FK_EmpComm_Users_CreatedBy = userId,
                FK_EmpComm_Users_ModidfiedBy = userId,
                ModifiedAt = DateTime.UtcNow.AddMinutes(120),
            };
            _uow.EmpCommRepo.Add(teleSalesCommObj);

            tbl_EmpCommissions mgrCommObj = new tbl_EmpCommissions()
            {
                CommValue = comms.MgrComm,
                FK_EmpComm_Contarct_Id = comms.ContractId,
                CreatedAt = DateTime.UtcNow.AddMinutes(120),
                Date = DateTime.UtcNow.AddMinutes(120),
                FK_EmpComm_Emp_Id = comms.MgrId,
                FK_EmpComm_Users_CreatedBy = userId,
                FK_EmpComm_Users_ModidfiedBy = userId,
                ModifiedAt = DateTime.UtcNow.AddMinutes(120),
            };
            _uow.EmpCommRepo.Add(mgrCommObj);

            bool savedAll = await _uow.SaveAsync() == 3;
            return savedAll;
        }
        public async Task<ContractCommissionsDto> SetRentContractCommissions(RentHeaderDto contract)
        {
            ContractCommissionsDto contractCommDto = new ContractCommissionsDto();
            DemandCentralDto demand = new DemandCentralDto();
            if (contract.DemandCat==Categories.Apartements)
            {
                 demand =Mapper.Map<tbl_DemandUnits, DemandCentralDto>((await _uow.DemandRepo.FindAsync(d => d.PK_DemandUnits_Id == contract.DemandUnits_Id)).FirstOrDefault());
            }
            else if (contract.DemandCat == Categories.Villas)
            {
                 demand = Mapper.Map<tbl_VillasDemands, DemandCentralDto>((await _uow.VillasDemandsRepo.FindAsync(d => d.PK_VillasDemands_Id == contract.DemandUnits_Id)).FirstOrDefault());
            }
            else if (contract.DemandCat == Categories.Lands)
            {
                 demand = Mapper.Map<tbl_LandsDemands, DemandCentralDto>((await _uow.LandsDemandsRepo.FindAsync(d => d.PK_LandsDemands_Id == contract.DemandUnits_Id)).FirstOrDefault());
            }
            else if (contract.DemandCat == Categories.Shops)
            {
                demand = Mapper.Map<tbl_ShopDemands, DemandCentralDto>((await _uow.ShopDemandsRepo.FindAsync(d => d.PK_ShopDemands_Id == contract.DemandUnits_Id)).FirstOrDefault());
            }

            contractCommDto.ContractId = contract.PK_RentAgreements_Id;
            contractCommDto.RentAmount = contract.ValueOfRental.ToString();
            contractCommDto.StringDate = contract.Date.ToShortDateString();
            //1
            tbl_Branches branch =(await _uow.BranchRepo.FindAsync(b => b.PK_Branch_Id == demand.BranchId)).FirstOrDefault();

            contractCommDto.MgrId = branch.FK_Branches_Users_MgrId;
            tbl_Users manager =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.MgrId)).FirstOrDefault();
            contractCommDto.MgrName = manager.FirstName + " " + manager.LastName;
            //2
            tbl_Users demandAssumeTeleSales =(await _uow.UserRepo.FindAsync(u => u.PK_Users_Id == demand.CreatedBy)).FirstOrDefault();
            tbl_Users demandAssumeSales =(await _uow.UserRepo.FindAsync(u => u.PK_Users_Id == demand.SalesId)).FirstOrDefault();
            //3
            tbl_Commissions commissionsPctgs = (await _uow.CommissionsRepo.GetAllAsync()).Where(c => !c.IsDeleted && c.FK_Commissions_Categories_Id == demand.CategoryId && c.FK_Commissions_Transactions_Id == demand.TransId).FirstOrDefault();
            contractCommDto.TelesalesCommissionPercent = commissionsPctgs.TelesalesComission.ToString();
            contractCommDto.SalesCommissionPercent = commissionsPctgs.SalesComission.ToString();
            contractCommDto.MgrCommissionPercent = commissionsPctgs.MgrCommission.ToString();

            tbl_EmpCommissions mgrCommissionAmount =(await _uow.EmpCommRepo.FindAsync(ec => ec.FK_EmpComm_Contarct_Id == contract.PK_RentAgreements_Id && ec.FK_EmpComm_Emp_Id == manager.PK_Users_Id)).FirstOrDefault();
            contractCommDto.MgrCommissionAmount = mgrCommissionAmount != null ? mgrCommissionAmount.CommValue : 0;

            if (demandAssumeTeleSales.tbl_Departements2.RegCode == DeptCodes.TeleSales && demandAssumeSales.tbl_Departements2.RegCode == DeptCodes.TeleSales)
            {
                contractCommDto.TeleSalesId = demand.CreatedBy;
                tbl_Users telesales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.TeleSalesId)).FirstOrDefault();
                contractCommDto.TeleSalesName = telesales.FirstName + " " + telesales.LastName;
                contractCommDto.SalesId = demand.SalesId;
                tbl_Users sales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.SalesId)).FirstOrDefault();
                contractCommDto.SalesName = sales.FirstName + " " + sales.LastName;
            }
            else if (demandAssumeTeleSales.tbl_Departements2.RegCode == DeptCodes.TeleSales && demandAssumeSales.tbl_Departements2.RegCode == DeptCodes.Sales)
            {
                //4
                contractCommDto.TeleSalesId = demand.CreatedBy;
                tbl_Users telesales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.TeleSalesId)).FirstOrDefault();
                contractCommDto.TeleSalesName = telesales.FirstName + " " + telesales.LastName;
                //5
                contractCommDto.SalesId = demand.SalesId;
                tbl_Users sales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.SalesId)).FirstOrDefault();
                contractCommDto.SalesName = sales.FirstName + " " + sales.LastName;
            }
            else if (demandAssumeTeleSales.tbl_Departements2.RegCode == DeptCodes.Sales && demandAssumeSales.tbl_Departements2.RegCode == DeptCodes.TeleSales)
            {
                //6
                contractCommDto.SalesId = demand.CreatedBy;
                tbl_Users sales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.SalesId)).FirstOrDefault();
                contractCommDto.SalesName = sales.FirstName + " " + sales.LastName;
                //7
                contractCommDto.TeleSalesId = demand.SalesId;
                tbl_Users teleSales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.TeleSalesId)).FirstOrDefault();
                contractCommDto.SalesName = teleSales.FirstName + " " + teleSales.LastName;
            }
            else if (demandAssumeTeleSales.tbl_Departements2.RegCode == DeptCodes.Sales && demandAssumeSales.tbl_Departements2.RegCode == DeptCodes.Sales)
            {
                //8
                contractCommDto.SalesId = demand.CreatedBy;
                tbl_Users sales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.SalesId)).FirstOrDefault();
                contractCommDto.SalesName = sales.FirstName + " " + sales.LastName;
                //9
                contractCommDto.TeleSalesId = demand.SalesId;
                tbl_Users teleSales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.TeleSalesId)).FirstOrDefault();
                contractCommDto.SalesName = teleSales.FirstName + " " + teleSales.LastName;
            }         
            else
            {
                //10
                contractCommDto.TeleSalesId = demand.CreatedBy;
                tbl_Users telesales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.TeleSalesId)).FirstOrDefault();
                contractCommDto.TeleSalesName = telesales.FirstName + " " + telesales.LastName;
                //11
                contractCommDto.SalesId = demand.SalesId;
                tbl_Users sales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.SalesId)).FirstOrDefault();
                contractCommDto.SalesName = sales.FirstName + " " + sales.LastName;
            }
            tbl_EmpCommissions telesalesCommissionAmount =(await _uow.EmpCommRepo.FindAsync(ec => ec.FK_EmpComm_Contarct_Id == contract.PK_RentAgreements_Id && ec.FK_EmpComm_Emp_Id == contractCommDto.TeleSalesId)).FirstOrDefault();
            contractCommDto.TelesalesCommissionAmount = telesalesCommissionAmount != null ? telesalesCommissionAmount.CommValue : 0;

            tbl_EmpCommissions salesCommissionAmount =(await _uow.EmpCommRepo.FindAsync(ec => ec.FK_EmpComm_Contarct_Id == contract.PK_RentAgreements_Id && ec.FK_EmpComm_Emp_Id == contractCommDto.SalesId)).FirstOrDefault();
            contractCommDto.SalesCommissionAmount = telesalesCommissionAmount != null ? telesalesCommissionAmount.CommValue : 0;
            return contractCommDto;
        }
        public async Task<ContractCommissionsDto> SetSalesContractCommissions(SaleHeaderDto contract)
        {
            ContractCommissionsDto contractCommDto = new ContractCommissionsDto();
            DemandCentralDto demand = new DemandCentralDto();
            if (contract.DemandCat == Categories.Apartements)
            {
                demand = Mapper.Map<tbl_DemandUnits, DemandCentralDto>((await _uow.DemandRepo.FindAsync(d => d.PK_DemandUnits_Id == contract.DemandUnits_Id)).FirstOrDefault());
            }
            else if (contract.DemandCat == Categories.Villas)
            {
                demand = Mapper.Map<tbl_VillasDemands, DemandCentralDto>((await _uow.VillasDemandsRepo.FindAsync(d => d.PK_VillasDemands_Id == contract.DemandUnits_Id)).FirstOrDefault());

            }
            else if (contract.DemandCat == Categories.Lands)
            {
                demand = Mapper.Map<tbl_LandsDemands, DemandCentralDto>((await _uow.LandsDemandsRepo.FindAsync(d => d.PK_LandsDemands_Id == contract.DemandUnits_Id)).FirstOrDefault());
            }
            else if (contract.DemandCat == Categories.Shops)
            {
                demand = Mapper.Map<tbl_ShopDemands, DemandCentralDto>((await _uow.ShopDemandsRepo.FindAsync(d => d.PK_ShopDemands_Id == contract.DemandUnits_Id)).FirstOrDefault());
            }
            contractCommDto.ContractId = contract.PK_SalesHeaders_Id;
            contractCommDto.TotalAmount = contract.TotalAmount.ToString();
            contractCommDto.StringDate = contract.Date.ToShortDateString();

            tbl_Branches branch = (await _uow.BranchRepo.FindAsync(b => b.PK_Branch_Id == demand.BranchId)).FirstOrDefault();

            contractCommDto.MgrId = branch.FK_Branches_Users_MgrId;
            tbl_Users manager =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.MgrId)).FirstOrDefault();
            contractCommDto.MgrName = manager.FirstName + " " + manager.LastName;

            tbl_Users DemandAssumeTeleSales =(await _uow.UserRepo.FindAsync(u => u.PK_Users_Id == demand.CreatedBy)).FirstOrDefault();
            tbl_Users DemandAssumeSales =(await _uow.UserRepo.FindAsync(u => u.PK_Users_Id == demand.SalesId)).FirstOrDefault();

            tbl_Commissions commissionsPctgs = (await _uow.CommissionsRepo.GetAllAsync()).Where(c => !c.IsDeleted && c.FK_Commissions_Categories_Id == demand.CategoryId && c.FK_Commissions_Transactions_Id == demand.TransId).FirstOrDefault();
            if (commissionsPctgs != null)
            {
                contractCommDto.TelesalesCommissionPercent = commissionsPctgs.TelesalesComission.ToString() + " %";
                contractCommDto.SalesCommissionPercent = commissionsPctgs.SalesComission.ToString() + " %";
                contractCommDto.MgrCommissionPercent = commissionsPctgs.MgrCommission.ToString() + " %";
            }

            tbl_EmpCommissions mgrCommissionAmount =(await _uow.EmpCommRepo.FindAsync(ec => ec.FK_EmpComm_Contarct_Id == contract.PK_SalesHeaders_Id && ec.FK_EmpComm_Emp_Id == manager.PK_Users_Id)).FirstOrDefault();
            contractCommDto.MgrCommissionAmount = mgrCommissionAmount != null ? mgrCommissionAmount.CommValue : 0;

            if (DemandAssumeTeleSales.tbl_Departements2.RegCode == DeptCodes.TeleSales && DemandAssumeSales.tbl_Departements2.RegCode == DeptCodes.TeleSales)
            {
                contractCommDto.TeleSalesId = demand.CreatedBy;
                tbl_Users telesales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.TeleSalesId)).FirstOrDefault();
                contractCommDto.TeleSalesName = telesales.FirstName + " " + telesales.LastName;
                contractCommDto.SalesId = demand.SalesId;
                tbl_Users sales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.SalesId)).FirstOrDefault();
                contractCommDto.SalesName = sales.FirstName + " " + sales.LastName;
            }
            else if (DemandAssumeTeleSales.tbl_Departements2.RegCode == DeptCodes.TeleSales && DemandAssumeSales.tbl_Departements2.RegCode == DeptCodes.Sales)
            {
                contractCommDto.TeleSalesId = demand.CreatedBy;
                tbl_Users telesales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.TeleSalesId)).FirstOrDefault();
                contractCommDto.TeleSalesName = telesales.FirstName + " " + telesales.LastName;
                contractCommDto.SalesId = demand.SalesId;
                tbl_Users sales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.SalesId)).FirstOrDefault();
                contractCommDto.SalesName = sales.FirstName + " " + sales.LastName;
            }
            else if (DemandAssumeTeleSales.tbl_Departements2.RegCode == DeptCodes.Sales && DemandAssumeSales.tbl_Departements2.RegCode == DeptCodes.TeleSales)
            {

                contractCommDto.SalesId = demand.CreatedBy;
                tbl_Users sales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.SalesId)).FirstOrDefault();
                contractCommDto.SalesName = sales.FirstName + " " + sales.LastName;
                contractCommDto.TeleSalesId = demand.SalesId;
                tbl_Users teleSales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.TeleSalesId)).FirstOrDefault();
                contractCommDto.SalesName = teleSales.FirstName + " " + teleSales.LastName;
            }
            else if (DemandAssumeTeleSales.tbl_Departements2.RegCode == DeptCodes.Sales && DemandAssumeSales.tbl_Departements2.RegCode == DeptCodes.Sales)
            {
                contractCommDto.SalesId = demand.CreatedBy;
                tbl_Users sales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.SalesId)).FirstOrDefault();
                contractCommDto.SalesName = sales.FirstName + " " + sales.LastName;
                contractCommDto.TeleSalesId = demand.SalesId;
                tbl_Users teleSales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.TeleSalesId)).FirstOrDefault();
                contractCommDto.SalesName = teleSales.FirstName + " " + teleSales.LastName;
            }
            else
            {
                contractCommDto.TeleSalesId = demand.CreatedBy;
                tbl_Users telesales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.TeleSalesId)).FirstOrDefault();
                contractCommDto.TeleSalesName = telesales.FirstName + " " + telesales.LastName;
                contractCommDto.SalesId = demand.SalesId;
                tbl_Users sales =(await _uow.UserRepo.FindAsync(U => U.PK_Users_Id == contractCommDto.SalesId)).FirstOrDefault();
                contractCommDto.SalesName = sales.FirstName + " " + sales.LastName;
            }

            tbl_EmpCommissions telesalesCommissionAmount = (await _uow.EmpCommRepo.FindAsync(ec => ec.FK_EmpComm_Contarct_Id == contract.PK_SalesHeaders_Id && ec.FK_EmpComm_Emp_Id == contractCommDto.TeleSalesId)).FirstOrDefault();
            contractCommDto.TelesalesCommissionAmount = telesalesCommissionAmount != null ? telesalesCommissionAmount.CommValue : 0;

            tbl_EmpCommissions salesCommissionAmount =(await _uow.EmpCommRepo.FindAsync(ec => ec.FK_EmpComm_Contarct_Id == contract.PK_SalesHeaders_Id && ec.FK_EmpComm_Emp_Id == contractCommDto.SalesId)).FirstOrDefault();
            contractCommDto.SalesCommissionAmount = telesalesCommissionAmount != null ? telesalesCommissionAmount.CommValue : 0;
            return contractCommDto;
        }
    }

}
