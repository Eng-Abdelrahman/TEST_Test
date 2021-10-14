using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.BLL.Services
{
    public class SaleContractService : ISaleContractService
    {
        private readonly IUnitOfWork _uow;
        private IConfirmation _conf;
        private readonly HttpServerUtilityBase _server;

        public SaleContractService(IUnitOfWork uow, IConfirmation conf, HttpServerUtilityBase server)
        {
            _uow = uow;
            _conf = conf;
            _server = server;
        }
        public async Task<bool> DeleteContract(int id, int userId)
        {
            bool istrue = false;
            tbl_SaleAgreementHeaders DBContract =(await _uow.SaleHeaderRepo.FindAsync(u => u.PK_SalesHeaders_Id == id)).FirstOrDefault();

            if (DBContract != null)
            {
                if (DBContract.IsInstallable && DBContract.DateOfLastInstall > DateTime.Now)
                {
                    return false;
                }
                DBContract.IsDeleted = true;
                DBContract.FK_SalesHeaders_Users_ModidfiedBy = userId;
                _uow.SaleHeaderRepo.Update(DBContract);

            }
            bool deleted = await _uow.SaveAsync() > 0;
            if (deleted)
            {
                if (DBContract.ContractUrl != null)
                {
                    string contractPDF = Directory
                                .GetFiles(_server.MapPath("/Assets/Img/SaleContractImages"), DBContract.ContractUrl.Split('/')[3], SearchOption.AllDirectories)
                                .FirstOrDefault();
                    if (contractPDF != null)
                    {
                        System.IO.File.Delete(contractPDF);
                    }

                }
                IEnumerable<tbl_SaleAgreementDetails> details =await  _uow.SaleDetailsRepo.FindAsync(d => d.FK_SaleDetails_SaleHeaders_Id == id);
                if (details != null && details.Any())
                {
                    foreach (tbl_SaleAgreementDetails item in details)
                    {
                        item.IsDeleted = true;
                        item.FK_SaleDetails_Users_ModidfiedBy = userId;
                        _uow.SaleDetailsRepo.Update(item);
                    }
                    istrue = await _uow.SaveAsync() > 0;
                }

                return istrue;
            }
            return false;
        }

        public async Task<SaleDetailsDto> FindDetailByID(int id)
        {
            tbl_SaleAgreementDetails details =(await _uow.SaleDetailsRepo.FindAsync(u => u.FK_SaleDetails_SaleHeaders_Id == id && !u.IsDeleted)).FirstOrDefault();
            return (details != null) ? Mapper.Map<tbl_SaleAgreementDetails, SaleDetailsDto>(details) : new SaleDetailsDto();
        }

        public async Task<SaleHeaderDto> FindHeaderByID(int id)
        {
            tbl_SaleAgreementHeaders contracts =(await _uow.SaleHeaderRepo.FindAsync(u => u.PK_SalesHeaders_Id == id)).FirstOrDefault();
            return (contracts != null) ? Mapper.Map<tbl_SaleAgreementHeaders, SaleHeaderDto>(contracts) : new SaleHeaderDto();
        }

        public async Task<List<SaleHeaderDto>> GetContracts(DateTime? from, DateTime? to, int catId)
        {
            List<tbl_SaleAgreementHeaders> contracts = new List<tbl_SaleAgreementHeaders>();
            if (from != null && to != null)
            {
                if (catId > 0)
                {
                    contracts = (await _uow.SaleHeaderRepo.FindAsync(u => u.IsDeleted == false && (u.Date >= from && u.Date <= to) && u.AvailableCat == catId)).ToList();

                }
                else
                {
                    contracts = ( await _uow.SaleHeaderRepo.FindAsync(u => u.IsDeleted == false && u.Date >= from && u.Date <= to)).ToList();

                }
            }
            else
            {
                if (catId>0)
                {
                    contracts = (await _uow.SaleHeaderRepo.FindAsync(u => u.IsDeleted == false&&u.AvailableCat==catId)).ToList();

                }
                else
                {
                    contracts =(await _uow.SaleHeaderRepo.FindAsync(u => u.IsDeleted == false)).ToList();

                }
            }
            if (contracts.Any() && contracts != null)
            {

                return Mapper.Map<List<tbl_SaleAgreementHeaders>, List<SaleHeaderDto>>(contracts.ToList());
            }
            return new List<SaleHeaderDto>();
        }

        public async Task<SelectList> GetPaymentBasis()
        {
            return new SelectList(Mapper.Map<List<tbl_PaymentBasis>, List<BasisDto>>((await _uow.PayBasisRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_PaymentBasis_Id", "Name");

        }

        public async Task<SelectList> GetPaymentBasisId(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_PaymentBasis>, List<BasisDto>>((await _uow.PayBasisRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_PaymentBasis_Id", "Name",await _uow.PayBasisRepo.FindAsync(b => b.PK_PaymentBasis_Id == id));

        }

        public async Task<SelectList> GetCats()
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Categories_Id", "CategoryName");
        }

        public async Task<IConfirmation> SaveContract(SaleHeaderDto contract, SaleDetailsDto details, int userId)
        {
            if (contract.PK_SalesHeaders_Id == 0)
            {
                tbl_SaleAgreementHeaders newContract = Mapper.Map<SaleHeaderDto, tbl_SaleAgreementHeaders>(contract);
                newContract.ContractUrl = contract.ContractUrl;
                newContract.FK_SalesHeaders_Users_EmpId = userId;
                newContract.IsDone = true;             
                newContract.Date = DateTime.UtcNow.AddMinutes(120);
                newContract.CreatedAt = DateTime.UtcNow.AddMinutes(120);
                newContract.FK_SalesHeaders_Users_CreatedBy = userId;
                newContract.FK_SalesHeaders_Users_ModidfiedBy = userId;
                _uow.SaleHeaderRepo.Add(newContract);
                if (await _uow.SaveAsync() > 0)
                {
                    tbl_SaleAgreementDetails detail = Mapper.Map<SaleDetailsDto, tbl_SaleAgreementDetails>(details);
                    detail.FK_SaleDetails_Users_CreatedBy = userId;
                    detail.FK_SaleDetails_Users_ModidfiedBy = userId;
                    detail.CreatedAt = DateTime.UtcNow.AddMinutes(120);
                    detail.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                    detail.FK_SaleDetails_SaleHeaders_Id = newContract.PK_SalesHeaders_Id;
                    _uow.SaleDetailsRepo.Add(detail);
                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (_conf.Valid)
                    {
                        _conf.Id = newContract.PK_SalesHeaders_Id;
                    }
                    return _conf;
                }
            }
            _conf.Valid = false;
            return _conf;
        }

        public async Task<bool> UpdateContract(SaleHeaderDto contract, SaleDetailsDto details, int userId)
        {
            tbl_SaleAgreementHeaders DBcontract =(await _uow.SaleHeaderRepo.FindAsync(u => u.PK_SalesHeaders_Id == contract.PK_SalesHeaders_Id)).FirstOrDefault();
            if (DBcontract != null)
            {
                DBcontract.BuyerAddress = contract.BuyerAddress;
                DBcontract.BuyerId = contract.BuyerId;
                if (contract.ContractUrl != null)
                {
                    string contractPDF = Directory
                        .GetFiles(_server.MapPath("/Assets/Img/SaleContractImages"), DBcontract.ContractUrl.Split('/')[3], SearchOption.AllDirectories)
                        .FirstOrDefault();

                    if (contractPDF != null)
                    {
                        System.IO.File.Delete(contractPDF);
                    }
                    DBcontract.ContractUrl = contract.ContractUrl;
                }
                DBcontract.TotalAmount = contract.TotalAmount;
                DBcontract.PaidAmount = contract.PaidAmount;
                DBcontract.DueAmount = contract.DueAmount;
                DBcontract.DateOfFirstInstall = contract.DateOfFirstInstall;
                DBcontract.DateOfNextInstall = contract.DateOfNextInstall;
                DBcontract.DateOfLastInstall = contract.DateOfLastInstall;
                DBcontract.FK_SalesHeaders_PaymentBasis_Id = contract.FK_SalesHeaders_PaymentBasis_Id;
                DBcontract.FK_SalesHeaders_Users_ModidfiedBy = userId;
                DBcontract.DateOfFirstInstall = contract.DateOfFirstInstall;
                DBcontract.DateOfNextInstall = contract.DateOfNextInstall;
                DBcontract.SellerAddress = contract.SellerAddress;
                DBcontract.SellerId = contract.SellerId;
                DBcontract.DefaultInstallValue = DBcontract.DefaultInstallValue;
                DBcontract.NextInstallValue = DBcontract.NextInstallValue;
                DBcontract.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                _uow.SaleHeaderRepo.Update(DBcontract);
                tbl_SaleAgreementDetails DBcontractDetail =(await _uow.SaleDetailsRepo.FindAsync(u => u.FK_SaleDetails_SaleHeaders_Id == contract.PK_SalesHeaders_Id)).FirstOrDefault();
                if (DBcontractDetail != null)
                {
                    DBcontractDetail.DetailContent = details.DetailContent;
                    DBcontractDetail.FK_SaleDetails_Users_ModidfiedBy = userId;
                    DBcontractDetail.ModifiedAt = DateTime.UtcNow.AddMinutes(120);

                    _uow.SaleDetailsRepo.Update(DBcontractDetail);
                }

                return await _uow.SaveAsync() > 0;

            }
            return false;
        }
        //hussainy
        public IConfirmation SavePhotos(HttpFileCollectionBase files)
        {
            IConfirmation conf = ValidatePhotos(files);
            if (conf.Valid)
            {
                try
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname = Path.Combine("Assets/Img/SaleContractImages/", Guid.NewGuid().ToString() + new FileInfo(file.FileName).Extension);
                        file.SaveAs(_server.MapPath("/" + fname));
                        _conf.UrlList.Add(fname);
                        _conf.Message = "تم حفظ الصور بنجاح!";
                    }
                    return _conf;
                }
                catch (Exception)
                {
                    _conf.Valid = false;
                    _conf.Message = "حدث خطا ما عند حفظ الصور!";
                }
            }

            return _conf;

        }

        public IConfirmation ValidatePhotos(HttpFileCollectionBase files)
        {

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                if (file.ContentLength > (15 * (1024 * 1024)))
                {
                    _conf.Message = "Not allowed to upload files over 15 MB";
                    _conf.Valid = false;
                    break;
                }
                string ext = file.ContentType.Split('/')[1].ToLower();
                if (!ext.Equals("pdf", StringComparison.OrdinalIgnoreCase))

                {
                    _conf.Message = "only allowed file types: .pdf";
                    _conf.Valid = false;
                    break;
                }
                else
                {
                    _conf.Valid = true;
                }
            }
            return _conf;
        }

        public async Task<bool> DeleteRelatedCompCommission(int id)
        {
            tbl_CompCommissions RelatedComCommission =(await _uow.CompCommRepo.FindAsync(comp => comp.FK_CommpComm_Contarct_Id == id)).FirstOrDefault();
            if (RelatedComCommission != null)
            {
                _uow.CompCommRepo.Remove(RelatedComCommission);
            }
            return await _uow.SaveAsync() > 0;
        }
        public async Task<bool> DeleteRelatedEmpCommission(int id)
        {
            IEnumerable<tbl_EmpCommissions> RelatedEmpCommission =await _uow.EmpCommRepo.FindAsync(emp => emp.FK_EmpComm_Contarct_Id == id);
            if (RelatedEmpCommission != null)
            {
                foreach (tbl_EmpCommissions item in RelatedEmpCommission)
                {
                    _uow.EmpCommRepo.Remove(item);
                }
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateNextIntstall(SaleHeaderDto nextSaleFee, int nextAmount, DateTime newDate, int userId, int nextInstallValue)
        {
            tbl_SaleAgreementHeaders nextSale =await _uow.SaleHeaderRepo.GetAsync(nextSaleFee.PK_SalesHeaders_Id);
            if (nextSale != null)
            {
                nextSale.DateOfNextInstall = newDate;
                nextSale.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                nextSale.FK_SalesHeaders_Users_ModidfiedBy = userId;
                nextSale.PaidAmount += nextAmount;
                nextSale.DueAmount -= nextAmount;
                nextSale.NextInstallValue = nextInstallValue;
            }
            return await _uow.SaveAsync() > 0;
        }
    }
}
