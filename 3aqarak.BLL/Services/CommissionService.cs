using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3aqarak.BLL.Dto;
using System.Web.Mvc;
using AutoMapper;
using _3aqarak.BLL.Models;

namespace _3aqarak.BLL.Services
{
    public class CommissionService : ICommissionsService
    {
        private readonly IUnitOfWork _uow;


        public CommissionService (IUnitOfWork uow)
        {
            _uow = uow;

        }
        public async Task<bool> DeleteCommission(int id, int userId)
        {
            var DBComm =(await _uow.CommissionsRepo.FindAsync(u => u.PK_Commissions_Id == id)).FirstOrDefault();
            if (DBComm != null)
            {
                DBComm.IsDeleted = true;
                DBComm.FK_Commissions_users_ModidfiedBy = userId;
                _uow.CommissionsRepo.Update(DBComm);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<CommissionsDto> FindByID(int id)
        {
            var commission =(await _uow.CommissionsRepo.FindAsync(u => u.PK_Commissions_Id == id)).FirstOrDefault();
            return (commission != null) ? Mapper.Map<tbl_Commissions, CommissionsDto>(commission) : new CommissionsDto();
        }

        public async Task<SelectList> GetCatList()
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.FindAsync(c=>!c.IsDeleted)).ToList()), "PK_Categories_Id", "CategoryName");
        }

        public async Task<SelectList> GetCatListById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.FindAsync(c => !c.IsDeleted)).ToList()), "PK_Categories_Id", "CategoryName", await _uow.CatRepo.FindAsync(c => c.PK_Categories_Id == id));
        }

        public async Task<List<CommissionsDto>> GetCommissions()
        {
            var commissions = await _uow.CommissionsRepo.FindAsync(u => u.IsDeleted == false);
            if (commissions.Any() && commissions != null)
            {
                return Mapper.Map<List<tbl_Commissions>, List<CommissionsDto>>(commissions.ToList());
            }
            return new List<CommissionsDto>();
        }

        public async Task<SelectList> GetTransList()
        {
            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(c => !c.IsDeleted)).ToList()), "PK_Transactions_Id", "TransType");
        }

        public async Task<SelectList> GetTransListById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(c => !c.IsDeleted)).ToList()), "PK_Transactions_Id", "TransType",(await  _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == id)).ToList());
        }

        public async Task<bool> SaveCommission(CommissionsDto commission, int userId)
        {

            if (commission.PK_Commissions_Id == 0)
            {
                var newComm = Mapper.Map<CommissionsDto, tbl_Commissions>(commission);
                newComm.FK_Commissions_users_CreatedBy = userId;
                newComm.FK_Commissions_users_ModidfiedBy = userId;
                newComm.CreatedAt = DateTime.UtcNow.AddMinutes(120);
                newComm.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                _uow.CommissionsRepo.Add(newComm);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateCommission(CommissionsDto commission, int userId)
        {
            var DBComm = (await _uow.CommissionsRepo.FindAsync(u => u.PK_Commissions_Id == commission.PK_Commissions_Id)).FirstOrDefault();
            if (DBComm != null)
            {
                DBComm.FK_Commissions_users_ModidfiedBy = userId;
                DBComm.TelesalesComission = commission.TelesalesComission;
                DBComm.SalesComission = commission.SalesComission;
                DBComm.MgrCommission = commission.MgrCommission;
                DBComm.FK_Commissions_Categories_Id = commission.FK_Commissions_Categories_Id;
                DBComm.FK_Commissions_Transactions_Id = commission.FK_Commissions_Transactions_Id;
                DBComm.FK_Commissions_users_ModidfiedBy = userId;
                DBComm.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                _uow.CommissionsRepo.Update(DBComm);

            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
