using _3aqarak.MVC.Hubs;
using _3aqarak.MVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace _3aqarak.MVC.NotificationCacheClasses
{
    public class NotificationCacheClass : INotificationCacheClasses
    {
        public void GetRentcontractsNotifications(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.DependencyChanged || reason == CacheItemRemovedReason.Expired)
            {
                NotificationHub.ShowRentalNotifications();
            }

        }
        public void GetExpectedNotifications(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.DependencyChanged || reason == CacheItemRemovedReason.Expired)
            {
                NotificationHub.ShowExpectedNotifications();
            }

        }
        public void GetCallsNotifications(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.DependencyChanged || reason == CacheItemRemovedReason.Expired)
            {
                NotificationHub.ShowCallsNotifications();
            }
        }
        public void GetPreviewNotifications(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.DependencyChanged || reason == CacheItemRemovedReason.Expired)
            {
                NotificationHub.ShowPreviewNotifications();
            }
        }

        public void GetRentalsToCollectNotifications(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.DependencyChanged || reason == CacheItemRemovedReason.Expired)
            {
                NotificationHub.showRentalsToCollectNotifications();
            }
        }

        public void GetSaleToCollectNotifications(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.DependencyChanged || reason == CacheItemRemovedReason.Expired)
            {
                NotificationHub.showSaleToCollectNotifications();
            }
        }

        public void GetFellowCallsNotifications(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.DependencyChanged||reason==CacheItemRemovedReason.Expired)
            {
                NotificationHub.ShowFellowupCallsNotifications();
            }
        }
    }
   
}