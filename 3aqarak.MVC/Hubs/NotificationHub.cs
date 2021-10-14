using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR.Infrastructure;
using Autofac;
using _3aqarak.MVC.ViewModels;
using _3aqarak.MVC.Helpers;

namespace _3aqarak.MVC.Hubs
{

    public class NotificationHub : Hub
    {
        private readonly ILifetimeScope _hubLifetimeScope;


        public NotificationHub(ILifetimeScope lifetimeScope)
        {
           
            // Create a lifetime scope for the hub.
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

        }

        public static void ShowCallsNotifications()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            
            //var confMgr = DependencyResolver.Current.GetService<IConnectionManager>();
            //var context = confMgr.GetHubContext<NotificationHub>();
            context.Clients.All.pushCallsNotifications();
        }

        public static void ShowFellowupCallsNotifications()
            {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            //var confMgr = DependencyResolver.Current.GetService<IConnectionManager>();
            //var context = confMgr.GetHubContext<NotificationHub>();
            context.Clients.All.pushFellowupCallsNotifications();
        }
        public static void ShowRentalNotifications()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            //var confMgr = DependencyResolver.Current.GetService<IConnectionManager>();
            //var context = confMgr.GetHubContext<NotificationHub>();
            context.Clients.All.pushRentalNotifications();
        }
        public static void ShowExpectedNotifications()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.pushExpectNotifications();
        }
        public static void ShowPreviewNotifications()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.pushPreviewNotifications();
        }
        public static void showRentalsToCollectNotifications()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.pushRentalsToCollectNotifications();
        }
        public static void showSaleToCollectNotifications()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.pushSaleToCollectNotifications();
        }
        public static void showDemandmatchedNotifications(List<MatchedDemandsHelper> matchedDemands , int AvailableId)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            var jsonDemands = Newtonsoft.Json.JsonConvert.SerializeObject(matchedDemands);          
            context.Clients.All.pushDemandMatchedNotifications(matchedDemands, Categories.Apartements , AvailableId);
        }

        public static void showVillaDemandmatchedNotifications(List<MatchedDemandsHelper> matchedDemands, int availableId)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            var jsonDemands = Newtonsoft.Json.JsonConvert.SerializeObject(matchedDemands);
            context.Clients.All.pushDemandMatchedNotifications(jsonDemands, Categories.Villas,availableId);
        }

        public static void showLandDemandmatchedNotifications(List<MatchedDemandsHelper> matchedDemands, int availableId)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            var jsonDemands = Newtonsoft.Json.JsonConvert.SerializeObject(matchedDemands);
            context.Clients.All.pushDemandMatchedNotifications(jsonDemands, Categories.Lands,availableId);
        }

        public static void showShopDemandmatchedNotifications(List<MatchedDemandsHelper> matchedDemands, int availableId)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            var jsonDemands = Newtonsoft.Json.JsonConvert.SerializeObject(matchedDemands);
            context.Clients.All.pushDemandMatchedNotifications(jsonDemands, Categories.Shops,availableId);
        }
        protected override void Dispose(bool disposing)
        {
            // Dipose the hub lifetime scope when the hub is disposed.
            if (disposing && _hubLifetimeScope != null)
            {
                _hubLifetimeScope.Dispose();
            }

            base.Dispose(disposing);
        }

       
    }
}