using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;

namespace _3aqarak.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpServerUtilityBase _server;
        private readonly HttpContextBase _context;
        private readonly HttpSessionStateBase _session;
        public MessageService(IUnitOfWork uow, IConfirmation conf, HttpServerUtilityBase server, HttpContextBase context, HttpSessionStateBase session)
        {
            _uow = uow;
            _conf = conf;
            _server = server;
            _session = session;
            _context = context;
        }
        public async Task<List<MessagesDto>> GetMessage(DateTime? from, DateTime? to, int userReceverId)
        {
            List<tbl_Messages> Message = new List<tbl_Messages>();
            if (from != null && to != null)
            {
                if (userReceverId > 0)
                {
                    Message = (await _uow.MessagesRepo.FindAsync(u => (u.DateTime >= from && u.DateTime <= to) && u.FK_Messages_Users_RecieverId == userReceverId)).ToList();

                }
                else
                {
                    Message = (await _uow.MessagesRepo.FindAsync(u => u.DateTime >= from && u.DateTime <= to)).ToList();

                }
            }
            else
            {
                if (userReceverId > 0)
                {
                    Message = (await _uow.MessagesRepo.FindAsync(u => u.FK_Messages_Users_RecieverId == userReceverId)).ToList();

                }
                else
                {
                    Message = (await _uow.MessagesRepo.GetAllAsync()).ToList();

                }
            }
            if (Message.Any() && Message != null)
            {

                return Mapper.Map<List<tbl_Messages>, List<MessagesDto>>(Message.ToList());
            }
            return new List<MessagesDto>();
        }


        /// <summary>
        /// this function for save new message
        /// </summary>
        /// <param name="Message">take data from viewModal</param>
        /// <returns> boolian </returns>
        public async Task<IConfirmation> SaveMessage(MessagesDto Message)
        {
            var NewMessage = Mapper.Map<MessagesDto, tbl_Messages>(Message);
            NewMessage.DateTime = DateTime.UtcNow.AddMinutes(120);
            _uow.MessagesRepo.Add(NewMessage);
            _conf.Valid = await _uow.SaveAsync() > 0;
            if (!_conf.Valid)
            {
                _conf.Message = "حدث خطأ أثناء إرسال الرساله الرجاء المحاوله مره أخرى!";
                return _conf;
            }
            _conf.Message = "تم الحفظ بنجاح!";

            return _conf;
        }
       

        /// <summary>
        /// this function for get Message date to edit it 
        /// </summary>
        /// <param name="id">Pk_Message_Id</param>
        /// <returns>MessagesDto</returns>
        public async Task<MessagesDto> EditMessage(int id)
        {
            List<MessagesDto> Message = Mapper.Map<List<tbl_Messages>, List<MessagesDto>>((await _uow.MessagesRepo.FindAsync
               (a => a.PK_Messages_Id == id )).ToList());

            if (Message.Any() && Message != null)
            {
                MessagesDto MessageList = Message.FirstOrDefault();

                return MessageList;
            }
            return new MessagesDto();
        }

        /// <summary>
        /// this function for update message
        /// </summary>
        /// <param name="Message">take data from viewModal</param>
        /// <returns>boolian</returns>
        public async Task<IConfirmation> UpdateMessage(MessagesDto Message)
        {
            if (Message.PK_Messages_Id > 0)
            {
                tbl_Messages DBMessage = (await _uow.MessagesRepo.FindAsync(a => a.PK_Messages_Id == Message.PK_Messages_Id)).FirstOrDefault();
                if (DBMessage != null)
                {
                    DBMessage.FK_Messages_Users_RecieverId = Message.FK_Messages_Users_RecieverId;
                    DBMessage.DateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                    DBMessage.DateTimeStart = Message.DateTimeStart;
                    DBMessage.DateTimeEnd = Message.DateTimeEnd;
                    DBMessage.MessageContent = Message.MessageContent;
                    DBMessage.IsCritical = Message.IsCritical;
                    DBMessage.Title = Message.Title;
                }
                _uow.MessagesRepo.Update(DBMessage);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "حدث خطأ أثناء تعديل البيانات الرجاء المحاوله مره أخرى";
                    return _conf;
                }
                _conf.Message = "تم الحفظ بنجاح!";
                return _conf;
            }
            _conf.Valid = false;
            _conf.Message = "لم يتم الحفظ حدث خطأ ما!";
            return _conf;

        }

        /// <summary>
        /// this function for Check message Is done
        /// </summary>
        /// <param name="Id">take Id</param>
        /// <returns>boolian</returns>
        public async Task<IConfirmation> CheckMessage(int Id)
        {
            if (Id > 0)
            {
                tbl_Messages DBMessage = (await _uow.MessagesRepo.FindAsync(a => a.PK_Messages_Id == Id)).FirstOrDefault();
                if (DBMessage != null)
                {
                    DBMessage.IsDone = true;
                    
                }
                _uow.MessagesRepo.Update(DBMessage);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "حدث خطأ أثناء تعديل البيانات الرجاء المحاوله مره أخرى";
                    return _conf;
                }
                _conf.Message = "تم المهمه بنجاح!";
                return _conf;
            }
            _conf.Valid = false;
            _conf.Message = "لم يتم الحفظ حدث خطأ ما!";
            return _conf;

        }

        /// <summary>
        /// Check IsRead Message
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>boolian</returns>
        public async Task<IConfirmation> CheckIsReadMessage(int Id)
        {
            if (Id > 0)
            {
                tbl_Messages DBMessage = (await _uow.MessagesRepo.FindAsync(a => a.PK_Messages_Id == Id)).FirstOrDefault();
                if (DBMessage != null)
                {
                    DBMessage.IsRead = true;

                }
                _uow.MessagesRepo.Update(DBMessage);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "حدث خطأ أثناء تعديل البيانات الرجاء المحاوله مره أخرى";
                    return _conf;
                }
                _conf.Message = "تم القراءه بنجاح!";
                return _conf;
            }
            _conf.Valid = false;
            _conf.Message = "لم يتم الحفظ حدث خطأ ما!";
            return _conf;

        }
    }
}
