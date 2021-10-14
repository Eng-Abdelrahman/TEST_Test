using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface ICallService
    {
        Task<bool> SaveClientCall(ClientCallDto callDto,int userId);

        Task<bool> SavePostponedCall(PostponedCallDto postDto,int userId);

        Task<bool> ClosePostponedCall(int callId, int userId);

    }
}
