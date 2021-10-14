using _3aqarak.BLL.Interfaces;
using _3aqarak.DAL.Repositories;
using Autofac;

namespace _3aqarak.DAL.DI
{
    public class AutoFacDALContainer:Module
    {
        private readonly string _connectionString;


        public AutoFacDALContainer(string connectionString)
        {
            this._connectionString = connectionString;

        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>()
                .WithParameter(new TypedParameter(typeof(string), this._connectionString));           

        }
    }
}
