using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Hubs;
using _3aqarak.MVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace _3aqarak.MVC.Controllers
{
    public class NotificationsController : Controller
    {
        // GET: Notifications
        private readonly string DB;
        private readonly INotificationService _noteService;
        private INotificationCacheClasses _noteCacheService;
        public NotificationsController(INotificationService noteService, INotificationCacheClasses noteCacheService)
        {
            DB = ConfigurationManager.ConnectionStrings["RealEstateDB"].ToString().Split(';')[1].Split('=')[1];
            _noteService = noteService;
            _noteCacheService = noteCacheService;
        }
        [HttpGet]
        public async Task<ActionResult> ExpectedNotifications()
        {
            SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_ExpectedContracts");
            CacheItemRemovedCallback onExpectedcacheRemoved = new CacheItemRemovedCallback(_noteCacheService.GetExpectedNotifications);
            var expected = (await _noteService.GetExpectedNotifications()).ExpectedContracts;
            HttpContext.Cache.Insert("expectedNotes", expected, sqlDepend, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, onExpectedcacheRemoved);
            return Json(new { expect = expected }, JsonRequestBehavior.AllowGet);
        }

        public async Task< ActionResult> CallsNotifications()
        {
            SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_PostbonedCalls");
            CacheItemRemovedCallback onCallscacheRemoved = new CacheItemRemovedCallback(_noteCacheService.GetCallsNotifications);
            var Calls = (await _noteService.GetCallsNotifications()).Calls;
            HttpContext.Cache.Insert("callNotes", Calls, sqlDepend, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCallscacheRemoved);
            return Json(new { calls = Calls }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> FellowCallsNotifications()
        {
            SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_FellowupCall");
            CacheItemRemovedCallback onFellowCallscacheRemoved = new CacheItemRemovedCallback(_noteCacheService.GetFellowCallsNotifications);
            var Calls = (await _noteService.GetFellowupCallsNotifications()).FellowupCalls;
            HttpContext.Cache.Insert("FellowcallNotes", Calls, sqlDepend, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration, CacheItemPriority.Default, onFellowCallscacheRemoved);
            return Json(new { calls = Calls }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> PreviewsNotification()
        {
            SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_PreviewHeaders");
            CacheItemRemovedCallback onPreviewcacheRemoved = new CacheItemRemovedCallback(_noteCacheService.GetPreviewNotifications);
            var Previews = (await _noteService.GetPreviewNotifications()).Previews;
            HttpContext.Cache.Insert("previewNotes", Previews, sqlDepend, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration, CacheItemPriority.Default, onPreviewcacheRemoved);
            return Json(new { previews = Previews }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> RentalNotifications()
        {
            SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_RentAgreementHeaders");
            CacheItemRemovedCallback onRentContractscacheRemoved = new CacheItemRemovedCallback(_noteCacheService.GetRentcontractsNotifications);
            var Rentals = (await _noteService.GetFinishedRentalsNotifications()).EndedContracts;
            HttpContext.Cache.Insert("endedRentContracts", Rentals, sqlDepend, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, onRentContractscacheRemoved);
            return Json(new { rentals = Rentals }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> RentalsToCollectNotifications()
        {
            SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_RentAgreementHeaders");
            CacheItemRemovedCallback onRentalsToCollectRemoved = new CacheItemRemovedCallback(_noteCacheService.GetRentalsToCollectNotifications);
            var RentalsToCollect = (await _noteService.GetFinishedRentalsToCollectNotifications()).RentalsToCollect;
            HttpContext.Cache.Insert("rentalsToCollect", RentalsToCollect, sqlDepend, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, onRentalsToCollectRemoved);
            return Json(new { rentalsToCollect = RentalsToCollect }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public async Task<ActionResult> SaleToCollectNotifications()
        {
            SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_SaleAgreementHeaders");
            CacheItemRemovedCallback onSaleToCollectRemoved = new CacheItemRemovedCallback(_noteCacheService.GetSaleToCollectNotifications);
            var SaleToCollect = (await _noteService.GetFinishedSaleToCollectNotifications()).SaleToCollect;
            HttpContext.Cache.Insert("salesToCollect", SaleToCollect, sqlDepend, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, onSaleToCollectRemoved);
            return Json(new { salesToCollect = SaleToCollect }, JsonRequestBehavior.AllowGet);
        }
    }
}