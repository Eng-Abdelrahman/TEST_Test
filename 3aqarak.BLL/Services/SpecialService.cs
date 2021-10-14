using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3aqarak.BLL.Dto;
using AutoMapper;
using _3aqarak.BLL.Models;
using System.Web.Mvc;

namespace _3aqarak.BLL.Services
{
    public class SpecialService : ISpecialService
    {
        private readonly IUnitOfWork _uow;

        public SpecialService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<bool> DeleteSpecial(int id, int userId)
        {
            var DBSpecial =(await _uow.SpecialRepo.FindAsync(u => u.PK_Specialization_Id == id)).FirstOrDefault();
            if (DBSpecial != null)
            {
                DBSpecial.IsDeleted = true;
                DBSpecial.FK_Specialization_Users_ModidfiedBy = userId;
                _uow.SpecialRepo.Update(DBSpecial);

            }

            return await _uow.SaveAsync() > 0;
        }

        public async Task<SpecialDto> FindByID(int id)
        {
            var Special =(await _uow.SpecialRepo.FindAsync(u => u.PK_Specialization_Id == id)).FirstOrDefault();
            return (Special != null) ? Mapper.Map<tbl_Specializations, SpecialDto>(Special) : new SpecialDto();
        }

        public async Task<SelectList> GetDepts()
        {
            return new SelectList((await _uow.DeptRepo.GetAllAsync()).Where(d => !d.IsDeleted), "PK_Departement_Id", "Name");
        }
     
        public async Task<SelectList> GetDeptsById(int deptId)
        {
            return new SelectList((await _uow.DeptRepo.GetAllAsync()).Where(d => !d.IsDeleted), "PK_Departement_Id", "Name",await _uow.DeptRepo.FindAsync(d => !d.IsDeleted&&d.PK_Departement_Id==deptId));
        }

        public async Task<List<SpecialDto>> GetSpecials(int deptId=0)
        {
            var specials = new List<tbl_Specializations>();
            if (deptId>0)
            {
                 specials =(await _uow.SpecialRepo.FindAsync(u => u.IsDeleted == false && u.FK_Specialization_Dept_DeptId == deptId)).ToList(); 
            }          
            if (specials.Any() && specials != null)
            {
                return Mapper.Map<List<tbl_Specializations>, List<SpecialDto>>(specials);
            }
            return new List<SpecialDto>();
        }

        public async Task<bool> SaveSpecial(SpecialDto Special, int userId)
        {
            if (Special.PK_Specialization_Id == 0)
            {
                var newSpecial = Mapper.Map<SpecialDto, tbl_Specializations>(Special);
                newSpecial.FK_Specialization_Users_CreatedBy = userId;
                newSpecial.FK_Specialization_Users_ModidfiedBy = userId;
                _uow.SpecialRepo.Add(newSpecial);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateSpecial(SpecialDto Special, int userId)
        {
            var DBSpecial =(await _uow.SpecialRepo.FindAsync(u => u.PK_Specialization_Id == Special.PK_Specialization_Id)).FirstOrDefault();
            if (DBSpecial != null)
            {
                DBSpecial.Name = Special.Name;
                DBSpecial.FK_Specialization_Users_ModidfiedBy = userId;
                DBSpecial.FK_Specialization_Dept_DeptId = Special.FK_Specialization_Dept_DeptId;
                _uow.SpecialRepo.Update(DBSpecial);

            }

            return await _uow.SaveAsync() > 0;
        }

    
    }
}
