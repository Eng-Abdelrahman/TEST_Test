using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Hubs;
using _3aqarak.MVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
//using System.Runtime.Caching;

namespace _3aqarak.MVC.Filters
{
    public class GetNotifications : ActionFilterAttribute
    {
        private INotificationService _noteService;
        private INotificationCacheClasses _noteCacheService;
        string DB = ConfigurationManager.ConnectionStrings["RealEstateDB"].ToString().Split(';')[1].Split('=')[1];
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _noteService = DependencyResolver.Current.GetService<INotificationService>();
            _noteCacheService = DependencyResolver.Current.GetService<INotificationCacheClasses>();

            if (HttpContext.Current.Cache["endedRentContracts"] == null)
            {
                SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_RentAgreementHeaders");
                CacheItemRemovedCallback onRentContractscacheRemoved = new CacheItemRemovedCallback(_noteCacheService.GetRentcontractsNotifications);
                HttpContext.Current.Cache.Insert("endedRentContracts",  _noteService.GetFinishedRentalsNotificationsWithoutAsync().EndedContracts, sqlDepend, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, onRentContractscacheRemoved);
            }
            if (HttpContext.Current.Cache["expectedNotes"] == null)
            {
                SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_ExpectedContracts");
                CacheItemRemovedCallback onExpectedcacheRemoved = new CacheItemRemovedCallback(_noteCacheService.GetExpectedNotifications);
                HttpContext.Current.Cache.Insert("expectedNotes",  _noteService.GetExpectedNotificationsWithoutAsync().ExpectedContracts, sqlDepend, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, onExpectedcacheRemoved);
            }
            if (HttpContext.Current.Cache["callNotes"] == null)
            {
                SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_PostbonedCalls");
                CacheItemRemovedCallback onCallscacheRemoved = new CacheItemRemovedCallback(_noteCacheService.GetCallsNotifications);
                HttpContext.Current.Cache.Insert("callNotes",  _noteService.GetCallsNotificationsWithoutAsync().Calls, sqlDepend, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCallscacheRemoved);
            }

            if (HttpContext.Current.Cache["FellowcallNotes"] == null)
            {
                SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_FellowupCall");
                CacheItemRemovedCallback onFellowCallscacheRemoved = new CacheItemRemovedCallback(_noteCacheService.GetFellowCallsNotifications);
                HttpContext.Current.Cache.Insert("FellowcallNotes", _noteService.GetFellowupCallsNotificationsWithoutAsync().FellowupCalls, sqlDepend, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration, CacheItemPriority.Default, onFellowCallscacheRemoved);
            }
            if (HttpContext.Current.Cache["previewNotes"] == null)
            {
                SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_PreviewHeaders");
                CacheItemRemovedCallback onPreviewCacheRemoved = new CacheItemRemovedCallback(_noteCacheService.GetPreviewNotifications);
                HttpContext.Current.Cache.Insert("previewNotes", ( _noteService.GetPreviewNotificationsWithoutAsync()).Previews, sqlDepend, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration, CacheItemPriority.Default, onPreviewCacheRemoved);
            }
            if (HttpContext.Current.Cache["rentalsToCollect"] == null)
            {
                SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_RentAgreementHeaders");
                CacheItemRemovedCallback onRentalsToCollectRemoved = new CacheItemRemovedCallback(_noteCacheService.GetRentalsToCollectNotifications);
                HttpContext.Current.Cache.Insert("rentalsToCollect", ( _noteService.GetFinishedRentalsToCollectNotificationsWithoutAsync()).RentalsToCollect, sqlDepend, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, onRentalsToCollectRemoved);
            }
            if (HttpContext.Current.Cache["salesToCollect"] == null)
            {
                SqlCacheDependency sqlDepend = new SqlCacheDependency(DB, "tbl_SaleAgreementHeaders");
                CacheItemRemovedCallback onSaleToCollectRemoved = new CacheItemRemovedCallback(_noteCacheService.GetSaleToCollectNotifications);
                HttpContext.Current.Cache.Insert("salesToCollect", ( _noteService.GetFinishedSaleToCollectNotificationsWithoutAsync()).SaleToCollect, sqlDepend, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, onSaleToCollectRemoved);
            }
        }


    }
}