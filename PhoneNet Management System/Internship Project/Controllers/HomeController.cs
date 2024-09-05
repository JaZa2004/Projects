using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Internship_Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Devices()
        {
            return View();
        }

        public ActionResult Client()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult PhoneNumbers()
        {
            return View();
        }
        public ActionResult Reservations()
        {
            return View();
        }

        public ActionResult ClientReport()
        {
            return View();
        }
        public ActionResult DeviceReport()
        {
            return View();
        }
    }
}