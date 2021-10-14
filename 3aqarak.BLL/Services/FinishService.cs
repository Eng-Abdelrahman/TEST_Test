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
    public class FinishService : IFinishService
    {

        private readonly IUnitOfWork _uow;
        private  IConfirmation _conf;

        public FinishService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;
        }

        public async Task<IConfirmation> CheckIsMaster(FinishingDto Finish)
        {
            if (Finish.PK_Finishings_Id == 0)
            {
                var exists = (await _uow.FinishRepo.FindAsync(f => f.IsMaster && !f.IsDeleted)).Any();
                if (exists)
                {
                    _conf.Valid = false;
                    _conf.Message = "لايمكن تكرار الصنف الرئيسي مرتين";
                }
                else
                {
                    _conf.Valid = true;
                }
            }
            else
            {
                var DBfinish =(await _uow.FinishRepo.FindAsync(f => f.PK_Finishings_Id == Finish.PK_Finishings_Id)).FirstOrDefault();
                if (DBfinish != null)
                {
                    var exists = (await _uow.FinishRepo.FindAsync(f => f.IsMaster && !f.IsDeleted && f.PK_Finishings_Id != DBfinish.PK_Finishings_Id)).Any();
                    if (exists)
                    {
                        _conf.Valid = false;
                        _conf.Message = "لايمكن تكرار الصنف الرئيسي مرتين";
                    }
                    else
                    {
                        _conf.Valid = true;
                    }
                }
                else
                {
                    _conf.Valid = false;
                    _conf.Message = "البيان غير موجود في قاعدة البيانات";
                }
            }
            return _conf;
        }

        public async Task<bool> DeleteFinish(int id, int userId)
        {
            var DBFinish =(await _uow.FinishRepo.FindAsync(u => u.PK_Finishings_Id == id)).FirstOrDefault();
            if (DBFinish != null)
            {
                DBFinish.IsDeleted = true;
                DBFinish.FK_Finishings_Users_ModidfiedBy = userId;
                _uow.FinishRepo.Update(DBFinish);

            }

            return await _uow.SaveAsync() > 0;
        }

        public async Task<FinishingDto> FindByID(int id)
        {
            var Finish =(await _uow.FinishRepo.FindAsync(u => u.PK_Finishings_Id == id)).FirstOrDefault();
            return (Finish != null) ? Mapper.Map<tbl_Finishings, FinishingDto>(Finish) : new FinishingDto();
        }

        public async Task<List<FinishingDto>> GetFinishings()
        {
            var Finish =await  _uow.FinishRepo.FindAsync(u => u.IsDeleted == false);
            if (Finish.Any() && Finish != null)
            {
                return Mapper.Map<List<tbl_Finishings>, List<FinishingDto>>(Finish.ToList());
            }
            return new List<FinishingDto>();
        }

        public async Task<bool> SaveFinish(FinishingDto Finish, int userId)
        {
            if (Finish.PK_Finishings_Id == 0)
            {
                var newFinish = Mapper.Map<FinishingDto, tbl_Finishings>(Finish);
                newFinish.FK_Finishings_Users_CreatedBy = userId;
                newFinish.FK_Finishings_Users_ModidfiedBy = userId;
                _uow.FinishRepo.Add(newFinish);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateFinish(FinishingDto Finish, int userId)
        {
            var DBFinish =(await _uow.FinishRepo.FindAsync(u => u.PK_Finishings_Id == Finish.PK_Finishings_Id)).FirstOrDefault();
            if (DBFinish != null)
            {
                DBFinish.Type = Finish.Type;
                DBFinish.FK_Finishings_Users_ModidfiedBy = userId;
                DBFinish.IsMaster = Finish.IsMaster;
                _uow.FinishRepo.Update(DBFinish);

            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
