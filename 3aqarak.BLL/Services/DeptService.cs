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
    public class DeptService : IDeptService
    {

        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;

        public DeptService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;
        }

        public async Task<bool> DeleteDept(int id, int userId)
        {
            var DBDept =(await _uow.DeptRepo.FindAsync(u => u.PK_Departement_Id == id)).FirstOrDefault();
            if (DBDept != null)
            {
                DBDept.IsDeleted = true;
                DBDept.FK_Depts_Users_ModidfiedBy = userId;
                _uow.DeptRepo.Update(DBDept);

            }

            return await _uow.SaveAsync() > 0;
        }

        

        public async Task<DeptDto> FindByID(int id)
        {
            var Dept =(await _uow.DeptRepo.FindAsync(u => u.PK_Departement_Id == id)).FirstOrDefault();
            return (Dept != null) ? Mapper.Map<tbl_Departements, DeptDto>(Dept) : new DeptDto();
        }

        public async Task<List<DeptDto>> GetDepts()
        {
            var Dept =(await  _uow.DeptRepo.GetAllAsync()).Where(u => u.IsDeleted == false);
            if (Dept.Any() && Dept != null)
            {
                return Mapper.Map<List<tbl_Departements>, List<DeptDto>>(Dept.ToList());
            }
            return new List<DeptDto>();
        }

        public async Task<IConfirmation> SaveDept(DeptDto Dept, int userId)
        {
            if (Dept.PK_Departement_Id == 0)
            {
                var exists =(await _uow.DeptRepo.FindAsync(t => t.RegCode == Dept.RegCode && !t.IsDeleted)).Any();
                if (exists)
                {
                    _conf.Valid = false;
                    _conf.Message = "لايمكن تكرار نفس الكود مرتين";
                    return _conf;
                }
                var newDept = Mapper.Map<DeptDto, tbl_Departements>(Dept);
                newDept.FK_Depts_Users_CreatedBy = userId;
                newDept.FK_Depts_Users_ModidfiedBy = userId;
                newDept.CreatedAt = DateTime.UtcNow.AddMinutes(120);
                newDept.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                _uow.DeptRepo.Add(newDept);
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

        public async Task<IConfirmation> UpdateDept(DeptDto Dept, int userId)
        {
            var DBDept =(await _uow.DeptRepo.FindAsync(u => u.PK_Departement_Id == Dept.PK_Departement_Id)).FirstOrDefault();
            if (DBDept != null)
            {
                var exists =(await  _uow.DeptRepo.FindAsync(t => t.RegCode == Dept.RegCode && !t.IsDeleted && t.PK_Departement_Id != DBDept.PK_Departement_Id)).Any();
                if (exists)
                {
                    _conf.Valid = false;
                    _conf.Message = "لايمكن تكرار نفس الكود مرتين";
                    return _conf;
                }
                DBDept.Name = Dept.Name;
                DBDept.FK_Depts_Users_ModidfiedBy = userId;
                DBDept.FK_Depts_Users_MgrId = Dept.FK_Depts_Users_MgrId;
                DBDept.RegCode = Dept.RegCode;
                _uow.DeptRepo.Update(DBDept);
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
