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
    public class CatService : ICatService
    {

        private readonly IUnitOfWork _uow;

        public CatService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> DeleteCat(int id,int userId)
        {
            var DBCat =(await _uow.CatRepo.FindAsync(u => u.PK_Categories_Id == id)).FirstOrDefault();
            if (DBCat != null)
            {
                DBCat.IsDeleted = true;
                DBCat.FK_Categories_Users_ModidfiedBy = userId;
                _uow.CatRepo.Update(DBCat);

            }

            return await _uow.SaveAsync() > 0;
        }

        public async Task<CatDto> FindByID(int id)
        {
            var cat =(await _uow.CatRepo.FindAsync(u => u.PK_Categories_Id == id)).FirstOrDefault();
            return (cat != null) ? Mapper.Map<tbl_Categories, CatDto>(cat) : new CatDto();
        }

        public async Task<List<CatDto>> GetCats()
        {
            var Cat =await  _uow.CatRepo.FindAsync(u => u.IsDeleted == false);
            if (Cat.Any() && Cat != null)
            {
                return Mapper.Map<List<tbl_Categories>, List<CatDto>>(Cat.ToList());
            }
            return new List<CatDto>();
        }

        public async Task<bool> SaveCat(CatDto cat, int userId)
        {
            if (cat.PK_Categories_Id == 0)
            {
                var newCat = Mapper.Map<CatDto, tbl_Categories>(cat);
                newCat.FK_Categories_Users_CreatedBy = userId;
                newCat.FK_Categories_Users_ModidfiedBy = userId;
                newCat.CreatedAt = DateTime.UtcNow.AddMinutes(120);
                newCat.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                _uow.CatRepo.Add(newCat);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateCat(CatDto cat, int userId)
        {
            var DBCat =(await _uow.CatRepo.FindAsync(u => u.PK_Categories_Id == cat.PK_Categories_Id)).FirstOrDefault();
            if (DBCat != null)
            {
                DBCat.CategoryName = cat.CategoryName;
                DBCat.FK_Categories_Users_ModidfiedBy = userId;
                _uow.CatRepo.Update(DBCat);

            }

            return await _uow.SaveAsync() > 0;
        }
    }
        
}
