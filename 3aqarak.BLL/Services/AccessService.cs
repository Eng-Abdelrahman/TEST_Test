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
    public class AccessService : IAccessService
    {
        private readonly IUnitOfWork _uow;
      

        public AccessService(IUnitOfWork uow)
        {
            _uow = uow;
          
        }    

        public async Task<bool> DeleteAccess(int id,int userId)
        {
            var DBAccess =(await _uow.AcssRepo.FindAsync(u => u.PK_Accessories_Id == id)).FirstOrDefault();
            if (DBAccess != null)
            {
                DBAccess.IsDeleted = true;
                DBAccess.FK_Accessories_Users_ModidfiedBy = userId;
                _uow.AcssRepo.Update(DBAccess);

            }

            return await _uow.SaveAsync() > 0;
        }

        public async Task<AccessDto> FindByID(int id)
        {
            var access = (await _uow.AcssRepo.FindAsync(u => u.PK_Accessories_Id == id)).FirstOrDefault();
            return (access != null) ? Mapper.Map<tbl_Accessories, AccessDto>(access) : new AccessDto();
        }

        public async Task<List<AccessDto>> GetAccessoriesAsync()
        {
            var Access = (await _uow.AcssRepo.GetAllAsync()).Where(u => u.IsDeleted == false);
            if (Access.Any() && Access != null)
            {
                return Mapper.Map<List<tbl_Accessories>, List<AccessDto>>(Access.ToList());
            }
            return new List<AccessDto>();
        }

        public async Task<bool> SaveAccess(AccessDto access, int userId)
        {
            if (access.PK_Accessories_Id == 0)
            {               
                var newAccess = Mapper.Map<AccessDto, tbl_Accessories>(access);
                newAccess.FK_Accessories_Users_CreatedBy = userId;
                newAccess.FK_Accessories_Users_ModidfiedBy = userId;
                _uow.AcssRepo.Add(newAccess);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async  Task<bool> UpdateAccess(AccessDto access, int userId)
        {
            var DBAccess = (await _uow.AcssRepo.FindAsync(u => u.PK_Accessories_Id == access.PK_Accessories_Id)).FirstOrDefault();
            if (DBAccess != null)
            {
                DBAccess.Name = access.Name;
                DBAccess.FK_Accessories_Users_ModidfiedBy = userId;
                _uow.AcssRepo.Update(DBAccess);

            }

            return await _uow.SaveAsync() > 0;
        }

      
    }
}
