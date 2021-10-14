using _3aqarak.BLL.Domain;
using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Services
{
    public class FellowupCallService : IFellowCallService
    {
        private readonly IUnitOfWork _uow;

        public FellowupCallService(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<bool> ConfirmCall(int callId, int userId)
        {
            var call =(await _uow.FellowupCallsRepo.FindAsync(c => c.PK_FellowupCalls_Id == callId)).FirstOrDefault();
            if (call == null) return false;
            call.IsDone = true;
            call.ModifiedAt= DateTime.UtcNow.AddMinutes(120);
            call.FK_FellowupCalls_Users_ModidfiedBy = userId;
           return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> CancellCall(int callId, int userId)
        {
            var call = (await _uow.FellowupCallsRepo.FindAsync(c => c.PK_FellowupCalls_Id == callId)).FirstOrDefault();
            if (call == null) return false;
            call.IsDeleted = true;
            call.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
            call.FK_FellowupCalls_Users_ModidfiedBy = userId;
            return await _uow.SaveAsync() > 0;
        }



        public async Task<bool> SaveFellowupCall(FellowCallDto postDto, int userId)
        {

           
                var newCall = Mapper.Map<FellowCallDto, tbl_FellowupCall>(postDto);
                newCall.FK_FellowupCalls_Users_EmpolyeeId = userId;
                newCall.FK_FellowupCalls_Users_ModidfiedBy = userId;
                newCall.FK_FellowupCalls_Users_CreatedBy = userId;
                newCall.CreatedAt = DateTime.UtcNow.AddMinutes(120);
                newCall.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                newCall.FK_ClientCalls_Clients_Id = postDto.ClientId;            
                _uow.FellowupCallsRepo.Add(newCall);

                return await _uow.SaveAsync().ConfigureAwait(false) > 0;
            }
           
        }
    }

