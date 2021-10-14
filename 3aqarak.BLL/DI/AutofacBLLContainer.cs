
using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using Autofac;

namespace _3aqarak.BLL.DI

{
    public class AutofacBLLContainer : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Confirmation>().As<IConfirmation>();
            builder.RegisterType<NotificationDto>().As<INotificationDto>();

        }
    }
}
