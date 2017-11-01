using MyHospital.BLL.Interfaces;
using MyHospital.BLL.Services;
using Ninject.Modules;

namespace MyHospital.Web.Utils
{
    public class MyHospitalModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDeseaseService>().To<DeseaseService>();
            Bind<IUserService>().To<UserService>();
        }
    }
}