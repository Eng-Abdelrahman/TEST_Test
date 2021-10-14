using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Services
{
    

    public class TransService : ITransService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;

        public TransService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;
        }

        public async Task<bool> DeleteTrans(int id, int userId)
        {
            var DBTrans = (await _uow.TransRepo.FindAsync(u => u.PK_Transactions_Id == id)).FirstOrDefault();
            if (DBTrans != null)
            {
                DBTrans.IsDeleted = true;
                DBTrans.FK_Transactions_Users_ModidfiedBy = userId;
                _uow.TransRepo.Update(DBTrans);

            }

            return await _uow.SaveAsync() > 0;
        }

        public async Task<TransDto> FindByID(int id)
        {
            var Trans = (await _uow.TransRepo.FindAsync(u => u.PK_Transactions_Id == id)).FirstOrDefault();
            return (Trans != null) ? Mapper.Map<tbl_Transactions, TransDto>(Trans) : new TransDto();
        }



        public async Task<List<TransDto>> GetTrans()
        {
            var Trans = await _uow.TransRepo.FindAsync(u => u.IsDeleted == false);
            if (Trans.Any() && Trans != null)
            {
                return Mapper.Map<List<tbl_Transactions>, List<TransDto>>(Trans.ToList());
            }
            return new List<TransDto>();
        }

        public async Task<IConfirmation> SaveTrans(TransDto Trans, int userId)
        {
            if (Trans.PK_Transactions_Id == 0)
            {
                var exists = (await _uow.TransRepo.GetAllAsync()).ToList().Exists(t => t.TransCode == Trans.TransCode && !t.IsDeleted);
                if (exists)
                {
                    _conf.Valid = false;
                    _conf.Message = "لايمكن تكرار نفس الكود مرتين";
                    return _conf;
                }
                var newTrans = Mapper.Map<TransDto, tbl_Transactions>(Trans);
                newTrans.FK_Transactions_Users_CreatedBy = userId;
                newTrans.FK_Transactions_Users_ModidfiedBy = userId;
                newTrans.CreatedAt = DateTime.UtcNow.AddMinutes(120);
                newTrans.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                _uow.TransRepo.Add(newTrans);
                _conf.Valid = await _uow.SaveAsync() > 0;

            }
            if (_conf.Valid)
            {
                _conf.Message = "تم الحفظ بنجاح!";
            }
            else
            {
                _conf.Message = "لم يتم الحفظ بنجاح!";
            }

            return _conf;
        }

        public async Task<IConfirmation> UpdateTrans(TransDto Trans, int userId)
        {
            var DBTrans = (await _uow.TransRepo.FindAsync(u => u.PK_Transactions_Id == Trans.PK_Transactions_Id)).FirstOrDefault();
            if (DBTrans != null)
            {
                var exists = (await _uow.TransRepo.GetAllAsync()).ToList().Exists(t => t.TransCode == Trans.TransCode && t.PK_Transactions_Id != DBTrans.PK_Transactions_Id && !t.IsDeleted);
                if (exists)
                {
                    _conf.Valid = false;
                    _conf.Message = "لايمكن تكرار نفس الكود مرتين";
                    return _conf;
                }
                DBTrans.TransType = Trans.TransType;
                DBTrans.FK_Transactions_Users_ModidfiedBy = userId;
                DBTrans.TransCode = Trans.TransCode;
                _uow.TransRepo.Update(DBTrans);
                _conf.Valid = await _uow.SaveAsync() > 0;
            }

            if (_conf.Valid)
            {
                _conf.Message = "تم الحفظ بنجاح!";
            }
            else
            {
                _conf.Message = "لم يتم الحفظ بنجاح!";
            }
            return _conf;
        }
    }
}
