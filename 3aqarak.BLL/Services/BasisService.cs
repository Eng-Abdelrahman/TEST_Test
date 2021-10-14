using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3aqarak.BLL.Dto;
using AutoMapper;
using _3aqarak.BLL.Models;

namespace _3aqarak.BLL.Services
{
    public class BasisService : IBasisService
    {
        private readonly IUnitOfWork _uow;

        public BasisService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> DeleteBasis(int id,int userId)
        {
            var DBBasis =(await _uow.PayBasisRepo.FindAsync(u => u.PK_PaymentBasis_Id == id)).FirstOrDefault();
            if (DBBasis != null)
            {
                DBBasis.IsDeleted = true;
                DBBasis.FK_PaymentBasis_Users_ModidfiedBy = userId;
                _uow.PayBasisRepo.Update(DBBasis);

            }

            return await _uow.SaveAsync() > 0;
        }

        public async Task<BasisDto> FindByID(int id)
        {
            var basis =(await _uow.PayBasisRepo.FindAsync(u => u.PK_PaymentBasis_Id == id)).FirstOrDefault();
            return (basis != null) ? Mapper.Map<tbl_PaymentBasis, BasisDto>(basis) : new BasisDto();
        }

        public async Task<List<BasisDto>> GetBasis()
        {
            var Basis =await _uow.PayBasisRepo.
                FindAsync(u => u.IsDeleted == false);
            if (Basis.Any() && Basis != null)
            {
                return Mapper.Map<List<tbl_PaymentBasis>, List<BasisDto>>(Basis.ToList());
            }
            return new List<BasisDto>();
        }

        public async Task<bool> SaveBasis(BasisDto basis, int userId)
        {
            if (basis.PK_PaymentBasis_Id == 0)
            {
                var newBasis = Mapper.Map<BasisDto, tbl_PaymentBasis>(basis);
                newBasis.FK_PaymentBasis_Users_CreatedBy = userId;
                newBasis.FK_PaymentBasis_Users_ModidfiedBy = userId;
                _uow.PayBasisRepo.Add(newBasis);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateBasis(BasisDto basis, int userId)
        {
            var DBBasis =(await _uow.PayBasisRepo.FindAsync(u => u.PK_PaymentBasis_Id == basis.PK_PaymentBasis_Id)).FirstOrDefault();
            if (DBBasis != null)
            {
                DBBasis.Name = basis.Name;
                DBBasis.NoOfDays = basis.NoOfDays;
                DBBasis.FK_PaymentBasis_Users_ModidfiedBy = userId;
                _uow.PayBasisRepo.Update(DBBasis);

            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
