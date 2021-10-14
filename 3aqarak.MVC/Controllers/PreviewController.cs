using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Controllers
{
    public class PreviewController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IDemandService _demandService;
        private readonly IAvailableService _availableService;
        private readonly IPreviewService _previewService;

        private IConfirmation _conf;

        public PreviewController(IClientService clientService, IPreviewService previewService, IDemandService demandService, IConfirmation conf, IAvailableService availableService)
        {
            _clientService = clientService;
            _demandService = demandService;
            _conf = conf;
            _availableService = availableService;
            _previewService = previewService;
        }
        public async Task<ActionResult> PreviewsList(PreviewFilter filter = null)
        {
            var previews = Mapper.Map<List<PreviewScreenDto>, List<PreviewScreenViewModel>>(await _previewService.GetPreviews(filter));
            return View(previews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPreview(int buyerId, int[] sellerId, int demandId, int[] availableIds, List<DateTime> dates, int demanCat, int[] availableCatIds)
        {
            var datesWithoutSeconds = new List<DateTime>();
            var repeatedList = new List<DateTime>();
            if (dates.Count > 1)
            {
                for (int i = 0; i < dates.Count; i++)
                {
                    datesWithoutSeconds.Add(new DateTime(dates[i].Year, dates[i].Month, dates[i].Day, dates[i].Hour, dates[i].Minute, 00));
                }
                for (int i = 0; i < datesWithoutSeconds.Count; i++)
                {
                    repeatedList = datesWithoutSeconds.FindAll(e => e == datesWithoutSeconds[i]);
                    if (repeatedList != null && repeatedList.Count > 1)
                    {
                        return Json(new { Valid = false, Message = "لقد ادخلت مواعيد معاينات متطابقة لنفس الشقه في نفس التوقيت" }, JsonRequestBehavior.AllowGet);

                    }
                    repeatedList.Clear();
                }
            }
            if (dates.Any(d => d < TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time")))
            {
                return Json(new { Valid = false, Message = "لقد اخترت مواعيد للمعاينات قبل تاريخ اليوم" }, JsonRequestBehavior.AllowGet);
            }
            if (await _previewService.IsPreviewExistsInSameTime(availableIds, dates, availableCatIds))
            {
                return Json(new { Valid = false, Message = "هذا الميعاد محجوز مسبقا !.. اختر وقت اخر" }, JsonRequestBehavior.AllowGet);
            }
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            _conf = await _previewService.CreatePreview(buyerId, sellerId, demandId, availableIds, userId, dates, demanCat, availableCatIds);
            return Json(_conf, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> PreviewDetails(string id)
        {
            var details = Mapper.Map<PreviewScreenDto, PreviewScreenViewModel>(await _previewService.GetPreviewDetails(int.Parse(id)));
            return View(details);
        }

        public async Task<ActionResult> SetPreviewResult(string id)
        {
            var details = Mapper.Map<PreviewScreenDto, PreviewScreenViewModel>(await _previewService.SetPreviewResults(int.Parse(id)));
            return View(details);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostponePreview(int preview, int detail, DateTime date)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            if (date < DateTime.Now)
            {
                return Json(new { Valid = false, message = "لقد اخترت مواعيد للمعاينات قبل تاريخ اليوم" }, JsonRequestBehavior.AllowGet);
            }
            var IsNewPreview = false;
            if (await _previewService.IsPostponePreviewExistsInSameTime(detail, date))
            {
                return Json(new { Valid = false, message = "هذا الميعاد محجوز مسبقا !.. اختر وقت اخر" }, JsonRequestBehavior.AllowGet);
            }           
                IsNewPreview = await _previewService.PostponePreview(preview, detail, date, userId);
                if (IsNewPreview)
                {
                    var ISpostponed = await _previewService.SetPreviewDetSatus(detail, PreviewStatus.IsPostponed);

                    return Json(new { Valid = IsNewPreview, message = "تم تاجيل المعاينه الى التاريخ الجديد!!" }, JsonRequestBehavior.AllowGet);

                }
            
            return Json(new { Valid = IsNewPreview, message = "لم يتم تاجيل المعاينه الى التاريخ الجديد!!" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelPreview(int detail)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var IsCancelled = await _previewService.SetPreviewDetSatus(detail, PreviewStatus.IsCancelled);
            return Json(IsCancelled, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SuspendPreview(int detail)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var IsSuspended = await _previewService.SetPreviewDetSatus(detail, PreviewStatus.IsSuspended);
            return Json(IsSuspended, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePreview(int id)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var IsDeleted = await _previewService.DeletePreview(id, userId);
            return Json(IsDeleted, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RejectPreview(int detail)
        {
            var userId = ((UserDto)Session["User"]).PK_Users_Id;
            var IsRejected = await _previewService.SetPreviewDetSatus(detail, PreviewStatus.IsRejected);
            return Json(IsRejected, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public async Task<JsonResult> getDatesRelated(DateTime date, int id ,int catId)
        {
            List<string> datesList = await _previewService.GetReservationDates(date, id,catId);
            return Json(datesList, JsonRequestBehavior.AllowGet);
        }
    }
}
