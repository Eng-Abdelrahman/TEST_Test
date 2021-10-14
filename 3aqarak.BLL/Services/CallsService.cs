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
    public class CallsService : ICallService
    {
        private readonly IUnitOfWork _uow;

        public CallsService(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<bool> SaveClientCall(ClientCallDto callDto, int userId)
        {
            if (callDto.PK_MarketingCalls_Id == 0)
            {
                var newCall = Mapper.Map<ClientCallDto, tbl_ClientsCalls>(callDto);
                newCall.FK_MarketingCalls_Users_CreatedBy = userId;
                newCall.FK_MarketingCalls_Users_ModidfiedBy = userId;
                newCall.FK_MarketingCalls_Users_EmpolyeeId = userId;
                newCall.CreatedAt = DateTime.UtcNow.AddMinutes(120);
                newCall.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                _uow.ClienCallRepo.Add(newCall);
            }
            return await _uow.SaveAsync().ConfigureAwait(false) > 0;
        }


        public async Task<bool> SavePostponedCall(PostponedCallDto postDto, int userId)
        {
            if (postDto.PK_PostbonedCalls == 0)
            {
                var newCall = Mapper.Map<PostponedCallDto, tbl_PostbonedCalls>(postDto);
                newCall.FK_MarketingCalls_Users_EmpolyeeId= userId;
                newCall.FK_PostbonedCalls_Users_ModidfiedBy = userId;
                newCall.FK_PostbonedCalls_Users_CreatedBy = userId;
                newCall.CreatedAt = DateTime.UtcNow.AddMinutes(120);
                newCall.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                _uow.PostponedRepo.Add(newCall);
            }
            return await _uow.SaveAsync().ConfigureAwait(false) > 0;
        }

        public async Task<bool> ClosePostponedCall(int callId, int userId)
        {
            var postCall =(await _uow.PostponedRepo.FindAsync(p => p.PK_PostbonedCalls == callId).ConfigureAwait(false)).FirstOrDefault();
            if (postCall!=null)
            {
                postCall.IsDone = true;
                postCall.FK_PostbonedCalls_Users_ModidfiedBy = userId;
                postCall.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                _uow.PostponedRepo.Update(postCall);
            }
            return await _uow.SaveAsync().ConfigureAwait(false) > 0;
        }


    }
}
