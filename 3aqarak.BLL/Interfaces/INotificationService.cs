using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface INotificationService
    {
        Task<INotificationDto> GetPreviewNotifications();
        INotificationDto GetPreviewNotificationsWithoutAsync();
        Task<INotificationDto> GetCallsNotifications();
        Task<INotificationDto> GetExpectedNotifications();
        Task<INotificationDto> GetFinishedRentalsNotifications();
        INotificationDto GetFinishedRentalsNotificationsWithoutAsync();
        INotificationDto GetExpectedNotificationsWithoutAsync();
        INotificationDto GetCallsNotificationsWithoutAsync();
        Task<INotificationDto> GetFinishedRentalsToCollectNotifications();
        INotificationDto GetFinishedRentalsToCollectNotificationsWithoutAsync();
        Task<INotificationDto> GetFinishedSaleToCollectNotifications();
        INotificationDto GetFinishedSaleToCollectNotificationsWithoutAsync();
        Task<INotificationDto> GetFellowupCallsNotifications();
       INotificationDto GetFellowupCallsNotificationsWithoutAsync();

    }
}
