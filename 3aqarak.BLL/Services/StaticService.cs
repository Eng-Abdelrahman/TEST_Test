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
    

    public class StaticService : IStaticService
    { 
        private readonly IUnitOfWork _uow;


        public StaticService(IUnitOfWork uow)
        {
            _uow = uow;

        }
        public async Task<bool> DeleteTemplate(int id, int userId)
        {
            var DBTemplate = (await _uow.STContRepo.FindAsync(u => u.PK_StatContract_Id == id)).FirstOrDefault();
            if (DBTemplate != null)
            {
                DBTemplate.IsDeleted = true;
                DBTemplate.FK_StatContract_Users_ModidfiedBy = userId;
                _uow.STContRepo.Update(DBTemplate);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<StaticDto> FindTemplateByID(int id)
        {
            var template = (await _uow.STContRepo.FindAsync(u => u.PK_StatContract_Id == id)).FirstOrDefault();
            return (template != null) ? Mapper.Map<tbl_StaticContracts, StaticDto>(template) : new StaticDto();
        }

        public async Task<SelectList> GetCatList()
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.FindAsync(c => !c.IsDeleted)).ToList()), "PK_Categories_Id", "CategoryName");
        }

        public async Task<SelectList> GetCatListById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.FindAsync(c => !c.IsDeleted)).ToList()), "PK_Categories_Id", "CategoryName", (await _uow.CatRepo.FindAsync(c => c.PK_Categories_Id == id)));
        }

        public async Task<List<StaticDto>> GetTemplates()
        {
            var templates = await _uow.STContRepo.FindAsync(u => u.IsDeleted == false);
            if (templates.Any() && templates != null)
            {
                return Mapper.Map<List<tbl_StaticContracts>, List<StaticDto>>(templates.ToList());
            }
            return new List<StaticDto>();
        }

        public async Task<SelectList> GetTransList()
        {
            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(c => !c.IsDeleted)).ToList()), "PK_Transactions_Id", "TransType");
        }

        public async Task<SelectList> GetTransListById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(c => !c.IsDeleted)).ToList()), "PK_Transactions_Id", "TransType", await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == id));
        }

        public async Task<bool> SaveTemplate(StaticDto template, int userId)
        {
            if (template.PK_StatContract_Id == 0)
            {
                var newTemp = Mapper.Map<StaticDto, tbl_StaticContracts>(template);
                newTemp.FK_StatContract_Users_CreatedBy = userId;
                newTemp.FK_StatContract_Users_ModidfiedBy = userId;
                newTemp.FK_StaticContract_Transaction_Transid = template.FK_StaticContract_Transaction_Transid;
                newTemp.FK_StaticContract_Categories_CatId = template.FK_StaticContract_Categories_CatId;
                _uow.STContRepo.Add(newTemp);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateTemplate(StaticDto template, int userId)
        {
            var DBTemplate = (await _uow.STContRepo.FindAsync(u => u.PK_StatContract_Id == template.PK_StatContract_Id)).FirstOrDefault();
            if (DBTemplate != null)
            {
                DBTemplate.FK_StatContract_Users_ModidfiedBy = userId;
                DBTemplate.ContactContent = template.ContactContent;
                DBTemplate.FK_StaticContract_Categories_CatId = template.FK_StaticContract_Categories_CatId;
                DBTemplate.FK_StaticContract_Transaction_Transid = template.FK_StaticContract_Transaction_Transid;
                _uow.STContRepo.Update(DBTemplate);

            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
