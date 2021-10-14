using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using _3aqarak.MVC.Areas.Dashboard.ViewModels;
using _3aqarak.MVC.Filters;

namespace _3aqarak.MVC.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IUSerService _userService;
        private readonly IClientService _clientService;
        private IConfirmation _conf;

        public MessageController(IConfirmation conf, IMessageService MessageService, IUSerService userService, IClientService clientService)
        {
            _messageService = MessageService;
            _userService = userService;
            _clientService = clientService;
            _conf = conf;
        }
        // GET: Message/Index
        public async Task<ActionResult> Index()
        {
            var MessageVM = new MessageViewModel()
            {
                User = await _userService.GetAllUsers(),
            };
            
            return View(MessageVM);
        }

        //Message/LoadData
        public async Task<ActionResult> LoadData()
        {
            DateTime? fromDate = new DateTime();
            DateTime? toDate = new DateTime();
            ViewModels.DataTableViewModel data = new ViewModels.DataTableViewModel
            {
                Draw = Request.Form.GetValues("draw").FirstOrDefault(),
                Start = Request.Form.GetValues("start").FirstOrDefault(),
                Length = Request.Form.GetValues("length").FirstOrDefault(),
                SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault(),
                SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault(),
                SearchValue = Request.Form.GetValues("search[value]").FirstOrDefault(),
            };
            if (!string.IsNullOrEmpty(Request.Form.GetValues("fromDate").FirstOrDefault()))
            {
                fromDate = DateTime.Parse(Request.Form.GetValues("fromDate").FirstOrDefault());
            }
            else
            {
                fromDate = null;
            }

            if (!string.IsNullOrEmpty(Request.Form.GetValues("toDate").FirstOrDefault()))
            {
                toDate = DateTime.Parse(Request.Form.GetValues("toDate").FirstOrDefault());
            }
            else
            {
                toDate = null;
            }
            var userReceverId = !string.IsNullOrEmpty(Request.Form["ReceverId"]) ? int.Parse(Request.Form["ReceverId"]) : 0;

            //Paging Size (10,20,50,100)
            data.PageSize = data.Length != null ? Convert.ToInt32(data.Length) : 0;
            data.Skip = data.Start != null ? Convert.ToInt32(data.Start) : 0;
            data.RecordsTotal = 0;
            ViewModels.DataTableViewModel tableData = await GetMessageTableData(data, fromDate, toDate, userReceverId);
            return Json(new
            {
                draw = tableData.Draw,
                recordsTotal = tableData.RecordsTotal,
                recordsFiltered = tableData.RecordsTotal,
                data = tableData.Message,
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<ViewModels.DataTableViewModel> GetMessageTableData(ViewModels.DataTableViewModel tableData, DateTime? from, DateTime? to, int userReceverId)
        {
            // Getting all entity data
            List<MessageViewModel> MessagesList = new List<MessageViewModel>();
            var Messages =await _messageService.GetMessage(from, to, userReceverId);
            foreach (var item in Messages)
            {
                MessagesList.Add(new MessageViewModel()
                {
                    PK_Messages_Id=item.PK_Messages_Id,
                    UserName = String.Concat((await _userService.FindUserByID(item.FK_Messages_Users_RecieverId)).FirstName, " ", (await _userService.FindUserByID(item.FK_Messages_Users_RecieverId)).LastName),
                    DateString = item.DateTime.ToShortDateString(),
                    MessageContent = item.MessageContent,
                    IsRead=item.IsRead,
                    IsDone=item.IsDone,
                    IsCritical=item.IsCritical,
                    DateTimeStartString = item.DateTimeStart.ToShortDateString(),
                    DateTimeEndString = item.DateTimeEnd.ToShortDateString(),
                    Title=item.Title,
                });;

            }
            //Search    
            if (!string.IsNullOrEmpty(tableData.SearchValue))
            {
                MessagesList = MessagesList.Where(e => e.DateString.Contains(tableData.SearchValue)).ToList();
            }

            //Sorting    
            if (!(string.IsNullOrEmpty(tableData.SortColumn) && string.IsNullOrEmpty(tableData.SortColumnDir)))
            {
                if (tableData.SortColumnDir == "asc")
                {
                    MessagesList = MessagesList.OrderBy(e => e.DateString).ToList();
                }
                else
                {
                    MessagesList = MessagesList.OrderByDescending(e => e.DateString).ToList();
                }

            }

            //total number of rows count     
            tableData.RecordsTotal = Messages.Count();

            //Paging
            Messages = Messages.Skip(tableData.Skip).Take(tableData.PageSize).ToList();

            tableData.Message = MessagesList;

            return tableData;
        }

        //Message/AddMessage
        public ActionResult AddMessage()
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            MessageViewModel Message = new MessageViewModel()
            {
                DateTime = DateTime.UtcNow.AddMinutes(120),
            };
            Message.FK_Messages_Users_SenderId = userId;
            return View();
        }

        //Message/SaveMessage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveMessage(MessageViewModel MessageVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            MessageVM.FK_Messages_Users_SenderId = userId;
            MessageVM.DateTime = DateTime.UtcNow.AddMinutes(120);
            if (MessageVM.DateTimeStart > MessageVM.DateTimeEnd)
            {
                return Json(new { valid = false, message = "تاريخ نهاية المهمه لابد ان يكون اكبر من بدايته!" }, JsonRequestBehavior.AllowGet);
            }
            if (MessageVM.MessageContent ==null)
            {
                return Json(new { valid = false, message = "لا بد من كتابة رساله للموظف!" }, JsonRequestBehavior.AllowGet);
            }
            if (MessageVM.Title == null)
            {
                return Json(new { valid = false, message = "لا بد من كتابة عنوان رساله للموظف!" }, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {

                MessagesDto MessageDto = Mapper.Map<MessageViewModel, MessagesDto>(MessageVM);

                    _conf = await _messageService.SaveMessage(MessageDto);    

            }
            if (_conf.Valid)
            {
                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);

        }
        //Message/EditMessage
        [DisableHtmlInputs]
        public async Task<ActionResult> EditMessage(string id)
        {
            if (HttpContext.Items["Disable"] != null && bool.Parse(HttpContext.Items["Disable"].ToString()))
            {
                ViewBag.Disable = true;
            }
            MessageViewModel Message = Mapper.Map<MessagesDto, MessageViewModel>(await _messageService.EditMessage(int.Parse(id)));
            int userId = ((UserDto)Session["User"]).PK_Users_Id;

            UserDto Reciever = (await _userService.FindUserByID(Message.FK_Messages_Users_RecieverId));
            ViewBag.UserName = Reciever.FirstName + " " + Reciever.LastName;
            
            return View(Message);
        }
        //Message/UpdateMessage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateMessage(MessageViewModel MesageVM)
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            MesageVM.FK_Messages_Users_SenderId = userId;
            if (MesageVM.DateTimeStart > MesageVM.DateTimeEnd)
            {
                return Json(new { valid = false, message = "تاريخ نهاية المهمه لابد ان يكون اكبر من بدايته!" }, JsonRequestBehavior.AllowGet);
            }
            if (MesageVM.MessageContent == null)
            {
                return Json(new { valid = false, message = "لا بد من كتابة رساله للموظف!" }, JsonRequestBehavior.AllowGet);
            }
            if (MesageVM.Title == null)
            {
                return Json(new { valid = false, message = "لا بد من كتابة عنوان رساله للموظف!" }, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                
                _conf = await _messageService.UpdateMessage(Mapper.Map<MessageViewModel, MessagesDto>(MesageVM));
            }
            if (_conf.Valid)
            {
               
                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        //Message/CheckMessage
        [HttpPost]
        public async Task<ActionResult> CheckMessage(int Id)
        {

            if (ModelState.IsValid)
            {

                _conf = await _messageService.CheckMessage(Id);
            }
            if (_conf.Valid)
            {

                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        //Message/CheckIsReadMessage
        [HttpPost]
        public async Task<ActionResult> CheckIsReadMessage(int Id)
        {

            if (ModelState.IsValid)
            {

                _conf = await _messageService.CheckIsReadMessage(Id);
            }
            if (_conf.Valid)
            {

                return Json(new { message = _conf.Message, valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "حدث خطا في حفظ البيانات", valid = _conf.Valid }, JsonRequestBehavior.AllowGet);
        }

        //Message/ViewMessage
        public async Task<ActionResult> ViewMessage()
        {
            int userId = ((UserDto)Session["User"]).PK_Users_Id;
            UserDto Reciever = (await _userService.FindUserByID(userId));

            var MessageVM = new MessageViewModel()
            {
                FK_Messages_Users_RecieverId = userId,
                UserName = Reciever.FirstName + " " + Reciever.LastName,
            };
            return View(MessageVM);
        }
        //Message/DetailMessage
        public async Task<ActionResult> DetailMessage(string id)
        {
            if (HttpContext.Items["Disable"] != null && bool.Parse(HttpContext.Items["Disable"].ToString()))
            {
                ViewBag.Disable = true;
            }
            MessageViewModel Message = Mapper.Map<MessagesDto, MessageViewModel>(await _messageService.EditMessage(int.Parse(id)));
            int userId = ((UserDto)Session["User"]).PK_Users_Id;

            UserDto Reciever = (await _userService.FindUserByID(Message.FK_Messages_Users_RecieverId));
            ViewBag.UserName = Reciever.FirstName + " " + Reciever.LastName;

            return View(Message);
        }
        //Message/EmpAutoComplete
        public async Task<ActionResult> EmpAutoComplete(string text)
        {
            var Users = Mapper.Map<List<UserDto>, List<UserViewModel>>(await _userService.UserAutoComplete(text));
            return Json(Users, JsonRequestBehavior.AllowGet);
        }
    }
}