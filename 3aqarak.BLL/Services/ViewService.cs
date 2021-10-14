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
    public class ViewService : IViewService
    {
        private readonly IUnitOfWork _uow;

        public ViewService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> DeleteView(int id, int userId)
        {
            var DBView =(await _uow.ViewsRepo.FindAsync(u => u.PK_Views_Id== id)).FirstOrDefault();
            if (DBView != null)
            {
                DBView.IsDeleted = true;
                DBView.FK_Views_Users_ModidfiedBy = userId;
                _uow.ViewsRepo.Update(DBView);

            }

            return await _uow.SaveAsync() > 0;
        }

        public async Task<ViewDto> FindByID(int id)
        {
            var Views =(await _uow.ViewsRepo.FindAsync(u => u.PK_Views_Id == id)).FirstOrDefault();
            return (Views != null) ? Mapper.Map<tbl_Views, ViewDto>(Views) : new ViewDto();
        }

        public async Task<List<ViewDto>> GetViews()
        {
            var views = await _uow.ViewsRepo.FindAsync(u => u.IsDeleted == false);
            if (views.Any() && views != null)
            {
                return Mapper.Map<List<tbl_Views>, List<ViewDto>>(views.ToList());
            }
            return new List<ViewDto>();
        }

        public async Task<bool> SaveView(ViewDto view, int userId)
        {
            if (view.PK_Views_Id == 0)
            {
                var newView = Mapper.Map<ViewDto, tbl_Views>(view);
                newView.FK_Views_Users_CreatedBy = userId;
                newView.FK_Views_Users_ModidfiedBy = userId;
                _uow.ViewsRepo.Add(newView);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateView(ViewDto view, int userId)
        {
            var DBView =(await _uow.ViewsRepo.FindAsync(u => u.PK_Views_Id == view.PK_Views_Id)).FirstOrDefault();
            if (DBView != null)
            {
                DBView.Name = view.Name;
                DBView.FK_Views_Users_ModidfiedBy = userId;
                _uow.ViewsRepo.Update(DBView);

            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
