using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Controllers
{
    public class ClientCallsController : Controller
    {
        private readonly IClientService _clientService;

        private readonly IUSerService _userService;
        private readonly ICallService _callService;

        public ClientCallsController(IClientService clientService, IUSerService userService, ICallService callService)
        {
            _clientService = clientService;
            _userService = userService;
            _callService = callService;
        }

        public async Task<ActionResult> GeClientContact(int clientId, int offerId, string OfferType, string catId)
        {
            var Client = _clientService.FindClientByID(clientId);
            var callVM = new ClientCallViewModel();
            callVM.ClientName =(await Client).Name;
            callVM.PhoneNumber = (await Client).Mobile;
            callVM.Clients_Id = clientId;
            if (OfferType == "demand")
            {
                callVM.DemandCode = offerId;

            }
            else
            {
                callVM.AvailableCode = offerId;
            }
            callVM.CategoryId = int.Parse(catId);

            return PartialView("_ClientCallView", callVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveClientCall(ClientCallViewModel callVM)
        {
            var valid = false;
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                var clientDto = Mapper.Map<ClientCallViewModel, ClientCallDto>(callVM);
                clientDto.DateTime = DateTime.Now;
                clientDto.FK_ClientCalls_Clients_Id = callVM.Clients_Id;
                valid = await _callService.SaveClientCall(clientDto, userId);
            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterPostponedCall(PostbonedCallViewModel callVM)
        {
            var valid = false;
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                var clientDto = Mapper.Map<PostbonedCallViewModel, ClientCallDto>(callVM);
                clientDto.DateTime = DateTime.UtcNow.AddMinutes(120);
                clientDto.FK_ClientCalls_Clients_Id = callVM.Clients_Id;
                valid = await _callService.SaveClientCall(clientDto, userId);
                if (valid)
                {
                    var closed = await _callService.ClosePostponedCall(callVM.PK_PostbonedCalls, userId);
                }
            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PostponedCallsIndex(string calls)
        {

            var callList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PostbonedCallViewModel>>(calls);
            return View(callList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> savepostponed(ClientCallViewModel callVM)
        {
            var validCall = false;
            if (callVM.DateTime.Value < DateTime.UtcNow.AddMinutes(120))
            {
                return Json(new { valid = validCall, message = " لقد ادخلت تاريخااو توقيتا قبل تاريخ اليوم او التوقيت الحالي" }, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                var postCallDto = Mapper.Map<ClientCallViewModel, PostponedCallDto>(callVM);
                postCallDto.FK_PostponedCalls_Clients_ClientId = callVM.Clients_Id;
                postCallDto.Nots = callVM.Notes;
                validCall = await _callService.SavePostponedCall(postCallDto, userId);
                if (validCall)
                {
                    return Json(new { valid = validCall, message = " تم الحفظ بنجاح" }, JsonRequestBehavior.AllowGet);

                }
            }
            return Json(new { valid = validCall, message = " لم يتم الحفظ بنجاح" }, JsonRequestBehavior.AllowGet);
        }
    }
}