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
    public class FinancialService : IFinancialService
    {
        private readonly IUnitOfWork _uow;

        public FinancialService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> DeleteFinancialItem(int id, int userId)
        {
            var DBFinancial =(await _uow.FinancialRepo.FindAsync(u => u.PK_Item_Id == id)).FirstOrDefault();
            if (DBFinancial != null)
            {
                DBFinancial.IsDeleted = true;
                DBFinancial.FK_FinancialItems_Users_ModidfiedBy = userId;
                _uow.FinancialRepo.Update(DBFinancial);

            }

            return await _uow.SaveAsync() > 0;
        }

        public async Task<FinancialItemsDto> FindByID(int id)
        {
            var financial =(await _uow.FinancialRepo.FindAsync(u => u.PK_Item_Id == id)).FirstOrDefault();
            return (financial != null) ? Mapper.Map<tbl_FinancialItems,FinancialItemsDto>(financial) : new FinancialItemsDto();
        }

        public async Task<List<FinancialItemsDto>> GetFinancialItems( bool IsExpenses)
        {
            var Financial = await  _uow.FinancialRepo.FindAsync(u => u.IsDeleted == false&&u.IsExpenses==IsExpenses);
            if (Financial.Any() && Financial != null)
            {
                return Mapper.Map<List<tbl_FinancialItems>, List<FinancialItemsDto>>(Financial.ToList());
            }
            return new List<FinancialItemsDto>();
        }

        public async Task<bool> SaveFinancialItem(FinancialItemsDto financialItem, int userId)
        {
            if (financialItem.PK_Item_Id == 0)
            {
                var newFinancial = Mapper.Map<FinancialItemsDto, tbl_FinancialItems>(financialItem);
                newFinancial.FK_FinancialItems_Users_CreatedBy = userId;
                newFinancial.FK_FinancialItems_Users_ModidfiedBy = userId;
                _uow.FinancialRepo.Add(newFinancial);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateFinancialItem(FinancialItemsDto financialItem, int userId)
        {

            var DBFinancial =(await _uow.FinancialRepo.FindAsync(u => u.PK_Item_Id == financialItem.PK_Item_Id)).FirstOrDefault();
            if (DBFinancial != null)
            {
                DBFinancial.Description = financialItem.Description;
                DBFinancial.Date = financialItem.Date;
                DBFinancial.FK_FinancialItems_Users_ModidfiedBy = userId;
                DBFinancial.Amount = financialItem.Amount;
                DBFinancial.IsExpenses = financialItem.IsExpenses;              
                _uow.FinancialRepo.Update(DBFinancial);

            }
            return await _uow.SaveAsync() > 0;
        }
    }
}
