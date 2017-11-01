using MyHospital.DAL.Interfaces;
using MyHospital.DAL.Repositories;
using Ninject.Modules;

namespace MyHospital.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<MyHospitalUnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
