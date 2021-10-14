using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.BLL.Services
{
    public class ExpectedContractService : IexpectedContractService
    {

        private readonly IUnitOfWork _uow;

        public ExpectedContractService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> CancelExpectedContract(int id, int userId)
        {
            tbl_ExpectedContracts DBExpected =await _uow.ExpectedRepo.GetAsync(id);
            if (DBExpected != null)
            {
                DBExpected.IsCancelled = true;
                DBExpected.FK_ExpectContracts_Users_ModidfiedBy = userId;
                DBExpected.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                _uow.ExpectedRepo.Update(DBExpected);
            }

            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> CloseExpected(int id, int userId)
        {
            tbl_ExpectedContracts expected =(await _uow.ExpectedRepo.FindAsync(u => u.PK_ExpectContracts_Id == id && !u.IsDeleted)).FirstOrDefault();
            expected.IsDone = true;
            expected.FK_ExpectContracts_Users_ModidfiedBy = userId;
            _uow.ExpectedRepo.Update(expected);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> DeleteExpectedContract(int id, int userId)
        {
            tbl_ExpectedContracts DBExpected =await _uow.ExpectedRepo.GetAsync(id);
            if (DBExpected != null)
            {
                DBExpected.IsDeleted = true;
                DBExpected.FK_ExpectContracts_Users_ModidfiedBy = userId;
                DBExpected.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                _uow.ExpectedRepo.Update(DBExpected);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<ExpectedContractDto> FindByID(int id)
        {
            tbl_ExpectedContracts expected =(await _uow.ExpectedRepo.FindAsync(u => u.PK_ExpectContracts_Id == id && !u.IsDeleted)).FirstOrDefault();
            return (expected != null) ? Mapper.Map<tbl_ExpectedContracts, ExpectedContractDto>(expected) : new ExpectedContractDto();
        }

        public async Task<SelectList> GetCats()
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Categories_Id", "CategoryName");

        }

        public async Task<List<ExpectedContractDto>> GetExpectedContracts(DateTime from, DateTime to, int catId)
        {
            var ExpectedContracts = new List<tbl_ExpectedContracts>();
            if (catId>0)
            {
               ExpectedContracts =(await  _uow.ExpectedRepo.FindAsync(u => u.AvailableCat == catId && u.IsDeleted == false && !u.IsCancelled && !u.IsDone &&
            ((u.PostponeDateTime != null && (DbFunctions.TruncateTime(u.PostponeDateTime.Value) >= from.Date && DbFunctions.TruncateTime(u.PostponeDateTime.Value) <= to.Date)) ||
            (u.PostponeDateTime == null && (DbFunctions.TruncateTime(u.ExpectDateTime) >= from.Date && DbFunctions.TruncateTime(u.ExpectDateTime) <= to.Date))))).ToList();
            }
            else
            {
                ExpectedContracts =(await _uow.ExpectedRepo.FindAsync(u => u.IsDeleted == false && !u.IsCancelled && !u.IsDone &&
                        ((u.PostponeDateTime != null && (DbFunctions.TruncateTime(u.PostponeDateTime.Value) >= DbFunctions.TruncateTime(from.Date) && DbFunctions.TruncateTime(u.PostponeDateTime.Value) <= DbFunctions.TruncateTime(to.Date))) ||
                        (u.PostponeDateTime == null && (DbFunctions.TruncateTime(u.ExpectDateTime) >= DbFunctions.TruncateTime(from.Date) && DbFunctions.TruncateTime(u.ExpectDateTime) <= DbFunctions.TruncateTime(to.Date)))))).ToList();
            }

            if (ExpectedContracts.Any() && ExpectedContracts != null)
            {
                List<ExpectedContractDto> expectedDtos = Mapper.Map<List<tbl_ExpectedContracts>, List<ExpectedContractDto>>(ExpectedContracts);
                return expectedDtos;
            }
            return new List<ExpectedContractDto>();
        }

        public async Task<bool> SaveExpectedContract(ExpectedContractDto ExpectedContract, int userId)
        {
            tbl_ExpectedContracts newContract = Mapper.Map<ExpectedContractDto, tbl_ExpectedContracts>(ExpectedContract);
            newContract.AvailableCat = ExpectedContract.CategoryId;
            newContract.FK_ExpectContracts_Users_CreatedBy = userId;
            newContract.FK_ExpectContracts_Users_ModidfiedBy = userId;
            newContract.CreatedAt = DateTime.UtcNow.AddMinutes(120);
            newContract.ModifiedAt = DateTime.UtcNow.AddMinutes(120);

            _uow.ExpectedRepo.Add(newContract);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateExpectedContract(ExpectedContractDto expectedContract, int userId)
        {
            tbl_ExpectedContracts DBExpected =await _uow.ExpectedRepo.GetAsync(expectedContract.PK_ExpectContracts_Id);
            if (DBExpected != null)
            {
                DBExpected.Deposit = expectedContract.Deposit;
                DBExpected.Notes = expectedContract.Notes;
                if ((DBExpected.ExpectDateTime.Date < expectedContract.ExpectDateTime.Date))
                {
                    DBExpected.PostponeDateTime = expectedContract.ExpectDateTime;
                    DBExpected.IsPostponed = true;
                }
                else
                {
                    DBExpected.ExpectDateTime = expectedContract.ExpectDateTime;
                }
                DBExpected.FK_ExpectContracts_Users_ModidfiedBy = userId;
                DBExpected.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                DBExpected.SellerAddress = expectedContract.SellerAddress;
                DBExpected.SellerNationalId = expectedContract.SellerNationalId;
                DBExpected.BuyerAddress = expectedContract.BuyerAddress;
                DBExpected.BuyerNationalId = expectedContract.BuyerNationalId;
                _uow.ExpectedRepo.Update(DBExpected);
            }
            return await _uow.SaveAsync() > 0;
        }



    }
}
