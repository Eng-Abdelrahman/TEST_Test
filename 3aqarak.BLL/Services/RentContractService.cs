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

    public class RentContractService : IRentContractService
    {
        private readonly IUnitOfWork _uow;
        private IConfirmation _conf;
        private readonly HttpServerUtilityBase _server;

        public RentContractService(IUnitOfWork uow, IConfirmation conf, HttpServerUtilityBase server)
        {
            _uow = uow;
            _conf = conf;
            _server = server;
        }
        public async Task<bool> DeleteContract(int id, int userId)
        {

            tbl_RentAgreementHeaders DBContract = (await _uow.RentHeaderRepo.FindAsync(u => u.PK_RentAgreements_Id == id)).FirstOrDefault();
            if (DBContract != null)
            {
                DBContract.IsDeleted = true;
                DBContract.ModidfiedBy = userId;
                _uow.RentHeaderRepo.Update(DBContract);

            }
            bool deleted = await _uow.SaveAsync() > 0;
            if (deleted)
            {
                if (DBContract.ContractUrl != null)
                {
                    string contractPDF = Directory
                            .GetFiles(_server.MapPath("/Assets/Img/RentContractImages"), DBContract.ContractUrl.Split('/')[3], SearchOption.AllDirectories)
                            .FirstOrDefault();
                    if (contractPDF != null)
                    {
                        System.IO.File.Delete(contractPDF);
                    }
                }
                IEnumerable<tbl_RentAgreementDetails> details = await _uow.RentDetailsRepo.FindAsync(d => d.FK_RentHeaders_RentDetails_Id == id);
                if (details != null || details.Any())
                {
                    foreach (tbl_RentAgreementDetails item in details)
                    {
                        item.IsDeleted = true;
                        item.FK_RentDetails_Users_ModidfiedBy = userId;
                    }
                }
                return await _uow.SaveAsync() > 0;
            }
            return false;
        }

        public async Task<RentHeaderDto> FindHeaderByID(int id)
        {
            tbl_RentAgreementHeaders contracts = (await _uow.RentHeaderRepo.FindAsync(u => u.PK_RentAgreements_Id == id)).FirstOrDefault();
            return (contracts != null) ? Mapper.Map<tbl_RentAgreementHeaders, RentHeaderDto>(contracts) : new RentHeaderDto();
        }

        public async Task<RentDetailsDto> FindDetailByID(int id)
        {
            tbl_RentAgreementDetails details = (await _uow.RentDetailsRepo.FindAsync(u => u.FK_RentHeaders_RentDetails_Id == id && !u.IsDeleted)).FirstOrDefault();
            return (details != null) ? Mapper.Map<tbl_RentAgreementDetails, RentDetailsDto>(details) : new RentDetailsDto();
        }

        public async Task<List<RentHeaderDto>> GetContracts(DateTime? from, DateTime? to, int catId)
        {
            List<tbl_RentAgreementHeaders> contracts = new List<tbl_RentAgreementHeaders>();
            if (from != null && to != null)
            {
                if (catId > 0)
                {
                    contracts = (await _uow.RentHeaderRepo.FindAsync(u => u.IsDeleted == false && (u.Date >= from && u.Date <= to) && u.AvailableCat == catId)).ToList();
                }
                else
                {
                    contracts = (await _uow.RentHeaderRepo.FindAsync(u => u.IsDeleted == false && (u.Date >= from && u.Date <= to))).ToList();

                }

            }
            else
            {
                if (catId > 0)
                {
                    contracts = (await _uow.RentHeaderRepo.FindAsync(u => u.IsDeleted == false && u.AvailableCat == catId)).ToList();

                }
                else
                {
                    contracts = (await _uow.RentHeaderRepo.FindAsync(u => u.IsDeleted == false)).ToList();

                }
            }
            if (contracts.Any() && contracts != null)
            {

                return Mapper.Map<List<tbl_RentAgreementHeaders>, List<RentHeaderDto>>(contracts.ToList());
            }
            return new List<RentHeaderDto>();
        }

        public async Task<SelectList> GetPaymentBasis()
        {
            return new SelectList(Mapper.Map<List<tbl_PaymentBasis>, List<BasisDto>>((await _uow.PayBasisRepo.GetAllAsync()).ToList().Where(v => v.IsDeleted == false).ToList()), "PK_PaymentBasis_Id", "Name");

        }
        public async Task<SelectList> GetCats()
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Categories_Id", "CategoryName");
        }
        public async Task<SelectList> GetPaymentBasisId(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_PaymentBasis>, List<BasisDto>>((await _uow.PayBasisRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_PaymentBasis_Id", "Name", await _uow.PayBasisRepo.FindAsync(b => b.PK_PaymentBasis_Id == id));

        }

        public async Task<IConfirmation> SaveContract(RentHeaderDto contract, RentDetailsDto details, int userId)
        {
            if (contract.PK_RentAgreements_Id == 0)
            {
                tbl_RentAgreementHeaders newContract = Mapper.Map<RentHeaderDto, tbl_RentAgreementHeaders>(contract);
                newContract.ContractUrl = contract.ContractUrl;
                newContract.FK_RentAgreements_Users_EmpId = userId;
                newContract.HasEnded = false;
                newContract.Date = DateTime.UtcNow.AddHours(2);
                newContract.CreatedAt = DateTime.UtcNow.AddHours(2);
                newContract.ModifiedAt = DateTime.UtcNow.AddHours(2);
                newContract.ModidfiedBy = userId;
                _uow.RentHeaderRepo.Add(newContract);
                if (await _uow.SaveAsync() > 0)
                {
                    tbl_RentAgreementDetails detail = Mapper.Map<RentDetailsDto, tbl_RentAgreementDetails>(details);
                    detail.FK_RentDetails_Users_CreatedBy = userId;
                    detail.FK_RentDetails_Users_ModidfiedBy = userId;
                    detail.FK_RentHeaders_RentDetails_Id = newContract.PK_RentAgreements_Id;
                    _uow.RentDetailsRepo.Add(detail);
                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (_conf.Valid)
                    {
                        _conf.Id = newContract.PK_RentAgreements_Id;
                    }
                    return _conf;
                }
            }
            _conf.Valid = false;
            return _conf;

        }

        public async Task<bool> UpdateContract(RentHeaderDto contract, RentDetailsDto details, int userId)
        {
            tbl_RentAgreementHeaders DBcontract = (await _uow.RentHeaderRepo.FindAsync(u => u.PK_RentAgreements_Id == contract.PK_RentAgreements_Id)).FirstOrDefault();
            if (DBcontract != null)
            {
                DBcontract.BuyerAddress = contract.BuyerAddress;
                DBcontract.BuyerIdNumber = contract.BuyerIdNumber;
                DBcontract.BuyerIdSource = contract.BuyerIdSource;
                DBcontract.BuyerNationality = contract.BuyerNationality;
                if (contract.ContractUrl != null)
                {
                    string contractPDF = Directory
                        .GetFiles(_server.MapPath("/Assets/Img/RentContractImages"), DBcontract.ContractUrl.Split('/')[3], SearchOption.AllDirectories)
                        .FirstOrDefault();
                    if (contractPDF != null)
                    {
                        System.IO.File.Delete(contractPDF);
                    }
                    DBcontract.ContractUrl = contract.ContractUrl;
                }
                DBcontract.DateNxtRent = contract.DateNxtRent;
                DBcontract.FK_RentAgreementHeader_PaymentBasis_Id = contract.FK_RentAgreementHeader_PaymentBasis_Id;
                DBcontract.ModidfiedBy = userId;
                DBcontract.RentalEndDate = contract.RentalEndDate;
                DBcontract.RentalStartDate = contract.RentalStartDate;
                DBcontract.SellerAddress = contract.SellerAddress;
                DBcontract.SellerIdNumber = contract.SellerIdNumber;
                DBcontract.SellerIdSource = contract.SellerIdSource;
                DBcontract.SellerNationality = contract.SellerNationality;
                DBcontract.ValueOfRental = contract.ValueOfRental;
                DBcontract.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                _uow.RentHeaderRepo.Update(DBcontract);
                tbl_RentAgreementDetails DBcontractDetail = (await _uow.RentDetailsRepo.FindAsync(u => u.FK_RentHeaders_RentDetails_Id == contract.PK_RentAgreements_Id)).FirstOrDefault();
                if (DBcontractDetail != null)
                {
                    DBcontractDetail.DetailContent = details.DetailContent;
                    DBcontractDetail.FK_RentDetails_Users_ModidfiedBy = userId;
                    _uow.RentDetailsRepo.Update(DBcontractDetail);
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
                        string fname = Path.Combine("Assets/Img/RentContractImages/", Guid.NewGuid().ToString() + new FileInfo(file.FileName).Extension);
                        file.SaveAs(_server.MapPath("/" + fname));
                        _conf.UrlList.Add(fname);
                        _conf.Message = "تم حفظ الصور بنجاح!";
                    }
                    return _conf;
                }
                catch (Exception ex)
                {
                    string exception = ex.Message;
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
                    _conf.Message = "Not allowed to upload files over 15 MB ";
                    _conf.Valid = false;
                    break;
                }
                string ext = file.ContentType.Split('/')[1].ToLower();
                if (!ext.Equals("jpeg", StringComparison.OrdinalIgnoreCase) &&
                    !ext.Equals("png", StringComparison.OrdinalIgnoreCase) &&
                     !ext.Equals("jpg", StringComparison.OrdinalIgnoreCase) &&
                     !ext.Equals("pdf", StringComparison.OrdinalIgnoreCase)
                     )

                {
                    _conf.Message = "only allowed file types:.png,.jpg,.jpeg,.pdf";
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
            tbl_CompCommissions RelatedComCommission = (await _uow.CompCommRepo.FindAsync(comp => comp.FK_CommpComm_Contarct_Id == id)).FirstOrDefault();
            if (RelatedComCommission != null)
            {
                _uow.CompCommRepo.Remove(RelatedComCommission);
            }
            return await _uow.SaveAsync() > 0;
        }
        public async Task<bool> DeleteRelatedEmpCommission(int id)
        {
            IEnumerable<tbl_EmpCommissions> RelatedEmpCommission = await _uow.EmpCommRepo.FindAsync(emp => emp.FK_EmpComm_Contarct_Id == id);
            if (RelatedEmpCommission != null)
            {
                foreach (tbl_EmpCommissions item in RelatedEmpCommission)
                {
                    _uow.EmpCommRepo.Remove(item);
                }
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> EndRentContract(RentHeaderDto header)
        {
            tbl_RentAgreementHeaders DBContract = (await _uow.RentHeaderRepo.FindAsync(h => h.PK_RentAgreements_Id == header.PK_RentAgreements_Id)).FirstOrDefault();
            DBContract.HasEnded = true;
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateNextRent(RentHeaderDto nextRent, DateTime newDate, int userId)
        {
            tbl_RentAgreementHeaders updatedrent = await _uow.RentHeaderRepo.GetAsync(nextRent.PK_RentAgreements_Id);
            if (updatedrent != null)
            {
                updatedrent.DateNxtRent = newDate;
                updatedrent.ModidfiedBy = userId;
                updatedrent.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
            }
            return await _uow.SaveAsync() > 0;
        }


    }
}
