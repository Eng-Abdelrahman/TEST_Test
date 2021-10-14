using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace _3aqarak.MVC.Interfaces
{
    public interface INotificationCacheClasses
    {
        void GetCallsNotifications(string key, object value, CacheItemRemovedReason reason);
        void GetExpectedNotifications(string key, object value, CacheItemRemovedReason reason);
        void GetPreviewNotifications(string key, object value, CacheItemRemovedReason reason);
        void GetRentcontractsNotifications(string key, object value, CacheItemRemovedReason reason);
        void GetRentalsToCollectNotifications(string key, object value, CacheItemRemovedReason reason);
        void GetSaleToCollectNotifications(string key, object value, CacheItemRemovedReason reason);
        void GetFellowCallsNotifications(string key, object value, CacheItemRemovedReason reason);
    }
}