using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using MyHospital.BLL.Infrastructure;
using MyHospital.BLL.Interfaces;
using MyHospital.BLL.Services;
using MyHospital.Web.Utils;
using Ninject;
using Ninject.Modules;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(MyHospital.Web.App_Start.StartUp))]

namespace MyHospital.Web.App_Start
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/StartPage"),
            });
        }

        private IUserService CreateUserService()
        {
            NinjectModule myHospitalModule = new MyHospitalModule();
            NinjectModule serviceModule = new ServiceModule("Hospital");
            IKernel ninjectKernel = new StandardKernel(myHospitalModule, serviceModule);
            var userServise = ninjectKernel.Get<IUserService>();
            return userServise;
        }
    }
}