using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyHospital.Web.Controllers
{

    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("UserProfile","Account");
        }
        
        public ActionResult StartPage()
        {
            return View();
        }
    }
}