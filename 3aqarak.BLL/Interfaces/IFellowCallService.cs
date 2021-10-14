using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IFellowCallService
    {

        Task<bool> SaveFellowupCall(FellowCallDto postDto, int userId);

        Task<bool> ConfirmCall(int callId, int userId);
        Task<bool> CancellCall(int callId, int userId);
    }
}
