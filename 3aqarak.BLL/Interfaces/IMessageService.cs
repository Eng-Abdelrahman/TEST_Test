using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IMessageService
    {
        Task<List<MessagesDto>> GetMessage(DateTime? from, DateTime? to, int userReceverId);
        Task<IConfirmation> SaveMessage(MessagesDto Message);
        Task<IConfirmation> UpdateMessage(MessagesDto Message);
        Task<MessagesDto> EditMessage(int id);
        Task<IConfirmation> CheckMessage(int Id);
        Task<IConfirmation> CheckIsReadMessage(int Id);

    }
}
